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
        [StaticInterface("Int16ArrayConstructor")]
        [FormerInterface]
        public class Int16Array : IList<short>, ICollection<short>, IEnumerable<short>, IEnumerable, IH5Class, IReadOnlyList<short>, IReadOnlyCollection<short>, ICollection, IObject
        {
            public extern Int16Array(uint length);

            public extern Int16Array(
              Union<es5.ArrayLike<short>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            public extern Int16Array(es5.ArrayLike<short> arrayOrArrayBuffer);

            public extern Int16Array(es5.ArrayBufferLike arrayOrArrayBuffer);

            public extern Int16Array(es5.ArrayBuffer arrayOrArrayBuffer);

            public extern Int16Array(es5.ArrayBufferLike buffer, uint byteOffset);

            public extern Int16Array(es5.ArrayBuffer buffer, uint byteOffset);

            public extern Int16Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

            public extern Int16Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public static es5.Int16Array prototype
            {
                get;
            }

            [Name("BYTES_PER_ELEMENT")]
            public static double BYTES_PER_ELEMENT_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Int16Array of(params short[] items);

            public static extern es5.Int16Array from(es5.ArrayLike<short> arrayLike);

            public static extern es5.Int16Array from(
              es5.ArrayLike<short> arrayLike,
              es5.Int16Array.fromFn mapfn);

            public static extern es5.Int16Array from(
              es5.ArrayLike<short> arrayLike,
              es5.Int16Array.fromFn mapfn,
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

            public virtual extern es5.Int16Array copyWithin(long target, long start);

            public virtual extern es5.Int16Array copyWithin(long target, long start, long end);

            public virtual extern bool every(es5.Int16Array.everyFn callbackfn);

            public virtual extern bool every(es5.Int16Array.everyFn callbackfn, object thisArg);

            public virtual extern es5.Int16Array fill(short value);

            public virtual extern es5.Int16Array fill(short value, uint start);

            public virtual extern es5.Int16Array fill(short value, uint start, uint end);

            public virtual extern es5.Int16Array filter(es5.Int16Array.filterFn callbackfn);

            public virtual extern es5.Int16Array filter(
              es5.Int16Array.filterFn callbackfn,
              object thisArg);

            public virtual extern Union<short, Undefined> find(
              es5.Int16Array.findFn predicate);

            public virtual extern Union<short, Undefined> find(
              es5.Int16Array.findFn predicate,
              object thisArg);

            public virtual extern uint findIndex(es5.Int16Array.findIndexFn predicate);

            public virtual extern uint findIndex(es5.Int16Array.findIndexFn predicate, object thisArg);

            public virtual extern void forEach(es5.Int16Array.forEachFn callbackfn);

            public virtual extern void forEach(es5.Int16Array.forEachFn callbackfn, object thisArg);

            public virtual extern uint indexOf(short searchElement);

            public virtual extern uint indexOf(short searchElement, uint fromIndex);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern uint lastIndexOf(short searchElement);

            public virtual extern uint lastIndexOf(short searchElement, uint fromIndex);

            public virtual uint length
            {
                get;
            }

            public virtual extern es5.Int16Array map(es5.Int16Array.mapFn callbackfn);

            public virtual extern es5.Int16Array map(es5.Int16Array.mapFn callbackfn, object thisArg);

            public virtual extern long reduce(es5.Int16Array.reduceFn callbackfn);

            public virtual extern long reduce(es5.Int16Array.reduceFn callbackfn, long initialValue);

            public virtual extern U reduce<U>(es5.Int16Array.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern long reduceRight(es5.Int16Array.reduceRightFn callbackfn);

            public virtual extern long reduceRight(
              es5.Int16Array.reduceRightFn callbackfn,
              long initialValue);

            public virtual extern U reduceRight<U>(
              es5.Int16Array.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern es5.Int16Array reverse();

            public virtual extern void set(es5.ArrayLike<short> array);

            public virtual extern void set(es5.ArrayLike<short> array, uint offset);

            public virtual extern es5.Int16Array slice();

            public virtual extern es5.Int16Array slice(uint start);

            public virtual extern es5.Int16Array slice(uint start, uint end);

            public virtual extern bool some(es5.Int16Array.someFn callbackfn);

            public virtual extern bool some(es5.Int16Array.someFn callbackfn, object thisArg);

            public virtual extern es5.Int16Array sort();

            public virtual extern es5.Int16Array sort(es5.Int16Array.sortFn compareFn);

            public virtual extern es5.Int16Array subarray(uint begin);

            public virtual extern es5.Int16Array subarray(uint begin, int end);

            public virtual extern string toLocaleString();

            public virtual extern string toString();

            public virtual extern short this[uint index] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<short>.IndexOf(short item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<short>.Insert(int index, short item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<short>.RemoveAt(int index);

            extern short IList<short>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<short>.Add(short item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<short>.CopyTo(short[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<short>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<short>.Contains(short item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<short>.Remove(short item);

            int ICollection<short>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<short>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<short> IEnumerable<short>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern short IReadOnlyList<short>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<short>.Count
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
            public delegate bool everyFn(short value, uint index, es5.Int16Array array);

            [Generated]
            public delegate object filterFn(short value, uint index, es5.Int16Array array);

            [Generated]
            public delegate bool findFn(short value, uint index, es5.Int16Array obj);

            [Generated]
            public delegate bool findIndexFn(short value, uint index, es5.Int16Array obj);

            [Generated]
            public delegate void forEachFn(short value, uint index, es5.Int16Array array);

            [Generated]
            public delegate double mapFn(short value, uint index, es5.Int16Array array);

            [Generated]
            public delegate double reduceFn(
              long previousValue,
              short currentValue,
              uint currentIndex,
              es5.Int16Array array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              short currentValue,
              uint currentIndex,
              es5.Int16Array array);

            [Generated]
            public delegate double reduceRightFn(
              long previousValue,
              short currentValue,
              uint currentIndex,
              es5.Int16Array array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              short currentValue,
              uint currentIndex,
              es5.Int16Array array);

            [Generated]
            public delegate bool someFn(short value, uint index, es5.Int16Array array);

            [Generated]
            public delegate double sortFn(short a, short b);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
