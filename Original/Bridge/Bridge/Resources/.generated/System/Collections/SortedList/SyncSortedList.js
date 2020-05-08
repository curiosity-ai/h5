    Bridge.define("System.Collections.SortedList.SyncSortedList", {
        inherits: [System.Collections.SortedList],
        $kind: "nested class",
        fields: {
            _list: null,
            _root: null
        },
        props: {
            Count: {
                get: function () {
                    this._root;
                    {
                        return this._list.Count;
                    }
                }
            },
            SyncRoot: {
                get: function () {
                    return this._root;
                }
            },
            IsReadOnly: {
                get: function () {
                    return this._list.IsReadOnly;
                }
            },
            IsFixedSize: {
                get: function () {
                    return this._list.IsFixedSize;
                }
            },
            IsSynchronized: {
                get: function () {
                    return true;
                }
            },
            Capacity: {
                get: function () {
                    this._root;
                    {
                        return this._list.Capacity;
                    }
                }
            }
        },
        alias: [
            "Count", "System$Collections$ICollection$Count",
            "SyncRoot", "System$Collections$ICollection$SyncRoot",
            "IsReadOnly", "System$Collections$IDictionary$IsReadOnly",
            "IsFixedSize", "System$Collections$IDictionary$IsFixedSize",
            "IsSynchronized", "System$Collections$ICollection$IsSynchronized",
            "getItem", "System$Collections$IDictionary$getItem",
            "setItem", "System$Collections$IDictionary$setItem",
            "add", "System$Collections$IDictionary$add",
            "clear", "System$Collections$IDictionary$clear",
            "clone", "System$ICloneable$clone",
            "contains", "System$Collections$IDictionary$contains",
            "copyTo", "System$Collections$ICollection$copyTo",
            "GetEnumerator", "System$Collections$IDictionary$GetEnumerator",
            "GetEnumerator", "System$Collections$IEnumerable$GetEnumerator",
            "remove", "System$Collections$IDictionary$remove"
        ],
        ctors: {
            ctor: function (list) {
                this.$initialize();
                System.Collections.SortedList.ctor.call(this);
                this._list = list;
                this._root = list.SyncRoot;
            }
        },
        methods: {
            getItem: function (key) {
                this._root;
                {
                    return this._list.getItem(key);
                }
            },
            setItem: function (key, value) {
                this._root;
                {
                    this._list.setItem(key, value);
                }
            },
            add: function (key, value) {
                this._root;
                {
                    this._list.add(key, value);
                }
            },
            clear: function () {
                this._root;
                {
                    this._list.clear();
                }
            },
            clone: function () {
                this._root;
                {
                    return this._list.clone();
                }
            },
            contains: function (key) {
                this._root;
                {
                    return this._list.contains(key);
                }
            },
            ContainsKey: function (key) {
                this._root;
                {
                    return this._list.ContainsKey(key);
                }
            },
            ContainsValue: function (key) {
                this._root;
                {
                    return this._list.ContainsValue(key);
                }
            },
            copyTo: function (array, index) {
                this._root;
                {
                    this._list.copyTo(array, index);
                }
            },
            GetByIndex: function (index) {
                this._root;
                {
                    return this._list.GetByIndex(index);
                }
            },
            GetEnumerator: function () {
                this._root;
                {
                    return this._list.GetEnumerator();
                }
            },
            GetKey: function (index) {
                this._root;
                {
                    return this._list.GetKey(index);
                }
            },
            GetKeyList: function () {
                this._root;
                {
                    return this._list.GetKeyList();
                }
            },
            GetValueList: function () {
                this._root;
                {
                    return this._list.GetValueList();
                }
            },
            IndexOfKey: function (key) {
                if (key == null) {
                    throw new System.ArgumentNullException.$ctor1("key");
                }

                return this._list.IndexOfKey(key);
            },
            IndexOfValue: function (value) {
                this._root;
                {
                    return this._list.IndexOfValue(value);
                }
            },
            RemoveAt: function (index) {
                this._root;
                {
                    this._list.RemoveAt(index);
                }
            },
            remove: function (key) {
                this._root;
                {
                    this._list.remove(key);
                }
            },
            SetByIndex: function (index, value) {
                this._root;
                {
                    this._list.SetByIndex(index, value);
                }
            },
            ToKeyValuePairsArray: function () {
                return this._list.ToKeyValuePairsArray();
            },
            TrimToSize: function () {
                this._root;
                {
                    this._list.TrimToSize();
                }
            }
        }
    });
