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

            var guardItem = H5.$toStringGuard[H5.$toStringGuard.length - 1];

            if (instance.toString === Object.prototype.toString || guardItem && guardItem === instance) {
                return H5.Reflection.getTypeFullName(instance);
            }

            H5.$toStringGuard.push(instance);

            var result = instance.toString();

            H5.$toStringGuard.pop();

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
                    return this.fn.getHashCode ? this.fn.getHashCode(this.v) : H5.getHashCode(this.v);
                },
                equals: function (o) {
                    if (this === o) {
                        return true;
                    }

                    var eq = this.equals;
                    this.equals = null;
                    var r = H5.equals(this.v, o);
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

            if (noclone && H5.isFunction(noclone)) {
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

                if (T && T !== t && !H5.isObject(T)) {
                    throw new System.InvalidCastException.$ctor1("Specified cast is not valid.");
                }

                if (!noclone && v && v.$clone) {
                    v = v.$clone();
                }

                return v;
            }

            if (H5.isArray(o)) {
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
            return H5.virtual(name, true);
        },

        virtual: function (name, isClass) {
            var type = H5.unroll(name);

            if (!type || !H5.isFunction(type)) {
                var old = H5.Class.staticInitAllow;
                type = isClass ? H5.define(name) : H5.definei(name);
                H5.Class.staticInitAllow = true;

                if (type.$staticInit) {
                    type.$staticInit();
                }

                H5.Class.staticInitAllow = old;
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
            if (!o || H5.isPlainObject(o) || typeof o != "object") {
                return o;
            }

            if (typeof o.toJSON == "function") {
                return o.toJSON();
            }

            if (H5.isArray(o)) {
                var arr = [];

                for (var i = 0; i < o.length; i++) {
                    arr.push(H5.toPlain(o[i]));
                }

                return arr;
            }

            var newo = {},
                m;

            for (var key in o) {
                m = o[key];

                if (!H5.isFunction(m)) {
                    newo[key] = m;
                }
            }

            return newo;
        },

        ref: function (o, n) {
            if (H5.isArray(n)) {
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
            var scopeType = H5.getType(scope),
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

                H5.property(scope, aliasName, aliasCfg, false, scopeType, true);
            }
            else {
                for (var j = 0; j < descriptors.length; j++) {
                    var d = descriptors[j];

                    if (d.name === name) {
                        var aliasCfg = {},
                            aliasName = "$" + H5.getTypeAlias(d.cls) + "$" + name;

                        if (d.get) {
                            aliasCfg.get = d.get;
                        }

                        if (d.set) {
                            aliasCfg.set = d.set;
                        }

                        H5.property(scope, aliasName, aliasCfg, false, scopeType, true);
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
                var backingField = H5.getTypeAlias(cls) + "$" + name;

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
                    scope[name] = H5.fn.combine(scope[name], value);
                } : function (value) {
                    this[name] = H5.fn.combine(this[name], value);
                };
            })(name, scope, statics);

            scope[removeName] = (function (name, scope, statics) {
                return statics ? function (value) {
                    scope[name] = H5.fn.remove(scope[name], value);
                } : function (value) {
                    this[name] = H5.fn.remove(this[name], value);
                };
            })(name, scope, statics);
        },

        createInstance: function (type, nonPublic, args) {
            if (H5.isArray(nonPublic)) {
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
                type === H5.Int) {
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
                return H5.Reflection.applyConstructor(type, args);
            }

            if (type.$kind === 'interface') {
                throw new System.MissingMethodException.$ctor1('Default constructor not found for type ' + H5.getTypeName(type));
            }

            var ctors = H5.Reflection.getMembers(type, 1, 54);

            if (ctors.length > 0) {
                var pctors = ctors.filter(function (c) { return !c.isSynthetic && !c.sm; });

                for (var idx = 0; idx < pctors.length; idx++) {
                    var c = pctors[idx],
                        isDefault = (c.pi || []).length === 0;

                    if (isDefault) {
                        if (nonPublic || c.a === 2) {
                            return H5.Reflection.invokeCI(c, []);
                        }
                        throw new System.MissingMethodException.$ctor1('Default constructor not found for type ' + H5.getTypeName(type));
                    }
                }

                if (type.$$name && !(ctors.length == 1 && ctors[0].isSynthetic)) {
                    throw new System.MissingMethodException.$ctor1('Default constructor not found for type ' + H5.getTypeName(type));
                }
            }

            return new type();
        },

        clone: function (obj) {
            if (obj == null) {
                return obj;
            }

            if (H5.isArray(obj)) {
                return System.Array.clone(obj);
            }

            if (H5.isString(obj)) {
                return obj;
            }

            var name;

            if (H5.isFunction(H5.getProperty(obj, name = "System$ICloneable$clone"))) {
                return obj[name]();
            }

            if (H5.is(obj, System.ICloneable)) {
                return obj.clone();
            }

            if (H5.isFunction(obj.$clone)) {
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
                    if (H5.is(from[name], System.ICloneable)) {
                        to[name] = H5.clone(from[name]);
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
                scope = H5.global;
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

            if (typeof H5.global.jQuery !== "undefined") {
                H5.global.jQuery(delayfn);
            } else {
                if (typeof H5.global.document === "undefined" ||
                    H5.global.document.readyState === "complete" ||
                    H5.global.document.readyState === "loaded" ||
                    H5.global.document.readyState === "interactive") {
                    delayfn();
                } else {
                    H5.on("DOMContentLoaded", H5.global.document, delayfn);
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
                var ret = fn.call(scope || elem, H5.global.event);

                if (ret === false) {
                    H5.global.event.returnValue = false;
                    H5.global.event.cancelBubble = true;
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

            if (H5.isArray(v)) {
                for (var i = 0; i < v.length; i++) {
                    r = r + ((r * m | 0) + (v[i] == null ? 0 : H5.getHashCode(v[i]))) | 0;
                }

                return r;
            }

            return r = r + ((r * m | 0) + (v == null ? 0 : H5.getHashCode(v))) | 0;
        },

        getHashCode: function (value, safe, deep) {
            // In CLR: mutable object should keep on returning same value
            // H5 goals: make it deterministic (to make testing easier) without breaking CLR contracts
            //     for value types it returns deterministic values (f.e. for int 3 it returns 3)
            //     for reference types it returns random value

            if (value && value.$boxed && value.type.getHashCode) {
                return value.type.getHashCode(H5.unbox(value, true));
            }

            value = H5.unbox(value, true);

            if (H5.isEmpty(value, true)) {
                if (safe) {
                    return 0;
                }

                throw new System.InvalidOperationException.$ctor1("HashCode cannot be calculated for empty value");
            }

            if (value.getHashCode && H5.isFunction(value.getHashCode) && !value.__insideHashCode && value.getHashCode.length === 0) {
                value.__insideHashCode = true;
                var r = value.getHashCode();

                delete value.__insideHashCode;

                return r;
            }

            if (H5.isBoolean(value)) {
                return value ? 1 : 0;
            }

            if (H5.isDate(value)) {
                var val = value.ticks !== undefined ? value.ticks : System.DateTime.getTicks(value);

                return val.toNumber() & 0xFFFFFFFF;
            }

            if (value === Number.POSITIVE_INFINITY) {
                return 0x7FF00000;
            }

            if (value === Number.NEGATIVE_INFINITY) {
                return 0xFFF00000;
            }

            if (H5.isNumber(value)) {
                if (Math.floor(value) === value) {
                    return value;
                }

                value = value.toExponential();
            }

            if (H5.isString(value)) {
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

            if (deep !== false && value.hasOwnProperty("Item1") && H5.isPlainObject(value)) {
                deep = true;
            }

            if (deep && typeof value == "object") {
                var result = 0,
                    temp;

                for (var property in value) {
                    if (value.hasOwnProperty(property)) {
                        temp = H5.isEmpty(value[property], true) ? 0 : H5.getHashCode(value[property]);
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
            } else if (H5.Reflection.isEnum(type)) {
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

            var type = (obj.$$name || typeof obj === "function") ? obj : H5.getType(obj),
                alias;

            if (type.$$alias) {
                return type.$$alias;
            }

            alias = H5.$$aliasCache[type];
            if (alias) {
                return alias;
            }

            if (type.$isArray) {
                var elementName = H5.getTypeAlias(type.$elementType);
                alias = elementName + "$Array" + (type.$rank > 1 ? ("$" + type.$rank) : "");

                if (type.$$name) {
                    type.$$alias = alias;
                } else {
                    H5.$$aliasCache[type] = alias;
                }

                return alias;
            }

            var name = obj.$$name || H5.getTypeName(obj);

            if (type.$typeArguments && !type.$isGenericTypeDefinition) {
                name = type.$genericTypeDefinition.$$name;

                for (var i = 0; i < type.$typeArguments.length; i++) {
                    var ta = type.$typeArguments[i];
                    name += "$" + H5.getTypeAlias(ta);
                }
            }

            alias = name.replace(/[\.\(\)\,\+]/g, "$");

            if (type.$module) {
                alias = type.$module + "$" + alias;
            }

            if (type.$$name) {
                type.$$alias = alias;
            } else {
                H5.$$aliasCache[type] = alias;
            }

            return alias;
        },

        getTypeName: function (obj) {
            return H5.Reflection.getTypeFullName(obj);
        },

        hasValue: function (obj) {
            return H5.unbox(obj, true) != null;
        },

        hasValue$1: function () {
            if (arguments.length === 0) {
                return false;
            }

            var i = 0;

            for (i; i < arguments.length; i++) {
                if (H5.unbox(arguments[i], true) == null) {
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
                } else if (!H5.Reflection.isInterface(type) && !type.$nullable) {
                    return obj.type === type || H5.isObject(type) || type === System.ValueType && H5.Reflection.isValueType(obj.type);
                }

                if (ignoreFn !== true && type.$is) {
                    return type.$is(H5.unbox(obj, true));
                }

                if (H5.Reflection.isAssignableFrom(type, obj.type)) {
                    return true;
                }

                obj = H5.unbox(obj, true);
            }

            var ctor = obj.constructor === Object && obj.$getType ? obj.$getType() : H5.Reflection.convertType(obj.constructor);

            if (type.constructor === Function && obj instanceof type || ctor === type || H5.isObject(type)) {
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

                        return type.isAssignableFrom ? type.isAssignableFrom(ctor) : H5.Reflection.getInterfaces(ctor).indexOf(type) >= 0;
                    }

                    if (H5.isArray(obj, ctor)) {
                        return System.Array.is(obj, type);
                    }
                }

                if (ignoreFn !== true && type.$is) {
                    return type.$is(obj);
                }

                if (type.$literal) {
                    if (H5.isPlainObject(obj)) {
                        if (obj.$getType) {
                            return H5.Reflection.isAssignableFrom(type, obj.$getType());
                        }

                        return true;
                    }
                }

                return false;
            }

            if (tt === "string") {
                type = H5.unroll(type);
            }

            if (tt === "function" && (H5.getType(obj).prototype instanceof type)) {
                return true;
            }

            if (ignoreFn !== true) {
                if (typeof (type.$is) === "function") {
                    return type.$is(obj);
                }

                if (typeof (type.isAssignableFrom) === "function") {
                    return type.isAssignableFrom(H5.getType(obj));
                }
            }

            if (H5.isArray(obj)) {
                return System.Array.is(obj, type);
            }

            return tt === "object" && ((ctor === type) || (obj instanceof type));
        },

        as: function (obj, type, allowNull) {
            if (H5.is(obj, type, false, allowNull)) {
                return obj != null && obj.$boxed && type !== Object && type !== System.Object ? obj.v : obj;
            }
            return null;
        },

        cast: function (obj, type, allowNull) {
            if (obj == null) {
                return obj;
            }

            var result = H5.is(obj, type, false, allowNull) ? obj : null;

            if (result === null) {
                throw new System.InvalidCastException.$ctor1("Unable to cast type " + (obj ? H5.getTypeName(obj) : "'null'") + " to type " + H5.getTypeName(type));
            }

            if (obj.$boxed && type !== Object && type !== System.Object) {
                return obj.v;
            }

            return result;
        },

        apply: function (obj, values, callback) {
            var names = H5.getPropertyNames(values, true),
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
            var names = H5.getPropertyNames(from, false),
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

            // Maps instance of plain JS value or Object into H5 object.
            // Used for deserialization. Proper deserialization requires reflection that is currently not supported in H5.
            // It currently is only capable to deserialize:
            // -instance of single class or primitive
            // -array of primitives
            // -array of single class
            if (to instanceof System.Decimal && typeof from === "number") {
                return new System.Decimal(from);
            }

            if (to instanceof System.Int64 && H5.isNumber(from)) {
                return new System.Int64(from);
            }

            if (to instanceof System.UInt64 && H5.isNumber(from)) {
                return new System.UInt64(from);
            }

            if (to instanceof Boolean || H5.isBoolean(to) ||
                typeof to === "number" ||
                to instanceof String || H5.isString(to) ||
                to instanceof Function || H5.isFunction(to) ||
                to instanceof Date || H5.isDate(to) ||
                H5.getType(to).$number) {
                return from;
            }

            var key,
                i,
                value,
                toValue,
                fn;

            if (H5.isArray(from) && H5.isFunction(to.add || to.push)) {
                fn = H5.isArray(to) ? to.push : to.add;

                for (i = 0; i < from.length; i++) {
                    var item = from[i];

                    if (!H5.isArray(item)) {
                        item = [typeof elemFactory === "undefined" ? item : H5.merge(elemFactory(), item)];
                    }

                    fn.apply(to, item);
                }
            } else {
                var t = H5.getType(to),
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
                                to[key] = H5.merge(to[key], value);
                            } else {
                                H5.merge(to[key], value);
                            }
                        } else if (typeof to[key] === "function") {
                            if (key.match(/^\s*get[A-Z]/)) {
                                H5.merge(to[key](), value);
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
                                    to[setter1](H5.merge(to[getter](), value));
                                } else {
                                    to[setter1](value);
                                }
                            } else if (typeof to[setter2] === "function" && typeof value !== "function") {
                                getter = "g" + setter2.slice(1);

                                if (typeof to[getter] === "function") {
                                    to[setter2](H5.merge(to[getter](), value));
                                } else {
                                    to[setter2](value);
                                }
                            } else if (value && value.constructor === Object && to[key]) {
                                toValue = to[key];
                                H5.merge(toValue, value);
                            } else {
                                var isNumber = H5.isNumber(from);

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

            if (arguments.length === 2 && H5.isFunction(fnName)) {
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

            if (T && H5.isFunction(H5.getProperty(obj, name = "System$Collections$Generic$IEnumerable$1$" + H5.getTypeAlias(T) + "$GetEnumerator"))) {
                return obj[name]();
            }

            if (T && H5.isFunction(H5.getProperty(obj, name = "System$Collections$Generic$IEnumerable$1$GetEnumerator"))) {
                return obj[name]();
            }

            if (H5.isFunction(H5.getProperty(obj, name = "System$Collections$IEnumerable$GetEnumerator"))) {
                return obj[name]();
            }

            if (T && obj && obj.GetEnumerator) {
                return obj.GetEnumerator();
            }

            if ((Object.prototype.toString.call(obj) === "[object Array]") ||
                (obj && H5.isDefined(obj.length))) {
                return new H5.ArrayEnumerator(obj, T);
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
            if (H5.isHtmlAttributeCollection(obj) && !this.isValidHtmlAttributeName(propertyName)) {
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
            return (typeof value === "undefined" || value === null) || (!allowEmpty ? value === "" : false) || ((!allowEmpty && H5.isArray(value)) ? value.length === 0 : false);
        },

        toArray: function (ienumerable) {
            var i,
                item,
                len,
                result = [];

            if (H5.isArray(ienumerable)) {
                for (i = 0, len = ienumerable.length; i < len; ++i) {
                    result.push(ienumerable[i]);
                }
            } else {
                i = H5.getEnumerator(ienumerable);

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

            return H5.arrayTypes.indexOf(c) >= 0 || c.$isArray || Array.isArray(obj);
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
            if (H5.isArray(value)) {
                for (var i = 0; i < value.length; i++) {
                    var v = value[i];

                    if (H5.isString(v)) {
                        value[i] = H5.unroll(v, scope);
                    }
                }

                return;
            }

            var d = value.split("."),
                o = (scope || H5.global)[d[0]],
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
            return H5.hasValue(a) ? a === b : !H5.hasValue(b);
        },

        rE: function (a, b) {
            return H5.hasValue(a) ? a === b : !H5.hasValue(b);
        },

        staticEquals: function (a, b) {
            if (!H5.hasValue(a)) {
                return !H5.hasValue(b);
            }

            return H5.hasValue(b) ? H5.equals(a, b) : false;
        },

        equals: function (a, b) {
            if (a == null && b == null) {
                return true;
            }

            var guardItem = H5.$equalsGuard[H5.$equalsGuard.length - 1];

            if (guardItem && guardItem.a === a && guardItem.b === b) {
                return a === b;
            }

            H5.$equalsGuard.push({a: a, b: b});

            var fn = function (a, b) {
                if (a && a.$boxed && a.type.equals && a.type.equals.length === 2) {
                    return a.type.equals(a, b);
                }

                if (b && b.$boxed && b.type.equals && b.type.equals.length === 2) {
                    return b.type.equals(b, a);
                }

                if (a && H5.isFunction(a.equals) && a.equals.length === 1) {
                    return a.equals(b);
                }

                if (b && H5.isFunction(b.equals) && b.equals.length === 1) {
                    return b.equals(a);
                } if (H5.isFunction(a) && H5.isFunction(b)) {
                    return H5.fn.equals.call(a, b);
                } else if (H5.isDate(a) && H5.isDate(b)) {
                    if (a.kind !== undefined && a.ticks !== undefined && b.kind !== undefined && b.ticks !== undefined) {
                        return a.ticks.equals(b.ticks);
                    }

                    return a.valueOf() === b.valueOf();
                } else if (H5.isNull(a) && H5.isNull(b)) {
                    return true;
                } else if (H5.isNull(a) !== H5.isNull(b)) {
                    return false;
                }

                var eq = a === b;

                if (!eq && typeof a === "object" && typeof b === "object" && a !== null && b !== null && a.$kind === "struct" && b.$kind === "struct" && a.$$name === b.$$name) {
                    return H5.getHashCode(a) === H5.getHashCode(b) && H5.objectEquals(a, b);
                }

                if (!eq && a && b && a.hasOwnProperty("Item1") && H5.isPlainObject(a) && b.hasOwnProperty("Item1") && H5.isPlainObject(b)) {
                    return H5.objectEquals(a, b, true);
                }

                return eq;
            };

            var result = fn(a, b);
            H5.$equalsGuard.pop();

            return result;
        },

        objectEquals: function (a, b, oneLevel) {
            H5.$$leftChain = [];
            H5.$$rightChain = [];

            var result = H5.deepEquals(a, b, oneLevel);

            delete H5.$$leftChain;
            delete H5.$$rightChain;

            return result;
        },

        deepEquals: function (a, b, oneLevel) {
            if (typeof a === "object" && typeof b === "object") {
                if (a === b) {
                    return true;
                }

                if (H5.$$leftChain.indexOf(a) > -1 || H5.$$rightChain.indexOf(b) > -1) {
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
                        H5.$$leftChain.push(a);
                        H5.$$rightChain.push(b);

                        if (!H5.deepEquals(a[p], b[p])) {
                            return false;
                        }

                        H5.$$leftChain.pop();
                        H5.$$rightChain.pop();
                    } else {
                        if (!H5.equals(a[p], b[p])) {
                            return false;
                        }
                    }
                }

                return true;
            } else {
                return H5.equals(a, b);
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
                a = H5.unbox(a, true);
            }

            if (b && b.$boxed) {
                b = H5.unbox(b, true);
            }

            if (typeof a === "number" && typeof b === "number") {
                return H5.numberCompare(a, b);
            }

            if (!H5.isDefined(a, true)) {
                if (safe) {
                    return 0;
                }

                throw new System.NullReferenceException();
            } else if (H5.isString(a)) {
                return System.String.compare(a, b);
            } else if (H5.isNumber(a) || H5.isBoolean(a)) {
                return a < b ? -1 : (a > b ? 1 : 0);
            } else if (H5.isDate(a)) {
                if (a.kind !== undefined && a.ticks !== undefined) {
                    return H5.compare(System.DateTime.getTicks(a), System.DateTime.getTicks(b));
                }

                return H5.compare(a.valueOf(), b.valueOf());
            }

            var name;

            if (T && H5.isFunction(H5.getProperty(a, name = "System$IComparable$1$" + H5.getTypeAlias(T) + "$compareTo"))) {
                return a[name](b);
            }

            if (T && H5.isFunction(H5.getProperty(a, name = "System$IComparable$1$compareTo"))) {
                return a[name](b);
            }

            if (H5.isFunction(H5.getProperty(a, name = "System$IComparable$compareTo"))) {
                return a[name](b);
            }

            if (H5.isFunction(a.compareTo)) {
                return a.compareTo(b);
            }

            if (T && H5.isFunction(H5.getProperty(b, name = "System$IComparable$1$" + H5.getTypeAlias(T) + "$compareTo"))) {
                return -b[name](a);
            }

            if (T && H5.isFunction(H5.getProperty(b, name = "System$IComparable$1$compareTo"))) {
                return -b[name](a);
            }

            if (H5.isFunction(H5.getProperty(b, name = "System$IComparable$compareTo"))) {
                return -b[name](a);
            }

            if (H5.isFunction(b.compareTo)) {
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

            if (!H5.isDefined(a, true)) {
                throw new System.NullReferenceException();
            } else if (H5.isNumber(a) || H5.isString(a) || H5.isBoolean(a)) {
                return a === b;
            } else if (H5.isDate(a)) {
                if (a.kind !== undefined && a.ticks !== undefined) {
                    return System.DateTime.getTicks(a).equals(System.DateTime.getTicks(b));
                }

                return a.valueOf() === b.valueOf();
            }

            var name;

            if (T && a != null && H5.isFunction(H5.getProperty(a, name = "System$IEquatable$1$" + H5.getTypeAlias(T) + "$equalsT"))) {
                return a[name](b);
            }

            if (T && b != null && H5.isFunction(H5.getProperty(b, name = "System$IEquatable$1$" + H5.getTypeAlias(T) + "$equalsT"))) {
                return b[name](a);
            }

            if (H5.isFunction(a) && H5.isFunction(b)) {
                return H5.fn.equals.call(a, b);
            }

            return a.equalsT ? a.equalsT(b) : b.equalsT(a);
        },

        format: function (obj, formatString, provider) {
            if (obj && obj.$boxed) {
                if (obj.type.$kind === "enum") {
                    return System.Enum.format(obj.type, obj.v, formatString);
                } else if (obj.type === System.Char) {
                    return System.Char.format(H5.unbox(obj, true), formatString, provider);
                } else if (obj.type.format) {
                    return obj.type.format(H5.unbox(obj, true), formatString, provider);
                }
            }

            if (H5.isNumber(obj)) {
                return H5.Int.format(obj, formatString, provider);
            } else if (H5.isDate(obj)) {
                return System.DateTime.format(obj, formatString, provider);
            }

            var name;

            if (H5.isFunction(H5.getProperty(obj, name = "System$IFormattable$format"))) {
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
                var type = H5.getType(instance);
                return H5.Reflection.isAssignableFrom(T, type) ? type : T;
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

            return H5.Reflection.convertType(result);
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
            return H5.hasValue(a) ? a : b;
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

                obj = obj || H5.global;

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
                return H5.fn.bind(obj, method, args, appendArgs, true);
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
                    fn = H5.fn.makeFn(function () {
                        H5.caller.unshift(this);

                        var result = null;

                        try {
                            result = method.apply(obj, arguments);
                        } finally {
                            H5.caller.shift(this);
                        }

                        return result;
                    }, method.length);
                } else {
                    fn = H5.fn.makeFn(function () {
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

                        H5.caller.unshift(this);

                        var result = null;

                        try {
                            result = method.apply(obj, callArgs);
                        } finally {
                            H5.caller.shift(this);
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
                fn.equals = H5.fn.equals;

                return fn;
            },

            bindScope: function (obj, method) {
                var fn = H5.fn.makeFn(function () {
                    var callArgs = Array.prototype.slice.call(arguments, 0);

                    callArgs.unshift.apply(callArgs, [obj]);

                    H5.caller.unshift(this);

                    var result = null;

                    try {
                        result = method.apply(obj, callArgs);
                    } finally {
                        H5.caller.shift(this);
                    }

                    return result;
                }, method.length);

                fn.$method = method;
                fn.$scope = obj;
                fn.equals = H5.fn.equals;

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

                    return fn ? H5.fn.$build([fn]) : fn;
                }

                var list1 = fn1.$invocationList ? fn1.$invocationList : [fn1],
                    list2 = fn2.$invocationList ? fn2.$invocationList : [fn2];

                return H5.fn.$build(list1.concat(list2));
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
                    if (H5.fn.equalInvocationLists(list1, list2, i, list2.length)) {
                        if (list1.length - list2.length == 0) {
                            return null;
                        } else if (list1.length - list2.length == 1) {
                            return list1[i != 0 ? 0 : list1.length - 1];
                        } else {
                            list1.splice(i, list2.length);

                            return H5.fn.$build(list1);
                        }
                    }
                }

                return fn1;
            },

            equalInvocationLists: function (a, b, start, count) {
                for (var i = 0; i < count; i = (i + 1) | 0) {
                    if (!(H5.equals(a[System.Array.index(((start + i) | 0), a)], b[System.Array.index(i, b)]))) {
                        return false;
                    }
                }

                return true;
            },
        },

        sleep: function (ms, timeout) {
            if (H5.hasValue(timeout)) {
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
                fn = H5.global[fnName || "require"];

            if (amd && amd.length > 0) {
                fn(amd, function () {
                    var loads = Array.prototype.slice.call(arguments, 0);

                    if (cjs && cjs.length > 0) {
                        for (var i = 0; i < cjs.length; i++) {
                            loads.push(fn(cjs[i]));
                        }
                    }

                    callback.apply(H5.global, loads);
                    tcs.setResult();
                });
            } else if (cjs && cjs.length > 0) {
                var t = new System.Threading.Tasks.Task();
                t.status = System.Threading.Tasks.TaskStatus.ranToCompletion;

                var loads = [];

                for (var j = 0; j < cjs.length; j++) {
                    loads.push(fn(cjs[j]));
                }

                callback.apply(H5.global, loads);

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

    globals.H5 = core;
    globals.H5.caller = [];
    globals.H5.$equalsGuard = [];
    globals.H5.$toStringGuard = [];

    if (globals.console) {
        globals.H5.Console = globals.console;
    }

    globals.System = {};
    globals.System.Diagnostics = {};
    globals.System.Diagnostics.Contracts = {};
    globals.System.Threading = {};
