using System;

namespace Newtonsoft.Json.Serialization
{
    /// <summary>
    /// Allows users to control class loading and mandate what class to load.
    /// </summary>
    public interface ISerializationBinder
    {
        /// <summary>
        /// When implemented, controls the binding of a serialized object to a type.
        /// </summary>
        /// <param name="assemblyName">Specifies the Assembly name of the serialized object.</param>
        /// <param name="typeName">Specifies the <see cref="System.Type"/> name of the serialized object</param>
        /// <returns>The type of the object the formatter creates a new instance of.</returns>
        Type BindToType(string assemblyName, string typeName);

        /// <summary>
        /// When implemented, controls the binding of a serialized object to a type.
        /// </summary>
        /// <param name="serializedType">The type of the object the formatter creates a new instance of.</param>
        /// <param name="assemblyName">Specifies the Assembly name of the serialized object.</param>
        /// <param name="typeName">Specifies the <see cref="System.Type"/> name of the serialized object.</param>
        void BindToName(Type serializedType, out string assemblyName, out string typeName);
    }
}