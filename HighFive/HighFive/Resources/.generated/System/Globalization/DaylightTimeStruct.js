    HighFive.define("System.Globalization.DaylightTimeStruct", {
        $kind: "struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new System.Globalization.DaylightTimeStruct(); }
            }
        },
        fields: {
            Start: null,
            End: null,
            Delta: null
        },
        ctors: {
            init: function () {
                this.Start = System.DateTime.getDefaultValue();
                this.End = System.DateTime.getDefaultValue();
                this.Delta = new System.TimeSpan();
            },
            $ctor1: function (start, end, delta) {
                this.$initialize();
                this.Start = start;
                this.End = end;
                this.Delta = delta;
            },
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            getHashCode: function () {
                var h = HighFive.addHash([7445027511, this.Start, this.End, this.Delta]);
                return h;
            },
            equals: function (o) {
                if (!HighFive.is(o, System.Globalization.DaylightTimeStruct)) {
                    return false;
                }
                return HighFive.equals(this.Start, o.Start) && HighFive.equals(this.End, o.End) && HighFive.equals(this.Delta, o.Delta);
            },
            $clone: function (to) {
                var s = to || new System.Globalization.DaylightTimeStruct();
                s.Start = this.Start;
                s.End = this.End;
                s.Delta = this.Delta;
                return s;
            }
        }
    });
