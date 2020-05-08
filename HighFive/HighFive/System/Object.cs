namespace System
{
    [H5.External]
    [H5.IgnoreCast]
    [H5.Constructor("{ }")]
    public class Object
    {
        public virtual extern object this[string name]
        {
            [H5.External]
            get;
            [H5.External]
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        [H5.Template("H5.toString({this})")]
        public virtual extern string ToString();

        [H5.Convention(H5.Notation.CamelCase)]
        public virtual extern string ToLocaleString();

        [H5.Convention(H5.Notation.CamelCase)]
        public virtual extern object ValueOf();

        [H5.Convention(H5.Notation.CamelCase)]
        public virtual extern bool HasOwnProperty(object v);

        [H5.Convention(H5.Notation.CamelCase)]
        public virtual extern bool IsPrototypeOf(object v);

        [H5.Convention(H5.Notation.CamelCase)]
        public virtual extern bool PropertyIsEnumerable(object v);

        [H5.Convention(H5.Notation.CamelCase)]
        [H5.Template("<self>{this:type}")]
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
        [H5.Template("H5.clone({this})")]
        protected extern Object MemberwiseClone();

        [H5.Convention(H5.Notation.CamelCase)]
        [H5.Template("H5.referenceEquals({a}, {b})")]
        public static extern bool ReferenceEquals(object a, object b);

        [H5.Convention(H5.Notation.CamelCase)]
        [H5.Template("H5.equals({this}, {o})")]
        public virtual extern bool Equals(object o);

        [H5.Convention(H5.Notation.CamelCase)]
        [H5.Template("H5.equals({a}, {b})")]
        public static extern bool Equals(object a, object b);

        [H5.Convention(H5.Notation.CamelCase)]
        [H5.Template("H5.getHashCode({this})")]
        public virtual extern int GetHashCode();

        [H5.Convention(H5.Notation.CamelCase)]
        [H5.Template("Object.getOwnPropertyNames({obj})")]
        [H5.Unbox(true)]
        public static extern string[] GetOwnPropertyNames(object obj);

        [H5.Convention(H5.Notation.CamelCase)]
        [H5.Template("{T}.prototype")]
        public static extern dynamic GetPrototype<T>();

        public readonly Type Constructor;

#pragma warning disable 169
        private readonly Type ctor;
#pragma warning restore 169

        [H5.Template("{this}")]
        public virtual extern dynamic ToDynamic();
    }

    [H5.External]
    public static class ObjectExtensions
    {
        [H5.Template("{0}")]
        [H5.Unbox(true)]
        public static extern T As<T>(this object obj);

        [H5.Template("H5.cast({obj}, {T})")]
        public static extern T Cast<T>(this object obj);

        [H5.Template("H5.as({obj}, {T})")]
        public static extern T TryCast<T>(this object obj) where T : class;

        [H5.Template("H5.is({obj}, {T})")]
        public static extern bool Is<T>(this object obj);
    }
}