namespace System.ComponentModel
{
    using System;

    /// <summary>
    /// Specifies whether a property or event should be displayed in a Properties window.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.All)]
    public sealed class BrowsableAttribute : Attribute
    {
        /// <summary>
        /// Specifies that a property or event can be modified at design time. This field is read-only.
        /// </summary>
        public static readonly BrowsableAttribute Yes = new BrowsableAttribute(true);

        /// <summary>
        /// Specifies that a property or event cannot be modified at design time. This field is read-only.
        /// </summary>
        public static readonly BrowsableAttribute No = new BrowsableAttribute(false);

        /// <summary>
        /// Specifies the default value for the BrowsableAttribute, which is BrowsableAttribute.Yes. This field is read-only.
        /// </summary>
        public static readonly BrowsableAttribute Default = Yes;

        private bool browsable = true;

        /// <summary>
        /// Initializes a new instance of the BrowsableAttribute class.
        /// </summary>
        /// <param name="browsable"></param>
        public BrowsableAttribute(bool browsable)
        {
            this.browsable = browsable;
        }

        /// <summary>
        /// Gets a value indicating whether an object is browsable.
        /// </summary>
        public bool Browsable
        {
            get
            {
                return browsable;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }


            return (obj is BrowsableAttribute other) && other.Browsable == browsable;
        }

        public override int GetHashCode()
        {
            return browsable.GetHashCode();
        }
    }
}