namespace System.Diagnostics.Contracts
{
    [HighFive.Enum(HighFive.Emit.Name)]
    [HighFive.External]
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