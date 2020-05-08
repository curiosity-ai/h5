namespace System.Diagnostics.Contracts
{
    /// <summary>
    /// Attribute that specifies that an assembly is a reference assembly with contracts.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    [H5.External]
    public sealed class ContractReferenceAssemblyAttribute : Attribute
    {
    }
}