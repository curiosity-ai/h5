    H5.define("System.Collections.Generic.HashSet$1.Slot", function (T) { return {
        $kind: "nested struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new (System.Collections.Generic.HashSet$1.Slot(T))(); }
            }
        },
        fields: {
            hashCode: 0,
            value: H5.getDefaultValue(T),
            next: 0
        },
        ctors: {
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            getHashCode: function () {
                var h = H5.addHash([1953459283, this.hashCode, this.value, this.next]);
                return h;
            },
            equals: function (o) {
                if (!H5.is(o, System.Collections.Generic.HashSet$1.Slot(T))) {
                    return false;
                }
                return H5.equals(this.hashCode, o.hashCode) && H5.equals(this.value, o.value) && H5.equals(this.next, o.next);
            },
            $clone: function (to) {
                var s = to || new (System.Collections.Generic.HashSet$1.Slot(T))();
                s.hashCode = this.hashCode;
                s.value = this.value;
                s.next = this.next;
                return s;
            }
        }
    }; });
