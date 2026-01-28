# Compiler Configuration

The H5 compiler is configured primarily through a `h5.json` file located in your project root.

## Basic `h5.json` Structure

```json
{
  "output": "dist",
  "module": {
    "type": "ES6"
  },
  "sourceMap": true
}
```

## Common Options

- **`output`**: The directory where generated JavaScript files will be placed.
- **`fileName`**: The name of the generated JavaScript file (defaults to the assembly name).
- **`module`**: Configuration for JavaScript modules.
  - `type`: `AMD`, `CommonJS`, `UMD`, `ES6`, or `None`.
- **`sourceMap`**: Boolean indicating whether to generate source maps for debugging C# in the browser.
- **`define`**: A list of preprocessor symbols to define during compilation.

## Advanced Configuration

### Resource Management

H5 can manage and bundle external assets (JS, CSS, fonts) using the `resources` section. These resources are automatically injected into the generated `index.html`.

```json
{
  "resources": [
    {
      "name": "app-dependencies.js",
      "files": [
        "assets/js/jquery.min.js",
        "assets/js/bootstrap.bundle.min.js"
      ],
      "output": "js"
    },
    {
      "name": "styles.css",
      "files": [ "assets/css/*.css" ],
      "output": "css"
    }
  ]
}
```

- **`name`**: The name of the bundled file.
- **`files`**: A list of files or wildcards to include.
- **`output`**: The sub-directory in the output folder where the bundle will be placed.

### HTML Injection

The H5 compiler can generate a basic `index.html` file and inject references to the generated script and the defined resources. This is enabled by default unless `html` configuration is explicitly customized or disabled.

### Configuration Merging

You can create environment-specific configuration files:
- `h5.Debug.json`
- `h5.Release.json`

When building in a specific configuration (e.g., `Debug`), the compiler will first load `h5.json` and then merge it with `h5.Debug.json`. Values in the configuration-specific file will overwrite those in the base `h5.json`.

## MSBuild Properties

You can also control some compiler behaviors via MSBuild properties in your `.csproj`:

- `<UpdateH5>false</UpdateH5>`: Disables automatic updates of the H5 compiler tool.
- `<H5NoCore>true</H5NoCore>`: Prevents the compiler from automatically including the core library.
