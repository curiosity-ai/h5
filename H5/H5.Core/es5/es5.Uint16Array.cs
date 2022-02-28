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
        [StaticInterface("Uint16ArrayConstructor")]
        [FormerInterface]
        public class Uint16Array : IList<ushort>, ICollection<ushort>, IEnumerable<ushort>, IEnumerable, IH5Class, IReadOnlyList<ushort>, IReadOnlyCollection<ushort>, ICollection, IObject
        {
            public extern Uint16Array(uint length);

            public extern Uint16Array(
              Union<es5.ArrayLike<ushort>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            public extern Uint16Array(es5.ArrayLike<ushort> arrayOrArrayBuffer);

            public extern Uint16Array(es5.ArrayBufferLike arrayOrArrayBuffer);

            public extern Uint16Array(es5.ArrayBuffer arrayOrArrayBuffer);

            public extern Uint16Array(es5.ArrayBufferLike buffer, uint byteOffset);

            public extern Uint16Array(es5.ArrayBuffer buffer, uint byteOffset);

            public extern Uint16Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

            public extern Uint16Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public static es5.Uint16Array prototype
            {
                get;
            }

            [Name("BYTES_PER_ELEMENT")]
            public static double BYTES_PER_ELEMENT_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Uint16Array of(params ushort[] items);

            public static extern es5.Uint16Array from(es5.ArrayLike<ushort> arrayLike);

            public static extern es5.Uint16Array from(
              es5.ArrayLike<ushort> arrayLike,
              es5.Uint16Array.fromFn mapfn);

            public static extern es5.Uint16Array from(
              es5.ArrayLike<ushort> arrayLike,
              es5.Uint16Array.fromFn mapfn,
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

            public virtual extern es5.Uint16Array copyWithin(long target, long start);

            public virtual extern es5.Uint16Array copyWithin(long target, long start, long end);

            public virtual extern bool every(es5.Uint16Array.everyFn callbackfn);

            public virtual extern bool every(es5.Uint16Array.everyFn callbackfn, object thisArg);

            public virtual extern es5.Uint16Array fill(ushort value);

            public virtual extern es5.Uint16Array fill(ushort value, uint start);

            public virtual extern es5.Uint16Array fill(ushort value, uint start, uint end);

            public virtual extern es5.Uint16Array filter(es5.Uint16Array.filterFn callbackfn);

            public virtual extern es5.Uint16Array filter(
              es5.Uint16Array.filterFn callbackfn,
              object thisArg);

            public virtual extern Union<ushort, Undefined> find(
              es5.Uint16Array.findFn predicate);

            public virtual extern Union<ushort, Undefined> find(
              es5.Uint16Array.findFn predicate,
              object thisArg);

            public virtual extern uint findIndex(es5.Uint16Array.findIndexFn predicate);

            public virtual extern uint findIndex(es5.Uint16Array.findIndexFn predicate, object thisArg);

            public virtual extern void forEach(es5.Uint16Array.forEachFn callbackfn);

            public virtual extern void forEach(es5.Uint16Array.forEachFn callbackfn, object thisArg);

            public virtual extern uint indexOf(ushort searchElement);

            public virtual extern uint indexOf(ushort searchElement, uint fromIndex);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern uint lastIndexOf(ushort searchElement);

            public virtual extern uint lastIndexOf(ushort searchElement, uint fromIndex);

            public virtual uint length
            {
                get;
            }

            public virtual extern es5.Uint16Array map(es5.Uint16Array.mapFn callbackfn);

            public virtual extern es5.Uint16Array map(es5.Uint16Array.mapFn callbackfn, object thisArg);

            public virtual extern long reduce(es5.Uint16Array.reduceFn callbackfn);

            public virtual extern long reduce(es5.Uint16Array.reduceFn callbackfn, long initialValue);

            public virtual extern U reduce<U>(es5.Uint16Array.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern long reduceRight(es5.Uint16Array.reduceRightFn callbackfn);

            public virtual extern long reduceRight(
              es5.Uint16Array.reduceRightFn callbackfn,
              long initialValue);

            public virtual extern U reduceRight<U>(
              es5.Uint16Array.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern es5.Uint16Array reverse();

            public virtual extern void set(es5.ArrayLike<ushort> array);

            public virtual extern void set(es5.ArrayLike<ushort> array, uint offset);

            public virtual extern es5.Uint16Array slice();

            public virtual extern es5.Uint16Array slice(uint start);

            public virtual extern es5.Uint16Array slice(uint start, uint end);

            public virtual extern bool some(es5.Uint16Array.someFn callbackfn);

            public virtual extern bool some(es5.Uint16Array.someFn callbackfn, object thisArg);

            public virtual extern es5.Uint16Array sort();

            public virtual extern es5.Uint16Array sort(es5.Uint16Array.sortFn compareFn);

            public virtual extern es5.Uint16Array subarray(uint begin);

            public virtual extern es5.Uint16Array subarray(uint begin, int end);

            public virtual extern string toLocaleString();

            public virtual extern string toString();

            public virtual extern ushort this[uint index] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<ushort>.IndexOf(ushort item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<ushort>.Insert(int index, ushort item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<ushort>.RemoveAt(int index);

            extern ushort IList<ushort>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<ushort>.Add(ushort item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<ushort>.CopyTo(ushort[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<ushort>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<ushort>.Contains(ushort item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<ushort>.Remove(ushort item);

            int ICollection<ushort>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<ushort>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<ushort> IEnumerable<ushort>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern ushort IReadOnlyList<ushort>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<ushort>.Count
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
            public delegate bool everyFn(ushort value, uint index, es5.Uint16Array array);

            [Generated]
            public delegate object filterFn(ushort value, uint index, es5.Uint16Array array);

            [Generated]
            public delegate bool findFn(ushort value, uint index, es5.Uint16Array obj);

            [Generated]
            public delegate bool findIndexFn(ushort value, uint index, es5.Uint16Array obj);

            [Generated]
            public delegate void forEachFn(ushort value, uint index, es5.Uint16Array array);

            [Generated]
            public delegate double mapFn(ushort value, uint index, es5.Uint16Array array);

            [Generated]
            public delegate double reduceFn(
              long previousValue,
              ushort currentValue,
              uint currentIndex,
              es5.Uint16Array array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              ushort currentValue,
              uint currentIndex,
              es5.Uint16Array array);

            [Generated]
            public delegate double reduceRightFn(
              long previousValue,
              ushort currentValue,
              uint currentIndex,
              es5.Uint16Array array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              ushort currentValue,
              uint currentIndex,
              es5.Uint16Array array);

            [Generated]
            public delegate bool someFn(ushort value, uint index, es5.Uint16Array array);

            [Generated]
            public delegate double sortFn(ushort a, ushort b);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
