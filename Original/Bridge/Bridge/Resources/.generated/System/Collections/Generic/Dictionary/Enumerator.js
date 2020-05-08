    Bridge.define("System.Collections.Generic.Dictionary$2.Enumerator", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IEnumerator$1(System.Collections.Generic.KeyValuePair$2(TKey,TValue)),System.Collections.IDictionaryEnumerator],
        $kind: "nested struct",
        statics: {
            fields: {
                DictEntry: 0,
                KeyValuePair: 0
            },
            ctors: {
                init: function () {
                    this.DictEntry = 1;
                    this.KeyValuePair = 2;
                }
            },
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.Dictionary$2.Enumerator(TKey,TValue))(); }
            }
        },
        fields: {
            dictionary: null,
            version: 0,
            index: 0,
            current: null,
            getEnumeratorRetType: 0
        },
        props: {
            Current: {
                get: function () {
                    return this.current;
                }
            },
            System$Collections$IEnumerator$Current: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this.dictionary.count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    if (this.getEnumeratorRetType === System.Collections.Generic.Dictionary$2.Enumerator(TKey,TValue).DictEntry) {
                        return new System.Collections.DictionaryEntry.$ctor1(this.current.key, this.current.value).$clone();
                    } else {
                        return new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(this.current.key, this.current.value);
                    }
                }
            },
            System$Collections$IDictionaryEnumerator$Entry: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this.dictionary.count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return new System.Collections.DictionaryEntry.$ctor1(this.current.key, this.current.value);
                }
            },
            System$Collections$IDictionaryEnumerator$Key: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this.dictionary.count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return this.current.key;
                }
            },
            System$Collections$IDictionaryEnumerator$Value: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this.dictionary.count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return this.current.value;
                }
            }
        },
        alias: [
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "Current", ["System$Collections$Generic$IEnumerator$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"],
            "Dispose", "System$IDisposable$Dispose"
        ],
        ctors: {
            init: function () {
                this.current = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue))();
            },
            $ctor1: function (dictionary, getEnumeratorRetType) {
                this.$initialize();
                this.dictionary = dictionary;
                this.version = dictionary.version;
                this.index = 0;
                this.getEnumeratorRetType = getEnumeratorRetType;
                this.current = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).ctor();
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            moveNext: function () {
                var $t, $t1, $t2;
                if (this.version !== this.dictionary.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                while ((this.index >>> 0) < ((this.dictionary.count) >>> 0)) {
                    if (($t = this.dictionary.entries)[System.Array.index(this.index, $t)].hashCode >= 0) {
                        this.current = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(($t1 = this.dictionary.entries)[System.Array.index(this.index, $t1)].key, ($t2 = this.dictionary.entries)[System.Array.index(this.index, $t2)].value);
                        this.index = (this.index + 1) | 0;
                        return true;
                    }
                    this.index = (this.index + 1) | 0;
                }

                this.index = (this.dictionary.count + 1) | 0;
                this.current = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).ctor();
                return false;
            },
            Dispose: function () { },
            System$Collections$IEnumerator$reset: function () {
                if (this.version !== this.dictionary.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.current = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).ctor();
            },
            getHashCode: function () {
                var h = Bridge.addHash([3788985113, this.dictionary, this.version, this.index, this.current, this.getEnumeratorRetType]);
                return h;
            },
            equals: function (o) {
                if (!Bridge.is(o, System.Collections.Generic.Dictionary$2.Enumerator(TKey,TValue))) {
                    return false;
                }
                return Bridge.equals(this.dictionary, o.dictionary) && Bridge.equals(this.version, o.version) && Bridge.equals(this.index, o.index) && Bridge.equals(this.current, o.current) && Bridge.equals(this.getEnumeratorRetType, o.getEnumeratorRetType);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.Generic.Dictionary$2.Enumerator(TKey,TValue))();
                s.dictionary = this.dictionary;
                s.version = this.version;
                s.index = this.index;
                s.current = this.current;
                s.getEnumeratorRetType = this.getEnumeratorRetType;
                return s;
            }
        }
    }; });
