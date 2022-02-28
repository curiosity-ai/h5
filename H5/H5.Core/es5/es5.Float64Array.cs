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
        [CombinedClass]
        [StaticInterface("Float64ArrayConstructor")]
        [FormerInterface]
        public class Float64Array : IList<double>, ICollection<double>, IEnumerable<double>, IEnumerable, IH5Class, IReadOnlyList<double>, IReadOnlyCollection<double>, ICollection, IObject
        {

            public extern Float64Array(uint length);

            public extern Float64Array(
              Union<es5.ArrayLike<double>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            public extern Float64Array(es5.ArrayLike<double> arrayOrArrayBuffer);

            public extern Float64Array(es5.ArrayBufferLike arrayOrArrayBuffer);

            public extern Float64Array(es5.ArrayBuffer arrayOrArrayBuffer);

            public extern Float64Array(es5.ArrayBufferLike buffer, uint byteOffset);

            public extern Float64Array(es5.ArrayBuffer buffer, uint byteOffset);

            public extern Float64Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

            public extern Float64Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public static es5.Float64Array prototype
            {
                get;
            }

            [Name("BYTES_PER_ELEMENT")]
            public static double BYTES_PER_ELEMENT_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Float64Array of(params double[] items);

            public static extern es5.Float64Array from(es5.ArrayLike<double> arrayLike);

            public static extern es5.Float64Array from(
              es5.ArrayLike<double> arrayLike,
              es5.Float64Array.fromFn mapfn);

            public static extern es5.Float64Array from(
              es5.ArrayLike<double> arrayLike,
              es5.Float64Array.fromFn mapfn,
              object thisArg);

            public virtual int BYTES_PER_ELEMENT
            {
                get;
            }

            public virtual es5.ArrayBufferLike buffer
            {
                get;
            }

            public virtual uint byteLength
            {
                get;
            }

            public virtual uint byteOffset
            {
                get;
            }

            public virtual extern es5.Float64Array copyWithin(long target, long start);

            public virtual extern es5.Float64Array copyWithin(long target, long start, long end);

            public virtual extern bool every(es5.Float64Array.everyFn callbackfn);

            public virtual extern bool every(es5.Float64Array.everyFn callbackfn, object thisArg);

            public virtual extern es5.Float64Array fill(double value);

            public virtual extern es5.Float64Array fill(double value, uint start);

            public virtual extern es5.Float64Array fill(double value, uint start, uint end);

            public virtual extern es5.Float64Array filter(es5.Float64Array.filterFn callbackfn);

            public virtual extern es5.Float64Array filter(
              es5.Float64Array.filterFn callbackfn,
              object thisArg);

            public virtual extern Union<double, Undefined> find(
              es5.Float64Array.findFn predicate);

            public virtual extern Union<double, Undefined> find(
              es5.Float64Array.findFn predicate,
              object thisArg);

            public virtual extern uint findIndex(es5.Float64Array.findIndexFn predicate);

            public virtual extern uint findIndex(es5.Float64Array.findIndexFn predicate, object thisArg);

            public virtual extern void forEach(es5.Float64Array.forEachFn callbackfn);

            public virtual extern void forEach(es5.Float64Array.forEachFn callbackfn, object thisArg);

            public virtual extern uint indexOf(double searchElement);

            public virtual extern uint indexOf(double searchElement, uint fromIndex);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern uint lastIndexOf(double searchElement);

            public virtual extern uint lastIndexOf(double searchElement, uint fromIndex);

            public virtual uint length
            {
                get;
            }

            public virtual extern es5.Float64Array map(es5.Float64Array.mapFn callbackfn);

            public virtual extern es5.Float64Array map(
              es5.Float64Array.mapFn callbackfn,
              object thisArg);

            public virtual extern long reduce(es5.Float64Array.reduceFn callbackfn);

            public virtual extern long reduce(es5.Float64Array.reduceFn callbackfn, long initialValue);

            public virtual extern U reduce<U>(es5.Float64Array.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern long reduceRight(es5.Float64Array.reduceRightFn callbackfn);

            public virtual extern long reduceRight(
              es5.Float64Array.reduceRightFn callbackfn,
              long initialValue);

            public virtual extern U reduceRight<U>(
              es5.Float64Array.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern es5.Float64Array reverse();

            public virtual extern void set(es5.ArrayLike<double> array);

            public virtual extern void set(es5.ArrayLike<double> array, uint offset);

            public virtual extern es5.Float64Array slice();

            public virtual extern es5.Float64Array slice(uint start);

            public virtual extern es5.Float64Array slice(uint start, uint end);

            public virtual extern bool some(es5.Float64Array.someFn callbackfn);

            public virtual extern bool some(es5.Float64Array.someFn callbackfn, object thisArg);

            public virtual extern es5.Float64Array sort();

            public virtual extern es5.Float64Array sort(es5.Float64Array.sortFn compareFn);

            public virtual extern es5.Float64Array subarray(uint begin);

            public virtual extern es5.Float64Array subarray(uint begin, int end);

            public virtual extern string toLocaleString();

            public virtual extern string toString();

            public virtual extern double this[uint index] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<double>.IndexOf(double item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<double>.Insert(int index, double item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<double>.RemoveAt(int index);

            extern double IList<double>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<double>.Add(double item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<double>.CopyTo(double[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<double>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<double>.Contains(double item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<double>.Remove(double item);

            int ICollection<double>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<double>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<double> IEnumerable<double>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern double IReadOnlyList<double>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<double>.Count
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
            public delegate bool everyFn(double value, uint index, es5.Float64Array array);

            [Generated]
            public delegate object filterFn(double value, uint index, es5.Float64Array array);

            [Generated]
            public delegate bool findFn(double value, uint index, es5.Float64Array obj);

            [Generated]
            public delegate bool findIndexFn(double value, uint index, es5.Float64Array obj);

            [Generated]
            public delegate void forEachFn(double value, uint index, es5.Float64Array array);

            [Generated]
            public delegate double mapFn(double value, uint index, es5.Float64Array array);

            [Generated]
            public delegate double reduceFn(
              long previousValue,
              double currentValue,
              uint currentIndex,
              es5.Float64Array array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              double currentValue,
              uint currentIndex,
              es5.Float64Array array);

            [Generated]
            public delegate double reduceRightFn(
              long previousValue,
              double currentValue,
              uint currentIndex,
              es5.Float64Array array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              double currentValue,
              uint currentIndex,
              es5.Float64Array array);

            [Generated]
            public delegate bool someFn(double value, uint index, es5.Float64Array array);

            [Generated]
            public delegate double sortFn(double a, double b);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
