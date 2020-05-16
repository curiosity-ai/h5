    H5.define("System.StringComparer", {
        inherits: [System.Collections.Generic.IComparer$1(System.String),System.Collections.Generic.IEqualityComparer$1(System.String)],
        statics: {
            fields: {
                _ordinal: null,
                _ordinalIgnoreCase: null
            },
            props: {
                Ordinal: {
                    get: function () {
                        return System.StringComparer._ordinal;
                    }
                },
                OrdinalIgnoreCase: {
                    get: function () {
                        return System.StringComparer._ordinalIgnoreCase;
                    }
                }
            },
            ctors: {
                init: function () {
                    this._ordinal = new System.OrdinalComparer(false);
                    this._ordinalIgnoreCase = new System.OrdinalComparer(true);
                }
            }
        },
        methods: {
            Compare: function (x, y) {
                if (H5.referenceEquals(x, y)) {
                    return 0;
                }
                if (x == null) {
                    return -1;
                }
                if (y == null) {
                    return 1;
                }
                var sa;
                if (((sa = H5.as(x, System.String))) != null) {
                    var sb;
                    if (((sb = H5.as(y, System.String))) != null) {
                        return this.compare(sa, sb);
                    }
                }
                var ia;
                if (((ia = H5.as(x, System.IComparable))) != null) {
                    return H5.compare(ia, y);
                }

                throw new System.ArgumentException.$ctor1("At least one object must implement IComparable.");
            },
            Equals: function (x, y) {
                if (H5.referenceEquals(x, y)) {
                    return true;
                }
                if (x == null || y == null) {
                    return false;
                }
                var sa;
                if (((sa = H5.as(x, System.String))) != null) {
                    var sb;
                    if (((sb = H5.as(y, System.String))) != null) {
                        return this.equals2(sa, sb);
                    }
                }
                return H5.equals(x, y);
            },
            GetHashCode: function (obj) {
                if (obj == null) {
                    throw new System.ArgumentNullException.$ctor1("obj");
                }
                var s;
                if (((s = H5.as(obj, System.String))) != null) {
                    return this.getHashCode2(s);
                }
                return H5.getHashCode(obj);
            }
        }
    });
