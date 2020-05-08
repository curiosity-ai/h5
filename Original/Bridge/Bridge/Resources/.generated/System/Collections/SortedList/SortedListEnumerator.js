    Bridge.define("System.Collections.SortedList.SortedListEnumerator", {
        inherits: [System.Collections.IDictionaryEnumerator,System.ICloneable],
        $kind: "nested class",
        statics: {
            fields: {
                Keys: 0,
                Values: 0,
                DictEntry: 0
            },
            ctors: {
                init: function () {
                    this.Keys = 1;
                    this.Values = 2;
                    this.DictEntry = 3;
                }
            }
        },
        fields: {
            sortedList: null,
            key: null,
            value: null,
            index: 0,
            startIndex: 0,
            endIndex: 0,
            version: 0,
            current: false,
            getObjectRetType: 0
        },
        props: {
            Key: {
                get: function () {
                    if (this.version !== this.sortedList.version) {
                        throw new System.InvalidOperationException.ctor();
                    }
                    if (this.current === false) {
                        throw new System.InvalidOperationException.ctor();
                    }
                    return this.key;
                }
            },
            Entry: {
                get: function () {
                    if (this.version !== this.sortedList.version) {
                        throw new System.InvalidOperationException.ctor();
                    }
                    if (this.current === false) {
                        throw new System.InvalidOperationException.ctor();
                    }
                    return new System.Collections.DictionaryEntry.$ctor1(this.key, this.value);
                }
            },
            Current: {
                get: function () {
                    if (this.current === false) {
                        throw new System.InvalidOperationException.ctor();
                    }

                    if (this.getObjectRetType === System.Collections.SortedList.SortedListEnumerator.Keys) {
                        return this.key;
                    } else {
                        if (this.getObjectRetType === System.Collections.SortedList.SortedListEnumerator.Values) {
                            return this.value;
                        } else {
                            return new System.Collections.DictionaryEntry.$ctor1(this.key, this.value).$clone();
                        }
                    }
                }
            },
            Value: {
                get: function () {
                    if (this.version !== this.sortedList.version) {
                        throw new System.InvalidOperationException.ctor();
                    }
                    if (this.current === false) {
                        throw new System.InvalidOperationException.ctor();
                    }
                    return this.value;
                }
            }
        },
        alias: [
            "clone", "System$ICloneable$clone",
            "Key", "System$Collections$IDictionaryEnumerator$Key",
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "Entry", "System$Collections$IDictionaryEnumerator$Entry",
            "Current", "System$Collections$IEnumerator$Current",
            "Value", "System$Collections$IDictionaryEnumerator$Value",
            "reset", "System$Collections$IEnumerator$reset"
        ],
        ctors: {
            ctor: function (sortedList, index, count, getObjRetType) {
                this.$initialize();
                this.sortedList = sortedList;
                this.index = index;
                this.startIndex = index;
                this.endIndex = (index + count) | 0;
                this.version = sortedList.version;
                this.getObjectRetType = getObjRetType;
                this.current = false;
            }
        },
        methods: {
            clone: function () {
                return Bridge.clone(this);
            },
            moveNext: function () {
                var $t, $t1;
                if (this.version !== this.sortedList.version) {
                    throw new System.InvalidOperationException.ctor();
                }
                if (this.index < this.endIndex) {
                    this.key = ($t = this.sortedList.keys)[System.Array.index(this.index, $t)];
                    this.value = ($t1 = this.sortedList.values)[System.Array.index(this.index, $t1)];
                    this.index = (this.index + 1) | 0;
                    this.current = true;
                    return true;
                }
                this.key = null;
                this.value = null;
                this.current = false;
                return false;
            },
            reset: function () {
                if (this.version !== this.sortedList.version) {
                    throw new System.InvalidOperationException.ctor();
                }
                this.index = this.startIndex;
                this.current = false;
                this.key = null;
                this.value = null;
            }
        }
    });
