namespace System.Collections
{
    [HighFive.External]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.Reflectable]
    public interface ICollection : IEnumerable, HighFive.IHighFiveClass
    {
        /// <summary>
        /// Gets the number of elements contained in the ICollection.
        /// </summary>
        int Count
        {
            [HighFive.Template("System.Array.getCount({this})")]
            get;
        }

        [HighFive.Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
        void CopyTo(Array array, int arrayIndex);

        /// <summary>
        /// Gets an object that can be used to synchronize access to the System.Collections.ICollection.
        /// </summary>
        /// <returns>
        /// An object that can be used to synchronize access to the System.Collections.ICollection.
        /// </returns>
        object SyncRoot { get; }

        /// <summary>
        /// Gets a value indicating whether access to the System.Collections.ICollection
        /// is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// true if access to the System.Collections.ICollection is synchronized (thread
        /// safe); otherwise, false.
        /// </returns>
        bool IsSynchronized { get; }
    }
}