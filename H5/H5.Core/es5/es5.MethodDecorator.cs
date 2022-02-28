// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

namespace H5.Core
{
    public  static partial class es5
    {
        public abstract class MethodDecorator : IObject
        {
            [Template("{this}({0}, {1}, {2})")]
            public extern Union<es5.TypedPropertyDescriptor<T>, H5.Core.Void> Invoke<T>(
              H5.Core.Object target,
              Union<string, symbol> propertyKey,
              es5.TypedPropertyDescriptor<T> descriptor);

            [Template("{this}({0}, {1}, {2})")]
            public extern Union<es5.TypedPropertyDescriptor<T>, H5.Core.Void> Invoke<T>(
              H5.Core.Object target,
              string propertyKey,
              es5.TypedPropertyDescriptor<T> descriptor);

            [Template("{this}({0}, {1}, {2})")]
            public extern Union<es5.TypedPropertyDescriptor<T>, H5.Core.Void> Invoke<T>(
              H5.Core.Object target,
              symbol propertyKey,
              es5.TypedPropertyDescriptor<T> descriptor);
        }
    }
}
