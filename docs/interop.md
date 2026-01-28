# JavaScript Interop

H5 provides several ways to interact with native JavaScript code.

## Calling JavaScript from C#

### Using `[External]` and `[Name]`
The most common way is to define C# "stubs" that match the JavaScript API.
```csharp
[External]
[Name("window")]
public static class Window {
    [Name("alert")]
    public static extern void Alert(string message);
}

// Usage
Window.Alert("Hello from C#!");
```

### Using `Script.Write`
For quick snippets of JavaScript, you can use `Script.Write`.
```csharp
Script.Write("console.log('Direct JS');");
```

### Using dynamic
H5 supports the `dynamic` keyword, which allows you to call JavaScript members without pre-defined stubs.
```csharp
dynamic obj = Script.Write("{}");
obj.someProperty = 123;
```

## Calling C# from JavaScript

H5 generates JavaScript objects that represent your C# classes. By default, they are placed in a namespace matching your C# namespace.

```csharp
namespace MyApp {
    public class Utils {
        public static void SayHello() {
            Console.WriteLine("Hello!");
        }
    }
}
```

In JavaScript:
```javascript
MyApp.Utils.sayHello();
```

## Data Mapping

- **Primitives**: `int`, `double`, `bool`, `string` map directly to their JavaScript equivalents.
- **Objects**: C# objects are represented as JavaScript objects with H5-specific metadata.
- **Object Literals**: Classes marked with `[ObjectLiteral]` are treated as plain `{}` objects.
- **Tasks**: `System.Threading.Tasks.Task` maps to JavaScript `Promise`.
