# Wow it actually called the C# code I wrote for `DateTimeOffset.TryParseExact`!
# The output is "True\nFalse\nTrue" vs "True\nTrue\nTrue".
# It failed on: `DateTimeOffset.TryParseExact("20231005+02:00", "yyyyMMddzzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out var res2)`
# The implementation delegates to `DateTime.TryParseExact`, but `DateTime` does NOT support parsing "zzz" (time zone offset) out of the box to yield a `DateTime` that retains it correctly, OR it parses it but ignores it when extracting to `DateTimeOffset`?
# Wait! `DateTime.TryParseExact` parses `20231005+02:00` into `DateTime` but discards the explicit offset `+02:00` or maps it to `Local` depending on `DateTimeStyles`.
# The second `DateTimeOffset.TryParseExact("2023-10-05 14:30:15", new[] { "yyyy-MM-dd HH:mm:ss", "yyyy/MM/dd" })` PASSED!
# It's exactly the `zzz` parsing that `DateTime` might not be doing correctly in H5 natively, OR my implementation of `TryParseExact` doesn't handle offsets correctly!
import re

with open("H5/H5/shared/System/DateTimeOffset.cs", "r") as f:
    content = f.read()

content = content.replace(
    """public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out DateTimeOffset result) {
            if (DateTime.TryParseExact(input, format, formatProvider, styles, out var d)) {
                result = new DateTimeOffset(d);
                return true;
            }
            result = default(DateTimeOffset);
            return false;
        }""",
    """public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out DateTimeOffset result) {
            bool parsed = DateTimeParse.TryParseExact(input, format, DateTimeFormatInfo.CurrentInfo, styles, out var d, out var offset);
            result = new DateTimeOffset(d.Ticks, offset);
            return parsed;
        }"""
)

content = content.replace(
    """public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out DateTimeOffset result) {
            if (DateTime.TryParseExact(input, formats, formatProvider, styles, out var d)) {
                result = new DateTimeOffset(d);
                return true;
            }
            result = default(DateTimeOffset);
            return false;
        }""",
    """public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out DateTimeOffset result) {
            bool parsed = DateTimeParse.TryParseExactMultiple(input, formats, DateTimeFormatInfo.CurrentInfo, styles, out var d, out var offset);
            result = new DateTimeOffset(d.Ticks, offset);
            return parsed;
        }"""
)
# Wait, DateTimeParse is inside `H5/shared/System/Globalization/DateTimeParse.cs`
# Let's check if `TryParseExact(input, format, formatProvider, styles, out d, out offset)` exists.
# We commented it out earlier when reading the file.
