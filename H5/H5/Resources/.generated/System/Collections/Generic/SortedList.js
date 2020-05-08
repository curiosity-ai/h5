    H5.define("System.Collections.Generic.SortedList$2", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IDictionary$2(TKey,TValue),System.Collections.IDictionary,System.Collections.Generic.IReadOnlyDictionary$2(TKey,TValue)],
        statics: {
            fields: {
                _defaultCapacity: 0,
                MaxArrayLength: 0,
                emptyKeys: null,
                emptyValues: null
            },
            ctors: {
                init: function () {
                    this._defaultCapacity = 4;
                    this.MaxArrayLength = 2146435071;
                    this.emptyKeys = System.Array.init(0, function (){
                        return H5.getDefaultValue(TKey);
                    }, TKey);
                    this.emptyValues = System.Array.init(0, function (){
                        return H5.getDefaultValue(TValue);
                    }, TValue);
                }
            },
            methods: {
                IsCompatibleKey: function (key) {
                    if (key == null) {
                        System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.key);
                    }

                    return (H5.is(key, TKey));
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
                    if (value !== this.keys.length) {
                        if (value < this._size) {
                            System.ThrowHelper.ThrowArgumentOutOfRangeException$2(System.ExceptionArgument.value, System.ExceptionResource.ArgumentOutOfRange_SmallCapacity);
                        }

                        if (value > 0) {
                            var newKeys = System.Array.init(value, function (){
                                return H5.getDefaultValue(TKey);
                            }, TKey);
                            var newValues = System.Array.init(value, function (){
                                return H5.getDefaultValue(TValue);
                            }, TValue);
                            if (this._size > 0) {
                                System.Array.copy(this.keys, 0, newKeys, 0, this._size);
                                System.Array.copy(this.values, 0, newValues, 0, this._size);
                            }
                            this.keys = newKeys;
                            this.values = newValues;
                        } else {
                            this.keys = System.Collections.Generic.SortedList$2(TKey,TValue).emptyKeys;
                            this.values = System.Collections.Generic.SortedList$2(TKey,TValue).emptyValues;
                        }
                    }
                }
            },
            Comparer: {
                get: function () {
                    return this.comparer;
                }
            },
            Count: {
                get: function () {
                    return this._size;
                }
            },
            Keys: {
                get: function () {
                    return this.GetKeyListHelper();
                }
            },
            System$Collections$Generic$IDictionary$2$Keys: {
                get: function () {
                    return this.GetKeyListHelper();
                }
            },
            System$Collections$IDictionary$Keys: {
                get: function () {
                    return this.GetKeyListHelper();
                }
            },
            System$Collections$Generic$IReadOnlyDictionary$2$Keys: {
                get: function () {
                    return this.GetKeyListHelper();
                }
            },
            Values: {
                get: function () {
                    return this.GetValueListHelper();
                }
            },
            System$Collections$Generic$IDictionary$2$Values: {
                get: function () {
                    return this.GetValueListHelper();
                }
            },
            System$Collections$IDictionary$Values: {
                get: function () {
                    return this.GetValueListHelper();
                }
            },
            System$Collections$Generic$IReadOnlyDictionary$2$Values: {
                get: function () {
                    return this.GetValueListHelper();
                }
            },
            System$Collections$Generic$ICollection$1$IsReadOnly: {
                get: function () {
                    return false;
                }
            },
            System$Collections$IDictionary$IsReadOnly: {
                get: function () {
                    return false;
                }
            },
            System$Collections$IDictionary$IsFixedSize: {
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
                    return null;
                }
            }
        },
        alias: [
            "add", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$add",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$add", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$add",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$contains", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$contains",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$remove", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$remove",
            "Count", ["System$Collections$Generic$IReadOnlyCollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Count", "System$Collections$Generic$IReadOnlyCollection$1$Count"],
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Count",
            "System$Collections$Generic$IDictionary$2$Keys", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Keys",
            "System$Collections$Generic$IReadOnlyDictionary$2$Keys", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Keys",
            "System$Collections$Generic$IDictionary$2$Values", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Values",
            "System$Collections$Generic$IReadOnlyDictionary$2$Values", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Values",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$IsReadOnly", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$IsReadOnly",
            "clear", "System$Collections$IDictionary$clear",
            "clear", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$clear",
            "containsKey", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$containsKey",
            "containsKey", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$containsKey",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$copyTo", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$copyTo",
            "System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$GetEnumerator", "System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$GetEnumerator",
            "getItem", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$getItem",
            "setItem", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$setItem",
            "getItem", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$getItem",
            "setItem", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$setItem",
            "getItem$1", "System$Collections$IDictionary$getItem",
            "setItem$1", "System$Collections$IDictionary$setItem",
            "tryGetValue", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$tryGetValue",
            "tryGetValue", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$tryGetValue",
            "remove", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$remove"
        ],
        ctors: {
            ctor: function () {
                this.$initialize();
                this.keys = System.Collections.Generic.SortedList$2(TKey,TValue).emptyKeys;
                this.values = System.Collections.Generic.SortedList$2(TKey,TValue).emptyValues;
                this._size = 0;
                this.comparer = new (System.Collections.Generic.Comparer$1(TKey))(System.Collections.Generic.Comparer$1.$default.fn);
            },
            $ctor4: function (capacity) {
                this.$initialize();
                if (capacity < 0) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$1(System.ExceptionArgument.capacity);
                }
                this.keys = System.Array.init(capacity, function (){
                    return H5.getDefaultValue(TKey);
                }, TKey);
                this.values = System.Array.init(capacity, function (){
                    return H5.getDefaultValue(TValue);
                }, TValue);
                this.comparer = new (System.Collections.Generic.Comparer$1(TKey))(System.Collections.Generic.Comparer$1.$default.fn);
            },
            $ctor1: function (comparer) {
                System.Collections.Generic.SortedList$2(TKey,TValue).ctor.call(this);
                if (comparer != null) {
                    this.comparer = comparer;
                }
            },
            $ctor5: function (capacity, comparer) {
                System.Collections.Generic.SortedList$2(TKey,TValue).$ctor1.call(this, comparer);
                this.Capacity = capacity;
            },
            $ctor2: function (dictionary) {
                System.Collections.Generic.SortedList$2(TKey,TValue).$ctor3.call(this, dictionary, null);
            },
            $ctor3: function (dictionary, comparer) {
                System.Collections.Generic.SortedList$2(TKey,TValue).$ctor5.call(this, (dictionary != null ? System.Array.getCount(dictionary, System.Collections.Generic.KeyValuePair$2(TKey,TValue)) : 0), comparer);
                if (dictionary == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.dictionary);
                }

                System.Array.copyTo(dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Keys"], this.keys, 0, TKey);
                System.Array.copyTo(dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Values"], this.values, 0, TValue);
                System.Array.sortDict(this.keys, this.values, 0, null, this.comparer);
                this._size = System.Array.getCount(dictionary, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
            }
        },
        methods: {
            getItem: function (key) {
                var i = this.IndexOfKey(key);
                if (i >= 0) {
                    return this.values[System.Array.index(i, this.values)];
                }

                throw new System.Collections.Generic.KeyNotFoundException.ctor();
            },
            setItem: function (key, value) {
                if (key == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.key);
                }
                var i = System.Array.binarySearch(this.keys, 0, this._size, key, this.comparer);
                if (i >= 0) {
                    this.values[System.Array.index(i, this.values)] = value;
                    this.version = (this.version + 1) | 0;
                    return;
                }
                this.Insert(~i, key, value);
            },
            getItem$1: function (key) {
                if (System.Collections.Generic.SortedList$2(TKey,TValue).IsCompatibleKey(key)) {
                    var i = this.IndexOfKey(H5.cast(H5.unbox(key, TKey), TKey));
                    if (i >= 0) {
                        return this.values[System.Array.index(i, this.values)];
                    }
                }

                return null;
            },
            setItem$1: function (key, value) {
                if (!System.Collections.Generic.SortedList$2(TKey,TValue).IsCompatibleKey(key)) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.key);
                }

                System.ThrowHelper.IfNullAndNullsAreIllegalThenThrow(TValue, value, System.ExceptionArgument.value);

                try {
                    var tempKey = H5.cast(H5.unbox(key, TKey), TKey);
                    try {
                        this.setItem(tempKey, H5.cast(H5.unbox(value, TValue), TValue));
                    } catch ($e1) {
                        $e1 = System.Exception.create($e1);
                        if (H5.is($e1, System.InvalidCastException)) {
                            System.ThrowHelper.ThrowWrongValueTypeArgumentException(System.Object, value, TValue);
                        } else {
                            throw $e1;
                        }
                    }
                } catch ($e2) {
                    $e2 = System.Exception.create($e2);
                    if (H5.is($e2, System.InvalidCastException)) {
                        System.ThrowHelper.ThrowWrongKeyTypeArgumentException(System.Object, key, TKey);
                    } else {
                        throw $e2;
                    }
                }
            },
            add: function (key, value) {
                if (key == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.key);
                }
                var i = System.Array.binarySearch(this.keys, 0, this._size, key, this.comparer);
                if (i >= 0) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Argument_AddingDuplicate);
                }
                this.Insert(~i, key, value);
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$add: function (keyValuePair) {
                this.add(keyValuePair.key, keyValuePair.value);
            },
            System$Collections$IDictionary$add: function (key, value) {
                if (key == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.key);
                }

                System.ThrowHelper.IfNullAndNullsAreIllegalThenThrow(TValue, value, System.ExceptionArgument.value);

                try {
                    var tempKey = H5.cast(H5.unbox(key, TKey), TKey);

                    try {
                        this.add(tempKey, H5.cast(H5.unbox(value, TValue), TValue));
                    } catch ($e1) {
                        $e1 = System.Exception.create($e1);
                        if (H5.is($e1, System.InvalidCastException)) {
                            System.ThrowHelper.ThrowWrongValueTypeArgumentException(System.Object, value, TValue);
                        } else {
                            throw $e1;
                        }
                    }
                } catch ($e2) {
                    $e2 = System.Exception.create($e2);
                    if (H5.is($e2, System.InvalidCastException)) {
                        System.ThrowHelper.ThrowWrongKeyTypeArgumentException(System.Object, key, TKey);
                    } else {
                        throw $e2;
                    }
                }
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$contains: function (keyValuePair) {
                var index = this.IndexOfKey(keyValuePair.key);
                if (index >= 0 && System.Collections.Generic.EqualityComparer$1(TValue).def.equals2(this.values[System.Array.index(index, this.values)], keyValuePair.value)) {
                    return true;
                }
                return false;
            },
            System$Collections$IDictionary$contains: function (key) {
                if (System.Collections.Generic.SortedList$2(TKey,TValue).IsCompatibleKey(key)) {
                    return this.containsKey(H5.cast(H5.unbox(key, TKey), TKey));
                }
                return false;
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$remove: function (keyValuePair) {
                var index = this.IndexOfKey(keyValuePair.key);
                if (index >= 0 && System.Collections.Generic.EqualityComparer$1(TValue).def.equals2(this.values[System.Array.index(index, this.values)], keyValuePair.value)) {
                    this.RemoveAt(index);
                    return true;
                }
                return false;
            },
            remove: function (key) {
                var i = this.IndexOfKey(key);
                if (i >= 0) {
                    this.RemoveAt(i);
                }
                return i >= 0;
            },
            System$Collections$IDictionary$remove: function (key) {
                if (System.Collections.Generic.SortedList$2(TKey,TValue).IsCompatibleKey(key)) {
                    this.remove(H5.cast(H5.unbox(key, TKey), TKey));
                }
            },
            GetKeyListHelper: function () {
                if (this.keyList == null) {
                    this.keyList = new (System.Collections.Generic.SortedList$2.KeyList(TKey,TValue))(this);
                }
                return this.keyList;
            },
            GetValueListHelper: function () {
                if (this.valueList == null) {
                    this.valueList = new (System.Collections.Generic.SortedList$2.ValueList(TKey,TValue))(this);
                }
                return this.valueList;
            },
            clear: function () {
                this.version = (this.version + 1) | 0;
                System.Array.fill(this.keys, function () {
                    return H5.getDefaultValue(TKey);
                }, 0, this._size);
                System.Array.fill(this.values, function () {
                    return H5.getDefaultValue(TValue);
                }, 0, this._size);
                this._size = 0;
            },
            containsKey: function (key) {
                return this.IndexOfKey(key) >= 0;
            },
            ContainsValue: function (value) {
                return this.IndexOfValue(value) >= 0;
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$copyTo: function (array, arrayIndex) {
                if (array == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.array);
                }

                if (arrayIndex < 0 || arrayIndex > array.length) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$2(System.ExceptionArgument.arrayIndex, System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }

                if (((array.length - arrayIndex) | 0) < this.Count) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }

                for (var i = 0; i < this.Count; i = (i + 1) | 0) {
                    var entry = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(this.keys[System.Array.index(i, this.keys)], this.values[System.Array.index(i, this.values)]);
                    array[System.Array.index(((arrayIndex + i) | 0), array)] = entry;
                }
            },
            System$Collections$ICollection$copyTo: function (array, arrayIndex) {
                if (array == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.array);
                }

                if (System.Array.getRank(array) !== 1) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_RankMultiDimNotSupported);
                }

                if (System.Array.getLower(array, 0) !== 0) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_NonZeroLowerBound);
                }

                if (arrayIndex < 0 || arrayIndex > array.length) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$2(System.ExceptionArgument.arrayIndex, System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }

                if (((array.length - arrayIndex) | 0) < this.Count) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }

                var keyValuePairArray = H5.as(array, System.Array.type(System.Collections.Generic.KeyValuePair$2(TKey,TValue)));
                if (keyValuePairArray != null) {
                    for (var i = 0; i < this.Count; i = (i + 1) | 0) {
                        keyValuePairArray[System.Array.index(((i + arrayIndex) | 0), keyValuePairArray)] = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(this.keys[System.Array.index(i, this.keys)], this.values[System.Array.index(i, this.values)]);
                    }
                } else {
                    var objects = H5.as(array, System.Array.type(System.Object));
                    if (objects == null) {
                        System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Argument_InvalidArrayType);
                    }

                    try {
                        for (var i1 = 0; i1 < this.Count; i1 = (i1 + 1) | 0) {
                            objects[System.Array.index(((i1 + arrayIndex) | 0), objects)] = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(this.keys[System.Array.index(i1, this.keys)], this.values[System.Array.index(i1, this.values)]);
                        }
                    } catch ($e1) {
                        $e1 = System.Exception.create($e1);
                        if (H5.is($e1, System.ArrayTypeMismatchException)) {
                            System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Argument_InvalidArrayType);
                        } else {
                            throw $e1;
                        }
                    }

                }
            },
            EnsureCapacity: function (min) {
                var newCapacity = this.keys.length === 0 ? System.Collections.Generic.SortedList$2(TKey,TValue)._defaultCapacity : H5.Int.mul(this.keys.length, 2);
                if ((newCapacity >>> 0) > System.Collections.Generic.SortedList$2(TKey,TValue).MaxArrayLength) {
                    newCapacity = System.Collections.Generic.SortedList$2(TKey,TValue).MaxArrayLength;
                }
                if (newCapacity < min) {
                    newCapacity = min;
                }
                this.Capacity = newCapacity;
            },
            GetByIndex: function (index) {
                if (index < 0 || index >= this._size) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$2(System.ExceptionArgument.index, System.ExceptionResource.ArgumentOutOfRange_Index);
                }
                return this.values[System.Array.index(index, this.values)];
            },
            GetEnumerator: function () {
                return new (System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue)).$ctor1(this, System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue).KeyValuePair).$clone();
            },
            System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$GetEnumerator: function () {
                return new (System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue)).$ctor1(this, System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue).KeyValuePair).$clone();
            },
            System$Collections$IDictionary$GetEnumerator: function () {
                return new (System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue)).$ctor1(this, System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue).DictEntry).$clone();
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return new (System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue)).$ctor1(this, System.Collections.Generic.SortedList$2.Enumerator(TKey,TValue).KeyValuePair).$clone();
            },
            GetKey: function (index) {
                if (index < 0 || index >= this._size) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$2(System.ExceptionArgument.index, System.ExceptionResource.ArgumentOutOfRange_Index);
                }
                return this.keys[System.Array.index(index, this.keys)];
            },
            IndexOfKey: function (key) {
                if (key == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.key);
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
            tryGetValue: function (key, value) {
                var i = this.IndexOfKey(key);
                if (i >= 0) {
                    value.v = this.values[System.Array.index(i, this.values)];
                    return true;
                }

                value.v = H5.getDefaultValue(TValue);
                return false;
            },
            RemoveAt: function (index) {
                if (index < 0 || index >= this._size) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$2(System.ExceptionArgument.index, System.ExceptionResource.ArgumentOutOfRange_Index);
                }
                this._size = (this._size - 1) | 0;
                if (index < this._size) {
                    System.Array.copy(this.keys, ((index + 1) | 0), this.keys, index, ((this._size - index) | 0));
                    System.Array.copy(this.values, ((index + 1) | 0), this.values, index, ((this._size - index) | 0));
                }
                this.keys[System.Array.index(this._size, this.keys)] = H5.getDefaultValue(TKey);
                this.values[System.Array.index(this._size, this.values)] = H5.getDefaultValue(TValue);
                this.version = (this.version + 1) | 0;
            },
            TrimExcess: function () {
                var threshold = H5.Int.clip32(this.keys.length * 0.9);
                if (this._size < threshold) {
                    this.Capacity = this._size;
                }
            }
        }
    }; });
