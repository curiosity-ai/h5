using System.Collections;
using System.Collections.Generic;
using HighFive;

namespace Tesserae
{
    /// <summary>
    /// This is essentially a type alias for a typed array that may be present on method parameters to specify that the method will not mutate the data. It only exists at compile time, at runtime the reference
    /// will be the underlying array. This means that there is no cost to casting to this type from an array (so even very large arrays may be cast to it without there being any worries regarding performance
    /// and whether every element will be visited and copied during the translation).
    ///
    /// It is somewhat similar to the ReadOnlyCollection return when AsReadOnly() is called on a List - it may only be used to indicate that the receiver of the read only reference will not change the data, it
    /// does not guarantee that the data itself is immutable (if something still holds a reference to the mutable array then changing that content will result in those changes appearing through the read only
    /// wrapper).
    /// </summary>
    public sealed class ReadOnlyArray<T> : IEnumerable<T>
    {
        [Template("{data}")]
        public extern ReadOnlyArray(T[] data);

        [External] // Required due to https://github.com/bridgedotnet/Bridge/issues/4015
        public extern T this[int index] { [Template("{this}[{index}]")] get; }

        public extern int Length { [Name("length")] get; }

        [External]
        public extern IEnumerator<T> GetEnumerator();

        // Can't use extern on an explicitly-implemented interface method so this method needs a body (even though it will never be called)
        IEnumerator IEnumerable.GetEnumerator() => null;

        // Support an implicit operator to ReadOnlyArray from a regular array so that a method may specify that it accepts a read only version (because it won't change it) but allow the caller to pass in a
        // regular array, rather than making them jump through hoops. There will be no operator to go the other way because that could break the contract - eg. method A has a regular array and passes it to B,
        // which receives it as a read only version; method B then passes it to method C, whose argument specifies a regular array because it may mutate it; method B may not pass the underlying array to method
        // C as method B has said that it won't change the data (instead ToArray() should be called when method B calls method C, so that a copy of the data is given to method C and method B doesn't aid and abet
        // this mutation)
        public static implicit operator ReadOnlyArray<T>(T[] data) => Script.Write<ReadOnlyArray<T>>("{0}", data);
    }

    public static class ArrayExtensions
    {
        /// <summary>
        /// Depending upon the use case, the code may be clearer if this extension method or used or it may be clearer to rely upon and explicit or implicit cast from array to ReadOnlyArray
        /// </summary>
        public static ReadOnlyArray<T> AsReadOnlyArray<T>(this T[] source) => (ReadOnlyArray<T>)source;
    }
}
