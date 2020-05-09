using System;

namespace Newtonsoft.Json
{
    /// <summary>
    /// Instructs the JsonConvert to always serialize the member with the specified name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        internal NullValueHandling? _nullValueHandling;
        internal DefaultValueHandling? _defaultValueHandling;
        internal ObjectCreationHandling? _objectCreationHandling;
        internal TypeNameHandling? _typeNameHandling;
        internal Required? _required;
        internal int? _order;

        /// <summary>
        /// Gets or sets the null value handling used when serializing this property.
        /// </summary>
        /// <value>The null value handling.</value>
        public NullValueHandling NullValueHandling
        {
            get { return _nullValueHandling ?? default(NullValueHandling); }
            set { _nullValueHandling = value; }
        }

        /// <summary>
        /// Gets or sets the default value handling used when serializing this property.
        /// </summary>
        /// <value>The default value handling.</value>
        public DefaultValueHandling DefaultValueHandling
        {
            get { return _defaultValueHandling ?? default(DefaultValueHandling); }
            set { _defaultValueHandling = value; }
        }

        /// <summary>
        /// Gets or sets the object creation handling used when deserializing this property.
        /// </summary>
        /// <value>The object creation handling.</value>
        public ObjectCreationHandling ObjectCreationHandling
        {
            get { return _objectCreationHandling ?? default(ObjectCreationHandling); }
            set { _objectCreationHandling = value; }
        }

        /// <summary>
        /// Gets or sets the type name handling used when serializing this property.
        /// </summary>
        /// <value>The type name handling.</value>
        public TypeNameHandling TypeNameHandling
        {
            get { return _typeNameHandling ?? default(TypeNameHandling); }
            set { _typeNameHandling = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this property is required.
        /// </summary>
        /// <value>
        /// 	A value indicating whether this property is required.
        /// </value>
        public Required Required
        {
            get { return _required ?? Required.Default; }
            set { _required = value; }
        }

        /// <summary>
        /// Gets or sets the order of serialization of a member.
        /// </summary>
        /// <value>The numeric order of serialization.</value>
        public int Order
        {
            get { return _order ?? default(int); }
            set { _order = value; }
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonPropertyAttribute"/> class.
        /// </summary>
        public JsonPropertyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonPropertyAttribute"/> class with the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public JsonPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }
    }
}