    H5.define("System.OrdinalComparer", {
        inherits: [System.StringComparer],
        fields: {
            _ignoreCase: false
        },
        alias: [
            "compare", ["System$Collections$Generic$IComparer$1$System$String$compare", "System$Collections$Generic$IComparer$1$compare"],
            "equals2", ["System$Collections$Generic$IEqualityComparer$1$System$String$equals2", "System$Collections$Generic$IEqualityComparer$1$equals2"],
            "getHashCode2", ["System$Collections$Generic$IEqualityComparer$1$System$String$getHashCode2", "System$Collections$Generic$IEqualityComparer$1$getHashCode2"]
        ],
        ctors: {
            ctor: function (ignoreCase) {
                this.$initialize();
                System.StringComparer.ctor.call(this);
                this._ignoreCase = ignoreCase;
            }
        },
        methods: {
            compare: function (x, y) {
                if (H5.referenceEquals(x, y)) {
                    return 0;
                }
                if (x == null) {
                    return -1;
                }
                if (y == null) {
                    return 1;
                }

                if (this._ignoreCase) {
                    return System.String.compare(x, y, 5);
                }

                return System.String.compare(x, y, false);
            },
            equals2: function (x, y) {
                if (H5.referenceEquals(x, y)) {
                    return true;
                }
                if (x == null || y == null) {
                    return false;
                }

                if (this._ignoreCase) {
                    if (x.length !== y.length) {
                        return false;
                    }
                    return (System.String.compare(x, y, 5) === 0);
                }
                return System.String.equals(x, y);
            },
            equals: function (obj) {
                var comparer;
                if (!(((comparer = H5.as(obj, System.OrdinalComparer))) != null)) {
                    return false;
                }
                return (this._ignoreCase === comparer._ignoreCase);
            },
            getHashCode2: function (obj) {
                if (obj == null) {
                    throw new System.ArgumentNullException.$ctor1("obj");
                }

                if (this._ignoreCase && obj != null) {
                    return H5.getHashCode(obj.toLowerCase());
                }

                return H5.getHashCode(obj);
            },
            getHashCode: function () {
                var name = "OrdinalComparer";
                var hashCode = H5.getHashCode(name);
                return this._ignoreCase ? (~hashCode) : hashCode;
            }
        }
    });
