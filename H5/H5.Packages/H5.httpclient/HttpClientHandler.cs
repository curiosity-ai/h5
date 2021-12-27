// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using HttpHandlerType = System.Net.Http.BrowserHttpHandler;

namespace System.Net.Http
{
    public partial class HttpClientHandler : HttpMessageHandler
    {
        private readonly HttpHandlerType _underlyingHandler;

        private HttpMessageHandler Handler { get; }

        public HttpClientHandler()
        {
            _underlyingHandler = new HttpHandlerType();

            Handler = _underlyingHandler;
        }

        //TODO: Can we support ICredentials?

        //public ICredentials? Credentials
        //{
        //    get => _underlyingHandler.Credentials;
        //    set => _underlyingHandler.Credentials = value;
        //}

        public bool AllowAutoRedirect
        {
            get => _underlyingHandler.AllowAutoRedirect;
            set => _underlyingHandler.AllowAutoRedirect = value;
        }

        public int MaxAutomaticRedirections
        {
            get => _underlyingHandler.MaxAutomaticRedirections;
            set => _underlyingHandler.MaxAutomaticRedirections = value;
        }

        protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Handler.SendAsync(request, cancellationToken);
    }
}