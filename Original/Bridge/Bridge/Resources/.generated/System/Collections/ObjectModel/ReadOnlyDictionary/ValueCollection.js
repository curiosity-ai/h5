    Bridge.define("System.Collections.ObjectModel.ReadOnlyDictionary$2.ValueCollection", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.ICollection$1(TValue),System.Collections.ICollection,System.Collections.Generic.IReadOnlyCollection$1(TValue)],
        $kind: "nested class",
        fields: {
            _collection: null
        },
        props: {
            Count: {
                get: function () {
                    return System.Array.getCount(this._collection, TValue);
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
            "System$Collections$Generic$ICollection$1$add", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$add",
            "System$Collections$Generic$ICollection$1$clear", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$clear",
            "System$Collections$Generic$ICollection$1$contains", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$contains",
            "copyTo", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$copyTo",
            "Count", ["System$Collections$Generic$IReadOnlyCollection$1$" + Bridge.getTypeAlias(TValue) + "$Count", "System$Collections$Generic$IReadOnlyCollection$1$Count"],
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$Count",
            "System$Collections$Generic$ICollection$1$IsReadOnly", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$IsReadOnly",
            "System$Collections$Generic$ICollection$1$remove", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TValue) + "$remove",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$" + Bridge.getTypeAlias(TValue) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"]
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
                return System.Array.contains(this._collection, item, TValue);
            },
            copyTo: function (array, arrayIndex) {
                System.Array.copyTo(this._collection, array, arrayIndex, TValue);
            },
            System$Collections$ICollection$copyTo: function (array, index) {
                System.Collections.ObjectModel.ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper(TValue, this._collection, array, index);
            },
            System$Collections$Generic$ICollection$1$remove: function (item) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            GetEnumerator: function () {
                return Bridge.getEnumerator(this._collection, TValue);
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return Bridge.getEnumerator(Bridge.cast(this._collection, System.Collections.IEnumerable));
            }
        }
    }; });
