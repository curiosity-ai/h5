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
        [StaticInterface("Uint8ArrayConstructor")]
        [FormerInterface]
        public class Uint8Array : IList<byte>, ICollection<byte>, IEnumerable<byte>, IEnumerable, IH5Class, IReadOnlyList<byte>, IReadOnlyCollection<byte>, ICollection, IObject
        {
            public extern Uint8Array(uint length);

            public extern Uint8Array(
              Union<es5.ArrayLike<byte>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            public extern Uint8Array(es5.ArrayLike<byte> arrayOrArrayBuffer);

            public extern Uint8Array(es5.ArrayBufferLike arrayOrArrayBuffer);

            public extern Uint8Array(es5.ArrayBuffer arrayOrArrayBuffer);

            public extern Uint8Array(es5.ArrayBufferLike buffer, uint byteOffset);

            public extern Uint8Array(es5.ArrayBuffer buffer, uint byteOffset);

            public extern Uint8Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

            public extern Uint8Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public static es5.Uint8Array prototype
            {
                get;
            }

            [Name("BYTES_PER_ELEMENT")]
            public static double BYTES_PER_ELEMENT_Static
            {
                get;
            }

            [ExpandParams]
            public static extern es5.Uint8Array of(params byte[] items);

            public static extern es5.Uint8Array from(es5.ArrayLike<byte> arrayLike);

            public static extern es5.Uint8Array from(
              es5.ArrayLike<byte> arrayLike,
              es5.Uint8Array.fromFn mapfn);

            public static extern es5.Uint8Array from(
              es5.ArrayLike<byte> arrayLike,
              es5.Uint8Array.fromFn mapfn,
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

            public virtual extern es5.Uint8Array copyWithin(long target, long start);

            public virtual extern es5.Uint8Array copyWithin(long target, long start, long end);

            public virtual extern bool every(es5.Uint8Array.everyFn callbackfn);

            public virtual extern bool every(es5.Uint8Array.everyFn callbackfn, object thisArg);

            public virtual extern es5.Uint8Array fill(byte value);

            public virtual extern es5.Uint8Array fill(byte value, uint start);

            public virtual extern es5.Uint8Array fill(byte value, uint start, uint end);

            public virtual extern es5.Uint8Array filter(es5.Uint8Array.filterFn callbackfn);

            public virtual extern es5.Uint8Array filter(
              es5.Uint8Array.filterFn callbackfn,
              object thisArg);

            public virtual extern Union<byte, Undefined> find(
              es5.Uint8Array.findFn predicate);

            public virtual extern Union<byte, Undefined> find(
              es5.Uint8Array.findFn predicate,
              object thisArg);

            public virtual extern uint findIndex(es5.Uint8Array.findIndexFn predicate);

            public virtual extern uint findIndex(es5.Uint8Array.findIndexFn predicate, object thisArg);

            public virtual extern void forEach(es5.Uint8Array.forEachFn callbackfn);

            public virtual extern void forEach(es5.Uint8Array.forEachFn callbackfn, object thisArg);

            public virtual extern uint indexOf(byte searchElement);

            public virtual extern uint indexOf(byte searchElement, uint fromIndex);

            public virtual extern string join();

            public virtual extern string join(string separator);

            public virtual extern uint lastIndexOf(byte searchElement);

            public virtual extern uint lastIndexOf(byte searchElement, uint fromIndex);

            public virtual uint length
            {
                get;
            }

            public virtual extern es5.Uint8Array map(es5.Uint8Array.mapFn callbackfn);

            public virtual extern es5.Uint8Array map(es5.Uint8Array.mapFn callbackfn, object thisArg);

            public virtual extern long reduce(es5.Uint8Array.reduceFn callbackfn);

            public virtual extern long reduce(es5.Uint8Array.reduceFn callbackfn, long initialValue);

            public virtual extern U reduce<U>(es5.Uint8Array.reduceFn2<U> callbackfn, U initialValue);

            public virtual extern long reduceRight(es5.Uint8Array.reduceRightFn callbackfn);

            public virtual extern long reduceRight(
              es5.Uint8Array.reduceRightFn callbackfn,
              long initialValue);

            public virtual extern U reduceRight<U>(
              es5.Uint8Array.reduceRightFn2<U> callbackfn,
              U initialValue);

            public virtual extern es5.Uint8Array reverse();

            public virtual extern void set(es5.ArrayLike<byte> array);

            public virtual extern void set(es5.ArrayLike<byte> array, uint offset);

            public virtual extern es5.Uint8Array slice();

            public virtual extern es5.Uint8Array slice(uint start);

            public virtual extern es5.Uint8Array slice(uint start, uint end);

            public virtual extern bool some(es5.Uint8Array.someFn callbackfn);

            public virtual extern bool some(es5.Uint8Array.someFn callbackfn, object thisArg);

            public virtual extern es5.Uint8Array sort();

            public virtual extern es5.Uint8Array sort(es5.Uint8Array.sortFn compareFn);

            public virtual extern es5.Uint8Array subarray(uint begin);

            public virtual extern es5.Uint8Array subarray(uint begin, int end);

            public virtual extern string toLocaleString();

            public virtual extern string toString();

            public virtual extern byte this[uint index] { get; set; }

            [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
            extern int IList<byte>.IndexOf(byte item);

            [Template("System.Array.insert({this}, {index}, {item}, {T})")]
            extern void IList<byte>.Insert(int index, byte item);

            [Template("System.Array.removeAt({this}, {index}, {T})")]
            extern void IList<byte>.RemoveAt(int index);

            extern byte IList<byte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

            [Template("System.Array.add({this}, {item}, {T})")]
            extern void ICollection<byte>.Add(byte item);

            [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
            extern void ICollection<byte>.CopyTo(byte[] array, int arrayIndex);

            [Template("System.Array.clear({this}, {T})")]
            extern void ICollection<byte>.Clear();

            [Template("System.Array.contains({this}, {item}, {T})")]
            extern bool ICollection<byte>.Contains(byte item);

            [Template("System.Array.remove({this}, {item}, {T})")]
            extern bool ICollection<byte>.Remove(byte item);

            int ICollection<byte>.Count
            {
                [Template("System.Array.getCount({this}, {T})")]
                get;
            }

            bool ICollection<byte>.IsReadOnly
            {
                [Template("System.Array.getIsReadOnly({this}, {T})")]
                get;
            }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<byte> IEnumerable<byte>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();

            extern byte IReadOnlyList<byte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

            int IReadOnlyCollection<byte>.Count
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
            public delegate bool everyFn(byte value, uint index, es5.Uint8Array array);

            [Generated]
            public delegate object filterFn(byte value, uint index, es5.Uint8Array array);

            [Generated]
            public delegate bool findFn(byte value, uint index, es5.Uint8Array obj);

            [Generated]
            public delegate bool findIndexFn(byte value, uint index, es5.Uint8Array obj);

            [Generated]
            public delegate void forEachFn(byte value, uint index, es5.Uint8Array array);

            [Generated]
            public delegate double mapFn(byte value, uint index, es5.Uint8Array array);

            [Generated]
            public delegate double reduceFn(
              long previousValue,
              byte currentValue,
              uint currentIndex,
              es5.Uint8Array array);

            [Generated]
            public delegate U reduceFn2<U>(
              U previousValue,
              byte currentValue,
              uint currentIndex,
              es5.Uint8Array array);

            [Generated]
            public delegate double reduceRightFn(
              long previousValue,
              byte currentValue,
              uint currentIndex,
              es5.Uint8Array array);

            [Generated]
            public delegate U reduceRightFn2<U>(
              U previousValue,
              byte currentValue,
              uint currentIndex,
              es5.Uint8Array array);

            [Generated]
            public delegate bool someFn(byte value, uint index, es5.Uint8Array array);

            [Generated]
            public delegate double sortFn(byte a, byte b);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
