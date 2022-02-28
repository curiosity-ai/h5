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
        [StaticInterface("Int8ArrayConstructor")]
        [FormerInterface]
        public class Int8Array : IList<sbyte>, ICollection<sbyte>, IEnumerable<sbyte>, IEnumerable, IH5Class, IReadOnlyList<sbyte>, IReadOnlyCollection<sbyte>, ICollection, IObject
        {
            public extern Int8Array(uint length);

            public extern Int8Array(
              Union<es5.ArrayLike<sbyte>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            public extern Int8Array(es5.ArrayLike<sbyte> arrayOrArrayBuffer);

            public extern Int8Array(es5.ArrayBufferLike arrayOrArrayBuffer);

            public extern Int8Array(es5.ArrayBuffer arrayOrArrayBuffer);

            public extern Int8Array(es5.ArrayBufferLike buffer, uint byteOffset);

            public extern Int8Array(es5.ArrayBuffer buffer, uint byteOffset);

            public extern Int8Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

            public extern Int8Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public static es5.Int8Array prototype
            {
                get;
            }

            [Name("BYTES_PER_ELEMENT")]
            public static double BYTES_PER_ELEMENT_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Int8Array of(params sbyte[] items);

            public static extern es5.Int8Array from(es5.ArrayLike<sbyte> arrayLike);

            public static extern es5.Int8Array from(
              es5.ArrayLike<sbyte> arrayLike,
              es5.Int8Array.fromFn mapfn);

            public static extern es5.Int8Array from(
              es5.ArrayLike<sbyte> arrayLike,
              es5.Int8Array.fromFn mapfn,
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

            public virtual extern es5.Int8Array copyWithin(long target, long start);

            public virtual extern es5.Int8Array copyWithin(long target, long start, long end);

            public virtual extern bool every(es5.Int8Array.everyFn callbackfn);

            public virtual extern bool every(es5.Int8Array.everyFn callbackfn, object thisArg);

            public virtual extern es5.Int8Array fill(sbyte value);

            public virtual extern es5.Int8Array fill(sbyte value, uint start);

            public virtual extern es5.Int8Array fill(sbyte value, uint start, uint end);

            public virtual extern es5.Int8Array filter(es5.Int8Array.filterFn callbackfn);

            public virtual extern es5.Int8Array filter(
              es5.Int8Array.filterFn callbackfn,
              object thisArg);

            public virtual extern Union<sbyte, Undefined> find(
              es5.Int8Array.findFn predicate);

            public virtual extern Union<sbyte, Undefined> find(
              es5.Int8Array.findFn predicate,
              object thisArg);

            public virtual extern uint findIndex(es5.Int8Array.findIndexFn predicate);

            public virtual extern uint findIndex(es5.Int8Array.findIndexFn predicate, object thisArg);

            public virtual extern void forEach(es5.Int8Array.forEachFn callbackfn);

            public virtual extern void forEach(es5.Int8Array.forEachFn callbackfn, object thisArg);

            public virtual extern uint indexOf(sbyte searchElement);

            public virtual extern uint indexOf(sbyte searchElement, uint fromIndex);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern uint lastIndexOf(sbyte searchElement);

            public virtual extern uint lastIndexOf(sbyte searchElement, uint fromIndex);

            public virtual uint length
            {
                get;
            }

            public virtual extern es5.Int8Array map(es5.Int8Array.mapFn callbackfn);

            public virtual extern es5.Int8Array map(es5.Int8Array.mapFn callbackfn, object thisArg);

            public virtual extern long reduce(es5.Int8Array.reduceFn callbackfn);

            public virtual extern long reduce(es5.Int8Array.reduceFn callbackfn, long initialValue);

            public virtual extern U reduce<U>(es5.Int8Array.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern long reduceRight(es5.Int8Array.reduceRightFn callbackfn);

            public virtual extern long reduceRight(
              es5.Int8Array.reduceRightFn callbackfn,
              long initialValue);

            public virtual extern U reduceRight<U>(
              es5.Int8Array.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern es5.Int8Array reverse();

            public virtual extern void set(es5.ArrayLike<sbyte> array);

            public virtual extern void set(es5.ArrayLike<sbyte> array, uint offset);

            public virtual extern es5.Int8Array slice();

            public virtual extern es5.Int8Array slice(uint start);

            public virtual extern es5.Int8Array slice(uint start, uint end);

            public virtual extern bool some(es5.Int8Array.someFn callbackfn);

            public virtual extern bool some(es5.Int8Array.someFn callbackfn, object thisArg);

            public virtual extern es5.Int8Array sort();

            public virtual extern es5.Int8Array sort(es5.Int8Array.sortFn compareFn);

            public virtual extern es5.Int8Array subarray(uint begin);

            public virtual extern es5.Int8Array subarray(uint begin, int end);

            public virtual extern string toLocaleString();

            public virtual extern string toString();

            public virtual extern sbyte this[uint index] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<sbyte>.IndexOf(sbyte item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<sbyte>.Insert(int index, sbyte item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<sbyte>.RemoveAt(int index);

            extern sbyte IList<sbyte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<sbyte>.Add(sbyte item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<sbyte>.CopyTo(sbyte[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<sbyte>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<sbyte>.Contains(sbyte item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<sbyte>.Remove(sbyte item);

            int ICollection<sbyte>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<sbyte>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get
        ;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<sbyte> IEnumerable<sbyte>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern sbyte IReadOnlyList<sbyte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<sbyte>.Count
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
            public delegate bool everyFn(sbyte value, uint index, es5.Int8Array array);

            [Generated]
            public delegate object filterFn(sbyte value, uint index, es5.Int8Array array);

            [Generated]
            public delegate bool findFn(sbyte value, uint index, es5.Int8Array obj);

            [Generated]
            public delegate bool findIndexFn(sbyte value, uint index, es5.Int8Array obj);

            [Generated]
            public delegate void forEachFn(sbyte value, uint index, es5.Int8Array array);

            [Generated]
            public delegate double mapFn(sbyte value, uint index, es5.Int8Array array);

            [Generated]
            public delegate double reduceFn(
              long previousValue,
              sbyte currentValue,
              uint currentIndex,
              es5.Int8Array array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              sbyte currentValue,
              uint currentIndex,
              es5.Int8Array array);

            [Generated]
            public delegate double reduceRightFn(
              long previousValue,
              sbyte currentValue,
              uint currentIndex,
              es5.Int8Array array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              sbyte currentValue,
              uint currentIndex,
              es5.Int8Array array);

            [Generated]
            public delegate bool someFn(sbyte value, uint index, es5.Int8Array array);

            [Generated]
            public delegate double sortFn(sbyte a, sbyte b);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
