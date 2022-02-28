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
        public interface ArrayConstructor : IObject
        {
            [Template("new {this}()")]
            object[] New();

            [Template("new {this}({0})")]
            object[] New(double arrayLength);

            [Template("new {this}({0})")]
            T[] New<T>(double arrayLength);

            [Template("new {this}({0})")]
            T[] New<T>(params T[] items);

            [Template("{this}()")]
            object[] Self();

            [Template("{this}({0})")]
            object[] Self(double arrayLength);

            [Template("{this}({0})")]
            T[] Self<T>(double arrayLength);

            [Template("{this}({0})")]
            T[] Self<T>(params T[] items);

            bool isArray(object arg);

            es5.Array<object> prototype { get; }
        }
    }
}
