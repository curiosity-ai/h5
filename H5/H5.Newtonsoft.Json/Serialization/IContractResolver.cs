using System;

namespace Newtonsoft.Json.Serialization
{
    /// <summary>
    /// Used by <see cref="JsonConvert"/> to resolve a contract for a given <see cref="Type"/>.
    /// </summary>
    public interface IContractResolver
    {
        /*
        /// <summary>
        /// Resolves the contract for a given type.
        /// </summary>
        /// <param name="type">The type to resolve a contract for.</param>
        /// <returns>The contract for a given type.</returns>
        JsonContract ResolveContract(Type type);
        */
    }
}