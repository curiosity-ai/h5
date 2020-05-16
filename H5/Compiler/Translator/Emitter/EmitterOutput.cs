using H5.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H5.Translator
{
    public class EmitterOutput : IEmitterOutput
    {
        public EmitterOutput(string fileName)
        {
            FileName = fileName;
            ModuleOutput = new Dictionary<Module, StringBuilder>();
            NonModuletOutput = new StringBuilder();
            TopOutput = new StringBuilder();
            BottomOutput = new StringBuilder();
            ModuleDependencies = new Dictionary<string, List<IPluginDependency>>();
            Names = new List<string>();
        }

        public bool IsMetadata { get; set; }

        public string FileName { get; set; }

        public StringBuilder TopOutput { get; set; }

        public StringBuilder BottomOutput { get; set; }

        public StringBuilder NonModuletOutput { get; set; }

        public Dictionary<Module, StringBuilder> ModuleOutput { get; set; }

        public Dictionary<string, List<IPluginDependency>> ModuleDependencies { get; set; }

        public System.Collections.Generic.List<IPluginDependency> NonModuleDependencies { get; set; }

        public bool IsDefaultOutput
        {
            get
            {
                return FileName == AssemblyInfo.DEFAULT_FILENAME;
            }
        }

        public List<string> Names
        {
            get; set;
        }
    }

    public class EmitterOutputs : Dictionary<string, IEmitterOutput>, IEmitterOutputs
    {
        public IEmitterOutput FindModuleOutput(string moduleName)
        {
            if (this.Any(o => o.Value.ModuleOutput.Keys.Any(m => m.Name == moduleName)))
            {
                return this.First(o => o.Value.ModuleOutput.Keys.Any(m => m.Name == moduleName)).Value;
            }

            return null;
        }

        public IEmitterOutput DefaultOutput
        {
            get
            {
                return this.First(o => o.Value.IsDefaultOutput).Value;
            }
        }
    }
}