namespace System.Collections
{
    [H5.External]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.Reflectable]
    public interface ICollection : IEnumerable, H5.IH5Class
    {
        /// <summary>
        /// Gets the number of elements contained in the ICollection.
        /// </summary>
        int Count
        {
            [H5.Template("System.Array.getCount({this})")]
            get;
        }

        [H5.Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
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