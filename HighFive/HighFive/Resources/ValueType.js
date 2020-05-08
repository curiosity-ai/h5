H5.define("System.ValueType", {
    statics: {
        methods: {
            $is: function (obj) {
                return H5.Reflection.isValueType(H5.getType(obj));
            }
        }
    }
});
