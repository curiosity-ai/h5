    HighFive.define("System.IFormattable", {
        $kind: "interface",
        statics: {
            $is: function (obj) {
                if (HighFive.isNumber(obj) || HighFive.isDate(obj)) {
                    return true;
                }

                return HighFive.is(obj, System.IFormattable, true);
            }
        }
    });

    HighFive.define("System.IComparable", {
        $kind: "interface",

        statics: {
            $is: function (obj) {
                if (HighFive.isNumber(obj) || HighFive.isDate(obj) || HighFive.isBoolean(obj) || HighFive.isString(obj)) {
                    return true;
                }

                return HighFive.is(obj, System.IComparable, true);
            }
        }
    });

    HighFive.define("System.IFormatProvider", {
        $kind: "interface"
    });

    HighFive.define("System.ICloneable", {
        $kind: "interface"
    });

    HighFive.define("System.IComparable$1", function (T) {
        return {
            $kind: "interface",

            statics: {
                $is: function (obj) {
                    if (HighFive.isNumber(obj) && T.$number && T.$is(obj) || HighFive.isDate(obj) && (T === Date || T === System.DateTime) || HighFive.isBoolean(obj) && (T === Boolean || T === System.Boolean) || HighFive.isString(obj) && (T === String || T === System.String)) {
                        return true;
                    }

                    return HighFive.is(obj, System.IComparable$1(T), true);
                },

                isAssignableFrom: function (type) {
                    if (type === System.DateTime && T === Date) {
                        return true;
                    }

                    return HighFive.Reflection.getInterfaces(type).indexOf(System.IComparable$1(T)) >= 0;
                }
            }
        };
    });

    HighFive.define("System.IEquatable$1", function (T) {
        return {
            $kind: "interface",

            statics: {
                $is: function (obj) {
                    if (HighFive.isNumber(obj) && T.$number && T.$is(obj) || HighFive.isDate(obj) && (T === Date || T === System.DateTime) || HighFive.isBoolean(obj) && (T === Boolean || T === System.Boolean) || HighFive.isString(obj) && (T === String || T === System.String)) {
                        return true;
                    }

                    return HighFive.is(obj, System.IEquatable$1(T), true);
                },

                isAssignableFrom: function (type) {
                    if (type === System.DateTime && T === Date) {
                        return true;
                    }

                    return HighFive.Reflection.getInterfaces(type).indexOf(System.IEquatable$1(T)) >= 0;
                }
            }
        };
    });

    HighFive.define("HighFive.IPromise", {
        $kind: "interface"
    });

    HighFive.define("System.IDisposable", {
        $kind: "interface"
    });

    HighFive.define("System.IAsyncResult", {
        $kind: "interface"
    });
