namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("Math")]
    public static class Math
    {
        [H5.Convention]
        public const double E = 2.7182818284590452354;

        [H5.Convention]
        public const double PI = 3.14159265358979323846;

        public static extern int Abs(int x);

        public static extern double Abs(double x);

        [H5.Template("{l}.abs()")]
        public static extern long Abs(long l);

        [H5.Template("{l}.abs()")]
        public static extern decimal Abs(decimal l);

        /// <summary>
        /// Returns the larger of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        public static extern byte Max(byte val1, byte val2);

        /// <summary>
        /// Returns the larger of two 8-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        
        public static extern sbyte Max(sbyte val1, sbyte val2);

        /// <summary>
        /// Returns the larger of two 16-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 16-bit signed integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        public static extern short Max(short val1, short val2);

        /// <summary>
        /// Returns the larger of two 16-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        
        public static extern ushort Max(ushort val1, ushort val2);

        /// <summary>
        /// Returns the larger of two single-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        public static extern float Max(float val1, float val2);

        /// <summary>
        /// Returns the larger of two 32-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 32-bit signed integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        public static extern int Max(int val1, int val2);

        /// <summary>
        /// Returns the larger of two 32-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        
        public static extern uint Max(uint val1, uint val2);

        /// <summary>
        /// Returns the larger of two double-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        public static extern double Max(double val1, double val2);

        /// <summary>
        /// Returns the larger of two 64-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 64-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 64-bit signed integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        [H5.Template("System.Int64.max({val1}, {val2})")]
        public static extern long Max(long val1, long val2);

        /// <summary>
        /// Returns the larger of two 64-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        [H5.Template("System.UInt64.max({val1}, {val2})")]
        
        public static extern ulong Max(ulong val1, ulong val2);

        /// <summary>
        /// Returns the larger of two decimal numbers.
        /// </summary>
        /// <param name="val1">The first of two decimal numbers to compare.</param>
        /// <param name="val2">The second of two decimal numbers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is larger.</returns>
        [H5.Template("System.Decimal.max({val1}, {val2})")]
        public static extern decimal Max(decimal val1, decimal val2);

        /// <summary>
        /// Returns the smaller of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        public static extern byte Min(byte val1, byte val2);

        /// <summary>
        /// Returns the smaller of two 8-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        
        public static extern sbyte Min(sbyte val1, sbyte val2);

        /// <summary>
        /// Returns the smaller of two 16-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 16-bit signed integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        public static extern short Min(short val1, short val2);

        /// <summary>
        /// Returns the smaller of two 16-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        
        public static extern ushort Min(ushort val1, ushort val2);

        /// <summary>
        /// Returns the smaller of two single-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        public static extern float Min(float val1, float val2);

        /// <summary>
        /// Returns the smaller of two 32-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 32-bit signed integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        public static extern int Min(int val1, int val2);

        /// <summary>
        /// Returns the smaller of two 32-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        
        public static extern uint Min(uint val1, uint val2);

        /// <summary>
        /// Returns the smaller of two double-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        public static extern double Min(double val1, double val2);

        /// <summary>
        /// Returns the smaller of two 64-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 64-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 64-bit signed integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        [H5.Template("System.Int64.min({val1}, {val2})")]
        public static extern long Min(long val1, long val2);

        /// <summary>
        /// Returns the smaller of two 64-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        [H5.Template("System.UInt64.min({val1}, {val2})")]
        
        public static extern ulong Min(ulong val1, ulong val2);

        /// <summary>
        /// Returns the smaller of two decimal numbers.
        /// </summary>
        /// <param name="val1">The first of two decimal numbers to compare.</param>
        /// <param name="val2">The second of two decimal numbers to compare.</param>
        /// <returns>Parameter val1 or val2, whichever is smaller.</returns>
        [H5.Template("System.Decimal.min({val1}, {val2})")]
        public static extern decimal Min(decimal val1, decimal val2);

        public static extern double Random();

        public static extern double Sqrt(double x);

        [H5.Template("{d}.ceil()")]
        public static extern decimal Ceiling(decimal d);

        [H5.Name("ceil")]
        public static extern double Ceiling(double d);

        public static extern double Floor(double x);

        [H5.Template("{d}.floor()")]
        public static extern decimal Floor(decimal d);

        [H5.Template("System.Decimal.round({x}, 6)")]
        public static extern decimal Round(decimal x);

        [H5.Template("H5.Math.round({d}, 0, 6)")]
        public static extern double Round(double d);

        [H5.Template("Math.round({d})")]
        public static extern double JsRound(double d);

        [H5.Template("System.Decimal.toDecimalPlaces({d}, {digits}, 6)")]
        public static extern decimal Round(decimal d, int digits);

        [H5.Template("H5.Math.round({d}, {digits}, 6)")]
        public static extern double Round(double d, int digits);

        [H5.Template("System.Decimal.round({d}, {method})")]
        public static extern decimal Round(decimal d, MidpointRounding method);

        [H5.Template("H5.Math.round({d}, 0, {method})")]
        public static extern double Round(double d, MidpointRounding method);

        [H5.Template("System.Decimal.toDecimalPlaces({d}, {digits}, {method})")]
        public static extern decimal Round(decimal d, int digits, MidpointRounding method);

        [H5.Template("H5.Math.round({d}, {digits}, {method})")]
        public static extern double Round(double d, int digits, MidpointRounding method);

        [H5.Template("H5.Math.IEEERemainder({x}, {y})")]
        public static extern double IEEERemainder(double x, double y);

        public static extern double Exp(double x);

        [H5.Template("{x}.exponential()")]
        public static extern decimal Exp(decimal x);

        [H5.Template("H5.Math.log({x})")]
        public static extern double Log(double x);

        [H5.Template("H5.Math.logWithBase({x}, {logBase})")]
        public static extern double Log(double x, double logBase);

        [H5.Template("H5.Math.logWithBase({x}, 10.0)")]
        public static extern double Log10(double x);

        [H5.Template("{x}.pow({y})")]
        public static extern decimal Pow(decimal x, decimal y);

        public static extern double Pow(double x, double y);

        public static extern double Pow(int x, int y);

        public static extern double Acos(double x);

        public static extern double Asin(double x);

        public static extern double Atan(double x);

        public static extern double Atan2(double y, double x);

        public static extern double Cos(double x);

        public static extern double Sin(double x);

        public static extern double Tan(double x);

        [H5.Template("H5.Int.trunc({d})")]
        public static extern double Truncate(double d);

        [H5.Template("{d}.trunc()")]
        public static extern decimal Truncate(decimal d);

        [H5.Template("H5.Int.sign({value})")]
        public static extern int Sign(double value);

        [H5.Template("{value}.sign()")]
        public static extern int Sign(decimal value);

        [H5.Template("H5.Math.divRem({a}, {b}, {result})")]
        public static extern int DivRem(int a, int b, out int result);

        [H5.Template("System.Int64.divRem({a}, {b}, {result})")]
        public static extern long DivRem(long a, long b, out long result);

        [H5.Template("H5.Math.sinh({value})")]
        public static extern double Sinh(double value);

        [H5.Template("H5.Math.cosh({value})")]
        public static extern double Cosh(double value);

        [H5.Template("H5.Math.tanh({value})")]
        public static extern double Tanh(double value);
    }
}