// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using System;
using System.Threading.Tasks;

namespace H5.Core
{
    public  static partial class es5
    {
        [IgnoreCast]
        [IgnoreGeneric(AllowInTypeScript = true)]
        [Virtual]
        [FormerInterface]
        public class Promise<T>
        {
            public extern es5.Promise<T> then();

            public extern es5.Promise<H5.Core.Void> then(Action<T> onfulfilled);

            public extern es5.Promise<TResult> then<TResult>(Func<T, TResult> onfulfilled);

            public extern es5.Promise<TResult> then<TResult>(Func<T, es5.Promise<TResult>> onfulfilled);

            public extern es5.Promise<TResult> then<TResult>(
              Func<T, es5.PromiseLike<TResult>> onfulfilled);

            public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
              Func<T, TResult1> onfulfilled,
              Func<object, TResult2> onrejected);

            public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
              Func<T, es5.Promise<TResult1>> onfulfilled,
              Func<object, es5.Promise<TResult2>> onrejected);

            public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
              Func<T, es5.PromiseLike<TResult1>> onfulfilled,
              Func<object, es5.PromiseLike<TResult2>> onrejected);

            [Name("catch")]
            public extern es5.Promise<T> @catch();

            [Name("catch")]
            public extern es5.Promise<Union<T, H5.Core.Void>> @catch(
              Action<object> onrejected);

            [Name("catch")]
            public extern es5.Promise<T> @catch(Func<object, T> onrejected);

            [Name("catch")]
            public extern es5.Promise<T> @catch(Func<object, es5.Promise<T>> onrejected);

            [Name("catch")]
            public extern es5.Promise<T> @catch(Func<object, es5.PromiseLike<T>> onrejected);

            [Name("catch")]
            public extern es5.Promise<Union<T, TResult>> @catch<TResult>(
              Func<object, TResult> onrejected);

            [Name("catch")]
            public extern es5.Promise<Union<T, TResult>> @catch<TResult>(
              Func<object, es5.Promise<TResult>> onrejected);

            [Name("catch")]
            public extern es5.Promise<Union<T, TResult>> @catch<TResult>(
              Func<object, es5.PromiseLike<TResult>> onrejected);

            [Template("System.Threading.Tasks.Task.fromPromise({this})")]
            public extern Task<T> ToTask();

            [Template("{0}")]
            public static extern implicit operator es5.PromiseLike<T>(es5.Promise<T> promise);
        }
    }
}
