using System.Reflection;

namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreCast]
    [H5.Name("Function")]
    public class Delegate
    {
        public extern int Length
        {
            [H5.Template("{this}.length")]
            get;
        }

        protected extern Delegate(object target, string method);

        protected extern Delegate(Type target, string method);

        protected extern Delegate();

        public virtual extern object Apply(object thisArg);

        public virtual extern object Apply();

        public virtual extern object Apply(object thisArg, Array args);

        public virtual extern object Call(object thisArg, params object[] args);

        public virtual extern object Call(object thisArg);

        public virtual extern object Call();

        [H5.Template("H5.fn.combine({0}, {1})")]
        public static extern Delegate Combine(Delegate a, Delegate b);

        [H5.Template("H5.fn.remove({0}, {1})")]
        public static extern Delegate Remove(Delegate source, Delegate value);

        [H5.Template("H5.staticEquals({a}, {b})")]
        public static extern bool operator ==(Delegate a, Delegate b);

        [H5.Template("!H5.staticEquals({a}, {b})")]
        public static extern bool operator !=(Delegate a, Delegate b);

        [H5.Template("H5.Reflection.createDelegate({method}, {firstArgument})")]
        public static extern Delegate CreateDelegate(Type type, object firstArgument, MethodInfo method);

        [H5.Template("H5.fn.getInvocationList({this})")]
        public extern Delegate[] GetInvocationList();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreCast]
    [H5.Name("Function")]
    public class MulticastDelegate : Delegate
    {
        protected extern MulticastDelegate();

        protected extern MulticastDelegate(object target, string method);

        protected extern MulticastDelegate(Type target, string method);

        [H5.Template("H5.staticEquals({a}, {b})")]
        public static extern bool operator ==(MulticastDelegate a, MulticastDelegate b);

        [H5.Template("!H5.staticEquals({a}, {b})")]
        public static extern bool operator !=(MulticastDelegate a, MulticastDelegate b);
    }
}