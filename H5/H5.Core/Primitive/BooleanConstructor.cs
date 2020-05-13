// Decompiled with JetBrains decompiler
// Type: H5.Core.BooleanConstructor
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using H5;

namespace H5.Core
{
    [IgnoreCast]
    [Namespace(false)]
    public interface BooleanConstructor : IObject
    {
        [Template("new {this}()")]
        Boolean New();

        [Template("new {this}({0})")]
        Boolean New(object value);

        [Template("{this}()")]
        bool Self();

        [Template("{this}({0})")]
        bool Self(object value);

        Boolean prototype { get; }
    }
}
