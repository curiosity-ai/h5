// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace System.Collections
{
    // An IEqualityComparer is a mechanism to consume custom performant comparison infrastructure
    // that can be consumed by some of the common collections.
    public interface IEqualityComparer
    {
        [H5.Convention(H5.Notation.CamelCase)]
        bool Equals(object x, object y);
        [H5.Convention(H5.Notation.CamelCase)]
        int GetHashCode(object obj);
    }
}
