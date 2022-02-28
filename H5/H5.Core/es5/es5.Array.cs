// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace H5.Core
{
    public  static partial class es5
    {
        [IgnoreGeneric(AllowInTypeScript = true)]
        [CombinedClass]
        [StaticInterface("ArrayConstructor")]
        [FormerInterface]
        public class Array<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IH5Class, IReadOnlyList<T>, IReadOnlyCollection<T>, ICollection, IObject
        {
            public extern Array();

            public extern Array(double arrayLength);

            [ExpandParams]
            public extern Array(params T[] items);

            public static es5.Array<object> prototype
            {
                get;
            }

            public static extern object[] Self();

            public static extern object[] Self(double arrayLength);

            public static extern T[] Self<T>(double arrayLength);

            [ExpandParams]
            public static extern T[] Self<T>(params T[] items);

            public static extern bool isArray(object arg);

            public virtual double length
            {
                get; set;
            }

            public virtual extern string toString();

            public virtual extern string toLocaleString();

            [ExpandParams]
            public virtual extern double push(params T[] items);

            public virtual extern Union<T, Undefined> pop();

            [ExpandParams]
            public virtual extern T[] concat(params es5.ConcatArray<T>[] items);

            [ExpandParams]
            public virtual extern T[] concat(params Union<T, es5.ConcatArray<T>>[] items);

            [ExpandParams]
            public virtual extern T[] concat(params T[] items);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern T[] reverse();

            public virtual extern Union<T, Undefined> shift();

            public virtual extern T[] slice();

            public virtual extern T[] slice(double start);

            public virtual extern T[] slice(double start, double end);

            public virtual extern es5.Array<T> sort();

            public virtual extern es5.Array<T> sort(es5.Array<T>.sortFn compareFn);

            public virtual extern T[] splice(double start);

            public virtual extern T[] splice(double start, double deleteCount);

            [ExpandParams]
            public virtual extern T[] splice(double start, double deleteCount, params T[] items);

            [ExpandParams]
            public virtual extern double unshift(params T[] items);

            public virtual extern double indexOf(T searchElement);

            public virtual extern double indexOf(T searchElement, double fromIndex);

            public virtual extern double lastIndexOf(T searchElement);

            public virtual extern double lastIndexOf(T searchElement, double fromIndex);

            public virtual extern bool every(es5.Array<T>.everyFn callbackfn);

            public virtual extern bool every(es5.Array<T>.everyFn callbackfn, object thisArg);

            public virtual extern bool some(es5.Array<T>.someFn callbackfn);

            public virtual extern bool some(es5.Array<T>.someFn callbackfn, object thisArg);

            public virtual extern void forEach(es5.Array<T>.forEachFn callbackfn);

            public virtual extern void forEach(es5.Array<T>.forEachFn callbackfn, object thisArg);

            public virtual extern U[] map<U>(es5.Array<T>.mapFn<U> callbackfn);

            public virtual extern U[] map<U>(es5.Array<T>.mapFn<U> callbackfn, object thisArg);

            [Where("S", new string[] { "T" }, EnableImplicitConversion = true)]
            public virtual extern S[] filter<S>(es5.Array<T>.filterFn<S> callbackfn);

            [Where("S", new string[] { "T" }, EnableImplicitConversion = true)]
            public virtual extern S[] filter<S>(es5.Array<T>.filterFn<S> callbackfn, object thisArg);

            public virtual extern T[] filter(es5.Array<T>.filterFn2 callbackfn);

            public virtual extern T[] filter(es5.Array<T>.filterFn2 callbackfn, object thisArg);

            public virtual extern T reduce(es5.Array<T>.reduceFn callbackfn);

            public virtual extern T reduce(es5.Array<T>.reduceFn callbackfn, T initialValue);

            public virtual extern U reduce<U>(es5.Array<T>.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern T reduceRight(es5.Array<T>.reduceRightFn callbackfn);

            public virtual extern T reduceRight(es5.Array<T>.reduceRightFn callbackfn, T initialValue);

            public virtual extern U reduceRight<U>(
              es5.Array<T>.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern T this[double n] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<T>.IndexOf(T item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<T>.Insert(int index, T item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<T>.RemoveAt(int index);

            extern T IList<T>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<T>.Add(T item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<T>.CopyTo(T[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<T>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<T>.Contains(T item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<T>.Remove(T item);

            int ICollection<T>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<T>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<T> IEnumerable<T>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern T IReadOnlyList<T>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<T>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
            extern void ICollection.CopyTo(Array array, int arrayIndex);

            int ICollection.Count
            {
                [Template("System.Array.getCount({this})")]
                get;
            }

            object ICollection.SyncRoot
            {
                get;
            }

            bool ICollection.IsSynchronized
            {
                get;
            }

            [Generated]
            public delegate double sortFn(T a, T b);

            [Generated]
            public delegate bool everyFn(T value, double index, T[] array);

            [Generated]
            public delegate bool someFn(T value, double index, T[] array);

            [Generated]
            public delegate void forEachFn(T value, double index, T[] array);

            [Generated]
            public delegate U mapFn<U>(T value, double index, T[] array);

            [Generated]
            [Where("S", new string[] { "T" }, EnableImplicitConversion = true)]
            public delegate bool filterFn<S>(T value, double index, T[] array);

            [Generated]
            public delegate object filterFn2(T value, double index, T[] array);

            [Generated]
            public delegate T reduceFn(T previousValue, T currentValue, double currentIndex, T[] array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              T currentValue,
              double currentIndex,
              T[] array);

            [Generated]
            public delegate T reduceRightFn(
              T previousValue,
              T currentValue,
              double currentIndex,
              T[] array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              T currentValue,
              double currentIndex,
              T[] array);
        }
    }
}
