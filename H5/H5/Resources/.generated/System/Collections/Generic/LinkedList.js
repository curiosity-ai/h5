    H5.define("System.Collections.Generic.LinkedList$1", function (T) { return {
        inherits: [System.Collections.Generic.ICollection$1(T),System.Collections.ICollection,System.Collections.Generic.IReadOnlyCollection$1(T)],
        statics: {
            fields: {
                VersionName: null,
                CountName: null,
                ValuesName: null
            },
            ctors: {
                init: function () {
                    this.VersionName = "Version";
                    this.CountName = "Count";
                    this.ValuesName = "Data";
                }
            }
        },
        fields: {
            head: null,
            count: 0,
            version: 0
        },
        props: {
            Count: {
                get: function () {
                    return this.count;
                }
            },
            First: {
                get: function () {
                    return this.head;
                }
            },
            Last: {
                get: function () {
                    return this.head == null ? null : this.head.prev;
                }
            },
            System$Collections$Generic$ICollection$1$IsReadOnly: {
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
            "Count", ["System$Collections$Generic$IReadOnlyCollection$1$" + H5.getTypeAlias(T) + "$Count", "System$Collections$Generic$IReadOnlyCollection$1$Count"],
            "Count", "System$Collections$ICollection$Count",
            "Count", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$Count",
            "System$Collections$Generic$ICollection$1$IsReadOnly", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$IsReadOnly",
            "System$Collections$Generic$ICollection$1$add", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$add",
            "clear", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$clear",
            "contains", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$contains",
            "copyTo", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$copyTo",
            "System$Collections$Generic$IEnumerable$1$GetEnumerator", "System$Collections$Generic$IEnumerable$1$" + H5.getTypeAlias(T) + "$GetEnumerator",
            "remove", "System$Collections$Generic$ICollection$1$" + H5.getTypeAlias(T) + "$remove"
        ],
        ctors: {
            ctor: function () {
                this.$initialize();
            },
            $ctor1: function (collection) {
                var $t;
                this.$initialize();
                if (collection == null) {
                    throw new System.ArgumentNullException.$ctor1("collection");
                }

                $t = H5.getEnumerator(collection, T);
                try {
                    while ($t.moveNext()) {
                        var item = $t.Current;
                        this.AddLast(item);
                    }
                } finally {
                    if (H5.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }
            }
        },
        methods: {
            System$Collections$Generic$ICollection$1$add: function (value) {
                this.AddLast(value);
            },
            AddAfter: function (node, value) {
                this.ValidateNode(node);
                var result = new (System.Collections.Generic.LinkedListNode$1(T)).$ctor1(node.list, value);
                this.InternalInsertNodeBefore(node.next, result);
                return result;
            },
            AddAfter$1: function (node, newNode) {
                this.ValidateNode(node);
                this.ValidateNewNode(newNode);
                this.InternalInsertNodeBefore(node.next, newNode);
                newNode.list = this;
            },
            AddBefore: function (node, value) {
                this.ValidateNode(node);
                var result = new (System.Collections.Generic.LinkedListNode$1(T)).$ctor1(node.list, value);
                this.InternalInsertNodeBefore(node, result);
                if (H5.referenceEquals(node, this.head)) {
                    this.head = result;
                }
                return result;
            },
            AddBefore$1: function (node, newNode) {
                this.ValidateNode(node);
                this.ValidateNewNode(newNode);
                this.InternalInsertNodeBefore(node, newNode);
                newNode.list = this;
                if (H5.referenceEquals(node, this.head)) {
                    this.head = newNode;
                }
            },
            AddFirst: function (value) {
                var result = new (System.Collections.Generic.LinkedListNode$1(T)).$ctor1(this, value);
                if (this.head == null) {
                    this.InternalInsertNodeToEmptyList(result);
                } else {
                    this.InternalInsertNodeBefore(this.head, result);
                    this.head = result;
                }
                return result;
            },
            AddFirst$1: function (node) {
                this.ValidateNewNode(node);

                if (this.head == null) {
                    this.InternalInsertNodeToEmptyList(node);
                } else {
                    this.InternalInsertNodeBefore(this.head, node);
                    this.head = node;
                }
                node.list = this;
            },
            AddLast: function (value) {
                var result = new (System.Collections.Generic.LinkedListNode$1(T)).$ctor1(this, value);
                if (this.head == null) {
                    this.InternalInsertNodeToEmptyList(result);
                } else {
                    this.InternalInsertNodeBefore(this.head, result);
                }
                return result;
            },
            AddLast$1: function (node) {
                this.ValidateNewNode(node);

                if (this.head == null) {
                    this.InternalInsertNodeToEmptyList(node);
                } else {
                    this.InternalInsertNodeBefore(this.head, node);
                }
                node.list = this;
            },
            clear: function () {
                var current = this.head;
                while (current != null) {
                    var temp = current;
                    current = current.Next;
                    temp.Invalidate();
                }

                this.head = null;
                this.count = 0;
                this.version = (this.version + 1) | 0;
            },
            contains: function (value) {
                return this.Find(value) != null;
            },
            copyTo: function (array, index) {
                if (array == null) {
                    throw new System.ArgumentNullException.$ctor1("array");
                }

                if (index < 0 || index > array.length) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if (((array.length - index) | 0) < this.Count) {
                    throw new System.ArgumentException.ctor();
                }

                var node = this.head;
                if (node != null) {
                    do {
                        array[System.Array.index(H5.identity(index, ((index = (index + 1) | 0))), array)] = node.item;
                        node = node.next;
                    } while (!H5.referenceEquals(node, this.head));
                }
            },
            System$Collections$ICollection$copyTo: function (array, index) {
                if (array == null) {
                    throw new System.ArgumentNullException.$ctor1("array");
                }

                if (System.Array.getRank(array) !== 1) {
                    throw new System.ArgumentException.ctor();
                }

                if (System.Array.getLower(array, 0) !== 0) {
                    throw new System.ArgumentException.ctor();
                }

                if (index < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("index");
                }

                if (((array.length - index) | 0) < this.Count) {
                    throw new System.ArgumentException.ctor();
                }

                var tArray = H5.as(array, System.Array.type(T));
                if (tArray != null) {
                    this.copyTo(tArray, index);
                } else {
                    var targetType = (H5.getType(array).$elementType || null);
                    var sourceType = T;
                    if (!(H5.Reflection.isAssignableFrom(targetType, sourceType) || H5.Reflection.isAssignableFrom(sourceType, targetType))) {
                        throw new System.ArgumentException.ctor();
                    }

                    var objects = H5.as(array, System.Array.type(System.Object));
                    if (objects == null) {
                        throw new System.ArgumentException.ctor();
                    }
                    var node = this.head;
                    try {
                        if (node != null) {
                            do {
                                objects[System.Array.index(H5.identity(index, ((index = (index + 1) | 0))), objects)] = node.item;
                                node = node.next;
                            } while (!H5.referenceEquals(node, this.head));
                        }
                    } catch ($e1) {
                        $e1 = System.Exception.create($e1);
                        if (H5.is($e1, System.ArrayTypeMismatchException)) {
                            throw new System.ArgumentException.ctor();
                        } else {
                            throw $e1;
                        }
                    }
                }
            },
            Find: function (value) {
                var node = this.head;
                var c = System.Collections.Generic.EqualityComparer$1(T).def;
                if (node != null) {
                    if (value != null) {
                        do {
                            if (c.equals2(node.item, value)) {
                                return node;
                            }
                            node = node.next;
                        } while (!H5.referenceEquals(node, this.head));
                    } else {
                        do {
                            if (node.item == null) {
                                return node;
                            }
                            node = node.next;
                        } while (!H5.referenceEquals(node, this.head));
                    }
                }
                return null;
            },
            FindLast: function (value) {
                if (this.head == null) {
                    return null;
                }

                var last = this.head.prev;
                var node = last;
                var c = System.Collections.Generic.EqualityComparer$1(T).def;
                if (node != null) {
                    if (value != null) {
                        do {
                            if (c.equals2(node.item, value)) {
                                return node;
                            }

                            node = node.prev;
                        } while (!H5.referenceEquals(node, last));
                    } else {
                        do {
                            if (node.item == null) {
                                return node;
                            }
                            node = node.prev;
                        } while (!H5.referenceEquals(node, last));
                    }
                }
                return null;
            },
            GetEnumerator: function () {
                return new (System.Collections.Generic.LinkedList$1.Enumerator(T)).$ctor1(this);
            },
            System$Collections$Generic$IEnumerable$1$GetEnumerator: function () {
                return this.GetEnumerator().$clone();
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return this.GetEnumerator().$clone();
            },
            remove: function (value) {
                var node = this.Find(value);
                if (node != null) {
                    this.InternalRemoveNode(node);
                    return true;
                }
                return false;
            },
            Remove: function (node) {
                this.ValidateNode(node);
                this.InternalRemoveNode(node);
            },
            RemoveFirst: function () {
                if (this.head == null) {
                    throw new System.InvalidOperationException.ctor();
                }
                this.InternalRemoveNode(this.head);
            },
            RemoveLast: function () {
                if (this.head == null) {
                    throw new System.InvalidOperationException.ctor();
                }
                this.InternalRemoveNode(this.head.prev);
            },
            InternalInsertNodeBefore: function (node, newNode) {
                newNode.next = node;
                newNode.prev = node.prev;
                node.prev.next = newNode;
                node.prev = newNode;
                this.version = (this.version + 1) | 0;
                this.count = (this.count + 1) | 0;
            },
            InternalInsertNodeToEmptyList: function (newNode) {
                System.Diagnostics.Debug.Assert$1(this.head == null && this.count === 0, "LinkedList must be empty when this method is called!");
                newNode.next = newNode;
                newNode.prev = newNode;
                this.head = newNode;
                this.version = (this.version + 1) | 0;
                this.count = (this.count + 1) | 0;
            },
            InternalRemoveNode: function (node) {
                System.Diagnostics.Debug.Assert$1(H5.referenceEquals(node.list, this), "Deleting the node from another list!");
                System.Diagnostics.Debug.Assert$1(this.head != null, "This method shouldn't be called on empty list!");
                if (H5.referenceEquals(node.next, node)) {
                    System.Diagnostics.Debug.Assert$1(this.count === 1 && H5.referenceEquals(this.head, node), "this should only be true for a list with only one node");
                    this.head = null;
                } else {
                    node.next.prev = node.prev;
                    node.prev.next = node.next;
                    if (H5.referenceEquals(this.head, node)) {
                        this.head = node.next;
                    }
                }
                node.Invalidate();
                this.count = (this.count - 1) | 0;
                this.version = (this.version + 1) | 0;
            },
            ValidateNewNode: function (node) {
                if (node == null) {
                    throw new System.ArgumentNullException.$ctor1("node");
                }

                if (node.list != null) {
                    throw new System.InvalidOperationException.ctor();
                }
            },
            ValidateNode: function (node) {
                if (node == null) {
                    throw new System.ArgumentNullException.$ctor1("node");
                }

                if (!H5.referenceEquals(node.list, this)) {
                    throw new System.InvalidOperationException.ctor();
                }
            }
        }
    }; });
