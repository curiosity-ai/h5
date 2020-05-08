    Bridge.define("System.Collections.Generic.Dictionary$2.KeyCollection", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.ICollection$1(TKey),System.Collections.ICollection,System.Collections.Generic.IReadOnlyCollection$1(TKey)],
        $kind: "nested class",
        fields: {
            dictionary: null
        },
        props: {
            Count: {
                get: function () {
                    return this.dictionary.Count;
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
                    return Bridge.cast(this.dictionary, System.Collections.ICollection).System$Collections$ICollection$SyncRoot;
                }
            }
        },
        alias: [
            "copyTo", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TKey) + "$copyTo",
            "Count", ["System$Collections$Generic$IReadOnlyCollection$1$" + Bridge.getTypeAlias(TKey) + "$Count", "System$Collections$Generic$IReadOnlyCollection$1$Count"],
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TKey) + "$Count",
            "System$Collections$Generic$ICollection$1$IsReadOnly", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TKey) + "$IsReadOnly",
            "System$Collections$Generic$ICollection$1$add", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TKey) + "$add",
            "System$Collections$Generic$ICollection$1$clear", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TKey) + "$clear",
            "System$Collections$Generic$ICollection$1$contains", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TKey) + "$contains",
            "System$Collections$Generic$ICollection$1$remove", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(TKey) + "$remove",
            "System$Collections$Generic$IEnumerable$1$GetEnumerator", "System$Collections$Generic$IEnumerable$1$" + Bridge.getTypeAlias(TKey) + "$GetEnumerator"
        ],
        ctors: {
            ctor: function (dictionary) {
                this.$initialize();
                if (dictionary == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.dictionary);
                }
                this.dictionary = dictionary;
            }
        },
        methods: {
            GetEnumerator: function () {
                return new (System.Collections.Generic.Dictionary$2.KeyCollection.Enumerator(TKey,TValue)).$ctor1(this.dictionary);
            },
            System$Collections$Generic$IEnumerable$1$GetEnumerator: function () {
                return new (System.Collections.Generic.Dictionary$2.KeyCollection.Enumerator(TKey,TValue)).$ctor1(this.dictionary).$clone();
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return new (System.Collections.Generic.Dictionary$2.KeyCollection.Enumerator(TKey,TValue)).$ctor1(this.dictionary).$clone();
            },
            copyTo: function (array, index) {
                if (array == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.array);
                }

                if (index < 0 || index > array.length) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$2(System.ExceptionArgument.index, System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }

                if (((array.length - index) | 0) < this.dictionary.Count) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }

                var count = this.dictionary.count;
                var entries = this.dictionary.entries;
                for (var i = 0; i < count; i = (i + 1) | 0) {
                    if (entries[System.Array.index(i, entries)].hashCode >= 0) {
                        array[System.Array.index(Bridge.identity(index, ((index = (index + 1) | 0))), array)] = entries[System.Array.index(i, entries)].key;
                    }
                }
            },
            System$Collections$ICollection$copyTo: function (array, index) {
                if (array == null) {
                    System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.array);
                }

                if (System.Array.getRank(array) !== 1) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_RankMultiDimNotSupported);
                }

                if (System.Array.getLower(array, 0) !== 0) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_NonZeroLowerBound);
                }

                if (index < 0 || index > array.length) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$2(System.ExceptionArgument.index, System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }

                if (((array.length - index) | 0) < this.dictionary.Count) {
                    System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }

                var keys = Bridge.as(array, System.Array.type(TKey));
                if (keys != null) {
                    this.copyTo(keys, index);
                } else {
                    var objects = Bridge.as(array, System.Array.type(System.Object));
                    if (objects == null) {
                        System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Argument_InvalidArrayType);
                    }

                    var count = this.dictionary.count;
                    var entries = this.dictionary.entries;
                    try {
                        for (var i = 0; i < count; i = (i + 1) | 0) {
                            if (entries[System.Array.index(i, entries)].hashCode >= 0) {
                                objects[System.Array.index(Bridge.identity(index, ((index = (index + 1) | 0))), objects)] = entries[System.Array.index(i, entries)].key;
                            }
                        }
                    } catch ($e1) {
                        $e1 = System.Exception.create($e1);
                        if (Bridge.is($e1, System.ArrayTypeMismatchException)) {
                            System.ThrowHelper.ThrowArgumentException(System.ExceptionResource.Argument_InvalidArrayType);
                        } else {
                            throw $e1;
                        }
                    }
                }
            },
            System$Collections$Generic$ICollection$1$add: function (item) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_KeyCollectionSet);
            },
            System$Collections$Generic$ICollection$1$clear: function () {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_KeyCollectionSet);
            },
            System$Collections$Generic$ICollection$1$contains: function (item) {
                return this.dictionary.containsKey(item);
            },
            System$Collections$Generic$ICollection$1$remove: function (item) {
                System.ThrowHelper.ThrowNotSupportedException$1(System.ExceptionResource.NotSupported_KeyCollectionSet);
                return false;
            }
        }
    }; });
