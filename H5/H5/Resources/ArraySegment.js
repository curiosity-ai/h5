    H5.define("System.ArraySegment", {
        $kind: "struct",

        statics: {
            getDefaultValue: function () {
                return new System.ArraySegment();
            }
        },

        ctor: function (array, offset, count) {
            this.$initialize();

            if (arguments.length === 0) {
                this.array = null;
                this.offset = 0;
                this.count = 0;

                return;
            }

            if (array == null) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            this.array = array;

            if (H5.isNumber(offset)) {
                if (offset < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("offset");
                }

                this.offset = offset;
            } else {
                this.offset = 0;
            }

            if (H5.isNumber(count)) {
                if (count < 0) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("count");
                }

                this.count = count;
            } else {
                this.count = array.length;
            }

            if (array.length - this.offset < this.count) {
                throw new ArgumentException();
            }
        },

        getArray: function () {
            return this.array;
        },

        getCount: function () {
            return this.count;
        },

        getOffset: function () {
            return this.offset;
        },

        getHashCode: function () {
            var h = H5.addHash([5322976039, this.array, this.count, this.offset]);

            return h;
        },

        equals: function (o) {
            if (!H5.is(o, System.ArraySegment)) {
                return false;
            }

            return H5.equals(this.array, o.array) && H5.equals(this.count, o.count) && H5.equals(this.offset, o.offset);
        },

        $clone: function (to) { return this; }
    });
