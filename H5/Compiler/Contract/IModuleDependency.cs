namespace H5.Contract
{
    public interface IModuleDependency
    {
        string DependencyName { get; set; }

        string VariableName { get; set; }

        ModuleType? Type{ get; set; }
    }
}