﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System
{
    [H5.NonScriptable]
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed class NonSerializedAttribute : Attribute
    {
        public NonSerializedAttribute()
        {
        }
    }
}
