namespace System.Collections.Generic
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public abstract class Comparer : IComparer
    {
        public static extern Comparer Default
        {
            [H5.Template("new (System.Collections.Generic.Comparer$1(Object))(System.Collections.Generic.Comparer$1.$default.fn)")]
            get;
        }

        public abstract int Compare(Object x, Object y);
    }
}