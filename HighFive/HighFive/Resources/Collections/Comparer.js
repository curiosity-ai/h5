    HighFive.define("System.Collections.Generic.Comparer$1", function (T) {
        return {
            inherits: [System.Collections.Generic.IComparer$1(T)],

            ctor: function (fn) {
                this.$initialize();
                this.fn = fn;
                this.compare = fn;
                this["System$Collections$Generic$IComparer$1$" + HighFive.getTypeAlias(T) + "$compare"] = fn;
                this["System$Collections$Generic$IComparer$1$compare"] = fn;
            }
        }
    });

    System.Collections.Generic.Comparer$1.$default = new (System.Collections.Generic.Comparer$1(System.Object))(function (x, y) {
        if (!HighFive.hasValue(x)) {
            return !HighFive.hasValue(y) ? 0 : -1;
        } else if (!HighFive.hasValue(y)) {
            return 1;
        }

        return HighFive.compare(x, y);
    });

    System.Collections.Generic.Comparer$1.get = function (obj, T) {
        var m;

        if (T && (m = obj["System$Collections$Generic$IComparer$1$" + HighFive.getTypeAlias(T) + "$compare"])) {
            return m.bind(obj);
        }

        if (m = obj["System$Collections$Generic$IComparer$1$compare"]) {
            return m.bind(obj);
        }

        return obj.compare.bind(obj);
    };
