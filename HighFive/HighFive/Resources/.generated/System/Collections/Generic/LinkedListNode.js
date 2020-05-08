    HighFive.define("System.Collections.Generic.LinkedListNode$1", function (T) { return {
        fields: {
            list: null,
            next: null,
            prev: null,
            item: HighFive.getDefaultValue(T)
        },
        props: {
            List: {
                get: function () {
                    return this.list;
                }
            },
            Next: {
                get: function () {
                    return this.next == null || HighFive.referenceEquals(this.next, this.list.head) ? null : this.next;
                }
            },
            Previous: {
                get: function () {
                    return this.prev == null || HighFive.referenceEquals(this, this.list.head) ? null : this.prev;
                }
            },
            Value: {
                get: function () {
                    return this.item;
                },
                set: function (value) {
                    this.item = value;
                }
            }
        },
        ctors: {
            ctor: function (value) {
                this.$initialize();
                this.item = value;
            },
            $ctor1: function (list, value) {
                this.$initialize();
                this.list = list;
                this.item = value;
            }
        },
        methods: {
            Invalidate: function () {
                this.list = null;
                this.next = null;
                this.prev = null;
            }
        }
    }; });
