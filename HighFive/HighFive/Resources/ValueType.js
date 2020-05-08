HighFive.define("System.ValueType", {
    statics: {
        methods: {
            $is: function (obj) {
                return HighFive.Reflection.isValueType(HighFive.getType(obj));
            }
        }
    }
});
