    HighFive.define("System.Collections.Generic.List$1.Enumerator", function (T) { return {
        inherits: [System.Collections.Generic.IEnumerator$1(T),System.Collections.IEnumerator],
        $kind: "nested struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.List$1.Enumerator(T))(); }
            }
        },
        fields: {
            list: null,
            index: 0,
            version: 0,
            current: HighFive.getDefaultValue(T)
        },
        props: {
            Current: {
                get: function () {
                    return this.current;
                }
            },
            System$Collections$IEnumerator$Current: {
                get: function () {
                    if (this.index === 0 || this.index === ((this.list._size + 1) | 0)) {
                        throw new System.InvalidOperationException.ctor();
                    }
                    return this.Current;
                }
            }
        },
        alias: [
            "Dispose", "System$IDisposable$Dispose",
            "moveNext", "System$Collections$IEnumerator$moveNext",
            "Current", ["System$Collections$Generic$IEnumerator$1$" + HighFive.getTypeAlias(T) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"]
        ],
        ctors: {
            $ctor1: function (list) {
                this.$initialize();
                this.list = list;
                this.index = 0;
                this.version = list._version;
                this.current = HighFive.getDefaultValue(T);
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            Dispose: function () { },
            moveNext: function () {

                var localList = this.list;

                if (this.version === localList._version && ((this.index >>> 0) < (localList._size >>> 0))) {
                    this.current = localList._items[System.Array.index(this.index, localList._items)];
                    this.index = (this.index + 1) | 0;
                    return true;
                }
                return this.MoveNextRare();
            },
            MoveNextRare: function () {
                if (this.version !== this.list._version) {
                    throw new System.InvalidOperationException.ctor();
                }

                this.index = (this.list._size + 1) | 0;
                this.current = HighFive.getDefaultValue(T);
                return false;
            },
            System$Collections$IEnumerator$reset: function () {
                if (this.version !== this.list._version) {
                    throw new System.InvalidOperationException.ctor();
                }

                this.index = 0;
                this.current = HighFive.getDefaultValue(T);
            },
            getHashCode: function () {
                var h = HighFive.addHash([3788985113, this.list, this.index, this.version, this.current]);
                return h;
            },
            equals: function (o) {
                if (!HighFive.is(o, System.Collections.Generic.List$1.Enumerator(T))) {
                    return false;
                }
                return HighFive.equals(this.list, o.list) && HighFive.equals(this.index, o.index) && HighFive.equals(this.version, o.version) && HighFive.equals(this.current, o.current);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.Generic.List$1.Enumerator(T))();
                s.list = this.list;
                s.index = this.index;
                s.version = this.version;
                s.current = this.current;
                return s;
            }
        }
    }; });
