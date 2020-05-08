using System.Reflection;

namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreCast]
    [HighFive.Name("Function")]
    public class Delegate
    {
        public extern int Length
        {
            [HighFive.Template("{this}.length")]
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

        [HighFive.Template("HighFive.fn.combine({0}, {1})")]
        public static extern Delegate Combine(Delegate a, Delegate b);

        [HighFive.Template("HighFive.fn.remove({0}, {1})")]
        public static extern Delegate Remove(Delegate source, Delegate value);

        [HighFive.Template("HighFive.staticEquals({a}, {b})")]
        public static extern bool operator ==(Delegate a, Delegate b);

        [HighFive.Template("!HighFive.staticEquals({a}, {b})")]
        public static extern bool operator !=(Delegate a, Delegate b);

        [HighFive.Template("HighFive.Reflection.createDelegate({method}, {firstArgument})")]
        public static extern Delegate CreateDelegate(Type type, object firstArgument, MethodInfo method);

        [HighFive.Template("HighFive.fn.getInvocationList({this})")]
        public extern Delegate[] GetInvocationList();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreCast]
    [HighFive.Name("Function")]
    public class MulticastDelegate : Delegate
    {
        protected extern MulticastDelegate();

        protected extern MulticastDelegate(object target, string method);

        protected extern MulticastDelegate(Type target, string method);

        [HighFive.Template("HighFive.staticEquals({a}, {b})")]
        public static extern bool operator ==(MulticastDelegate a, MulticastDelegate b);

        [HighFive.Template("!HighFive.staticEquals({a}, {b})")]
        public static extern bool operator !=(MulticastDelegate a, MulticastDelegate b);
    }
}