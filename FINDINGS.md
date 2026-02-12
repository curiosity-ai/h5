# Findings

## Integers_ArithmeticAndBitwise Failure

The test `Integers_ArithmeticAndBitwise` failed because `Console.WriteLine` with a `long` argument prints the type name `System.Int64` instead of the numeric value in H5.

**Roslyn Output:**
```
...
2469135780246
...
```

**H5 Output:**
```
...
System.Int64
...
```

## Enums Failure

The test `Enums` failed because `Console.WriteLine(enumValue)` prints the underlying integer value instead of the string representation (name), and `[Flags]` enums are not formatted as comma-separated strings.

**Roslyn Output:**
```
Green
2
Read, Write
True
False
```

**H5 Output:**
```
2
2
3
True
False
```
