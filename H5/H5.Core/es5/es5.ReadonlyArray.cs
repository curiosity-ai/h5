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
        [IgnoreGeneric(AllowInTypeScript = true)]
        [Virtual]
        [FormerInterface]
        public abstract class ReadonlyArray<T> : IObject
        {
            public abstract double length { get; }

            public abstract string toString();

            public abstract string toLocaleString();

            [ExpandParams]
            public abstract T[] concat(params es5.ConcatArray<T>[] items);

            [ExpandParams]
            public abstract T[] concat(params Union<T, es5.ConcatArray<T>>[] items);

            [ExpandParams]
            public abstract T[] concat(params T[] items);

            public abstract string join();

            public abstract string join(string separator);

            public abstract T[] slice();

            public abstract T[] slice(double start);

            public abstract T[] slice(double start, double end);

            public abstract double indexOf(T searchElement);

            public abstract double indexOf(T searchElement, double fromIndex);

            public abstract double lastIndexOf(T searchElement);

            public abstract double lastIndexOf(T searchElement, double fromIndex);

            public abstract bool every(es5.ReadonlyArray<T>.everyFn callbackfn);

            public abstract bool every(es5.ReadonlyArray<T>.everyFn callbackfn, object thisArg);

            public abstract bool some(es5.ReadonlyArray<T>.someFn callbackfn);

            public abstract bool some(es5.ReadonlyArray<T>.someFn callbackfn, object thisArg);

            public abstract void forEach(es5.ReadonlyArray<T>.forEachFn callbackfn);

            public abstract void forEach(es5.ReadonlyArray<T>.forEachFn callbackfn, object thisArg);

            public abstract U[] map<U>(es5.ReadonlyArray<T>.mapFn<U> callbackfn);

            public abstract U[] map<U>(es5.ReadonlyArray<T>.mapFn<U> callbackfn, object thisArg);

            [Where("S", new string[] { "T" }, EnableImplicitConversion = true)]
            public abstract S[] filter<S>(es5.ReadonlyArray<T>.filterFn<S> callbackfn);

            [Where("S", new string[] { "T" }, EnableImplicitConversion = true)]
            public abstract S[] filter<S>(es5.ReadonlyArray<T>.filterFn<S> callbackfn, object thisArg);

            public abstract T[] filter(es5.ReadonlyArray<T>.filterFn2 callbackfn);

            public abstract T[] filter(es5.ReadonlyArray<T>.filterFn2 callbackfn, object thisArg);

            public abstract T reduce(es5.ReadonlyArray<T>.reduceFn callbackfn);

            public abstract T reduce(es5.ReadonlyArray<T>.reduceFn callbackfn, T initialValue);

            public abstract U reduce<U>(es5.ReadonlyArray<T>.reduceFn2<U> callbackfn, U initialValue);

            public abstract T reduceRight(es5.ReadonlyArray<T>.reduceRightFn callbackfn);

            public abstract T reduceRight(es5.ReadonlyArray<T>.reduceRightFn callbackfn, T initialValue);

            public abstract U reduceRight<U>(
              es5.ReadonlyArray<T>.reduceRightFn2<U> callbackfn,
              U initialValue);

            public abstract T this[double n] { get; }

            [Generated]
            public delegate bool everyFn(T value, double index, es5.ReadonlyArray<T> array);

            [Generated]
            public delegate bool someFn(T value, double index, es5.ReadonlyArray<T> array);

            [Generated]
            public delegate void forEachFn(T value, double index, es5.ReadonlyArray<T> array);

            [Generated]
            public delegate U mapFn<U>(T value, double index, es5.ReadonlyArray<T> array);

            [Where("S", new string[] { "T" }, EnableImplicitConversion = true)]
            [Generated]
            public delegate bool filterFn<S>(T value, double index, es5.ReadonlyArray<T> array);

            [Generated]
            public delegate object filterFn2(T value, double index, es5.ReadonlyArray<T> array);

            [Generated]
            public delegate T reduceFn(
              T previousValue,
              T currentValue,
              double currentIndex,
              es5.ReadonlyArray<T> array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              T currentValue,
              double currentIndex,
              es5.ReadonlyArray<T> array);

            [Generated]
            public delegate T reduceRightFn(
              T previousValue,
              T currentValue,
              double currentIndex,
              es5.ReadonlyArray<T> array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              T currentValue,
              double currentIndex,
              es5.ReadonlyArray<T> array);
        }
    }
}
