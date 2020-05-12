    H5.define("System.Collections.ObjectModel.ReadOnlyDictionary$2.KeyCollection", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.ICollection$1(TKey),System.Collections.ICollection,System.Collections.Generic.IReadOnlyCollection$1(TKey)],
        $kind: "nested class",
        fields: {
            _collection: null
        },
        props: {
            Count: {
                get: function () {
                    return System.Array.getCount(this._collection, TKey);
                }
            },
            System$Collections$Generic$ICollection$1$IsReadOnly: {
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
                    return null;
                }
            }
        },
        alias: [
            "System$Collections$Generic$ICollection$1$add", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$add",
            "System$Collections$Generic$ICollection$1$clear", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$clear",
            "System$Collections$Generic$ICollection$1$contains", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$contains",
            "copyTo", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$copyTo",
            "Count", ["System$Collections$Generic$IReadOnlyCollection$1$" + H5.getTypeAlias(TKey) + "$Count", "System$Collections$Generic$IReadOnlyCollection$1$Count"],
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$Count",
            "System$Collections$Generic$ICollection$1$IsReadOnly", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$IsReadOnly",
            "System$Collections$Generic$ICollection$1$remove", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(TKey) + "$remove",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$" + H5.getTypeAlias(TKey) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"]
        ],
        ctors: {
            ctor: function (collection) {
                this.$initialize();
                if (collection == null) {
                    throw new System.ArgumentNullException.$ctor1("collection");
                }
                this._collection = collection;
            }
        },
        methods: {
            System$Collections$Generic$ICollection$1$add: function (item) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$Generic$ICollection$1$clear: function () {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$Generic$ICollection$1$contains: function (item) {
                return System.Array.contains(this._collection, item, TKey);
            },
            copyTo: function (array, arrayIndex) {
                System.Array.copyTo(this._collection, array, arrayIndex, TKey);
            },
            System$Collections$ICollection$copyTo: function (array, index) {
                System.Collections.ObjectModel.ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper(TKey, this._collection, array, index);
            },
            System$Collections$Generic$ICollection$1$remove: function (item) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            GetEnumerator: function () {
                return H5.getEnumerator(this._collection, TKey);
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return H5.getEnumerator(H5.cast(this._collection, System.Collections.IEnumerable));
            }
        }
    }; });
