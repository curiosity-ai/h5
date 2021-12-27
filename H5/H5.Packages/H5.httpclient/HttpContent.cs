// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static H5.Core.dom;
using static H5.Core.es5;

namespace System.Net.Http
{
    public abstract class HttpContent
    {
        internal XMLHttpRequest _request;
        private HttpContentHeaders _headers;

        public HttpContentHeaders Headers 
        {
            get
            {
                if (_headers == null)
                {
                    _headers = new HttpContentHeaders(this);
                }
                return _headers;
            }
        }

        internal HttpContent()
        {
        }

        internal HttpContent(XMLHttpRequest request)
        {
            _request = request;
        }

        internal long? GetComputedOrBufferLength()
        {
            return null;
        }

        public string ReadAsString() => _request.responseText;
        public ArrayBuffer ReadAsArrayBuffer() => _request.response.As<ArrayBuffer>();
        public Blob ReadAsBlob() => _request.response.As<Blob>();
        public T ReadAsObjectLiteral<T>() => _request.response.As<T>();
    }
}