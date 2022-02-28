// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

namespace H5.Core
{
    public  static partial class es5
    {
        [CombinedClass]
        [StaticInterface("Math.Interface")]
        public static class Math
        {
            public static double E
            {
                get;
            }

            public static double LN10
            {
                get;
            }

            public static double LN2
            {
                get;
            }

            public static double LOG2E
            {
                get;
            }

            public static double LOG10E
            {
                get;
            }

            public static double PI
            {
                get;
            }

            public static double SQRT1_2
            {
                get;
            }

            public static double SQRT2
            {
                get;
            }

            public static extern double abs(double x);

            public static extern double acos(double x);

            public static extern double asin(double x);

            public static extern double atan(double x);

            public static extern double atan2(double y, double x);

            public static extern double ceil(double x);

            public static extern double cos(double x);

            public static extern double exp(double x);

            public static extern double floor(double x);

            public static extern double log(double x);

            [ExpandParams]
            public static extern double max(params double[] values);

            [ExpandParams]
            public static extern double min(params double[] values);

            public static extern double pow(double x, double y);

            public static extern double random();

            public static extern double round(double x);

            public static extern double sin(double x);

            public static extern double sqrt(double x);

            public static extern double tan(double x);

            [Generated]
            [IgnoreCast]
            [ClassInterface]
            [Name("Math")]
            public interface Interface : IObject
            {
                double E { get; }

                double LN10 { get; }

                double LN2 { get; }

                double LOG2E { get; }

                double LOG10E { get; }

                double PI { get; }

                double SQRT1_2 { get; }

                double SQRT2 { get; }

                double abs(double x);

                double acos(double x);

                double asin(double x);

                double atan(double x);

                double atan2(double y, double x);

                double ceil(double x);

                double cos(double x);

                double exp(double x);

                double floor(double x);

                double log(double x);

                [ExpandParams]
                double max(params double[] values);

                [ExpandParams]
                double min(params double[] values);

                double pow(double x, double y);

                double random();

                double round(double x);

                double sin(double x);

                double sqrt(double x);

                double tan(double x);
            }
        }
    }
}
