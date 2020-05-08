    H5.define("System.Collections.Generic.SortedList$2.SortedListValueEnumerator", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IEnumerator$1(TValue),System.Collections.IEnumerator],
        $kind: "nested class",
        fields: {
            _sortedList: null,
            index: 0,
            version: 0,
            currentValue: H5.getDefaultValue(TValue)
        },
        props: {
            Current: {
                get: function () {
                    return this.currentValue;
                }
            },
            System$Collections$IEnumerator$Current: {
                get: function () {
                    if (this.index === 0 || (this.index === ((this._sortedList.Count + 1) | 0))) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return this.currentValue;
                }
            }
        },
        alias: [
            "Dispose", "System$IDisposable$Dispose",
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "Current", ["System$Collections$Generic$IEnumerator$1$" + H5.getTypeAlias(TValue) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"]
        ],
        ctors: {
            ctor: function (sortedList) {
                this.$initialize();
                this._sortedList = sortedList;
                this.version = sortedList.version;
            }
        },
        methods: {
            Dispose: function () {
                this.index = 0;
                this.currentValue = H5.getDefaultValue(TValue);
            },
            moveNext: function () {
                var $t;
                if (this.version !== this._sortedList.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if ((this.index >>> 0) < ((this._sortedList.Count) >>> 0)) {
                    this.currentValue = ($t = this._sortedList.values)[System.Array.index(this.index, $t)];
                    this.index = (this.index + 1) | 0;
                    return true;
                }

                this.index = (this._sortedList.Count + 1) | 0;
                this.currentValue = H5.getDefaultValue(TValue);
                return false;
            },
            System$Collections$IEnumerator$reset: function () {
                if (this.version !== this._sortedList.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
                this.index = 0;
                this.currentValue = H5.getDefaultValue(TValue);
            }
        }
    }; });
