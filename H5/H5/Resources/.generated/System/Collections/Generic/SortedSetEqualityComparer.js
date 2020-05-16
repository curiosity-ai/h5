    H5.define("System.Collections.Generic.SortedSetEqualityComparer$1", function (T) { return {
        inherits: [System.Collections.Generic.IEqualityComparer$1(System.Collections.Generic.SortedSet$1(T))],
        fields: {
            comparer: null,
            e_comparer: null
        },
        alias: [
            "equals2", ["System$Collections$Generic$IEqualityComparer$1$System$Collections$Generic$SortedSet$1$" + H5.getTypeAlias(T) + "$equals2", "System$Collections$Generic$IEqualityComparer$1$equals2"],
            "getHashCode2", ["System$Collections$Generic$IEqualityComparer$1$System$Collections$Generic$SortedSet$1$" + H5.getTypeAlias(T) + "$getHashCode2", "System$Collections$Generic$IEqualityComparer$1$getHashCode2"]
        ],
        ctors: {
            ctor: function () {
                System.Collections.Generic.SortedSetEqualityComparer$1(T).$ctor2.call(this, null, null);
            },
            $ctor1: function (comparer) {
                System.Collections.Generic.SortedSetEqualityComparer$1(T).$ctor2.call(this, comparer, null);
            },
            $ctor3: function (memberEqualityComparer) {
                System.Collections.Generic.SortedSetEqualityComparer$1(T).$ctor2.call(this, null, memberEqualityComparer);
            },
            $ctor2: function (comparer, memberEqualityComparer) {
                this.$initialize();
                if (comparer == null) {
                    this.comparer = new (System.Collections.Generic.Comparer$1(T))(System.Collections.Generic.Comparer$1.$default.fn);
                } else {
                    this.comparer = comparer;
                }
                if (memberEqualityComparer == null) {
                    this.e_comparer = System.Collections.Generic.EqualityComparer$1(T).def;
                } else {
                    this.e_comparer = memberEqualityComparer;
                }
            }
        },
        methods: {
            equals2: function (x, y) {
                return System.Collections.Generic.SortedSet$1(T).SortedSetEquals(x, y, this.comparer);
            },
            equals: function (obj) {
                var comparer;
                if (!(((comparer = H5.as(obj, System.Collections.Generic.SortedSetEqualityComparer$1(T)))) != null)) {
                    return false;
                }
                return (H5.referenceEquals(this.comparer, comparer.comparer));
            },
            getHashCode2: function (obj) {
                var $t;
                var hashCode = 0;
                if (obj != null) {
                    $t = H5.getEnumerator(obj);
                    try {
                        while ($t.moveNext()) {
                            var t = $t.Current;
                            hashCode = hashCode ^ (this.e_comparer[H5.geti(this.e_comparer, "System$Collections$Generic$IEqualityComparer$1$" + H5.getTypeAlias(T) + "$getHashCode2", "System$Collections$Generic$IEqualityComparer$1$getHashCode2")](t) & 2147483647);
                        }
                    } finally {
                        if (H5.is($t, System.IDisposable)) {
                            $t.System$IDisposable$Dispose();
                        }
                    }
                }
                return hashCode;
            },
            getHashCode: function () {
                return H5.getHashCode(this.comparer) ^ H5.getHashCode(this.e_comparer);
            }
        }
    }; });
