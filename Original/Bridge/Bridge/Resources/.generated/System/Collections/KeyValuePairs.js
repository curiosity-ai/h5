    Bridge.define("System.Collections.KeyValuePairs", {
        fields: {
            key: null,
            value: null
        },
        props: {
            Key: {
                get: function () {
                    return this.key;
                }
            },
            Value: {
                get: function () {
                    return this.value;
                }
            }
        },
        ctors: {
            ctor: function (key, value) {
                this.$initialize();
                this.value = value;
                this.key = key;
            }
        }
    });
