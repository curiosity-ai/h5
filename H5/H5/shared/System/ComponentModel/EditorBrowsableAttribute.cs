// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.ComponentModel
{
    [H5.NonScriptable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Delegate | AttributeTargets.Interface)]
    public sealed class EditorBrowsableAttribute : Attribute
    {
        private EditorBrowsableState browsableState;

        public EditorBrowsableAttribute(EditorBrowsableState state)
        {
            browsableState = state;
        }

        public EditorBrowsableAttribute() : this(EditorBrowsableState.Always) { }

        public EditorBrowsableState State
        {
            get { return browsableState; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }


            return (obj is EditorBrowsableAttribute other) && other.browsableState == browsableState;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [H5.NonScriptable]
    [H5.Enum(H5.Emit.Value)]
    public enum EditorBrowsableState
    {
        Always,
        Never,
        Advanced
    }
}
