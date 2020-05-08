    HighFive.define("System.ComponentModel.DefaultValueAttribute", {
        inherits: [System.Attribute],
        fields: {
            _value: null
        },
        props: {
            Value: {
                get: function () {
                    return this._value;
                }
            }
        },
        ctors: {
            $ctor11: function (type, value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                try {
                    if ((type.prototype instanceof System.Enum)) {
                        this._value = System.Enum.parse(type, value, true);
                    } else if (HighFive.referenceEquals(type, System.TimeSpan)) {
                        throw System.NotImplemented.ByDesign;
                    } else {
                        throw System.NotImplemented.ByDesign;
                    }
                } catch ($e1) {
                    $e1 = System.Exception.create($e1);
                }
            },
            $ctor2: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.Char, String.fromCharCode, System.Char.getHashCode);
            },
            $ctor1: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.Byte);
            },
            $ctor4: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.Int16);
            },
            $ctor5: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.Int32);
            },
            $ctor6: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = value;
            },
            $ctor9: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.Single, System.Single.format, System.Single.getHashCode);
            },
            $ctor3: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.Double, System.Double.format, System.Double.getHashCode);
            },
            ctor: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.Boolean, System.Boolean.toString);
            },
            $ctor10: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = value;
            },
            $ctor7: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = value;
            },
            $ctor8: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.SByte);
            },
            $ctor12: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.UInt16);
            },
            $ctor13: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = HighFive.box(value, System.UInt32);
            },
            $ctor14: function (value) {
                this.$initialize();
                System.Attribute.ctor.call(this);
                this._value = value;
            }
        },
        methods: {
            equals: function (obj) {
                if (HighFive.referenceEquals(obj, this)) {
                    return true;
                }

                var other = HighFive.as(obj, System.ComponentModel.DefaultValueAttribute);

                if (other != null) {
                    if (this.Value != null) {
                        return HighFive.equals(this.Value, other.Value);
                    } else {
                        return (other.Value == null);
                    }
                }
                return false;
            },
            getHashCode: function () {
                return HighFive.getHashCode(this);
            },
            setValue: function (value) {
                this._value = value;
            }
        }
    });
