    H5.define("System.Collections.Generic.EqualityComparer$1", function (T) {
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
                    "equals2", ["System$Collections$Generic$IEqualityComparer$1$" + H5.getTypeAlias(T) + "$equals2", "System$Collections$Generic$IEqualityComparer$1$equals2"],
                    "getHashCode2", ["System$Collections$Generic$IEqualityComparer$1$" + H5.getTypeAlias(T) + "$getHashCode2", "System$Collections$Generic$IEqualityComparer$1$getHashCode2"]
                ]
            },

            equals2: function (x, y) {
                if (!H5.isDefined(x, true)) {
                    return !H5.isDefined(y, true);
                } else if (H5.isDefined(y, true)) {
                    var isH5 = x && x.$$name;

                    if (H5.isFunction(x) && H5.isFunction(y)) {
                        return H5.fn.equals.call(x, y);
                    } else if (!isH5 || x && x.$boxed || y && y.$boxed) {
                        return H5.equals(x, y);
                    } else if (H5.isFunction(x.equalsT)) {
                        return H5.equalsT(x, y);
                    } else if (H5.isFunction(x.equals)) {
                        return H5.equals(x, y);
                    }

                    return x === y;
                }

                return false;
            },

            getHashCode2: function (obj) {
                return H5.isDefined(obj, true) ? H5.getHashCode(obj) : 0;
            }
        };
    });

    System.Collections.Generic.EqualityComparer$1.$default = new (System.Collections.Generic.EqualityComparer$1(System.Object))();
