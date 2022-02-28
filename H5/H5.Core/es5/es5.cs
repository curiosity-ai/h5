// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using H5;
using H5.Core;

namespace H5.Core
{
    [Scope]
    [GlobalMethods]
    public  static partial class es5
    {
        public static readonly double NaN;
        public static readonly double Infinity;

        public static extern object eval(string x);

        public static extern double parseInt(string s);

        public static extern double parseInt(string s, double radix);

        public static extern double parseFloat(string @string);

        public static extern bool isNaN(double number);

        public static extern bool isFinite(double number);

        public static extern string decodeURI(string encodedURI);

        public static extern string decodeURIComponent(string encodedURIComponent);

        public static extern string encodeURI(string uri);

        public static extern string encodeURIComponent(string uriComponent);

        public static extern string escape(string @string);

        public static extern string unescape(string @string);

        public delegate void PropertyDecorator(H5.Core.Object target, Union<string, symbol> propertyKey);

        public delegate void ParameterDecorator(
          H5.Core.Object target,
          Union<string, symbol> propertyKey,
          double parameterIndex);

        public delegate es5.PromiseLike<T> PromiseConstructorLike<T>(
          es5.PromiseConstructorLikeFn<T> executor);

        [Generated]
        public delegate void PromiseConstructorLikeFn2<T>(Union<T, es5.PromiseLike<T>> value);

        [Generated]
        public delegate void PromiseConstructorLikeFn3<T>(object reason);

        [Generated]
        public delegate void PromiseConstructorLikeFn<T>(
          es5.PromiseConstructorLikeFn2<T> resolve,
          es5.PromiseConstructorLikeFn3<T> reject);
    }
}
