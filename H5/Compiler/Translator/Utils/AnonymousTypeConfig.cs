using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Contract
{
    public class AnonymousTypeConfig : IAnonymousTypeConfig
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public AnonymousType Type { get; set; }

        public bool Emitted { get; set; }
    }
}