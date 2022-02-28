// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

namespace H5.Core
{
    public  static partial class es5
    {
        [IgnoreCast]
        public interface DateConstructor : IObject
        {
            [Template("new {this}()")]
            es5.Date New();

            [Template("new {this}({0})")]
            es5.Date New(double value);

            [Template("new {this}({0})")]
            es5.Date New(string value);

            [Template("new {this}({0}, {1})")]
            es5.Date New(double year, double month);

            [Template("new {this}({0}, {1}, {2})")]
            es5.Date New(double year, double month, double date);

            [Template("new {this}({0}, {1}, {2}, {3})")]
            es5.Date New(double year, double month, double date, double hours);

            [Template("new {this}({0}, {1}, {2}, {3}, {4})")]
            es5.Date New(double year, double month, double date, double hours, double minutes);

            [Template("new {this}({0}, {1}, {2}, {3}, {4}, {5})")]
            es5.Date New(
              double year,
              double month,
              double date,
              double hours,
              double minutes,
              double seconds);

            [Template("new {this}({0}, {1}, {2}, {3}, {4}, {5}, {6})")]
            es5.Date New(
              double year,
              double month,
              double date,
              double hours,
              double minutes,
              double seconds,
              double ms);

            [Template("{this}()")]
            string Self();

            es5.Date prototype { get; }

            double parse(string s);

            double UTC(double year, double month);

            double UTC(double year, double month, double date);

            double UTC(double year, double month, double date, double hours);

            double UTC(double year, double month, double date, double hours, double minutes);

            double UTC(
              double year,
              double month,
              double date,
              double hours,
              double minutes,
              double seconds);

            double UTC(
              double year,
              double month,
              double date,
              double hours,
              double minutes,
              double seconds,
              double ms);

            double now();
        }
    }
}
