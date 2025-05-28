    H5.define("System.IFormattable", {
        $kind: "interface",
        statics: {
            $is: function (obj) {
                if (H5.isNumber(obj) || H5.isDate(obj)) {
                    return true;
                }

                return H5.is(obj, System.IFormattable, true);
            }
        }
    });

    H5.define("System.IComparable", {
        $kind: "interface",

        statics: {
            $is: function (obj) {
                if (H5.isNumber(obj) || H5.isDate(obj) || H5.isBoolean(obj) || H5.isString(obj)) {
                    return true;
                }

                return H5.is(obj, System.IComparable, true);
            }
        }
    });

    H5.define("System.IFormatProvider", {
        $kind: "interface"
    });

    H5.define("System.ICloneable", {
        $kind: "interface"
    });

    H5.define("System.IComparable$1", function (T) {
        return {
            $kind: "interface",

            statics: {
                $is: function (obj) {
                    if (H5.isNumber(obj) && T.$number && T.$is(obj) || H5.isDate(obj) && (T === Date || T === System.DateTime) || H5.isBoolean(obj) && (T === Boolean || T === System.Boolean) || H5.isString(obj) && (T === String || T === System.String)) {
                        return true;
                    }

                    return H5.is(obj, System.IComparable$1(T), true);
                },

                isAssignableFrom: function (type) {
                    if (type === System.DateTime && T === Date) {
                        return true;
                    }

                    return H5.Reflection.getInterfaces(type).indexOf(System.IComparable$1(T)) >= 0;
                }
            }
        };
    });

    H5.define("System.IEquatable$1", function (T) {
        return {
            $kind: "interface",

            statics: {
                $is: function (obj) {
                    if (H5.isNumber(obj) && T.$number && T.$is(obj) || H5.isDate(obj) && (T === Date || T === System.DateTime) || H5.isBoolean(obj) && (T === Boolean || T === System.Boolean) || H5.isString(obj) && (T === String || T === System.String)) {
                        return true;
                    }

                    return H5.is(obj, System.IEquatable$1(T), true);
                },

                isAssignableFrom: function (type) {
                    if (type === System.DateTime && T === Date) {
                        return true;
                    }

                    return H5.Reflection.getInterfaces(type).indexOf(System.IEquatable$1(T)) >= 0;
                }
            }
        };
    });

    H5.define("H5.IPromise", {
        $kind: "interface"
    });

    H5.define("System.IDisposable", {
        $kind: "interface"
    });

    H5.define("System.IAsyncResult", {
        $kind: "interface"
    });

    H5.define("System.IServiceProvider", {
        $kind: "interface"
    });
