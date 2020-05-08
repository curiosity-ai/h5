using System.Collections.Generic;

namespace H5.Contract
{
    public interface IEmitterOutputs : IDictionary<string, IEmitterOutput>
    {
        IEmitterOutput DefaultOutput
        {
            get;
        }

        IEmitterOutput FindModuleOutput(string moduleName);
    }
}