// End-to-end validation of h5's ESM output mode.
//
// This script:
//   1. Builds Tests/Placeholder (which uses h5.json `outputModuleType: "ESM"`).
//   2. Serves the generated .mjs / .js files plus the local test index.html via
//      a tiny http server.
//   3. Drives Chromium through Playwright, lets the app execute, then verifies
//      the globals the app wrote to `window` and the resulting DOM.
//
// Usage:
//   node test-esm.mjs

import { chromium } from "playwright";
import { spawnSync } from "node:child_process";
import http from "node:http";
import fs from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const placeholderProject = path.resolve(__dirname, "..", "Placeholder", "Placeholder.csproj");
const buildOutput = path.resolve(__dirname, "..", "Placeholder", "bin", "Debug", "netstandard2.0", "h5");

if (process.env.SKIP_BUILD !== "1") {
    console.log("Building", placeholderProject);
    const build = spawnSync("dotnet", ["build", placeholderProject, "--nologo", "-v:minimal"], { stdio: "inherit" });
    if (build.status !== 0) {
        console.error("dotnet build failed");
        process.exit(build.status || 1);
    }
}

try {
    await fs.access(buildOutput);
} catch {
    console.error("Build output directory missing:", buildOutput);
    process.exit(1);
}

const mime = {
    ".html": "text/html; charset=utf-8",
    ".mjs":  "text/javascript; charset=utf-8",
    ".js":   "text/javascript; charset=utf-8",
    ".css":  "text/css; charset=utf-8",
    ".json": "application/json; charset=utf-8",
};

const server = http.createServer(async (req, res) => {
    const reqPath = decodeURIComponent(new URL(req.url, "http://localhost").pathname);

    if (reqPath === "/favicon.ico") {
        res.writeHead(204);
        res.end();
        return;
    }

    // /index.html is served from this directory; everything else is served from
    // the h5 build output directory.
    const localFile = reqPath === "/" || reqPath === "/index.html" ? "/index.html" : null;
    const filePath = localFile
        ? path.join(__dirname, localFile)
        : path.join(buildOutput, reqPath);

    try {
        const data = await fs.readFile(filePath);
        const ext = path.extname(filePath).toLowerCase();
        res.writeHead(200, { "content-type": mime[ext] || "application/octet-stream" });
        res.end(data);
    } catch {
        res.writeHead(404, { "content-type": "text/plain" });
        res.end("Not found: " + req.url);
    }
});

await new Promise((resolve) => server.listen(0, "127.0.0.1", resolve));
const { port } = server.address();
const url = `http://127.0.0.1:${port}/index.html`;
console.log("Serving from", buildOutput);
console.log("URL:        ", url);

const browser = await chromium.launch({
    executablePath: process.env.CHROMIUM_BIN || "/opt/pw-browsers/chromium-1194/chrome-linux/chrome",
    args: ["--no-sandbox"],
});
const context = await browser.newContext();
const page = await context.newPage();

const consoleMessages = [];
page.on("console",   (msg)  => consoleMessages.push(`[${msg.type()}] ${msg.text()}`));
page.on("pageerror", (err) => consoleMessages.push(`[error] ${err.message}`));

const networkErrors = [];
page.on("response", (response) => {
    if (response.status() >= 400) {
        networkErrors.push(`${response.status()} ${response.url()}`);
    }
});

await page.goto(url, { waitUntil: "networkidle" });

const result = await page.evaluate(() => ({
    greeting: window.__h5_test_greeting,
    sum:      window.__h5_test_sum,
    message:  window.__h5_test_message,
    bodyHtml: document.body.innerHTML,
    helloEl:  document.getElementById("hello") ? document.getElementById("hello").textContent : null,
}));

// Confirm the generated app.mjs really uses ES module syntax instead of legacy
// h5.define globals.
const appMjs = await fs.readFile(path.join(buildOutput, "app.mjs"), "utf8");
const placeholderLibMjs = await fs.readFile(path.join(buildOutput, "PlaceholderLib.mjs"), "utf8");
const h5Mjs = await fs.readFile(path.join(buildOutput, "h5.mjs"), "utf8");

console.log("\n=== Page evaluation ===");
console.log(JSON.stringify(result, null, 2));

console.log("\n=== Console messages ===");
for (const m of consoleMessages) console.log("  " + m);

if (networkErrors.length > 0) {
    console.log("\n=== Network errors ===");
    for (const e of networkErrors) console.log("  " + e);
}

await browser.close();
server.close();

let failed = false;
function expect(label, condition) {
    if (!condition) {
        console.error("FAIL:", label);
        failed = true;
    } else {
        console.log("OK  :", label);
    }
}

console.log("\n=== Assertions: runtime behaviour ===");
expect("greeting equals 'Hello, h5!'",  result.greeting === "Hello, h5!");
expect("sum equals '42'",               result.sum === "42");
expect("message contains '(42)'",       (result.message || "").includes("(42)"));
expect("hello element rendered",        (result.helloEl || "").includes("Hello, h5!"));
expect("no console errors",             !consoleMessages.some(m => m.startsWith("[error]")));
expect("no network errors",             networkErrors.length === 0);

console.log("\n=== Assertions: generated ESM shape ===");
expect("app.mjs imports H5 from ./h5.mjs", appMjs.includes(`import { H5 } from "./h5.mjs"`));
expect("app.mjs exports H5",               appMjs.includes(`export { H5 }`));
expect("PlaceholderLib.mjs imports H5",    placeholderLibMjs.includes(`import { H5 } from "./h5.mjs"`));
expect("PlaceholderLib.mjs exports H5",    placeholderLibMjs.includes(`export { H5 }`));
expect("h5.mjs re-exports global H5",      h5Mjs.includes(`globalThis.H5`) && h5Mjs.includes(`export { H5 }`));

if (failed) {
    console.error("\nTest FAILED");
    process.exit(1);
}
console.log("\nTest PASSED");
