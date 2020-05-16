    H5.define("System.ComponentModel.BrowsableAttribute", {
        inherits: [System.Attribute],
        statics: {
            fields: {
                yes: null,
                no: null,
                default: null
            },
            ctors: {
                init: function () {
                    this.yes = new System.ComponentModel.BrowsableAttribute(true);
                    this.no = new System.ComponentModel.BrowsableAttribute(false);
                    this.default = System.ComponentModel.BrowsableAttribute.yes;
                }
            }
        },
        fields: {
            browsable: false
        },
        props: {
            Browsable: {
                get: function () {
                    return this.browsable;
                }
            }
        },
        ctors: {
            init: function () {
                this.browsable = true;
            },
            ctor: function (browsable) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this.browsable = browsable;
            }
        },
        methods: {
            equals: function (obj) {
                if (H5.referenceEquals(obj, this)) {
                    return true;
                }
                var other;

                return (((other = H5.as(obj, System.ComponentModel.BrowsableAttribute))) != null) && other.Browsable === this.browsable;
            },
            getHashCode: function () {
                return H5.getHashCode(this.browsable);
            }
        }
    });
