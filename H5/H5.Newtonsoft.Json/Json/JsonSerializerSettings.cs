using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
    /// <summary>
    /// Specifies the settings on a <see cref="JsonConvert"/> object.
    /// </summary>
    public class JsonSerializerSettings
    {
        internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;
        internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;
        internal DefaultValueHandling? _defaultValueHandling;
        internal TypeNameHandling? _typeNameHandling;
        internal NullValueHandling? _nullValueHandling;
        internal ObjectCreationHandling? _objectCreationHandling;

        /// <summary>
        /// Gets or sets how null values are handled during serialization and deserialization.
        /// </summary>
        /// <value>Null value handling.</value>
        public NullValueHandling NullValueHandling
        {
            get
            {
                return _nullValueHandling ?? DefaultNullValueHandling;
            }
            set
            {
                _nullValueHandling = value;
            }
        }

        public ObjectCreationHandling ObjectCreationHandling
        {
            get { return _objectCreationHandling ?? default(ObjectCreationHandling); }
            set { _objectCreationHandling = value; }
        }

        /// <summary>
        /// Gets or sets the default value handling used when serializing a property.
        /// </summary>
        /// <value>The default value handling.</value>
        public DefaultValueHandling DefaultValueHandling
        {
            get { return _defaultValueHandling ?? default(DefaultValueHandling); }
            set { _defaultValueHandling = value; }
        }

        /// <summary>
        /// Gets or sets how type name writing and reading is handled by the serializer.
        /// </summary>
        /// <remarks>
        /// <see cref="JsonSerializerSettings.TypeNameHandling"/> should be used with caution when your application deserializes JSON from an external source.
        /// Incoming types should be validated with a custom SerializationBinder />
        /// when deserializing with a value other than <see cref="TypeNameHandling.None"/>.
        /// </remarks>
        /// <value>The type name handling.</value>
        public TypeNameHandling TypeNameHandling
        {
            get
            {
                return _typeNameHandling ?? DefaultTypeNameHandling;
            }
            set
            {
                _typeNameHandling = value;
            }
        }

        /// <summary>
        /// Gets or sets the contract resolver used by the serializer when
        /// serializing .NET objects to JSON and vice versa.
        /// </summary>
        /// <value>The contract resolver.</value>
        public IContractResolver ContractResolver
        {
            get; set;
        }

        public ISerializationBinder SerializationBinder
        {
            get; set;
        }
    }
}