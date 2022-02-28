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
        [StaticInterface("Uint32ArrayConstructor")]
        [FormerInterface]
        public class Uint32Array : IList<uint>, ICollection<uint>, IEnumerable<uint>, IEnumerable, IH5Class, IReadOnlyList<uint>, IReadOnlyCollection<uint>, ICollection, IObject
        {

            public extern Uint32Array(uint length);

            public extern Uint32Array(
              Union<es5.ArrayLike<uint>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            public extern Uint32Array(es5.ArrayLike<uint> arrayOrArrayBuffer);

            public extern Uint32Array(es5.ArrayBufferLike arrayOrArrayBuffer);

            public extern Uint32Array(es5.ArrayBuffer arrayOrArrayBuffer);

            public extern Uint32Array(es5.ArrayBufferLike buffer, uint byteOffset);

            public extern Uint32Array(es5.ArrayBuffer buffer, uint byteOffset);

            public extern Uint32Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

            public extern Uint32Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public static es5.Uint32Array prototype
            {
                get;
            }

            [Name("BYTES_PER_ELEMENT")]
            public static double BYTES_PER_ELEMENT_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Uint32Array of(params uint[] items);

            public static extern es5.Uint32Array from(es5.ArrayLike<uint> arrayLike);

            public static extern es5.Uint32Array from(
              es5.ArrayLike<uint> arrayLike,
              es5.Uint32Array.fromFn mapfn);

            public static extern es5.Uint32Array from(
              es5.ArrayLike<uint> arrayLike,
              es5.Uint32Array.fromFn mapfn,
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

            public virtual extern es5.Uint32Array copyWithin(long target, long start);

            public virtual extern es5.Uint32Array copyWithin(long target, long start, long end);

            public virtual extern bool every(es5.Uint32Array.everyFn callbackfn);

            public virtual extern bool every(es5.Uint32Array.everyFn callbackfn, object thisArg);

            public virtual extern es5.Uint32Array fill(uint value);

            public virtual extern es5.Uint32Array fill(uint value, uint start);

            public virtual extern es5.Uint32Array fill(uint value, uint start, uint end);

            public virtual extern es5.Uint32Array filter(es5.Uint32Array.filterFn callbackfn);

            public virtual extern es5.Uint32Array filter(
              es5.Uint32Array.filterFn callbackfn,
              object thisArg);

            public virtual extern Union<uint, Undefined> find(
              es5.Uint32Array.findFn predicate);

            public virtual extern Union<uint, Undefined> find(
              es5.Uint32Array.findFn predicate,
              object thisArg);

            public virtual extern uint findIndex(es5.Uint32Array.findIndexFn predicate);

            public virtual extern uint findIndex(es5.Uint32Array.findIndexFn predicate, object thisArg);

            public virtual extern void forEach(es5.Uint32Array.forEachFn callbackfn);

            public virtual extern void forEach(es5.Uint32Array.forEachFn callbackfn, object thisArg);

            public virtual extern uint indexOf(uint searchElement);

            public virtual extern uint indexOf(uint searchElement, uint fromIndex);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern uint lastIndexOf(uint searchElement);

            public virtual extern uint lastIndexOf(uint searchElement, uint fromIndex);

            public virtual uint length
            {
                get;
            }

            public virtual extern es5.Uint32Array map(es5.Uint32Array.mapFn callbackfn);

            public virtual extern es5.Uint32Array map(es5.Uint32Array.mapFn callbackfn, object thisArg);

            public virtual extern long reduce(es5.Uint32Array.reduceFn callbackfn);

            public virtual extern long reduce(es5.Uint32Array.reduceFn callbackfn, long initialValue);

            public virtual extern U reduce<U>(es5.Uint32Array.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern long reduceRight(es5.Uint32Array.reduceRightFn callbackfn);

            public virtual extern long reduceRight(
              es5.Uint32Array.reduceRightFn callbackfn,
              long initialValue);

            public virtual extern U reduceRight<U>(
              es5.Uint32Array.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern es5.Uint32Array reverse();

            public virtual extern void set(es5.ArrayLike<uint> array);

            public virtual extern void set(es5.ArrayLike<uint> array, uint offset);

            public virtual extern es5.Uint32Array slice();

            public virtual extern es5.Uint32Array slice(uint start);

            public virtual extern es5.Uint32Array slice(uint start, uint end);

            public virtual extern bool some(es5.Uint32Array.someFn callbackfn);

            public virtual extern bool some(es5.Uint32Array.someFn callbackfn, object thisArg);

            public virtual extern es5.Uint32Array sort();

            public virtual extern es5.Uint32Array sort(es5.Uint32Array.sortFn compareFn);

            public virtual extern es5.Uint32Array subarray(uint begin);

            public virtual extern es5.Uint32Array subarray(uint begin, int end);

            public virtual extern string toLocaleString();

            public virtual extern string toString();

            public virtual extern uint this[uint index] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<uint>.IndexOf(uint item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<uint>.Insert(int index, uint item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<uint>.RemoveAt(int index);

            extern uint IList<uint>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<uint>.Add(uint item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<uint>.CopyTo(uint[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<uint>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<uint>.Contains(uint item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<uint>.Remove(uint item);

            int ICollection<uint>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<uint>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<uint> IEnumerable<uint>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern uint IReadOnlyList<uint>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<uint>.Count
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
            public delegate bool everyFn(uint value, uint index, es5.Uint32Array array);

            [Generated]
            public delegate object filterFn(uint value, uint index, es5.Uint32Array array);

            [Generated]
            public delegate bool findFn(uint value, uint index, es5.Uint32Array obj);

            [Generated]
            public delegate bool findIndexFn(uint value, uint index, es5.Uint32Array obj);

            [Generated]
            public delegate void forEachFn(uint value, uint index, es5.Uint32Array array);

            [Generated]
            public delegate double mapFn(uint value, uint index, es5.Uint32Array array);

            [Generated]
            public delegate double reduceFn(
              long previousValue,
              uint currentValue,
              uint currentIndex,
              es5.Uint32Array array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              uint currentValue,
              uint currentIndex,
              es5.Uint32Array array);

            [Generated]
            public delegate double reduceRightFn(
              long previousValue,
              uint currentValue,
              uint currentIndex,
              es5.Uint32Array array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              uint currentValue,
              uint currentIndex,
              es5.Uint32Array array);

            [Generated]
            public delegate bool someFn(uint value, uint index, es5.Uint32Array array);

            [Generated]
            public delegate double sortFn(uint a, uint b);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
