![HighFive.NET logo](https://speed.highfive.net/identity/highfivedotnet-sh.png)

<p align="center"><img src="https://user-images.githubusercontent.com/62210/29276839-1759fbe8-80cd-11e7-921c-d509e0e2a22c.png"></p>

[![Build status](https://ci.appveyor.com/api/projects/status/nm2f0c0u1jx0sniq/branch/master?svg=true)](https://ci.appveyor.com/project/ObjectDotNet/highfive/branch/master)
[![Build Status](https://travis-ci.org/highfivedotnet/HighFive.svg?branch=master)](https://travis-ci.org/highfivedotnet/HighFive)
[![NuGet Status](https://img.shields.io/nuget/v/HighFive.svg)](https://www.nuget.org/packages/HighFive)
[![Join the chat at https://gitter.im/highfivedotnet/HighFive](https://badges.gitter.im/highfivedotnet/HighFive.svg)](https://gitter.im/highfivedotnet/HighFive?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[HighFive.NET](http://highfive.net/) is an open source C#-to-JavaScript Compiler. Write your application in C# and run on billions of devices.

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
HighFive.define("Demo.Program", {
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

* Read the [Getting Started](https://github.com/highfivedotnet/HighFive/wiki) Knowledge Base article
* Try [Deck](https://deck.net/) if you want to play
* Installation:
  * Add **HighFive.NET** Visual Studio extension, or 
  * Use [NuGet](https://www.nuget.org/packages/highfive) to install into a C# Class Library project (`Install-Package HighFive`), or
  * [Download](http://highfive.net/download/) the Visual Studio Code starter project
* The [Attribute Reference](https://github.com/highfivedotnet/HighFive/wiki/attribute-reference) documentation is important
* The [Global Configuration](https://github.com/highfivedotnet/HighFive/wiki/global-configuration) documentation is important
* Check out [H5](https://retyped.com/) for 2400+ supported libraries ([demos](https://demos.retyped.com))
* Licensed under [Apache License, Version 2.0](https://github.com/highfivedotnet/HighFive/blob/master/LICENSE.md)
* Need Help? HighFive.NET [Forums](http://forums.highfive.net/) or GitHub [Issues](https://github.com/highfivedotnet/HighFive/issues)
* [@highfivedotnet](https://twitter.com/highfivedotnet) on Twitter
* [Gitter](https://gitter.im/highfivedotnet/HighFive) for messaging

## Getting Started

A great place to start if you're new to HighFive is reviewing the [Getting Started](https://github.com/highfivedotnet/HighFive/wiki) wiki.

The easiest place to see HighFive in action is [Deck.NET](https://deck.net/). 

[![Video Tutorial](https://user-images.githubusercontent.com/62210/30412015-ee0e9ccc-98d1-11e7-9a28-3bc02b900190.png)](https://www.youtube.com/watch?v=cEUR1UthE2c)

## Sample

The following code sample demonstrates a simple **App.cs** class that will run automatically on page load and write a message to the HighFive Console.

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

The C# class above will be compiled into JavaScript and added to **/HighFive/ouput/demo.js** within your project. By default, HighFive will use the Namespace name as the file name. In this case: **demo.js**. There are many options to control the output of your JavaScript files, and the [Attribute Reference](https://github.com/highfivedotnet/HighFive/wiki/attribute-reference) is important [documentation](https://github.com/highfivedotnet/HighFive/wiki) to review.

```javascript
HighFive.define("Demo.Program", {
    main: function Main() {
        System.Console.WriteLine("Hello World!");
    }
});
```

## Installation

A full list of installation options available at [highfive.net/download/](http://highfive.net/download/), including full support for Visual Studio and Visual Studio Community on Windows, and Visual Studio Mac.

### HighFive for Visual Studio

If you're using Visual Studio for Windows, the easiest way to get started is by adding the HighFive.NET for Visual Studio [extension](https://visualstudiogallery.msdn.microsoft.com/dca5c80f-a0df-4944-8343-9c905db84757).

From within Visual Studio, go to the `Tools > Extensions and Updates...`.

![HighFive for Visual Studio](https://user-images.githubusercontent.com/62210/29292228-932ebb7e-8103-11e7-952a-3088274acf10.png)

From the options on the left side, be sure to select **Online**, then search for **HighFive**. Clicking **Download** will install HighFive for Visual Studio. After installation is complete, Visual Studio may require a restart. 

![Visual Studio Extensions and Updates](https://user-images.githubusercontent.com/62210/29292229-93406b44-8103-11e7-90a0-30232486a5a7.png)

Once installation is complete you will have a new **HighFive.NET** project type. When creating new HighFive enabled projects, select this project type. 
### NuGet

Another option is installation of HighFive into a new **C# Class Library** project using [NuGet](https://www.nuget.org/packages/highfive). Within the NuGet Package Manager, search for **HighFive** and click to install. 

HighFive can also be installed using the NuGet Command Line tool by running the following command:

```
Install-Package HighFive
```

More information regarding Nuget package installation for HighFive is available in the [Documentation](https://github.com/highfivedotnet/HighFive/wiki/nuget-installation).

## Contributing

Interested in contributing to HighFive? Please see [CONTRIBUTING.md](https://github.com/highfivedotnet/HighFive/blob/master/.github/CONTRIBUTING.md).

We also flag some Issues as [up-for-grabs](https://github.com/highfivedotnet/HighFive/issues?q=is%3Aopen+is%3Aissue+label%3Aup-for-grabs). These are generally easy introductions to the inner workings of HighFive, and are items we just haven't had time to implement. Your help is always appreciated.

## Badges

Show your support by adding a **built with HighFive.NET** badge to your projects README or website.

[![Built with HighFive.NET](https://img.shields.io/badge/built%20with-HighFive.NET-blue.svg)](http://highfive.net/)

#### Markdown

```md
[![Built with HighFive.NET](https://img.shields.io/badge/built%20with-HighFive.NET-blue.svg)](http://highfive.net/)
```

#### HTML

```html
<a href="http://highfive.net/">
    <img src="https://img.shields.io/badge/built%20with-HighFive.NET-blue.svg" title="Built with HighFive.NET" />
</a>
```

## How to Help

We need your help spreading the word about HighFive. Any of the following items will help:

1. Star the [HighFive](https://github.com/highfivedotnet/HighFive/) project on GitHub
1. Add a [Badge](#badges)
1. Leave a review at [Visual Studio Gallery](https://marketplace.visualstudio.com/items?itemName=HighFiveNET.HighFiveNET)
1. Blog about HighFive.NET
1. Tweet about [@highfivedotnet](https://twitter.com/highfivedotnet)
1. Start a discussion on [Reddit](http://reddit.com/r/programming) or [Hacker News](https://news.ycombinator.com/)
1. Answer HighFive related questions on [StackOverflow](http://stackoverflow.com/questions/tagged/highfive.net)
1. Give a local usergroup presentation on HighFive
1. Give a conference talk on HighFive
1. Provide feedback ([forums](http://forums.highfive.net), [GitHub](https://github.com/highfivedotnet/HighFive/issues) or [email](mailto:hello@highfive.net))

## Testing

HighFive is continually tested with the full test runner available at http://testing.highfive.net/. 

## Credits

HighFive is developed by the team at [Object.NET](http://object.net/). Frameworks and Tools for .NET Developers.

## License

**Apache License, Version 2.0**

Please see [LICENSE](https://github.com/highfivedotnet/HighFive/blob/master/LICENSE.md) for details.
