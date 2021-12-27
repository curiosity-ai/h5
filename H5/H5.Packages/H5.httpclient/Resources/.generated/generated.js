    H5.define("System.Net.Http.HttpMessageHandler", {
        ctors: {
            ctor: function () {
                this.$initialize();
            }
        }
    });

    H5.define("System.Net.Http.HttpContent", {
        fields: {
            _request: null,
            _headers: null
        },
        props: {
            Headers: {
                get: function () {
                    if (this._headers == null) {
                        this._headers = new System.Net.Http.Headers.HttpContentHeaders(this);
                    }
                    return this._headers;
                }
            }
        },
        ctors: {
            ctor: function () {
                this.$initialize();
            },
            $ctor1: function (request) {
                this.$initialize();
                this._request = request;
            }
        },
        methods: {
            GetComputedOrBufferLength: function () {
                return System.Int64.lift(null);
            },
            ReadAsString: function () {
                return this._request.responseText;
            },
            ReadAsArrayBuffer: function () {
                return H5.unbox(this._request.response);
            },
            ReadAsBlob: function () {
                return H5.unbox(this._request.response);
            },
            ReadAsObjectLiteral: function (T) {
                return H5.unbox(this._request.response);
            }
        }
    });

    H5.define("System.Net.Http.Headers.HttpHeaders", {
        inherits: [System.Collections.Generic.IEnumerable$1(System.Collections.Generic.KeyValuePair$2(System.String,System.String))],
        fields: {
            _headerStore: null,
            _request: null
        },
        props: {
            HeaderStore: {
                get: function () {
                    return this._headerStore;
                }
            }
        },
        alias: ["GetEnumerator", ["System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$System$String$System$String$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"]],
        ctors: {
            ctor: function (request) {
                if (request === void 0) { request = null; }

                this.$initialize();
                this._request = request;
            }
        },
        methods: {
            Add: function (descriptor, value) {
                //TODO: How to handle the case where we override something?
                if (H5.is(this._request, System.Object)) {
                    this._request.setRequestHeader(descriptor, value);
                } else {
                    if (this._headerStore == null) {
                        this._headerStore = new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor();
                    }
                    this._headerStore.add(descriptor, value);
                }
            },
            Clear: function () {
                this._headerStore != null ? this._headerStore.clear() : null;
            },
            Contains: function (descriptor) {
                return this._headerStore != null && this._headerStore.containsKey(descriptor);
            },
            GetHeaderString: function (descriptor) {
                if (H5.is(this._request, System.Object)) {
                    return this._request.getResponseHeader(descriptor);
                }
                if (H5.is(this._headerStore, System.Object)) {
                    var val = { };
                    return this._headerStore.tryGetValue(descriptor, val) ? val.v : "";
                }
                return "";
            },
            GetEnumerator: function () {
                return this._headerStore != null && this._headerStore.Count > 0 ? this.GetEnumeratorCore() : H5.getEnumerator(H5.cast(System.Array.init([], System.Collections.Generic.KeyValuePair$2(System.String,System.String)), System.Collections.Generic.IEnumerable$1(System.Collections.Generic.KeyValuePair$2(System.String,System.String))), System.Collections.Generic.KeyValuePair$2(System.String,System.String));
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return this.GetEnumerator();
            },
            GetEnumeratorCore: function () {
                var $s = 0,
                    $jff,
                    $rv,
                    $t,
                    header,
                    $ae;

                var $en = new (H5.GeneratorEnumerator$1(System.Collections.Generic.KeyValuePair$2(System.String,System.String)))(H5.fn.bind(this, function () {
                    try {
                        for (;;) {
                            switch ($s) {
                                case 0: {
                                    $t = H5.getEnumerator(this._headerStore);
                                        $s = 1;
                                        continue;
                                }
                                case 1: {
                                    if ($t.moveNext()) {
                                            header = $t.Current;
                                            $s = 2;
                                            continue;
                                        }
                                    $s = 4;
                                    continue;
                                }
                                case 2: {
                                    $en.current = header;
                                        $s = 3;
                                        return true;
                                }
                                case 3: {
                                    $s = 1;
                                    continue;
                                }
                                case 4: {

                                }
                                default: {
                                    return false;
                                }
                            }
                        }
                    } catch($ae1) {
                        $ae = System.Exception.create($ae1);
                        throw $ae;
                    }
                }));
                return $en;
            },
            SetOrRemoveParsedValue: function (descriptor, value) {
                if (value == null) {
                    this.Remove(descriptor);
                } else {
                    this.Add(descriptor, value);
                }
            },
            Remove: function (descriptor) {
                return this._headerStore != null && this._headerStore.remove(descriptor);
            },
            TryGetHeaderValue: function (descriptor, value) {
                if (this._headerStore == null) {
                    value.v = null;
                    return false;
                }

                return this._headerStore.tryGetValue(descriptor, value);
            },
            AddHeaders: function (sourceHeaders) {
                var $t;
                $t = H5.getEnumerator(sourceHeaders);
                try {
                    while ($t.moveNext()) {
                        var kv = $t.Current;
                        this.Add(kv.key, kv.value);
                    }
                } finally {
                    if (H5.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }
            }
        }
    });

    H5.define("System.Net.Http.HttpMessageInvoker", {
        inherits: [System.IDisposable],
        fields: {
            _handler: null
        },
        alias: ["Dispose", "System$IDisposable$Dispose"],
        ctors: {
            ctor: function (handler) {
                this.$initialize();
                if (handler == null) {
                    throw new System.ArgumentNullException.$ctor1("handler");
                }
                this._handler = handler;
            }
        },
        methods: {
            Dispose: function () { },
            SendAsync: function (request, cancellationToken) {
                if (request == null) {
                    throw new System.ArgumentNullException.$ctor1("request");
                }

                return this._handler.SendAsync(request, cancellationToken);
            }
        }
    });

    H5.define("System.Net.Http.HttpMethod", {
        inherits: function () { return [System.IEquatable$1(System.Net.Http.HttpMethod)]; },
        statics: {
            fields: {
                s_getMethod: null,
                s_putMethod: null,
                s_postMethod: null,
                s_deleteMethod: null,
                s_headMethod: null,
                s_optionsMethod: null,
                s_traceMethod: null,
                s_patchMethod: null
            },
            props: {
                Get: {
                    get: function () {
                        return System.Net.Http.HttpMethod.s_getMethod;
                    }
                },
                Put: {
                    get: function () {
                        return System.Net.Http.HttpMethod.s_putMethod;
                    }
                },
                Post: {
                    get: function () {
                        return System.Net.Http.HttpMethod.s_postMethod;
                    }
                },
                Delete: {
                    get: function () {
                        return System.Net.Http.HttpMethod.s_deleteMethod;
                    }
                },
                Head: {
                    get: function () {
                        return System.Net.Http.HttpMethod.s_headMethod;
                    }
                },
                Options: {
                    get: function () {
                        return System.Net.Http.HttpMethod.s_optionsMethod;
                    }
                },
                Trace: {
                    get: function () {
                        return System.Net.Http.HttpMethod.s_traceMethod;
                    }
                },
                Patch: {
                    get: function () {
                        return System.Net.Http.HttpMethod.s_patchMethod;
                    }
                }
            },
            ctors: {
                init: function () {
                    this.s_getMethod = new System.Net.Http.HttpMethod("GET");
                    this.s_putMethod = new System.Net.Http.HttpMethod("PUT");
                    this.s_postMethod = new System.Net.Http.HttpMethod("POST");
                    this.s_deleteMethod = new System.Net.Http.HttpMethod("DELETE");
                    this.s_headMethod = new System.Net.Http.HttpMethod("HEAD");
                    this.s_optionsMethod = new System.Net.Http.HttpMethod("OPTIONS");
                    this.s_traceMethod = new System.Net.Http.HttpMethod("TRACE");
                    this.s_patchMethod = new System.Net.Http.HttpMethod("PATCH");
                }
            },
            methods: {
                op_Equality: function (left, right) {
                    return System.Net.Http.HttpMethod.op_Equality(left, null) || System.Net.Http.HttpMethod.op_Equality(right, null) ? H5.referenceEquals(left, right) : left.equalsT(right);
                },
                op_Inequality: function (left, right) {
                    return !(System.Net.Http.HttpMethod.op_Equality(left, right));
                }
            }
        },
        fields: {
            _method: null,
            _hashcode: 0
        },
        props: {
            Method: {
                get: function () {
                    return this._method;
                }
            },
            MustHaveRequestBody: {
                get: function () {
                    return !H5.referenceEquals(this, System.Net.Http.HttpMethod.Get) && !H5.referenceEquals(this, System.Net.Http.HttpMethod.Head) && !H5.referenceEquals(this, System.Net.Http.HttpMethod.Options) && !H5.referenceEquals(this, System.Net.Http.HttpMethod.Delete);
                }
            }
        },
        alias: ["equalsT", "System$IEquatable$1$System$Net$Http$HttpMethod$equalsT"],
        ctors: {
            ctor: function (method) {
                this.$initialize();
                this._method = method;
            }
        },
        methods: {
            equalsT: function (other) {
                if (System.Net.Http.HttpMethod.op_Equality(other, null)) {
                    return false;
                }

                return System.String.equals(this._method, other._method, 5);
            },
            equals: function (obj) {
                return this.equalsT(H5.as(obj, System.Net.Http.HttpMethod));
            },
            getHashCode: function () {
                if (this._hashcode === 0) {
                    this._hashcode = System.StringComparer.OrdinalIgnoreCase.getHashCode2(this._method);
                }

                return this._hashcode;
            },
            toString: function () {
                return this._method;
            }
        }
    });

    H5.define("System.Net.Http.HttpParseResult", {
        $kind: "enum",
        statics: {
            fields: {
                Parsed: 0,
                NotParsed: 1,
                InvalidFormat: 2
            }
        }
    });

    H5.define("System.Net.Http.HttpRequestException", {
        inherits: [System.Exception],
        props: {
            StatusCode: null
        },
        ctors: {
            ctor: function () {
                System.Net.Http.HttpRequestException.$ctor2.call(this, null, null);
            },
            $ctor1: function (message) {
                System.Net.Http.HttpRequestException.$ctor2.call(this, message, null);
            },
            $ctor2: function (message, inner) {
                this.$initialize();
                System.Exception.ctor.call(this, message, inner);
                if (inner != null) {
                    this.HResult = inner.HResult;
                }
            },
            $ctor3: function (message, inner, statusCode) {
                System.Net.Http.HttpRequestException.$ctor2.call(this, message, inner);
                this.StatusCode = statusCode;
            }
        }
    });

    H5.define("System.Net.Http.HttpRequestMessage", {
        inherits: [System.IDisposable],
        statics: {
            fields: {
                MessageNotYetSent: 0,
                MessageAlreadySent: 0,
                MessageIsRedirect: 0
            },
            ctors: {
                init: function () {
                    this.MessageNotYetSent = 0;
                    this.MessageAlreadySent = 1;
                    this.MessageIsRedirect = 2;
                }
            }
        },
        fields: {
            _sendStatus: 0,
            _responseType: null,
            _method: null,
            _requestUri: null,
            _headers: null,
            _version: null,
            _content: null,
            _disposed: false,
            _options: null,
            _request: null
        },
        props: {
            ResponseType: {
                set: function (value) {
                    this.CheckDisposed();
                    this._responseType = value;
                }
            },
            Version: {
                get: function () {
                    return this._version;
                },
                set: function (value) {
                    if (System.Version.op_Equality(value, null)) {
                        throw new System.ArgumentNullException.$ctor1("value");
                    }
                    this.CheckDisposed();

                    this._version = value;
                }
            },
            Content: {
                get: function () {
                    return this._content;
                },
                set: function (value) {
                    this.CheckDisposed();
                    // It's OK to set a 'null' content, even if the method is POST/PUT.
                    this._content = value;
                }
            },
            Method: {
                get: function () {
                    return this._method;
                },
                set: function (value) {
                    if (System.Net.Http.HttpMethod.op_Equality(value, null)) {
                        throw new System.ArgumentNullException.$ctor1("value");
                    }
                    this.CheckDisposed();

                    this._method = value;
                }
            },
            RequestUri: {
                get: function () {
                    return this._requestUri;
                },
                set: function (value) {
                    this.CheckDisposed();
                    this._requestUri = value;
                }
            },
            Headers: {
                get: function () {
                    if (this._headers == null) {
                        this._headers = new System.Net.Http.Headers.HttpRequestHeaders(this._request);
                    }
                    return this._headers;
                }
            },
            HasHeaders: {
                get: function () {
                    return this._headers != null;
                }
            },
            Options: {
                get: function () {
                    if (this._options == null) {
                        this._options = new System.Net.Http.HttpRequestOptions();
                    }
                    return this._options;
                }
            }
        },
        alias: ["Dispose", "System$IDisposable$Dispose"],
        ctors: {
            init: function () {
                this._sendStatus = 0;
                this._request = new (XMLHttpRequest)();
            },
            ctor: function () {
                System.Net.Http.HttpRequestMessage.$ctor2.call(this, System.Net.Http.HttpMethod.Get, null);
            },
            $ctor2: function (method, requestUri) {
                this.$initialize();
                // It's OK to have a 'null' request Uri. If HttpClient is used, the 'BaseAddress' will be added.
                // If there is no 'BaseAddress', sending this request message will throw.
                // Note that we also allow the string to be empty: null and empty are considered equivalent.
                this._method = method || ($asm.$.System.Net.Http.HttpRequestMessage.f1)();
                this._requestUri = requestUri;
            },
            $ctor1: function (method, requestUri) {
                System.Net.Http.HttpRequestMessage.$ctor2.call(this, method, System.String.isNullOrEmpty(requestUri) ? null : new System.Uri(requestUri));
            }
        },
        methods: {
            MarkAsSent: function () {
                if (this._sendStatus === System.Net.Http.HttpRequestMessage.MessageNotYetSent) {
                    this._sendStatus = System.Net.Http.HttpRequestMessage.MessageAlreadySent;
                    return true;
                }
                return false;
            },
            WasSentByHttpClient: function () {
                return (this._sendStatus & System.Net.Http.HttpRequestMessage.MessageAlreadySent) !== 0;
            },
            MarkAsRedirected: function () {
                this._sendStatus = this._sendStatus | System.Net.Http.HttpRequestMessage.MessageIsRedirect;
            },
            WasRedirected: function () {
                return (this._sendStatus & System.Net.Http.HttpRequestMessage.MessageIsRedirect) !== 0;
            },
            Dispose: function () {
                this._disposed = true;
            },
            CheckDisposed: function () {
                if (this._disposed) {
                    throw new System.ObjectDisposedException.$ctor1(H5.getTypeName(H5.getType(this)));
                }
            }
        }
    });

    H5.ns("System.Net.Http.HttpRequestMessage", $asm.$);

    H5.apply($asm.$.System.Net.Http.HttpRequestMessage, {
        f1: function () {
            throw new System.ArgumentNullException.$ctor1("method");
        }
    });

    H5.define("System.Net.Http.HttpRequestOptions", {
        inherits: [System.Collections.Generic.IDictionary$2(System.String,System.Object)],
        props: {
            Options: null,
            System$Collections$Generic$IDictionary$2$System$String$System$Object$Keys: {
                get: function () {
                    return this.Options.Keys;
                }
            },
            System$Collections$Generic$IDictionary$2$System$String$System$Object$Values: {
                get: function () {
                    return this.Options.Values;
                }
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$Count: {
                get: function () {
                    return this.Options.Count;
                }
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$IsReadOnly: {
                get: function () {
                    return System.Array.getIsReadOnly(H5.cast(this.Options, System.Collections.Generic.IDictionary$2(System.String,System.Object)), System.Collections.Generic.KeyValuePair$2(System.String,System.Object));
                }
            }
        },
        alias: ["System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"],
        ctors: {
            init: function () {
                this.Options = new (System.Collections.Generic.Dictionary$2(System.String,System.Object)).ctor();
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            System$Collections$Generic$IDictionary$2$System$String$System$Object$getItem: function (key) {
                return this.Options.getItem(key);
            },
            System$Collections$Generic$IDictionary$2$System$String$System$Object$setItem: function (key, value) {
                this.Options.setItem(key, value);
            },
            System$Collections$Generic$IDictionary$2$System$String$System$Object$add: function (key, value) {
                this.Options.add(key, value);
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$add: function (item) {
                System.Array.add(H5.cast(this.Options, System.Collections.Generic.IDictionary$2(System.String,System.Object)), item, System.Collections.Generic.KeyValuePair$2(System.String,System.Object));
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$clear: function () {
                this.Options.clear();
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$contains: function (item) {
                return System.Array.contains(H5.cast(this.Options, System.Collections.Generic.IDictionary$2(System.String,System.Object)), item, System.Collections.Generic.KeyValuePair$2(System.String,System.Object));
            },
            System$Collections$Generic$IDictionary$2$System$String$System$Object$containsKey: function (key) {
                return this.Options.containsKey(key);
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$copyTo: function (array, arrayIndex) {
                System.Array.copyTo(H5.cast(this.Options, System.Collections.Generic.IDictionary$2(System.String,System.Object)), array, arrayIndex, System.Collections.Generic.KeyValuePair$2(System.String,System.Object));
            },
            System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$GetEnumerator: function () {
                return this.Options.GetEnumerator().$clone();
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return H5.getEnumerator(H5.cast(this.Options, System.Collections.IEnumerable));
            },
            System$Collections$Generic$IDictionary$2$System$String$System$Object$remove: function (key) {
                return this.Options.remove(key);
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$System$String$System$Object$remove: function (item) {
                return System.Array.remove(H5.cast(this.Options, System.Collections.Generic.IDictionary$2(System.String,System.Object)), item, System.Collections.Generic.KeyValuePair$2(System.String,System.Object));
            },
            System$Collections$Generic$IDictionary$2$System$String$System$Object$tryGetValue: function (key, value) {
                return this.Options.tryGetValue(key, value);
            },
            TryGetValue: function (TValue, key, value) {
                var _value = { };
                var tvalue;
                if (this.Options.tryGetValue(key.Key, _value) && ((tvalue = H5.as(_value.v, TValue))) != null) {
                    value.v = tvalue;
                    return true;
                }

                value.v = H5.getDefaultValue(TValue);
                return false;
            },
            Set: function (TValue, key, value) {
                this.Options.setItem(key.Key, value);
            }
        }
    });

    H5.define("System.Net.Http.HttpRequestOptionsKey$1", function (TValue) { return {
        $kind: "struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Net.Http.HttpRequestOptionsKey$1(TValue))(); }
            }
        },
        props: {
            Key: null
        },
        ctors: {
            $ctor1: function (key) {
                this.$initialize();
                this.Key = key;
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            getHashCode: function () {
                var h = H5.addHash([8758703446, this.Key]);
                return h;
            },
            equals: function (o) {
                if (!H5.is(o, System.Net.Http.HttpRequestOptionsKey$1(TValue))) {
                    return false;
                }
                return H5.equals(this.Key, o.Key);
            },
            $clone: function (to) { return this; }
        }
    }; });

    H5.define("System.Net.Http.HttpResponseMessage", {
        inherits: [System.IDisposable],
        statics: {
            fields: {
                DefaultStatusCode: 0
            },
            ctors: {
                init: function () {
                    this.DefaultStatusCode = System.Net.HttpStatusCode.OK;
                }
            }
        },
        fields: {
            _statusCode: 0,
            _headers: null,
            _reasonPhrase: null,
            _requestMessage: null,
            _content: null,
            _disposed: false
        },
        props: {
            Content: {
                get: function () {
                    if (this._content == null) {
                        this._content = new System.Net.Http.EmptyContent();
                    }
                    return this._content;
                },
                set: function (value) {
                    this.CheckDisposed();
                    this._content = value;
                }
            },
            StatusCode: {
                get: function () {
                    return this._statusCode;
                },
                set: function (value) {
                    if ((value < 0) || (value > 999)) {
                        throw new System.ArgumentOutOfRangeException.$ctor1("value");
                    }
                    this.CheckDisposed();

                    this._statusCode = value;
                }
            },
            ReasonPhrase: {
                get: function () {
                    if (this._reasonPhrase != null) {
                        return this._reasonPhrase;
                    }
                    // Provide a default if one was not set.
                    return System.Net.HttpStatusDescription.Get$1(this.StatusCode);
                },
                set: function (value) {
                    if ((value != null) && this.ContainsNewLineCharacter(value)) {
                        throw new System.FormatException.$ctor1("The reason phrase must not contain new-line characters.");
                    }
                    this.CheckDisposed();

                    this._reasonPhrase = value; // It's OK to have a 'null' reason phrase.
                }
            },
            Headers: {
                get: function () {
                    if (this._headers == null) {
                        this._headers = new System.Net.Http.Headers.HttpResponseHeaders(this._requestMessage._request);
                    }
                    return this._headers;
                }
            },
            RequestMessage: {
                get: function () {
                    return this._requestMessage;
                },
                set: function (value) {
                    this.CheckDisposed();
                    this._requestMessage = value;
                }
            },
            IsSuccessStatusCode: {
                get: function () {
                    return (this._statusCode >= 200) && (this._statusCode <= 299);
                }
            }
        },
        alias: ["Dispose", "System$IDisposable$Dispose"],
        ctors: {
            ctor: function (requestObject) {
                System.Net.Http.HttpResponseMessage.$ctor1.call(this, System.Net.Http.HttpResponseMessage.DefaultStatusCode, requestObject);
            },
            $ctor1: function (statusCode, requestObject) {
                this.$initialize();
                if ((statusCode < 0) || (statusCode > 999)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("statusCode");
                }

                this._statusCode = statusCode;
            }
        },
        methods: {
            SetStatusCodeWithoutValidation: function (value) {
                this._statusCode = value;
            },
            SetReasonPhraseWithoutValidation: function (value) {
                this._reasonPhrase = value;
            },
            EnsureSuccessStatusCode: function () {
                if (!this.IsSuccessStatusCode) {
                    throw new System.Net.Http.HttpRequestException.$ctor3(System.String.format("Response status code does not indicate success: {0} ({1}).", H5.box(this._statusCode, System.Int32), this.ReasonPhrase), null, this._statusCode);
                }

                return this;
            },
            ContainsNewLineCharacter: function (value) {
                var $t;
                $t = H5.getEnumerator(value);
                try {
                    while ($t.moveNext()) {
                        var character = $t.Current;
                        if ((character === 13) || (character === 10)) {
                            return true;
                        }
                    }
                } finally {
                    if (H5.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }
                return false;
            },
            Dispose: function () {
                this._disposed = true;

            },
            CheckDisposed: function () {
                if (this._disposed) {
                    throw new System.ObjectDisposedException.$ctor1(H5.getTypeName(H5.getType(this)));
                }
            }
        }
    });

    H5.define("System.Net.HttpStatusCode", {
        $kind: "enum",
        statics: {
            fields: {
                Continue: 100,
                SwitchingProtocols: 101,
                Processing: 102,
                EarlyHints: 103,
                OK: 200,
                Created: 201,
                Accepted: 202,
                NonAuthoritativeInformation: 203,
                NoContent: 204,
                ResetContent: 205,
                PartialContent: 206,
                MultiStatus: 207,
                AlreadyReported: 208,
                IMUsed: 226,
                MultipleChoices: 300,
                Ambiguous: 300,
                MovedPermanently: 301,
                Moved: 301,
                Found: 302,
                Redirect: 302,
                SeeOther: 303,
                RedirectMethod: 303,
                NotModified: 304,
                UseProxy: 305,
                Unused: 306,
                TemporaryRedirect: 307,
                RedirectKeepVerb: 307,
                PermanentRedirect: 308,
                BadRequest: 400,
                Unauthorized: 401,
                PaymentRequired: 402,
                Forbidden: 403,
                NotFound: 404,
                MethodNotAllowed: 405,
                NotAcceptable: 406,
                ProxyAuthenticationRequired: 407,
                RequestTimeout: 408,
                Conflict: 409,
                Gone: 410,
                LengthRequired: 411,
                PreconditionFailed: 412,
                RequestEntityTooLarge: 413,
                RequestUriTooLong: 414,
                UnsupportedMediaType: 415,
                RequestedRangeNotSatisfiable: 416,
                ExpectationFailed: 417,
                MisdirectedRequest: 421,
                UnprocessableEntity: 422,
                Locked: 423,
                FailedDependency: 424,
                UpgradeRequired: 426,
                PreconditionRequired: 428,
                TooManyRequests: 429,
                RequestHeaderFieldsTooLarge: 431,
                UnavailableForLegalReasons: 451,
                InternalServerError: 500,
                NotImplemented: 501,
                BadGateway: 502,
                ServiceUnavailable: 503,
                GatewayTimeout: 504,
                HttpVersionNotSupported: 505,
                VariantAlsoNegotiates: 506,
                InsufficientStorage: 507,
                LoopDetected: 508,
                NotExtended: 510,
                NetworkAuthenticationRequired: 511
            }
        }
    });

    H5.define("System.Net.HttpStatusDescription", {
        statics: {
            methods: {
                Get$1: function (code) {
                    return System.Net.HttpStatusDescription.Get(code);
                },
                Get: function (code) {
                    switch (code) {
                        case 100: 
                            return "Continue";
                        case 101: 
                            return "Switching Protocols";
                        case 102: 
                            return "Processing";
                        case 103: 
                            return "Early Hints";
                        case 200: 
                            return "OK";
                        case 201: 
                            return "Created";
                        case 202: 
                            return "Accepted";
                        case 203: 
                            return "Non-Authoritative Information";
                        case 204: 
                            return "No Content";
                        case 205: 
                            return "Reset Content";
                        case 206: 
                            return "Partial Content";
                        case 207: 
                            return "Multi-Status";
                        case 208: 
                            return "Already Reported";
                        case 226: 
                            return "IM Used";
                        case 300: 
                            return "Multiple Choices";
                        case 301: 
                            return "Moved Permanently";
                        case 302: 
                            return "Found";
                        case 303: 
                            return "See Other";
                        case 304: 
                            return "Not Modified";
                        case 305: 
                            return "Use Proxy";
                        case 307: 
                            return "Temporary Redirect";
                        case 308: 
                            return "Permanent Redirect";
                        case 400: 
                            return "Bad Request";
                        case 401: 
                            return "Unauthorized";
                        case 402: 
                            return "Payment Required";
                        case 403: 
                            return "Forbidden";
                        case 404: 
                            return "Not Found";
                        case 405: 
                            return "Method Not Allowed";
                        case 406: 
                            return "Not Acceptable";
                        case 407: 
                            return "Proxy Authentication Required";
                        case 408: 
                            return "Request Timeout";
                        case 409: 
                            return "Conflict";
                        case 410: 
                            return "Gone";
                        case 411: 
                            return "Length Required";
                        case 412: 
                            return "Precondition Failed";
                        case 413: 
                            return "Request Entity Too Large";
                        case 414: 
                            return "Request-Uri Too Long";
                        case 415: 
                            return "Unsupported Media Type";
                        case 416: 
                            return "Requested Range Not Satisfiable";
                        case 417: 
                            return "Expectation Failed";
                        case 421: 
                            return "Misdirected Request";
                        case 422: 
                            return "Unprocessable Entity";
                        case 423: 
                            return "Locked";
                        case 424: 
                            return "Failed Dependency";
                        case 426: 
                            return "Upgrade Required";
                        case 428: 
                            return "Precondition Required";
                        case 429: 
                            return "Too Many Requests";
                        case 431: 
                            return "Request Header Fields Too Large";
                        case 451: 
                            return "Unavailable For Legal Reasons";
                        case 500: 
                            return "Internal Server Error";
                        case 501: 
                            return "Not Implemented";
                        case 502: 
                            return "Bad Gateway";
                        case 503: 
                            return "Service Unavailable";
                        case 504: 
                            return "Gateway Timeout";
                        case 505: 
                            return "Http Version Not Supported";
                        case 506: 
                            return "Variant Also Negotiates";
                        case 507: 
                            return "Insufficient Storage";
                        case 508: 
                            return "Loop Detected";
                        case 510: 
                            return "Not Extended";
                        case 511: 
                            return "Network Authentication Required";
                        default: 
                            return null;
                    }
                }
            }
        }
    });

    H5.define("System.Net.Http.BrowserHttpHandler", {
        inherits: [System.Net.Http.HttpMessageHandler],
        props: {
            AllowAutoRedirect: false,
            MaxAutomaticRedirections: 0
        },
        ctors: {
            init: function () {
                this.AllowAutoRedirect = true;
                this.MaxAutomaticRedirections = 50;
            }
        },
        methods: {
            SendAsync: function (request, cancellationToken) {
                return this.SendAsync$1(request, this.MaxAutomaticRedirections, cancellationToken);
            },
            SendAsync$1: function (request, redirectCount, cancellationToken) {
                var $s = 0,
                    $t1, 
                    $tr1, 
                    $jff, 
                    $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                    $rv, 
                    requestObject, 
                    abortCts, 
                    abortRegistration, 
                    tcs, 
                    stringContent, 
                    formContent, 
                    $ae, 
                    $asyncBody = H5.fn.bind(this, function () {
                        try {
                            for (;;) {
                                $s = System.Array.min([0,1], $s);
                                switch ($s) {
                                    case 0: {
                                        if (request == null) {
                                            throw new System.ArgumentNullException.$ctor1("request");
                                        }

                                        requestObject = request._request;

                                        requestObject.open(request.Method.Method, request.RequestUri.getAbsoluteUri());

                                        if (request.Content != null) {
                                            request.Headers.AddHeaders(request.Content.Headers);
                                        }

                                        abortCts = System.Threading.CancellationTokenSource.createLinked(cancellationToken);

                                        abortRegistration = abortCts.token.register(function () {
                                            requestObject.abort();
                                            abortCts.dispose();
                                        });

                                        tcs = new System.Threading.Tasks.TaskCompletionSource();

                                        requestObject.onreadystatechange = H5.fn.bind(this, function (e) {
                                            if (requestObject.readyState === 0) {
                                                tcs.trySetCanceled();
                                                tcs = null;
                                                return;
                                            }

                                            if (requestObject.readyState === 4) {
                                                if (requestObject.status === 302) {
                                                    if (redirectCount > 0) {
                                                        redirectCount = (redirectCount - 1) | 0;
                                                        System.Threading.Tasks.Task.run(H5.fn.bind(this, function () {
                                                            var $s = 0,
                                                                $t1, 
                                                                $tr1, 
                                                                $jff, 
                                                                $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                                                                $rv, 
                                                                location, 
                                                                response, 
                                                                E, 
                                                                $ae, 
                                                                $ae1, 
                                                                $asyncBody = H5.fn.bind(this, function () {
                                                                    try {
                                                                        for (;;) {
                                                                            $s = System.Array.min([1,2,3,4], $s);
                                                                            switch ($s) {

                                                                                case 1: {
                                                                                    request.MarkAsRedirected();
                                                                                    location = requestObject.getResponseHeader("Location");
                                                                                    $t1 = this.SendAsync$1(request, redirectCount, cancellationToken);
                                                                                    $s = 2;
                                                                                    if ($t1.isCompleted()) {
                                                                                        continue;
                                                                                    }
                                                                                    $t1.continue($asyncBody);
                                                                                    return;
                                                                                }
                                                                                case 2: {
                                                                                    $tr1 = $t1.getAwaitedResult();
                                                                                    response = $tr1;
                                                                                    tcs.trySetResult(response);
                                                                                    $s = 4;
                                                                                    continue;
                                                                                }
                                                                                case 3: {
                                                                                    tcs.trySetException(E);
                                                                                    $ae = null;
                                                                                    $s = 4;
                                                                                    continue;
                                                                                }
                                                                                case 4: {
                                                                                    $tcs.setResult(null);
                                                                                    return;
                                                                                }
                                                                                default: {
                                                                                    $tcs.setResult(null);
                                                                                    return;
                                                                                }
                                                                            }
                                                                        }
                                                                    } catch($ae1) {
                                                                        $ae = System.Exception.create($ae1);
                                                                        if ( $s >= 1 && $s <= 2 ) {
                                                                            E = $ae;
                                                                            $s = 3;
                                                                            $asyncBody();
                                                                            return;
                                                                        }
                                                                        $tcs.setException($ae);
                                                                    }
                                                                }, arguments);

                                                            $asyncBody();
                                                            return $tcs.task;
                                                        }));
                                                    } else {
                                                        tcs.trySetException(new System.Exception("Maximum number of redirects hit"));
                                                    }
                                                }

                                                var httpResponse = new System.Net.Http.HttpResponseMessage.$ctor1(requestObject.status, requestObject);
                                                httpResponse.Content = new System.Net.Http.BrowserHttpHandler.BrowserHttpContent(requestObject);

                                                if (requestObject.status >= 200 && requestObject.status < 300) {
                                                    tcs.trySetResult(httpResponse);
                                                }
                                            }
                                        });

                                        if (H5.is(request.Content, System.Object)) {
                                            if (((stringContent = H5.as(request.Content, System.Net.Http.StringContent))) != null) {
                                                requestObject.send(stringContent.Content);
                                            } else {
                                                if (((formContent = H5.as(request.Content, System.Net.Http.FormContent))) != null) {
                                                    requestObject.send(formContent.Content);
                                                }
                                            }
                                        } else {
                                            requestObject.send();
                                        }

                                        $t1 = tcs.task;
                                        $s = 1;
                                        if ($t1.isCompleted()) {
                                            continue;
                                        }
                                        $t1.continue($asyncBody);
                                        return;
                                    }
                                    case 1: {
                                        $tr1 = $t1.getAwaitedResult();
                                        $tcs.setResult($tr1);
                                        return;
                                    }
                                    default: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                }
                            }
                        } catch($ae1) {
                            $ae = System.Exception.create($ae1);
                            $tcs.setException($ae);
                        }
                    }, arguments);

                $asyncBody();
                return $tcs.task;
            }
        }
    });

    H5.define("System.Net.Http.BrowserHttpHandler.BrowserHttpContent", {
        inherits: [System.Net.Http.HttpContent],
        $kind: "nested class",
        ctors: {
            ctor: function (request) {
                this.$initialize();
                System.Net.Http.HttpContent.$ctor1.call(this, request);
            }
        }
    });

    H5.define("System.Net.Http.EmptyContent", {
        inherits: [System.Net.Http.HttpContent]
    });

    H5.define("System.Net.Http.FormContent", {
        inherits: [System.Net.Http.HttpContent],
        props: {
            Content: null
        },
        ctors: {
            ctor: function (content) {
                this.$initialize();
                System.Net.Http.HttpContent.ctor.call(this);
                this.Content = content;
            }
        }
    });

    H5.define("System.Net.Http.Headers.HttpContentHeaders", {
        inherits: [System.Net.Http.Headers.HttpHeaders],
        fields: {
            _parent: null
        },
        ctors: {
            ctor: function (parent) {
                this.$initialize();
                System.Net.Http.Headers.HttpHeaders.ctor.call(this, parent._request);
                this._parent = parent;
            }
        }
    });

    H5.define("System.Net.Http.Headers.HttpRequestHeaders", {
        inherits: [System.Net.Http.Headers.HttpHeaders],
        ctors: {
            ctor: function (request) {
                this.$initialize();
                System.Net.Http.Headers.HttpHeaders.ctor.call(this, request);
            }
        },
        methods: {
            AddHeaders: function (sourceHeaders) {
                System.Net.Http.Headers.HttpHeaders.prototype.AddHeaders.call(this, sourceHeaders);
            }
        }
    });

    H5.define("System.Net.Http.Headers.HttpResponseHeaders", {
        inherits: [System.Net.Http.Headers.HttpHeaders],
        ctors: {
            ctor: function (request) {
                this.$initialize();
                System.Net.Http.Headers.HttpHeaders.ctor.call(this, request);
            }
        },
        methods: {
            AddHeaders: function (sourceHeaders) {
                System.Net.Http.Headers.HttpHeaders.prototype.AddHeaders.call(this, sourceHeaders);
            }
        }
    });

    H5.define("System.Net.Http.HttpClient", {
        inherits: [System.Net.Http.HttpMessageInvoker],
        statics: {
            fields: {
                s_defaultTimeout: null,
                s_maxTimeout: null,
                s_infiniteTimeout: null,
                _absoluteUrl: null
            },
            ctors: {
                init: function () {
                    this.s_defaultTimeout = new System.TimeSpan();
                    this.s_maxTimeout = new System.TimeSpan();
                    this.s_infiniteTimeout = new System.TimeSpan();
                    this.s_defaultTimeout = System.TimeSpan.fromSeconds(100);
                    this.s_maxTimeout = System.TimeSpan.fromMilliseconds(2147483647);
                    this.s_infiniteTimeout = new System.TimeSpan(0, 0, 0, 0, -1);
                    this._absoluteUrl = new System.Text.RegularExpressions.Regex.ctor("^(?:[a-z]+:)?\\/\\/", 1);
                }
            },
            methods: {
                IsAbsoluteUri: function (uri) {
                    return System.Net.Http.HttpClient._absoluteUrl.isMatch(H5.toString(uri));
                },
                ThrowForNullResponse: function (response) {
                    if (response == null) {
                        throw new System.InvalidOperationException.$ctor1("Handler did not return a response message.");
                    }
                },
                FinishSend: function (cts, disposeCts) {
                    // Dispose of the CancellationTokenSource if it was created specially for this request
                    // rather than being used across multiple requests.
                    if (disposeCts) {
                        cts.dispose();
                    }
                },
                CheckRequestMessage: function (request) {
                    if (!request.MarkAsSent()) {
                        throw new System.InvalidOperationException.$ctor1("The request message was already sent. Cannot send the same request message multiple times.");
                    }
                }
            }
        },
        fields: {
            _operationStarted: false,
            _disposed: false,
            _pendingRequestsCts: null,
            _defaultRequestHeaders: null,
            _baseAddress: null,
            _timeout: null
        },
        props: {
            DefaultRequestHeaders: {
                get: function () {

                    if (this._defaultRequestHeaders == null) {
                        this._defaultRequestHeaders = new System.Net.Http.Headers.HttpRequestHeaders(null);
                    }
                    return this._defaultRequestHeaders;
                }
            },
            BaseAddress: {
                get: function () {
                    return this._baseAddress;
                },
                set: function (value) {
                    // It's OK to not have a base address specified, but if one is, it needs to be absolute.
                    if (H5.is(value, System.Object) && !System.Net.Http.HttpClient.IsAbsoluteUri(value)) {
                        throw new System.ArgumentException.$ctor3("The base address must be an absolute URI.", "value");
                    }

                    this.CheckDisposedOrStarted();
                    this._baseAddress = value;
                }
            },
            Timeout: {
                get: function () {
                    return this._timeout;
                },
                set: function (value) {
                    if (System.TimeSpan.neq(value, System.Net.Http.HttpClient.s_infiniteTimeout) && (System.TimeSpan.lte(value, System.TimeSpan.zero) || System.TimeSpan.gt(value, System.Net.Http.HttpClient.s_maxTimeout))) {
                        throw new System.ArgumentOutOfRangeException.$ctor1("value");
                    }

                    this.CheckDisposedOrStarted();
                    this._timeout = value;
                }
            }
        },
        alias: ["Dispose", "System$IDisposable$Dispose"],
        ctors: {
            init: function () {
                this._timeout = new System.TimeSpan();
            },
            ctor: function () {
                System.Net.Http.HttpClient.$ctor1.call(this, new System.Net.Http.HttpClientHandler());
            },
            $ctor1: function (handler) {
                this.$initialize();
                System.Net.Http.HttpMessageInvoker.ctor.call(this, handler);
                this._timeout = System.Net.Http.HttpClient.s_defaultTimeout;
                this._pendingRequestsCts = new System.Threading.CancellationTokenSource();
            }
        },
        methods: {
            GetStringAsync: function (requestUri) {
                return this.GetStringAsync$2(this.CreateUri(requestUri));
            },
            GetStringAsync$2: function (requestUri) {
                return this.GetStringAsync$3(requestUri, System.Threading.CancellationToken.none);
            },
            GetStringAsync$1: function (requestUri, cancellationToken) {
                return this.GetStringAsync$3(this.CreateUri(requestUri), cancellationToken);
            },
            GetStringAsync$3: function (requestUri, cancellationToken) {
                var request = this.CreateRequestMessage(System.Net.Http.HttpMethod.Get, requestUri);
                // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
                this.CheckRequestBeforeSend(request);
                return this.GetStringAsyncCore(request, cancellationToken);
            },
            GetStringAsyncCore: function (request, cancellationToken) {
                var $s = 0,
                    $t1, 
                    $tr1, 
                    $jff, 
                    $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                    $rv, 
                    cts, 
                    disposeCts, 
                    pendingRequestsCts, 
                    response, 
                    $ae, 
                    $ae1, 
                    $asyncBody = H5.fn.bind(this, function () {
                        try {
                            for (;;) {
                                $s = System.Array.min([0,1,2,3,4], $s);
                                switch ($s) {
                                    case 0: {
                                        cts = { };
                                        disposeCts = { };
                                        pendingRequestsCts = { };
                                        H5.Deconstruct(this.PrepareCancellationTokenSource(cancellationToken).$clone(), cts, disposeCts, pendingRequestsCts);
                                        response = null;
                                        $s = 1;
                                        continue;
                                    }
                                    case 1: {
                                        // Wait for the response message and make sure it completed successfully.
                                        request.ResponseType = "text";
                                        $t1 = System.Net.Http.HttpMessageInvoker.prototype.SendAsync.call(this, request, cts.v.token);
                                        $s = 2;
                                        if ($t1.isCompleted()) {
                                            continue;
                                        }
                                        $t1.continue($asyncBody);
                                        return;
                                    }
                                    case 2: {
                                        $tr1 = $t1.getAwaitedResult();
                                        response = $tr1;
                                        System.Net.Http.HttpClient.ThrowForNullResponse(response);
                                        response.EnsureSuccessStatusCode();
                                        $rv = response.Content.ReadAsString();
                                        $s = 3;
                                        continue;
                                    }
                                    case 3: {
                                        System.Net.Http.HttpClient.FinishSend(cts.v, disposeCts.v);

                                        if ($jff > -1) {
                                            $s = $jff;
                                            $jff = null;
                                        } else if ($ae) {
                                            $tcs.setException($ae);
                                            return;
                                        } else if (H5.isDefined($rv)) {
                                            $tcs.setResult($rv);
                                            return;
                                        }
                                        $s = 4;
                                        continue;
                                    }
                                    case 4: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                    default: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                }
                            }
                        } catch($ae1) {
                            $ae = System.Exception.create($ae1);
                            if ($s >= 1 && $s <= 2) {
                                $s = 3;
                                $asyncBody();
                                return;
                            }
                            $tcs.setException($ae);
                        }
                    }, arguments);

                $asyncBody();
                return $tcs.task;
            },
            GetByteArrayAsync: function (requestUri) {
                return this.GetByteArrayAsync$2(this.CreateUri(requestUri));
            },
            GetByteArrayAsync$2: function (requestUri) {
                return this.GetByteArrayAsync$3(requestUri, System.Threading.CancellationToken.none);
            },
            GetByteArrayAsync$1: function (requestUri, cancellationToken) {
                return this.GetByteArrayAsync$3(this.CreateUri(requestUri), cancellationToken);
            },
            GetByteArrayAsync$3: function (requestUri, cancellationToken) {
                var request = this.CreateRequestMessage(System.Net.Http.HttpMethod.Get, requestUri);

                // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
                this.CheckRequestBeforeSend(request);

                return this.GetByteArrayAsyncCore(request, cancellationToken);
            },
            GetByteArrayAsyncCore: function (request, cancellationToken) {
                var $s = 0,
                    $t1, 
                    $tr1, 
                    $jff, 
                    $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                    $rv, 
                    cts, 
                    disposeCts, 
                    pendingRequestsCts, 
                    response, 
                    $ae, 
                    $ae1, 
                    $asyncBody = H5.fn.bind(this, function () {
                        try {
                            for (;;) {
                                $s = System.Array.min([0,1,2,3,4], $s);
                                switch ($s) {
                                    case 0: {
                                        cts = { };
                                        disposeCts = { };
                                        pendingRequestsCts = { };
                                        H5.Deconstruct(this.PrepareCancellationTokenSource(cancellationToken).$clone(), cts, disposeCts, pendingRequestsCts);
                                        response = null;
                                        $s = 1;
                                        continue;
                                    }
                                    case 1: {
                                        request.ResponseType = "arraybuffer";
                                        // Wait for the response message and make sure it completed successfully.
                                        $t1 = System.Net.Http.HttpMessageInvoker.prototype.SendAsync.call(this, request, cts.v.token);
                                        $s = 2;
                                        if ($t1.isCompleted()) {
                                            continue;
                                        }
                                        $t1.continue($asyncBody);
                                        return;
                                    }
                                    case 2: {
                                        $tr1 = $t1.getAwaitedResult();
                                        response = $tr1;
                                        System.Net.Http.HttpClient.ThrowForNullResponse(response);
                                        response.EnsureSuccessStatusCode();
                                        $rv = response.Content.ReadAsArrayBuffer();
                                        $s = 3;
                                        continue;
                                    }
                                    case 3: {
                                        System.Net.Http.HttpClient.FinishSend(cts.v, disposeCts.v);

                                        if ($jff > -1) {
                                            $s = $jff;
                                            $jff = null;
                                        } else if ($ae) {
                                            $tcs.setException($ae);
                                            return;
                                        } else if (H5.isDefined($rv)) {
                                            $tcs.setResult($rv);
                                            return;
                                        }
                                        $s = 4;
                                        continue;
                                    }
                                    case 4: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                    default: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                }
                            }
                        } catch($ae1) {
                            $ae = System.Exception.create($ae1);
                            if ($s >= 1 && $s <= 2) {
                                $s = 3;
                                $asyncBody();
                                return;
                            }
                            $tcs.setException($ae);
                        }
                    }, arguments);

                $asyncBody();
                return $tcs.task;
            },
            GetBlobAsync: function (requestUri) {
                return this.GetBlobAsync$2(this.CreateUri(requestUri));
            },
            GetBlobAsync$1: function (requestUri, cancellationToken) {
                return this.GetBlobAsync$3(this.CreateUri(requestUri), cancellationToken);
            },
            GetBlobAsync$2: function (requestUri) {
                return this.GetBlobAsync$3(requestUri, System.Threading.CancellationToken.none);
            },
            GetBlobAsync$3: function (requestUri, cancellationToken) {
                var request = this.CreateRequestMessage(System.Net.Http.HttpMethod.Get, requestUri);

                // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
                this.CheckRequestBeforeSend(request);

                return this.GetBlobAsyncCore(request, cancellationToken);
            },
            GetBlobAsyncCore: function (request, cancellationToken) {
                var $s = 0,
                    $t1, 
                    $tr1, 
                    $jff, 
                    $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                    $rv, 
                    cts, 
                    disposeCts, 
                    pendingRequestsCts, 
                    response, 
                    $ae, 
                    $ae1, 
                    $asyncBody = H5.fn.bind(this, function () {
                        try {
                            for (;;) {
                                $s = System.Array.min([0,1,2,3,4], $s);
                                switch ($s) {
                                    case 0: {
                                        cts = { };
                                        disposeCts = { };
                                        pendingRequestsCts = { };
                                        H5.Deconstruct(this.PrepareCancellationTokenSource(cancellationToken).$clone(), cts, disposeCts, pendingRequestsCts);
                                        response = null;
                                        $s = 1;
                                        continue;
                                    }
                                    case 1: {
                                        // Wait for the response message and make sure it completed successfully.
                                        request.ResponseType = "blob";
                                        $t1 = System.Net.Http.HttpMessageInvoker.prototype.SendAsync.call(this, request, cts.v.token);
                                        $s = 2;
                                        if ($t1.isCompleted()) {
                                            continue;
                                        }
                                        $t1.continue($asyncBody);
                                        return;
                                    }
                                    case 2: {
                                        $tr1 = $t1.getAwaitedResult();
                                        response = $tr1;
                                        System.Net.Http.HttpClient.ThrowForNullResponse(response);
                                        response.EnsureSuccessStatusCode();
                                        $rv = response.Content.ReadAsBlob();
                                        $s = 3;
                                        continue;
                                    }
                                    case 3: {
                                        System.Net.Http.HttpClient.FinishSend(cts.v, disposeCts.v);

                                        if ($jff > -1) {
                                            $s = $jff;
                                            $jff = null;
                                        } else if ($ae) {
                                            $tcs.setException($ae);
                                            return;
                                        } else if (H5.isDefined($rv)) {
                                            $tcs.setResult($rv);
                                            return;
                                        }
                                        $s = 4;
                                        continue;
                                    }
                                    case 4: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                    default: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                }
                            }
                        } catch($ae1) {
                            $ae = System.Exception.create($ae1);
                            if ($s >= 1 && $s <= 2) {
                                $s = 3;
                                $asyncBody();
                                return;
                            }
                            $tcs.setException($ae);
                        }
                    }, arguments);

                $asyncBody();
                return $tcs.task;
            },
            GetObjectLiteralAsync: function (T, requestUri) {
                return this.GetObjectLiteralAsync$2(T, this.CreateUri(requestUri));
            },
            GetObjectLiteralAsync$1: function (T, requestUri, cancellationToken) {
                return this.GetObjectLiteralAsync$3(T, this.CreateUri(requestUri), cancellationToken);
            },
            GetObjectLiteralAsync$2: function (T, requestUri) {
                return this.GetObjectLiteralAsync$3(T, requestUri, System.Threading.CancellationToken.none);
            },
            GetObjectLiteralAsync$3: function (T, requestUri, cancellationToken) {
                var request = this.CreateRequestMessage(System.Net.Http.HttpMethod.Get, requestUri);

                // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
                this.CheckRequestBeforeSend(request);

                return this.GetObjectLiteralAsyncCore(T, request, cancellationToken);
            },
            GetObjectLiteralAsyncCore: function (T, request, cancellationToken) {
                var $s = 0,
                    $t1, 
                    $tr1, 
                    $jff, 
                    $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                    $rv, 
                    cts, 
                    disposeCts, 
                    pendingRequestsCts, 
                    response, 
                    $ae, 
                    $ae1, 
                    $asyncBody = H5.fn.bind(this, function () {
                        try {
                            for (;;) {
                                $s = System.Array.min([0,1,2,3,4], $s);
                                switch ($s) {
                                    case 0: {
                                        cts = { };
                                        disposeCts = { };
                                        pendingRequestsCts = { };
                                        H5.Deconstruct(this.PrepareCancellationTokenSource(cancellationToken).$clone(), cts, disposeCts, pendingRequestsCts);
                                        response = null;
                                        $s = 1;
                                        continue;
                                    }
                                    case 1: {
                                        request.ResponseType = "json";
                                        // Wait for the response message and make sure it completed successfully.
                                        $t1 = System.Net.Http.HttpMessageInvoker.prototype.SendAsync.call(this, request, cts.v.token);
                                        $s = 2;
                                        if ($t1.isCompleted()) {
                                            continue;
                                        }
                                        $t1.continue($asyncBody);
                                        return;
                                    }
                                    case 2: {
                                        $tr1 = $t1.getAwaitedResult();
                                        response = $tr1;
                                        System.Net.Http.HttpClient.ThrowForNullResponse(response);
                                        response.EnsureSuccessStatusCode();
                                        $rv = response.Content.ReadAsObjectLiteral(T);
                                        $s = 3;
                                        continue;
                                    }
                                    case 3: {
                                        System.Net.Http.HttpClient.FinishSend(cts.v, disposeCts.v);

                                        if ($jff > -1) {
                                            $s = $jff;
                                            $jff = null;
                                        } else if ($ae) {
                                            $tcs.setException($ae);
                                            return;
                                        } else if (H5.isDefined($rv)) {
                                            $tcs.setResult($rv);
                                            return;
                                        }
                                        $s = 4;
                                        continue;
                                    }
                                    case 4: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                    default: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                }
                            }
                        } catch($ae1) {
                            $ae = System.Exception.create($ae1);
                            if ($s >= 1 && $s <= 2) {
                                $s = 3;
                                $asyncBody();
                                return;
                            }
                            $tcs.setException($ae);
                        }
                    }, arguments);

                $asyncBody();
                return $tcs.task;
            },
            GetAsync: function (requestUri) {
                return this.GetAsync$2(this.CreateUri(requestUri));
            },
            GetAsync$2: function (requestUri) {
                return this.GetAsync$2(requestUri);
            },
            GetAsync$1: function (requestUri, cancellationToken) {
                return this.GetAsync$3(this.CreateUri(requestUri), cancellationToken);
            },
            GetAsync$3: function (requestUri, cancellationToken) {
                return this.SendAsync(this.CreateRequestMessage(System.Net.Http.HttpMethod.Get, requestUri), cancellationToken);
            },
            PostAsync: function (requestUri, content) {
                return this.PostAsync$2(this.CreateUri(requestUri), content);
            },
            PostAsync$2: function (requestUri, content) {
                return this.PostAsync$3(requestUri, content, System.Threading.CancellationToken.none);
            },
            PostAsync$1: function (requestUri, content, cancellationToken) {
                return this.PostAsync$3(this.CreateUri(requestUri), content, cancellationToken);
            },
            PostAsync$3: function (requestUri, content, cancellationToken) {
                var request = this.CreateRequestMessage(System.Net.Http.HttpMethod.Post, requestUri);
                request.Content = content;
                return this.SendAsync(request, cancellationToken);
            },
            PutAsync: function (requestUri, content) {
                return this.PutAsync$2(this.CreateUri(requestUri), content);
            },
            PutAsync$2: function (requestUri, content) {
                return this.PutAsync$3(requestUri, content, System.Threading.CancellationToken.none);
            },
            PutAsync$1: function (requestUri, content, cancellationToken) {
                return this.PutAsync$3(this.CreateUri(requestUri), content, cancellationToken);
            },
            PutAsync$3: function (requestUri, content, cancellationToken) {
                var request = this.CreateRequestMessage(System.Net.Http.HttpMethod.Put, requestUri);
                request.Content = content;
                return this.SendAsync(request, cancellationToken);
            },
            PatchAsync: function (requestUri, content) {
                return this.PatchAsync$2(this.CreateUri(requestUri), content);
            },
            PatchAsync$2: function (requestUri, content) {
                return this.PatchAsync$3(requestUri, content, System.Threading.CancellationToken.none);
            },
            PatchAsync$1: function (requestUri, content, cancellationToken) {
                return this.PatchAsync$3(this.CreateUri(requestUri), content, cancellationToken);
            },
            PatchAsync$3: function (requestUri, content, cancellationToken) {
                var request = this.CreateRequestMessage(System.Net.Http.HttpMethod.Patch, requestUri);
                request.Content = content;
                return this.SendAsync(request, cancellationToken);
            },
            DeleteAsync: function (requestUri) {
                return this.DeleteAsync$2(this.CreateUri(requestUri));
            },
            DeleteAsync$2: function (requestUri) {
                return this.DeleteAsync$3(requestUri, System.Threading.CancellationToken.none);
            },
            DeleteAsync$1: function (requestUri, cancellationToken) {
                return this.DeleteAsync$3(this.CreateUri(requestUri), cancellationToken);
            },
            DeleteAsync$3: function (requestUri, cancellationToken) {
                return this.SendAsync(this.CreateRequestMessage(System.Net.Http.HttpMethod.Delete, requestUri), cancellationToken);
            },
            SendAsync$1: function (request) {
                return this.SendAsync(request, System.Threading.CancellationToken.none);
            },
            SendAsync: function (request, cancellationToken) {
                var Core = null;
                // Called outside of async state machine to propagate certain exception even without awaiting the returned task.
                this.CheckRequestBeforeSend(request);
                var cts = { };
                var disposeCts = { };
                var pendingRequestsCts = { };
                H5.Deconstruct(this.PrepareCancellationTokenSource(cancellationToken).$clone(), cts, disposeCts, pendingRequestsCts);
                Core = H5.fn.bind(this, function () {
                    var $s = 0,
                        $t1, 
                        $tr1, 
                        $jff, 
                        $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                        $rv, 
                        response, 
                        $ae, 
                        $ae1, 
                        $asyncBody = H5.fn.bind(this, function () {
                            try {
                                for (;;) {
                                    $s = System.Array.min([1,2,3,4], $s);
                                    switch ($s) {

                                        case 1: {
                                            // Wait for the send request to complete, getting back the response.
                                            $t1 = System.Net.Http.HttpMessageInvoker.prototype.SendAsync.call(this, request, cts.v.token);
                                            $s = 2;
                                            if ($t1.isCompleted()) {
                                                continue;
                                            }
                                            $t1.continue($asyncBody);
                                            return;
                                        }
                                        case 2: {
                                            $tr1 = $t1.getAwaitedResult();
                                            response = $tr1;
                                            System.Net.Http.HttpClient.ThrowForNullResponse(response);
                                            $rv = response;
                                            $s = 3;
                                            continue;
                                        }
                                        case 3: {
                                            System.Net.Http.HttpClient.FinishSend(cts.v, disposeCts.v);

                                            if ($jff > -1) {
                                                $s = $jff;
                                                $jff = null;
                                            } else if ($ae) {
                                                $tcs.setException($ae);
                                                return;
                                            } else if (H5.isDefined($rv)) {
                                                $tcs.setResult($rv);
                                                return;
                                            }
                                            $s = 4;
                                            continue;
                                        }
                                        case 4: {
                                            $tcs.setResult(null);
                                            return;
                                        }
                                        default: {
                                            $tcs.setResult(null);
                                            return;
                                        }
                                    }
                                }
                            } catch($ae1) {
                                $ae = System.Exception.create($ae1);
                                if ($s >= 1 && $s <= 2) {
                                    $s = 3;
                                    $asyncBody();
                                    return;
                                }
                                $tcs.setException($ae);
                            }
                        }, arguments);

                    $asyncBody();
                    return $tcs.task;
                });

                return Core();


            },
            CheckRequestBeforeSend: function (request) {
                if (request == null) {
                    throw new System.ArgumentNullException.$ctor1("request");
                }

                this.CheckDisposed();
                System.Net.Http.HttpClient.CheckRequestMessage(request);

                this.SetOperationStarted();

                // PrepareRequestMessage will resolve the request address against the base address.
                this.PrepareRequestMessage(request);
            },
            CancelPendingRequests: function () {
                this.CheckDisposed();

                // With every request we link this cancellation token source.
                var currentCts = this._pendingRequestsCts;
                this._pendingRequestsCts = new System.Threading.CancellationTokenSource();

                currentCts.cancel();
                currentCts.dispose();
            },
            Dispose: function () {
                if (!this._disposed) {
                    this._disposed = true;

                    // Cancel all pending requests (if any). Note that we don't call CancelPendingRequests() but cancel
                    // the CTS directly. The reason is that CancelPendingRequests() would cancel the current CTS and create
                    // a new CTS. We don't want a new CTS in this case.
                    this._pendingRequestsCts.cancel();
                    this._pendingRequestsCts.dispose();
                }

                System.Net.Http.HttpMessageInvoker.prototype.Dispose.call(this);
            },
            SetOperationStarted: function () {
                // This method flags the HttpClient instances as "active". I.e. we executed at least one request (or are
                // in the process of doing so). This information is used to lock-down all property setters. Once a
                // Send/SendAsync operation started, no property can be changed.
                if (!this._operationStarted) {
                    this._operationStarted = true;
                }
            },
            CheckDisposedOrStarted: function () {
                this.CheckDisposed();
                if (this._operationStarted) {
                    throw new System.InvalidOperationException.$ctor1("This instance has already started one or more requests. Properties can only be modified before sending the first request.");
                }
            },
            CheckDisposed: function () {
                if (this._disposed) {
                    throw new System.ObjectDisposedException.$ctor1(H5.getTypeName(H5.getType(this)));
                }
            },
            PrepareRequestMessage: function (request) {
                var requestUri = null;
                if ((System.Uri.equals(request.RequestUri, null)) && (System.Uri.equals(this._baseAddress, null))) {
                    throw new System.InvalidOperationException.$ctor1("An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set.");
                }
                if (System.Uri.equals(request.RequestUri, null)) {
                    requestUri = this._baseAddress;
                } else {
                    // If the request Uri is an absolute Uri, just use it. Otherwise try to combine it with the base Uri.
                    if (!System.Net.Http.HttpClient.IsAbsoluteUri(request.RequestUri)) {
                        if (System.Uri.equals(this._baseAddress, null)) {
                            throw new System.InvalidOperationException.$ctor1("An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set.");
                        } else {
                            requestUri = new System.Uri((H5.toString(this._baseAddress) || "") + (H5.toString(request.RequestUri) || ""));
                        }
                    }
                }

                // We modified the original request Uri. Assign the new Uri to the request message.
                if (System.Uri.notEquals(requestUri, null)) {
                    request.RequestUri = requestUri;
                }

                // Add default headers
                if (this._defaultRequestHeaders != null) {
                    request.Headers.AddHeaders(this._defaultRequestHeaders);
                }
            },
            PrepareCancellationTokenSource: function (cancellationToken) {
                // We need a CancellationTokenSource to use with the request.  We always have the global
                // _pendingRequestsCts to use, plus we may have a token provided by the caller, and we may
                // have a timeout.  If we have a timeout or a caller-provided token, we need to create a new
                // CTS (we can't, for example, timeout the pending requests CTS, as that could cancel other
                // unrelated operations).  Otherwise, we can use the pending requests CTS directly.

                // Snapshot the current pending requests cancellation source. It can change concurrently due to cancellation being requested
                // and it being replaced, and we need a stable view of it: if cancellation occurs and the caller's token hasn't been canceled,
                // it's either due to this source or due to the timeout, and checking whether this source is the culprit is reliable whereas
                // it's more approximate checking elapsed time.
                var pendingRequestsCts = this._pendingRequestsCts;

                var hasTimeout = System.TimeSpan.neq(this._timeout, System.Net.Http.HttpClient.s_infiniteTimeout);
                if (hasTimeout || cancellationToken.getCanBeCanceled()) {
                    var cts = System.Threading.CancellationTokenSource.createLinked(cancellationToken, pendingRequestsCts.token);
                    if (hasTimeout) {
                        cts.cancelAfter(this._timeout.ticks / 10000);
                    }

                    return new (System.ValueTuple$3(System.Threading.CancellationTokenSource,System.Boolean,System.Threading.CancellationTokenSource)).$ctor1(cts, true, pendingRequestsCts);
                }

                return new (System.ValueTuple$3(System.Threading.CancellationTokenSource,System.Boolean,System.Threading.CancellationTokenSource)).$ctor1(pendingRequestsCts, false, pendingRequestsCts);
            },
            CreateUri: function (uri) {
                return System.String.isNullOrEmpty(uri) ? null : new System.Uri(uri);
            },
            CreateRequestMessage: function (method, uri) {
                return new System.Net.Http.HttpRequestMessage.$ctor2(method, uri);
            }
        }
    });

    H5.define("System.Net.Http.HttpClientHandler", {
        inherits: [System.Net.Http.HttpMessageHandler],
        fields: {
            _underlyingHandler: null
        },
        props: {
            Handler: null,
            AllowAutoRedirect: {
                get: function () {
                    return this._underlyingHandler.AllowAutoRedirect;
                },
                set: function (value) {
                    this._underlyingHandler.AllowAutoRedirect = value;
                }
            },
            MaxAutomaticRedirections: {
                get: function () {
                    return this._underlyingHandler.MaxAutomaticRedirections;
                },
                set: function (value) {
                    this._underlyingHandler.MaxAutomaticRedirections = value;
                }
            }
        },
        ctors: {
            ctor: function () {
                this.$initialize();
                System.Net.Http.HttpMessageHandler.ctor.call(this);
                this._underlyingHandler = new System.Net.Http.BrowserHttpHandler();

                this.Handler = this._underlyingHandler;
            }
        },
        methods: {
            SendAsync: function (request, cancellationToken) {
                return this.Handler.SendAsync(request, cancellationToken);
            }
        }
    });

    H5.define("System.Net.Http.StringContent", {
        inherits: [System.Net.Http.HttpContent],
        statics: {
            fields: {
                DefaultMediaType: null
            },
            ctors: {
                init: function () {
                    this.DefaultMediaType = "text/plain";
                }
            }
        },
        props: {
            Content: null,
            MediaType: null
        },
        ctors: {
            ctor: function (content) {
                System.Net.Http.StringContent.$ctor1.call(this, content, System.Net.Http.StringContent.DefaultMediaType);
            },
            $ctor1: function (content, mediaType) {
                this.$initialize();
                System.Net.Http.HttpContent.ctor.call(this);
                this.MediaType = mediaType;
                this.Content = content;
            }
        }
    });
