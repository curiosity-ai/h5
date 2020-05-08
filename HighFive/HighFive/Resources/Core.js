    var core = {
        global: globals,

        isNode: Object.prototype.toString.call(typeof process !== "undefined" ? process : 0) === "[object process]",

        emptyFn: function () { },

        identity: function (x) {
            return x;
        },

        Deconstruct: function (obj) {
            var args = Array.prototype.slice.call(arguments, 1);

            for (var i = 0; i < args.length; i++) {
                args[i].v = i == 7 ? obj["Rest"] : obj["Item" + (i + 1)];
            }
        },

        toString: function (instance) {
            if (instance == null) {
                throw new System.ArgumentNullException();
            }

            var guardItem = HighFive.$toStringGuard[HighFive.$toStringGuard.length - 1];

            if (instance.toString === Object.prototype.toString || guardItem && guardItem === instance) {
                return HighFive.Reflection.getTypeFullName(instance);
            }

            HighFive.$toStringGuard.push(instance);

            var result = instance.toString();

            HighFive.$toStringGuard.pop();

            return result;
        },

        geti: function (scope, name1, name2) {
            if (scope[name1] !== undefined) {
                return name1;
            }

            if (name2 && scope[name2] != undefined) {
                return name2;
            }

            var name = name2 || name1;
            var idx = name.lastIndexOf("$");

            if (/\$\d+$/g.test(name)) {
                idx = name.lastIndexOf("$", idx - 1);
            }

            return name.substr(idx + 1);
        },

        box: function (v, T, toStr, hashCode) {
            if (v && v.$boxed) {
                return v;
            }

            if (v == null) {
                return v;
            }

            if (v.$clone) {
                v = v.$clone();
            }

            return {
                $boxed: true,
                fn: {
                    toString: toStr,
                    getHashCode: hashCode
                },
                v: v,
                type: T,
                constructor: T,
                getHashCode: function () {
                    return this.fn.getHashCode ? this.fn.getHashCode(this.v) : HighFive.getHashCode(this.v);
                },
                equals: function (o) {
                    if (this === o) {
                        return true;
                    }

                    var eq = this.equals;
                    this.equals = null;
                    var r = HighFive.equals(this.v, o);
                    this.equals = eq;

                    return r;
                },
                valueOf: function () {
                    return this.v;
                },
                toString: function () {
                    return this.fn.toString ? this.fn.toString(this.v) : this.v.toString();
                }
            };
        },

        unbox: function (o, noclone) {
            var T;

            if (noclone && HighFive.isFunction(noclone)) {
                T = noclone;
                noclone = false;
            }

            if (o && o.$boxed) {
                var v = o.v,
                    t = o.type;

                if (T && T.$nullable) {
                    T = T.$nullableType;
                }

                if (T && T.$kind === "enum") {
                    T = System.Enum.getUnderlyingType(T);
                }

                if (t.$nullable) {
                    t = t.$nullableType;
                }

                if (t.$kind === "enum") {
                    t = System.Enum.getUnderlyingType(t);
                }

                if (T && T !== t && !HighFive.isObject(T)) {
                    throw new System.InvalidCastException.$ctor1("Specified cast is not valid.");
                }

                if (!noclone && v && v.$clone) {
                    v = v.$clone();
                }

                return v;
            }

            if (HighFive.isArray(o)) {
                for (var i = 0; i < o.length; i++) {
                    var item = o[i];

                    if (item && item.$boxed) {
                        item = item.v;

                        if (item.$clone) {
                            item = item.$clone();
                        }
                    } else if (!noclone && item && item.$clone) {
                        item = item.$clone();
                    }

                    o[i] = item;
                }
            }

            if (o && !noclone && o.$clone) {
                o = o.$clone();
            }

            return o;
        },

        virtualc: function (name) {
            return HighFive.virtual(name, true);
        },

        virtual: function (name, isClass) {
            var type = HighFive.unroll(name);

            if (!type || !HighFive.isFunction(type)) {
                var old = HighFive.Class.staticInitAllow;
                type = isClass ? HighFive.define(name) : HighFive.definei(name);
                HighFive.Class.staticInitAllow = true;

                if (type.$staticInit) {
                    type.$staticInit();
                }

                HighFive.Class.staticInitAllow = old;
            }

            return type;
        },

        safe: function (fn) {
            try {
                return fn();
            } catch (ex) {
            }

            return false;
        },

        literal: function (type, obj) {
            obj.$getType = function () { return type };

            return obj;
        },

        isJSObject: function (value) {
            return Object.prototype.toString.call(value) === "[object Object]";
        },

        isPlainObject: function (obj) {
            if (typeof obj == "object" && obj !== null) {
                if (typeof Object.getPrototypeOf == "function") {
                    var proto = Object.getPrototypeOf(obj);

                    return proto === Object.prototype || proto === null;
                }

                return Object.prototype.toString.call(obj) === "[object Object]";
            }

            return false;
        },

        toPlain: function (o) {
            if (!o || HighFive.isPlainObject(o) || typeof o != "object") {
                return o;
            }

            if (typeof o.toJSON == "function") {
                return o.toJSON();
            }

            if (HighFive.isArray(o)) {
                var arr = [];

                for (var i = 0; i < o.length; i++) {
                    arr.push(HighFive.toPlain(o[i]));
                }

                return arr;
            }

            var newo = {},
                m;

            for (var key in o) {
                m = o[key];

                if (!HighFive.isFunction(m)) {
                    newo[key] = m;
                }
            }

            return newo;
        },

        ref: function (o, n) {
            if (HighFive.isArray(n)) {
                n = System.Array.toIndex(o, n);
            }

            var proxy = {};

            Object.defineProperty(proxy, "v", {
                get: function () {
                    if (n == null) {
                        return o;
                    }

                    return o[n];
                },

                set: function (value) {
                    if (n == null) {
                        if (value && value.$clone) {
                            value.$clone(o);
                        } else {
                            o = value;
                        }
                    }

                    o[n] = value;
                }
            });

            return proxy;
        },

        ensureBaseProperty: function (scope, name, alias) {
            var scopeType = HighFive.getType(scope),
                descriptors = scopeType.$descriptors || [];

            scope.$propMap = scope.$propMap || {};

            if (scope.$propMap[name]) {
                return scope;
            }

            if ((!scopeType.$descriptors || scopeType.$descriptors.length === 0) && alias) {
                var aliasCfg = {},
                    aliasName = "$" + alias + "$" + name;

                aliasCfg.get = function () {
                    return scope[name];
                };

                aliasCfg.set = function (value) {
                    scope[name] = value;
                };

                HighFive.property(scope, aliasName, aliasCfg, false, scopeType, true);
            }
            else {
                for (var j = 0; j < descriptors.length; j++) {
                    var d = descriptors[j];

                    if (d.name === name) {
                        var aliasCfg = {},
                            aliasName = "$" + HighFive.getTypeAlias(d.cls) + "$" + name;

                        if (d.get) {
                            aliasCfg.get = d.get;
                        }

                        if (d.set) {
                            aliasCfg.set = d.set;
                        }

                        HighFive.property(scope, aliasName, aliasCfg, false, scopeType, true);
                    }
                }
            }

            scope.$propMap[name] = true;

            return scope;
        },

        property: function (scope, name, v, statics, cls, alias) {
            var cfg = {
                enumerable: alias ? false : true,
                configurable: true
            };

            if (v && v.get) {
                cfg.get = v.get;
            }

            if (v && v.set) {
                cfg.set = v.set;
            }

            if (!v || !(v.get || v.set)) {
                var backingField = HighFive.getTypeAlias(cls) + "$" + name;

                cls.$init = cls.$init || {};

                if (statics) {
                    cls.$init[backingField] = v;
                }

                (function (cfg, scope, backingField, v) {
                    cfg.get = function () {
                        var o = this.$init[backingField];

                        return o === undefined ? v : o;
                    };

                    cfg.set = function (value) {
                        this.$init[backingField] = value;
                    };
                })(cfg, scope, backingField, v);
            }

            Object.defineProperty(scope, name, cfg);

            return cfg;
        },

        event: function (scope, name, v, statics) {
            scope[name] = v;

            var rs = name.charAt(0) === "$",
                cap = rs ? name.slice(1) : name,
                addName = "add" + cap,
                removeName = "remove" + cap,
                lastSep = name.lastIndexOf("$"),
                endsNum = lastSep > 0 && ((name.length - lastSep - 1) > 0) && !isNaN(parseInt(name.substr(lastSep + 1)));

            if (endsNum) {
                lastSep = name.substring(0, lastSep - 1).lastIndexOf("$");
            }

            if (lastSep > 0 && lastSep !== (name.length - 1)) {
                addName = name.substring(0, lastSep) + "add" + name.substr(lastSep + 1);
                removeName = name.substring(0, lastSep) + "remove" + name.substr(lastSep + 1);
            }

            scope[addName] = (function (name, scope, statics) {
                return statics ? function (value) {
                    scope[name] = HighFive.fn.combine(scope[name], value);
                } : function (value) {
                    this[name] = HighFive.fn.combine(this[name], value);
                };
            })(name, scope, statics);

            scope[removeName] = (function (name, scope, statics) {
                return statics ? function (value) {
                    scope[name] = HighFive.fn.remove(scope[name], value);
                } : function (value) {
                    this[name] = HighFive.fn.remove(this[name], value);
                };
            })(name, scope, statics);
        },

        createInstance: function (type, nonPublic, args) {
            if (HighFive.isArray(nonPublic)) {
                args = nonPublic;
                nonPublic = false;
            }

            if (type === System.Decimal) {
                return System.Decimal.Zero;
            }

            if (type === System.Int64) {
                return System.Int64.Zero;
            }

            if (type === System.UInt64) {
                return System.UInt64.Zero;
            }

            if (type === System.Double ||
                type === System.Single ||
                type === System.Byte ||
                type === System.SByte ||
                type === System.Int16 ||
                type === System.UInt16 ||
                type === System.Int32 ||
                type === System.UInt32 ||
                type === HighFive.Int) {
                return 0;
            }

            if (typeof (type.createInstance) === "function") {
                return type.createInstance();
            } else if (typeof (type.getDefaultValue) === "function") {
                return type.getDefaultValue();
            } else if (type === Boolean || type === System.Boolean) {
                return false;
            } else if (type === System.DateTime) {
                return System.DateTime.getDefaultValue();
            } else if (type === Date) {
                return new Date();
            } else if (type === Number) {
                return 0;
            } else if (type === String || type === System.String) {
                return "";
            } else if (type && type.$literal) {
                return type.ctor();
            } else if (args && args.length > 0) {
                return HighFive.Reflection.applyConstructor(type, args);
            }

            if (type.$kind === 'interface') {
                throw new System.MissingMethodException.$ctor1('Default constructor not found for type ' + HighFive.getTypeName(type));
            }

            var ctors = HighFive.Reflection.getMembers(type, 1, 54);

            if (ctors.length > 0) {
                var pctors = ctors.filter(function (c) { return !c.isSynthetic && !c.sm; });

                for (var idx = 0; idx < pctors.length; idx++) {
                    var c = pctors[idx],
                        isDefault = (c.pi || []).length === 0;

                    if (isDefault) {
                        if (nonPublic || c.a === 2) {
                            return HighFive.Reflection.invokeCI(c, []);
                        }
                        throw new System.MissingMethodException.$ctor1('Default constructor not found for type ' + HighFive.getTypeName(type));
                    }
                }

                if (type.$$name && !(ctors.length == 1 && ctors[0].isSynthetic)) {
                    throw new System.MissingMethodException.$ctor1('Default constructor not found for type ' + HighFive.getTypeName(type));
                }
            }

            return new type();
        },

        clone: function (obj) {
            if (obj == null) {
                return obj;
            }

            if (HighFive.isArray(obj)) {
                return System.Array.clone(obj);
            }

            if (HighFive.isString(obj)) {
                return obj;
            }

            var name;

            if (HighFive.isFunction(HighFive.getProperty(obj, name = "System$ICloneable$clone"))) {
                return obj[name]();
            }

            if (HighFive.is(obj, System.ICloneable)) {
                return obj.clone();
            }

            if (HighFive.isFunction(obj.$clone)) {
                return obj.$clone();
            }

            return null;
        },

        copy: function (to, from, keys, toIf) {
            if (typeof keys === "string") {
                keys = keys.split(/[,;\s]+/);
            }

            for (var name, i = 0, n = keys ? keys.length : 0; i < n; i++) {
                name = keys[i];

                if (toIf !== true || to[name] == undefined) {
                    if (HighFive.is(from[name], System.ICloneable)) {
                        to[name] = HighFive.clone(from[name]);
                    } else {
                        to[name] = from[name];
                    }
                }
            }

            return to;
        },

        get: function (t) {
            if (t && t.$staticInit !== null) {
                t.$staticInit();
            }

            return t;
        },

        ns: function (ns, scope) {
            var nsParts = ns.split("."),
                i = 0;

            if (!scope) {
                scope = HighFive.global;
            }

            for (i = 0; i < nsParts.length; i++) {
                if (typeof scope[nsParts[i]] === "undefined") {
                    scope[nsParts[i]] = {};
                }

                scope = scope[nsParts[i]];
            }

            return scope;
        },

        ready: function (fn, scope) {
            var delayfn = function () {
                if (scope) {
                    fn.apply(scope);
                } else {
                    fn();
                }
            };

            if (typeof HighFive.global.jQuery !== "undefined") {
                HighFive.global.jQuery(delayfn);
            } else {
                if (typeof HighFive.global.document === "undefined" ||
                    HighFive.global.document.readyState === "complete" ||
                    HighFive.global.document.readyState === "loaded" ||
                    HighFive.global.document.readyState === "interactive") {
                    delayfn();
                } else {
                    HighFive.on("DOMContentLoaded", HighFive.global.document, delayfn);
                }
            }
        },

        on: function (event, elem, fn, scope) {
            var listenHandler = function (e) {
                var ret = fn.apply(scope || this, arguments);

                if (ret === false) {
                    e.stopPropagation();
                    e.preventDefault();
                }

                return (ret);
            };

            var attachHandler = function () {
                var ret = fn.call(scope || elem, HighFive.global.event);

                if (ret === false) {
                    HighFive.global.event.returnValue = false;
                    HighFive.global.event.cancelBubble = true;
                }

                return (ret);
            };

            if (elem.addEventListener) {
                elem.addEventListener(event, listenHandler, false);
            } else {
                elem.attachEvent("on" + event, attachHandler);
            }
        },

        addHash: function (v, r, m) {
            if (isNaN(r)) {
                r = 17;
            }

            if (isNaN(m)) {
                m = 23;
            }

            if (HighFive.isArray(v)) {
                for (var i = 0; i < v.length; i++) {
                    r = r + ((r * m | 0) + (v[i] == null ? 0 : HighFive.getHashCode(v[i]))) | 0;
                }

                return r;
            }

            return r = r + ((r * m | 0) + (v == null ? 0 : HighFive.getHashCode(v))) | 0;
        },

        getHashCode: function (value, safe, deep) {
            // In CLR: mutable object should keep on returning same value
            // HighFive.NET goals: make it deterministic (to make testing easier) without breaking CLR contracts
            //     for value types it returns deterministic values (f.e. for int 3 it returns 3)
            //     for reference types it returns random value

            if (value && value.$boxed && value.type.getHashCode) {
                return value.type.getHashCode(HighFive.unbox(value, true));
            }

            value = HighFive.unbox(value, true);

            if (HighFive.isEmpty(value, true)) {
                if (safe) {
                    return 0;
                }

                throw new System.InvalidOperationException.$ctor1("HashCode cannot be calculated for empty value");
            }

            if (value.getHashCode && HighFive.isFunction(value.getHashCode) && !value.__insideHashCode && value.getHashCode.length === 0) {
                value.__insideHashCode = true;
                var r = value.getHashCode();

                delete value.__insideHashCode;

                return r;
            }

            if (HighFive.isBoolean(value)) {
                return value ? 1 : 0;
            }

            if (HighFive.isDate(value)) {
                var val = value.ticks !== undefined ? value.ticks : System.DateTime.getTicks(value);

                return val.toNumber() & 0xFFFFFFFF;
            }

            if (value === Number.POSITIVE_INFINITY) {
                return 0x7FF00000;
            }

            if (value === Number.NEGATIVE_INFINITY) {
                return 0xFFF00000;
            }

            if (HighFive.isNumber(value)) {
                if (Math.floor(value) === value) {
                    return value;
                }

                value = value.toExponential();
            }

            if (HighFive.isString(value)) {
                if (Math.imul) {
                    for (var i = 0, h = 0; i < value.length; i++)
                        h = Math.imul(31, h) + value.charCodeAt(i) | 0;
                    return h;
                } else {
                    var h = 0, l = value.length, i = 0;
                    if (l > 0)
                        while (i < l)
                            h = (h << 5) - h + value.charCodeAt(i++) | 0;
                    return h;
                }
            }

            if (value.$$hashCode) {
                return value.$$hashCode;
            }

            if (deep !== false && value.hasOwnProperty("Item1") && HighFive.isPlainObject(value)) {
                deep = true;
            }

            if (deep && typeof value == "object") {
                var result = 0,
                    temp;

                for (var property in value) {
                    if (value.hasOwnProperty(property)) {
                        temp = HighFive.isEmpty(value[property], true) ? 0 : HighFive.getHashCode(value[property]);
                        result = 29 * result + temp;
                    }
                }

                if (result !== 0) {
                    value.$$hashCode = result;

                    return result;
                }
            }

            value.$$hashCode = (Math.random() * 0x100000000) | 0;

            return value.$$hashCode;
        },

        getDefaultValue: function (type) {
            if (type == null) {
                throw new System.ArgumentNullException.$ctor1("type");
            } else if ((type.getDefaultValue) && type.getDefaultValue.length === 0) {
                return type.getDefaultValue();
            } else if (HighFive.Reflection.isEnum(type)) {
                return System.Enum.parse(type, 0);
            } else if (type === Boolean || type === System.Boolean) {
                return false;
            } else if (type === System.DateTime) {
                return System.DateTime.getDefaultValue();
            } else if (type === Date) {
                return new Date();
            } else if (type === Number) {
                return 0;
            }

            return null;
        },

        $$aliasCache: [],

        getTypeAlias: function (obj) {
            if (obj.$$alias) {
                return obj.$$alias;
            }

            var type = (obj.$$name || typeof obj === "function") ? obj : HighFive.getType(obj),
                alias;

            if (type.$$alias) {
                return type.$$alias;
            }

            alias = HighFive.$$aliasCache[type];
            if (alias) {
                return alias;
            }

            if (type.$isArray) {
                var elementName = HighFive.getTypeAlias(type.$elementType);
                alias = elementName + "$Array" + (type.$rank > 1 ? ("$" + type.$rank) : "");

                if (type.$$name) {
                    type.$$alias = alias;
                } else {
                    HighFive.$$aliasCache[type] = alias;
                }

                return alias;
            }

            var name = obj.$$name || HighFive.getTypeName(obj);

            if (type.$typeArguments && !type.$isGenericTypeDefinition) {
                name = type.$genericTypeDefinition.$$name;

                for (var i = 0; i < type.$typeArguments.length; i++) {
                    var ta = type.$typeArguments[i];
                    name += "$" + HighFive.getTypeAlias(ta);
                }
            }

            alias = name.replace(/[\.\(\)\,\+]/g, "$");

            if (type.$module) {
                alias = type.$module + "$" + alias;
            }

            if (type.$$name) {
                type.$$alias = alias;
            } else {
                HighFive.$$aliasCache[type] = alias;
            }

            return alias;
        },

        getTypeName: function (obj) {
            return HighFive.Reflection.getTypeFullName(obj);
        },

        hasValue: function (obj) {
            return HighFive.unbox(obj, true) != null;
        },

        hasValue$1: function () {
            if (arguments.length === 0) {
                return false;
            }

            var i = 0;

            for (i; i < arguments.length; i++) {
                if (HighFive.unbox(arguments[i], true) == null) {
                    return false;
                }
            }

            return true;
        },

        isObject: function (type) {
            return type === Object || type === System.Object;
        },

        is: function (obj, type, ignoreFn, allowNull) {
            if (obj == null) {
                return !!allowNull;
            }

            if (type === System.Object) {
                type = Object;
            }

            var tt = typeof type;

            if (tt === "boolean") {
                return type;
            }

            if (obj.$boxed) {
                if (obj.type.$kind === "enum" && (obj.type.prototype.$utype === type || type === System.Enum || type === System.IFormattable || type === System.IComparable)) {
                    return true;
                } else if (!HighFive.Reflection.isInterface(type) && !type.$nullable) {
                    return obj.type === type || HighFive.isObject(type) || type === System.ValueType && HighFive.Reflection.isValueType(obj.type);
                }

                if (ignoreFn !== true && type.$is) {
                    return type.$is(HighFive.unbox(obj, true));
                }

                if (HighFive.Reflection.isAssignableFrom(type, obj.type)) {
                    return true;
                }

                obj = HighFive.unbox(obj, true);
            }

            var ctor = obj.constructor === Object && obj.$getType ? obj.$getType() : HighFive.Reflection.convertType(obj.constructor);

            if (type.constructor === Function && obj instanceof type || ctor === type || HighFive.isObject(type)) {
                return true;
            }

            var hasObjKind = ctor.$kind || ctor.$$inherits,
                hasTypeKind = type.$kind;

            if (hasObjKind || hasTypeKind) {
                var isInterface = type.$isInterface;

                if (isInterface) {
                    if (hasObjKind) {
                        if (ctor.$isArrayEnumerator) {
                            return System.Array.is(obj, type);
                        }

                        return type.isAssignableFrom ? type.isAssignableFrom(ctor) : HighFive.Reflection.getInterfaces(ctor).indexOf(type) >= 0;
                    }

                    if (HighFive.isArray(obj, ctor)) {
                        return System.Array.is(obj, type);
                    }
                }

                if (ignoreFn !== true && type.$is) {
                    return type.$is(obj);
                }

                if (type.$literal) {
                    if (HighFive.isPlainObject(obj)) {
                        if (obj.$getType) {
                            return HighFive.Reflection.isAssignableFrom(type, obj.$getType());
                        }

                        return true;
                    }
                }

                return false;
            }

            if (tt === "string") {
                type = HighFive.unroll(type);
            }

            if (tt === "function" && (HighFive.getType(obj).prototype instanceof type)) {
                return true;
            }

            if (ignoreFn !== true) {
                if (typeof (type.$is) === "function") {
                    return type.$is(obj);
                }

                if (typeof (type.isAssignableFrom) === "function") {
                    return type.isAssignableFrom(HighFive.getType(obj));
                }
            }

            if (HighFive.isArray(obj)) {
                return System.Array.is(obj, type);
            }

            return tt === "object" && ((ctor === type) || (obj instanceof type));
        },

        as: function (obj, type, allowNull) {
            if (HighFive.is(obj, type, false, allowNull)) {
                return obj != null && obj.$boxed && type !== Object && type !== System.Object ? obj.v : obj;
            }
            return null;
        },

        cast: function (obj, type, allowNull) {
            if (obj == null) {
                return obj;
            }

            var result = HighFive.is(obj, type, false, allowNull) ? obj : null;

            if (result === null) {
                throw new System.InvalidCastException.$ctor1("Unable to cast type " + (obj ? HighFive.getTypeName(obj) : "'null'") + " to type " + HighFive.getTypeName(type));
            }

            if (obj.$boxed && type !== Object && type !== System.Object) {
                return obj.v;
            }

            return result;
        },

        apply: function (obj, values, callback) {
            var names = HighFive.getPropertyNames(values, true),
                i;

            for (i = 0; i < names.length; i++) {
                var name = names[i];

                if (typeof obj[name] === "function" && typeof values[name] !== "function") {
                    obj[name](values[name]);
                } else {
                    obj[name] = values[name];
                }
            }

            if (callback) {
                callback.call(obj, obj);
            }

            return obj;
        },

        copyProperties: function (to, from) {
            var names = HighFive.getPropertyNames(from, false),
                i;

            for (i = 0; i < names.length; i++) {
                var name = names[i],
                    own = from.hasOwnProperty(name),
                    dcount = name.split("$").length;

                if (own && (dcount === 1 || dcount === 2 && name.match("\$\d+$"))) {
                    to[name] = from[name];
                }

            }

            return to;
        },

        merge: function (to, from, callback, elemFactory) {
            if (to == null) {
                return from;
            }

            // Maps instance of plain JS value or Object into HighFive object.
            // Used for deserialization. Proper deserialization requires reflection that is currently not supported in HighFive.
            // It currently is only capable to deserialize:
            // -instance of single class or primitive
            // -array of primitives
            // -array of single class
            if (to instanceof System.Decimal && typeof from === "number") {
                return new System.Decimal(from);
            }

            if (to instanceof System.Int64 && HighFive.isNumber(from)) {
                return new System.Int64(from);
            }

            if (to instanceof System.UInt64 && HighFive.isNumber(from)) {
                return new System.UInt64(from);
            }

            if (to instanceof Boolean || HighFive.isBoolean(to) ||
                typeof to === "number" ||
                to instanceof String || HighFive.isString(to) ||
                to instanceof Function || HighFive.isFunction(to) ||
                to instanceof Date || HighFive.isDate(to) ||
                HighFive.getType(to).$number) {
                return from;
            }

            var key,
                i,
                value,
                toValue,
                fn;

            if (HighFive.isArray(from) && HighFive.isFunction(to.add || to.push)) {
                fn = HighFive.isArray(to) ? to.push : to.add;

                for (i = 0; i < from.length; i++) {
                    var item = from[i];

                    if (!HighFive.isArray(item)) {
                        item = [typeof elemFactory === "undefined" ? item : HighFive.merge(elemFactory(), item)];
                    }

                    fn.apply(to, item);
                }
            } else {
                var t = HighFive.getType(to),
                    descriptors = t && t.$descriptors;

                if (from) {
                    for (key in from) {
                        value = from[key];

                        var descriptor = null;

                        if (descriptors) {
                            for (var i = descriptors.length - 1; i >= 0; i--) {
                                if (descriptors[i].name === key) {
                                    descriptor = descriptors[i];

                                    break;
                                }
                            }
                        }

                        if (descriptor != null) {
                            if (descriptor.set) {
                                to[key] = HighFive.merge(to[key], value);
                            } else {
                                HighFive.merge(to[key], value);
                            }
                        } else if (typeof to[key] === "function") {
                            if (key.match(/^\s*get[A-Z]/)) {
                                HighFive.merge(to[key](), value);
                            } else {
                                to[key](value);
                            }
                        } else {
                            var setter1 = "set" + key.charAt(0).toUpperCase() + key.slice(1),
                                setter2 = "set" + key,
                                getter;

                            if (typeof to[setter1] === "function" && typeof value !== "function") {
                                getter = "g" + setter1.slice(1);

                                if (typeof to[getter] === "function") {
                                    to[setter1](HighFive.merge(to[getter](), value));
                                } else {
                                    to[setter1](value);
                                }
                            } else if (typeof to[setter2] === "function" && typeof value !== "function") {
                                getter = "g" + setter2.slice(1);

                                if (typeof to[getter] === "function") {
                                    to[setter2](HighFive.merge(to[getter](), value));
                                } else {
                                    to[setter2](value);
                                }
                            } else if (value && value.constructor === Object && to[key]) {
                                toValue = to[key];
                                HighFive.merge(toValue, value);
                            } else {
                                var isNumber = HighFive.isNumber(from);

                                if (to[key] instanceof System.Decimal && isNumber) {
                                    return new System.Decimal(from);
                                }

                                if (to[key] instanceof System.Int64 && isNumber) {
                                    return new System.Int64(from);
                                }

                                if (to[key] instanceof System.UInt64 && isNumber) {
                                    return new System.UInt64(from);
                                }

                                to[key] = value;
                            }
                        }
                    }
                } else {
                    if (callback) {
                        callback.call(to, to);
                    }

                    return from;
                }
            }

            if (callback) {
                callback.call(to, to);
            }

            return to;
        },

        getEnumerator: function (obj, fnName, T) {
            if (typeof obj === "string") {
                obj = System.String.toCharArray(obj);
            }

            if (arguments.length === 2 && HighFive.isFunction(fnName)) {
                T = fnName;
                fnName = null;
            }

            if (fnName && obj && obj[fnName]) {
                return obj[fnName].call(obj);
            }

            if (!T && obj && obj.GetEnumerator) {
                return obj.GetEnumerator();
            }

            var name;

            if (T && HighFive.isFunction(HighFive.getProperty(obj, name = "System$Collections$Generic$IEnumerable$1$" + HighFive.getTypeAlias(T) + "$GetEnumerator"))) {
                return obj[name]();
            }

            if (T && HighFive.isFunction(HighFive.getProperty(obj, name = "System$Collections$Generic$IEnumerable$1$GetEnumerator"))) {
                return obj[name]();
            }

            if (HighFive.isFunction(HighFive.getProperty(obj, name = "System$Collections$IEnumerable$GetEnumerator"))) {
                return obj[name]();
            }

            if (T && obj && obj.GetEnumerator) {
                return obj.GetEnumerator();
            }

            if ((Object.prototype.toString.call(obj) === "[object Array]") ||
                (obj && HighFive.isDefined(obj.length))) {
                return new HighFive.ArrayEnumerator(obj, T);
            }

            throw new System.InvalidOperationException.$ctor1("Cannot create Enumerator.");
        },

        getPropertyNames: function (obj, includeFunctions) {
            var names = [],
                name;

            for (name in obj) {
                if (includeFunctions || typeof obj[name] !== "function") {
                    names.push(name);
                }
            }

            return names;
        },

        getProperty: function (obj, propertyName) {
            if (HighFive.isHtmlAttributeCollection(obj) && !this.isValidHtmlAttributeName(propertyName)) {
                return undefined;
            }

            return obj[propertyName];
        },

        isValidHtmlAttributeName : function (name) {
            // https://html.spec.whatwg.org/multipage/syntax.html#attributes-2

            if (!name || !name.length) {
                return false;
            }

            var r = /^[a-zA-Z_][\w\-]*$/;

            return r.test(name);
        },

        isHtmlAttributeCollection: function (obj) {
            return typeof obj !== "undefined" && (Object.prototype.toString.call(obj) === "[object NamedNodeMap]");
        },

        isDefined: function (value, noNull) {
            return typeof value !== "undefined" && (noNull ? value !== null : true);
        },

        isEmpty: function (value, allowEmpty) {
            return (typeof value === "undefined" || value === null) || (!allowEmpty ? value === "" : false) || ((!allowEmpty && HighFive.isArray(value)) ? value.length === 0 : false);
        },

        toArray: function (ienumerable) {
            var i,
                item,
                len,
                result = [];

            if (HighFive.isArray(ienumerable)) {
                for (i = 0, len = ienumerable.length; i < len; ++i) {
                    result.push(ienumerable[i]);
                }
            } else {
                i = HighFive.getEnumerator(ienumerable);

                while (i.moveNext()) {
                    item = i.Current;
                    result.push(item);
                }
            }

            return result;
        },

        toList: function (ienumerable, T) {
            return new (System.Collections.Generic.List$1(T || System.Object).$ctor1)(ienumerable);
        },

        arrayTypes: [globals.Array, globals.Uint8Array, globals.Int8Array, globals.Int16Array, globals.Uint16Array, globals.Int32Array, globals.Uint32Array, globals.Float32Array, globals.Float64Array, globals.Uint8ClampedArray],

        isArray: function (obj, ctor) {
            var c = ctor || (obj != null ? obj.constructor : null);

            if (!c) {
                return false;
            }

            return HighFive.arrayTypes.indexOf(c) >= 0 || c.$isArray || Array.isArray(obj);
        },

        isFunction: function (obj) {
            return typeof (obj) === "function";
        },

        isDate: function (obj) {
            return obj instanceof Date || Object.prototype.toString.call(obj) === "[object Date]";
        },

        isNull: function (value) {
            return (value === null) || (value === undefined);
        },

        isBoolean: function (value) {
            return typeof value === "boolean";
        },

        isNumber: function (value) {
            return typeof value === "number" && isFinite(value);
        },

        isString: function (value) {
            return typeof value === "string";
        },

        unroll: function (value, scope) {
            if (HighFive.isArray(value)) {
                for (var i = 0; i < value.length; i++) {
                    var v = value[i];

                    if (HighFive.isString(v)) {
                        value[i] = HighFive.unroll(v, scope);
                    }
                }

                return;
            }

            var d = value.split("."),
                o = (scope || HighFive.global)[d[0]],
                i = 1;

            for (i; i < d.length; i++) {
                if (!o) {
                    return null;
                }

                o = o[d[i]];
            }

            return o;
        },

        referenceEquals: function (a, b) {
            return HighFive.hasValue(a) ? a === b : !HighFive.hasValue(b);
        },

        staticEquals: function (a, b) {
            if (!HighFive.hasValue(a)) {
                return !HighFive.hasValue(b);
            }

            return HighFive.hasValue(b) ? HighFive.equals(a, b) : false;
        },

        equals: function (a, b) {
            if (a == null && b == null) {
                return true;
            }

            var guardItem = HighFive.$equalsGuard[HighFive.$equalsGuard.length - 1];

            if (guardItem && guardItem.a === a && guardItem.b === b) {
                return a === b;
            }

            HighFive.$equalsGuard.push({a: a, b: b});

            var fn = function (a, b) {
                if (a && a.$boxed && a.type.equals && a.type.equals.length === 2) {
                    return a.type.equals(a, b);
                }

                if (b && b.$boxed && b.type.equals && b.type.equals.length === 2) {
                    return b.type.equals(b, a);
                }

                if (a && HighFive.isFunction(a.equals) && a.equals.length === 1) {
                    return a.equals(b);
                }

                if (b && HighFive.isFunction(b.equals) && b.equals.length === 1) {
                    return b.equals(a);
                } if (HighFive.isFunction(a) && HighFive.isFunction(b)) {
                    return HighFive.fn.equals.call(a, b);
                } else if (HighFive.isDate(a) && HighFive.isDate(b)) {
                    if (a.kind !== undefined && a.ticks !== undefined && b.kind !== undefined && b.ticks !== undefined) {
                        return a.ticks.equals(b.ticks);
                    }

                    return a.valueOf() === b.valueOf();
                } else if (HighFive.isNull(a) && HighFive.isNull(b)) {
                    return true;
                } else if (HighFive.isNull(a) !== HighFive.isNull(b)) {
                    return false;
                }

                var eq = a === b;

                if (!eq && typeof a === "object" && typeof b === "object" && a !== null && b !== null && a.$kind === "struct" && b.$kind === "struct" && a.$$name === b.$$name) {
                    return HighFive.getHashCode(a) === HighFive.getHashCode(b) && HighFive.objectEquals(a, b);
                }

                if (!eq && a && b && a.hasOwnProperty("Item1") && HighFive.isPlainObject(a) && b.hasOwnProperty("Item1") && HighFive.isPlainObject(b)) {
                    return HighFive.objectEquals(a, b, true);
                }

                return eq;
            };

            var result = fn(a, b);
            HighFive.$equalsGuard.pop();

            return result;
        },

        objectEquals: function (a, b, oneLevel) {
            HighFive.$$leftChain = [];
            HighFive.$$rightChain = [];

            var result = HighFive.deepEquals(a, b, oneLevel);

            delete HighFive.$$leftChain;
            delete HighFive.$$rightChain;

            return result;
        },

        deepEquals: function (a, b, oneLevel) {
            if (typeof a === "object" && typeof b === "object") {
                if (a === b) {
                    return true;
                }

                if (HighFive.$$leftChain.indexOf(a) > -1 || HighFive.$$rightChain.indexOf(b) > -1) {
                    return false;
                }

                var p;

                for (p in b) {
                    if (b.hasOwnProperty(p) !== a.hasOwnProperty(p)) {
                        return false;
                    } else if (typeof b[p] !== typeof a[p]) {
                        return false;
                    }
                }

                for (p in a) {
                    if (b.hasOwnProperty(p) !== a.hasOwnProperty(p)) {
                        return false;
                    } else if (typeof a[p] !== typeof b[p]) {
                        return false;
                    }

                    if (a[p] === b[p]) {
                        continue;
                    } else if (typeof (a[p]) === "object" && !oneLevel) {
                        HighFive.$$leftChain.push(a);
                        HighFive.$$rightChain.push(b);

                        if (!HighFive.deepEquals(a[p], b[p])) {
                            return false;
                        }

                        HighFive.$$leftChain.pop();
                        HighFive.$$rightChain.pop();
                    } else {
                        if (!HighFive.equals(a[p], b[p])) {
                            return false;
                        }
                    }
                }

                return true;
            } else {
                return HighFive.equals(a, b);
            }
        },

        numberCompare : function (a, b) {
            if (a < b) {
                return -1;
            }

            if (a > b) {
                return 1;
            }

            if (a == b) {
                return 0;
            }

            if (!isNaN(a)) {
                return 1;
            }

            if (!isNaN(b)) {
                return -1;
            }

            return 0;
        },

        compare: function (a, b, safe, T) {
            if (a && a.$boxed) {
                a = HighFive.unbox(a, true);
            }

            if (b && b.$boxed) {
                b = HighFive.unbox(b, true);
            }

            if (typeof a === "number" && typeof b === "number") {
                return HighFive.numberCompare(a, b);
            }

            if (!HighFive.isDefined(a, true)) {
                if (safe) {
                    return 0;
                }

                throw new System.NullReferenceException();
            } else if (HighFive.isString(a)) {
                return System.String.compare(a, b);
            } else if (HighFive.isNumber(a) || HighFive.isBoolean(a)) {
                return a < b ? -1 : (a > b ? 1 : 0);
            } else if (HighFive.isDate(a)) {
                if (a.kind !== undefined && a.ticks !== undefined) {
                    return HighFive.compare(System.DateTime.getTicks(a), System.DateTime.getTicks(b));
                }

                return HighFive.compare(a.valueOf(), b.valueOf());
            }

            var name;

            if (T && HighFive.isFunction(HighFive.getProperty(a, name = "System$IComparable$1$" + HighFive.getTypeAlias(T) + "$compareTo"))) {
                return a[name](b);
            }

            if (T && HighFive.isFunction(HighFive.getProperty(a, name = "System$IComparable$1$compareTo"))) {
                return a[name](b);
            }

            if (HighFive.isFunction(HighFive.getProperty(a, name = "System$IComparable$compareTo"))) {
                return a[name](b);
            }

            if (HighFive.isFunction(a.compareTo)) {
                return a.compareTo(b);
            }

            if (T && HighFive.isFunction(HighFive.getProperty(b, name = "System$IComparable$1$" + HighFive.getTypeAlias(T) + "$compareTo"))) {
                return -b[name](a);
            }

            if (T && HighFive.isFunction(HighFive.getProperty(b, name = "System$IComparable$1$compareTo"))) {
                return -b[name](a);
            }

            if (HighFive.isFunction(HighFive.getProperty(b, name = "System$IComparable$compareTo"))) {
                return -b[name](a);
            }

            if (HighFive.isFunction(b.compareTo)) {
                return -b.compareTo(a);
            }

            if (safe) {
                return 0;
            }

            throw new System.Exception("Cannot compare items");
        },

        equalsT: function (a, b, T) {
            if (a && a.$boxed && a.type.equalsT && a.type.equalsT.length === 2) {
                return a.type.equalsT(a, b);
            }

            if (b && b.$boxed && b.type.equalsT && b.type.equalsT.length === 2) {
                return b.type.equalsT(b, a);
            }

            if (!HighFive.isDefined(a, true)) {
                throw new System.NullReferenceException();
            } else if (HighFive.isNumber(a) || HighFive.isString(a) || HighFive.isBoolean(a)) {
                return a === b;
            } else if (HighFive.isDate(a)) {
                if (a.kind !== undefined && a.ticks !== undefined) {
                    return System.DateTime.getTicks(a).equals(System.DateTime.getTicks(b));
                }

                return a.valueOf() === b.valueOf();
            }

            var name;

            if (T && a != null && HighFive.isFunction(HighFive.getProperty(a, name = "System$IEquatable$1$" + HighFive.getTypeAlias(T) + "$equalsT"))) {
                return a[name](b);
            }

            if (T && b != null && HighFive.isFunction(HighFive.getProperty(b, name = "System$IEquatable$1$" + HighFive.getTypeAlias(T) + "$equalsT"))) {
                return b[name](a);
            }

            if (HighFive.isFunction(a) && HighFive.isFunction(b)) {
                return HighFive.fn.equals.call(a, b);
            }

            return a.equalsT ? a.equalsT(b) : b.equalsT(a);
        },

        format: function (obj, formatString, provider) {
            if (obj && obj.$boxed) {
                if (obj.type.$kind === "enum") {
                    return System.Enum.format(obj.type, obj.v, formatString);
                } else if (obj.type === System.Char) {
                    return System.Char.format(HighFive.unbox(obj, true), formatString, provider);
                } else if (obj.type.format) {
                    return obj.type.format(HighFive.unbox(obj, true), formatString, provider);
                }
            }

            if (HighFive.isNumber(obj)) {
                return HighFive.Int.format(obj, formatString, provider);
            } else if (HighFive.isDate(obj)) {
                return System.DateTime.format(obj, formatString, provider);
            }

            var name;

            if (HighFive.isFunction(HighFive.getProperty(obj, name = "System$IFormattable$format"))) {
                return obj[name](formatString, provider);
            }

            return obj.format(formatString, provider);
        },

        getType: function (instance, T) {
            if (instance && instance.$boxed) {
                return instance.type;
            }

            if (instance == null) {
                throw new System.NullReferenceException.$ctor1("instance is null");
            }

            if (T) {
                var type = HighFive.getType(instance);
                return HighFive.Reflection.isAssignableFrom(T, type) ? type : T;
            }

            if (typeof (instance) === "number") {
                if (!isNaN(instance) && isFinite(instance) && Math.floor(instance, 0) === instance) {
                    return System.Int32;
                } else {
                    return System.Double;
                }
            }

            if (instance.$type) {
                return instance.$type;
            }

            if (instance.$getType) {
                return instance.$getType();
            }

            var result = null;

            try {
                result = instance.constructor;
            } catch (ex) {
                result = Object;
            }

            if (result === Object) {
                var str = instance.toString(),
                    match = (/\[object (.{1,})\]/).exec(str),
                    name = (match && match.length > 1) ? match[1] : "Object";

                if (name != "Object") {
                    result = instance;
                }
            }

            return HighFive.Reflection.convertType(result);
        },

        isLower: function (c) {
            var s = String.fromCharCode(c);

            return s === s.toLowerCase() && s !== s.toUpperCase();
        },

        isUpper: function (c) {
            var s = String.fromCharCode(c);

            return s !== s.toLowerCase() && s === s.toUpperCase();
        },

        coalesce: function (a, b) {
            return HighFive.hasValue(a) ? a : b;
        },

        fn: {
            equals: function (fn) {
                if (this === fn) {
                    return true;
                }

                if (fn == null || (this.constructor !== fn.constructor)) {
                    return false;
                }

                if (this.$invocationList && fn.$invocationList) {
                    if (this.$invocationList.length !== fn.$invocationList.length) {
                        return false;
                    }

                    for (var i = 0; i < this.$invocationList.length; i++) {
                        if (this.$invocationList[i] !== fn.$invocationList[i]) {
                            return false;
                        }
                    }

                    return true;
                }

                return this.equals && (this.equals === fn.equals) && this.$method && (this.$method === fn.$method) && this.$scope && (this.$scope === fn.$scope);
            },

            call: function (obj, fnName) {
                var args = Array.prototype.slice.call(arguments, 2);

                obj = obj || HighFive.global;

                return obj[fnName].apply(obj, args);
            },

            makeFn: function (fn, length) {
                switch (length) {
                    case 0:
                        return function () {
                            return fn.apply(this, arguments);
                        };
                    case 1:
                        return function (a) {
                            return fn.apply(this, arguments);
                        };
                    case 2:
                        return function (a, b) {
                            return fn.apply(this, arguments);
                        };
                    case 3:
                        return function (a, b, c) {
                            return fn.apply(this, arguments);
                        };
                    case 4:
                        return function (a, b, c, d) {
                            return fn.apply(this, arguments);
                        };
                    case 5:
                        return function (a, b, c, d, e) {
                            return fn.apply(this, arguments);
                        };
                    case 6:
                        return function (a, b, c, d, e, f) {
                            return fn.apply(this, arguments);
                        };
                    case 7:
                        return function (a, b, c, d, e, f, g) {
                            return fn.apply(this, arguments);
                        };
                    case 8:
                        return function (a, b, c, d, e, f, g, h) {
                            return fn.apply(this, arguments);
                        };
                    case 9:
                        return function (a, b, c, d, e, f, g, h, i) {
                            return fn.apply(this, arguments);
                        };
                    case 10:
                        return function (a, b, c, d, e, f, g, h, i, j) {
                            return fn.apply(this, arguments);
                        };
                    case 11:
                        return function (a, b, c, d, e, f, g, h, i, j, k) {
                            return fn.apply(this, arguments);
                        };
                    case 12:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l) {
                            return fn.apply(this, arguments);
                        };
                    case 13:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l, m) {
                            return fn.apply(this, arguments);
                        };
                    case 14:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l, m, n) {
                            return fn.apply(this, arguments);
                        };
                    case 15:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) {
                            return fn.apply(this, arguments);
                        };
                    case 16:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) {
                            return fn.apply(this, arguments);
                        };
                    case 17:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q) {
                            return fn.apply(this, arguments);
                        };
                    case 18:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r) {
                            return fn.apply(this, arguments);
                        };
                    case 19:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s) {
                            return fn.apply(this, arguments);
                        };
                    default:
                        return function (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t) {
                            return fn.apply(this, arguments);
                        };
                }
            },

            cacheBind: function (obj, method, args, appendArgs) {
                return HighFive.fn.bind(obj, method, args, appendArgs, true);
            },

            bind: function (obj, method, args, appendArgs, cache) {
                if (method && method.$method === method && method.$scope === obj) {
                    return method;
                }

                if (obj && cache && obj.$$bind) {
                    for (var i = 0; i < obj.$$bind.length; i++) {
                        if (obj.$$bind[i].$method === method) {
                            return obj.$$bind[i];
                        }
                    }
                }

                var fn;

                if (arguments.length === 2) {
                    fn = HighFive.fn.makeFn(function () {
                        HighFive.caller.unshift(this);

                        var result = null;

                        try {
                            result = method.apply(obj, arguments);
                        } finally {
                            HighFive.caller.shift(this);
                        }

                        return result;
                    }, method.length);
                } else {
                    fn = HighFive.fn.makeFn(function () {
                        var callArgs = args || arguments;

                        if (appendArgs === true) {
                            callArgs = Array.prototype.slice.call(arguments, 0);
                            callArgs = callArgs.concat(args);
                        } else if (typeof appendArgs === "number") {
                            callArgs = Array.prototype.slice.call(arguments, 0);

                            if (appendArgs === 0) {
                                callArgs.unshift.apply(callArgs, args);
                            } else if (appendArgs < callArgs.length) {
                                callArgs.splice.apply(callArgs, [appendArgs, 0].concat(args));
                            } else {
                                callArgs.push.apply(callArgs, args);
                            }
                        }

                        HighFive.caller.unshift(this);

                        var result = null;

                        try {
                            result = method.apply(obj, callArgs);
                        } finally {
                            HighFive.caller.shift(this);
                        }

                        return result;
                    }, method.length);
                }

                if (obj && cache) {
                    obj.$$bind = obj.$$bind || [];
                    obj.$$bind.push(fn);
                }

                fn.$method = method;
                fn.$scope = obj;
                fn.equals = HighFive.fn.equals;

                return fn;
            },

            bindScope: function (obj, method) {
                var fn = HighFive.fn.makeFn(function () {
                    var callArgs = Array.prototype.slice.call(arguments, 0);

                    callArgs.unshift.apply(callArgs, [obj]);

                    HighFive.caller.unshift(this);

                    var result = null;

                    try {
                        result = method.apply(obj, callArgs);
                    } finally {
                        HighFive.caller.shift(this);
                    }

                    return result;
                }, method.length);

                fn.$method = method;
                fn.$scope = obj;
                fn.equals = HighFive.fn.equals;

                return fn;
            },

            $build: function (handlers) {
                if (!handlers || handlers.length === 0) {
                    return null;
                }

                var fn = function () {
                    var result = null,
                        i,
                        handler;

                    for (i = 0; i < handlers.length; i++) {
                        handler = handlers[i];
                        result = handler.apply(null, arguments);
                    }

                    return result;
                };

                fn.$invocationList = handlers ? Array.prototype.slice.call(handlers, 0) : [];
                handlers = fn.$invocationList.slice();

                return fn;
            },

            combine: function (fn1, fn2) {
                if (!fn1 || !fn2) {
                    var fn = fn1 || fn2;

                    return fn ? HighFive.fn.$build([fn]) : fn;
                }

                var list1 = fn1.$invocationList ? fn1.$invocationList : [fn1],
                    list2 = fn2.$invocationList ? fn2.$invocationList : [fn2];

                return HighFive.fn.$build(list1.concat(list2));
            },

            getInvocationList: function (fn) {
                if (fn == null) {
                    throw new System.ArgumentNullException();
                }

                if (!fn.$invocationList) {
                    fn.$invocationList = [fn];
                }

                return fn.$invocationList;
            },

            remove: function (fn1, fn2) {
                if (!fn1 || !fn2) {
                    return fn1 || null;
                }

                var list1 = fn1.$invocationList ? fn1.$invocationList.slice(0) : [fn1],
                    list2 = fn2.$invocationList ? fn2.$invocationList : [fn2],
                    result = [],
                    exclude,
                    i,
                    j;

                exclude = -1;

                for (i = list1.length - list2.length; i >= 0; i--) {
                    if (HighFive.fn.equalInvocationLists(list1, list2, i, list2.length)) {
                        if (list1.length - list2.length == 0) {
                            return null;
                        } else if (list1.length - list2.length == 1) {
                            return list1[i != 0 ? 0 : list1.length - 1];
                        } else {
                            list1.splice(i, list2.length);

                            return HighFive.fn.$build(list1);
                        }
                    }
                }

                return fn1;
            },

            equalInvocationLists: function (a, b, start, count) {
                for (var i = 0; i < count; i = (i + 1) | 0) {
                    if (!(HighFive.equals(a[System.Array.index(((start + i) | 0), a)], b[System.Array.index(i, b)]))) {
                        return false;
                    }
                }

                return true;
            },
        },

        sleep: function (ms, timeout) {
            if (HighFive.hasValue(timeout)) {
                ms = timeout.getTotalMilliseconds();
            }

            if (isNaN(ms) || ms < -1 || ms > 2147483647) {
                throw new System.ArgumentOutOfRangeException.$ctor4("timeout", "Number must be either non-negative and less than or equal to Int32.MaxValue or -1");
            }

            if (ms == -1) {
                ms = 2147483647;
            }

            var start = new Date().getTime();

            while ((new Date().getTime() - start) < ms) {
                if ((new Date().getTime() - start) > 2147483647) {
                    break;
                }
            }
        },

        getMetadata: function (t) {
            var m = t.$getMetadata ? t.$getMetadata() : t.$metadata;

            return m;
        },

        loadModule: function (config, callback) {
            var amd = config.amd,
                cjs = config.cjs,
                fnName = config.fn;

            var tcs = new System.Threading.Tasks.TaskCompletionSource(),
                fn = HighFive.global[fnName || "require"];

            if (amd && amd.length > 0) {
                fn(amd, function () {
                    var loads = Array.prototype.slice.call(arguments, 0);

                    if (cjs && cjs.length > 0) {
                        for (var i = 0; i < cjs.length; i++) {
                            loads.push(fn(cjs[i]));
                        }
                    }

                    callback.apply(HighFive.global, loads);
                    tcs.setResult();
                });
            } else if (cjs && cjs.length > 0) {
                var t = new System.Threading.Tasks.Task();
                t.status = System.Threading.Tasks.TaskStatus.ranToCompletion;

                var loads = [];

                for (var j = 0; j < cjs.length; j++) {
                    loads.push(fn(cjs[j]));
                }

                callback.apply(HighFive.global, loads);

                return t;
            } else {
                var t = new System.Threading.Tasks.Task();
                t.status = System.Threading.Tasks.TaskStatus.ranToCompletion;

                return t;
            }

            return tcs.task;
        }
    };

    if (!globals.setImmediate) {
        core.setImmediate = (function () {
            var head = {},
                tail = head;

            var id = Math.random();

            function onmessage (e) {
                if (e.data != id) {
                    return;
                }

                head = head.next;
                var func = head.func;
                delete head.func;
                func();
            }

            if (typeof window !== "undefined") {
                if (window.addEventListener) {
                    window.addEventListener("message", onmessage);
                } else {
                    window.attachEvent("onmessage", onmessage);
                }
            }

            return function (func) {
                tail = tail.next = { func: func };

                if (typeof window !== "undefined") {
                    window.postMessage(id, "*");
                }
            };
        }());
    } else {
        core.setImmediate = globals.setImmediate.bind(globals);
    }

    globals.HighFive = core;
    globals.HighFive.caller = [];
    globals.HighFive.$equalsGuard = [];
    globals.HighFive.$toStringGuard = [];

    if (globals.console) {
        globals.HighFive.Console = globals.console;
    }

    globals.System = {};
    globals.System.Diagnostics = {};
    globals.System.Diagnostics.Contracts = {};
    globals.System.Threading = {};
