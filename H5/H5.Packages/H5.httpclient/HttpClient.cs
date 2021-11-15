// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static H5.Core.dom;
using static H5.Core.es5;

namespace System.Net.Http
{
    public partial class HttpClient : HttpMessageInvoker
    {
        private static readonly TimeSpan s_defaultTimeout = TimeSpan.FromSeconds(100);
        private static readonly TimeSpan s_maxTimeout = TimeSpan.FromMilliseconds(int.MaxValue);
        private static readonly TimeSpan s_infiniteTimeout = new TimeSpan(0, 0, 0, 0, Threading.Timeout.Infinite);

        private volatile bool _operationStarted;
        private volatile bool _disposed;

        private CancellationTokenSource _pendingRequestsCts;
        private HttpRequestHeaders _defaultRequestHeaders;

        private Uri _baseAddress;
        private TimeSpan _timeout;

        private static Regex _absoluteUrl = new Regex(@"^(?:[a-z]+:)?\/\/", RegexOptions.IgnoreCase);

        private static bool IsAbsoluteUri(Uri uri) => _absoluteUrl.IsMatch(uri.ToString());

        public HttpRequestHeaders DefaultRequestHeaders
        {
            get
            {

                if (_defaultRequestHeaders is null)
                {
                    _defaultRequestHeaders = new HttpRequestHeaders(null);
                }
                return _defaultRequestHeaders;
            }
        }

        public Uri BaseAddress
        {
            get => _baseAddress;
            set
            {
                // It's OK to not have a base address specified, but if one is, it needs to be absolute.
                if (value is object && !IsAbsoluteUri(value))
                {
                    throw new ArgumentException("The base address must be an absolute URI.", nameof(value));
                }

                CheckDisposedOrStarted();

                _baseAddress = value;
            }
        }

        public TimeSpan Timeout
        {
            get => _timeout;
            set
            {
                if (value != s_infiniteTimeout && (value <= TimeSpan.Zero || value > s_maxTimeout))
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                CheckDisposedOrStarted();
                _timeout = value;
            }
        }

        public HttpClient() : this(new HttpClientHandler())
        {
        }

        public HttpClient(HttpMessageHandler handler) : base(handler)
        {
            _timeout = s_defaultTimeout;
            _pendingRequestsCts = new CancellationTokenSource();
        }

        public Task<string> GetStringAsync(string requestUri) =>
            GetStringAsync(CreateUri(requestUri));

        public Task<string> GetStringAsync(Uri requestUri) =>
            GetStringAsync(requestUri, CancellationToken.None);

        public Task<string> GetStringAsync(string requestUri, CancellationToken cancellationToken) =>
            GetStringAsync(CreateUri(requestUri), cancellationToken);

        public Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = CreateRequestMessage(HttpMethod.Get, requestUri);
            // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
            CheckRequestBeforeSend(request);
            return GetStringAsyncCore(request, cancellationToken);
        }

        private async Task<string> GetStringAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            (CancellationTokenSource cts, bool disposeCts, CancellationTokenSource pendingRequestsCts) = PrepareCancellationTokenSource(cancellationToken);
            HttpResponseMessage response = null;
            try
            {
                // Wait for the response message and make sure it completed successfully.
                request.ResponseType = XMLHttpRequestResponseType.text;
                response = await base.SendAsync(request, cts.Token);
                ThrowForNullResponse(response);
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsString();
            }
            finally
            {
                FinishSend(cts, disposeCts);
            }
        }

        public Task<ArrayBuffer> GetByteArrayAsync(string requestUri) =>
            GetByteArrayAsync(CreateUri(requestUri));

        public Task<ArrayBuffer> GetByteArrayAsync(Uri requestUri) =>
            GetByteArrayAsync(requestUri, CancellationToken.None);

        public Task<ArrayBuffer> GetByteArrayAsync(string requestUri, CancellationToken cancellationToken) =>
            GetByteArrayAsync(CreateUri(requestUri), cancellationToken);

        public Task<ArrayBuffer> GetByteArrayAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = CreateRequestMessage(HttpMethod.Get, requestUri);

            // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
            CheckRequestBeforeSend(request);

            return GetByteArrayAsyncCore(request, cancellationToken);
        }

        private async Task<ArrayBuffer> GetByteArrayAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            (CancellationTokenSource cts, bool disposeCts, CancellationTokenSource pendingRequestsCts) = PrepareCancellationTokenSource(cancellationToken);
            HttpResponseMessage response = null;
            try
            {
                request.ResponseType = XMLHttpRequestResponseType.arraybuffer;
                // Wait for the response message and make sure it completed successfully.
                response = await base.SendAsync(request, cts.Token);
                ThrowForNullResponse(response);
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsArrayBuffer();
            }
            finally
            {
                FinishSend(cts, disposeCts);
            }
        }

        public Task<Blob> GetBlobAsync(string requestUri) =>
            GetBlobAsync(CreateUri(requestUri));

        public Task<Blob> GetBlobAsync(string requestUri, CancellationToken cancellationToken) =>
            GetBlobAsync(CreateUri(requestUri), cancellationToken);

        public Task<Blob> GetBlobAsync(Uri requestUri) =>
            GetBlobAsync(requestUri, CancellationToken.None);

        public Task<Blob> GetBlobAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = CreateRequestMessage(HttpMethod.Get, requestUri);

            // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
            CheckRequestBeforeSend(request);

            return GetBlobAsyncCore(request, cancellationToken);
        }

        private async Task<Blob> GetBlobAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            (CancellationTokenSource cts, bool disposeCts, CancellationTokenSource pendingRequestsCts) = PrepareCancellationTokenSource(cancellationToken);
            HttpResponseMessage response = null;
            try
            {
                // Wait for the response message and make sure it completed successfully.
                request.ResponseType = XMLHttpRequestResponseType.blob;
                response = await base.SendAsync(request, cts.Token);
                ThrowForNullResponse(response);
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsBlob();
            }
            finally
            {
                FinishSend(cts, disposeCts);
            }
        }


        public Task<T> GetObjectLiteralAsync<T>(string requestUri) =>
            GetObjectLiteralAsync<T>(CreateUri(requestUri));

        public Task<T> GetObjectLiteralAsync<T>(string requestUri, CancellationToken cancellationToken) =>
            GetObjectLiteralAsync<T>(CreateUri(requestUri), cancellationToken);

        public Task<T> GetObjectLiteralAsync<T>(Uri requestUri) =>
            GetObjectLiteralAsync<T>(requestUri, CancellationToken.None);

        public Task<T> GetObjectLiteralAsync<T>(Uri requestUri, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = CreateRequestMessage(HttpMethod.Get, requestUri);

            // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
            CheckRequestBeforeSend(request);

            return GetObjectLiteralAsyncCore<T>(request, cancellationToken);
        }

        private async Task<T> GetObjectLiteralAsyncCore<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            (CancellationTokenSource cts, bool disposeCts, CancellationTokenSource pendingRequestsCts) = PrepareCancellationTokenSource(cancellationToken);
            HttpResponseMessage response = null;
            try
            {
                request.ResponseType = XMLHttpRequestResponseType.json;
                // Wait for the response message and make sure it completed successfully.
                response = await base.SendAsync(request, cts.Token);
                ThrowForNullResponse(response);
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsObjectLiteral<T>();
            }
            finally
            {
                FinishSend(cts, disposeCts);
            }
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri) =>
            GetAsync(CreateUri(requestUri));

        public Task<HttpResponseMessage> GetAsync(Uri requestUri) =>
            GetAsync(requestUri);

        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken) =>
            GetAsync(CreateUri(requestUri), cancellationToken);

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken) =>
            SendAsync(CreateRequestMessage(HttpMethod.Get, requestUri), cancellationToken);

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) =>
            PostAsync(CreateUri(requestUri), content);

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content) =>
            PostAsync(requestUri, content, CancellationToken.None);

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) =>
            PostAsync(CreateUri(requestUri), content, cancellationToken);

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = CreateRequestMessage(HttpMethod.Post, requestUri);
            request.Content = content;
            return SendAsync(request, cancellationToken);
        }

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content) =>
            PutAsync(CreateUri(requestUri), content);

        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content) =>
            PutAsync(requestUri, content, CancellationToken.None);

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) =>
            PutAsync(CreateUri(requestUri), content, cancellationToken);

        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = CreateRequestMessage(HttpMethod.Put, requestUri);
            request.Content = content;
            return SendAsync(request, cancellationToken);
        }

        public Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content) =>
            PatchAsync(CreateUri(requestUri), content);

        public Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content) =>
            PatchAsync(requestUri, content, CancellationToken.None);

        public Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) =>
            PatchAsync(CreateUri(requestUri), content, cancellationToken);

        public Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = CreateRequestMessage(HttpMethod.Patch, requestUri);
            request.Content = content;
            return SendAsync(request, cancellationToken);
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestUri) =>
            DeleteAsync(CreateUri(requestUri));

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri) =>
            DeleteAsync(requestUri, CancellationToken.None);

        public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken) =>
            DeleteAsync(CreateUri(requestUri), cancellationToken);

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken) =>
            SendAsync(CreateRequestMessage(HttpMethod.Delete, requestUri), cancellationToken);

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) =>
            SendAsync(request, CancellationToken.None);

        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
            CheckRequestBeforeSend(request);
            (CancellationTokenSource cts, bool disposeCts, CancellationTokenSource pendingRequestsCts) = PrepareCancellationTokenSource(cancellationToken);

            return Core();

            async Task<HttpResponseMessage> Core()
            {
                HttpResponseMessage response;
                try
                {
                    // Wait for the send request to complete, getting back the response.
                    response = await base.SendAsync(request, cts.Token);
                    ThrowForNullResponse(response);
                    return response;
                }
                finally
                {
                    FinishSend(cts, disposeCts);
                }
            }
        }

        private void CheckRequestBeforeSend(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CheckDisposed();
            CheckRequestMessage(request);

            SetOperationStarted();

            // PrepareRequestMessage will resolve the request address against the base address.
            PrepareRequestMessage(request);
        }

        private static void ThrowForNullResponse(HttpResponseMessage response)
        {
            if (response is null)
            {
                throw new InvalidOperationException("Handler did not return a response message.");
            }
        }

        private static void FinishSend(CancellationTokenSource cts, bool disposeCts)
        {
            // Dispose of the CancellationTokenSource if it was created specially for this request
            // rather than being used across multiple requests.
            if (disposeCts)
            {
                cts.Dispose();
            }
        }

        public void CancelPendingRequests()
        {
            CheckDisposed();

            // With every request we link this cancellation token source.
            var currentCts = _pendingRequestsCts;
            _pendingRequestsCts= new CancellationTokenSource();

            currentCts.Cancel();
            currentCts.Dispose();
        }

        public override void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                // Cancel all pending requests (if any). Note that we don't call CancelPendingRequests() but cancel
                // the CTS directly. The reason is that CancelPendingRequests() would cancel the current CTS and create
                // a new CTS. We don't want a new CTS in this case.
                _pendingRequestsCts.Cancel();
                _pendingRequestsCts.Dispose();
            }

            base.Dispose();
        }

        private void SetOperationStarted()
        {
            // This method flags the HttpClient instances as "active". I.e. we executed at least one request (or are
            // in the process of doing so). This information is used to lock-down all property setters. Once a
            // Send/SendAsync operation started, no property can be changed.
            if (!_operationStarted)
            {
                _operationStarted = true;
            }
        }

        private void CheckDisposedOrStarted()
        {
            CheckDisposed();
            if (_operationStarted)
            {
                throw new InvalidOperationException("This instance has already started one or more requests. Properties can only be modified before sending the first request.");
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().ToString());
            }
        }

        private static void CheckRequestMessage(HttpRequestMessage request)
        {
            if (!request.MarkAsSent())
            {
                throw new InvalidOperationException("The request message was already sent. Cannot send the same request message multiple times.");
            }
        }

        private void PrepareRequestMessage(HttpRequestMessage request)
        {
            Uri requestUri = null;
            if ((request.RequestUri == null) && (_baseAddress == null))
            {
                throw new InvalidOperationException("An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set.");
            }
            if (request.RequestUri == null)
            {
                requestUri = _baseAddress;
            }
            else
            {
                // If the request Uri is an absolute Uri, just use it. Otherwise try to combine it with the base Uri.
                if (!IsAbsoluteUri(request.RequestUri))
                {
                    if (_baseAddress == null)
                    {
                        throw new InvalidOperationException("An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set.");
                    }
                    else
                    {
                        requestUri = new Uri(_baseAddress.ToString() + request.RequestUri.ToString());
                    }
                }
            }

            // We modified the original request Uri. Assign the new Uri to the request message.
            if (requestUri != null)
            {
                request.RequestUri = requestUri;
            }

            // Add default headers
            if (_defaultRequestHeaders != null)
            {
                request.Headers.AddHeaders(_defaultRequestHeaders);
            }
        }

        private (CancellationTokenSource TokenSource, bool DisposeTokenSource, CancellationTokenSource PendingRequestsCts) PrepareCancellationTokenSource(CancellationToken cancellationToken)
        {
            // We need a CancellationTokenSource to use with the request.  We always have the global
            // _pendingRequestsCts to use, plus we may have a token provided by the caller, and we may
            // have a timeout.  If we have a timeout or a caller-provided token, we need to create a new
            // CTS (we can't, for example, timeout the pending requests CTS, as that could cancel other
            // unrelated operations).  Otherwise, we can use the pending requests CTS directly.

            // Snapshot the current pending requests cancellation source. It can change concurrently due to cancellation being requested
            // and it being replaced, and we need a stable view of it: if cancellation occurs and the caller's token hasn't been canceled,
            // it's either due to this source or due to the timeout, and checking whether this source is the culprit is reliable whereas
            // it's more approximate checking elapsed time.
            CancellationTokenSource pendingRequestsCts = _pendingRequestsCts;

            bool hasTimeout = _timeout != s_infiniteTimeout;
            if (hasTimeout || cancellationToken.CanBeCanceled)
            {
                CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, pendingRequestsCts.Token);
                if (hasTimeout)
                {
                    cts.CancelAfter(_timeout);
                }

                return (cts, DisposeTokenSource: true, pendingRequestsCts);
            }

            return (pendingRequestsCts, DisposeTokenSource: false, pendingRequestsCts);
        }

        private Uri CreateUri(string uri) => string.IsNullOrEmpty(uri) ? null : new Uri(uri);

        private HttpRequestMessage CreateRequestMessage(HttpMethod method, Uri uri) => new HttpRequestMessage(method, uri);
    }
}