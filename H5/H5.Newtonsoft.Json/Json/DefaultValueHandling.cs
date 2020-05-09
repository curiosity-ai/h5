using System;
using System.ComponentModel;

namespace Newtonsoft.Json
{
    /// <summary>
    /// Specifies default value handling options.
    /// </summary>
    [Flags]
    public enum DefaultValueHandling
    {
        /// <summary>
        /// Include members where the member value is the same as the member's default value when serializing objects.
        /// Included members are written to JSON. Has no effect when deserializing.
        /// </summary>
        Include = 0,

        /// <summary>
        /// Ignore members where the member value is the same as the member's default value when serializing objects
        /// so that it is not written to JSON.
        /// This option will ignore all default values (e.g. <c>null</c> for objects and nullable types; <c>0</c> for integers,
        /// decimals and floating point numbers; and <c>false</c> for booleans). 
        /// </summary>
        Ignore = 1,

        /// <summary>
        /// Members with a default value but no JSON will be set to their default value when deserializing.
        /// </summary>
        Populate = 2,

        /// <summary>
        /// Ignore members where the member value is the same as the member's default value when serializing objects
        /// and set members to their default value when deserializing.
        /// </summary>
        IgnoreAndPopulate = Ignore | Populate
    }
}