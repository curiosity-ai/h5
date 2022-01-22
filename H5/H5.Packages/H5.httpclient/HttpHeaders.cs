// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Collections.Generic;
using static H5.Core.dom;

namespace System.Net.Http.Headers
{
    public abstract class HttpHeaders : IEnumerable<KeyValuePair<string, string>>
    {
        private Dictionary<string, string> _headerStore;

        internal readonly XMLHttpRequest _request;

        protected HttpHeaders(XMLHttpRequest request = null)
        {
            _request = request;
        }

        internal Dictionary<string, string> HeaderStore => _headerStore;


        internal void ApplyHeadersToRequest(XMLHttpRequest request)
        {
            if (_headerStore is object)
            {
                foreach(var kv in _headerStore)
                {
                    request.setRequestHeader(kv.Key, kv.Value);
                }
            }
        }

        public void Add(string descriptor, string value)
        {
            if (_headerStore is null) _headerStore = new Dictionary<string, string>();
            _headerStore.Add(descriptor, value);
        }

        public void Clear() => _headerStore?.Clear();

        public bool Contains(string descriptor)
        {
            return _headerStore != null && _headerStore.ContainsKey(descriptor);
        }

        internal string GetHeaderString(string descriptor)
        {
            if (_request is object) return _request.getResponseHeader(descriptor);
            if (this._headerStore is object) return _headerStore.TryGetValue(descriptor, out var val) ? val : "";
            return "";
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _headerStore != null && _headerStore.Count > 0 ?
                GetEnumeratorCore() :
                ((IEnumerable<KeyValuePair<string, string>>)Array.Empty<KeyValuePair<string, string>>()).GetEnumerator();

        private IEnumerator<KeyValuePair<string, string>> GetEnumeratorCore()
        {
            foreach (KeyValuePair<string, string> header in _headerStore)
            {
                yield return header;
            }
        }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() => GetEnumerator();


        internal void SetOrRemoveParsedValue(string descriptor, string value)
        {
            if (value == null)
            {
                Remove(descriptor);
            }
            else
            {
                Add(descriptor, value);
            }
        }

        public bool Remove(string descriptor) => _headerStore != null && _headerStore.Remove(descriptor);

        internal bool TryGetHeaderValue(string descriptor, out string value)
        {
            if (_headerStore == null)
            {
                value = null;
                return false;
            }

            return _headerStore.TryGetValue(descriptor, out value);
        }

        internal virtual void AddHeaders(HttpHeaders sourceHeaders)
        {
            foreach(var kv in sourceHeaders)
            {
                Add(kv.Key, kv.Value);
            }
        }
    }
}