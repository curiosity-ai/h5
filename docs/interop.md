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
For quick snippets of JavaScript, you can use `Script.Write`. You can pass parameters from C# and access them using `{0}`, `{1}`, etc.

```csharp
int a = 10;
int b = 20;
// Passing parameters
Script.Write("console.log('Sum: ' + ({0} + {1}))", a, b);

// Getting output
int sum = Script.Write<int>("{0} + {1}", a, b);
```

### Pseudo Casting with `.As<T>()`
Sometimes you need to treat an object as another type without any actual runtime casting or conversion. The `.As<T>()` extension method tells the compiler to treat the expression as type `T` in C#, but it generates no extra JavaScript code.

This is particularly useful when dealing with Union types in `H5.Core`.

```csharp
using H5.Core;
using static H5.Core.dom;

// getBoundingClientRect returns Union<ClientRect, DOMRect>
var rect = element.getBoundingClientRect();

// Use .As<T>() to "cast" to one of the types
var domRect = rect.As<DOMRect>();
Console.WriteLine(domRect.width);
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
  - **Performance**: They are extremely fast because they bypass serialization (like `Newtonsoft.Json`) when interacting with external JS libraries.
  - **Limitations**: They should only contain primitive types, other object literal classes, or simple arrays. They do **not** support full C# features like methods with logic, constructors with logic, or complex C# types (unless those are also object literals).

  ```csharp
  [ObjectLiteral]
  public class Options {
      public string Color;
      public int Intensity;
  }

  // Usage with external JS
  [External]
  public static class MyLib {
      public static extern void Init(Options options);
  }
  ```

- **Tasks and Promises**: While C# `Task` and JavaScript `Promise` are conceptually similar, they are not automatically interchangeable when calling external code.
  - Use `.ToTask()` (available on `es5.Promise<T>`) to convert a JavaScript `Promise` into a C# `Task` that can be awaited.

  ```csharp
  // Assuming an external library returns a Promise
  es5.Promise<string> promise = ExternalLib.DoWork();
  string result = await promise.ToTask();
  ```
