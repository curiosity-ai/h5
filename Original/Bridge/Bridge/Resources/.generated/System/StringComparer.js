    Bridge.define("System.StringComparer", {
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
                if (Bridge.referenceEquals(x, y)) {
                    return 0;
                }
                if (x == null) {
                    return -1;
                }
                if (y == null) {
                    return 1;
                }

                var sa = Bridge.as(x, System.String);
                if (sa != null) {
                    var sb = Bridge.as(y, System.String);
                    if (sb != null) {
                        return this.compare(sa, sb);
                    }
                }

                var ia = Bridge.as(x, System.IComparable);
                if (ia != null) {
                    return Bridge.compare(ia, y);
                }

                throw new System.ArgumentException.$ctor1("At least one object must implement IComparable.");
            },
            Equals: function (x, y) {
                if (Bridge.referenceEquals(x, y)) {
                    return true;
                }
                if (x == null || y == null) {
                    return false;
                }

                var sa = Bridge.as(x, System.String);
                if (sa != null) {
                    var sb = Bridge.as(y, System.String);
                    if (sb != null) {
                        return this.equals2(sa, sb);
                    }
                }
                return Bridge.equals(x, y);
            },
            GetHashCode: function (obj) {
                if (obj == null) {
                    throw new System.ArgumentNullException.$ctor1("obj");
                }

                var s = Bridge.as(obj, System.String);
                if (s != null) {
                    return this.getHashCode2(s);
                }
                return Bridge.getHashCode(obj);
            }
        }
    });
