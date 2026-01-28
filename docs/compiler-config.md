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

## MSBuild Properties

You can also control some compiler behaviors via MSBuild properties in your `.csproj`:

- `<UpdateH5>false</UpdateH5>`: Disables automatic updates of the H5 compiler tool.
- `<H5NoCore>true</H5NoCore>`: Prevents the compiler from automatically including the core library.
