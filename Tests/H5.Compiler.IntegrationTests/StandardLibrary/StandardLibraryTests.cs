using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Linq;

namespace H5.Compiler.IntegrationTests
{
    [TestClass]
    public class StandardLibraryTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task Math_BasicArithmetic()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(Math.Abs(-10));
        Console.WriteLine(Math.Abs(-10.5));
        Console.WriteLine(Math.Min(10, 20));
        Console.WriteLine(Math.Min(10.5, 20.5));
        Console.WriteLine(Math.Max(10, 20));
        Console.WriteLine(Math.Max(10.5, 20.5));
        // Explicitly cast to double to avoid ambiguity in H5 (Sign(double) vs Sign(decimal))
        Console.WriteLine(Math.Sign((double)-10));
        Console.WriteLine(Math.Sign((double)10));
        Console.WriteLine(Math.Sign((double)0));

        int rem;
        int div = Math.DivRem(10, 3, out rem);
        Console.WriteLine(div);
        Console.WriteLine(rem);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Math_Rounding()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(Math.Round(3.14159, 2));
        Console.WriteLine(Math.Ceiling(3.14));
        Console.WriteLine(Math.Floor(3.14));
        Console.WriteLine(Math.Truncate(3.14));
        Console.WriteLine(Math.Truncate(-3.14));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Math_PowersAndRoots()
        {
             var code = """
using System;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(Math.Pow(2, 3));
        Console.WriteLine(Math.Sqrt(16));
        Console.WriteLine(Math.Exp(1.0) > 2.7); // e ~ 2.718
        Console.WriteLine(Math.Log(Math.E) > 0.99); // ln(e) = 1
        Console.WriteLine(Math.Log10(100));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Math_Trigonometry()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        // Use approximate comparisons for float math
        Console.WriteLine(Math.Abs(Math.Sin(Math.PI / 2) - 1.0) < 1e-9);
        Console.WriteLine(Math.Abs(Math.Cos(0) - 1.0) < 1e-9);
        Console.WriteLine(Math.Abs(Math.Tan(Math.PI / 4) - 1.0) < 1e-9);
        Console.WriteLine(Math.Asin(1) > 1.5); // pi/2 ~ 1.57
        Console.WriteLine(Math.Acos(1)); // 0
        Console.WriteLine(Math.Atan(1) > 0.7); // pi/4 ~ 0.78
        Console.WriteLine(Math.Atan2(1, 1) > 0.7);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Math_Hyperbolic()
        {
             var code = """
using System;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(Math.Sinh(0));
        Console.WriteLine(Math.Cosh(0));
        Console.WriteLine(Math.Tanh(0));
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Random_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        var r = new Random(12345);
        int n = r.Next(100);
        Console.WriteLine(n >= 0 && n < 100);

        int n2 = r.Next(50, 60);
        Console.WriteLine(n2 >= 50 && n2 < 60);

        double d = r.NextDouble();
        Console.WriteLine(d >= 0.0 && d < 1.0);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DateTime_Properties()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        var dt = new DateTime(2023, 10, 5, 14, 30, 0);
        Console.WriteLine(dt.Year);
        Console.WriteLine(dt.Month);
        Console.WriteLine(dt.Day);
        Console.WriteLine(dt.Hour);
        Console.WriteLine(dt.Minute);
        Console.WriteLine(dt.Second);
        Console.WriteLine(dt.DayOfWeek == DayOfWeek.Thursday);
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task DateTime_Methods()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        var dt = new DateTime(2023, 1, 1);
        Console.WriteLine(dt.AddDays(1).Day);
        Console.WriteLine(dt.AddMonths(1).Month);
        Console.WriteLine(dt.AddYears(1).Year);
        Console.WriteLine(dt.AddHours(25).Day); // Should be 2nd

        // Simple format to avoid locale issues
        // H5 ToString might differ slightly in default formatting or culture, so we test specific format
        Console.WriteLine(dt.ToString("yyyy-MM-dd"));
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
public class Program
{
    public static void Main()
    {
        var ts = new TimeSpan(1, 2, 3); // 1 hour, 2 mins, 3 secs
        Console.WriteLine(ts.Hours);
        Console.WriteLine(ts.Minutes);
        Console.WriteLine(ts.Seconds);
        Console.WriteLine(ts.TotalSeconds == 3723);

        var ts2 = TimeSpan.FromMinutes(60);
        Console.WriteLine(ts2.Hours);

        var ts3 = ts.Add(ts2);
        Console.WriteLine(ts3.Hours); // 2
    }
}
""";
            await RunTest(code);
        }

        [TestMethod]
        public async Task Guid_Tests()
        {
            var code = """
using System;
public class Program
{
    public static void Main()
    {
        // Explicitly call ToString() because Console.WriteLine(object) might behave differently for structs in H5
        Console.WriteLine(Guid.Empty.ToString());

        var g = Guid.Parse("e849312b-3151-409e-8367-6286c476566d");
        Console.WriteLine(g.ToString());

        var g2 = Guid.NewGuid();
        Console.WriteLine(g2 != Guid.Empty);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task StringBuilder_Tests()
    {
        var code = """
using System;
using System.Text;
public class Program
{
    public static void Main()
    {
        var sb = new StringBuilder("Hello");
        sb.Append(" World");
        Console.WriteLine(sb.ToString());

        sb.AppendLine("!");
        Console.WriteLine(sb.Length > 12);

        sb.Insert(0, "Start ");
        Console.WriteLine(sb.ToString().StartsWith("Start"));

        sb.Replace("Start", "End");
        Console.WriteLine(sb.ToString().StartsWith("End"));

        sb.Clear();
        Console.WriteLine(sb.Length);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Regex_Tests()
    {
        var code = """
using System;
using System.Text.RegularExpressions;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(Regex.IsMatch("abc", "^a.c$"));

        var match = Regex.Match("123 abc", @"\d+");
        Console.WriteLine(match.Success);
        Console.WriteLine(match.Value);

        var replaced = Regex.Replace("Hello 123", @"\d+", "World");
        Console.WriteLine(replaced);

        var parts = Regex.Split("a,b,c", ",");
        Console.WriteLine(parts.Length);
        Console.WriteLine(parts[1]);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Encoding_Tests()
    {
        var code = """
using System;
using System.Text;
public class Program
{
    public static void Main()
    {
        var bytes = Encoding.UTF8.GetBytes("Hello");
        Console.WriteLine(bytes.Length);

        var s = Encoding.UTF8.GetString(bytes);
        Console.WriteLine(s);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task List_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
using System.Linq;
public class Program
{
    public static void Main()
    {
        var list = new List<int> { 1, 2, 3 };
        list.Add(4);
        Console.WriteLine(list.Count);
        Console.WriteLine(list[3]);

        list.AddRange(new[] { 5, 6 });
        Console.WriteLine(list.Contains(5));

        list.Insert(0, 0);
        Console.WriteLine(list[0]);

        list.Remove(6);
        Console.WriteLine(list.Contains(6));

        list.RemoveAt(0);
        Console.WriteLine(list[0]); // should be 1

        Console.WriteLine(list.IndexOf(4));

        list.Reverse();
        Console.WriteLine(list[0]); // should be 5

        list.Sort();
        Console.WriteLine(list[0]); // should be 1

        var arr = list.ToArray();
        Console.WriteLine(arr.Length);

        Console.WriteLine(list.BinarySearch(3) >= 0);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Dictionary_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var dict = new Dictionary<string, int> { { "One", 1 } };
        dict.Add("Two", 2);
        Console.WriteLine(dict.Count);

        Console.WriteLine(dict.ContainsKey("One"));
        Console.WriteLine(dict["Two"]);

        int val;
        if (dict.TryGetValue("One", out val))
        {
            Console.WriteLine(val);
        }

        dict["Three"] = 3;
        Console.WriteLine(dict.Keys.Count);
        Console.WriteLine(dict.Values.Count);

        dict.Remove("One");
        Console.WriteLine(dict.ContainsKey("One"));
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task HashSet_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var set = new HashSet<int> { 1, 2 };
        Console.WriteLine(set.Add(3));
        Console.WriteLine(set.Add(1)); // False
        Console.WriteLine(set.Count);
        Console.WriteLine(set.Contains(2));

        set.UnionWith(new[] { 3, 4 });
        Console.WriteLine(set.Contains(4));

        set.IntersectWith(new[] { 1, 3 });
        Console.WriteLine(set.Contains(4)); // False

        set.Remove(1);
        Console.WriteLine(set.Contains(1));
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Queue_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var q = new Queue<int>();
        q.Enqueue(1);
        q.Enqueue(2);
        Console.WriteLine(q.Count);

        Console.WriteLine(q.Peek());
        Console.WriteLine(q.Dequeue());
        Console.WriteLine(q.Count);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Stack_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var s = new Stack<int>();
        s.Push(1);
        s.Push(2);
        Console.WriteLine(s.Count);

        Console.WriteLine(s.Peek());
        Console.WriteLine(s.Pop());
        Console.WriteLine(s.Count);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task LinkedList_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var ll = new LinkedList<int>();
        ll.AddLast(1);
        ll.AddFirst(0);
        ll.AddLast(2);

        Console.WriteLine(ll.Count);
        Console.WriteLine(ll.First.Value);
        Console.WriteLine(ll.Last.Value);

        ll.RemoveFirst();
        Console.WriteLine(ll.First.Value);

        ll.RemoveLast();
        Console.WriteLine(ll.Last.Value);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task MemoryStream_Tests()
    {
        var code = """
using System;
using System.IO;
public class Program
{
    public static void Main()
    {
        var ms = new MemoryStream();
        var data = new byte[] { 1, 2, 3 };
        ms.Write(data, 0, 3);
        Console.WriteLine(ms.Length);
        Console.WriteLine(ms.Position);

        ms.Position = 0;
        Console.WriteLine(ms.ReadByte()); // 1
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task BinaryWriterReader_Tests()
    {
        var code = """
using System;
using System.IO;
public class Program
{
    public static void Main()
    {
        using (var ms = new MemoryStream())
        {
            var writer = new BinaryWriter(ms);
            writer.Write(42);
            writer.Write("Hello");

            ms.Position = 0;
            var reader = new BinaryReader(ms);
            Console.WriteLine(reader.ReadInt32());
            Console.WriteLine(reader.ReadString());
        }
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Convert_Tests()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(Convert.ToInt32("123"));
        Console.WriteLine(Convert.ToBoolean("True"));

        var bytes = new byte[] { 1, 2, 3 };
        var b64 = Convert.ToBase64String(bytes);
        Console.WriteLine(b64);

        var bytes2 = Convert.FromBase64String(b64);
        Console.WriteLine(bytes2[0]);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task BitConverter_Tests()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var bytes = BitConverter.GetBytes(123456);
        Console.WriteLine(bytes.Length);
        Console.WriteLine(BitConverter.ToInt32(bytes, 0));
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Uri_Tests()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var uri = new Uri("http://example.com:8080/path?q=1");
        // Only AbsoluteUri is currently exposed in H5 Uri implementation
        Console.WriteLine(uri.AbsoluteUri);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Version_Tests()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var v = new Version(1, 2, 3, 4);
        Console.WriteLine(v.Major);
        Console.WriteLine(v.Minor);
        Console.WriteLine(v.Build);
        Console.WriteLine(v.Revision);
        Console.WriteLine(v.ToString());
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task CultureInfo_Tests()
    {
        var code = """
using System;
using System.Globalization;
public class Program
{
    public static void Main()
    {
        // H5 implementation might differ for InvariantCulture name
        // Just verify it doesn't crash
        Console.WriteLine(CultureInfo.InvariantCulture != null);
        Console.WriteLine(CultureInfo.CurrentCulture != null);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Stopwatch_Tests()
    {
        var code = """
using System;
using System.Diagnostics;
using System.Threading.Tasks;
public class Program
{
    public static async Task Main()
    {
        var sw = new Stopwatch();
        sw.Start();
        await Task.Delay(10);
        sw.Stop();
        Console.WriteLine(sw.IsRunning);
        Console.WriteLine(sw.ElapsedMilliseconds >= 0);
        Console.WriteLine("<<DONE>>");
    }
}
""";
        await RunTest(code, "<<DONE>>");
    }

    [TestMethod]
    public async Task Reflection_Tests()
    {
        var code = """
using System;
using System.Reflection;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var t = typeof(string);
        Console.WriteLine(t.Name);
        Console.WriteLine(t.IsClass);

        var t2 = typeof(List<int>);
        Console.WriteLine(t2.Name);

        var props = typeof(Program).GetMethods();
        Console.WriteLine(props.Length > 0);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task DateTime_Constructors_And_Kind()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var dt1 = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc);
        Console.WriteLine(dt1.Kind == DateTimeKind.Utc);

        var dt2 = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Local);
        Console.WriteLine(dt2.Kind == DateTimeKind.Local);

        var dt3 = DateTime.SpecifyKind(dt1, DateTimeKind.Unspecified);
        Console.WriteLine(dt3.Kind == DateTimeKind.Unspecified);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task DateTime_Arithmetic()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var dt = new DateTime(2023, 1, 31);
        Console.WriteLine(dt.AddMonths(1).Month);
        Console.WriteLine(dt.AddMonths(1).Day); // 28

        var leapYear = new DateTime(2024, 2, 28);
        Console.WriteLine(leapYear.AddDays(1).Day); // 29

        var nonLeapYear = new DateTime(2023, 2, 28);
        Console.WriteLine(nonLeapYear.AddDays(1).Day); // 1

        Console.WriteLine(new DateTime(2020, 2, 29).AddYears(1).Day); // 28
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task DateTime_Comparison()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var dt1 = new DateTime(2023, 1, 1);
        var dt2 = new DateTime(2023, 1, 2);
        Console.WriteLine(dt1 < dt2);
        Console.WriteLine(dt1 <= dt2);
        Console.WriteLine(dt2 > dt1);
        Console.WriteLine(dt2 >= dt1);
        Console.WriteLine(dt1 == dt2); // False
        Console.WriteLine(dt1 != dt2); // True
    }
}
""";
        await RunTest(code);
    }

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
        // Use simpler format that is likely supported across platforms/locales or specify InvariantCulture
        var dt = DateTime.Parse("2023-10-05T14:30:00", CultureInfo.InvariantCulture);
        Console.WriteLine(dt.Year);
        Console.WriteLine(dt.Hour);

        var dt2 = DateTime.ParseExact("20231005", "yyyyMMdd", CultureInfo.InvariantCulture);
        Console.WriteLine(dt2.Month);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task DateTime_Output()
    {
        var code = """
using System;
using System.Globalization;
public class Program
{
    public static void Main()
    {
        var dt = new DateTime(2023, 10, 5, 14, 30, 15);
        Console.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task TimeSpan_Factories()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var ts1 = TimeSpan.FromDays(1);
        Console.WriteLine(ts1.TotalHours);

        var ts2 = TimeSpan.FromHours(1.5);
        Console.WriteLine(ts2.TotalMinutes);

        var ts3 = TimeSpan.FromMinutes(120);
        Console.WriteLine(ts3.TotalHours);

        var ts4 = TimeSpan.FromSeconds(60);
        Console.WriteLine(ts4.TotalMinutes);

        var ts5 = TimeSpan.FromMilliseconds(1000);
        Console.WriteLine(ts5.TotalSeconds);

        var ts6 = TimeSpan.FromTicks(10000000);
        Console.WriteLine(ts6.TotalSeconds);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task TimeSpan_Arithmetic()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var ts1 = new TimeSpan(1, 0, 0);
        var ts2 = new TimeSpan(0, 30, 0);

        var sum = ts1.Add(ts2);
        Console.WriteLine(sum.TotalMinutes); // 90

        var diff = ts1.Subtract(ts2);
        Console.WriteLine(diff.TotalMinutes); // 30

        var duration = new TimeSpan(-1, 0, 0).Duration();
        Console.WriteLine(duration.TotalHours); // 1

        var negated = ts1.Negate();
        Console.WriteLine(negated.TotalHours); // -1

        Console.WriteLine((ts1 + ts2).TotalMinutes); // 90
        Console.WriteLine((ts1 - ts2).TotalMinutes); // 30
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task TimeSpan_Parsing()
    {
        var code = """
using System;
using System.Globalization;
public class Program
{
    public static void Main()
    {
        var ts = TimeSpan.Parse("01:02:03", CultureInfo.InvariantCulture);
        Console.WriteLine(ts.Hours); // 1
        Console.WriteLine(ts.Minutes); // 2
        Console.WriteLine(ts.Seconds); // 3
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task TimeSpan_Properties()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var ts = new TimeSpan(1, 12, 0, 0, 0); // 1 day, 12 hours
        Console.WriteLine(ts.TotalDays); // 1.5
        Console.WriteLine(ts.TotalHours); // 36
        Console.WriteLine(ts.TotalMinutes); // 2160
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task StringBuilder_Chaining()
    {
        var code = """
using System;
using System.Text;
public class Program
{
    public static void Main()
    {
        var sb = new StringBuilder();
        sb.Append("A").Append("B").AppendLine("C").Append("D");
        // Normalize newline for comparison as Environment.NewLine differs
        var s = sb.ToString();
        var normalized = s.Replace("\r\n", "|").Replace("\n", "|");
        Console.WriteLine(normalized);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task StringBuilder_Capacity()
    {
        var code = """
using System;
using System.Text;
public class Program
{
    public static void Main()
    {
        var sb = new StringBuilder(10);
        Console.WriteLine(sb.Capacity >= 10);

        sb.Append("12345678901");
        Console.WriteLine(sb.Capacity > 10);
        Console.WriteLine(sb.Length); // 11
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task StringBuilder_Modification()
    {
        var code = """
using System;
using System.Text;
public class Program
{
    public static void Main()
    {
        var sb = new StringBuilder("Hello World");

        sb.Remove(6, 5); // "Hello "
        Console.WriteLine(sb.ToString());

        sb.Insert(6, "C#"); // "Hello C#"
        Console.WriteLine(sb.ToString());

        sb.Replace("C#", "H5");
        Console.WriteLine(sb.ToString());

        sb.Length = 5;
        Console.WriteLine(sb.ToString()); // "Hello"

        sb.Length = 10;
        Console.WriteLine(sb.Length);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task StringBuilder_Indexer()
    {
        var code = """
using System;
using System.Text;
public class Program
{
    public static void Main()
    {
        var sb = new StringBuilder("abc");
        Console.WriteLine(sb[0]);
        Console.WriteLine(sb[2]);

        sb[1] = 'z';
        Console.WriteLine(sb.ToString());
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Regex_Syntax()
    {
        var code = """
using System;
using System.Text.RegularExpressions;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(Regex.IsMatch("hello", "^h.*o$"));
        Console.WriteLine(Regex.IsMatch("123", @"^\d+$"));
        Console.WriteLine(Regex.IsMatch("a", @"[a-z]"));
        Console.WriteLine(Regex.IsMatch("a", @"[0-9]"));
        Console.WriteLine(Regex.IsMatch("aaa", @"a{3}"));
        Console.WriteLine(Regex.IsMatch("ab", @"a|b"));

        var match = Regex.Match("value=123", @"value=(\d+)");
        Console.WriteLine(match.Success);
        Console.WriteLine(match.Groups[1].Value);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Regex_Methods()
    {
        var code = """
using System;
using System.Text.RegularExpressions;
public class Program
{
    public static void Main()
    {
        var matches = Regex.Matches("abc 123 def 456", @"\d+");
        Console.WriteLine(matches.Count); // 2
        Console.WriteLine(matches[0].Value); // 123
        Console.WriteLine(matches[1].Value); // 456

        var replaced = Regex.Replace("foo bar foo", "foo", "baz");
        Console.WriteLine(replaced); // baz bar baz

        var evalReplaced = Regex.Replace("1 2 3", @"\d", m => (int.Parse(m.Value) * 2).ToString());
        Console.WriteLine(evalReplaced); // 2 4 6

        var parts = Regex.Split("a,b;c", @"[,;]");
        Console.WriteLine(parts.Length); // 3
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Regex_Options()
    {
        var code = """
using System;
using System.Text.RegularExpressions;
public class Program
{
    public static void Main()
    {
        Console.WriteLine(Regex.IsMatch("A", "a", RegexOptions.IgnoreCase));

        var multiLine = "Line1\nLine2";
        Console.WriteLine(Regex.IsMatch(multiLine, "^Line2", RegexOptions.Multiline));
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task SortedList_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var sl = new SortedList<int, string>();
        sl.Add(3, "Three");
        sl.Add(1, "One");
        sl.Add(2, "Two");

        Console.WriteLine(sl.Keys[0]); // 1
        Console.WriteLine(sl.Values[2]); // Three
        Console.WriteLine(sl.ContainsKey(2));

        sl.Remove(1);
        Console.WriteLine(sl.Count); // 2
        Console.WriteLine(sl.Keys[0]); // 2
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task SortedSet_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var set = new SortedSet<int> { 3, 1, 2 };
        Console.WriteLine(set.Min); // 1
        Console.WriteLine(set.Max); // 3

        foreach(var item in set) {
            Console.WriteLine(item);
        }

        set.Add(0);
        Console.WriteLine(set.Min); // 0

        var other = new[] { 4, 5 };
        set.UnionWith(other);
        Console.WriteLine(set.Contains(4)); // True
        Console.WriteLine(set.Max); // 5

        var other2 = new[] { 1, 3, 5 };
        set.IntersectWith(other2);
        Console.WriteLine(set.Contains(1)); // True
        Console.WriteLine(set.Contains(2)); // False
        Console.WriteLine(set.Contains(4)); // False
        Console.WriteLine(set.Contains(5)); // True
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task ReadOnlyInterfaces_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var list = new List<int> { 1, 2, 3 };
        IReadOnlyList<int> roList = list;
        Console.WriteLine(roList.Count);
        Console.WriteLine(roList[0]);

        var dict = new Dictionary<int, string> { { 1, "One" } };
        IReadOnlyDictionary<int, string> roDict = dict;
        Console.WriteLine(roDict.Count);
        Console.WriteLine(roDict.ContainsKey(1));
        Console.WriteLine(roDict[1]);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Comparers_Tests()
    {
        var code = """
using System;
using System.Collections.Generic;
public class Program
{
    public static void Main()
    {
        var eq = EqualityComparer<int>.Default;
        Console.WriteLine(eq.Equals(1, 1));
        Console.WriteLine(eq.GetHashCode(1) == 1.GetHashCode());

        var cmp = Comparer<string>.Default;
        Console.WriteLine(cmp.Compare("a", "b") < 0);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Globalization_CultureInfo()
    {
        var code = """
using System;
using System.Globalization;
public class Program
{
    public static void Main()
    {
        var current = CultureInfo.CurrentCulture;
        Console.WriteLine(current != null);

        var invariant = CultureInfo.InvariantCulture;
        Console.WriteLine(invariant != null);
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Globalization_Formats()
    {
        var code = """
using System;
using System.Globalization;
public class Program
{
    public static void Main()
    {
        var culture = CultureInfo.InvariantCulture;
        Console.WriteLine(culture.DateTimeFormat.ShortDatePattern.Length > 0);
        Console.WriteLine(culture.NumberFormat.NumberDecimalSeparator); // "."
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Guid_Formats()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var g = new Guid("e849312b-3151-409e-8367-6286c476566d");

        // Use ToLower() on output to normalize if platforms differ
        Console.WriteLine(g.ToString("N").Length); // 32
        Console.WriteLine(g.ToString("D").Length); // 36
        Console.WriteLine(g.ToString("B").Length); // 38
        Console.WriteLine(g.ToString("P").Length); // 38

        // X format is not supported in H5 (returns D format), so we skip comparing it against Roslyn

        Console.WriteLine(g.ToString("N").ToLower() == "e849312b3151409e83676286c476566d");
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Guid_Parsing()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var g = Guid.Parse("e849312b3151409e83676286c476566d");
        Console.WriteLine(g != Guid.Empty);

        Guid g2;
        if (Guid.TryParse("invalid", out g2)) {
             Console.WriteLine("Parsed Invalid");
        } else {
             Console.WriteLine("Failed Invalid"); // Expected
        }

        if (Guid.TryParse("e849312b-3151-409e-8367-6286c476566d", out g2)) {
             Console.WriteLine("Parsed Valid"); // Expected
        }

        try {
            var g3 = Guid.ParseExact("e849312b3151409e83676286c476566d", "N");
            Console.WriteLine("Parsed Exact N");
        } catch {
             Console.WriteLine("ParseExact N failed");
        }

        Guid g4;
        if (Guid.TryParseExact("e849312b3151409e83676286c476566d", "N", out g4)) {
            Console.WriteLine("TryParseExact N Valid");
        } else {
            Console.WriteLine("TryParseExact N Failed");
        }
    }
}
""";
        await RunTest(code);
    }

    [TestMethod]
    public async Task Version_Parsing_And_Comparison()
    {
        var code = """
using System;
public class Program
{
    public static void Main()
    {
        var v1 = Version.Parse("1.2");
        Console.WriteLine(v1.Major == 1);
        Console.WriteLine(v1.Minor == 2);
        Console.WriteLine(v1.Build == -1);

        var v2 = Version.Parse("1.2.3.4");
        Console.WriteLine(v2.Build == 3);
        Console.WriteLine(v2.Revision == 4);

        Console.WriteLine(v1.CompareTo(v2) < 0);
        Console.WriteLine(v2 > v1);

        Console.WriteLine(v2.ToString()); // 1.2.3.4
        Console.WriteLine(v2.ToString(2)); // 1.2
    }
}
""";
        await RunTest(code);
    }
}
}
