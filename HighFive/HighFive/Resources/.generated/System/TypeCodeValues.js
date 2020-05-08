    HighFive.define("System.TypeCodeValues", {
        statics: {
            fields: {
                Empty: null,
                Object: null,
                DBNull: null,
                Boolean: null,
                Char: null,
                SByte: null,
                Byte: null,
                Int16: null,
                UInt16: null,
                Int32: null,
                UInt32: null,
                Int64: null,
                UInt64: null,
                Single: null,
                Double: null,
                Decimal: null,
                DateTime: null,
                String: null
            },
            ctors: {
                init: function () {
                    this.Empty = "0";
                    this.Object = "1";
                    this.DBNull = "2";
                    this.Boolean = "3";
                    this.Char = "4";
                    this.SByte = "5";
                    this.Byte = "6";
                    this.Int16 = "7";
                    this.UInt16 = "8";
                    this.Int32 = "9";
                    this.UInt32 = "10";
                    this.Int64 = "11";
                    this.UInt64 = "12";
                    this.Single = "13";
                    this.Double = "14";
                    this.Decimal = "15";
                    this.DateTime = "16";
                    this.String = "18";
                }
            }
        }
    });
