using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Contract
{
    public interface IAnonymousTypeConfig
    {
        string Code{ get; set; }

        string Name{ get; set; }

        AnonymousType Type{ get; set; }

        bool Emitted{ get; set; }
    }
}