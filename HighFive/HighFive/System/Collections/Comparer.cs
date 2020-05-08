namespace System.Collections.Generic
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public abstract class Comparer : IComparer
    {
        public static extern Comparer Default
        {
            [HighFive.Template("new (System.Collections.Generic.Comparer$1(Object))(System.Collections.Generic.Comparer$1.$default.fn)")]
            get;
        }

        public abstract int Compare(Object x, Object y);
    }
}