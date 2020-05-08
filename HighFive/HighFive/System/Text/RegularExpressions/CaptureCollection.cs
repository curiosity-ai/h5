using System.Collections;

namespace System.Text.RegularExpressions
{
    /// <summary>
    /// Represents the set of captures made by a single capturing group.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public class CaptureCollection : ICollection
    {
        internal extern CaptureCollection();

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        public extern object SyncRoot
        {
            [HighFive.Template("getSyncRoot()")]
            get;
        }

        /// <summary>
        /// Gets a value that indicates whether access to the collection is synchronized (thread-safe).
        /// </summary>
        public extern bool IsSynchronized
        {
            [HighFive.Template("getIsSynchronized()")]
            get;
        }

        /// <summary>
        /// Gets a value that indicates whether the collection is read only.
        /// </summary>
        public extern bool IsReadOnly
        {
            [HighFive.Template("getIsReadOnly()")]
            get;
        }

        /// <summary>
        /// Gets the number of substrings captured by the group.
        /// </summary>
        public extern int Count
        {
            [HighFive.Template("getCount()")]
            get;
        }

        /// <summary>
        /// Gets an individual member of the collection.
        /// </summary>

        public extern Capture this[int i]
        {
            [HighFive.Template("get({0})")]
            get;
        }

        /// <summary>
        /// Copies all the elements of the collection to the given array beginning at the given index.
        /// </summary>
        public extern void CopyTo(Array array, int arrayIndex);

        /// <summary>
        /// Provides an enumerator that iterates through the collection.
        /// </summary>
        [HighFive.Convention(HighFive.Notation.None)]
        public extern IEnumerator GetEnumerator();
    }
}