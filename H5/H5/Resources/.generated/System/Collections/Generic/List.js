    H5.define("System.Collections.Generic.List$1", function (T) { return {
        inherits: [System.Collections.Generic.IList$1(T),System.Collections.IList,System.Collections.Generic.IReadOnlyList$1(T)],
        statics: {
            fields: {
                _defaultCapacity: 0,
                _emptyArray: null
            },
            ctors: {
                init: function () {
                    this._defaultCapacity = 4;
                    this._emptyArray = System.Array.init(0, function (){
                        return H5.getDefaultValue(T);
                    }, T);
                }
            },
            methods: {
                IsCompatibleObject: function (value) {
                    return ((H5.is(value, T)) || (value == null && H5.getDefaultValue(T) == null));
                }
            }
        },
        fields: {
            _items: null,
            _size: 0,
            _version: 0
        },
        props: {
            Capacity: {
                get: function () {
                    return this._items.length;
                },
                set: function (value) {
                    if (value < this._size) {
                        throw new System.ArgumentOutOfRangeException.$ctor1("value");
                    }

                    if (value !== this._items.length) {
                        if (value > 0) {
                            var newItems = System.Array.init(value, function (){
                                return H5.getDefaultValue(T);
                            }, T);
                            if (this._size > 0) {
                                System.Array.copy(this._items, 0, newItems, 0, this._size);
                            }
                            this._items = newItems;
                        } else {
                            this._items = System.Collections.Generic.List$1(T)._emptyArray;
                        }
                    }
                }
            },
            Count: {
                get: function () {
                    return this._size;
                }
            },
            System$Collections$IList$IsFixedSize: {
                get: function () {
                    return false;
                }
            },
            System$Collections$Generic$ICollection$1$IsReadOnly: {
                get: function () {
                    return false;
                }
            },
            System$Collections$IList$IsReadOnly: {
                get: function () {
                    return false;
                }
            },
            System$Collections$ICollection$IsSynchronized: {
                get: function () {
                    return false;
                }
            },
            System$Collections$ICollection$SyncRoot: {
                get: function () {
                    return this;
                }
            }
        },
        alias: [
            "Count", ["System$Collections$Generic$IReadOnlyCollection$1$" + H5.getTypeAlias(T) + "$Count", "System$Collections$Generic$IReadOnlyCollection$1$Count"],
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$Count",
            "System$Collections$Generic$ICollection$1$IsReadOnly", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$IsReadOnly",
            "getItem", ["System$Collections$Generic$IReadOnlyList$1$" + H5.getTypeAlias(T) + "$getItem", "System$Collections$Generic$IReadOnlyList$1$getItem"],
            "setItem", ["System$Collections$Generic$IReadOnlyList$1$" + H5.getTypeAlias(T) + "$setItem", "System$Collections$Generic$IReadOnlyList$1$setItem"],
            "getItem", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(T) + "$getItem",
            "setItem", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(T) + "$setItem",
            "add", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$add",
            "clear", "System$Collections$IList$clear",
            "clear", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$clear",
            "contains", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$contains",
            "copyTo", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$copyTo",
            "System$Collections$Generic$IEnumerable$1$GetEnumerator", "System$Collections$Generic$IEnumerable$1$" + H5.getTypeAlias(T) + "$GetEnumerator",
            "indexOf", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(T) + "$indexOf",
            "insert", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(T) + "$insert",
            "remove", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$remove",
            "removeAt", "System$Collections$IList$removeAt",
            "removeAt", "System$Collections$Generic$IList$1$" + H5.getTypeAlias(T) + "$removeAt"
        ],
        ctors: {
            ctor: function () {
                this.$initialize();
                this._items = System.Collections.Generic.List$1(T)._emptyArray;
            },
            $ctor2: function (capacity) {
                this.$initialize();
                if (capacity < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("capacity");
                }

                if (capacity === 0) {
                    this._items = System.Collections.Generic.List$1(T)._emptyArray;
                } else {
                    this._items = System.Array.init(capacity, function (){
                        return H5.getDefaultValue(T);
                    }, T);
                }
            },
            $ctor1: function (collection) {
                this.$initialize();
                if (collection == null) {
                    throw new System.ArgumentNullException.$ctor1("collection");
                }
                var c;
                if (((c = H5.as(collection, System.Collections.Generic.ICollection$1(T)))) != null) {
                    var count = System.Array.getCount(c, T);
                    if (count === 0) {
                        this._items = System.Collections.Generic.List$1(T)._emptyArray;
                    } else {
                        this._items = System.Array.init(count, function (){
                            return H5.getDefaultValue(T);
                        }, T);
                        System.Array.copyTo(c, this._items, 0, T);
                        this._size = count;
                    }
                } else {
                    this._size = 0;
                    this._items = System.Collections.Generic.List$1(T)._emptyArray;

                    var en = H5.getEnumerator(collection, T);
                    try {
                        while (en.System$Collections$IEnumerator$moveNext()) {
                            this.add(en[H5.geti(en, "System$Collections$Generic$IEnumerator$1$" + H5.getTypeAlias(T) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")]);
                        }
                    }
                    finally {
                        if (H5.hasValue(en)) {
                            en.System$IDisposable$Dispose();
                        }
                    }
                }
            }
        },
        methods: {
            getItem: function (index) {
                if ((index >>> 0) >= (this._size >>> 0)) {
                    throw new System.ArgumentOutOfRangeException.ctor();
                }
                return this._items[System.Array.index(index, this._items)];
            },
            setItem: function (index, value) {
                if ((index >>> 0) >= (this._size >>> 0)) {
                    throw new System.ArgumentOutOfRangeException.ctor();
                }
                this._items[System.Array.index(index, this._items)] = value;
                this._version = (this._version + 1) | 0;
            },
            System$Collections$IList$getItem: function (index) {
                return this.getItem(index);
            },
            System$Collections$IList$setItem: function (index, value) {
                if (value == null && !(H5.getDefaultValue(T) == null)) {
                    throw new System.ArgumentNullException.$ctor1("value");
                }

                try {
                    this.setItem(index, H5.cast(H5.unbox(value, T), T));
                } catch ($e1) {
                    $e1 = System.Exception.create($e1);
                    if (H5.is($e1, System.InvalidCastException)) {
                        throw new System.ArgumentException.$ctor1("value");
                    } else {
                        throw $e1;
                    }
                }
            },
            add: function (item) {
                if (this._size === this._items.length) {
                    this.EnsureCapacity(((this._size + 1) | 0));
                }
                this._items[System.Array.index(H5.identity(this._size, ((this._size = (this._size + 1) | 0))), this._items)] = item;
                this._version = (this._version + 1) | 0;
            },
            System$Collections$IList$add: function (item) {
                if (item == null && !(H5.getDefaultValue(T) == null)) {
                    throw new System.ArgumentNullException.$ctor1("item");
                }

                try {
                    this.add(H5.cast(H5.unbox(item, T), T));
                } catch ($e1) {
                    $e1 = System.Exception.create($e1);
                    if (H5.is($e1, System.InvalidCastException)) {
                        throw new System.ArgumentException.$ctor1("item");
                    } else {
                        throw $e1;
                    }
                }

                return ((this.Count - 1) | 0);
            },
            AddRange: function (collection) {
                this.InsertRange(this._size, collection);
            },
            AsReadOnly: function () {
                return new (System.Collections.ObjectModel.ReadOnlyCollection$1(T))(this);
            },
            BinarySearch$2: function (index, count, item, comparer) {
                if (index < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }
                if (count < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }
                if (((this._size - index) | 0) < count) {
                    throw new System.ArgumentException.ctor();
                }

                return System.Array.binarySearch(this._items, index, count, item, comparer);
            },
            BinarySearch: function (item) {
                return this.BinarySearch$2(0, this.Count, item, null);
            },
            BinarySearch$1: function (item, comparer) {
                return this.BinarySearch$2(0, this.Count, item, comparer);
            },
            clear: function () {
                if (this._size > 0) {
                    System.Array.fill(this._items, function () {
                        return H5.getDefaultValue(T);
                    }, 0, this._size);
                    this._size = 0;
                }
                this._version = (this._version + 1) | 0;
            },
            contains: function (item) {
                if (item == null) {
                    for (var i = 0; i < this._size; i = (i + 1) | 0) {
                        if (this._items[System.Array.index(i, this._items)] == null) {
                            return true;
                        }
                    }
                    return false;
                } else {
                    var c = System.Collections.Generic.EqualityComparer$1(T).def;
                    for (var i1 = 0; i1 < this._size; i1 = (i1 + 1) | 0) {
                        if (c.equals2(this._items[System.Array.index(i1, this._items)], item)) {
                            return true;
                        }
                    }
                    return false;
                }
            },
            System$Collections$IList$contains: function (item) {
                if (System.Collections.Generic.List$1(T).IsCompatibleObject(item)) {
                    return this.contains(H5.cast(H5.unbox(item, T), T));
                }
                return false;
            },
            ConvertAll: function (TOutput, converter) {
                if (H5.staticEquals(converter, null)) {
                    throw new System.ArgumentNullException.$ctor1("converter");
                }

                var list = new (System.Collections.Generic.List$1(TOutput)).$ctor2(this._size);
                for (var i = 0; i < this._size; i = (i + 1) | 0) {
                    list._items[System.Array.index(i, list._items)] = converter(this._items[System.Array.index(i, this._items)]);
                }
                list._size = this._size;
                return list;
            },
            CopyTo: function (array) {
                this.copyTo(array, 0);
            },
            System$Collections$ICollection$copyTo: function (array, arrayIndex) {
                if ((array != null) && (System.Array.getRank(array) !== 1)) {
                    throw new System.ArgumentException.$ctor1("array");
                }

                System.Array.copy(this._items, 0, array, arrayIndex, this._size);
            },
            CopyTo$1: function (index, array, arrayIndex, count) {
                if (((this._size - index) | 0) < count) {
                    throw new System.ArgumentException.ctor();
                }

                System.Array.copy(this._items, index, array, arrayIndex, count);
            },
            copyTo: function (array, arrayIndex) {
                System.Array.copy(this._items, 0, array, arrayIndex, this._size);
            },
            EnsureCapacity: function (min) {
                if (this._items.length < min) {
                    var newCapacity = this._items.length === 0 ? System.Collections.Generic.List$1(T)._defaultCapacity : H5.Int.mul(this._items.length, 2);
                    if ((newCapacity >>> 0) > 2146435071) {
                        newCapacity = 2146435071;
                    }
                    if (newCapacity < min) {
                        newCapacity = min;
                    }
                    this.Capacity = newCapacity;
                }
            },
            Exists: function (match) {
                return this.FindIndex$2(match) !== -1;
            },
            Find: function (match) {
                if (H5.staticEquals(match, null)) {
                    throw new System.ArgumentNullException.$ctor1("match");
                }

                for (var i = 0; i < this._size; i = (i + 1) | 0) {
                    if (match(this._items[System.Array.index(i, this._items)])) {
                        return this._items[System.Array.index(i, this._items)];
                    }
                }
                return H5.getDefaultValue(T);
            },
            FindAll: function (match) {
                if (H5.staticEquals(match, null)) {
                    throw new System.ArgumentNullException.$ctor1("match");
                }

                var list = new (System.Collections.Generic.List$1(T)).ctor();
                for (var i = 0; i < this._size; i = (i + 1) | 0) {
                    if (match(this._items[System.Array.index(i, this._items)])) {
                        list.add(this._items[System.Array.index(i, this._items)]);
                    }
                }
                return list;
            },
            FindIndex$2: function (match) {
                return this.FindIndex(0, this._size, match);
            },
            FindIndex$1: function (startIndex, match) {
                return this.FindIndex(startIndex, ((this._size - startIndex) | 0), match);
            },
            FindIndex: function (startIndex, count, match) {
                if ((startIndex >>> 0) > (this._size >>> 0)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("startIndex");
                }

                if (count < 0 || startIndex > ((this._size - count) | 0)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                if (H5.staticEquals(match, null)) {
                    throw new System.ArgumentNullException.$ctor1("match");
                }

                var endIndex = (startIndex + count) | 0;
                for (var i = startIndex; i < endIndex; i = (i + 1) | 0) {
                    if (match(this._items[System.Array.index(i, this._items)])) {
                        return i;
                    }
                }
                return -1;
            },
            FindLast: function (match) {
                if (H5.staticEquals(match, null)) {
                    throw new System.ArgumentNullException.$ctor1("match");
                }

                for (var i = (this._size - 1) | 0; i >= 0; i = (i - 1) | 0) {
                    if (match(this._items[System.Array.index(i, this._items)])) {
                        return this._items[System.Array.index(i, this._items)];
                    }
                }
                return H5.getDefaultValue(T);
            },
            FindLastIndex$2: function (match) {
                return this.FindLastIndex(((this._size - 1) | 0), this._size, match);
            },
            FindLastIndex$1: function (startIndex, match) {
                return this.FindLastIndex(startIndex, ((startIndex + 1) | 0), match);
            },
            FindLastIndex: function (startIndex, count, match) {
                if (H5.staticEquals(match, null)) {
                    throw new System.ArgumentNullException.$ctor1("match");
                }

                if (this._size === 0) {
                    if (startIndex !== -1) {
                        throw new System.ArgumentOutOfRangeException.$ctor1("startIndex");
                    }
                } else {
                    if ((startIndex >>> 0) >= (this._size >>> 0)) {
                        throw new System.ArgumentOutOfRangeException.$ctor1("startIndex");
                    }
                }

                if (count < 0 || ((((startIndex - count) | 0) + 1) | 0) < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                var endIndex = (startIndex - count) | 0;
                for (var i = startIndex; i > endIndex; i = (i - 1) | 0) {
                    if (match(this._items[System.Array.index(i, this._items)])) {
                        return i;
                    }
                }
                return -1;
            },
            ForEach: function (action) {
                if (H5.staticEquals(action, null)) {
                    throw new System.ArgumentNullException.$ctor1("match");
                }

                var version = this._version;

                for (var i = 0; i < this._size; i = (i + 1) | 0) {
                    if (version !== this._version) {
                        break;
                    }
                    action(this._items[System.Array.index(i, this._items)]);
                }

                if (version !== this._version) {
                    throw new System.InvalidOperationException.ctor();
                }
            },
            GetEnumerator: function () {
                return new (System.Collections.Generic.List$1.Enumerator(T)).$ctor1(this);
            },
            System$Collections$Generic$IEnumerable$1$GetEnumerator: function () {
                return new (System.Collections.Generic.List$1.Enumerator(T)).$ctor1(this).$clone();
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return new (System.Collections.Generic.List$1.Enumerator(T)).$ctor1(this).$clone();
            },
            GetRange: function (index, count) {
                if (index < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if (count < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                if (((this._size - index) | 0) < count) {
                    throw new System.ArgumentException.ctor();
                }

                var list = new (System.Collections.Generic.List$1(T)).$ctor2(count);
                System.Array.copy(this._items, index, list._items, 0, count);
                list._size = count;
                return list;
            },
            indexOf: function (item) {
                return System.Array.indexOfT(this._items, item, 0, this._size);
            },
            System$Collections$IList$indexOf: function (item) {
                if (System.Collections.Generic.List$1(T).IsCompatibleObject(item)) {
                    return this.indexOf(H5.cast(H5.unbox(item, T), T));
                }
                return -1;
            },
            IndexOf: function (item, index) {
                if (index > this._size) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }
                return System.Array.indexOfT(this._items, item, index, ((this._size - index) | 0));
            },
            IndexOf$1: function (item, index, count) {
                if (index > this._size) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if (count < 0 || index > ((this._size - count) | 0)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                return System.Array.indexOfT(this._items, item, index, count);
            },
            insert: function (index, item) {
                if ((index >>> 0) > (this._size >>> 0)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }
                if (this._size === this._items.length) {
                    this.EnsureCapacity(((this._size + 1) | 0));
                }
                if (index < this._size) {
                    System.Array.copy(this._items, index, this._items, ((index + 1) | 0), ((this._size - index) | 0));
                }
                this._items[System.Array.index(index, this._items)] = item;
                this._size = (this._size + 1) | 0;
                this._version = (this._version + 1) | 0;
            },
            System$Collections$IList$insert: function (index, item) {
                if (item == null && !(H5.getDefaultValue(T) == null)) {
                    throw new System.ArgumentNullException.$ctor1("item");
                }

                try {
                    this.insert(index, H5.cast(H5.unbox(item, T), T));
                } catch ($e1) {
                    $e1 = System.Exception.create($e1);
                    if (H5.is($e1, System.InvalidCastException)) {
                        throw new System.ArgumentException.$ctor1("item");
                    } else {
                        throw $e1;
                    }
                }
            },
            InsertRange: function (index, collection) {
                if (collection == null) {
                    throw new System.ArgumentNullException.$ctor1("collection");
                }

                if ((index >>> 0) > (this._size >>> 0)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }
                var c;
                if (((c = H5.as(collection, System.Collections.Generic.ICollection$1(T)))) != null) {
                    var count = System.Array.getCount(c, T);
                    if (count > 0) {
                        this.EnsureCapacity(((this._size + count) | 0));
                        if (index < this._size) {
                            System.Array.copy(this._items, index, this._items, ((index + count) | 0), ((this._size - index) | 0));
                        }

                        if (H5.referenceEquals(this, c)) {
                            System.Array.copy(this._items, 0, this._items, index, index);
                            System.Array.copy(this._items, ((index + count) | 0), this._items, H5.Int.mul(index, 2), ((this._size - index) | 0));
                        } else {
                            var itemsToInsert = System.Array.init(count, function (){
                                return H5.getDefaultValue(T);
                            }, T);
                            System.Array.copyTo(c, itemsToInsert, 0, T);
                            System.Array.copy(itemsToInsert, 0, this._items, index, itemsToInsert.length);
                        }
                        this._size = (this._size + count) | 0;
                    }
                } else {
                    var en = H5.getEnumerator(collection, T);
                    try {
                        while (en.System$Collections$IEnumerator$moveNext()) {
                            this.insert(H5.identity(index, ((index = (index + 1) | 0))), en[H5.geti(en, "System$Collections$Generic$IEnumerator$1$" + H5.getTypeAlias(T) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")]);
                        }
                    }
                    finally {
                        if (H5.hasValue(en)) {
                            en.System$IDisposable$Dispose();
                        }
                    }
                }
                this._version = (this._version + 1) | 0;
            },
            LastIndexOf: function (item) {
                if (this._size === 0) {
                    return -1;
                } else {
                    return this.LastIndexOf$2(item, ((this._size - 1) | 0), this._size);
                }
            },
            LastIndexOf$1: function (item, index) {
                if (index >= this._size) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }
                return this.LastIndexOf$2(item, index, ((index + 1) | 0));
            },
            LastIndexOf$2: function (item, index, count) {
                if ((this.Count !== 0) && (index < 0)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if ((this.Count !== 0) && (count < 0)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                if (this._size === 0) {
                    return -1;
                }

                if (index >= this._size) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if (count > ((index + 1) | 0)) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                return System.Array.lastIndexOfT(this._items, item, index, count);
            },
            remove: function (item) {
                var index = this.indexOf(item);
                if (index >= 0) {
                    this.removeAt(index);
                    return true;
                }

                return false;
            },
            System$Collections$IList$remove: function (item) {
                if (System.Collections.Generic.List$1(T).IsCompatibleObject(item)) {
                    this.remove(H5.cast(H5.unbox(item, T), T));
                }
            },
            RemoveAll: function (match) {
                if (H5.staticEquals(match, null)) {
                    throw new System.ArgumentNullException.$ctor1("match");
                }

                var freeIndex = 0;

                while (freeIndex < this._size && !match(this._items[System.Array.index(freeIndex, this._items)])) {
                    freeIndex = (freeIndex + 1) | 0;
                }
                if (freeIndex >= this._size) {
                    return 0;
                }

                var current = (freeIndex + 1) | 0;
                while (current < this._size) {
                    while (current < this._size && match(this._items[System.Array.index(current, this._items)])) {
                        current = (current + 1) | 0;
                    }

                    if (current < this._size) {
                        this._items[System.Array.index(H5.identity(freeIndex, ((freeIndex = (freeIndex + 1) | 0))), this._items)] = this._items[System.Array.index(H5.identity(current, ((current = (current + 1) | 0))), this._items)];
                    }
                }

                System.Array.fill(this._items, function () {
                    return H5.getDefaultValue(T);
                }, freeIndex, ((this._size - freeIndex) | 0));
                var result = (this._size - freeIndex) | 0;
                this._size = freeIndex;
                this._version = (this._version + 1) | 0;
                return result;
            },
            removeAt: function (index) {
                if ((index >>> 0) >= (this._size >>> 0)) {
                    throw new System.ArgumentOutOfRangeException.ctor();
                }
                this._size = (this._size - 1) | 0;
                if (index < this._size) {
                    System.Array.copy(this._items, ((index + 1) | 0), this._items, index, ((this._size - index) | 0));
                }
                this._items[System.Array.index(this._size, this._items)] = H5.getDefaultValue(T);
                this._version = (this._version + 1) | 0;
            },
            RemoveRange: function (index, count) {
                if (index < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if (count < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                if (((this._size - index) | 0) < count) {
                    throw new System.ArgumentException.ctor();
                }

                if (count > 0) {
                    var i = this._size;
                    this._size = (this._size - count) | 0;
                    if (index < this._size) {
                        System.Array.copy(this._items, ((index + count) | 0), this._items, index, ((this._size - index) | 0));
                    }
                    System.Array.fill(this._items, function () {
                        return H5.getDefaultValue(T);
                    }, this._size, count);
                    this._version = (this._version + 1) | 0;
                }
            },
            Reverse: function () {
                this.Reverse$1(0, this.Count);
            },
            Reverse$1: function (index, count) {
                if (index < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if (count < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                if (((this._size - index) | 0) < count) {
                    throw new System.ArgumentException.ctor();
                }
                System.Array.reverse(this._items, index, count);
                this._version = (this._version + 1) | 0;
            },
            Sort: function () {
                this.Sort$3(0, this.Count, null);
            },
            Sort$1: function (comparer) {
                this.Sort$3(0, this.Count, comparer);
            },
            Sort$3: function (index, count, comparer) {
                if (index < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if (count < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                if (((this._size - index) | 0) < count) {
                    throw new System.ArgumentException.ctor();
                }

                System.Array.sort(this._items, index, count, comparer);
                this._version = (this._version + 1) | 0;
            },
            Sort$2: function (comparison) {
                if (H5.staticEquals(comparison, null)) {
                    throw new System.ArgumentNullException.$ctor1("comparison");
                }

                if (this._size > 0) {
                    if (this._items.length === this._size) {
                        System.Array.sort(this._items, comparison);
                    } else {
                        var newItems = System.Array.init(this._size, function (){
                            return H5.getDefaultValue(T);
                        }, T);
                        System.Array.copy(this._items, 0, newItems, 0, this._size);
                        System.Array.sort(newItems, comparison);
                        System.Array.copy(newItems, 0, this._items, 0, this._size);
                    }
                }
            },
            ToArray: function () {

                var array = System.Array.init(this._size, function (){
                    return H5.getDefaultValue(T);
                }, T);
                System.Array.copy(this._items, 0, array, 0, this._size);
                return array;
            },
            TrimExcess: function () {
                var threshold = H5.Int.clip32(this._items.length * 0.9);
                if (this._size < threshold) {
                    this.Capacity = this._size;
                }
            },
            TrueForAll: function (match) {
                if (H5.staticEquals(match, null)) {
                    throw new System.ArgumentNullException.$ctor1("match");
                }

                for (var i = 0; i < this._size; i = (i + 1) | 0) {
                    if (!match(this._items[System.Array.index(i, this._items)])) {
                        return false;
                    }
                }
                return true;
            },
            toJSON: function () {
                var newItems = System.Array.init(this._size, function (){
                    return H5.getDefaultValue(T);
                }, T);
                if (this._size > 0) {
                    System.Array.copy(this._items, 0, newItems, 0, this._size);
                }

                return newItems;
            }
        }
    }; });
