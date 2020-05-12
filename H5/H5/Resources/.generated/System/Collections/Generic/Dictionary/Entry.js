    H5.define("System.Collections.Generic.Dictionary$2.Entry", function (TKey, TValue) { return {
        $kind: "nested struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.Dictionary$2.Entry(TKey,TValue))(); }
            }
        },
        fields: {
            hashCode: 0,
            next: 0,
            key: H5.getDefaultValue(TKey),
            value: H5.getDefaultValue(TValue)
        },
        ctors: {
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            getHashCode: function () {
                var h = H5.addHash([1920233150, this.hashCode, this.next, this.key, this.value]);
                return h;
            },
            equals: function (o) {
                if (!H5.is(o, System.Collections.Generic.Dictionary$2.Entry(TKey,TValue))) {
                    return false;
                }
                return H5.equals(this.hashCode, o.hashCode) && H5.equals(this.next, o.next) && H5.equals(this.key, o.key) && H5.equals(this.value, o.value);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.Generic.Dictionary$2.Entry(TKey,TValue))();
                s.hashCode = this.hashCode;
                s.next = this.next;
                s.key = this.key;
                s.value = this.value;
                return s;
            }
        }
    }; });
