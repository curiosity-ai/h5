using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static H5.Core.dom;
using static H5.Core.es5;

namespace System.Net.Http
{
    internal sealed class BrowserHttpHandler : HttpMessageHandler
    {
        public bool AllowAutoRedirect { get; set; } = true;
        public int MaxAutomaticRedirections { get; set; } = 50;

        protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => SendAsync(request, MaxAutomaticRedirections, cancellationToken);

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, int redirectCount, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestObject = request._request;

            requestObject.open(request.Method.Method, request.RequestUri.AbsoluteUri);

            if (request.Content != null)
            {
                request.Headers.AddHeaders(request.Content.Headers);
            }

            var abortCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            var abortRegistration = abortCts.Token.Register(() =>
            {
                requestObject.abort();
                abortCts.Dispose();
            });

            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            requestObject.onreadystatechange = e =>
            {
                if (requestObject.readyState == 0)
                {
                    tcs.TrySetCanceled();
                    tcs = null;
                    return;
                }

                if (requestObject.readyState == 4 /*AjaxReadyState.Done*/)
                {
                    if (requestObject.status == 302)
                    {
                        if (redirectCount > 0)
                        {
                            redirectCount--;
                            Task.Run(async () =>
                            {
                                try
                                {
                                    request.MarkAsRedirected();
                                    var location = requestObject.getResponseHeader("Location");
                                    var response = await SendAsync(request, redirectCount, cancellationToken);
                                    tcs.TrySetResult(response);
                                }
                                catch (Exception E)
                                {
                                    tcs.TrySetException(E);
                                }
                            });
                        }
                        else
                        {
                            tcs.TrySetException(new Exception("Maximum number of redirects hit"));
                        }
                    }

                    var httpResponse = new HttpResponseMessage((HttpStatusCode)requestObject.status, requestObject);
                    httpResponse.Content = new BrowserHttpContent(requestObject);

                    if (requestObject.status >= 200 && requestObject.status < 300)
                    {
                        tcs.TrySetResult(httpResponse);
                    }
                }
            };

            if (request.Content is object)
            {
                if (request.Content is StringContent stringContent)
                {
                    requestObject.send(stringContent.Content);
                }
                else if (request.Content is FormContent formContent)
                {
                    requestObject.send(formContent.Content);
                }
            }
            else
            {
                requestObject.send();
            }

            return await tcs.Task;
        }

        private sealed class BrowserHttpContent : HttpContent
        {
            public BrowserHttpContent(XMLHttpRequest request) : base(request)
            {
            }
        }
    }
}