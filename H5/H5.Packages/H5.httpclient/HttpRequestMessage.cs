// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using static H5.Core.dom;

namespace System.Net.Http
{
    public class HttpRequestMessage : IDisposable
    {
        private const int MessageNotYetSent = 0;
        private const int MessageAlreadySent = 1;
        private const int MessageIsRedirect = 2;

        // Track whether the message has been sent.
        // The message shouldn't be sent again if this field is equal to MessageAlreadySent.
        private int _sendStatus = MessageNotYetSent;

        private XMLHttpRequestResponseType _responseType;
        private HttpMethod _method;
        private Uri _requestUri;
        private HttpRequestHeaders _headers;
        private Version _version;
        private HttpContent _content;
        private bool _disposed;
        private HttpRequestOptions _options;

        internal XMLHttpRequest _request = new XMLHttpRequest();

        internal XMLHttpRequestResponseType ResponseType { set { CheckDisposed(); _responseType = value; } }

        public Version Version
        {
            get { return _version; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                CheckDisposed();

                _version = value;
            }
        }

        public HttpContent Content
        {
            get { return _content; }
            set
            {
                CheckDisposed();
                // It's OK to set a 'null' content, even if the method is POST/PUT.
                _content = value;
            }
        }

        public HttpMethod Method
        {
            get { return _method; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                CheckDisposed();

                _method = value;
            }
        }

        public Uri RequestUri
        {
            get { return _requestUri; }
            set
            {
                CheckDisposed();
                _requestUri = value;
            }
        }

        public HttpRequestHeaders Headers
        {
            get
            {
                if (_headers is null) _headers = new HttpRequestHeaders();
                return _headers;
            }
        }

        internal bool HasHeaders => _headers != null;

        /// <summary>
        /// Gets the collection of options to configure the HTTP request.
        /// </summary>
        public HttpRequestOptions Options
        {
            get
            {
                if (_options is null) _options = new HttpRequestOptions();
                return _options;
            }
        }

        public HttpRequestMessage() : this(HttpMethod.Get, (Uri)null)
        {
        }

        public HttpRequestMessage(HttpMethod method, Uri requestUri)
        {
            // It's OK to have a 'null' request Uri. If HttpClient is used, the 'BaseAddress' will be added.
            // If there is no 'BaseAddress', sending this request message will throw.
            // Note that we also allow the string to be empty: null and empty are considered equivalent.
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _requestUri = requestUri;
        }

        public HttpRequestMessage(HttpMethod method, string requestUri)
            : this(method, string.IsNullOrEmpty(requestUri) ? null : new Uri(requestUri))
        {
        }

        internal bool MarkAsSent()
        {
            if(_sendStatus == MessageNotYetSent)
            {
                _sendStatus = MessageAlreadySent;
                return true;
            }
            return false;
        }

        internal bool WasSentByHttpClient() => (_sendStatus & MessageAlreadySent) != 0;

        internal void MarkAsRedirected() => _sendStatus |= MessageIsRedirect;

        internal bool WasRedirected() => (_sendStatus & MessageIsRedirect) != 0;

        public void Dispose()
        {
            _disposed = true;
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }
        }
    }
}