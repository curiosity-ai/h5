    HighFive.define("System.Collections.Generic.EqualityComparer$1", function (T) {
        return {
            inherits: [System.Collections.Generic.IEqualityComparer$1(T)],

            statics: {
                config: {
                    init: function () {
                        this.def = new (System.Collections.Generic.EqualityComparer$1(T))();
                    }
                }
            },

            config: {
                alias: [
                    "equals2", ["System$Collections$Generic$IEqualityComparer$1$" + HighFive.getTypeAlias(T) + "$equals2", "System$Collections$Generic$IEqualityComparer$1$equals2"],
                    "getHashCode2", ["System$Collections$Generic$IEqualityComparer$1$" + HighFive.getTypeAlias(T) + "$getHashCode2", "System$Collections$Generic$IEqualityComparer$1$getHashCode2"]
                ]
            },

            equals2: function (x, y) {
                if (!HighFive.isDefined(x, true)) {
                    return !HighFive.isDefined(y, true);
                } else if (HighFive.isDefined(y, true)) {
                    var isHighFive = x && x.$$name;

                    if (HighFive.isFunction(x) && HighFive.isFunction(y)) {
                        return HighFive.fn.equals.call(x, y);
                    } else if (!isHighFive || x && x.$boxed || y && y.$boxed) {
                        return HighFive.equals(x, y);
                    } else if (HighFive.isFunction(x.equalsT)) {
                        return HighFive.equalsT(x, y);
                    } else if (HighFive.isFunction(x.equals)) {
                        return HighFive.equals(x, y);
                    }

                    return x === y;
                }

                return false;
            },

            getHashCode2: function (obj) {
                return HighFive.isDefined(obj, true) ? HighFive.getHashCode(obj) : 0;
            }
        };
    });

    System.Collections.Generic.EqualityComparer$1.$default = new (System.Collections.Generic.EqualityComparer$1(System.Object))();
