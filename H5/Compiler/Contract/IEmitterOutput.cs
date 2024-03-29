using System.Collections.Generic;

namespace H5.Contract
{
    public interface IEmitterOutput
    {
        bool IsMetadata { get; set; }

        string FileName { get; set; }

        bool IsDefaultOutput { get; }

        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<IModuleDependency>> ModuleDependencies { get; set; }

        System.Collections.Generic.List<IModuleDependency> NonModuleDependencies { get; set; }

        System.Collections.Generic.Dictionary<Module, System.Text.StringBuilder> ModuleOutput { get; set; }

        System.Text.StringBuilder NonModuletOutput { get; set; }

        System.Text.StringBuilder TopOutput { get; set; }

        System.Text.StringBuilder BottomOutput { get; set; }

        List<string> Names{ get; set; }
    }
}