using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Translator.Roslyn.Tests.Ported
{
    [TestClass]
    public class FuzzerTests : TranslatorTestBase
    {
        [TestMethod]
        public async Task Test_Fail_537251934()
        {
            var code = """
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static string Method_0(int p0, int p1)
    {
        p0 = (26) + (53);
        for (int i0 = 0; i0 < 5; i0++)
        {
            for (int i1 = 0; i1 < 2; i1++)
            {
                if (true)
                {
                    string v2 = "1ac5f314";
                    bool v3 = true;
                    bool v4 = (v3) == (false);
                }
            }
        }

        if ((false) && (true))
        {
            Console.WriteLine("Trace 5");
            bool v5 = false;
            v5 = v5;
        }

        Console.WriteLine(p1);
        p0 = ((((p0) + (p0)) - (p1)) + (79)) + ((98) - ((50) + (0)));
        return "333c1954";
    }

    public static async Task Main()
    {
        Console.WriteLine("Program Start");
        Console.WriteLine("Trace 0");
        Console.WriteLine("Trace 0");
        for (int i0 = 0; i0 < 7; i0++)
        {
            Console.WriteLine(i0);
            i0 = await await Task.FromResult(Task.FromResult(37));
            try
            {
                if (false)
                {
                    bool v1 = await await Task.FromResult(Task.FromResult(false));
                    string v2 = "25b1388c";
                    bool v3 = (true) && (false);
                }
                else
                {
                    bool v4 = ("d19bd942") == ("9ec8ee08");
                    Console.WriteLine("Trace 5");
                }
            }
            catch (NullReferenceException ex5)
            {
                Console.WriteLine("Trace 6");
            }
        }

        try
        {
        }
        catch (ArgumentException ex6)
        {
            if ((89) < (94))
            {
                for (int i7 = 0; i7 < 2; i7++)
                {
                    bool v8 = ((false) && (true)) && (false);
                    v8 = true;
                }

                switch (await Task.FromResult("6f5e7f53"))
                {
                    case "1a91219a":
                        bool v9 = false;
                        string v10 = Method_0(67, 38);
                        break;
                    case "29746379":
                        Console.WriteLine("Trace");
                        bool v11 = (false) == (true);
                        break;
                    default:
                        int v12 = 16;
                        break;
                }
            }
            else
            {
                try
                {
                    string v13 = "8c9b1c1e";
                    bool v14 = await await Task.FromResult(Task.FromResult(true));
                }
                catch (Exception ex15)
                {
                    bool v16 = await Task.FromResult(true);
                }
                finally
                {
                    bool v17 = true;
                }
            }
        }
        finally
        {
            try
            {
                string v18 = Method_0(((await Task.FromResult(97)) - (88)) * (44), await Task.FromResult(66));
            }
            catch (NullReferenceException ex19)
            {
                await Task.Delay(51);
            }
        }

        switch (23)
        {
            case 34:
                if (true)
                {
                    int v20 = 62;
                    v20 = 78;
                }

                break;
            case 92:
                try
                {
                    await Task.Delay(56);
                    Console.WriteLine("Trace");
                }
                catch (Exception ex21)
                {
                    await Task.Delay(98);
                }

                break;
            default:
                for (int i22 = 0; i22 < 7; i22++)
                {
                    try
                    {
                        string v23 = await Task.FromResult("8af6079d");
                        bool v24 = true;
                    }
                    catch (Exception ex25)
                    {
                        string v26 = Method_0(await Task.FromResult(28), i22);
                    }

                    switch (i22)
                    {
                        case 96:
                            int v27 = 66;
                            break;
                        case 8:
                            string v28 = await Task.FromResult("2dfe3cfe");
                            break;
                        default:
                            Console.WriteLine("Trace 29");
                            break;
                    }
                }

                break;
        }

        try
        {
            await Task.Delay(14);
        }
        catch (ArgumentException ex29)
        {
            await Task.Delay(87);
        }
        finally
        {
            await Task.Delay(47);
        }

        await Task.Delay(35);
        Console.WriteLine("Trace");
        await Task.Delay(53);
        Console.WriteLine("Program End");
    }
}
""";
            await RunTest(code, "Program End"); // Wait for specific output to ensure completion
        }
    }
}
