import re

with open("H5/H5/shared/System/DateTimeOffset.cs", "r") as f:
    content = f.read()

# Remove the explicit implementation of `ParseExact` that we added because there was already one.
content = content.replace(
    "public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles) {\n            if (TryParseExact(input, format, formatProvider, styles, out var d)) {\n                return d;\n            }\n            throw new FormatException(\"String was not recognized as a valid DateTimeOffset.\");\n        }",
    ""
)

# And now check `TryParseExact` signature clash.
if "DateTimeOffset already defines a member called 'TryParseExact'" not in "error CS0111: Type 'DateTimeOffset' already defines a member called 'ParseExact' with the same parameter types":
    pass # Wait, it only complained about ParseExact!

with open("H5/H5/shared/System/DateTimeOffset.cs", "w") as f:
    f.write(content)
