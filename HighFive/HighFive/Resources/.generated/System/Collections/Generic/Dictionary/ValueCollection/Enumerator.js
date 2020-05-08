    HighFive.define("System.Collections.Generic.Dictionary$2.ValueCollection.Enumerator", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IEnumerator$1(TValue),System.Collections.IEnumerator],
        $kind: "nested struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.Dictionary$2.ValueCollection.Enumerator(TKey,TValue))(); }
            }
        },
        fields: {
            dictionary: null,
            index: 0,
            version: 0,
            currentValue: HighFive.getDefaultValue(TValue)
        },
        props: {
            Current: {
                get: function () {
                    return this.currentValue;
                }
            },
            System$Collections$IEnumerator$Current: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this.dictionary.count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return this.currentValue;
                }
            }
        },
        alias: [
            "Dispose", "System$IDisposable$Dispose",
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "Current", ["System$Collections$Generic$IEnumerator$1$" + HighFive.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"]
        ],
        ctors: {
            $ctor1: function (dictionary) {
                this.$initialize();
                this.dictionary = dictionary;
                this.version = dictionary.version;
                this.index = 0;
                this.currentValue = HighFive.getDefaultValue(TValue);
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
                        this.currentValue = ($t1 = this.dictionary.entries)[System.Array.index(this.index, $t1)].value;
                        this.index = (this.index + 1) | 0;
                        return true;
                    }
                    this.index = (this.index + 1) | 0;
                }
                this.index = (this.dictionary.count + 1) | 0;
                this.currentValue = HighFive.getDefaultValue(TValue);
                return false;
            },
            System$Collections$IEnumerator$reset: function () {
                if (this.version !== this.dictionary.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
                this.index = 0;
                this.currentValue = HighFive.getDefaultValue(TValue);
            },
            getHashCode: function () {
                var h = HighFive.addHash([3788985113, this.dictionary, this.index, this.version, this.currentValue]);
                return h;
            },
            equals: function (o) {
                if (!HighFive.is(o, System.Collections.Generic.Dictionary$2.ValueCollection.Enumerator(TKey,TValue))) {
                    return false;
                }
                return HighFive.equals(this.dictionary, o.dictionary) && HighFive.equals(this.index, o.index) && HighFive.equals(this.version, o.version) && HighFive.equals(this.currentValue, o.currentValue);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.Generic.Dictionary$2.ValueCollection.Enumerator(TKey,TValue))();
                s.dictionary = this.dictionary;
                s.index = this.index;
                s.version = this.version;
                s.currentValue = this.currentValue;
                return s;
            }
        }
    }; });
