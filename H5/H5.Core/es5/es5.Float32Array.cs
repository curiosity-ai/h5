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
        [StaticInterface("Float32ArrayConstructor")]
        [FormerInterface]
        public class Float32Array : IList<float>, ICollection<float>, IEnumerable<float>, IEnumerable, IH5Class, IReadOnlyList<float>, IReadOnlyCollection<float>, ICollection, IObject
        {
            public extern Float32Array(uint length);

            public extern Float32Array(
              Union<es5.ArrayLike<float>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            public extern Float32Array(es5.ArrayLike<float> arrayOrArrayBuffer);

            public extern Float32Array(es5.ArrayBufferLike arrayOrArrayBuffer);

            public extern Float32Array(es5.ArrayBuffer arrayOrArrayBuffer);

            public extern Float32Array(es5.ArrayBufferLike buffer, uint byteOffset);

            public extern Float32Array(es5.ArrayBuffer buffer, uint byteOffset);

            public extern Float32Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

            public extern Float32Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public static es5.Float32Array prototype
            {
                get;
            }

            [Name("BYTES_PER_ELEMENT")]
            public static double BYTES_PER_ELEMENT_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Float32Array of(params float[] items);

            public static extern es5.Float32Array from(es5.ArrayLike<float> arrayLike);

            public static extern es5.Float32Array from(
              es5.ArrayLike<float> arrayLike,
              es5.Float32Array.fromFn mapfn);

            public static extern es5.Float32Array from(
              es5.ArrayLike<float> arrayLike,
              es5.Float32Array.fromFn mapfn,
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

            public virtual extern es5.Float32Array copyWithin(long target, long start);

            public virtual extern es5.Float32Array copyWithin(long target, long start, long end);

            public virtual extern bool every(es5.Float32Array.everyFn callbackfn);

            public virtual extern bool every(es5.Float32Array.everyFn callbackfn, object thisArg);

            public virtual extern es5.Float32Array fill(float value);

            public virtual extern es5.Float32Array fill(float value, uint start);

            public virtual extern es5.Float32Array fill(float value, uint start, uint end);

            public virtual extern es5.Float32Array filter(es5.Float32Array.filterFn callbackfn);

            public virtual extern es5.Float32Array filter(
              es5.Float32Array.filterFn callbackfn,
              object thisArg);

            public virtual extern Union<float, Undefined> find(
              es5.Float32Array.findFn predicate);

            public virtual extern Union<float, Undefined> find(
              es5.Float32Array.findFn predicate,
              object thisArg);

            public virtual extern uint findIndex(es5.Float32Array.findIndexFn predicate);

            public virtual extern uint findIndex(es5.Float32Array.findIndexFn predicate, object thisArg);

            public virtual extern void forEach(es5.Float32Array.forEachFn callbackfn);

            public virtual extern void forEach(es5.Float32Array.forEachFn callbackfn, object thisArg);

            public virtual extern uint indexOf(float searchElement);

            public virtual extern uint indexOf(float searchElement, uint fromIndex);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern uint lastIndexOf(float searchElement);

            public virtual extern uint lastIndexOf(float searchElement, uint fromIndex);

            public virtual uint length
            {
                get;
            }

            public virtual extern es5.Float32Array map(es5.Float32Array.mapFn callbackfn);

            public virtual extern es5.Float32Array map(
              es5.Float32Array.mapFn callbackfn,
              object thisArg);

            public virtual extern long reduce(es5.Float32Array.reduceFn callbackfn);

            public virtual extern long reduce(es5.Float32Array.reduceFn callbackfn, long initialValue);

            public virtual extern U reduce<U>(es5.Float32Array.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern long reduceRight(es5.Float32Array.reduceRightFn callbackfn);

            public virtual extern long reduceRight(
              es5.Float32Array.reduceRightFn callbackfn,
              long initialValue);

            public virtual extern U reduceRight<U>(
              es5.Float32Array.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern es5.Float32Array reverse();

            public virtual extern void set(es5.ArrayLike<float> array);

            public virtual extern void set(es5.ArrayLike<float> array, uint offset);

            public virtual extern es5.Float32Array slice();

            public virtual extern es5.Float32Array slice(uint start);

            public virtual extern es5.Float32Array slice(uint start, uint end);

            public virtual extern bool some(es5.Float32Array.someFn callbackfn);

            public virtual extern bool some(es5.Float32Array.someFn callbackfn, object thisArg);

            public virtual extern es5.Float32Array sort();

            public virtual extern es5.Float32Array sort(es5.Float32Array.sortFn compareFn);

            public virtual extern es5.Float32Array subarray(uint begin);

            public virtual extern es5.Float32Array subarray(uint begin, int end);

            public virtual extern string toLocaleString();

            public virtual extern string toString();

            public virtual extern float this[uint index] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<float>.IndexOf(float item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<float>.Insert(int index, float item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<float>.RemoveAt(int index);

            extern float IList<float>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<float>.Add(float item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<float>.CopyTo(float[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<float>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<float>.Contains(float item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<float>.Remove(float item);

            int ICollection<float>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<float>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<float> IEnumerable<float>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern float IReadOnlyList<float>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<float>.Count
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
            public delegate bool everyFn(float value, uint index, es5.Float32Array array);

            [Generated]
            public delegate object filterFn(float value, uint index, es5.Float32Array array);

            [Generated]
            public delegate bool findFn(float value, uint index, es5.Float32Array obj);

            [Generated]
            public delegate bool findIndexFn(float value, uint index, es5.Float32Array obj);

            [Generated]
            public delegate void forEachFn(float value, uint index, es5.Float32Array array);

            [Generated]
            public delegate double mapFn(float value, uint index, es5.Float32Array array);

            [Generated]
            public delegate double reduceFn(
              long previousValue,
              float currentValue,
              uint currentIndex,
              es5.Float32Array array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              float currentValue,
              uint currentIndex,
              es5.Float32Array array);

            [Generated]
            public delegate double reduceRightFn(
              long previousValue,
              float currentValue,
              uint currentIndex,
              es5.Float32Array array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              float currentValue,
              uint currentIndex,
              es5.Float32Array array);

            [Generated]
            public delegate bool someFn(float value, uint index, es5.Float32Array array);

            [Generated]
            public delegate double sortFn(float a, float b);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
