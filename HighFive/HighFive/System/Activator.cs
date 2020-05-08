namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public static class Activator
    {
        [H5.Template("H5.createInstance({type}, {arguments:array})", "H5.Reflection.applyConstructor({type}, {arguments:array})")]
        public static extern object CreateInstance(Type type, params object[] arguments);

        [H5.Template("H5.createInstance({T}, {arguments:array})", "H5.Reflection.applyConstructor({T}, {arguments:array})")]
        public static extern T CreateInstance<T>(params object[] arguments);

        [H5.Template("H5.createInstance({type})")]
        public static extern object CreateInstance(Type type);

        [H5.Template("H5.createInstance({type}, {nonPublic})")]
        public static extern object CreateInstance(Type type, bool nonPublic);

        [H5.Template("H5.createInstance({T})")]
        public static extern T CreateInstance<T>();
    }
}