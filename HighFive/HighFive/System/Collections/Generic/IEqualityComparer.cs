namespace System.Collections.Generic
{
    [H5.External]
    [H5.Reflectable]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    public interface IEqualityComparer<in T> : H5.IH5Class
    {
        [H5.Name("equals2")]
        bool Equals(T x, T y);

        [H5.Name("getHashCode2")]
        int GetHashCode(T obj);
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public abstract class EqualityComparer<T> : IEqualityComparer<T>, H5.IH5Class
    {
        public static extern EqualityComparer<T> Default { 
            [H5.Template("System.Collections.Generic.EqualityComparer$1({T}).def")]
            get; 
        }

        //private extern EqualityComparer();

        public virtual extern bool Equals(T x, T y);

        public virtual extern int GetHashCode(T obj);
    }
}