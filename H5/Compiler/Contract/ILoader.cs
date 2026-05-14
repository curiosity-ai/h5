namespace H5.Contract
{
    /// <summary>
    /// Retained for source compatibility. The legacy AMD / CommonJS / ES6 loader
    /// selectors have been removed; the packaging of generated JavaScript is now
    /// controlled via <c>OutputModuleType</c> in <c>h5.json</c>.
    /// </summary>
    public enum ModuleLoaderType
    {
        /// <summary>The only supported value.</summary>
        Global = 0,
    }

    public interface IModuleLoader
    {
        ModuleLoaderType Type{ get; set; }

        string FunctionName{ get; set; }

        bool ManualLoading{ get; set; }

        string ManualLoadingMask{ get; set; }

        bool SkipManualVariables{ get; set; }

        bool IsManual(string name);
    }
}