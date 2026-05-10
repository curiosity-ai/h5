using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class DateAndTimeTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task DateTime_Parsing()
        {
            var code = """
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
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DateTimeOffset_Parsing()
        {
            var code = """
using System;
using System.Globalization;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(DateTimeOffset.TryParse("2023-10-05T14:30:00+02:00", out var res1) && res1.Offset.TotalHours == 2);

        Console.WriteLine(DateTimeOffset.TryParseExact("20231005", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var res2) && res2.Month == 10);

        Console.WriteLine(DateTimeOffset.TryParseExact("2023-10-05 14:30:15", new[] { "yyyy-MM-dd HH:mm:ss", "yyyy/MM/dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var res3) && res3.Hour == 14);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DateTime_Tests()
        {
            var code = """
using System;
using System.Globalization;

public class Program
{
    public static void Main()
    {
        var dt = new DateTime(2023, 10, 5, 12, 30, 0);
        Console.WriteLine(dt.Year);
        Console.WriteLine(dt.Month);
        Console.WriteLine(dt.Day);
        Console.WriteLine(dt.Hour);
        Console.WriteLine(dt.Minute);
        Console.WriteLine(dt.Second);

        var dt2 = dt.AddDays(1);
        Console.WriteLine(dt2.Day);

        var dt3 = dt.AddHours(2);
        Console.WriteLine(dt3.Hour);

        var diff = dt2 - dt;
        Console.WriteLine(diff.TotalDays); // 1

        Console.WriteLine(dt < dt2);
        Console.WriteLine(dt == dt);

        Console.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss"));

        var parsed = DateTime.Parse("2023-10-05T12:30:00", CultureInfo.InvariantCulture);
        Console.WriteLine(parsed.Year);
        Console.WriteLine(parsed.Hour);

        DateTime res;
        if (DateTime.TryParse("2023-10-05", out res)) Console.WriteLine(res.Year);
        if (!DateTime.TryParse("invalid", out res)) Console.WriteLine("Format handled");
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DateTimeOffset_Tests()
        {
            var code = """
using System;
using System.Globalization;

public class Program
{
    public static void Main()
    {
        var offset = TimeSpan.FromHours(2);
        var dto = new DateTimeOffset(2023, 10, 5, 14, 30, 0, offset);

        Console.WriteLine(dto.Year);
        Console.WriteLine(dto.Offset.TotalHours); // 2

        var utc = dto.UtcDateTime;
        Console.WriteLine(utc.Hour); // 12 (14 - 2)

        var dto2 = dto.AddHours(1);
        Console.WriteLine(dto2.Hour); // 15

        Console.WriteLine(dto < dto2);

        // Comparison with different offsets
        var dto3 = new DateTimeOffset(2023, 10, 5, 12, 30, 0, TimeSpan.Zero);
        // dto (14:30 +02:00) == 12:30 UTC
        // dto3 (12:30 +00:00) == 12:30 UTC
        Console.WriteLine(dto == dto3); // True

        // H5 implementation might use local time for ToString if not specified?
        // Or specific format. Let's test properties mainly or standard ToString if predictable.
        // H5 DateTimeOffset.ToString() delegates to DateTime.ToString() on ClockDateTime (Local kind).
        // So output depends on browser timezone potentially?
        // Let's stick to properties and explicit formatting if supported, or just testing validity.

        // Parse
        try {
            var parsed = DateTimeOffset.Parse("2023-10-05T12:30:00Z", CultureInfo.InvariantCulture);
            Console.WriteLine(parsed.UtcDateTime.Hour); // 12
            Console.WriteLine(parsed.Offset == TimeSpan.Zero);
        } catch (Exception ex) {
            Console.WriteLine("Parse failed: " + ex.Message);
        }

        DateTimeOffset res;
        if (DateTimeOffset.TryParse("2023-10-05T14:30:00+02:00", out res)) {
             Console.WriteLine(res.Hour); // 14
             Console.WriteLine(res.Offset.TotalHours); // 2
        }
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task TimeSpan_Tests()
        {
            var code = """
using System;
using System.Globalization;

public class Program
{
    public static void Main()
    {
        var ts = new TimeSpan(1, 2, 3);
        Console.WriteLine(ts.Hours);
        Console.WriteLine(ts.Minutes);
        Console.WriteLine(ts.Seconds);

        var ts2 = TimeSpan.FromMinutes(90);
        Console.WriteLine(ts2.Hours); // 1
        Console.WriteLine(ts2.Minutes); // 30
        Console.WriteLine(ts2.TotalMinutes); // 90

        var sum = ts + ts2;
        Console.WriteLine(sum.Hours); // 2 (1 + 1)
        Console.WriteLine(sum.Minutes); // 32 (2 + 30)

        var diff = ts - ts2;
        Console.WriteLine(diff.TotalMinutes); // ~ -27

        Console.WriteLine(ts < ts2);

        Console.WriteLine(ts.ToString()); // 01:02:03 or similar

        var parsed = TimeSpan.Parse("01:02:03", CultureInfo.InvariantCulture);
        Console.WriteLine(parsed.TotalSeconds);

        TimeSpan res;
        if (TimeSpan.TryParse("02:30:00", out res)) Console.WriteLine(res.TotalMinutes); // 150
        if (!TimeSpan.TryParse("invalid", out res)) Console.WriteLine("Format handled");
    }
}
""";
            await RunTest(code);
        }
    }
}
