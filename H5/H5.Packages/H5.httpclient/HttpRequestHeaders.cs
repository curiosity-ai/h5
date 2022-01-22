// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using static H5.Core.dom;

namespace System.Net.Http.Headers
{
    public sealed class HttpRequestHeaders : HttpHeaders
    {
        internal HttpRequestHeaders() : base(null)
        {
        }

        internal override void AddHeaders(HttpHeaders sourceHeaders)
        {
            base.AddHeaders(sourceHeaders);
        }
    }
}