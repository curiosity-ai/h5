![H5.NET logo](https://speed.h5.net/identity/h5dotnet-sh.png)

<p align="center"><img src="https://user-images.githubusercontent.com/62210/29276839-1759fbe8-80cd-11e7-921c-d509e0e2a22c.png"></p>

[![Build status](https://ci.appveyor.com/api/projects/status/nm2f0c0u1jx0sniq/branch/master?svg=true)](https://ci.appveyor.com/project/ObjectDotNet/h5/branch/master)
[![Build Status](https://travis-ci.org/h5dotnet/H5.svg?branch=master)](https://travis-ci.org/h5dotnet/H5)
[![NuGet Status](https://img.shields.io/nuget/v/H5.svg)](https://www.nuget.org/packages/H5)
[![Join the chat at https://gitter.im/h5dotnet/H5](https://badges.gitter.im/h5dotnet/H5.svg)](https://gitter.im/h5dotnet/H5?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[H5.NET](http://h5.net/) is an open source C#-to-JavaScript Compiler. Write your application in C# and run on billions of devices.

### Write in C#. Run in a Web Browser.

<table>
<tr><td align="center" width="50%">C#</td><td></td><td align="center"  width="50%">JavaScript</td></tr>
<tr>
<td>
<pre lang="csharp">
public class Program
{
    public static void Main()
    {
        var msg = "Hello, World!";

            Console.WriteLine(msg);
        }
}
</pre>
</td>
<td><h1>&#8680;</h1></td>
<td>
<pre lang="javascript">
H5.define("Demo.Program", {
    main: function Main () {
        var msg = "Hello, World!";

            System.Console.WriteLine(msg);
        }
});
</pre>
</td>
</tr>
</table>

Run the sample above at [Deck.NET](https://deck.net/helloworld).

## TL;DR

* Read the [Getting Started](https://github.com/curiosity-ai/h5/wiki) Knowledge Base article
* Try [Deck](https://deck.net/) if you want to play
* Installation:
  * Add **H5.NET** Visual Studio extension, or 
  * Use [NuGet](https://www.nuget.org/packages/h5) to install into a C# Class Library project (`Install-Package H5`), or
  * [Download](http://h5.net/download/) the Visual Studio Code starter project
* The [Attribute Reference](https://github.com/curiosity-ai/h5/wiki/attribute-reference) documentation is important
* The [Global Configuration](https://github.com/curiosity-ai/h5/wiki/global-configuration) documentation is important
* Check out [H5](https://retyped.com/) for 2400+ supported libraries ([demos](https://demos.retyped.com))
* Licensed under [Apache License, Version 2.0](https://github.com/curiosity-ai/h5/blob/master/LICENSE.md)
* Need Help? H5.NET [Forums](http://forums.h5.net/) or GitHub [Issues](https://github.com/curiosity-ai/h5/issues)
* [@h5dotnet](https://twitter.com/h5dotnet) on Twitter
* [Gitter](https://gitter.im/h5dotnet/H5) for messaging

## Getting Started

A great place to start if you're new to H5 is reviewing the [Getting Started](https://github.com/curiosity-ai/h5/wiki) wiki.

The easiest place to see H5 in action is [Deck.NET](https://deck.net/). 

[![Video Tutorial](https://user-images.githubusercontent.com/62210/30412015-ee0e9ccc-98d1-11e7-9a28-3bc02b900190.png)](https://www.youtube.com/watch?v=cEUR1UthE2c)

## Sample

The following code sample demonstrates a simple **App.cs** class that will run automatically on page load and write a message to the H5 Console.

**Example ([Deck](https://deck.net/7fb39e336182bea04c695ab43379cd8c))**

```csharp
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello World!");
    }
}
```

The C# class above will be compiled into JavaScript and added to **/H5/ouput/demo.js** within your project. By default, H5 will use the Namespace name as the file name. In this case: **demo.js**. There are many options to control the output of your JavaScript files, and the [Attribute Reference](https://github.com/curiosity-ai/h5/wiki/attribute-reference) is important [documentation](https://github.com/curiosity-ai/h5/wiki) to review.

```javascript
H5.define("Demo.Program", {
    main: function Main() {
        System.Console.WriteLine("Hello World!");
    }
});
```

## Installation

A full list of installation options available at [h5.net/download/](http://h5.net/download/), including full support for Visual Studio and Visual Studio Community on Windows, and Visual Studio Mac.

### H5 for Visual Studio

If you're using Visual Studio for Windows, the easiest way to get started is by adding the H5.NET for Visual Studio [extension](https://visualstudiogallery.msdn.microsoft.com/dca5c80f-a0df-4944-8343-9c905db84757).

From within Visual Studio, go to the `Tools > Extensions and Updates...`.

![H5 for Visual Studio](https://user-images.githubusercontent.com/62210/29292228-932ebb7e-8103-11e7-952a-3088274acf10.png)

From the options on the left side, be sure to select **Online**, then search for **H5**. Clicking **Download** will install H5 for Visual Studio. After installation is complete, Visual Studio may require a restart. 

![Visual Studio Extensions and Updates](https://user-images.githubusercontent.com/62210/29292229-93406b44-8103-11e7-90a0-30232486a5a7.png)

Once installation is complete you will have a new **H5.NET** project type. When creating new H5 enabled projects, select this project type. 
### NuGet

Another option is installation of H5 into a new **C# Class Library** project using [NuGet](https://www.nuget.org/packages/h5). Within the NuGet Package Manager, search for **H5** and click to install. 

H5 can also be installed using the NuGet Command Line tool by running the following command:

```
Install-Package H5
```

More information regarding Nuget package installation for H5 is available in the [Documentation](https://github.com/curiosity-ai/h5/wiki/nuget-installation).

## Contributing

Interested in contributing to H5? Please see [CONTRIBUTING.md](https://github.com/curiosity-ai/h5/blob/master/.github/CONTRIBUTING.md).

We also flag some Issues as [up-for-grabs](https://github.com/curiosity-ai/h5/issues?q=is%3Aopen+is%3Aissue+label%3Aup-for-grabs). These are generally easy introductions to the inner workings of H5, and are items we just haven't had time to implement. Your help is always appreciated.

## Badges

Show your support by adding a **built with H5.NET** badge to your projects README or website.

[![Built with H5.NET](https://img.shields.io/badge/built%20with-H5.NET-blue.svg)](http://h5.net/)

#### Markdown

```md
[![Built with H5.NET](https://img.shields.io/badge/built%20with-H5.NET-blue.svg)](http://h5.net/)
```

#### HTML

```html
<a href="http://h5.net/">
    <img src="https://img.shields.io/badge/built%20with-H5.NET-blue.svg" title="Built with H5.NET" />
</a>
```

## How to Help

We need your help spreading the word about H5. Any of the following items will help:

1. Star the [H5](https://github.com/curiosity-ai/h5/) project on GitHub
1. Add a [Badge](#badges)
1. Leave a review at [Visual Studio Gallery](https://marketplace.visualstudio.com/items?itemName=H5NET.H5NET)
1. Blog about H5.NET
1. Tweet about [@h5dotnet](https://twitter.com/h5dotnet)
1. Start a discussion on [Reddit](http://reddit.com/r/programming) or [Hacker News](https://news.ycombinator.com/)
1. Answer H5 related questions on [StackOverflow](http://stackoverflow.com/questions/tagged/h5.net)
1. Give a local usergroup presentation on H5
1. Give a conference talk on H5
1. Provide feedback ([forums](http://forums.h5.net), [GitHub](https://github.com/curiosity-ai/h5/issues) or [email](mailto:hello@h5.net))

## Testing

H5 is continually tested with the full test runner available at http://testing.h5.net/. 

## Credits

H5 is developed by the team at [Object.NET](http://object.net/). Frameworks and Tools for .NET Developers.

## License

**Apache License, Version 2.0**

Please see [LICENSE](https://github.com/curiosity-ai/h5/blob/master/LICENSE.md) for details.
