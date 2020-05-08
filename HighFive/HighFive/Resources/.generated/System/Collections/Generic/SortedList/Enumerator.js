    HighFive.define("System.Collections.Generic.SortedList$2.Enumerator", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IEnumerator$1(System.Collections.Generic.KeyValuePair$2(TKey,TValue)),System.Collections.IDictionaryEnumerator],
        $kind: "nested struct",
        statics: {
            fields: {
                KeyValuePair: 0,
                DictEntry: 0
            },
            ctors: {
                init: function () {
                    this.KeyValuePair = 1;
                    this.DictEntry = 2;
                }
            },
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue))(); }
            }
        },
        fields: {
            _sortedList: null,
            key: HighFive.getDefaultValue(TKey),
            value: HighFive.getDefaultValue(TValue),
            index: 0,
            version: 0,
            getEnumeratorRetType: 0
        },
        props: {
            System$Collections$IDictionaryEnumerator$Key: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this._sortedList.Count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return this.key;
                }
            },
            System$Collections$IDictionaryEnumerator$Entry: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this._sortedList.Count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return new System.Collections.DictionaryEntry.$ctor1(this.key, this.value);
                }
            },
            Current: {
                get: function () {
                    return new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(this.key, this.value);
                }
            },
            System$Collections$IEnumerator$Current: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this._sortedList.Count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    if (this.getEnumeratorRetType === System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue).DictEntry) {
                        return new System.Collections.DictionaryEntry.$ctor1(this.key, this.value).$clone();
                    } else {
                        return new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(this.key, this.value);
                    }
                }
            },
            System$Collections$IDictionaryEnumerator$Value: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this._sortedList.Count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return this.value;
                }
            }
        },
        alias: [
            "Dispose", "System$IDisposable$Dispose",
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "Current", ["System$Collections$Generic$IEnumerator$1$System$Collections$Generic$KeyValuePair$2$" + HighFive.getTypeAlias(TKey) + "$" + HighFive.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"]
        ],
        ctors: {
            $ctor1: function (sortedList, getEnumeratorRetType) {
                this.$initialize();
                this._sortedList = sortedList;
                this.index = 0;
                this.version = this._sortedList.version;
                this.getEnumeratorRetType = getEnumeratorRetType;
                this.key = HighFive.getDefaultValue(TKey);
                this.value = HighFive.getDefaultValue(TValue);
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            Dispose: function () {
                this.index = 0;
                this.key = HighFive.getDefaultValue(TKey);
                this.value = HighFive.getDefaultValue(TValue);
            },
            moveNext: function () {
                var $t, $t1;
                if (this.version !== this._sortedList.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if ((this.index >>> 0) < ((this._sortedList.Count) >>> 0)) {
                    this.key = ($t = this._sortedList.keys)[System.Array.index(this.index, $t)];
                    this.value = ($t1 = this._sortedList.values)[System.Array.index(this.index, $t1)];
                    this.index = (this.index + 1) | 0;
                    return true;
                }

                this.index = (this._sortedList.Count + 1) | 0;
                this.key = HighFive.getDefaultValue(TKey);
                this.value = HighFive.getDefaultValue(TValue);
                return false;
            },
            System$Collections$IEnumerator$reset: function () {
                if (this.version !== this._sortedList.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.key = HighFive.getDefaultValue(TKey);
                this.value = HighFive.getDefaultValue(TValue);
            },
            getHashCode: function () {
                var h = HighFive.addHash([3788985113, this._sortedList, this.key, this.value, this.index, this.version, this.getEnumeratorRetType]);
                return h;
            },
            equals: function (o) {
                if (!HighFive.is(o, System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue))) {
                    return false;
                }
                return HighFive.equals(this._sortedList, o._sortedList) && HighFive.equals(this.key, o.key) && HighFive.equals(this.value, o.value) && HighFive.equals(this.index, o.index) && HighFive.equals(this.version, o.version) && HighFive.equals(this.getEnumeratorRetType, o.getEnumeratorRetType);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue))();
                s._sortedList = this._sortedList;
                s.key = this.key;
                s.value = this.value;
                s.index = this.index;
                s.version = this.version;
                s.getEnumeratorRetType = this.getEnumeratorRetType;
                return s;
            }
        }
    }; });
