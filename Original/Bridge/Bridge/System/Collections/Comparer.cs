namespace System.Collections.Generic
{
    [Bridge.Convention(Member = Bridge.ConventionMember.Field | Bridge.ConventionMember.Method, Notation = Bridge.Notation.CamelCase)]
    [Bridge.External]
    public abstract class Comparer : IComparer
    {
        public static extern Comparer Default
        {
            [Bridge.Template("new (System.Collections.Generic.Comparer$1(Object))(System.Collections.Generic.Comparer$1.$default.fn)")]
            get;
        }

        public abstract int Compare(Object x, Object y);
    }
}