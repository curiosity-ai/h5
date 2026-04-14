import re

with open("H5/H5/System/DateTime.cs", "r") as f:
    content = f.read()

if "System.Globalization.DateTimeStyles" not in content:
    content = content.replace(
        "public static extern bool TryParseExact(string s, string format, IFormatProvider provider, out DateTime result);",
        "public static extern bool TryParseExact(string s, string format, IFormatProvider provider, out DateTime result);\n\n        [H5.Template(\"System.DateTime.tryParseExact({0}, {1}, {2}, {result}, {3} === 8)\")]\n        public static extern bool TryParseExact(string s, string format, IFormatProvider provider, System.Globalization.DateTimeStyles style, out DateTime result);\n\n        [H5.Template(\"System.DateTime.tryParseExact({0}, {1}, {2}, {result}, {3} === 8)\")]\n        public static extern bool TryParseExact(string s, string[] formats, IFormatProvider provider, System.Globalization.DateTimeStyles style, out DateTime result);"
    )

    content = content.replace(
        "public static extern DateTime ParseExact(string s, string format, IFormatProvider provider);",
        "public static extern DateTime ParseExact(string s, string format, IFormatProvider provider);\n\n        [H5.Template(\"System.DateTime.parseExact({0}, {1}, {2}, {3} === 8)\")]\n        public static extern DateTime ParseExact(string s, string format, IFormatProvider provider, System.Globalization.DateTimeStyles style);\n\n        [H5.Template(\"System.DateTime.parseExact({0}, {1}, {2}, {3} === 8)\")]\n        public static extern DateTime ParseExact(string s, string[] formats, IFormatProvider provider, System.Globalization.DateTimeStyles style);"
    )

    with open("H5/H5/System/DateTime.cs", "w") as f:
        f.write(content)

with open("H5/H5/shared/System/DateTime.cs", "r") as f:
    content = f.read()

if "TryParseExact(String s, String format, IFormatProvider provider, DateTimeStyles style" in content:
    content = content.replace(
        "public static Boolean TryParseExact(String s, String format, IFormatProvider provider, DateTimeStyles style, out DateTime result) {\n            throw NotImplemented.ByDesign;\n            // TODO: NotSupported\n            //DateTimeFormatInfo.ValidateStyles(style, \"style\");\n            //return DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, out result);\n        }",
        "public static Boolean TryParseExact(String s, String format, IFormatProvider provider, DateTimeStyles style, out DateTime result) {\n            result = default(DateTime);\n            return false;\n        }"
    )

    content = content.replace(
        "public static Boolean TryParseExact(String s, String[] formats, IFormatProvider provider, DateTimeStyles style, out DateTime result) {\n            throw NotImplemented.ByDesign;\n            // TODO: NotSupported\n            //DateTimeFormatInfo.ValidateStyles(style, \"style\");\n            //return DateTimeParse.TryParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style, out result);\n        }",
        "public static Boolean TryParseExact(String s, String[] formats, IFormatProvider provider, DateTimeStyles style, out DateTime result) {\n            result = default(DateTime);\n            return false;\n        }"
    )

    content = content.replace(
        "// TODO: NotSupported\n        //public static DateTime ParseExact(String s, String format, IFormatProvider provider) {\n        //    return (DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None));\n        //}",
        "public static DateTime ParseExact(String s, String format, IFormatProvider provider) {\n            if (TryParseExact(s, format, provider, DateTimeStyles.None, out var result)) return result;\n            throw new FormatException(\"String was not recognized as a valid DateTime.\");\n        }"
    )

    content = content.replace(
        "// TODO: NotSupported\n        //public static DateTime ParseExact(String s, String format, IFormatProvider provider, DateTimeStyles style) {\n        //    DateTimeFormatInfo.ValidateStyles(style, \"style\");\n        //    return (DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style));\n        //}",
        "public static DateTime ParseExact(String s, String format, IFormatProvider provider, DateTimeStyles style) {\n            if (TryParseExact(s, format, provider, style, out var result)) return result;\n            throw new FormatException(\"String was not recognized as a valid DateTime.\");\n        }"
    )

    content = content.replace(
        "// TODO: NotSupported\n        //public static DateTime ParseExact(String s, String[] formats, IFormatProvider provider, DateTimeStyles style) {\n        //    DateTimeFormatInfo.ValidateStyles(style, \"style\");\n        //    return DateTimeParse.ParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style);\n        //}",
        "public static DateTime ParseExact(String s, String[] formats, IFormatProvider provider, DateTimeStyles style) {\n            if (TryParseExact(s, formats, provider, style, out var result)) return result;\n            throw new FormatException(\"String was not recognized as a valid DateTime.\");\n        }"
    )

    with open("H5/H5/shared/System/DateTime.cs", "w") as f:
        f.write(content)

with open("H5/H5/shared/System/DateTimeOffset.cs", "r") as f:
    content = f.read()

if "public static bool TryParseExact" not in content:
    content = content.replace(
        "// Ensures the TimeSpan is valid to go in a DateTimeOffset.",
        "public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out DateTimeOffset result) {\n            if (DateTime.TryParseExact(input, format, formatProvider, styles, out var d)) {\n                result = new DateTimeOffset(d);\n                return true;\n            }\n            result = default(DateTimeOffset);\n            return false;\n        }\n\n        public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out DateTimeOffset result) {\n            if (DateTime.TryParseExact(input, formats, formatProvider, styles, out var d)) {\n                result = new DateTimeOffset(d);\n                return true;\n            }\n            result = default(DateTimeOffset);\n            return false;\n        }\n\n        public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles) {\n            if (TryParseExact(input, format, formatProvider, styles, out var d)) {\n                return d;\n            }\n            throw new FormatException(\"String was not recognized as a valid DateTimeOffset.\");\n        }\n\n        public static DateTimeOffset ParseExact(string input, string[] formats, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles) {\n            if (TryParseExact(input, formats, formatProvider, styles, out var d)) {\n                return d;\n            }\n            throw new FormatException(\"String was not recognized as a valid DateTimeOffset.\");\n        }\n\n        // Ensures the TimeSpan is valid to go in a DateTimeOffset."
    )

    with open("H5/H5/shared/System/DateTimeOffset.cs", "w") as f:
        f.write(content)

with open("Tests/H5.Compiler.IntegrationTests/StandardLibrary/DateAndTimeTests.cs", "r") as f:
    content = f.read()

new_datetime_tests = """
        [TestMethod]
        public async Task DateTime_Parsing()
        {
            var code = \"\"\"
using System;
using System.Globalization;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(DateTime.TryParse("2023-10-05", out var res1) && res1.Year == 2023);

        Console.WriteLine(DateTime.TryParseExact("20231005", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var res2) && res2.Month == 10);

        Console.WriteLine(DateTime.TryParseExact("2023-10-05 14:30:15", new[] { "yyyy-MM-dd HH:mm:ss", "yyyy/MM/dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var res3) && res3.Hour == 14);

        try { DateTime.ParseExact("invalid", "yyyyMMdd", CultureInfo.InvariantCulture); Console.WriteLine(false); } catch { Console.WriteLine(true); }
        try { DateTime.Parse("invalid"); Console.WriteLine(false); } catch { Console.WriteLine(true); }
    }
}
\"\"\";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DateTimeOffset_Parsing()
        {
            var code = \"\"\"
using System;
using System.Globalization;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(DateTimeOffset.TryParse("2023-10-05T14:30:00+02:00", out var res1) && res1.Offset.TotalHours == 2);

        Console.WriteLine(DateTimeOffset.TryParseExact("20231005+02:00", "yyyyMMddzzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out var res2) && res2.Offset.TotalHours == 2);

        Console.WriteLine(DateTimeOffset.TryParseExact("2023-10-05 14:30:15", new[] { "yyyy-MM-dd HH:mm:ss", "yyyy/MM/dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var res3) && res3.Hour == 14);
    }
}
\"\"\";
            await RunTest(code);
        }
"""
if "DateTime_Parsing" not in content:
    content = content.replace("public class DateAndTimeTests : IntegrationTestBase\n    {", "public class DateAndTimeTests : IntegrationTestBase\n    {" + new_datetime_tests)
    with open("Tests/H5.Compiler.IntegrationTests/StandardLibrary/DateAndTimeTests.cs", "w") as f:
        f.write(content)
