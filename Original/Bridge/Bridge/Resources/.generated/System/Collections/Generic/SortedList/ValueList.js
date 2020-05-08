    Bridge.define("System.Collections.Generic.SortedList$2.ValueList", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IList$1(TValue),System.Collections.ICollection],
        $kind: "nested class",
        fields: {
            _dict: null
        },
        props: {
            Count: {
                get: function () {
                    return this._dict._size;
                }
            },
            IsReadOnly: {
                get: function () {
                    return true;
                }
            },
            System$Collections$ICollection$IsSynchronized: {
                get: function () {
                    return false;
                }
            },
            System$Collections$ICollection$SyncRoot: {
                get: function () {
                    return Bridge.cast(this._dict, System.Collections.ICollection).System$Collections$ICollection$SyncRoot;
                }
            }
        },
        alias: [
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$Count",
            "IsReadOnly", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$IsReadOnly",
            "add", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$add",
            "clear", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$clear",
            "contains", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$contains",
            "copyTo", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$copyTo",
            "insert", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(TValue) + "$insert",
            "getItem", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(TValue) + "$getItem",
            "setItem", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(TValue) + "$setItem",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$" + Bridge.getTypeAlias(TValue) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"],
            "indexOf", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(TValue) + "$indexOf",
            "remove", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$remove",
            "removeAt", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(TValue) + "$removeAt"
        ],
        ctors: {
            ctor: function (dictionary) {
                this.$initialize();
                this._dict = dictionary;
            }
        },
        methods: {
            getItem: function (index) {
                return this._dict.GetByIndex(index);
            },
            setItem: function (index, value) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
            },
            add: function (key) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
            },
            clear: function () {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
            },
            contains: function (value) {
                return this._dict.ContainsValue(value);
            },
            copyTo: function (array, arrayIndex) {
                System.Array.copy(this._dict.values, 0, array, arrayIndex, this._dict.Count);
            },
            System$Collections$ICollection$copyTo: function (array, arrayIndex) {
                if (array != null && System.Array.getRank(array) !== 1) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_RankMultiDimNotSupported);
                }

                try {
                    System.Array.copy(this._dict.values, 0, array, arrayIndex, this._dict.Count);
                } catch ($e1) {
                    $e1 = System.Exception.create($e1);
                    if (Bridge.is($e1, System.ArrayTypeMismatchException)) {
                        System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Argument_InvalidArrayType);
                    } else {
                        throw $e1;
                    }
                }
            },
            insert: function (index, value) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
            },
            GetEnumerator: function () {
                return new (System.Collections.Generic.SortedList$2.SortedListValueEnumerator(TKey,TValue))(this._dict);
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return new (System.Collections.Generic.SortedList$2.SortedListValueEnumerator(TKey,TValue))(this._dict);
            },
            indexOf: function (value) {
                return System.Array.indexOfT(this._dict.values, value, 0, this._dict.Count);
            },
            remove: function (value) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
                return false;
            },
            removeAt: function (index) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
            }
        }
    }; });
