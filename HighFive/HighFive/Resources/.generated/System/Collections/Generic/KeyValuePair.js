    H5.define("System.Collections.Generic.KeyValuePair$2", function (TKey, TValue) { return {
        $kind: "struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.KeyValuePair$2(TKey,TValue))(); }
            }
        },
        fields: {
            key$1: H5.getDefaultValue(TKey),
            value$1: H5.getDefaultValue(TValue)
        },
        props: {
            key: {
                get: function () {
                    return this.key$1;
                }
            },
            value: {
                get: function () {
                    return this.value$1;
                }
            }
        },
        ctors: {
            $ctor1: function (key, value) {
                this.$initialize();
                this.key$1 = key;
                this.value$1 = value;
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            toString: function () {
                var s = System.Text.StringBuilderCache.Acquire();
                s.append(String.fromCharCode(91));
                if (this.key != null) {
                    s.append(H5.toString(this.key));
                }
                s.append(", ");
                if (this.value != null) {
                    s.append(H5.toString(this.value));
                }
                s.append(String.fromCharCode(93));
                return System.Text.StringBuilderCache.GetStringAndRelease(s);
            },
            Deconstruct: function (key, value) {
                key.v = this.key;
                value.v = this.value;
            },
            getHashCode: function () {
                var h = H5.addHash([5072499452, this.key$1, this.value$1]);
                return h;
            },
            equals: function (o) {
                if (!H5.is(o, System.Collections.Generic.KeyValuePair$2(TKey,TValue))) {
                    return false;
                }
                return H5.equals(this.key$1, o.key$1) && H5.equals(this.value$1, o.value$1);
            },
            $clone: function (to) { return this; }
        }
    }; });
