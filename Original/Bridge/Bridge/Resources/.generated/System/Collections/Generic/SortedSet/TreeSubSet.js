    Bridge.define("System.Collections.Generic.SortedSet$1.TreeSubSet", function (T) { return {
        inherits: [System.Collections.Generic.SortedSet$1(T)],
        $kind: "nested class",
        fields: {
            underlying: null,
            min: Bridge.getDefaultValue(T),
            max: Bridge.getDefaultValue(T),
            lBoundActive: false,
            uBoundActive: false
        },
        alias: [
            "contains", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$contains",
            "clear", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$clear"
        ],
        ctors: {
            $ctor1: function (Underlying, Min, Max, lowerBoundActive, upperBoundActive) {
                this.$initialize();
                System.Collections.Generic.SortedSet$1(T).$ctor1.call(this, Underlying.Comparer);
                this.underlying = Underlying;
                this.min = Min;
                this.max = Max;
                this.lBoundActive = lowerBoundActive;
                this.uBoundActive = upperBoundActive;
                this.root = this.underlying.FindRange$1(this.min, this.max, this.lBoundActive, this.uBoundActive);
                this.count = 0;
                this.version = -1;
                this.VersionCheckImpl();
            },
            ctor: function () {
                this.$initialize();
                System.Collections.Generic.SortedSet$1(T).ctor.call(this);
                this.comparer = null;
            }
        },
        methods: {
            AddIfNotPresent: function (item) {

                if (!this.IsWithinRange(item)) {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException$1(System.ExceptionArgument.collection);
                }

                var ret = this.underlying.AddIfNotPresent(item);
                this.VersionCheck();

                return ret;
            },
            contains: function (item) {
                this.VersionCheck();
                return System.Collections.Generic.SortedSet$1(T).prototype.contains.call(this, item);
            },
            DoRemove: function (item) {

                if (!this.IsWithinRange(item)) {
                    return false;
                }

                var ret = this.underlying.remove(item);
                this.VersionCheck();
                return ret;
            },
            clear: function () {


                if (this.count === 0) {
                    return;
                }

                var toRemove = new (System.Collections.Generic.List$1(T)).ctor();
                this.BreadthFirstTreeWalk(function (n) {
                    toRemove.add(n.Item);
                    return true;
                });
                while (toRemove.Count !== 0) {
                    this.underlying.remove(toRemove.getItem(((toRemove.Count - 1) | 0)));
                    toRemove.removeAt(((toRemove.Count - 1) | 0));
                }
                this.root = null;
                this.count = 0;
                this.version = this.underlying.version;
            },
            IsWithinRange: function (item) {
                var $t, $t1;

                var comp = (this.lBoundActive ? ($t = this.Comparer)[Bridge.geti($t, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](this.min, item) : -1);
                if (comp > 0) {
                    return false;
                }
                comp = (this.uBoundActive ? ($t1 = this.Comparer)[Bridge.geti($t1, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](this.max, item) : 1);
                if (comp < 0) {
                    return false;
                }
                return true;
            },
            InOrderTreeWalk$1: function (action, reverse) {
                var $t, $t1;
                this.VersionCheck();

                if (this.root == null) {
                    return true;
                }

                var stack = new (System.Collections.Generic.Stack$1(System.Collections.Generic.SortedSet$1.Node(T))).$ctor2(Bridge.Int.mul(2, System.Collections.Generic.SortedSet$1(T).log2(((this.count + 1) | 0))));
                var current = this.root;
                while (current != null) {
                    if (this.IsWithinRange(current.Item)) {
                        stack.Push(current);
                        current = (reverse ? current.Right : current.Left);
                    } else if (this.lBoundActive && ($t = this.Comparer)[Bridge.geti($t, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](this.min, current.Item) > 0) {
                        current = current.Right;
                    } else {
                        current = current.Left;
                    }
                }

                while (stack.Count !== 0) {
                    current = stack.Pop();
                    if (!action(current)) {
                        return false;
                    }

                    var node = (reverse ? current.Left : current.Right);
                    while (node != null) {
                        if (this.IsWithinRange(node.Item)) {
                            stack.Push(node);
                            node = (reverse ? node.Right : node.Left);
                        } else if (this.lBoundActive && ($t1 = this.Comparer)[Bridge.geti($t1, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](this.min, node.Item) > 0) {
                            node = node.Right;
                        } else {
                            node = node.Left;
                        }
                    }
                }
                return true;
            },
            BreadthFirstTreeWalk: function (action) {
                var $t, $t1;
                this.VersionCheck();

                if (this.root == null) {
                    return true;
                }

                var processQueue = new (System.Collections.Generic.List$1(System.Collections.Generic.SortedSet$1.Node(T))).ctor();
                processQueue.add(this.root);
                var current;

                while (processQueue.Count !== 0) {
                    current = processQueue.getItem(0);
                    processQueue.removeAt(0);
                    if (this.IsWithinRange(current.Item) && !action(current)) {
                        return false;
                    }
                    if (current.Left != null && (!this.lBoundActive || ($t = this.Comparer)[Bridge.geti($t, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](this.min, current.Item) < 0)) {
                        processQueue.add(current.Left);
                    }
                    if (current.Right != null && (!this.uBoundActive || ($t1 = this.Comparer)[Bridge.geti($t1, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](this.max, current.Item) > 0)) {
                        processQueue.add(current.Right);
                    }

                }
                return true;
            },
            FindNode: function (item) {

                if (!this.IsWithinRange(item)) {
                    return null;
                }
                this.VersionCheck();
                return System.Collections.Generic.SortedSet$1(T).prototype.FindNode.call(this, item);
            },
            InternalIndexOf: function (item) {
                var $t, $t1;
                var count = -1;
                $t = Bridge.getEnumerator(this);
                try {
                    while ($t.moveNext()) {
                        var i = $t.Current;
                        count = (count + 1) | 0;
                        if (($t1 = this.Comparer)[Bridge.geti($t1, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](item, i) === 0) {
                            return count;
                        }
                    }
                } finally {
                    if (Bridge.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }
                return -1;
            },
            VersionCheck: function () {
                this.VersionCheckImpl();
            },
            VersionCheckImpl: function () {
                if (this.version !== this.underlying.version) {
                    this.root = this.underlying.FindRange$1(this.min, this.max, this.lBoundActive, this.uBoundActive);
                    this.version = this.underlying.version;
                    this.count = 0;
                    this.InOrderTreeWalk(Bridge.fn.bind(this, $asm.$.System.Collections.Generic.SortedSet$1.TreeSubSet.f1));
                }
            },
            GetViewBetween: function (lowerValue, upperValue) {
                var $t, $t1;

                if (this.lBoundActive && ($t = this.Comparer)[Bridge.geti($t, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](this.min, lowerValue) > 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("lowerValue");
                }
                if (this.uBoundActive && ($t1 = this.Comparer)[Bridge.geti($t1, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare")](this.max, upperValue) < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("upperValue");
                }
                var ret = Bridge.cast(this.underlying.GetViewBetween(lowerValue, upperValue), System.Collections.Generic.SortedSet$1.TreeSubSet(T));
                return ret;
            },
            IntersectWithEnumerable: function (other) {
                var $t;

                var toSave = new (System.Collections.Generic.List$1(T)).$ctor2(this.Count);
                $t = Bridge.getEnumerator(other, T);
                try {
                    while ($t.moveNext()) {
                        var item = $t.Current;
                        if (this.contains(item)) {
                            toSave.add(item);
                            this.remove(item);
                        }
                    }
                } finally {
                    if (Bridge.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }
                this.clear();
                this.AddAllElements(toSave);
            }
        }
    }; });

    Bridge.ns("System.Collections.Generic.SortedSet$1.TreeSubSet", $asm.$);

    Bridge.apply($asm.$.System.Collections.Generic.SortedSet$1.TreeSubSet, {
        f1: function (n) {
            this.count = (this.count + 1) | 0;
            return true;
        }
    });
