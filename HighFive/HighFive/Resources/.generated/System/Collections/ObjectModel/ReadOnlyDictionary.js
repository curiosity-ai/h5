    H5.define("System.Collections.ObjectModel.ReadOnlyDictionary$2", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IDictionary$2(TKey,TValue),System.Collections.IDictionary,System.Collections.Generic.IReadOnlyDictionary$2(TKey,TValue)],
        statics: {
            fields: {
                NotSupported_ReadOnlyCollection: null
            },
            ctors: {
                init: function () {
                    this.NotSupported_ReadOnlyCollection = "Collection is read-only.";
                }
            },
            methods: {
                IsCompatibleKey: function (key) {
                    if (key == null) {
                        throw new System.ArgumentNullException.$ctor1("key");
                    }
                    return H5.is(key, TKey);
                }
            }
        },
        fields: {
            m_dictionary: null,
            _keys: null,
            _values: null
        },
        props: {
            Dictionary: {
                get: function () {
                    return this.m_dictionary;
                }
            },
            Keys: {
                get: function () {
                    if (this._keys == null) {
                        this._keys = new (System.Collections.ObjectModel.ReadOnlyDictionary$2.KeyCollection(TKey,TValue))(this.m_dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Keys"]);
                    }
                    return this._keys;
                }
            },
            Values: {
                get: function () {
                    if (this._values == null) {
                        this._values = new (System.Collections.ObjectModel.ReadOnlyDictionary$2.ValueCollection(TKey,TValue))(this.m_dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Values"]);
                    }
                    return this._values;
                }
            },
            System$Collections$Generic$IDictionary$2$Keys: {
                get: function () {
                    return this.Keys;
                }
            },
            System$Collections$Generic$IDictionary$2$Values: {
                get: function () {
                    return this.Values;
                }
            },
            Count: {
                get: function () {
                    return System.Array.getCount(this.m_dictionary, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
                }
            },
            System$Collections$Generic$ICollection$1$IsReadOnly: {
                get: function () {
                    return true;
                }
            },
            System$Collections$IDictionary$IsFixedSize: {
                get: function () {
                    return true;
                }
            },
            System$Collections$IDictionary$IsReadOnly: {
                get: function () {
                    return true;
                }
            },
            System$Collections$IDictionary$Keys: {
                get: function () {
                    return this.Keys;
                }
            },
            System$Collections$IDictionary$Values: {
                get: function () {
                    return this.Values;
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
            },
            System$Collections$Generic$IReadOnlyDictionary$2$Keys: {
                get: function () {
                    return this.Keys;
                }
            },
            System$Collections$Generic$IReadOnlyDictionary$2$Values: {
                get: function () {
                    return this.Values;
                }
            }
        },
        alias: [
            "containsKey", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$containsKey",
            "containsKey", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$containsKey",
            "System$Collections$Generic$IDictionary$2$Keys", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Keys",
            "tryGetValue", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$tryGetValue",
            "tryGetValue", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$tryGetValue",
            "System$Collections$Generic$IDictionary$2$Values", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Values",
            "getItem", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$getItem",
            "System$Collections$Generic$IDictionary$2$add", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$add",
            "System$Collections$Generic$IDictionary$2$remove", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$remove",
            "System$Collections$Generic$IDictionary$2$getItem", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$getItem",
            "System$Collections$Generic$IDictionary$2$setItem", "System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$setItem",
            "Count", ["System$Collections$Generic$IReadOnlyCollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Count", "System$Collections$Generic$IReadOnlyCollection$1$Count"],
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Count",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$contains", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$contains",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$copyTo", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$copyTo",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$IsReadOnly", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$IsReadOnly",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$add", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$add",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$clear", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$clear",
            "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$remove", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$remove",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"],
            "System$Collections$Generic$IReadOnlyDictionary$2$Keys", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Keys",
            "System$Collections$Generic$IReadOnlyDictionary$2$Values", "System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$Values"
        ],
        ctors: {
            ctor: function (dictionary) {
                this.$initialize();
                if (dictionary == null) {
                    throw new System.ArgumentNullException.$ctor1("dictionary");
                }
                this.m_dictionary = dictionary;
            }
        },
        methods: {
            getItem: function (key) {
                return this.m_dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$getItem"](key);
            },
            System$Collections$Generic$IDictionary$2$getItem: function (key) {
                return this.m_dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$getItem"](key);
            },
            System$Collections$Generic$IDictionary$2$setItem: function (key, value) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$IDictionary$getItem: function (key) {
                if (System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).IsCompatibleKey(key)) {
                    return this.getItem(H5.cast(H5.unbox(key, TKey), TKey));
                }
                return null;
            },
            System$Collections$IDictionary$setItem: function (key, value) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            containsKey: function (key) {
                return this.m_dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$containsKey"](key);
            },
            tryGetValue: function (key, value) {
                return this.m_dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$tryGetValue"](key, value);
            },
            System$Collections$Generic$IDictionary$2$add: function (key, value) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$add: function (item) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$IDictionary$add: function (key, value) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$Generic$IDictionary$2$remove: function (key) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$remove: function (item) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$IDictionary$remove: function (key) {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$contains: function (item) {
                return System.Array.contains(this.m_dictionary, item, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
            },
            System$Collections$IDictionary$contains: function (key) {
                return System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).IsCompatibleKey(key) && this.containsKey(H5.cast(H5.unbox(key, TKey), TKey));
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$copyTo: function (array, arrayIndex) {
                System.Array.copyTo(this.m_dictionary, array, arrayIndex, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
            },
            System$Collections$ICollection$copyTo: function (array, index) {
                var $t, $t1;
                if (array == null) {
                    throw new System.ArgumentNullException.$ctor1("array");
                }

                if (System.Array.getRank(array) !== 1) {
                    throw new System.ArgumentException.$ctor1("Only single dimensional arrays are supported for the requested action.");
                }

                if (System.Array.getLower(array, 0) !== 0) {
                    throw new System.ArgumentException.$ctor1("The lower bound of target array must be zero.");
                }

                if (index < 0 || index > array.length) {
                    throw new System.ArgumentOutOfRangeException.$ctor4("index", "Non-negative number required.");
                }

                if (((array.length - index) | 0) < this.Count) {
                    throw new System.ArgumentException.$ctor1("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
                }

                var pairs = H5.as(array, System.Array.type(System.Collections.Generic.KeyValuePair$2(TKey,TValue)));
                if (pairs != null) {
                    System.Array.copyTo(this.m_dictionary, pairs, index, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
                } else {
                    var dictEntryArray = H5.as(array, System.Array.type(System.Collections.DictionaryEntry));
                    if (dictEntryArray != null) {
                        $t = H5.getEnumerator(this.m_dictionary, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
                        try {
                            while ($t.moveNext()) {
                                var item = $t.Current;
                                dictEntryArray[System.Array.index(H5.identity(index, ((index = (index + 1) | 0))), dictEntryArray)] = new System.Collections.DictionaryEntry.$ctor1(item.key, item.value);
                            }
                        } finally {
                            if (H5.is($t, System.IDisposable)) {
                                $t.System$IDisposable$Dispose();
                            }
                        }
                    } else {
                        var objects = H5.as(array, System.Array.type(System.Object));
                        if (objects == null) {
                            throw new System.ArgumentException.$ctor1("Target array type is not compatible with the type of items in the collection.");
                        }

                        try {
                            $t1 = H5.getEnumerator(this.m_dictionary, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
                            try {
                                while ($t1.moveNext()) {
                                    var item1 = $t1.Current;
                                    objects[System.Array.index(H5.identity(index, ((index = (index + 1) | 0))), objects)] = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(item1.key, item1.value);
                                }
                            } finally {
                                if (H5.is($t1, System.IDisposable)) {
                                    $t1.System$IDisposable$Dispose();
                                }
                            }
                        } catch ($e1) {
                            $e1 = System.Exception.create($e1);
                            if (H5.is($e1, System.ArrayTypeMismatchException)) {
                                throw new System.ArgumentException.$ctor1("Target array type is not compatible with the type of items in the collection.");
                            } else {
                                throw $e1;
                            }
                        }
                    }
                }
            },
            System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$clear: function () {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            System$Collections$IDictionary$clear: function () {
                throw new System.NotSupportedException.$ctor1(System.Collections.ObjectModel.ReadOnlyDictionary$2(TKey,TValue).NotSupported_ReadOnlyCollection);
            },
            GetEnumerator: function () {
                return H5.getEnumerator(this.m_dictionary, System.Collections.Generic.KeyValuePair$2(TKey,TValue));
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return H5.getEnumerator(H5.cast(this.m_dictionary, System.Collections.IEnumerable));
            },
            System$Collections$IDictionary$GetEnumerator: function () {
                var d = H5.as(this.m_dictionary, System.Collections.IDictionary);
                if (d != null) {
                    return d.System$Collections$IDictionary$GetEnumerator();
                }
                return new (System.Collections.ObjectModel.ReadOnlyDictionary$2.DictionaryEnumerator(TKey,TValue)).$ctor1(this.m_dictionary).$clone();
            }
        }
    }; });
