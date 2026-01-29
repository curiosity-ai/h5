# H5 Compiler (CLI)

The H5 Compiler (`h5.exe`) is a command-line tool that translates C# code into JavaScript. It allows for compiling entire projects, leveraging the `.csproj` file format.

It is available as a .NET Tool and can be installed via NuGet:

[h5-compiler on NuGet](https://www.nuget.org/packages/h5-compiler)

## Modes of Operation

The compiler can operate in several modes:

1.  **Client (Default)**: Parses arguments, finds a compilation server (or starts one if not found), and sends a compilation request. If `--no-server` is specified, it compiles the project directly in the current process.
2.  **Server (`server`)**: Starts a gRPC server that listens for compilation requests on port `44568`. This mode is useful for keeping the compiler warm and avoiding startup costs (like loading MSBuild libraries) for each compilation.
3.  **Start Server (`startserver`)**: Starts the server in a separate process.
4.  **Check Online (`check-if-online`)**: Checks if the compilation server is currently running and reachable.

## Usage

```bash
h5 [options] (<project-file>|<assembly-file>)
```

### Options

| Option | Description |
| :--- | :--- |
| `-p`, `--project <path>` | Specifies the path to the `.csproj` file to compile. If not provided, the compiler attempts to find a single `.csproj` file in the current directory. |
| `--h5 <path>` | Specifies the H5 location. |
| `-o`, `--output <path>` | Specifies the output directory for the generated JavaScript files. |
| `-c`, `--configuration <name>` | Sets the build configuration (e.g., `Debug`, `Release`). Defaults to `none`. |
| `--assembly-version <version>` | Overrides the assembly version. |
| `-D`, `--define <const-list>` | Semicolon-delimited list of project constants to define. |
| `-r`, `--rebuild` | Forces a rebuild of the assembly. |
| `--no-server` | Bypasses the compilation server and runs the compilation in the current process. This is the default behavior on non-Windows platforms (Linux, macOS) or CI environments. |
| `-S`, `--settings <name:value>` | Comma-delimited list of project settings (e.g., `AssemblyName:MyApp,RootNamespace:MyNs`). |
| `--trace` | Sets the log level to Trace for verbose output. |
| `-h`, `--help` | Shows the help message. |

### Settings (-S)

The `-S` option allows you to override specific project properties. Allowed settings include:
- `AssemblyName`
- `CheckForOverflowUnderflow`
- `Configuration`
- `DefineConstants`
- `OutputPath`
- `OutDir`
- `OutputType`
- `RootNamespace`

## Compilation Process

1.  **Argument Parsing**: The compiler parses the command-line arguments to build a `CompilationRequest`.
2.  **Project Location**: If no project is specified, it searches for a `.csproj` in the current directory.
3.  **Server Connection**:
    -   Unless `--no-server` is used (or detected as needed based on OS/Environment), it tries to connect to a local compilation server.
    -   If the server is not found, it launches a new server instance.
4.  **Compilation**:
    -   The compilation request is sent to the server (or processed locally).
    -   The server (or local processor) uses `MSBuild` to load the project and `H5.Translator` to convert the C# code to JavaScript.
    -   Status updates and logs are streamed back to the client.

## Logging

The compiler uses `ZLogger` for high-performance logging. Use `--trace` to see detailed logs about the compilation process.
