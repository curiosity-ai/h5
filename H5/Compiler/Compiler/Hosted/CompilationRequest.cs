using H5.Contract;
using H5.Translator;
using MessagePack;

namespace H5.Compiler.Hosted
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class CompilationRequest
    {
        public string ProjectLocation { get; set; }
        public string OutputLocation { get; set; }
        public string DefaultFileName { get; set; }
        public string H5Location { get; set; }
        public bool Rebuild { get; set; }
        public bool NoCompilationServer { get; set; }

        public string WorkingDirectory { get; set; }

        public ProjectProperties ProjectProperties { get; set; } = new ProjectProperties();

        internal CompilationOptions ToOptions()
        {
            return new CompilationOptions()
            {
                ProjectLocation = ProjectLocation,
                OutputLocation = OutputLocation,
                DefaultFileName = DefaultFileName,
                H5Location = H5Location,
                Rebuild = Rebuild,
                ProjectProperties = ProjectProperties
            };
        }
    }
}
