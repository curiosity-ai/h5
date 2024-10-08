namespace H5.Contract
{
    public class VersionContext
    {
        public class AssemblyVersion
        {
            public string CompanyName{ get; set; }

            public string Copyright{ get; set; }

            public string Description{ get; set; }

            public string Name{ get; set; }

            public string Version{ get; set; }
        }

        public AssemblyVersion Assembly{ get; set; }

        public AssemblyVersion H5{ get; set; }

        public AssemblyVersion Compiler{ get; set; }
    }
 }
