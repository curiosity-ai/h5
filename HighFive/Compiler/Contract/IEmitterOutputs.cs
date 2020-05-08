using System.Collections.Generic;

namespace HighFive.Contract
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