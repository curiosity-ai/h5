// Decompiled with JetBrains decompiler
// Type: H5.Core.NumberConstructor
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using H5;

namespace H5.Core
{
    [IgnoreCast]
    [Namespace(false)]
    [Virtual]
    public interface NumberConstructor : IObject
    {
        [Template("new {this}()")]
        Number New();

        [Template("new {this}({0})")]
        Number New(object value);

        [Template("{this}()")]
        double Self();

        [Template("{this}({0})")]
        double Self(object value);

        Number prototype { get; }

        double MAX_VALUE { get; }

        double MIN_VALUE { get; }

        double NaN { get; }

        double NEGATIVE_INFINITY { get; }

        double POSITIVE_INFINITY { get; }
    }
}
