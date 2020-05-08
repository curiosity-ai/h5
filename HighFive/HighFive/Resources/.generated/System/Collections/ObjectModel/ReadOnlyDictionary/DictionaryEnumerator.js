    H5.define("System.Collections.ObjectModel.ReadOnlyDictionary$2.DictionaryEnumerator", function (TKey, TValue) { return {
        inherits: [System.Collections.IDictionaryEnumerator],
        $kind: "nested struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Collections.ObjectModel.ReadOnlyDictionary$2.DictionaryEnumerator(TKey,TValue))(); }
            }
        },
        fields: {
            _dictionary: null,
            _enumerator: null
        },
        props: {
            Entry: {
                get: function () {
                    return new System.Collections.DictionaryEntry.$ctor1(this._enumerator[H5.geti(this._enumerator, "System$Collections$Generic$IEnumerator$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")].key, this._enumerator[H5.geti(this._enumerator, "System$Collections$Generic$IEnumerator$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")].value);
                }
            },
            Key: {
                get: function () {
                    return this._enumerator[H5.geti(this._enumerator, "System$Collections$Generic$IEnumerator$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")].key;
                }
            },
            Value: {
                get: function () {
                    return this._enumerator[H5.geti(this._enumerator, "System$Collections$Generic$IEnumerator$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")].value;
                }
            },
            Current: {
                get: function () {
                    return this.Entry.$clone();
                }
            }
        },
        alias: [
            "Entry", "System$Collections$IDictionaryEnumerator$Entry",
            "Key", "System$Collections$IDictionaryEnumerator$Key",
            "Value", "System$Collections$IDictionaryEnumerator$Value",
            "Current", "System$Collections$IEnumerator$Current",
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "reset", "System$Collections$IEnumerator$reset"
        ],
        ctors: {
            $ctor1: function (dictionary) {
                this.$initialize();
                this._dictionary = dictionary;
                this._enumerator = H5.getEnumerator(this._dictionary, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            moveNext: function () {
                return this._enumerator.System$Collections$IEnumerator$moveNext();
            },
            reset: function () {
                this._enumerator.System$Collections$IEnumerator$reset();
            },
            getHashCode: function () {
                var h = H5.addHash([9276503029, this._dictionary, this._enumerator]);
                return h;
            },
            equals: function (o) {
                if (!H5.is(o, System.Collections.ObjectModel.ReadOnlyDictionary$2.DictionaryEnumerator(TKey,TValue))) {
                    return false;
                }
                return H5.equals(this._dictionary, o._dictionary) && H5.equals(this._enumerator, o._enumerator);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.ObjectModel.ReadOnlyDictionary$2.DictionaryEnumerator(TKey,TValue))();
                s._dictionary = this._dictionary;
                s._enumerator = this._enumerator;
                return s;
            }
        }
    }; });
