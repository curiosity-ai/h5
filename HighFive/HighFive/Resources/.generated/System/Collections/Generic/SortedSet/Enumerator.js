    HighFive.define("System.Collections.Generic.SortedSet$1.Enumerator", function (T) { return {
        inherits: [System.Collections.Generic.IEnumerator$1(T),System.Collections.IEnumerator],
        $kind: "nested struct",
        statics: {
            fields: {
                dummyNode: null
            },
            ctors: {
                init: function () {
                    this.dummyNode = new (System.Collections.Generic.SortedSet$1.Node(T)).ctor(HighFive.getDefaultValue(T));
                }
            },
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.SortedSet$1.Enumerator(T))(); }
            }
        },
        fields: {
            tree: null,
            version: 0,
            stack: null,
            current: null,
            reverse: false
        },
        props: {
            Current: {
                get: function () {
                    if (this.current != null) {
                        return this.current.Item;
                    }
                    return HighFive.getDefaultValue(T);
                }
            },
            System$Collections$IEnumerator$Current: {
                get: function () {
                    if (this.current == null) {
                        System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return this.current.Item;
                }
            },
            NotStartedOrEnded: {
                get: function () {
                    return this.current == null;
                }
            }
        },
        alias: [
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "Dispose", "System$IDisposable$Dispose",
            "Current", ["System$Collections$Generic$IEnumerator$1$" + HighFive.getTypeAlias(T) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"]
        ],
        ctors: {
            $ctor1: function (set) {
                this.$initialize();
                this.tree = set;
                this.tree.VersionCheck();

                this.version = this.tree.version;

                this.stack = new (System.Collections.Generic.Stack$1(System.Collections.Generic.SortedSet$1.Node(T))).$ctor2(HighFive.Int.mul(2, System.Collections.Generic.SortedSet$1(T).log2(((set.Count + 1) | 0))));
                this.current = null;
                this.reverse = false;

                this.Intialize();
            },
            $ctor2: function (set, reverse) {
                this.$initialize();
                this.tree = set;
                this.tree.VersionCheck();
                this.version = this.tree.version;

                this.stack = new (System.Collections.Generic.Stack$1(System.Collections.Generic.SortedSet$1.Node(T))).$ctor2(HighFive.Int.mul(2, System.Collections.Generic.SortedSet$1(T).log2(((set.Count + 1) | 0))));
                this.current = null;
                this.reverse = reverse;

                this.Intialize();

            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            Intialize: function () {

                this.current = null;
                var node = this.tree.root;
                var next = null, other = null;
                while (node != null) {
                    next = (this.reverse ? node.Right : node.Left);
                    other = (this.reverse ? node.Left : node.Right);
                    if (this.tree.IsWithinRange(node.Item)) {
                        this.stack.Push(node);
                        node = next;
                    } else if (next == null || !this.tree.IsWithinRange(next.Item)) {
                        node = other;
                    } else {
                        node = next;
                    }
                }
            },
            moveNext: function () {

                this.tree.VersionCheck();

                if (this.version !== this.tree.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.stack.Count === 0) {
                    this.current = null;
                    return false;
                }

                this.current = this.stack.Pop();
                var node = (this.reverse ? this.current.Left : this.current.Right);
                var next = null, other = null;
                while (node != null) {
                    next = (this.reverse ? node.Right : node.Left);
                    other = (this.reverse ? node.Left : node.Right);
                    if (this.tree.IsWithinRange(node.Item)) {
                        this.stack.Push(node);
                        node = next;
                    } else if (other == null || !this.tree.IsWithinRange(other.Item)) {
                        node = next;
                    } else {
                        node = other;
                    }
                }
                return true;
            },
            Dispose: function () { },
            Reset: function () {
                if (this.version !== this.tree.version) {
                    System.ThrowHelper.ThrowInvalidOperationException(System.ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.stack.Clear();
                this.Intialize();
            },
            System$Collections$IEnumerator$reset: function () {
                this.Reset();
            },
            getHashCode: function () {
                var h = HighFive.addHash([3788985113, this.tree, this.version, this.stack, this.current, this.reverse]);
                return h;
            },
            equals: function (o) {
                if (!HighFive.is(o, System.Collections.Generic.SortedSet$1.Enumerator(T))) {
                    return false;
                }
                return HighFive.equals(this.tree, o.tree) && HighFive.equals(this.version, o.version) && HighFive.equals(this.stack, o.stack) && HighFive.equals(this.current, o.current) && HighFive.equals(this.reverse, o.reverse);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.Generic.SortedSet$1.Enumerator(T))();
                s.tree = this.tree;
                s.version = this.version;
                s.stack = this.stack;
                s.current = this.current;
                s.reverse = this.reverse;
                return s;
            }
        }
    }; });
