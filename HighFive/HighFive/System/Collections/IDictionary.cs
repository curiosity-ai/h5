using System.Collections.Generic;

namespace System.Collections
{
    [HighFive.External]
    [HighFive.Unbox(true)]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Reflectable]
    public interface IDictionary : ICollection, HighFive.IHighFiveClass
    {
        [HighFive.AccessorsIndexer]
        object this[object key]
        {
            get;
            set;
        }

        ICollection Keys
        {
            get;
        }

        ICollection Values
        {
            get;
        }

        /// <summary>
        /// Returns whether this dictionary contains a particular key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(Object key);

        /// <summary>
        /// Returns an System.Collections.IDictionaryEnumerator object for the System.Collections.IDictionary
        /// object.
        /// </summary>
        /// <returns>
        /// An System.Collections.IDictionaryEnumerator object for the System.Collections.IDictionary
        /// object.
        /// </returns>
        [HighFive.Convention(HighFive.Notation.None)]
        new IDictionaryEnumerator GetEnumerator();

        /// <summary>
        /// Adds a key-value pair to the dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(Object key, Object value);

        /// <summary>
        /// Removes all elements from the System.Collections.IDictionary object.
        /// </summary>
        /// <exception cref="System.NotSupportedException">
        /// The System.Collections.IDictionary object is read-only.
        /// </exception>
        void Clear();

        /// <summary>
        /// Gets a value indicating whether the System.Collections.IDictionary object is
        /// read-only.
        /// </summary>
        /// <returns>
        /// true if the System.Collections.IDictionary object is read-only; otherwise, false.
        /// </returns>
        bool IsReadOnly
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the System.Collections.IDictionary object has
        /// a fixed size.
        /// </summary>
        /// <returns>
        /// true if the System.Collections.IDictionary object has a fixed size; otherwise,
        /// false.
        /// </returns>
        bool IsFixedSize { get; }

        /// <summary>
        /// Removes a particular key from the dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        void Remove(Object key);
    }
}