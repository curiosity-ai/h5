/*--------------------------------------------------------------------------
 * linq.js - LINQ for JavaScript
 * ver 3.0.4-Beta5 (Jun. 20th, 2013)
 *
 * created and maintained by neuecc <ils@neue.cc>
 * licensed under MIT License
 * http://linqjs.codeplex.com/
 *------------------------------------------------------------------------*/

(function (root, undefined) {
    // ReadOnly Function
    var Functions = {
        Identity: function (x) { return x; },
        True: function () { return true; },
        Blank: function () { }
    };

    // const Type
    var Types = {
        Boolean: typeof true,
        Number: typeof 0,
        String: typeof "",
        Object: typeof {},
        Undefined: typeof undefined,
        Function: typeof function () { }
    };

    // createLambda cache
    var funcCache = { "": Functions.Identity };

    // private utility methods
    var Utils = {
        // Create anonymous function from lambda expression string
        createLambda: function (expression) {
            if (expression == null) return Functions.Identity;
            if (typeof expression === Types.String) {
                // get from cache
                var f = funcCache[expression];
                if (f != null) {
                    return f;
                }

                if (expression.indexOf("=>") === -1) {
                    var regexp = new RegExp("[$]+", "g");

                    var maxLength = 0;
                    var match;
                    while ((match = regexp.exec(expression)) != null) {
                        var paramNumber = match[0].length;
                        if (paramNumber > maxLength) {
                            maxLength = paramNumber;
                        }
                    }

                    var argArray = [];
                    for (var i = 1; i <= maxLength; i++) {
                        var dollar = "";
                        for (var j = 0; j < i; j++) {
                            dollar += "$";
                        }
                        argArray.push(dollar);
                    }

                    var args = Array.prototype.join.call(argArray, ",");

                    f = new Function(args, "return " + expression);
                    funcCache[expression] = f;
                    return f;
                }
                else {
                    var expr = expression.match(/^[(\s]*([^()]*?)[)\s]*=>(.*)/);
                    f = new Function(expr[1], "return " + expr[2]);
                    funcCache[expression] = f;
                    return f;
                }
            }
            return expression;
        },

        isIEnumerable: function (obj) {
            if (typeof Enumerator !== Types.Undefined) {
                try {
                    new Enumerator(obj); // check JScript(IE)'s Enumerator
                    return true;
                }
                catch (e) { }
            }

            return false;
        },

        // IE8's defineProperty is defined but cannot use, therefore check defineProperties
        defineProperty: (Object.defineProperties != null)
            ? function (target, methodName, value) {
                Object.defineProperty(target, methodName, {
                    enumerable: false,
                    configurable: true,
                    writable: true,
                    value: value
                })
            }
            : function (target, methodName, value) {
                target[methodName] = value;
            },

        compare: function (a, b) {
            return (a === b) ? 0
                 : (a > b) ? 1
                 : -1;
        },

        Dispose: function (obj) {
            if (obj != null) obj.Dispose();
        }
    };

    // IEnumerator State
    var State = { Before: 0, Running: 1, After: 2 };

    // "Enumerator" is conflict JScript's "Enumerator"
    var IEnumerator = function (initialize, tryGetNext, dispose) {
        var yielder = new Yielder();
        var state = State.Before;

        this.getCurrent = yielder.getCurrent;
        this.reset = function () { throw new Error("Reset is not supported"); };

        this.moveNext = function () {
            try {
                switch (state) {
                    case State.Before:
                        state = State.Running;
                        initialize();
                        // fall through
                    case State.Running:
                        if (tryGetNext.apply(yielder)) {
                            return true;
                        }
                        else {
                            this.Dispose();
                            return false;
                        }
                    case State.After:
                        return false;
                }
            }
            catch (e) {
                this.Dispose();
                throw e;
            }
        };

        this.Dispose = function () {
            if (state != State.Running) return;

            try {
                dispose();
            }
            finally {
                state = State.After;
            }
        };

        this.System$IDisposable$Dispose = this.Dispose;
        this.getCurrent$1 = this.getCurrent;
        this.System$Collections$IEnumerator$getCurrent = this.getCurrent;
        this.System$Collections$IEnumerator$moveNext = this.moveNext;
        this.System$Collections$IEnumerator$reset = this.reset;

        Object.defineProperties(this,
        {
            "Current$1": {
                get: this.getCurrent,
                enumerable: true
            },

            "Current": {
                get: this.getCurrent,
                enumerable: true
            },

            "System$Collections$IEnumerator$Current": {
                get: this.getCurrent,
                enumerable: true
            }
        });
    };

    IEnumerator.$$inherits = [];
    H5.Class.addExtend(IEnumerator, [System.IDisposable, System.Collections.IEnumerator]);

    // for tryGetNext
    var Yielder = function () {
        var current = null;
        this.getCurrent = function () { return current; };
        this.yieldReturn = function (value) {
            current = value;
            return true;
        };
        this.yieldBreak = function () {
            return false;
        };
    };

    // Enumerable constuctor
    var Enumerable = function (GetEnumerator) {
        this.GetEnumerator = GetEnumerator;
    };

    Enumerable.$$inherits = [];
    H5.Class.addExtend(Enumerable, [System.Collections.IEnumerable]);

    // Utility

    Enumerable.Utils = {}; // container

    Enumerable.Utils.createLambda = function (expression) {
        return Utils.createLambda(expression);
    };

    Enumerable.Utils.createEnumerable = function (GetEnumerator) {
        return new Enumerable(GetEnumerator);
    };

    Enumerable.Utils.createEnumerator = function (initialize, tryGetNext, dispose) {
        return new IEnumerator(initialize, tryGetNext, dispose);
    };

    Enumerable.Utils.extendTo = function (type) {
        var typeProto = type.prototype;
        var enumerableProto;

        if (type === Array) {
            enumerableProto = ArrayEnumerable.prototype;
            Utils.defineProperty(typeProto, "getSource", function () {
                return this;
            });
        }
        else {
            enumerableProto = Enumerable.prototype;
            Utils.defineProperty(typeProto, "GetEnumerator", function () {
                return Enumerable.from(this).GetEnumerator();
            });
        }

        for (var methodName in enumerableProto) {
            var func = enumerableProto[methodName];

            // already extended
            if (typeProto[methodName] == func) continue;

            // already defined(example Array#reverse/join/forEach...)
            if (typeProto[methodName] != null) {
                methodName = methodName + "ByLinq";
                if (typeProto[methodName] == func) continue; // recheck
            }

            if (func instanceof Function) {
                Utils.defineProperty(typeProto, methodName, func);
            }
        }
    };

    // Generator

    Enumerable.choice = function () // variable argument
    {
        var args = arguments;

        return new Enumerable(function () {
            return new IEnumerator(
                function () {
                    args = (args[0] instanceof Array) ? args[0]
                        : (args[0].GetEnumerator != null) ? args[0].ToArray()
                        : args;
                },
                function () {
                    return this.yieldReturn(args[Math.floor(Math.random() * args.length)]);
                },
                Functions.Blank);
        });
    };

    Enumerable.cycle = function () // variable argument
    {
        var args = arguments;

        return new Enumerable(function () {
            var index = 0;
            return new IEnumerator(
                function () {
                    args = (args[0] instanceof Array) ? args[0]
                        : (args[0].GetEnumerator != null) ? args[0].ToArray()
                        : args;
                },
                function () {
                    if (index >= args.length) index = 0;
                    return this.yieldReturn(args[index++]);
                },
                Functions.Blank);
        });
    };

    // private singleton
    var emptyEnumerable = new Enumerable(function () {
            return new IEnumerator(
                Functions.Blank,
                function () { return false; },
                Functions.Blank);
        });
    Enumerable.empty = function () {
        return emptyEnumerable;
    };

    Enumerable.from = function (obj, T) {
        if (obj == null) {
            return null;
        }
        if (obj instanceof Enumerable) {
            return obj;
        }
        if (typeof obj == Types.Number || typeof obj == Types.Boolean) {
            return Enumerable.repeat(obj, 1);
        }
        if (typeof obj == Types.String) {
            return new Enumerable(function () {
                var index = 0;
                return new IEnumerator(
                    Functions.Blank,
                    function () {
                        return (index < obj.length) ? this.yieldReturn(obj.charCodeAt(index++)) : false;
                    },
                    Functions.Blank);
            });
        }
        var ienum = H5.as(obj, System.Collections.IEnumerable);
        if (ienum) {
            return new Enumerable(function () {
                var enumerator;
                return new IEnumerator(
                    function () { enumerator = H5.getEnumerator(ienum, T); },
                    function () {
                        var ok = enumerator.moveNext();
                        return ok ? this.yieldReturn(enumerator.Current) : false;
                    },
                    function () {
                        var disposable = H5.as(enumerator, System.IDisposable);
                        if (disposable) {
                            disposable.Dispose();
                        }
                    }
                );
            });
        }
        if (typeof obj != Types.Function) {
            // array or array like object
            if (typeof obj.length == Types.Number) {
                return new ArrayEnumerable(obj);
            }

            // JScript's IEnumerable
            if (!(obj instanceof Object) && Utils.isIEnumerable(obj)) {
                return new Enumerable(function () {
                    var isFirst = true;
                    var enumerator;
                    return new IEnumerator(
                        function () { enumerator = new Enumerator(obj); },
                        function () {
                            if (isFirst) isFirst = false;
                            else enumerator.moveNext();

                            return (enumerator.atEnd()) ? false : this.yieldReturn(enumerator.item());
                        },
                        Functions.Blank);
                });
            }

            // WinMD IIterable<T>
            if (typeof Windows === Types.Object && typeof obj.first === Types.Function) {
                return new Enumerable(function () {
                    var isFirst = true;
                    var enumerator;
                    return new IEnumerator(
                        function () { enumerator = obj.first(); },
                        function () {
                            if (isFirst) isFirst = false;
                            else enumerator.moveNext();

                            return (enumerator.hasCurrent) ? this.yieldReturn(enumerator.current) : this.yieldBreak();
                        },
                        Functions.Blank);
                });
            }
        }

        // case function/object : Create keyValuePair[]
        return new Enumerable(function () {
            var array = [];
            var index = 0;

            return new IEnumerator(
                function () {
                    for (var key in obj) {
                        var value = obj[key];
                        if (!(value instanceof Function) && Object.prototype.hasOwnProperty.call(obj, key)) {
                            array.push({ key: key, value: value });
                        }
                    }
                },
                function () {
                    return (index < array.length)
                        ? this.yieldReturn(array[index++])
                        : false;
                },
                Functions.Blank);
        });
    },

    Enumerable.make = function (element) {
        return Enumerable.repeat(element, 1);
    };

    // Overload:function (input, pattern)
    // Overload:function (input, pattern, flags)
    Enumerable.matches = function (input, pattern, flags) {
        if (flags == null) flags = "";
        if (pattern instanceof RegExp) {
            flags += (pattern.ignoreCase) ? "i" : "";
            flags += (pattern.multiline) ? "m" : "";
            pattern = pattern.source;
        }
        if (flags.indexOf("g") === -1) flags += "g";

        return new Enumerable(function () {
            var regex;
            return new IEnumerator(
                function () { regex = new RegExp(pattern, flags); },
                function () {
                    var match = regex.exec(input);
                    return (match) ? this.yieldReturn(match) : false;
                },
                Functions.Blank);
        });
    };

    // Overload:function (start, count)
    // Overload:function (start, count, step)
    Enumerable.range = function (start, count, step) {
        if (step == null) step = 1;

        return new Enumerable(function () {
            var value;
            var index = 0;

            return new IEnumerator(
                function () { value = start - step; },
                function () {
                    return (index++ < count)
                        ? this.yieldReturn(value += step)
                        : this.yieldBreak();
                },
                Functions.Blank);
        });
    };

    // Overload:function (start, count)
    // Overload:function (start, count, step)
    Enumerable.rangeDown = function (start, count, step) {
        if (step == null) step = 1;

        return new Enumerable(function () {
            var value;
            var index = 0;

            return new IEnumerator(
                function () { value = start + step; },
                function () {
                    return (index++ < count)
                        ? this.yieldReturn(value -= step)
                        : this.yieldBreak();
                },
                Functions.Blank);
        });
    };

    // Overload:function (start, to)
    // Overload:function (start, to, step)
    Enumerable.rangeTo = function (start, to, step) {
        if (step == null) step = 1;

        if (start < to) {
            return new Enumerable(function () {
                var value;

                return new IEnumerator(
                function () { value = start - step; },
                function () {
                    var next = value += step;
                    return (next <= to)
                        ? this.yieldReturn(next)
                        : this.yieldBreak();
                },
                Functions.Blank);
            });
        }
        else {
            return new Enumerable(function () {
                var value;

                return new IEnumerator(
                function () { value = start + step; },
                function () {
                    var next = value -= step;
                    return (next >= to)
                        ? this.yieldReturn(next)
                        : this.yieldBreak();
                },
                Functions.Blank);
            });
        }
    };

    // Overload:function (element)
    // Overload:function (element, count)
    Enumerable.repeat = function (element, count) {
        if (count != null) return Enumerable.repeat(element).take(count);

        return new Enumerable(function () {
            return new IEnumerator(
                Functions.Blank,
                function () { return this.yieldReturn(element); },
                Functions.Blank);
        });
    };

    Enumerable.repeatWithFinalize = function (initializer, finalizer) {
        initializer = Utils.createLambda(initializer);
        finalizer = Utils.createLambda(finalizer);

        return new Enumerable(function () {
            var element;
            return new IEnumerator(
                function () { element = initializer(); },
                function () { return this.yieldReturn(element); },
                function () {
                    if (element != null) {
                        finalizer(element);
                        element = null;
                    }
                });
        });
    };

    // Overload:function (func)
    // Overload:function (func, count)
    Enumerable.generate = function (func, count) {
        if (count != null) return Enumerable.generate(func).take(count);
        func = Utils.createLambda(func);

        return new Enumerable(function () {
            return new IEnumerator(
                Functions.Blank,
                function () { return this.yieldReturn(func()); },
                Functions.Blank);
        });
    };

    // Overload:function ()
    // Overload:function (start)
    // Overload:function (start, step)
    Enumerable.toInfinity = function (start, step) {
        if (start == null) start = 0;
        if (step == null) step = 1;

        return new Enumerable(function () {
            var value;
            return new IEnumerator(
                function () { value = start - step; },
                function () { return this.yieldReturn(value += step); },
                Functions.Blank);
        });
    };

    // Overload:function ()
    // Overload:function (start)
    // Overload:function (start, step)
    Enumerable.toNegativeInfinity = function (start, step) {
        if (start == null) start = 0;
        if (step == null) step = 1;

        return new Enumerable(function () {
            var value;
            return new IEnumerator(
                function () { value = start + step; },
                function () { return this.yieldReturn(value -= step); },
                Functions.Blank);
        });
    };

    Enumerable.unfold = function (seed, func) {
        func = Utils.createLambda(func);

        return new Enumerable(function () {
            var isFirst = true;
            var value;
            return new IEnumerator(
                Functions.Blank,
                function () {
                    if (isFirst) {
                        isFirst = false;
                        value = seed;
                        return this.yieldReturn(value);
                    }
                    value = func(value);
                    return this.yieldReturn(value);
                },
                Functions.Blank);
        });
    };

    Enumerable.defer = function (enumerableFactory) {
        return new Enumerable(function () {
            var enumerator;

            return new IEnumerator(
                function () { enumerator = Enumerable.from(enumerableFactory()).GetEnumerator(); },
                function () {
                    return (enumerator.moveNext())
                        ? this.yieldReturn(enumerator.Current)
                        : this.yieldBreak();
                },
                function () {
                    Utils.Dispose(enumerator);
                });
        });
    };

    // Extension Methods

    /* Projection and Filtering Methods */

    // Overload:function (func)
    // Overload:function (func, resultSelector<element>)
    // Overload:function (func, resultSelector<element, nestLevel>)
    Enumerable.prototype.traverseBreadthFirst = function (func, resultSelector) {
        var source = this;
        func = Utils.createLambda(func);
        resultSelector = Utils.createLambda(resultSelector);

        return new Enumerable(function () {
            var enumerator;
            var nestLevel = 0;
            var buffer = [];

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    while (true) {
                        if (enumerator.moveNext()) {
                            buffer.push(enumerator.Current);
                            return this.yieldReturn(resultSelector(enumerator.Current, nestLevel));
                        }

                        var next = Enumerable.from(buffer).selectMany(function (x) { return func(x); });
                        if (!next.any()) {
                            return false;
                        }
                        else {
                            nestLevel++;
                            buffer = [];
                            Utils.Dispose(enumerator);
                            enumerator = next.GetEnumerator();
                        }
                    }
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (func)
    // Overload:function (func, resultSelector<element>)
    // Overload:function (func, resultSelector<element, nestLevel>)
    Enumerable.prototype.traverseDepthFirst = function (func, resultSelector) {
        var source = this;
        func = Utils.createLambda(func);
        resultSelector = Utils.createLambda(resultSelector);

        return new Enumerable(function () {
            var enumeratorStack = [];
            var enumerator;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    while (true) {
                        if (enumerator.moveNext()) {
                            var value = resultSelector(enumerator.Current, enumeratorStack.length);
                            enumeratorStack.push(enumerator);
                            enumerator = Enumerable.from(func(enumerator.Current)).GetEnumerator();
                            return this.yieldReturn(value);
                        }

                        if (enumeratorStack.length <= 0) return false;
                        Utils.Dispose(enumerator);
                        enumerator = enumeratorStack.pop();
                    }
                },
                function () {
                    try {
                        Utils.Dispose(enumerator);
                    }
                    finally {
                        Enumerable.from(enumeratorStack).forEach(function (s) { s.Dispose(); });
                    }
                });
        });
    };

    Enumerable.prototype.flatten = function () {
        var source = this;

        return new Enumerable(function () {
            var enumerator;
            var middleEnumerator = null;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    while (true) {
                        if (middleEnumerator != null) {
                            if (middleEnumerator.moveNext()) {
                                return this.yieldReturn(middleEnumerator.Current);
                            }
                            else {
                                middleEnumerator = null;
                            }
                        }

                        if (enumerator.moveNext()) {
                            if (enumerator.Current instanceof Array) {
                                Utils.Dispose(middleEnumerator);
                                middleEnumerator = Enumerable.from(enumerator.Current)
                                    .selectMany(Functions.Identity)
                                    .flatten()
                                    .GetEnumerator();
                                continue;
                            }
                            else {
                                return this.yieldReturn(enumerator.Current);
                            }
                        }

                        return false;
                    }
                },
                function () {
                    try {
                        Utils.Dispose(enumerator);
                    }
                    finally {
                        Utils.Dispose(middleEnumerator);
                    }
                });
        });
    };

    Enumerable.prototype.pairwise = function (selector) {
        var source = this;
        selector = Utils.createLambda(selector);

        return new Enumerable(function () {
            var enumerator;

            return new IEnumerator(
                function () {
                    enumerator = source.GetEnumerator();
                    enumerator.moveNext();
                },
                function () {
                    var prev = enumerator.Current;
                    return (enumerator.moveNext())
                        ? this.yieldReturn(selector(prev, enumerator.Current))
                        : false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (func)
    // Overload:function (seed,func<value,element>)
    Enumerable.prototype.scan = function (seed, func) {
        var isUseSeed;
        if (func == null) {
            func = Utils.createLambda(seed); // arguments[0]
            isUseSeed = false;
        } else {
            func = Utils.createLambda(func);
            isUseSeed = true;
        }
        var source = this;

        return new Enumerable(function () {
            var enumerator;
            var value;
            var isFirst = true;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    if (isFirst) {
                        isFirst = false;
                        if (!isUseSeed) {
                            if (enumerator.moveNext()) {
                                return this.yieldReturn(value = enumerator.Current);
                            }
                        }
                        else {
                            return this.yieldReturn(value = seed);
                        }
                    }

                    return (enumerator.moveNext())
                        ? this.yieldReturn(value = func(value, enumerator.Current))
                        : false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (selector<element>)
    // Overload:function (selector<element,index>)
    Enumerable.prototype.select = function (selector) {
        selector = Utils.createLambda(selector);

        if (selector.length <= 1) {
            return new WhereSelectEnumerable(this, null, selector);
        }
        else {
            var source = this;

            return new Enumerable(function () {
                var enumerator;
                var index = 0;

                return new IEnumerator(
                    function () { enumerator = source.GetEnumerator(); },
                    function () {
                        return (enumerator.moveNext())
                            ? this.yieldReturn(selector(enumerator.Current, index++))
                            : false;
                    },
                    function () { Utils.Dispose(enumerator); });
            });
        }
    };

    // Overload:function (collectionSelector<element>)
    // Overload:function (collectionSelector<element,index>)
    // Overload:function (collectionSelector<element>,resultSelector)
    // Overload:function (collectionSelector<element,index>,resultSelector)
    Enumerable.prototype.selectMany = function (collectionSelector, resultSelector) {
        var source = this;
        collectionSelector = Utils.createLambda(collectionSelector);
        if (resultSelector == null) resultSelector = function (a, b) { return b; };
        resultSelector = Utils.createLambda(resultSelector);

        return new Enumerable(function () {
            var enumerator;
            var middleEnumerator = undefined;
            var index = 0;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    if (middleEnumerator === undefined) {
                        if (!enumerator.moveNext()) return false;
                    }
                    do {
                        if (middleEnumerator == null) {
                            var middleSeq = collectionSelector(enumerator.Current, index++);
                            middleEnumerator = Enumerable.from(middleSeq).GetEnumerator();
                        }
                        if (middleEnumerator.moveNext()) {
                            return this.yieldReturn(resultSelector(enumerator.Current, middleEnumerator.Current));
                        }
                        Utils.Dispose(middleEnumerator);
                        middleEnumerator = null;
                    } while (enumerator.moveNext());
                    return false;
                },
                function () {
                    try {
                        Utils.Dispose(enumerator);
                    }
                    finally {
                        Utils.Dispose(middleEnumerator);
                    }
                });
        });
    };

    // Overload:function (predicate<element>)
    // Overload:function (predicate<element,index>)
    Enumerable.prototype.where = function (predicate) {
        predicate = Utils.createLambda(predicate);

        if (predicate.length <= 1) {
            return new WhereEnumerable(this, predicate);
        }
        else {
            var source = this;

            return new Enumerable(function () {
                var enumerator;
                var index = 0;

                return new IEnumerator(
                    function () { enumerator = source.GetEnumerator(); },
                    function () {
                        while (enumerator.moveNext()) {
                            if (predicate(enumerator.Current, index++)) {
                                return this.yieldReturn(enumerator.Current);
                            }
                        }
                        return false;
                    },
                    function () { Utils.Dispose(enumerator); });
            });
        }
    };

    // Overload:function (selector<element>)
    // Overload:function (selector<element,index>)
    Enumerable.prototype.choose = function (selector) {
        selector = Utils.createLambda(selector);
        var source = this;

        return new Enumerable(function () {
            var enumerator;
            var index = 0;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    while (enumerator.moveNext()) {
                        var result = selector(enumerator.Current, index++);
                        if (result != null) {
                            return this.yieldReturn(result);
                        }
                    }
                    return this.yieldBreak();
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    Enumerable.prototype.ofType = function (type) {
        var source = this;

        return new Enumerable(function () {
            var enumerator;

            return new IEnumerator(
                function () {
                    enumerator = H5.getEnumerator(source);
                },
                function () {
                    while (enumerator.moveNext()) {
                        var v = H5.as(enumerator.Current, type);
                        if (H5.hasValue(v)) {
                            return this.yieldReturn(v);
                        }
                    }
                    return false;
                },
                function () {
                    Utils.Dispose(enumerator);
                });
        });
    };

    // mutiple arguments, last one is selector, others are enumerable
    Enumerable.prototype.zip = function () {
        var args = arguments;
        var selector = Utils.createLambda(arguments[arguments.length - 1]);

        var source = this;
        // optimized case:argument is 2
        if (arguments.length == 2) {
            var second = arguments[0];

            if (second == null) {
                throw new System.ArgumentNullException();
            }

            return new Enumerable(function () {
                var firstEnumerator;
                var secondEnumerator;
                var index = 0;

                return new IEnumerator(
                function () {
                    firstEnumerator = source.GetEnumerator();
                    secondEnumerator = Enumerable.from(second).GetEnumerator();
                },
                function () {
                    if (firstEnumerator.moveNext() && secondEnumerator.moveNext()) {
                        return this.yieldReturn(selector(firstEnumerator.Current, secondEnumerator.Current, index++));
                    }
                    return false;
                },
                function () {
                    try {
                        Utils.Dispose(firstEnumerator);
                    } finally {
                        Utils.Dispose(secondEnumerator);
                    }
                });
            });
        }
        else {
            return new Enumerable(function () {
                var enumerators;
                var index = 0;

                return new IEnumerator(
                function () {
                    var array = Enumerable.make(source)
                        .concat(Enumerable.from(args).takeExceptLast().select(Enumerable.from))
                        .select(function (x) { return x.GetEnumerator() })
                        .ToArray();
                    enumerators = Enumerable.from(array);
                },
                function () {
                    if (enumerators.all(function (x) { return x.moveNext() })) {
                        var array = enumerators
                            .select(function (x) { return x.Current; })
                            .ToArray();
                        array.push(index++);
                        return this.yieldReturn(selector.apply(null, array));
                    }
                    else {
                        return this.yieldBreak();
                    }
                },
                function () {
                    Enumerable.from(enumerators).forEach(Utils.Dispose);
                });
            });
        }
    };

    // mutiple arguments
    Enumerable.prototype.merge = function () {
        var args = arguments;
        var source = this;

        return new Enumerable(function () {
            var enumerators;
            var index = -1;

            return new IEnumerator(
                function () {
                    enumerators = Enumerable.make(source)
                        .concat(Enumerable.from(args).select(Enumerable.from))
                        .select(function (x) { return x.GetEnumerator() })
                        .ToArray();
                },
                function () {
                    while (enumerators.length > 0) {
                        index = (index >= enumerators.length - 1) ? 0 : index + 1;
                        var enumerator = enumerators[index];

                        if (enumerator.moveNext()) {
                            return this.yieldReturn(enumerator.Current);
                        }
                        else {
                            enumerator.Dispose();
                            enumerators.splice(index--, 1);
                        }
                    }
                    return this.yieldBreak();
                },
                function () {
                    Enumerable.from(enumerators).forEach(Utils.Dispose);
                });
        });
    };

    /* Join Methods */

    // Overload:function (inner, outerKeySelector, innerKeySelector, resultSelector)
    // Overload:function (inner, outerKeySelector, innerKeySelector, resultSelector, compareSelector)
    Enumerable.prototype.join = function (inner, outerKeySelector, innerKeySelector, resultSelector, comparer) {
        outerKeySelector = Utils.createLambda(outerKeySelector);
        innerKeySelector = Utils.createLambda(innerKeySelector);
        resultSelector = Utils.createLambda(resultSelector);

        if (inner == null) {
            throw new System.ArgumentNullException();
        }

        var source = this;

        return new Enumerable(function () {
            var outerEnumerator;
            var lookup;
            var innerElements = null;
            var innerCount = 0;

            return new IEnumerator(
                function () {
                    outerEnumerator = source.GetEnumerator();
                    lookup = Enumerable.from(inner).toLookup(innerKeySelector, Functions.Identity, comparer);
                },
                function () {
                    while (true) {
                        if (innerElements != null) {
                            var innerElement = innerElements[innerCount++];
                            if (innerElement !== undefined) {
                                return this.yieldReturn(resultSelector(outerEnumerator.Current, innerElement));
                            }

                            innerElement = null;
                            innerCount = 0;
                        }

                        if (outerEnumerator.moveNext()) {
                            var key = outerKeySelector(outerEnumerator.Current);
                            innerElements = lookup.get(key).ToArray();
                        } else {
                            return false;
                        }
                    }
                },
                function () { Utils.Dispose(outerEnumerator); });
        });
    };

    // Overload:function (inner, outerKeySelector, innerKeySelector, resultSelector)
    // Overload:function (inner, outerKeySelector, innerKeySelector, resultSelector, compareSelector)
    Enumerable.prototype.groupJoin = function (inner, outerKeySelector, innerKeySelector, resultSelector, comparer) {
        outerKeySelector = Utils.createLambda(outerKeySelector);
        innerKeySelector = Utils.createLambda(innerKeySelector);
        resultSelector = Utils.createLambda(resultSelector);
        var source = this;

        if (inner == null) {
            throw new System.ArgumentNullException();
        }

        return new Enumerable(function () {
            var enumerator = source.GetEnumerator();
            var lookup = null;

            return new IEnumerator(
                function () {
                    enumerator = source.GetEnumerator();
                    lookup = Enumerable.from(inner).toLookup(innerKeySelector, Functions.Identity, comparer);
                },
                function () {
                    if (enumerator.moveNext()) {
                        var innerElement = lookup.get(outerKeySelector(enumerator.Current));
                        return this.yieldReturn(resultSelector(enumerator.Current, innerElement));
                    }
                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    /* Set Methods */

    Enumerable.prototype.all = function (predicate) {
        predicate = Utils.createLambda(predicate);

        var result = true;
        this.forEach(function (x) {
            if (!predicate(x)) {
                result = false;
                return false; // break
            }
        });
        return result;
    };

    // Overload:function ()
    // Overload:function (predicate)
    Enumerable.prototype.any = function (predicate) {
        predicate = Utils.createLambda(predicate);

        var enumerator = this.GetEnumerator();
        try {
            if (arguments.length == 0) return enumerator.moveNext(); // case:function ()

            while (enumerator.moveNext()) // case:function (predicate)
            {
                if (predicate(enumerator.Current)) return true;
            }
            return false;
        }
        finally {
            Utils.Dispose(enumerator);
        }
    };

    Enumerable.prototype.isEmpty = function () {
        return !this.any();
    };

    // multiple arguments
    Enumerable.prototype.concat = function () {
        var source = this;

        if (arguments.length == 1) {
            var second = arguments[0];

            if (second == null) {
                throw new System.ArgumentNullException();
            }

            return new Enumerable(function () {
                var firstEnumerator;
                var secondEnumerator;

                return new IEnumerator(
                function () { firstEnumerator = source.GetEnumerator(); },
                function () {
                    if (secondEnumerator == null) {
                        if (firstEnumerator.moveNext()) return this.yieldReturn(firstEnumerator.Current);
                        secondEnumerator = Enumerable.from(second).GetEnumerator();
                    }
                    if (secondEnumerator.moveNext()) return this.yieldReturn(secondEnumerator.Current);
                    return false;
                },
                function () {
                    try {
                        Utils.Dispose(firstEnumerator);
                    }
                    finally {
                        Utils.Dispose(secondEnumerator);
                    }
                });
            });
        }
        else {
            var args = arguments;

            return new Enumerable(function () {
                var enumerators;

                return new IEnumerator(
                    function () {
                        enumerators = Enumerable.make(source)
                            .concat(Enumerable.from(args).select(Enumerable.from))
                            .select(function (x) { return x.GetEnumerator() })
                            .ToArray();
                    },
                    function () {
                        while (enumerators.length > 0) {
                            var enumerator = enumerators[0];

                            if (enumerator.moveNext()) {
                                return this.yieldReturn(enumerator.Current);
                            }
                            else {
                                enumerator.Dispose();
                                enumerators.splice(0, 1);
                            }
                        }
                        return this.yieldBreak();
                    },
                    function () {
                        Enumerable.from(enumerators).forEach(Utils.Dispose);
                    });
            });
        }
    };

    Enumerable.prototype.insert = function (index, second) {
        var source = this;

        return new Enumerable(function () {
            var firstEnumerator;
            var secondEnumerator;
            var count = 0;
            var isEnumerated = false;

            return new IEnumerator(
                function () {
                    firstEnumerator = source.GetEnumerator();
                    secondEnumerator = Enumerable.from(second).GetEnumerator();
                },
                function () {
                    if (count == index && secondEnumerator.moveNext()) {
                        isEnumerated = true;
                        return this.yieldReturn(secondEnumerator.Current);
                    }
                    if (firstEnumerator.moveNext()) {
                        count++;
                        return this.yieldReturn(firstEnumerator.Current);
                    }
                    if (!isEnumerated && secondEnumerator.moveNext()) {
                        return this.yieldReturn(secondEnumerator.Current);
                    }
                    return false;
                },
                function () {
                    try {
                        Utils.Dispose(firstEnumerator);
                    }
                    finally {
                        Utils.Dispose(secondEnumerator);
                    }
                });
        });
    };

    Enumerable.prototype.alternate = function (alternateValueOrSequence) {
        var source = this;

        return new Enumerable(function () {
            var buffer;
            var enumerator;
            var alternateSequence;
            var alternateEnumerator;

            return new IEnumerator(
                function () {
                    if (alternateValueOrSequence instanceof Array || alternateValueOrSequence.GetEnumerator != null) {
                        alternateSequence = Enumerable.from(Enumerable.from(alternateValueOrSequence).ToArray()); // freeze
                    }
                    else {
                        alternateSequence = Enumerable.make(alternateValueOrSequence);
                    }
                    enumerator = source.GetEnumerator();
                    if (enumerator.moveNext()) buffer = enumerator.Current;
                },
                function () {
                    while (true) {
                        if (alternateEnumerator != null) {
                            if (alternateEnumerator.moveNext()) {
                                return this.yieldReturn(alternateEnumerator.Current);
                            }
                            else {
                                alternateEnumerator = null;
                            }
                        }

                        if (buffer == null && enumerator.moveNext()) {
                            buffer = enumerator.Current; // hasNext
                            alternateEnumerator = alternateSequence.GetEnumerator();
                            continue; // GOTO
                        }
                        else if (buffer != null) {
                            var retVal = buffer;
                            buffer = null;
                            return this.yieldReturn(retVal);
                        }

                        return this.yieldBreak();
                    }
                },
                function () {
                    try {
                        Utils.Dispose(enumerator);
                    }
                    finally {
                        Utils.Dispose(alternateEnumerator);
                    }
                });
        });
    };

    // Overload:function (value)
    // Overload:function (value, compareSelector)
    Enumerable.prototype.contains = function (value, comparer) {
        comparer = comparer || System.Collections.Generic.EqualityComparer$1.$default;
        var enumerator = this.GetEnumerator();
        try {
            while (enumerator.moveNext()) {
                if (comparer.equals2(enumerator.Current, value)) return true;
            }
            return false;
        }
        finally {
            Utils.Dispose(enumerator);
        }
    };

    Enumerable.prototype.defaultIfEmpty = function (defaultValue) {
        var source = this;
        if (defaultValue === undefined) defaultValue = null;

        return new Enumerable(function () {
            var enumerator;
            var isFirst = true;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    if (enumerator.moveNext()) {
                        isFirst = false;
                        return this.yieldReturn(enumerator.Current);
                    }
                    else if (isFirst) {
                        isFirst = false;
                        return this.yieldReturn(defaultValue);
                    }
                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function ()
    // Overload:function (compareSelector)
    Enumerable.prototype.distinct = function (comparer) {
        return this.except(Enumerable.empty(), comparer);
    };

    Enumerable.prototype.distinctUntilChanged = function (compareSelector) {
        compareSelector = Utils.createLambda(compareSelector);
        var source = this;

        return new Enumerable(function () {
            var enumerator;
            var compareKey;
            var initial;

            return new IEnumerator(
                function () {
                    enumerator = source.GetEnumerator();
                },
                function () {
                    while (enumerator.moveNext()) {
                        var key = compareSelector(enumerator.Current);

                        if (initial) {
                            initial = false;
                            compareKey = key;
                            return this.yieldReturn(enumerator.Current);
                        }

                        if (compareKey === key) {
                            continue;
                        }

                        compareKey = key;
                        return this.yieldReturn(enumerator.Current);
                    }
                    return this.yieldBreak();
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (second)
    // Overload:function (second, compareSelector)
    Enumerable.prototype.except = function (second, comparer) {
        var source = this;

        if (second == null) {
            throw new System.ArgumentNullException();
        }

        return new Enumerable(function () {
            var enumerator,
                keys,
                hasNull = false;

            return new IEnumerator(
                function () {
                    enumerator = source.GetEnumerator();
                    keys = new (System.Collections.Generic.Dictionary$2(System.Object, System.Object)).$ctor3(comparer);

                    Enumerable.from(second).forEach(function (key) {
                        if (key == null) {
                            hasNull = true;
                        }
                        else if (!keys.containsKey(key)) {
                            keys.add(key);
                        }
                    });
                },
                function () {
                    while (enumerator.moveNext()) {
                        var current = enumerator.Current;
                        if (current == null) {
                            if (!hasNull) {
                                hasNull = true;
                                return this.yieldReturn(current);
                            }
                        }
                        else if (!keys.containsKey(current)) {
                            keys.add(current);
                            return this.yieldReturn(current);
                        }
                    }
                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (second)
    // Overload:function (second, compareSelector)
    Enumerable.prototype.intersect = function (second, comparer) {
        var source = this;

        if (second == null) {
            throw new System.ArgumentNullException();
        }

        return new Enumerable(function () {
            var enumerator;
            var keys;
            var outs;
            var hasNull = false;
            var hasOutsNull = false;

            return new IEnumerator(
                function () {
                    enumerator = source.GetEnumerator();

                    keys = new (System.Collections.Generic.Dictionary$2(System.Object, System.Object)).$ctor3(comparer);
                    Enumerable.from(second).forEach(function (key) {
                        if (key == null) {
                            hasNull = true;
                        }
                        else if (!keys.containsKey(key)) {
                            keys.add(key);
                        }
                    });
                    outs = new (System.Collections.Generic.Dictionary$2(System.Object, System.Object)).$ctor3(comparer);
                },
                function () {
                    while (enumerator.moveNext()) {
                        var current = enumerator.Current;
                        if (current == null) {
                            if (!hasOutsNull && hasNull) {
                                hasOutsNull = true;
                                return this.yieldReturn(current);
                            }
                        } else if (!outs.containsKey(current) && keys.containsKey(current)) {
                            outs.add(current);
                            return this.yieldReturn(current);
                        }
                    }
                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (second)
    // Overload:function (second, compareSelector)
    Enumerable.prototype.sequenceEqual = function (second, comparer) {
        comparer = comparer || System.Collections.Generic.EqualityComparer$1.$default;

        if (second == null) {
            throw new System.ArgumentNullException();
        }

        var firstEnumerator = this.GetEnumerator();
        try {
            var secondEnumerator = Enumerable.from(second).GetEnumerator();
            try {
                while (firstEnumerator.moveNext()) {
                    if (!secondEnumerator.moveNext()
                    || !comparer.equals2(firstEnumerator.Current, secondEnumerator.Current)) {
                        return false;
                    }
                }

                if (secondEnumerator.moveNext()) return false;
                return true;
            }
            finally {
                Utils.Dispose(secondEnumerator);
            }
        }
        finally {
            Utils.Dispose(firstEnumerator);
        }
    };

    Enumerable.prototype.union = function (second, comparer) {
        var source = this;

        if (second == null) {
            throw new System.ArgumentNullException();
        }

        return new Enumerable(function () {
            var firstEnumerator;
            var secondEnumerator;
            var keys;
            var hasNull = false;

            return new IEnumerator(
                function () {
                    firstEnumerator = source.GetEnumerator();
                    keys = new (System.Collections.Generic.Dictionary$2(System.Object, System.Object)).$ctor3(comparer);
                },
                function () {
                    var current;
                    if (secondEnumerator === undefined) {
                        while (firstEnumerator.moveNext()) {
                            current = firstEnumerator.Current;
                            if (current == null) {
                                if (!hasNull) {
                                    hasNull = true;
                                    return this.yieldReturn(current);
                                }
                            }
                            else if (!keys.containsKey(current)) {
                                keys.add(current);
                                return this.yieldReturn(current);
                            }
                        }
                        secondEnumerator = Enumerable.from(second).GetEnumerator();
                    }
                    while (secondEnumerator.moveNext()) {
                        current = secondEnumerator.Current;
                        if (current == null) {
                            if (!hasNull) {
                                hasNull = true;
                                return this.yieldReturn(current);
                            }
                        }
                        else if (!keys.containsKey(current)) {
                            keys.add(current);
                            return this.yieldReturn(current);
                        }
                    }
                    return false;
                },
                function () {
                    try {
                        Utils.Dispose(firstEnumerator);
                    }
                    finally {
                        Utils.Dispose(secondEnumerator);
                    }
                });
        });
    };

    /* Ordering Methods */

    Enumerable.prototype.orderBy = function (keySelector, comparer) {
        return new OrderedEnumerable(this, keySelector, comparer, false);
    };

    Enumerable.prototype.orderByDescending = function (keySelector, comparer) {
        return new OrderedEnumerable(this, keySelector, comparer, true);
    };

    Enumerable.prototype.reverse = function () {
        var source = this;

        return new Enumerable(function () {
            var buffer;
            var index;

            return new IEnumerator(
                function () {
                    buffer = source.ToArray();
                    index = buffer.length;
                },
                function () {
                    return (index > 0)
                        ? this.yieldReturn(buffer[--index])
                        : false;
                },
                Functions.Blank);
        });
    };

    Enumerable.prototype.shuffle = function () {
        var source = this;

        return new Enumerable(function () {
            var buffer;

            return new IEnumerator(
                function () { buffer = source.ToArray(); },
                function () {
                    if (buffer.length > 0) {
                        var i = Math.floor(Math.random() * buffer.length);
                        return this.yieldReturn(buffer.splice(i, 1)[0]);
                    }
                    return false;
                },
                Functions.Blank);
        });
    };

    Enumerable.prototype.weightedSample = function (weightSelector) {
        weightSelector = Utils.createLambda(weightSelector);
        var source = this;

        return new Enumerable(function () {
            var sortedByBound;
            var totalWeight = 0;

            return new IEnumerator(
                function () {
                    sortedByBound = source
                        .choose(function (x) {
                            var weight = weightSelector(x);
                            if (weight <= 0) return null; // ignore 0

                            totalWeight += weight;
                            return { value: x, bound: totalWeight };
                        })
                        .ToArray();
                },
                function () {
                    if (sortedByBound.length > 0) {
                        var draw = Math.floor(Math.random() * totalWeight) + 1;

                        var lower = -1;
                        var upper = sortedByBound.length;
                        while (upper - lower > 1) {
                            var index = Math.floor((lower + upper) / 2);
                            if (sortedByBound[index].bound >= draw) {
                                upper = index;
                            }
                            else {
                                lower = index;
                            }
                        }

                        return this.yieldReturn(sortedByBound[upper].value);
                    }

                    return this.yieldBreak();
                },
                Functions.Blank);
        });
    };

    /* Grouping Methods */

    // Overload:function (keySelector)
    // Overload:function (keySelector,elementSelector)
    // Overload:function (keySelector,elementSelector,resultSelector)
    // Overload:function (keySelector,elementSelector,resultSelector,compareSelector)
    Enumerable.prototype.groupBy = function (keySelector, elementSelector, resultSelector, comparer) {
        var source = this;
        keySelector = Utils.createLambda(keySelector);
        elementSelector = Utils.createLambda(elementSelector);
        if (resultSelector != null) resultSelector = Utils.createLambda(resultSelector);

        return new Enumerable(function () {
            var enumerator;

            return new IEnumerator(
                function () {
                    enumerator = source.toLookup(keySelector, elementSelector, comparer)
                        .toEnumerable()
                        .GetEnumerator();
                },
                function () {
                    while (enumerator.moveNext()) {
                        return (resultSelector == null)
                            ? this.yieldReturn(enumerator.Current)
                            : this.yieldReturn(resultSelector(enumerator.Current.key(), enumerator.Current));
                    }
                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (keySelector)
    // Overload:function (keySelector,elementSelector)
    // Overload:function (keySelector,elementSelector,resultSelector)
    // Overload:function (keySelector,elementSelector,resultSelector,compareSelector)
    Enumerable.prototype.partitionBy = function (keySelector, elementSelector, resultSelector, comparer) {
        var source = this;
        keySelector = Utils.createLambda(keySelector);
        elementSelector = Utils.createLambda(elementSelector);
        comparer = comparer || System.Collections.Generic.EqualityComparer$1.$default;
        var hasResultSelector;
        if (resultSelector == null) {
            hasResultSelector = false;
            resultSelector = function (key, group) { return new Grouping(key, group); };
        }
        else {
            hasResultSelector = true;
            resultSelector = Utils.createLambda(resultSelector);
        }

        return new Enumerable(function () {
            var enumerator;
            var key;
            var group = [];

            return new IEnumerator(
                function () {
                    enumerator = source.GetEnumerator();
                    if (enumerator.moveNext()) {
                        key = keySelector(enumerator.Current);
                        group.push(elementSelector(enumerator.Current));
                    }
                },
                function () {
                    var hasNext;
                    while ((hasNext = enumerator.moveNext()) == true) {
                        if (comparer.equals2(key, keySelector(enumerator.Current))) {
                            group.push(elementSelector(enumerator.Current));
                        }
                        else break;
                    }

                    if (group.length > 0) {
                        var result = (hasResultSelector)
                            ? resultSelector(key, Enumerable.from(group))
                            : resultSelector(key, group);
                        if (hasNext) {
                            key = keySelector(enumerator.Current);
                            group = [elementSelector(enumerator.Current)];
                        }
                        else group = [];

                        return this.yieldReturn(result);
                    }

                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    Enumerable.prototype.buffer = function (count) {
        var source = this;

        return new Enumerable(function () {
            var enumerator;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    var array = [];
                    var index = 0;
                    while (enumerator.moveNext()) {
                        array.push(enumerator.Current);
                        if (++index >= count) return this.yieldReturn(array);
                    }
                    if (array.length > 0) return this.yieldReturn(array);
                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    /* Aggregate Methods */

    // Overload:function (func)
    // Overload:function (seed,func)
    // Overload:function (seed,func,resultSelector)
    Enumerable.prototype.aggregate = function (seed, func, resultSelector) {
        resultSelector = Utils.createLambda(resultSelector);
        return resultSelector(this.scan(seed, func, resultSelector).last());
    };

    // Overload:function ()
    // Overload:function (selector)
    Enumerable.prototype.average = function (selector, def) {
        if (selector && !def && !H5.isFunction(selector)) {
            def = selector;
            selector = null;
        }

        selector = Utils.createLambda(selector);

        var sum = def || 0;
        var count = 0;
        this.forEach(function (x) {
            x = selector(x);

            if (x instanceof System.Decimal || System.Int64.is64Bit(x)) {
                sum = x.add(sum);
            }
            else if (sum instanceof System.Decimal || System.Int64.is64Bit(sum)) {
                sum = sum.add(x);
            } else {
                sum += x;
            }

            ++count;
        });

        if (count === 0) {
            throw new System.InvalidOperationException.$ctor1("Sequence contains no elements");
        }

        return (sum instanceof System.Decimal || System.Int64.is64Bit(sum)) ? sum.div(count) : (sum / count);
    };

    Enumerable.prototype.nullableAverage = function (selector, def) {
        if (this.any(H5.isNull)) {
            return null;
        }

        return this.average(selector, def);
    };

    // Overload:function ()
    // Overload:function (predicate)
    Enumerable.prototype.count = function (predicate) {
        predicate = (predicate == null) ? Functions.True : Utils.createLambda(predicate);

        var count = 0;
        this.forEach(function (x, i) {
            if (predicate(x, i))++count;
        });
        return count;
    };

    // Overload:function ()
    // Overload:function (selector)
    Enumerable.prototype.max = function (selector) {
        if (selector == null) selector = Functions.Identity;
        return this.select(selector).aggregate(function (a, b) {
            return (H5.compare(a, b, true) === 1) ? a : b;
        });
    };

    Enumerable.prototype.nullableMax = function (selector) {
        if (this.any(H5.isNull)) {
            return null;
        }

        return this.max(selector);
    };

    // Overload:function ()
    // Overload:function (selector)
    Enumerable.prototype.min = function (selector) {
        if (selector == null) selector = Functions.Identity;
        return this.select(selector).aggregate(function (a, b) {
            return (H5.compare(a, b, true) === -1) ? a : b;
        });
    };

    Enumerable.prototype.nullableMin = function (selector) {
        if (this.any(H5.isNull)) {
            return null;
        }

        return this.min(selector);
    };

    Enumerable.prototype.maxBy = function (keySelector) {
        keySelector = Utils.createLambda(keySelector);
        return this.aggregate(function (a, b) {
            return (H5.compare(keySelector(a), keySelector(b), true) === 1) ? a : b;
        });
    };

    Enumerable.prototype.minBy = function (keySelector) {
        keySelector = Utils.createLambda(keySelector);
        return this.aggregate(function (a, b) {
            return (H5.compare(keySelector(a), keySelector(b), true) === -1) ? a : b;
        });
    };

    // Overload:function ()
    // Overload:function (selector)
    Enumerable.prototype.sum = function (selector, def) {
        if (selector && !def && !H5.isFunction(selector)) {
            def = selector;
            selector = null;
        }

        if (selector == null) selector = Functions.Identity;
        var s = this.select(selector).aggregate(0, function (a, b) {
             if (a instanceof System.Decimal || System.Int64.is64Bit(a)) {
                 return a.add(b);
             }
             if (b instanceof System.Decimal || System.Int64.is64Bit(b)) {
                 return b.add(a);
             }
             return a + b;
        });

        if (s === 0 && def) {
            return def;
        }

        return s;
    };

    Enumerable.prototype.nullableSum = function (selector, def) {
        if (this.any(H5.isNull)) {
            return null;
        }

        return this.sum(selector, def);
    };

    /* Paging Methods */

    Enumerable.prototype.elementAt = function (index) {
        var value;
        var found = false;
        this.forEach(function (x, i) {
            if (i == index) {
                value = x;
                found = true;
                return false;
            }
        });

        if (!found) throw new Error("index is less than 0 or greater than or equal to the number of elements in source.");
        return value;
    };

    Enumerable.prototype.elementAtOrDefault = function (index, defaultValue) {
        if (defaultValue === undefined) defaultValue = null;
        var value;
        var found = false;
        this.forEach(function (x, i) {
            if (i == index) {
                value = x;
                found = true;
                return false;
            }
        });

        return (!found) ? defaultValue : value;
    };

    // Overload:function ()
    // Overload:function (predicate)
    Enumerable.prototype.first = function (predicate) {
        if (predicate != null) return this.where(predicate).first();

        var value;
        var found = false;
        this.forEach(function (x) {
            value = x;
            found = true;
            return false;
        });

        if (!found) throw new Error("first:No element satisfies the condition.");
        return value;
    };

    Enumerable.prototype.firstOrDefault = function (predicate, defaultValue) {
        if (defaultValue === undefined) defaultValue = null;
        if (predicate != null) return this.where(predicate).firstOrDefault(null, defaultValue);

        var value;
        var found = false;
        this.forEach(function (x) {
            value = x;
            found = true;
            return false;
        });
        return (!found) ? defaultValue : value;
    };

    // Overload:function ()
    // Overload:function (predicate)
    Enumerable.prototype.last = function (predicate) {
        if (predicate != null) return this.where(predicate).last();

        var value;
        var found = false;
        this.forEach(function (x) {
            found = true;
            value = x;
        });

        if (!found) throw new Error("last:No element satisfies the condition.");
        return value;
    };

    // Overload:function (defaultValue)
    // Overload:function (defaultValue,predicate)
    Enumerable.prototype.lastOrDefault = function (predicate, defaultValue) {
        if (defaultValue === undefined) defaultValue = null;
        if (predicate != null) return this.where(predicate).lastOrDefault(null, defaultValue);

        var value;
        var found = false;
        this.forEach(function (x) {
            found = true;
            value = x;
        });
        return (!found) ? defaultValue : value;
    };

    // Overload:function ()
    // Overload:function (predicate)
    Enumerable.prototype.single = function (predicate) {
        if (predicate != null) return this.where(predicate).single();

        var value;
        var found = false;
        this.forEach(function (x) {
            if (!found) {
                found = true;
                value = x;
            } else throw new Error("single:sequence contains more than one element.");
        });

        if (!found) throw new Error("single:No element satisfies the condition.");
        return value;
    };

    // Overload:function (defaultValue)
    // Overload:function (defaultValue,predicate)
    Enumerable.prototype.singleOrDefault = function (predicate, defaultValue) {
        if (defaultValue === undefined) defaultValue = null;
        if (predicate != null) return this.where(predicate).singleOrDefault(null, defaultValue);

        var value;
        var found = false;
        this.forEach(function (x) {
            if (!found) {
                found = true;
                value = x;
            } else throw new Error("single:sequence contains more than one element.");
        });

        return (!found) ? defaultValue : value;
    };

    Enumerable.prototype.skip = function (count) {
        var source = this;

        return new Enumerable(function () {
            var enumerator;
            var index = 0;

            return new IEnumerator(
                function () {
                    enumerator = source.GetEnumerator();
                    while (index++ < count && enumerator.moveNext()) {
                    }
                    ;
                },
                function () {
                    return (enumerator.moveNext())
                        ? this.yieldReturn(enumerator.Current)
                        : false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (predicate<element>)
    // Overload:function (predicate<element,index>)
    Enumerable.prototype.skipWhile = function (predicate) {
        predicate = Utils.createLambda(predicate);
        var source = this;

        return new Enumerable(function () {
            var enumerator;
            var index = 0;
            var isSkipEnd = false;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    while (!isSkipEnd) {
                        if (enumerator.moveNext()) {
                            if (!predicate(enumerator.Current, index++)) {
                                isSkipEnd = true;
                                return this.yieldReturn(enumerator.Current);
                            }
                            continue;
                        } else return false;
                    }

                    return (enumerator.moveNext())
                        ? this.yieldReturn(enumerator.Current)
                        : false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    Enumerable.prototype.take = function (count) {
        var source = this;

        return new Enumerable(function () {
            var enumerator;
            var index = 0;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    return (index++ < count && enumerator.moveNext())
                        ? this.yieldReturn(enumerator.Current)
                        : false;
                },
                function () { Utils.Dispose(enumerator); }
            );
        });
    };

    // Overload:function (predicate<element>)
    // Overload:function (predicate<element,index>)
    Enumerable.prototype.takeWhile = function (predicate) {
        predicate = Utils.createLambda(predicate);
        var source = this;

        return new Enumerable(function () {
            var enumerator;
            var index = 0;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    return (enumerator.moveNext() && predicate(enumerator.Current, index++))
                        ? this.yieldReturn(enumerator.Current)
                        : false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function ()
    // Overload:function (count)
    Enumerable.prototype.takeExceptLast = function (count) {
        if (count == null) count = 1;
        var source = this;

        return new Enumerable(function () {
            if (count <= 0) return source.GetEnumerator(); // do nothing

            var enumerator;
            var q = [];

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    while (enumerator.moveNext()) {
                        if (q.length == count) {
                            q.push(enumerator.Current);
                            return this.yieldReturn(q.shift());
                        }
                        q.push(enumerator.Current);
                    }
                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    Enumerable.prototype.takeFromLast = function (count) {
        if (count <= 0 || count == null) return Enumerable.empty();
        var source = this;

        return new Enumerable(function () {
            var sourceEnumerator;
            var enumerator;
            var q = [];

            return new IEnumerator(
                function () { sourceEnumerator = source.GetEnumerator(); },
                function () {
                    if (enumerator == null) {
                        while (sourceEnumerator.moveNext()) {
                            if (q.length == count) q.shift();
                            q.push(sourceEnumerator.Current);
                        }
                        enumerator = Enumerable.from(q).GetEnumerator();
                    }
                    return (enumerator.moveNext())
                        ? this.yieldReturn(enumerator.Current)
                        : false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (item)
    // Overload:function (predicate)
    Enumerable.prototype.indexOf = function (item, comparer) {
        var found = null;

        // item as predicate
        if (typeof (item) === Types.Function) {
            this.forEach(function (x, i) {
                if (item(x, i)) {
                    found = i;
                    return false;
                }
            });
        }
        else {
            comparer = comparer || System.Collections.Generic.EqualityComparer$1.$default;
            this.forEach(function (x, i) {
                if (comparer.equals2(x, item)) {
                    found = i;
                    return false;
                }
            });
        }

        return (found !== null) ? found : -1;
    };

    // Overload:function (item)
    // Overload:function (predicate)
    Enumerable.prototype.lastIndexOf = function (item, comparer) {
        var result = -1;

        // item as predicate
        if (typeof (item) === Types.Function) {
            this.forEach(function (x, i) {
                if (item(x, i)) result = i;
            });
        }
        else {
            comparer = comparer || System.Collections.Generic.EqualityComparer$1.$default;
            this.forEach(function (x, i) {
                if (comparer.equals2(x, item)) result = i;
            });
        }

        return result;
    };

    /* Convert Methods */

    Enumerable.prototype.asEnumerable = function () {
        return Enumerable.from(this);
    };

    Enumerable.prototype.ToArray = function (T) {
        var array = System.Array.init([], T || System.Object);
        this.forEach(function (x) { array.push(x); });
        return array;
    };

    Enumerable.prototype.toList = function (T) {
        var array = [];
        this.forEach(function (x) { array.push(x); });
        return new (System.Collections.Generic.List$1(T || System.Object).$ctor1)(array);
    };

    // Overload:function (keySelector)
    // Overload:function (keySelector, elementSelector)
    // Overload:function (keySelector, elementSelector, compareSelector)
    Enumerable.prototype.toLookup = function (keySelector, elementSelector, comparer) {
        keySelector = Utils.createLambda(keySelector);
        elementSelector = Utils.createLambda(elementSelector);

        var dict = new (System.Collections.Generic.Dictionary$2(System.Object, System.Object)).$ctor3(comparer);
        var order = [];
        var nullKey;
        this.forEach(function (x) {
            var key = keySelector(x);
            var element = elementSelector(x);

            var array = { v: null };

            if (key == null) {
                if (!nullKey) {
                    nullKey = [];
                    order.push(key);
                }
                nullKey.push(element);
            }
            else if (dict.tryGetValue(key, array)) {
                array.v.push(element);
            }
            else {
                order.push(key);
                dict.add(key, [element]);
            }
        });
        return new Lookup(dict, order, nullKey);
    };

    Enumerable.prototype.toObject = function (keySelector, elementSelector) {
        keySelector = Utils.createLambda(keySelector);
        elementSelector = Utils.createLambda(elementSelector);

        var obj = {};
        this.forEach(function (x) {
            obj[keySelector(x)] = elementSelector(x);
        });
        return obj;
    };

    // Overload:function (keySelector, elementSelector)
    // Overload:function (keySelector, elementSelector, compareSelector)
    Enumerable.prototype.toDictionary = function (keySelector, elementSelector, keyType, valueType, comparer) {
        keySelector = Utils.createLambda(keySelector);
        elementSelector = Utils.createLambda(elementSelector);

        var dict = new (System.Collections.Generic.Dictionary$2(keyType, valueType)).$ctor3(comparer);
        this.forEach(function (x) {
            dict.add(keySelector(x), elementSelector(x));
        });
        return dict;
    };

    // Overload:function ()
    // Overload:function (replacer)
    // Overload:function (replacer, space)
    Enumerable.prototype.toJSONString = function (replacer, space) {
        if (typeof JSON === Types.Undefined || JSON.stringify == null) {
            throw new Error("toJSONString can't find JSON.stringify. This works native JSON support Browser or include json2.js");
        }
        return JSON.stringify(this.ToArray(), replacer, space);
    };

    // Overload:function ()
    // Overload:function (separator)
    // Overload:function (separator,selector)
    Enumerable.prototype.toJoinedString = function (separator, selector) {
        if (separator == null) separator = "";
        if (selector == null) selector = Functions.Identity;

        return this.select(selector).ToArray().join(separator);
    };

    /* Action Methods */

    // Overload:function (action<element>)
    // Overload:function (action<element,index>)
    Enumerable.prototype.doAction = function (action) {
        var source = this;
        action = Utils.createLambda(action);

        return new Enumerable(function () {
            var enumerator;
            var index = 0;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    if (enumerator.moveNext()) {
                        action(enumerator.Current, index++);
                        return this.yieldReturn(enumerator.Current);
                    }
                    return false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    // Overload:function (action<element>)
    // Overload:function (action<element,index>)
    // Overload:function (func<element,bool>)
    // Overload:function (func<element,index,bool>)
    Enumerable.prototype.forEach = function (action) {
        action = Utils.createLambda(action);

        var index = 0;
        var enumerator = this.GetEnumerator();
        try {
            while (enumerator.moveNext()) {
                if (action(enumerator.Current, index++) === false) break;
            }
        } finally {
            Utils.Dispose(enumerator);
        }
    };

    // Overload:function ()
    // Overload:function (separator)
    // Overload:function (separator,selector)
    Enumerable.prototype.write = function (separator, selector) {
        if (separator == null) separator = "";
        selector = Utils.createLambda(selector);

        var isFirst = true;
        this.forEach(function (item) {
            if (isFirst) isFirst = false;
            else document.write(separator);
            document.write(selector(item));
        });
    };

    // Overload:function ()
    // Overload:function (selector)
    Enumerable.prototype.writeLine = function (selector) {
        selector = Utils.createLambda(selector);

        this.forEach(function (item) {
            document.writeln(selector(item) + "<br />");
        });
    };

    Enumerable.prototype.force = function () {
        var enumerator = this.GetEnumerator();

        try {
            while (enumerator.moveNext()) {
            }
        }
        finally {
            Utils.Dispose(enumerator);
        }
    };

    /* Functional Methods */

    Enumerable.prototype.letBind = function (func) {
        func = Utils.createLambda(func);
        var source = this;

        return new Enumerable(function () {
            var enumerator;

            return new IEnumerator(
                function () {
                    enumerator = Enumerable.from(func(source)).GetEnumerator();
                },
                function () {
                    return (enumerator.moveNext())
                        ? this.yieldReturn(enumerator.Current)
                        : false;
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    Enumerable.prototype.share = function () {
        var source = this;
        var sharedEnumerator;
        var disposed = false;

        return new DisposableEnumerable(function () {
            return new IEnumerator(
                function () {
                    if (sharedEnumerator == null) {
                        sharedEnumerator = source.GetEnumerator();
                    }
                },
                function () {
                    if (disposed) throw new Error("enumerator is disposed");

                    return (sharedEnumerator.moveNext())
                        ? this.yieldReturn(sharedEnumerator.Current)
                        : false;
                },
                Functions.Blank
            );
        }, function () {
            disposed = true;
            Utils.Dispose(sharedEnumerator);
        });
    };

    Enumerable.prototype.memoize = function () {
        var source = this;
        var cache;
        var enumerator;
        var disposed = false;

        return new DisposableEnumerable(function () {
            var index = -1;

            return new IEnumerator(
                function () {
                    if (enumerator == null) {
                        enumerator = source.GetEnumerator();
                        cache = [];
                    }
                },
                function () {
                    if (disposed) throw new Error("enumerator is disposed");

                    index++;
                    if (cache.length <= index) {
                        return (enumerator.moveNext())
                            ? this.yieldReturn(cache[index] = enumerator.Current)
                            : false;
                    }

                    return this.yieldReturn(cache[index]);
                },
                Functions.Blank
            );
        }, function () {
            disposed = true;
            Utils.Dispose(enumerator);
            cache = null;
        });
    };

    /* Error Handling Methods */

    Enumerable.prototype.catchError = function (handler) {
        handler = Utils.createLambda(handler);
        var source = this;

        return new Enumerable(function () {
            var enumerator;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    try {
                        return (enumerator.moveNext())
                            ? this.yieldReturn(enumerator.Current)
                            : false;
                    } catch (e) {
                        handler(e);
                        return false;
                    }
                },
                function () { Utils.Dispose(enumerator); });
        });
    };

    Enumerable.prototype.finallyAction = function (finallyAction) {
        finallyAction = Utils.createLambda(finallyAction);
        var source = this;

        return new Enumerable(function () {
            var enumerator;

            return new IEnumerator(
                function () { enumerator = source.GetEnumerator(); },
                function () {
                    return (enumerator.moveNext())
                        ? this.yieldReturn(enumerator.Current)
                        : false;
                },
                function () {
                    try {
                        Utils.Dispose(enumerator);
                    } finally {
                        finallyAction();
                    }
                });
        });
    };

    /* For Debug Methods */

    // Overload:function ()
    // Overload:function (selector)
    Enumerable.prototype.log = function (selector) {
        selector = Utils.createLambda(selector);

        return this.doAction(function (item) {
            if (typeof console !== Types.Undefined) {
                console.log(selector(item));
            }
        });
    };

    // Overload:function ()
    // Overload:function (message)
    // Overload:function (message,selector)
    Enumerable.prototype.trace = function (message, selector) {
        if (message == null) message = "Trace";
        selector = Utils.createLambda(selector);

        return this.doAction(function (item) {
            if (typeof console !== Types.Undefined) {
                console.log(message, selector(item));
            }
        });
    };

    // private
    var defaultComparer = {
        compare: function (x, y) {
            if (!H5.hasValue(x)) {
                return !H5.hasValue(y) ? 0 : -1;
            } else if (!H5.hasValue(y)) {
                return 1;
            }

            if (typeof x == "string" && typeof y == "string") {
                var result = System.String.compare(x, y, true);

                if (result !== 0) {
                    return result;
                }
            }

            return H5.compare(x, y);
        }
    };
    var OrderedEnumerable = function (source, keySelector, comparer, descending, parent) {
        this.source = source;
        this.keySelector = Utils.createLambda(keySelector);
        this.comparer = comparer || defaultComparer;
        this.descending = descending;
        this.parent = parent;
    };
    OrderedEnumerable.prototype = new Enumerable();
    OrderedEnumerable.prototype.constructor = OrderedEnumerable;
    H5.definei("System.Linq.IOrderedEnumerable$1");
    OrderedEnumerable.$$inherits = [];
    H5.Class.addExtend(OrderedEnumerable, [System.Collections.IEnumerable, System.Linq.IOrderedEnumerable$1]);

    OrderedEnumerable.prototype.createOrderedEnumerable = function (keySelector, comparer, descending) {
        return new OrderedEnumerable(this.source, keySelector, comparer, descending, this);
    };

    OrderedEnumerable.prototype.thenBy = function (keySelector, comparer) {
        return this.createOrderedEnumerable(keySelector, comparer, false);
    };

    OrderedEnumerable.prototype.thenByDescending = function (keySelector, comparer) {
        return this.createOrderedEnumerable(keySelector, comparer, true);
    };

    OrderedEnumerable.prototype.GetEnumerator = function () {
        var self = this;
        var buffer;
        var indexes;
        var index = 0;

        return new IEnumerator(
            function () {
                buffer = [];
                indexes = [];
                self.source.forEach(function (item, index) {
                    buffer.push(item);
                    indexes.push(index);
                });
                var sortContext = SortContext.create(self, null);
                sortContext.GenerateKeys(buffer);

                indexes.sort(function (a, b) { return sortContext.compare(a, b); });
            },
            function () {
                return (index < indexes.length)
                    ? this.yieldReturn(buffer[indexes[index++]])
                    : false;
            },
            Functions.Blank
        );
    };

    var SortContext = function (keySelector, comparer, descending, child) {
        this.keySelector = keySelector;
        this.comparer = comparer;
        this.descending = descending;
        this.child = child;
        this.keys = null;
    };

    SortContext.create = function (orderedEnumerable, currentContext) {
        var context = new SortContext(orderedEnumerable.keySelector, orderedEnumerable.comparer, orderedEnumerable.descending, currentContext);
        if (orderedEnumerable.parent != null) return SortContext.create(orderedEnumerable.parent, context);
        return context;
    };

    SortContext.prototype.GenerateKeys = function (source) {
        var len = source.length;
        var keySelector = this.keySelector;
        var keys = new Array(len);
        for (var i = 0; i < len; i++) keys[i] = keySelector(source[i]);
        this.keys = keys;

        if (this.child != null) this.child.GenerateKeys(source);
    };

    SortContext.prototype.compare = function (index1, index2) {
        var comparison = this.comparer.compare(this.keys[index1], this.keys[index2]);

        if (comparison == 0) {
            if (this.child != null) return this.child.compare(index1, index2);
            return Utils.compare(index1, index2);
        }

        return (this.descending) ? -comparison : comparison;
    };

    var DisposableEnumerable = function (GetEnumerator, dispose) {
        this.Dispose = dispose;
        Enumerable.call(this, GetEnumerator);
    };
    DisposableEnumerable.prototype = new Enumerable();

    // optimize array or arraylike object

    var ArrayEnumerable = function (source) {
        this.getSource = function () { return source; };
    };
    ArrayEnumerable.prototype = new Enumerable();

    ArrayEnumerable.prototype.any = function (predicate) {
        return (predicate == null)
            ? (this.getSource().length > 0)
            : Enumerable.prototype.any.apply(this, arguments);
    };

    ArrayEnumerable.prototype.count = function (predicate) {
        return (predicate == null)
            ? this.getSource().length
            : Enumerable.prototype.count.apply(this, arguments);
    };

    ArrayEnumerable.prototype.elementAt = function (index) {
        var source = this.getSource();
        return (0 <= index && index < source.length)
            ? source[index]
            : Enumerable.prototype.elementAt.apply(this, arguments);
    };

    ArrayEnumerable.prototype.elementAtOrDefault = function (index, defaultValue) {
        if (defaultValue === undefined) defaultValue = null;
        var source = this.getSource();
        return (0 <= index && index < source.length)
            ? source[index]
            : defaultValue;
    };

    ArrayEnumerable.prototype.first = function (predicate) {
        var source = this.getSource();
        return (predicate == null && source.length > 0)
            ? source[0]
            : Enumerable.prototype.first.apply(this, arguments);
    };

    ArrayEnumerable.prototype.firstOrDefault = function (predicate, defaultValue) {
        if (defaultValue === undefined) defaultValue = null;
        if (predicate != null) {
            return Enumerable.prototype.firstOrDefault.apply(this, arguments);
        }

        var source = this.getSource();
        return source.length > 0 ? source[0] : defaultValue;
    };

    ArrayEnumerable.prototype.last = function (predicate) {
        var source = this.getSource();
        return (predicate == null && source.length > 0)
            ? source[source.length - 1]
            : Enumerable.prototype.last.apply(this, arguments);
    };

    ArrayEnumerable.prototype.lastOrDefault = function (predicate, defaultValue) {
        if (defaultValue === undefined) defaultValue = null;
        if (predicate != null) {
            return Enumerable.prototype.lastOrDefault.apply(this, arguments);
        }

        var source = this.getSource();
        return source.length > 0 ? source[source.length - 1] : defaultValue;
    };

    ArrayEnumerable.prototype.skip = function (count) {
        var source = this.getSource();

        return new Enumerable(function () {
            var index;

            return new IEnumerator(
                function () { index = (count < 0) ? 0 : count; },
                function () {
                    return (index < source.length)
                        ? this.yieldReturn(source[index++])
                        : false;
                },
                Functions.Blank);
        });
    };

    ArrayEnumerable.prototype.takeExceptLast = function (count) {
        if (count == null) count = 1;
        return this.take(this.getSource().length - count);
    };

    ArrayEnumerable.prototype.takeFromLast = function (count) {
        return this.skip(this.getSource().length - count);
    };

    ArrayEnumerable.prototype.reverse = function () {
        var source = this.getSource();

        return new Enumerable(function () {
            var index;

            return new IEnumerator(
                function () {
                    index = source.length;
                },
                function () {
                    return (index > 0)
                        ? this.yieldReturn(source[--index])
                        : false;
                },
                Functions.Blank);
        });
    };

    ArrayEnumerable.prototype.sequenceEqual = function (second, comparer) {
        if ((second instanceof ArrayEnumerable || second instanceof Array)
            && comparer == null
            && Enumerable.from(second).count() != this.count()) {
            return false;
        }

        return Enumerable.prototype.sequenceEqual.apply(this, arguments);
    };

    ArrayEnumerable.prototype.toJoinedString = function (separator, selector) {
        var source = this.getSource();
        if (selector != null || !(source instanceof Array)) {
            return Enumerable.prototype.toJoinedString.apply(this, arguments);
        }

        if (separator == null) separator = "";
        return source.join(separator);
    };

    ArrayEnumerable.prototype.GetEnumerator = function () {
        return new H5.ArrayEnumerator(this.getSource());
    };

    // optimization for multiple where and multiple select and whereselect

    var WhereEnumerable = function (source, predicate) {
        this.prevSource = source;
        this.prevPredicate = predicate; // predicate.length always <= 1
    };
    WhereEnumerable.prototype = new Enumerable();

    WhereEnumerable.prototype.where = function (predicate) {
        predicate = Utils.createLambda(predicate);

        if (predicate.length <= 1) {
            var prevPredicate = this.prevPredicate;
            var composedPredicate = function (x) { return prevPredicate(x) && predicate(x); };
            return new WhereEnumerable(this.prevSource, composedPredicate);
        }
        else {
            // if predicate use index, can't compose
            return Enumerable.prototype.where.call(this, predicate);
        }
    };

    WhereEnumerable.prototype.select = function (selector) {
        selector = Utils.createLambda(selector);

        return (selector.length <= 1)
            ? new WhereSelectEnumerable(this.prevSource, this.prevPredicate, selector)
            : Enumerable.prototype.select.call(this, selector);
    };

    WhereEnumerable.prototype.GetEnumerator = function () {
        var predicate = this.prevPredicate;
        var source = this.prevSource;
        var enumerator;

        return new IEnumerator(
            function () { enumerator = source.GetEnumerator(); },
            function () {
                while (enumerator.moveNext()) {
                    if (predicate(enumerator.Current)) {
                        return this.yieldReturn(enumerator.Current);
                    }
                }
                return false;
            },
            function () { Utils.Dispose(enumerator); });
    };

    var WhereSelectEnumerable = function (source, predicate, selector) {
        this.prevSource = source;
        this.prevPredicate = predicate; // predicate.length always <= 1 or null
        this.prevSelector = selector; // selector.length always <= 1
    };
    WhereSelectEnumerable.prototype = new Enumerable();

    WhereSelectEnumerable.prototype.where = function (predicate) {
        predicate = Utils.createLambda(predicate);

        return (predicate.length <= 1)
            ? new WhereEnumerable(this, predicate)
            : Enumerable.prototype.where.call(this, predicate);
    };

    WhereSelectEnumerable.prototype.select = function (selector) {
        selector = Utils.createLambda(selector);

        if (selector.length <= 1) {
            var prevSelector = this.prevSelector;
            var composedSelector = function (x) { return selector(prevSelector(x)); };
            return new WhereSelectEnumerable(this.prevSource, this.prevPredicate, composedSelector);
        }
        else {
            // if selector use index, can't compose
            return Enumerable.prototype.select.call(this, selector);
        }
    };

    WhereSelectEnumerable.prototype.GetEnumerator = function () {
        var predicate = this.prevPredicate;
        var selector = this.prevSelector;
        var source = this.prevSource;
        var enumerator;

        return new IEnumerator(
            function () { enumerator = source.GetEnumerator(); },
            function () {
                while (enumerator.moveNext()) {
                    if (predicate == null || predicate(enumerator.Current)) {
                        return this.yieldReturn(selector(enumerator.Current));
                    }
                }
                return false;
            },
            function () { Utils.Dispose(enumerator); });
    };

    // Collections

    // dictionary = Dictionary<TKey, TValue[]>
    var Lookup = function (dictionary, order, nullKey) {
        this.count = function () {
            return dictionary.Count;
        };
        this.get = function (key) {
            if (key == null) {
                return Enumerable.from(nullKey ? nullKey : []);
            }

            var value = { v: null };
            var success = dictionary.tryGetValue(key, value);
            return Enumerable.from(success ? value.v : []);
        };
        this.contains = function (key) {
            if (key == null) {
                return !!nullKey;
            }
            return dictionary.containsKey(key);
        };
        this.toEnumerable = function () {
            return Enumerable.from(order).select(function (key) {
                if (key == null) {
                    return new Grouping(key, nullKey);
                }
                return new Grouping(key, dictionary.getItem(key));
            });
        };
        this.GetEnumerator = function () {
            return this.toEnumerable().GetEnumerator();
        };
    };

    H5.definei("System.Linq.ILookup$2");
    Lookup.$$inherits = [];
    H5.Class.addExtend(Lookup, [System.Collections.IEnumerable, System.Linq.ILookup$2]);

    var Grouping = function (groupKey, elements) {
        this.key = function () {
            return groupKey;
        };
        ArrayEnumerable.call(this, elements);
    };
    Grouping.prototype = new ArrayEnumerable();
    H5.definei("System.Linq.IGrouping$2");
    Grouping.prototype.constructor = Grouping;

    Grouping.$$inherits = [];
    H5.Class.addExtend(Grouping, [System.Collections.IEnumerable, System.Linq.IGrouping$2]);

    // module export
    /*if (typeof define === Types.Function && define.amd) { // AMD
        define("linqjs", [], function () { return Enumerable; });
    } else if (typeof module !== Types.Undefined && module.exports) { // Node
        module.exports = Enumerable;
    } else {
        root.Enumerable = Enumerable;
    }*/

    H5.Linq = {};
    H5.Linq.Enumerable = Enumerable;

    System.Linq = System.Linq || {};
    System.Linq.Enumerable = Enumerable;
    System.Linq.Grouping$2 = Grouping;
    System.Linq.Lookup$2 = Lookup;
    System.Linq.OrderedEnumerable$1 = OrderedEnumerable;
})(H5.global);
