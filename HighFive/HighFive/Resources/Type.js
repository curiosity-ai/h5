H5.define("System.Type", {

    statics: {
        $is: function (instance) {
            return instance && instance.constructor === Function;
        },

        getTypeCode: function (t) {
            if (t == null) {
                return System.TypeCode.Empty;
            }
            if (t === System.Double) {
                return System.TypeCode.Double;
            }
            if (t === System.Single) {
                return System.TypeCode.Single;
            }
            if (t === System.Decimal) {
                return System.TypeCode.Decimal;
            }
            if (t === System.Byte) {
                return System.TypeCode.Byte;
            }
            if (t === System.SByte) {
                return System.TypeCode.SByte;
            }
            if (t === System.UInt16) {
                return System.TypeCode.UInt16;
            }
            if (t === System.Int16) {
                return System.TypeCode.Int16;
            }
            if (t === System.UInt32) {
                return System.TypeCode.UInt32;
            }
            if (t === System.Int32) {
                return System.TypeCode.Int32;
            }
            if (t === System.UInt64) {
                return System.TypeCode.UInt64;
            }
            if (t === System.Int64) {
                return System.TypeCode.Int64;
            }
            if (t === System.Boolean) {
                return System.TypeCode.Boolean;
            }
            if (t === System.Char) {
                return System.TypeCode.Char;
            }
            if (t === System.DateTime) {
                return System.TypeCode.DateTime;
            }
            if (t === System.String) {
                return System.TypeCode.String;
            }
            return System.TypeCode.Object;
        }
    }
});