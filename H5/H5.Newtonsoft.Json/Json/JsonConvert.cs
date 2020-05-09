using System;
using H5;

namespace Newtonsoft.Json
{
    /// <summary>
    /// Provides methods for converting between .NET types and JSON types.
    /// </summary>
    [External]
    public static class JsonConvert
    {
        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        [Unbox(false)]
        public static extern string SerializeObject(object value);

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>A JSON string representation of the object.</returns>
        [Unbox(false)]
        public static extern string SerializeObject(object value, Formatting formatting);

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting and <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [Unbox(false)]
        public static extern string SerializeObject(object value, JsonSerializerSettings settings);

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting and <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized. This parameter is used when TypeNameHandling is Auto to write out the type name if the type of the value does not match. Specifying the type is optional.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [Unbox(false)]
        [Template("Newtonsoft.Json.JsonConvert.SerializeObject({value}, null, {settings}, false, {type})")]
        public static extern string SerializeObject(object value, Type type, JsonSerializerSettings settings);

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting and <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [Unbox(false)]
        public static extern string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings);

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting and <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized. This parameter is used when TypeNameHandling is Auto to write out the type name if the type of the value does not match. Specifying the type is optional.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [Unbox(false)]
        [Template("Newtonsoft.Json.JsonConvert.SerializeObject({value}, {formatting}, {settings}, false, {type})")]
        public static extern string SerializeObject(object value, Type type, Formatting formatting, JsonSerializerSettings settings);

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static extern object DeserializeObject(string value, Type type);

        [Template("Newtonsoft.Json.JsonConvert.DeserializeObject({value}, System.Object, {settings})")]
        public static extern object DeserializeObject(string value, JsonSerializerSettings settings);

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [Template("Newtonsoft.Json.JsonConvert.DeserializeObject({value}, {T})")]
        public static extern T DeserializeObject<T>(string value);

        /// <summary>
        /// Deserializes the JSON to the given anonymous type.
        /// </summary>
        /// <typeparam name="T">
        /// The anonymous type to deserialize to. This can't be specified
        /// traditionally and must be inferred from the anonymous type passed
        /// as a parameter.
        /// </typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <returns>The deserialized anonymous type from the JSON string.</returns>
        [Template("Newtonsoft.Json.JsonConvert.DeserializeObject({value}, H5.getType({anonymousTypeObject}))")]
        public static extern T DeserializeAnonymousType<T>(string value, T anonymousTypeObject);

        /// <summary>
        /// Deserializes the JSON to the given anonymous type using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The anonymous type to deserialize to. This can't be specified
        /// traditionally and must be inferred from the anonymous type passed
        /// as a parameter.
        /// </typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <param name="settings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized anonymous type from the JSON string.</returns>
        [Template("Newtonsoft.Json.JsonConvert.DeserializeObject({value}, H5.getType({anonymousTypeObject}), {settings})")]
        public static extern T DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings);

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The object to deserialize.</param>
        /// <param name="settings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [Template("Newtonsoft.Json.JsonConvert.DeserializeObject({value}, {T}, {settings})")]
        public static extern T DeserializeObject<T>(string value, JsonSerializerSettings settings);

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <param name="settings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static extern object DeserializeObject(string value, Type type, JsonSerializerSettings settings);

        /// <summary>
        /// Populates the object with values from the JSON string.
        /// </summary>
        /// <param name="value">The JSON to populate values from.</param>
        /// <param name="target">The target object to populate values onto.</param>
        public static extern void PopulateObject(string value, object target);

        /// <summary>
        /// Populates the object with values from the JSON string using JsonSerializerSettings.
        /// </summary>
        /// <param name="value">The JSON to populate values from.</param>
        /// <param name="target">The target object to populate values onto.</param>
        /// <param name="settings">The JsonSerializerSettings used to deserialize the object. If this is null, default serialization settings will be used.</param>
        public static extern void PopulateObject(string value, object target, JsonSerializerSettings settings);
    }
}