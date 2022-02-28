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
        [StaticInterface("Int32ArrayConstructor")]
        [FormerInterface]
        public class Int32Array : IList<int>, ICollection<int>, IEnumerable<int>, IEnumerable, IH5Class, IReadOnlyList<int>, IReadOnlyCollection<int>, ICollection, IObject
        {

            public extern Int32Array(uint length);

            public extern Int32Array(
              Union<es5.ArrayLike<int>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            public extern Int32Array(es5.ArrayLike<int> arrayOrArrayBuffer);

            public extern Int32Array(es5.ArrayBufferLike arrayOrArrayBuffer);

            public extern Int32Array(es5.ArrayBuffer arrayOrArrayBuffer);

            public extern Int32Array(es5.ArrayBufferLike buffer, uint byteOffset);

            public extern Int32Array(es5.ArrayBuffer buffer, uint byteOffset);

            public extern Int32Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

            public extern Int32Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public static es5.Int32Array prototype
            {
                get;
            }

            [Name("BYTES_PER_ELEMENT")]
            public static double BYTES_PER_ELEMENT_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Int32Array of(params int[] items);

            public static extern es5.Int32Array from(es5.ArrayLike<int> arrayLike);

            public static extern es5.Int32Array from(
              es5.ArrayLike<int> arrayLike,
              es5.Int32Array.fromFn mapfn);

            public static extern es5.Int32Array from(
              es5.ArrayLike<int> arrayLike,
              es5.Int32Array.fromFn mapfn,
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

            public virtual extern es5.Int32Array copyWithin(long target, long start);

            public virtual extern es5.Int32Array copyWithin(long target, long start, long end);

            public virtual extern bool every(es5.Int32Array.everyFn callbackfn);

            public virtual extern bool every(es5.Int32Array.everyFn callbackfn, object thisArg);

            public virtual extern es5.Int32Array fill(int value);

            public virtual extern es5.Int32Array fill(int value, uint start);

            public virtual extern es5.Int32Array fill(int value, uint start, uint end);

            public virtual extern es5.Int32Array filter(es5.Int32Array.filterFn callbackfn);

            public virtual extern es5.Int32Array filter(
              es5.Int32Array.filterFn callbackfn,
              object thisArg);

            public virtual extern Union<int, Undefined> find(es5.Int32Array.findFn predicate);

            public virtual extern Union<int, Undefined> find(
              es5.Int32Array.findFn predicate,
              object thisArg);

            public virtual extern uint findIndex(es5.Int32Array.findIndexFn predicate);

            public virtual extern uint findIndex(es5.Int32Array.findIndexFn predicate, object thisArg);

            public virtual extern void forEach(es5.Int32Array.forEachFn callbackfn);

            public virtual extern void forEach(es5.Int32Array.forEachFn callbackfn, object thisArg);

            public virtual extern uint indexOf(int searchElement);

            public virtual extern uint indexOf(int searchElement, uint fromIndex);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern uint lastIndexOf(int searchElement);

            public virtual extern uint lastIndexOf(int searchElement, uint fromIndex);

            public virtual uint length
            {
                get;
            }

            public virtual extern es5.Int32Array map(es5.Int32Array.mapFn callbackfn);

            public virtual extern es5.Int32Array map(es5.Int32Array.mapFn callbackfn, object thisArg);

            public virtual extern long reduce(es5.Int32Array.reduceFn callbackfn);

            public virtual extern long reduce(es5.Int32Array.reduceFn callbackfn, long initialValue);

            public virtual extern U reduce<U>(es5.Int32Array.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern long reduceRight(es5.Int32Array.reduceRightFn callbackfn);

            public virtual extern long reduceRight(
              es5.Int32Array.reduceRightFn callbackfn,
              long initialValue);

            public virtual extern U reduceRight<U>(
              es5.Int32Array.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern es5.Int32Array reverse();

            public virtual extern void set(es5.ArrayLike<int> array);

            public virtual extern void set(es5.ArrayLike<int> array, uint offset);

            public virtual extern es5.Int32Array slice();

            public virtual extern es5.Int32Array slice(uint start);

            public virtual extern es5.Int32Array slice(uint start, uint end);

            public virtual extern bool some(es5.Int32Array.someFn callbackfn);

            public virtual extern bool some(es5.Int32Array.someFn callbackfn, object thisArg);

            public virtual extern es5.Int32Array sort();

            public virtual extern es5.Int32Array sort(es5.Int32Array.sortFn compareFn);

            public virtual extern es5.Int32Array subarray(uint begin);

            public virtual extern es5.Int32Array subarray(uint begin, int end);

            public virtual extern string toLocaleString();

            public virtual extern string toString();

            public virtual extern int this[uint index] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<int>.IndexOf(int item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<int>.Insert(int index, int item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<int>.RemoveAt(int index);

            extern int IList<int>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<int>.Add(int item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<int>.CopyTo(int[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<int>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<int>.Contains(int item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<int>.Remove(int item);

            int ICollection<int>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<int>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<int> IEnumerable<int>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern int IReadOnlyList<int>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<int>.Count
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
            public delegate bool everyFn(int value, uint index, es5.Int32Array array);

            [Generated]
            public delegate object filterFn(int value, uint index, es5.Int32Array array);

            [Generated]
            public delegate bool findFn(int value, uint index, es5.Int32Array obj);

            [Generated]
            public delegate bool findIndexFn(int value, uint index, es5.Int32Array obj);

            [Generated]
            public delegate void forEachFn(int value, uint index, es5.Int32Array array);

            [Generated]
            public delegate double mapFn(int value, uint index, es5.Int32Array array);

            [Generated]
            public delegate double reduceFn(
              long previousValue,
              int currentValue,
              uint currentIndex,
              es5.Int32Array array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              int currentValue,
              uint currentIndex,
              es5.Int32Array array);

            [Generated]
            public delegate double reduceRightFn(
              long previousValue,
              int currentValue,
              uint currentIndex,
              es5.Int32Array array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              int currentValue,
              uint currentIndex,
              es5.Int32Array array);

            [Generated]
            public delegate bool someFn(int value, uint index, es5.Int32Array array);

            [Generated]
            public delegate double sortFn(int a, int b);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
