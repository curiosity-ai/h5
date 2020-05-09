namespace System.Diagnostics.Contracts
{
    [H5.Enum(H5.Emit.Name)]
    [H5.External]
    public enum ContractFailureKind
    {
        Precondition,
        Postcondition,
        PostconditionOnException,
        Invariant,
        Assert,
        Assume,
    }
}