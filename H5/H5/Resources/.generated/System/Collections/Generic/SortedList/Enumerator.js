    H5.define("System.Collections.Generic.SortedList$2.Enumerator", function (TKey, TValue) { return {
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
            key: H5.getDefaultValue(TKey),
            value: H5.getDefaultValue(TValue),
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
            "Current", ["System$Collections$Generic$IEnumerator$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"]
        ],
        ctors: {
            $ctor1: function (sortedList, getEnumeratorRetType) {
                this.$initialize();
                this._sortedList = sortedList;
                this.index = 0;
                this.version = this._sortedList.version;
                this.getEnumeratorRetType = getEnumeratorRetType;
                this.key = H5.getDefaultValue(TKey);
                this.value = H5.getDefaultValue(TValue);
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            Dispose: function () {
                this.index = 0;
                this.key = H5.getDefaultValue(TKey);
                this.value = H5.getDefaultValue(TValue);
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
                this.key = H5.getDefaultValue(TKey);
                this.value = H5.getDefaultValue(TValue);
                return false;
            },
            System$Collections$IEnumerator$reset: function () {
                if (this.version !== this._sortedList.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.key = H5.getDefaultValue(TKey);
                this.value = H5.getDefaultValue(TValue);
            },
            getHashCode: function () {
                var h = H5.addHash([3788985113, this._sortedList, this.key, this.value, this.index, this.version, this.getEnumeratorRetType]);
                return h;
            },
            equals: function (o) {
                if (!H5.is(o, System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue))) {
                    return false;
                }
                return H5.equals(this._sortedList, o._sortedList) && H5.equals(this.key, o.key) && H5.equals(this.value, o.value) && H5.equals(this.index, o.index) && H5.equals(this.version, o.version) && H5.equals(this.getEnumeratorRetType, o.getEnumeratorRetType);
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
