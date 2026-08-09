using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class OptionalParametersReproTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task TestOptionalParametersWithNamedArgument()
        {
            var code = @"
using System;

public interface IComponent {}
public class TextBlock {}
public class HTMLImageElement {}
public class Button { public void NoHover() {} }

public class Program
{
    public static void CreateBrand(int logoHeight, int logoWidth, int fontSize, bool linkHome, int marginLeft = 8, Action<IComponent> onClick = null, Action<TextBlock> customizeText = null, Action<HTMLImageElement> customizeLogo = null, Action<Button> customizeButton = null)
    {
        Console.WriteLine(""CreateBrand called"");
        Console.WriteLine(""marginLeft: "" + marginLeft);
        if (customizeButton != null) {
            Console.WriteLine(""customizeButton is not null"");
            customizeButton(new Button());
        } else {
            Console.WriteLine(""customizeButton is null"");
        }
    }

    public static void Main()
    {
        CreateBrand(64, 64, 30, false, customizeButton: b => {
            Console.WriteLine(""NoHover called"");
            b.NoHover();
        });
    }
}
";
            await RunTest(code);
        }
    }
}
