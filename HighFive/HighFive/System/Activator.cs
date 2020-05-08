namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public static class Activator
    {
        [HighFive.Template("HighFive.createInstance({type}, {arguments:array})", "HighFive.Reflection.applyConstructor({type}, {arguments:array})")]
        public static extern object CreateInstance(Type type, params object[] arguments);

        [HighFive.Template("HighFive.createInstance({T}, {arguments:array})", "HighFive.Reflection.applyConstructor({T}, {arguments:array})")]
        public static extern T CreateInstance<T>(params object[] arguments);

        [HighFive.Template("HighFive.createInstance({type})")]
        public static extern object CreateInstance(Type type);

        [HighFive.Template("HighFive.createInstance({type}, {nonPublic})")]
        public static extern object CreateInstance(Type type, bool nonPublic);

        [HighFive.Template("HighFive.createInstance({T})")]
        public static extern T CreateInstance<T>();
    }
}