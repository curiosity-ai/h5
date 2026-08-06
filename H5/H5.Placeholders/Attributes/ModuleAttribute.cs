// Original: H5/H5/Attributes/ModuleAttribute.cs
using System;

namespace H5
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Interface)]
    public sealed class ModuleAttribute : Attribute
    {
        public ModuleAttribute()
        {
        }

        public ModuleAttribute(bool preventModuleName)
        {
        }

        public ModuleAttribute(string moduleName)
        {
        }

        public ModuleAttribute(string moduleName, bool preventModuleName)
        {
        }

        public ModuleAttribute(ModuleType type)
        {
        }

        public ModuleAttribute(ModuleType type, bool preventModuleName)
        {
        }

        public ModuleAttribute(ModuleType type, string moduleName)
        {
        }

        public ModuleAttribute(ModuleType type, string moduleName, bool preventModuleName)
        {
        }

        public string ExportAsNamespace
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
    }

    /// <summary>
    /// Retained for source compatibility with the historical <c>[Module]</c> attribute.
    /// The packaging of generated JavaScript is now driven by <c>OutputModuleType</c> in
    /// <c>h5.json</c> (either the default <c>H5.define</c> output or modern ES modules
    /// emitted as <c>.mjs</c>), so <c>[Module]</c> only carries naming information.
    /// </summary>
    public enum ModuleType
    {
        /// <summary>Default / unspecified.</summary>
        ESM = 0,
    }
}
