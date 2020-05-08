namespace System
{
    [HighFive.External]
    [HighFive.IgnoreCast]
    [HighFive.Constructor("{ }")]
    public class Object
    {
        public virtual extern object this[string name]
        {
            [HighFive.External]
            get;
            [HighFive.External]
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        [HighFive.Template("HighFive.toString({this})")]
        public virtual extern string ToString();

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public virtual extern string ToLocaleString();

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public virtual extern object ValueOf();

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public virtual extern bool HasOwnProperty(object v);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public virtual extern bool IsPrototypeOf(object v);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public virtual extern bool PropertyIsEnumerable(object v);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        [HighFive.Template("<self>{this:type}")]
        public extern Type GetType();

        // Returns a new object instance that is a memberwise copy of this
        // object.  This is always a shallow copy of the instance. The method is protected
        // so that other object may only call this method on themselves.  It is entended to
        // support the ICloneable interface.
        //
        // TODO: NotSupported
        //[System.Security.SecuritySafeCritical]  // auto-generated
        //[ResourceExposure(ResourceScope.None)]
        //[MethodImplAttribute(MethodImplOptions.InternalCall)]
        [HighFive.Template("HighFive.clone({this})")]
        protected extern Object MemberwiseClone();

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        [HighFive.Template("HighFive.referenceEquals({a}, {b})")]
        public static extern bool ReferenceEquals(object a, object b);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        [HighFive.Template("HighFive.equals({this}, {o})")]
        public virtual extern bool Equals(object o);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        [HighFive.Template("HighFive.equals({a}, {b})")]
        public static extern bool Equals(object a, object b);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        [HighFive.Template("HighFive.getHashCode({this})")]
        public virtual extern int GetHashCode();

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        [HighFive.Template("Object.getOwnPropertyNames({obj})")]
        [HighFive.Unbox(true)]
        public static extern string[] GetOwnPropertyNames(object obj);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        [HighFive.Template("{T}.prototype")]
        public static extern dynamic GetPrototype<T>();

        public readonly Type Constructor;

#pragma warning disable 169
        private readonly Type ctor;
#pragma warning restore 169

        [HighFive.Template("{this}")]
        public virtual extern dynamic ToDynamic();
    }

    [HighFive.External]
    public static class ObjectExtensions
    {
        [HighFive.Template("{0}")]
        [HighFive.Unbox(true)]
        public static extern T As<T>(this object obj);

        [HighFive.Template("HighFive.cast({obj}, {T})")]
        public static extern T Cast<T>(this object obj);

        [HighFive.Template("HighFive.as({obj}, {T})")]
        public static extern T TryCast<T>(this object obj) where T : class;

        [HighFive.Template("HighFive.is({obj}, {T})")]
        public static extern bool Is<T>(this object obj);
    }
}