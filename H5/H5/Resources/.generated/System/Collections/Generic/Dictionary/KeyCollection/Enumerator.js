    H5.define("System.Collections.Generic.Dictionary$2.KeyCollection.Enumerator", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IEnumerator$1(TKey),System.Collections.IEnumerator],
        $kind: "nested struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.Dictionary$2.KeyCollection.Enumerator(TKey,TValue))(); }
            }
        },
        fields: {
            dictionary: null,
            index: 0,
            version: 0,
            currentKey: H5.getDefaultValue(TKey)
        },
        props: {
            Current: {
                get: function () {
                    return this.currentKey;
                }
            },
            System$Collections$IEnumerator$Current: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this.dictionary.count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return this.currentKey;
                }
            }
        },
        alias: [
            "Dispose", "System$IDisposable$Dispose",
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "Current", ["System$Collections$Generic$IEnumerator$1$" + H5.getTypeAlias(TKey) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"]
        ],
        ctors: {
            $ctor1: function (dictionary) {
                this.$initialize();
                this.dictionary = dictionary;
                this.version = dictionary.version;
                this.index = 0;
                this.currentKey = H5.getDefaultValue(TKey);
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            Dispose: function () { },
            moveNext: function () {
                var $t, $t1;
                if (this.version !== this.dictionary.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                while ((this.index >>> 0) < ((this.dictionary.count) >>> 0)) {
                    if (($t = this.dictionary.entries)[System.Array.index(this.index, $t)].hashCode >= 0) {
                        this.currentKey = ($t1 = this.dictionary.entries)[System.Array.index(this.index, $t1)].key;
                        this.index = (this.index + 1) | 0;
                        return true;
                    }
                    this.index = (this.index + 1) | 0;
                }

                this.index = (this.dictionary.count + 1) | 0;
                this.currentKey = H5.getDefaultValue(TKey);
                return false;
            },
            System$Collections$IEnumerator$reset: function () {
                if (this.version !== this.dictionary.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.currentKey = H5.getDefaultValue(TKey);
            },
            getHashCode: function () {
                var h = H5.addHash([3788985113, this.dictionary, this.index, this.version, this.currentKey]);
                return h;
            },
            equals: function (o) {
                if (!H5.is(o, System.Collections.Generic.Dictionary$2.KeyCollection.Enumerator(TKey,TValue))) {
                    return false;
                }
                return H5.equals(this.dictionary, o.dictionary) && H5.equals(this.index, o.index) && H5.equals(this.version, o.version) && H5.equals(this.currentKey, o.currentKey);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.Generic.Dictionary$2.KeyCollection.Enumerator(TKey,TValue))();
                s.dictionary = this.dictionary;
                s.index = this.index;
                s.version = this.version;
                s.currentKey = this.currentKey;
                return s;
            }
        }
    }; });
