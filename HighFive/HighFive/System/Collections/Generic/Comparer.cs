namespace System.Collections.Generic
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public abstract class Comparer<T> : IComparer<T>
    {
        public static extern Comparer<T> Default
        {
            [H5.Template("new (System.Collections.Generic.Comparer$1({T}))(System.Collections.Generic.Comparer$1.$default.fn)")]
            get;
        }

        public abstract int Compare(T x, T y);

        [H5.Template("new (System.Collections.Generic.Comparer$1({T}))({comparison})")]
        public static extern Comparer<T> Create(Comparison<T> comparison);
    }
}