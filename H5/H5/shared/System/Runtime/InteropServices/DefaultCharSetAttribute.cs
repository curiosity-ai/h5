// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Runtime.InteropServices
{
    [H5.NonScriptable]
    [AttributeUsage(AttributeTargets.Module, Inherited = false)]
    public sealed class DefaultCharSetAttribute : Attribute
    {
        public DefaultCharSetAttribute(CharSet charSet)
        {
            CharSet = charSet;
        }

        public CharSet CharSet { get; }
    }
}
