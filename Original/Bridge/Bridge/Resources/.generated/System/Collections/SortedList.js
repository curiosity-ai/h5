    Bridge.define("System.Collections.SortedList", {
        inherits: [System.Collections.IDictionary,System.ICloneable],
        statics: {
            fields: {
                emptyArray: null
            },
            ctors: {
                init: function () {
                    this.emptyArray = System.Array.init(0, null, System.Object);
                }
            },
            methods: {
                Synchronized: function (list) {
                    if (list == null) {
                        throw new System.ArgumentNullException.$ctor1("list");
                    }

                    return new System.Collections.SortedList.SyncSortedList(list);
                }
            }
        },
        fields: {
            keys: null,
            values: null,
            _size: 0,
            version: 0,
            comparer: null,
            keyList: null,
            valueList: null
        },
        props: {
            Capacity: {
                get: function () {
                    return this.keys.length;
                },
                set: function (value) {
                    if (value < this.Count) {
                        throw new System.ArgumentOutOfRangeException.$ctor1("value");
                    }

                    if (value !== this.keys.length) {
                        if (value > 0) {
                            var newKeys = System.Array.init(value, null, System.Object);
                            var newValues = System.Array.init(value, null, System.Object);
                            if (this._size > 0) {
                                System.Array.copy(this.keys, 0, newKeys, 0, this._size);
                                System.Array.copy(this.values, 0, newValues, 0, this._size);
                            }
                            this.keys = newKeys;
                            this.values = newValues;
                        } else {
                            this.keys = System.Collections.SortedList.emptyArray;
                            this.values = System.Collections.SortedList.emptyArray;
                        }
                    }
                }
            },
            Count: {
                get: function () {
                    return this._size;
                }
            },
            Keys: {
                get: function () {
                    return this.GetKeyList();
                }
            },
            Values: {
                get: function () {
                    return this.GetValueList();
                }
            },
            IsReadOnly: {
                get: function () {
                    return false;
                }
            },
            IsFixedSize: {
                get: function () {
                    return false;
                }
            },
            IsSynchronized: {
                get: function () {
                    return false;
                }
            },
            SyncRoot: {
                get: function () {
                    return null;
                }
            }
        },
        alias: [
            "add", "System$Collections$IDictionary$add",
            "Count", "System$Collections$ICollection$Count",
            "Keys", "System$Collections$IDictionary$Keys",
            "Values", "System$Collections$IDictionary$Values",
            "IsReadOnly", "System$Collections$IDictionary$IsReadOnly",
            "IsFixedSize", "System$Collections$IDictionary$IsFixedSize",
            "IsSynchronized", "System$Collections$ICollection$IsSynchronized",
            "SyncRoot", "System$Collections$ICollection$SyncRoot",
            "clear", "System$Collections$IDictionary$clear",
            "clone", "System$ICloneable$clone",
            "contains", "System$Collections$IDictionary$contains",
            "copyTo", "System$Collections$ICollection$copyTo",
            "GetEnumerator", "System$Collections$IDictionary$GetEnumerator",
            "getItem", "System$Collections$IDictionary$getItem",
            "setItem", "System$Collections$IDictionary$setItem",
            "remove", "System$Collections$IDictionary$remove"
        ],
        ctors: {
            ctor: function () {
                this.$initialize();
                this.Init();
            },
            $ctor5: function (initialCapacity) {
                this.$initialize();
                if (initialCapacity < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("initialCapacity");
                }

                this.keys = System.Array.init(initialCapacity, null, System.Object);
                this.values = System.Array.init(initialCapacity, null, System.Object);
                this.comparer = new (System.Collections.Generic.Comparer$1(Object))(System.Collections.Generic.Comparer$1.$default.fn);
            },
            $ctor1: function (comparer) {
                System.Collections.SortedList.ctor.call(this);
                if (comparer != null) {
                    this.comparer = comparer;
                }
            },
            $ctor2: function (comparer, capacity) {
                System.Collections.SortedList.$ctor1.call(this, comparer);
                this.Capacity = capacity;
            },
            $ctor3: function (d) {
                System.Collections.SortedList.$ctor4.call(this, d, null);
            },
            $ctor4: function (d, comparer) {
                System.Collections.SortedList.$ctor2.call(this, comparer, (d != null ? System.Array.getCount(d) : 0));
                if (d == null) {
                    throw new System.ArgumentNullException.$ctor1("d");
                }

                System.Array.copyTo(d.System$Collections$IDictionary$Keys, this.keys, 0);
                System.Array.copyTo(d.System$Collections$IDictionary$Values, this.values, 0);
                System.Array.sortDict(this.keys, this.values, 0, null, comparer);
                this._size = System.Array.getCount(d);
            }
        },
        methods: {
            getItem: function (key) {
                var i = this.IndexOfKey(key);
                if (i >= 0) {
                    return this.values[System.Array.index(i, this.values)];
                }
                return null;
            },
            setItem: function (key, value) {
                if (key == null) {
                    throw new System.ArgumentNullException.$ctor1("key");
                }

                var i = System.Array.binarySearch(this.keys, 0, this._size, key, this.comparer);
                if (i >= 0) {
                    this.values[System.Array.index(i, this.values)] = value;
                    this.version = (this.version + 1) | 0;
                    return;
                }
                this.Insert(~i, key, value);
            },
            Init: function () {
                this.keys = System.Collections.SortedList.emptyArray;
                this.values = System.Collections.SortedList.emptyArray;
                this._size = 0;
                this.comparer = new (System.Collections.Generic.Comparer$1(Object))(System.Collections.Generic.Comparer$1.$default.fn);
            },
            add: function (key, value) {
                if (key == null) {
                    throw new System.ArgumentNullException.$ctor1("key");
                }

                var i = System.Array.binarySearch(this.keys, 0, this._size, key, this.comparer);
                if (i >= 0) {
                    throw new System.ArgumentException.ctor();
                }
                this.Insert(~i, key, value);
            },
            clear: function () {
                this.version = (this.version + 1) | 0;
                System.Array.fill(this.keys, null, 0, this._size);
                System.Array.fill(this.values, null, 0, this._size);
                this._size = 0;

            },
            clone: function () {
                var sl = new System.Collections.SortedList.$ctor5(this._size);
                System.Array.copy(this.keys, 0, sl.keys, 0, this._size);
                System.Array.copy(this.values, 0, sl.values, 0, this._size);
                sl._size = this._size;
                sl.version = this.version;
                sl.comparer = this.comparer;
                return sl;
            },
            contains: function (key) {
                return this.IndexOfKey(key) >= 0;
            },
            ContainsKey: function (key) {
                return this.IndexOfKey(key) >= 0;
            },
            ContainsValue: function (value) {
                return this.IndexOfValue(value) >= 0;
            },
            copyTo: function (array, arrayIndex) {
                if (array == null) {
                    throw new System.ArgumentNullException.$ctor1("array");
                }
                if (System.Array.getRank(array) !== 1) {
                    throw new System.ArgumentException.ctor();
                }
                if (arrayIndex < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("arrayIndex");
                }
                if (((array.length - arrayIndex) | 0) < this.Count) {
                    throw new System.ArgumentException.ctor();
                }
                for (var i = 0; i < this.Count; i = (i + 1) | 0) {
                    var entry = new System.Collections.DictionaryEntry.$ctor1(this.keys[System.Array.index(i, this.keys)], this.values[System.Array.index(i, this.values)]);
                    System.Array.set(array, entry.$clone(), ((i + arrayIndex) | 0));
                }
            },
            ToKeyValuePairsArray: function () {
                var array = System.Array.init(this.Count, null, System.Collections.KeyValuePairs);
                for (var i = 0; i < this.Count; i = (i + 1) | 0) {
                    array[System.Array.index(i, array)] = new System.Collections.KeyValuePairs(this.keys[System.Array.index(i, this.keys)], this.values[System.Array.index(i, this.values)]);
                }
                return array;
            },
            EnsureCapacity: function (min) {
                var newCapacity = this.keys.length === 0 ? 16 : Bridge.Int.mul(this.keys.length, 2);

                if ((newCapacity >>> 0) > 2146435071) {
                    newCapacity = 2146435071;
                }
                if (newCapacity < min) {
                    newCapacity = min;
                }
                this.Capacity = newCapacity;
            },
            GetByIndex: function (index) {
                if (index < 0 || index >= this.Count) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                return this.values[System.Array.index(index, this.values)];
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return new System.Collections.SortedList.SortedListEnumerator(this, 0, this._size, System.Collections.SortedList.SortedListEnumerator.DictEntry);
            },
            GetEnumerator: function () {
                return new System.Collections.SortedList.SortedListEnumerator(this, 0, this._size, System.Collections.SortedList.SortedListEnumerator.DictEntry);
            },
            GetKey: function (index) {
                if (index < 0 || index >= this.Count) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }
                return this.keys[System.Array.index(index, this.keys)];
            },
            GetKeyList: function () {
                if (this.keyList == null) {
                    this.keyList = new System.Collections.SortedList.KeyList(this);
                }
                return this.keyList;
            },
            GetValueList: function () {
                if (this.valueList == null) {
                    this.valueList = new System.Collections.SortedList.ValueList(this);
                }
                return this.valueList;
            },
            IndexOfKey: function (key) {
                if (key == null) {
                    throw new System.ArgumentNullException.$ctor1("key");
                }

                var ret = System.Array.binarySearch(this.keys, 0, this._size, key, this.comparer);
                return ret >= 0 ? ret : -1;
            },
            IndexOfValue: function (value) {
                return System.Array.indexOfT(this.values, value, 0, this._size);
            },
            Insert: function (index, key, value) {
                if (this._size === this.keys.length) {
                    this.EnsureCapacity(((this._size + 1) | 0));
                }
                if (index < this._size) {
                    System.Array.copy(this.keys, index, this.keys, ((index + 1) | 0), ((this._size - index) | 0));
                    System.Array.copy(this.values, index, this.values, ((index + 1) | 0), ((this._size - index) | 0));
                }
                this.keys[System.Array.index(index, this.keys)] = key;
                this.values[System.Array.index(index, this.values)] = value;
                this._size = (this._size + 1) | 0;
                this.version = (this.version + 1) | 0;
            },
            RemoveAt: function (index) {
                if (index < 0 || index >= this.Count) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                this._size = (this._size - 1) | 0;
                if (index < this._size) {
                    System.Array.copy(this.keys, ((index + 1) | 0), this.keys, index, ((this._size - index) | 0));
                    System.Array.copy(this.values, ((index + 1) | 0), this.values, index, ((this._size - index) | 0));
                }
                this.keys[System.Array.index(this._size, this.keys)] = null;
                this.values[System.Array.index(this._size, this.values)] = null;
                this.version = (this.version + 1) | 0;
            },
            remove: function (key) {
                var i = this.IndexOfKey(key);
                if (i >= 0) {
                    this.RemoveAt(i);
                }
            },
            SetByIndex: function (index, value) {
                if (index < 0 || index >= this.Count) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                this.values[System.Array.index(index, this.values)] = value;
                this.version = (this.version + 1) | 0;
            },
            TrimToSize: function () {
                this.Capacity = this._size;
            }
        }
    });
