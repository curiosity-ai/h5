    H5.define("System.Threading.Tasks.Task", {
        inherits: [System.IDisposable, System.IAsyncResult],

        config: {
            alias: [
                "dispose", "System$IDisposable$Dispose",
                "AsyncState", "System$IAsyncResult$AsyncState",
                "CompletedSynchronously", "System$IAsyncResult$CompletedSynchronously",
                "IsCompleted", "System$IAsyncResult$IsCompleted"
            ],

            properties: {
                IsCompleted: {
                    get: function () {
                        return this.isCompleted();
                    }
                }
            }
        },

        ctor: function (action, state) {
            this.$initialize();
            this.action = action;
            this.state = state;
            this.AsyncState = state;
            this.CompletedSynchronously = false;
            this.exception = null;
            this.status = System.Threading.Tasks.TaskStatus.created;
            this.callbacks = [];
            this.result = null;
        },

        statics: {
            queue: [],

            runQueue: function () {
                var queue = System.Threading.Tasks.Task.queue.slice(0);
                System.Threading.Tasks.Task.queue = [];

                for (var i = 0; i < queue.length; i++) {
                    queue[i]();
                }
            },

            schedule: function (fn) {
                System.Threading.Tasks.Task.queue.push(fn);
                H5.setImmediate(System.Threading.Tasks.Task.runQueue);
            },

            delay: function (delay, state) {
                var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                    token,
                    cancelCallback = false;

                if (H5.is(state, System.Threading.CancellationToken)) {
                    token = state;
                    state = undefined;
                }

                if (token) {
                    token.cancelWasRequested = function () {
                        if (!cancelCallback) {
                            cancelCallback = true;
                            clearTimeout(clear);

                            tcs.setCanceled();
                        }
                    };
                }

                var ms = delay;
                if (H5.is(delay, System.TimeSpan)) {
                    ms = delay.getTotalMilliseconds();
                }

                var clear = setTimeout(function () {
                    if (!cancelCallback) {
                        cancelCallback = true;
                        tcs.setResult(state);
                    }
                }, ms);

                if (token && token.getIsCancellationRequested()) {
                    H5.setImmediate(token.cancelWasRequested);
                }

                return tcs.task;
            },

            fromResult: function (result, T) {
                var t = new (System.Threading.Tasks.Task$1(T || System.Object))();

                t.status = System.Threading.Tasks.TaskStatus.ranToCompletion;
                t.result = result;

                return t;
            },

            fromException: function (exception, T) {
                var t = new (System.Threading.Tasks.Task$1(T || System.Object))();

                t.status = System.Threading.Tasks.TaskStatus.faulted;
                t.exception = exception;

                return t;
            },            

            run: function (fn) {
                var tcs = new System.Threading.Tasks.TaskCompletionSource();

                System.Threading.Tasks.Task.schedule(function () {
                    try {
                        var result = fn();

                        if (H5.is(result, System.Threading.Tasks.Task)) {
                            result.continueWith(function () {
                                if (result.isFaulted() || result.isCanceled()) {
                                    tcs.setException(result.exception.innerExceptions.Count > 0 ? result.exception.innerExceptions.getItem(0) : result.exception);
                                } else {
                                    tcs.setResult(result.getAwaitedResult());
                                }
                            });
                        } else {
                            tcs.setResult(result);
                        }
                    } catch (e) {
                        tcs.setException(System.Exception.create(e));
                    }
                });

                return tcs.task;
            },

            whenAll: function (tasks) {
                var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                    result,
                    executing,
                    cancelled = false,
                    exceptions = [],
                    i;

                if (H5.is(tasks, System.Collections.IEnumerable)) {
                    tasks = H5.toArray(tasks);
                } else if (!H5.isArray(tasks)) {
                    tasks = Array.prototype.slice.call(arguments, 0);
                }

                if (tasks.length === 0) {
                    tcs.setResult([]);

                    return tcs.task;
                }

                executing = tasks.length;
                result = new Array(tasks.length);

                for (i = 0; i < tasks.length; i++) {
                    (function (i) {
                        tasks[i].continueWith(function (t) {
                            switch (t.status) {
                                case System.Threading.Tasks.TaskStatus.ranToCompletion:
                                    result[i] = t.getResult();
                                    break;
                                case System.Threading.Tasks.TaskStatus.canceled:
                                    cancelled = true;
                                    break;
                                case System.Threading.Tasks.TaskStatus.faulted:
                                    System.Array.addRange(exceptions, t.exception.innerExceptions);
                                    break;
                                default:
                                    throw new System.InvalidOperationException.$ctor1("Invalid task status: " + t.status);
                            }

                            if (--executing === 0) {
                                if (exceptions.length > 0) {
                                    tcs.setException(exceptions);
                                } else if (cancelled) {
                                    tcs.setCanceled();
                                } else {
                                    tcs.setResult(result);
                                }
                            }
                        });
                    })(i);
                }

                return tcs.task;
            },

            whenAny: function (tasks) {
                if (H5.is(tasks, System.Collections.IEnumerable)) {
                    tasks = H5.toArray(tasks);
                } else if (!H5.isArray(tasks)) {
                    tasks = Array.prototype.slice.call(arguments, 0);
                }

                if (!tasks.length) {
                    throw new System.ArgumentException.$ctor1("At least one task is required");
                }

                var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                    i;

                for (i = 0; i < tasks.length; i++) {
                    tasks[i].continueWith(function (t) {
                        switch (t.status) {
                            case System.Threading.Tasks.TaskStatus.ranToCompletion:
                                tcs.trySetResult(t);
                                break;
                            case System.Threading.Tasks.TaskStatus.canceled:
                            case System.Threading.Tasks.TaskStatus.faulted:
                                tcs.trySetException(t.exception.innerExceptions);
                                break;
                            default:
                                throw new System.InvalidOperationException.$ctor1("Invalid task status: " + t.status);
                        }
                    });
                }

                return tcs.task;
            },

            fromCallback: function (target, method) {
                var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                    args = Array.prototype.slice.call(arguments, 2),
                    callback;

                callback = function (value) {
                    tcs.setResult(value);
                };

                args.push(callback);

                target[method].apply(target, args);

                return tcs.task;
            },

            fromCallbackResult: function (target, method, resultHandler) {
                var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                    args = Array.prototype.slice.call(arguments, 3),
                    callback;

                callback = function (value) {
                    tcs.setResult(value);
                };

                resultHandler(args, callback);

                target[method].apply(target, args);

                return tcs.task;
            },

            fromCallbackOptions: function (target, method, name) {
                var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                    args = Array.prototype.slice.call(arguments, 3),
                    callback;

                callback = function (value) {
                    tcs.setResult(value);
                };

                args[0] = args[0] || {};
                args[0][name] = callback;

                target[method].apply(target, args);

                return tcs.task;
            },

            fromPromise: function (promise, handler, errorHandler, progressHandler) {
                var tcs = new System.Threading.Tasks.TaskCompletionSource();

                if (!promise.then) {
                    promise = promise.promise();
                }

                if (typeof (handler) === 'number') {
                    handler = (function (i) {
                        return function () {
                            return arguments[i >= 0 ? i : (arguments.length + i)];
                        };
                    })(handler);
                } else if (typeof (handler) !== 'function') {
                    handler = function () {
                        return Array.prototype.slice.call(arguments, 0);
                    };
                }

                promise.then(function () {
                    tcs.setResult(handler ? handler.apply(null, arguments) : Array.prototype.slice.call(arguments, 0));
                }, function () {
                    tcs.setException(errorHandler ? errorHandler.apply(null, arguments) : new H5.PromiseException(Array.prototype.slice.call(arguments, 0)));
                }, progressHandler);

                return tcs.task;
            }
        },

        getException: function () {
            return this.isCanceled() ? null : this.exception;
        },

        waitt: function (timeout, token) {
            var ms = timeout,
                tcs = new System.Threading.Tasks.TaskCompletionSource(),
                cancelCallback = false;

            if (token) {
                token.cancelWasRequested = function () {
                    if (!cancelCallback) {
                        cancelCallback = true;
                        clearTimeout(clear);
                        tcs.setException(new System.OperationCanceledException.$ctor1(token));
                    }
                };
            }

            if (H5.is(timeout, System.TimeSpan)) {
                ms = timeout.getTotalMilliseconds();
            }

            var clear = setTimeout(function () {
                cancelCallback = true;
                tcs.setResult(false);
            }, ms);

            this.continueWith(function () {
                clearTimeout(clear);
                if (!cancelCallback) {
                    cancelCallback = true;
                    tcs.setResult(true);
                }
            })

            return tcs.task;
        },

        wait: function (token) {
            var me = this,
                tcs = new System.Threading.Tasks.TaskCompletionSource(),
                complete = false;

            if (token) {
                token.cancelWasRequested = function () {
                    if (!complete) {
                        complete = true;
                        tcs.setException(new System.OperationCanceledException.$ctor1(token));
                    }
                };
            }

            this.continueWith(function () {
                if (!complete) {
                    complete = true;
                    if (me.isFaulted() || me.isCanceled()) {
                        tcs.setException(me.exception);
                    } else {
                        tcs.setResult();
                    }
                }
            })

            return tcs.task;
        },

        c: function (continuationAction) {
            if (this.isCompleted()) {
                System.Threading.Tasks.Task.queue.push(continuationAction);
                System.Threading.Tasks.Task.runQueue();
            } else {
                this.callbacks.push(continuationAction);
            }
        },

        continue: function (continuationAction) {
            if (this.isCompleted()) {
                System.Threading.Tasks.Task.queue.push(continuationAction);
                System.Threading.Tasks.Task.runQueue();
            } else {
                this.callbacks.push(continuationAction);
            }
        },

        continueWith: function (continuationAction, raise) {
            var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                me = this,
                fn = raise ? function () {
                    tcs.setResult(continuationAction(me));
                } : function () {
                    try {
                        tcs.setResult(continuationAction(me));
                    } catch (e) {
                        tcs.setException(System.Exception.create(e));
                    }
                };

            if (this.isCompleted()) {
                //System.Threading.Tasks.Task.schedule(fn);
                System.Threading.Tasks.Task.queue.push(fn);
                System.Threading.Tasks.Task.runQueue();
            } else {
                this.callbacks.push(fn);
            }

            return tcs.task;
        },

        start: function () {
            if (this.status !== System.Threading.Tasks.TaskStatus.created) {
                throw new System.InvalidOperationException.$ctor1("Task was already started.");
            }

            var me = this;

            this.status = System.Threading.Tasks.TaskStatus.running;

            System.Threading.Tasks.Task.schedule(function () {
                try {
                    var result = me.action(me.state);

                    delete me.action;
                    delete me.state;

                    me.complete(result);
                } catch (e) {
                    me.fail(new System.AggregateException(null, [System.Exception.create(e)]));
                }
            });
        },

        runCallbacks: function () {
            var me = this;

            for (var i = 0; i < me.callbacks.length; i++) {
                me.callbacks[i](me);
            }

            delete me.callbacks;
        },

        complete: function (result) {
            if (this.isCompleted()) {
                return false;
            }

            this.result = result;
            this.status = System.Threading.Tasks.TaskStatus.ranToCompletion;
            this.runCallbacks();

            return true;
        },

        fail: function (error) {
            if (this.isCompleted()) {
                return false;
            }

            this.exception = error;
            this.status = this.exception.hasTaskCanceledException && this.exception.hasTaskCanceledException() ? System.Threading.Tasks.TaskStatus.canceled : System.Threading.Tasks.TaskStatus.faulted;
            this.runCallbacks();

            return true;
        },

        cancel: function (error) {
            if (this.isCompleted()) {
                return false;
            }

            this.exception = error || new System.AggregateException(null, [new System.Threading.Tasks.TaskCanceledException.$ctor3(this)]);
            this.status = System.Threading.Tasks.TaskStatus.canceled;
            this.runCallbacks();

            return true;
        },

        isCanceled: function () {
            return this.status === System.Threading.Tasks.TaskStatus.canceled;
        },

        isCompleted: function () {
            return this.status === System.Threading.Tasks.TaskStatus.ranToCompletion || this.status === System.Threading.Tasks.TaskStatus.canceled || this.status === System.Threading.Tasks.TaskStatus.faulted;
        },

        isC: function () {
            return this.status === System.Threading.Tasks.TaskStatus.ranToCompletion || this.status === System.Threading.Tasks.TaskStatus.canceled || this.status === System.Threading.Tasks.TaskStatus.faulted;
        },

        isFaulted: function () {
            return this.status === System.Threading.Tasks.TaskStatus.faulted;
        },

        _getResult: function (awaiting) {
            switch (this.status) {
                case System.Threading.Tasks.TaskStatus.ranToCompletion:
                    return this.result;
                case System.Threading.Tasks.TaskStatus.canceled:
                    if (this.exception && this.exception.innerExceptions) {
                        throw awaiting ? (this.exception.innerExceptions.Count > 0 ? this.exception.innerExceptions.getItem(0) : null) : this.exception;
                    }

                    var ex = new System.Threading.Tasks.TaskCanceledException.$ctor3(this);
                    throw awaiting ? ex : new System.AggregateException(null, [ex]);
                case System.Threading.Tasks.TaskStatus.faulted:
                    throw awaiting ? (this.exception.innerExceptions.Count > 0 ? this.exception.innerExceptions.getItem(0) : null) : this.exception;
                default:
                    throw new System.InvalidOperationException.$ctor1("Task is not yet completed.");
            }
        },

        getResult: function () {
            return this._getResult(false);
        },

        dispose: function () {},

        getAwaiter: function () {
            return this;
        },

        getAwaitedResult: function () {
            return this._getResult(true);
        },

        gAR: function () {
            return this._getResult(true);
        }

    });

    H5.define("System.Threading.Tasks.Task$1", function (T) {
        return {
            inherits: [System.Threading.Tasks.Task],
            ctor: function (action, state) {
                this.$initialize();
                System.Threading.Tasks.Task.ctor.call(this, action, state);
            }
        };
    });

    H5.define("System.Threading.Tasks.TaskStatus", {
        $kind: "enum",
        $statics: {
            created: 0,
            waitingForActivation: 1,
            waitingToRun: 2,
            running: 3,
            waitingForChildrenToComplete: 4,
            ranToCompletion: 5,
            canceled: 6,
            faulted: 7
        }
    });

    H5.define("System.Threading.Tasks.TaskCompletionSource", {
        ctor: function (state) {
            this.$initialize();
            this.task = new System.Threading.Tasks.Task(null, state);
            this.task.status = System.Threading.Tasks.TaskStatus.running;
        },

        setCanceled: function () {
            if (!this.task.cancel()) {
                throw new System.InvalidOperationException.$ctor1("Task was already completed.");
            }
        },

        sR: function (result) {
            if (!this.task.complete(result)) {
                throw new System.InvalidOperationException.$ctor1("Task was already completed.");
            }
        },

        setResult: function (result) {
            if (!this.task.complete(result)) {
                throw new System.InvalidOperationException.$ctor1("Task was already completed.");
            }
        },

        setException: function (exception) {
            if (!this.trySetException(exception)) {
                throw new System.InvalidOperationException.$ctor1("Task was already completed.");
            }
        },

        sE: function (exception) {
            if (!this.trySetException(exception)) {
                throw new System.InvalidOperationException.$ctor1("Task was already completed.");
            }
        },

        trySetCanceled: function () {
            return this.task.cancel();
        },

        trySetResult: function (result) {
            return this.task.complete(result);
        },

        trySetException: function (exception) {
            if (H5.is(exception, System.Exception)) {
                exception = [exception];
            }

            exception = new System.AggregateException(null, exception);

            if (exception.hasTaskCanceledException()) {
                return this.task.cancel(exception);
            }

            return this.task.fail(exception);
        }
    });

    H5.define("System.Threading.CancellationTokenSource", {
        inherits: [System.IDisposable],

        config: {
            alias: [
                "dispose", "System$IDisposable$Dispose"
            ]
        },

        ctor: function (delay) {
            this.$initialize();
            this.timeout = typeof delay === "number" && delay >= 0 ? setTimeout(H5.fn.bind(this, this.cancel), delay, -1) : null;
            this.isCancellationRequested = false;
            this.token = new System.Threading.CancellationToken(this);
            this.handlers = [];
        },

        cancel: function (throwFirst) {
            if (this.isCancellationRequested) {
                return;
            }

            this.isCancellationRequested = true;

            var x = [],
                h = this.handlers;

            this.clean();
            this.token.cancelWasRequested();

            for (var i = 0; i < h.length; i++) {
                try {
                    h[i].f(h[i].s);
                } catch (ex) {
                    if (throwFirst && throwFirst !== -1) {
                        throw ex;
                    }

                    x.push(ex);
                }
            }

            if (x.length > 0 && throwFirst !== -1) {
                throw new System.AggregateException(null, x);
            }
        },

        cancelAfter: function (delay) {
            if (this.isCancellationRequested) {
                return;
            }

            if (this.timeout) {
                clearTimeout(this.timeout);
            }

            this.timeout = setTimeout(H5.fn.bind(this, this.cancel), delay, -1);
        },

        register: function (f, s) {
            if (this.isCancellationRequested) {
                f(s);

                return new System.Threading.CancellationTokenRegistration();
            } else {
                var o = {
                    f: f,
                    s: s
                };

                this.handlers.push(o);

                return new System.Threading.CancellationTokenRegistration(this, o);
            }
        },

        deregister: function (o) {
            var ix = this.handlers.indexOf(o);

            if (ix >= 0) {
                this.handlers.splice(ix, 1);
            }
        },

        dispose: function () {
            this.clean();
        },

        clean: function () {
            if (this.timeout) {
                clearTimeout(this.timeout);
            }

            this.timeout = null;
            this.handlers = [];

            if (this.links) {
                for (var i = 0; i < this.links.length; i++) {
                    this.links[i].dispose();
                }

                this.links = null;
            }
        },

        statics: {
            createLinked: function () {
                var cts = new System.Threading.CancellationTokenSource();

                cts.links = [];

                var d = H5.fn.bind(cts, cts.cancel);

                for (var i = 0; i < arguments.length; i++) {
                    cts.links.push(arguments[i].register(d));
                }

                return cts;
            }
        }
    });

    H5.define("System.Threading.CancellationToken", {
         $kind: "struct",

        ctor: function (source) {
            this.$initialize();

            if (!H5.is(source, System.Threading.CancellationTokenSource)) {
                source = source ? System.Threading.CancellationToken.sourceTrue : System.Threading.CancellationToken.sourceFalse;
            }

            this.source = source;
        },

        cancelWasRequested: function () {

        },

        getCanBeCanceled: function () {
            return !this.source.uncancellable;
        },

        getIsCancellationRequested: function () {
            return this.source.isCancellationRequested;
        },

        throwIfCancellationRequested: function () {
            if (this.source.isCancellationRequested) {
                throw new System.OperationCanceledException.$ctor1(this);
            }
        },

        register: function (cb, s) {
            return this.source.register(cb, s);
        },

        getHashCode: function () {
            return H5.getHashCode(this.source);
        },

        equals: function (other) {
            return other.source === this.source;
        },

        equalsT: function (other) {
            return other.source === this.source;
        },

        statics: {
            sourceTrue: {
                isCancellationRequested: true,
                register: function (f, s) {
                    f(s);

                    return new System.Threading.CancellationTokenRegistration();
                }
            },
            sourceFalse: {
                uncancellable: true,
                isCancellationRequested: false,
                register: function () {
                    return new System.Threading.CancellationTokenRegistration();
                }
            },
            getDefaultValue: function () {
                return new System.Threading.CancellationToken();
            }
        }
    });

    System.Threading.CancellationToken.none = new System.Threading.CancellationToken();

    H5.define("System.Threading.CancellationTokenRegistration", {
        inherits: function () {
            return [System.IDisposable, System.IEquatable$1(System.Threading.CancellationTokenRegistration)];
        },

        $kind: "struct",

        config: {
            alias: [
                "dispose", "System$IDisposable$Dispose"
            ]
        },

        ctor: function (cts, o) {
            this.$initialize();
            this.cts = cts;
            this.o = o;
        },

        dispose: function () {
            if (this.cts) {
                this.cts.deregister(this.o);
                this.cts = this.o = null;
            }
        },

        equalsT: function (o) {
            return this === o;
        },

        equals: function (o) {
            return this === o;
        },

        statics: {
            getDefaultValue: function () {
                return new System.Threading.CancellationTokenRegistration();
            }
        }
    });
