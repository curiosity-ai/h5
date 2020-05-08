    HighFive.define("System.Collections.Generic.KeyValuePair$2", function (TKey, TValue) { return {
        $kind: "struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.KeyValuePair$2(TKey,TValue))(); }
            }
        },
        fields: {
            key$1: HighFive.getDefaultValue(TKey),
            value$1: HighFive.getDefaultValue(TValue)
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
                    s.append(HighFive.toString(this.key));
                }
                s.append(", ");
                if (this.value != null) {
                    s.append(HighFive.toString(this.value));
                }
                s.append(String.fromCharCode(93));
                return System.Text.StringBuilderCache.GetStringAndRelease(s);
            },
            Deconstruct: function (key, value) {
                key.v = this.key;
                value.v = this.value;
            },
            getHashCode: function () {
                var h = HighFive.addHash([5072499452, this.key$1, this.value$1]);
                return h;
            },
            equals: function (o) {
                if (!HighFive.is(o, System.Collections.Generic.KeyValuePair$2(TKey,TValue))) {
                    return false;
                }
                return HighFive.equals(this.key$1, o.key$1) && HighFive.equals(this.value$1, o.value$1);
            },
            $clone: function (to) { return this; }
        }
    }; });
