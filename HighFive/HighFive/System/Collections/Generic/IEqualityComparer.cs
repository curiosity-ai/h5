namespace System.Collections.Generic
{
    [HighFive.External]
    [HighFive.Reflectable]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    public interface IEqualityComparer<in T> : HighFive.IHighFiveClass
    {
        [HighFive.Name("equals2")]
        bool Equals(T x, T y);

        [HighFive.Name("getHashCode2")]
        int GetHashCode(T obj);
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public abstract class EqualityComparer<T> : IEqualityComparer<T>, HighFive.IHighFiveClass
    {
        [HighFive.Template("System.Collections.Generic.EqualityComparer$1({T}).def")]
        public static extern EqualityComparer<T> Default();

        //private extern EqualityComparer();

        public virtual extern bool Equals(T x, T y);

        public virtual extern int GetHashCode(T obj);
    }
}