// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public class HttpMessageInvoker : IDisposable
    {
        private readonly HttpMessageHandler _handler;

        public HttpMessageInvoker(HttpMessageHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            _handler = handler;
        }

        public virtual void Dispose()
        {
        }

        public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _handler.SendAsync(request, cancellationToken);
        }
    }
}