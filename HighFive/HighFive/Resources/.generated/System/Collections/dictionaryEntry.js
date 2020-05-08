    H5.define("System.Collections.DictionaryEntry", {
        $kind: "struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new System.Collections.DictionaryEntry(); }
            }
        },
        fields: {
            _key: null,
            _value: null
        },
        props: {
            Key: {
                get: function () {
                    return this._key;
                },
                set: function (value) {
                    this._key = value;
                }
            },
            Value: {
                get: function () {
                    return this._value;
                },
                set: function (value) {
                    this._value = value;
                }
            }
        },
        ctors: {
            $ctor1: function (key, value) {
                this.$initialize();
                this._key = key;
                this._value = value;
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            getHashCode: function () {
                var h = H5.addHash([5445305491, this._key, this._value]);
                return h;
            },
            equals: function (o) {
                if (!H5.is(o, System.Collections.DictionaryEntry)) {
                    return false;
                }
                return H5.equals(this._key, o._key) && H5.equals(this._value, o._value);
            },
            $clone: function (to) {
                var s = to || new System.Collections.DictionaryEntry();
                s._key = this._key;
                s._value = this._value;
                return s;
            }
        }
    });
