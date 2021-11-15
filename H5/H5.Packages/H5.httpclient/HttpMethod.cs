// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http
{
    public partial class HttpMethod : IEquatable<HttpMethod>
    {
        private readonly string _method;

        private int _hashcode;

        private static readonly HttpMethod s_getMethod = new HttpMethod("GET");
        private static readonly HttpMethod s_putMethod = new HttpMethod("PUT");
        private static readonly HttpMethod s_postMethod = new HttpMethod("POST");
        private static readonly HttpMethod s_deleteMethod = new HttpMethod("DELETE");
        private static readonly HttpMethod s_headMethod = new HttpMethod("HEAD");
        private static readonly HttpMethod s_optionsMethod = new HttpMethod("OPTIONS");
        private static readonly HttpMethod s_traceMethod = new HttpMethod("TRACE");
        private static readonly HttpMethod s_patchMethod = new HttpMethod("PATCH");

        public static HttpMethod Get => s_getMethod;

        public static HttpMethod Put => s_putMethod;

        public static HttpMethod Post => s_postMethod;

        public static HttpMethod Delete => s_deleteMethod;

        public static HttpMethod Head => s_headMethod;

        public static HttpMethod Options => s_optionsMethod;

        public static HttpMethod Trace => s_traceMethod;

        public static HttpMethod Patch => s_patchMethod;

        public string Method => _method;

        private HttpMethod(string method)
        {
            _method = method;
        }

        #region IEquatable<HttpMethod> Members

        public bool Equals(HttpMethod other)
        {
            if (other is null)
            {
                return false;
            }

            return string.Equals(_method, other._method, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return Equals(obj as HttpMethod);
        }

        public override int GetHashCode()
        {
            if (_hashcode == 0)
            {
                _hashcode = StringComparer.OrdinalIgnoreCase.GetHashCode(_method);
            }

            return _hashcode;
        }

        public override string ToString()
        {
            return _method;
        }

        public static bool operator ==(HttpMethod left, HttpMethod right)
        {
            return left is null || right is null ?
                ReferenceEquals(left, right) :
                left.Equals(right);
        }

        public static bool operator !=(HttpMethod left, HttpMethod right)
        {
            return !(left == right);
        }

        internal bool MustHaveRequestBody
        {
            get
            {
                return !ReferenceEquals(this, HttpMethod.Get) && !ReferenceEquals(this, HttpMethod.Head) &&
                       !ReferenceEquals(this, HttpMethod.Options) && !ReferenceEquals(this, HttpMethod.Delete);
            }
        }
    }
}