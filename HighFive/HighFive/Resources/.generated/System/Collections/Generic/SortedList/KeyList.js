    H5.define("System.Collections.Generic.SortedList$2.KeyList", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IList$1(TKey),System.Collections.ICollection],
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
                    return H5.cast(this._dict, System.Collections.ICollection).System$Collections$ICollection$SyncRoot;
                }
            }
        },
        alias: [
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$Count",
            "IsReadOnly", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$IsReadOnly",
            "add", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$add",
            "clear", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$clear",
            "contains", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$contains",
            "copyTo", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$copyTo",
            "insert", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(TKey) + "$insert",
            "getItem", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(TKey) + "$getItem",
            "setItem", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(TKey) + "$setItem",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$" + H5.getTypeAlias(TKey) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"],
            "indexOf", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(TKey) + "$indexOf",
            "remove", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$remove",
            "removeAt", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(TKey) + "$removeAt"
        ],
        ctors: {
            ctor: function (dictionary) {
                this.$initialize();
                this._dict = dictionary;
            }
        },
        methods: {
            getItem: function (index) {
                return this._dict.GetKey(index);
            },
            setItem: function (index, value) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_KeyCollectionSet);
            },
            add: function (key) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
            },
            clear: function () {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
            },
            contains: function (key) {
                return this._dict.containsKey(key);
            },
            copyTo: function (array, arrayIndex) {
                System.Array.copy(this._dict.keys, 0, array, arrayIndex, this._dict.Count);
            },
            System$Collections$ICollection$copyTo: function (array, arrayIndex) {
                if (array != null && System.Array.getRank(array) !== 1) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_RankMultiDimNotSupported);
                }

                try {
                    System.Array.copy(this._dict.keys, 0, array, arrayIndex, this._dict.Count);
                } catch ($e1) {
                    $e1 = System.Exception.create($e1);
                    if (H5.is($e1, System.ArrayTypeMismatchException)) {
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
                return new (System.Collections.Generic.SortedList$2.SortedListKeyEnumerator(TKey,TValue))(this._dict);
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return new (System.Collections.Generic.SortedList$2.SortedListKeyEnumerator(TKey,TValue))(this._dict);
            },
            indexOf: function (key) {
                if (key == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.key);
                }

                var i = System.Array.binarySearch(this._dict.keys, 0, this._dict.Count, key, this._dict.comparer);
                if (i >= 0) {
                    return i;
                }
                return -1;
            },
            remove: function (key) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
                return false;
            },
            removeAt: function (index) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_SortedListNestedWrite);
            }
        }
    }; });
