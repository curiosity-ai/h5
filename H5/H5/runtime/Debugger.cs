namespace System.Diagnostics
{
    [H5.Reflectable(false)]
    public static class Debugger
    {
        [H5.Template("debugger")]
        public static extern void Break();

        public static readonly string DefaultCategory;
        public static bool IsAttached { get { return true; } }
        public static bool IsLogging() { return true; }
        public static bool Launch() { return true; }
        public static void Log(int level, string category, string message) { }
        public static void NotifyOfCrossThreadDependency() { }
    }
}