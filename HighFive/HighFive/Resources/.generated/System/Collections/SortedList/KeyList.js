    H5.define("System.Collections.SortedList.KeyList", {
        inherits: [System.Collections.IList],
        $kind: "nested class",
        fields: {
            sortedList: null
        },
        props: {
            Count: {
                get: function () {
                    return this.sortedList._size;
                }
            },
            IsReadOnly: {
                get: function () {
                    return true;
                }
            },
            IsFixedSize: {
                get: function () {
                    return true;
                }
            },
            IsSynchronized: {
                get: function () {
                    return this.sortedList.IsSynchronized;
                }
            },
            SyncRoot: {
                get: function () {
                    return this.sortedList.SyncRoot;
                }
            }
        },
        alias: [
            "Count", "System$Collections$ICollection$Count",
            "IsReadOnly", "System$Collections$IList$IsReadOnly",
            "IsFixedSize", "System$Collections$IList$IsFixedSize",
            "IsSynchronized", "System$Collections$ICollection$IsSynchronized",
            "SyncRoot", "System$Collections$ICollection$SyncRoot",
            "add", "System$Collections$IList$add",
            "clear", "System$Collections$IList$clear",
            "contains", "System$Collections$IList$contains",
            "copyTo", "System$Collections$ICollection$copyTo",
            "insert", "System$Collections$IList$insert",
            "getItem", "System$Collections$IList$getItem",
            "setItem", "System$Collections$IList$setItem",
            "GetEnumerator", "System$Collections$IEnumerable$GetEnumerator",
            "indexOf", "System$Collections$IList$indexOf",
            "remove", "System$Collections$IList$remove",
            "removeAt", "System$Collections$IList$removeAt"
        ],
        ctors: {
            ctor: function (sortedList) {
                this.$initialize();
                this.sortedList = sortedList;
            }
        },
        methods: {
            getItem: function (index) {
                return this.sortedList.GetKey(index);
            },
            setItem: function (index, value) {
                throw new System.NotSupportedException.ctor();
            },
            add: function (key) {
                throw new System.NotSupportedException.ctor();
            },
            clear: function () {
                throw new System.NotSupportedException.ctor();
            },
            contains: function (key) {
                return this.sortedList.contains(key);
            },
            copyTo: function (array, arrayIndex) {
                if (array != null && System.Array.getRank(array) !== 1) {
                    throw new System.ArgumentException.ctor();
                }

                System.Array.copy(this.sortedList.keys, 0, array, arrayIndex, this.sortedList.Count);
            },
            insert: function (index, value) {
                throw new System.NotSupportedException.ctor();
            },
            GetEnumerator: function () {
                return new System.Collections.SortedList.SortedListEnumerator(this.sortedList, 0, this.sortedList.Count, System.Collections.SortedList.SortedListEnumerator.Keys);
            },
            indexOf: function (key) {
                if (key == null) {
                    throw new System.ArgumentNullException.$ctor1("key");
                }

                var i = System.Array.binarySearch(this.sortedList.keys, 0, this.sortedList.Count, key, this.sortedList.comparer);
                if (i >= 0) {
                    return i;
                }
                return -1;
            },
            remove: function (key) {
                throw new System.NotSupportedException.ctor();
            },
            removeAt: function (index) {
                throw new System.NotSupportedException.ctor();
            }
        }
    });
