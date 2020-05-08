    H5.define("System.Net.WebSockets.ClientWebSocket", {
        inherits: [System.IDisposable],

        ctor: function () {
            this.$initialize();
            this.messageBuffer = [];
            this.state = "none";
            this.options = new System.Net.WebSockets.ClientWebSocketOptions();
            this.disposed = false;
            this.closeStatus = null;
            this.closeStatusDescription = null;
        },

        getCloseStatus: function () {
            return this.closeStatus;
        },

        getState: function () {
            return this.state;
        },

        getCloseStatusDescription: function () {
            return this.closeStatusDescription;
        },

        getSubProtocol: function () {
            return this.socket ? this.socket.protocol : null;
        },

        onCloseHandler: function(event) {
            var reason,
                success = false;

            // See http://tools.ietf.org/html/rfc6455#section-7.4.1
            if (event.code == 1000) {
                reason = "Status code: " + event.code + ". Normal closure, meaning that the purpose for which the connection was established has been fulfilled.";
                success = true;
            } else if (event.code == 1001)
                reason = "Status code: " + event.code + ". An endpoint is \"going away\", such as a server going down or a browser having navigated away from a page.";
            else if (event.code == 1002)
                reason = "Status code: " + event.code + ". An endpoint is terminating the connection due to a protocol error";
            else if (event.code == 1003)
                reason = "Status code: " + event.code + ". An endpoint is terminating the connection because it has received a type of data it cannot accept (e.g., an endpoint that understands only text data MAY send this if it receives a binary message).";
            else if (event.code == 1004)
                reason = "Status code: " + event.code + ". Reserved. The specific meaning might be defined in the future.";
            else if (event.code == 1005)
                reason = "Status code: " + event.code + ". No status code was actually present.";
            else if (event.code == 1006)
                reason = "Status code: " + event.code + ". The connection was closed abnormally, e.g., without sending or receiving a Close control frame";
            else if (event.code == 1007)
                reason = "Status code: " + event.code + ". An endpoint is terminating the connection because it has received data within a message that was not consistent with the type of the message (e.g., non-UTF-8 [http://tools.ietf.org/html/rfc3629] data within a text message).";
            else if (event.code == 1008)
                reason = "Status code: " + event.code + ". An endpoint is terminating the connection because it has received a message that \"violates its policy\". This reason is given either if there is no other sutible reason, or if there is a need to hide specific details about the policy.";
            else if (event.code == 1009)
                reason = "Status code: " + event.code + ". An endpoint is terminating the connection because it has received a message that is too big for it to process.";
            else if (event.code == 1010) // Note that this status code is not used by the server, because it can fail the WebSocket handshake instead.
                reason = "Status code: " + event.code + ". An endpoint (client) is terminating the connection because it has expected the server to negotiate one or more extension, but the server didn't return them in the response message of the WebSocket handshake. <br /> Specifically, the extensions that are needed are: " + event.reason;
            else if (event.code == 1011)
                reason = "Status code: " + event.code + ". A server is terminating the connection because it encountered an unexpected condition that prevented it from fulfilling the request.";
            else if (event.code == 1015)
                reason = "Status code: " + event.code + ". The connection was closed due to a failure to perform a TLS handshake (e.g., the server certificate can't be verified).";
            else
                reason = "Unknown reason";

            return {
                code: event.code,
                reason: reason
            };
        },

        connectAsync: function (uri, cancellationToken) {
            if (this.state !== "none") {
                throw new System.InvalidOperationException.$ctor1("Socket is not in initial state");
            }

            this.options.setToReadOnly();
            this.state = "connecting";

            var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                self = this;

            try {
                this.socket = new WebSocket(uri.getAbsoluteUri(), this.options.requestedSubProtocols);

                this.socket.onerror = function (e) {
                    setTimeout(function () {
                        if (self.closeInfo && !self.closeInfo.success) {
                            e.message = self.closeInfo.reason;
                        }
                        tcs.setException(System.Exception.create(e));
                    }, 10);
                };

                this.socket.binaryType = "arraybuffer";
                this.socket.onopen = function () {
                    self.state = "open";
                    tcs.setResult(null);
                };

                this.socket.onmessage = function (e) {
                    var data = e.data,
                        message = {},
                        i;

                    message.bytes = [];

                    if (typeof (data) === "string") {
                        for (i = 0; i < data.length; ++i) {
                            message.bytes.push(data.charCodeAt(i));
                        }

                        message.messageType = "text";
                        self.messageBuffer.push(message);

                        return;
                    }

                    if (data instanceof ArrayBuffer) {
                        var dataView = new Uint8Array(data);

                        for (i = 0; i < dataView.length; i++) {
                            message.bytes.push(dataView[i]);
                        }

                        message.messageType = "binary";
                        self.messageBuffer.push(message);

                        return;
                    }

                    throw new System.ArgumentException.$ctor1("Invalid message type.");
                };

                this.socket.onclose = function (e) {
                    self.state = "closed";
                    self.closeStatus = e.code;
                    self.closeStatusDescription = e.reason;
                    self.closeInfo = self.onCloseHandler(e);
                }
            } catch (e) {
                tcs.setException(System.Exception.create(e));
            }

            return tcs.task;
        },

        sendAsync: function (buffer, messageType, endOfMessage, cancellationToken) {
            this.throwIfNotConnected();

            var tcs = new System.Threading.Tasks.TaskCompletionSource();

            try {
                if (messageType === "close") {
                    this.socket.close();
                } else {
                    var array = buffer.getArray(),
                        count = buffer.getCount(),
                        offset = buffer.getOffset();

                    var data = new Uint8Array(count);

                    for (var i = 0; i < count; i++) {
                        data[i] = array[i + offset];
                    }

                    if (messageType === "text") {
                        data = String.fromCharCode.apply(null, data);
                    }

                    this.socket.send(data);
                }

                tcs.setResult(null);
            } catch (e) {
                tcs.setException(System.Exception.create(e));
            }

            return tcs.task;
        },

        receiveAsync: function (buffer, cancellationToken) {
            this.throwIfNotConnected();

            var task,
                tcs = new System.Threading.Tasks.TaskCompletionSource(),
                self = this,
                asyncBody = H5.fn.bind(this, function () {
                    try {
                        if (cancellationToken.getIsCancellationRequested()) {
                            tcs.setException(new System.Threading.Tasks.TaskCanceledException("Receive has been cancelled.", tcs.task));

                            return;
                        }

                        if (self.messageBuffer.length === 0) {
                            task = System.Threading.Tasks.Task.delay(0);
                            task.continueWith(asyncBody);

                            return;
                        }

                        var message = self.messageBuffer[0],
                            array = buffer.getArray(),
                            resultBytes,
                            endOfMessage;

                        if (message.bytes.length <= array.length) {
                            self.messageBuffer.shift();
                            resultBytes = message.bytes;
                            endOfMessage = true;
                        } else {
                            resultBytes = message.bytes.slice(0, array.length);
                            message.bytes = message.bytes.slice(array.length, message.bytes.length);
                            endOfMessage = false;
                        }

                        for (var i = 0; i < resultBytes.length; i++) {
                            array[i] = resultBytes[i];
                        }

                        tcs.setResult(new System.Net.WebSockets.WebSocketReceiveResult(
                            resultBytes.length, message.messageType, endOfMessage));
                    } catch (e) {
                        tcs.setException(System.Exception.create(e));
                    }
                }, arguments);

            asyncBody();

            return tcs.task;
        },

        closeAsync: function (closeStatus, statusDescription, cancellationToken) {
            this.throwIfNotConnected();

            if (this.state !== "open") {
                throw new System.InvalidOperationException.$ctor1("Socket is not in connected state");
            }

            var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                self = this,
                task,
                asyncBody = function () {
                    if (self.state === "closed") {
                        tcs.setResult(null);
                        return;
                    }

                    if (cancellationToken.getIsCancellationRequested()) {
                        tcs.setException(new System.Threading.Tasks.TaskCanceledException("Closing has been cancelled.", tcs.task));
                        return;
                    }

                    task = System.Threading.Tasks.Task.delay(0);
                    task.continueWith(asyncBody);
                };
            try {
                this.state = "closesent";
                this.socket.close(closeStatus, statusDescription);
            } catch (e) {
                tcs.setException(System.Exception.create(e));
            }

            asyncBody();

            return tcs.task;
        },

        closeOutputAsync: function (closeStatus, statusDescription, cancellationToken) {
            this.throwIfNotConnected();

            if (this.state !== "open") {
                throw new System.InvalidOperationException.$ctor1("Socket is not in connected state");
            }

            var tcs = new System.Threading.Tasks.TaskCompletionSource();

            try {
                this.state = "closesent";
                this.socket.close(closeStatus, statusDescription);
                tcs.setResult(null);
            } catch (e) {
                tcs.setException(System.Exception.create(e));
            }

            return tcs.task;
        },

        abort: function () {
            this.Dispose();
        },

        Dispose: function () {
            if (this.disposed) {
                return;
            }

            this.disposed = true;
            this.messageBuffer = [];

            if (state === "open") {
                this.state = "closesent";
                this.socket.close();
            }
        },

        throwIfNotConnected: function () {
            if (this.disposed) {
                throw new System.InvalidOperationException.$ctor1("Socket is disposed.");
            }

            if (this.socket.readyState !== 1) {
                throw new System.InvalidOperationException.$ctor1("Socket is not connected.");
            }
        }
    });

    H5.define("System.Net.WebSockets.ClientWebSocketOptions", {
        ctor: function () {
            this.$initialize();
            this.isReadOnly = false;
            this.requestedSubProtocols = [];
        },

        setToReadOnly: function () {
            if (this.isReadOnly) {
                throw new System.InvalidOperationException.$ctor1("Options are already readonly.");
            }

            this.isReadOnly = true;
        },

        addSubProtocol: function (subProtocol) {
            if (this.isReadOnly) {
                throw new System.InvalidOperationException.$ctor1("Socket already started.");
            }

            if (this.requestedSubProtocols.indexOf(subProtocol) > -1) {
                throw new System.ArgumentException.$ctor1("Socket cannot have duplicate sub-protocols.", "subProtocol");
            }

            this.requestedSubProtocols.push(subProtocol);
        }
    });

    H5.define("System.Net.WebSockets.WebSocketReceiveResult", {
        ctor: function (count, messageType, endOfMessage, closeStatus, closeStatusDescription) {
            this.$initialize();
            this.count = count;
            this.messageType = messageType;
            this.endOfMessage = endOfMessage;
            this.closeStatus = closeStatus;
            this.closeStatusDescription = closeStatusDescription;
        },

        getCount: function () {
            return this.count;
        },

        getMessageType: function () {
            return this.messageType;
        },

        getEndOfMessage: function () {
            return this.endOfMessage;
        },

        getCloseStatus: function () {
            return this.closeStatus;
        },

        getCloseStatusDescription: function () {
            return this.closeStatusDescription;
        }
    });
