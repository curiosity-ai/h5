    var scope = {};

    scope.convert = {
        typeCodes: {
            Empty: 0,
            Object: 1,
            DBNull: 2,
            Boolean: 3,
            Char: 4,
            SByte: 5,
            Byte: 6,
            Int16: 7,
            UInt16: 8,
            Int32: 9,
            UInt32: 10,
            Int64: 11,
            UInt64: 12,
            Single: 13,
            Double: 14,
            Decimal: 15,
            DateTime: 16,
            String: 18
        },

        convertTypes: [
            null,
            System.Object,
            null,
            System.Boolean,
            System.Char,
            System.SByte,
            System.Byte,
            System.Int16,
            System.UInt16,
            System.Int32,
            System.UInt32,
            System.Int64,
            System.UInt64,
            System.Single,
            System.Double,
            System.Decimal,
            System.DateTime,
            System.Object,
            System.String
        ],

        toBoolean: function (value, formatProvider) {
            value = H5.unbox(value, true);

            switch (typeof (value)) {
                case "boolean":
                    return value;

                case "number":
                    return value !== 0; // non-zero int/float value is always converted to True;

                case "string":
                    var lowCaseVal = value.toLowerCase().trim();

                    if (lowCaseVal === "true") {
                        return true;
                    } else if (lowCaseVal === "false") {
                        return false;
                    } else {
                        throw new System.FormatException.$ctor1("String was not recognized as a valid Boolean.");
                    }

                case "object":
                    if (value == null) {
                        return false;
                    }

                    if (value instanceof System.Decimal) {
                        return !value.isZero();
                    }

                    if (System.Int64.is64Bit(value)) {
                        return value.ne(0);
                    }

                    break;
            }

            // TODO: #822 When IConvertible is implemented, try it before throwing InvalidCastEx
            var typeCode = scope.internal.suggestTypeCode(value);
            scope.internal.throwInvalidCastEx(typeCode, scope.convert.typeCodes.Boolean);

            // try converting using IConvertible
            return scope.convert.convertToType(scope.convert.typeCodes.Boolean, value, formatProvider || null);
        },

        toChar: function (value, formatProvider, valueTypeCode) {
            var typeCodes = scope.convert.typeCodes,
                isChar = H5.is(value, System.Char);

            value = H5.unbox(value, true);

            if (value instanceof System.Decimal) {
                value = value.toFloat();
            }

            if (value instanceof System.Int64 || value instanceof System.UInt64) {
                value = value.toNumber();
            }

            var type = typeof (value);

            valueTypeCode = valueTypeCode || scope.internal.suggestTypeCode(value);

            if (valueTypeCode === typeCodes.String && value == null) {
                type = "string";
            }

            if (valueTypeCode !== typeCodes.Object || isChar) {
                switch (type) {
                    case "boolean":
                        scope.internal.throwInvalidCastEx(typeCodes.Boolean, typeCodes.Char);

                    case "number":
                        var isFloatingType = scope.internal.isFloatingType(valueTypeCode);

                        if (isFloatingType || value % 1 !== 0) {
                            scope.internal.throwInvalidCastEx(valueTypeCode, typeCodes.Char);
                        }

                        scope.internal.validateNumberRange(value, typeCodes.Char, true);

                        return value;

                    case "string":
                        if (value == null) {
                            throw new System.ArgumentNullException.$ctor1("value");
                        }

                        if (value.length !== 1) {
                            throw new System.FormatException.$ctor1("String must be exactly one character long.");
                        }

                        return value.charCodeAt(0);
                }
            }

            if (valueTypeCode === typeCodes.Object || type === "object") {
                if (value == null) {
                    return 0;
                }

                if (H5.isDate(value)) {
                    scope.internal.throwInvalidCastEx(typeCodes.DateTime, typeCodes.Char);
                }
            }

            // TODO: #822 When IConvertible is implemented, try it before throwing InvalidCastEx
            scope.internal.throwInvalidCastEx(valueTypeCode, scope.convert.typeCodes.Char);

            // try converting using IConvertible
            return scope.convert.convertToType(typeCodes.Char, value, formatProvider || null);
        },

        toSByte: function (value, formatProvider, valueTypeCode) {
            return scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.SByte, valueTypeCode || null);
        },

        toByte: function (value, formatProvider) {
            return scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.Byte);
        },

        toInt16: function (value, formatProvider) {
            return scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.Int16);
        },

        toUInt16: function (value, formatProvider) {
            return scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.UInt16);
        },

        toInt32: function (value, formatProvider) {
            return scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.Int32);
        },

        toUInt32: function (value, formatProvider) {
            return scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.UInt32);
        },

        toInt64: function (value, formatProvider) {
            var result = scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.Int64);
            return new System.Int64(result);
        },

        toUInt64: function (value, formatProvider) {
            var result = scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.UInt64);
            return new System.UInt64(result);
        },

        toSingle: function (value, formatProvider) {
            return scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.Single);
        },

        toDouble: function (value, formatProvider) {
            return scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.Double);
        },

        toDecimal: function (value, formatProvider) {
            if (value instanceof System.Decimal) {
                return value;
            }

            return new System.Decimal(scope.internal.toNumber(value, formatProvider || null, scope.convert.typeCodes.Decimal));
        },

        toDateTime: function (value, formatProvider) {
            var typeCodes = scope.convert.typeCodes;

            value = H5.unbox(value, true);

            switch (typeof (value)) {
                case "boolean":
                    scope.internal.throwInvalidCastEx(typeCodes.Boolean, typeCodes.DateTime);

                case "number":
                    var fromType = scope.internal.suggestTypeCode(value);
                    scope.internal.throwInvalidCastEx(fromType, typeCodes.DateTime);

                case "string":
                    value = System.DateTime.parse(value, formatProvider || null);

                    return value;

                case "object":
                    if (value == null) {
                        return scope.internal.getMinValue(typeCodes.DateTime);
                    }

                    if (H5.isDate(value)) {
                        return value;
                    }

                    if (value instanceof System.Decimal) {
                        scope.internal.throwInvalidCastEx(typeCodes.Decimal, typeCodes.DateTime);
                    }

                    if (value instanceof System.Int64) {
                        scope.internal.throwInvalidCastEx(typeCodes.Int64, typeCodes.DateTime);
                    }

                    if (value instanceof System.UInt64) {
                        scope.internal.throwInvalidCastEx(typeCodes.UInt64, typeCodes.DateTime);
                    }

                    break;
            }

            // TODO: #822 When IConvertible is implemented, try it before throwing InvalidCastEx
            var valueTypeCode = scope.internal.suggestTypeCode(value);

            scope.internal.throwInvalidCastEx(valueTypeCode, scope.convert.typeCodes.DateTime);

            // try converting using IConvertible
            return scope.convert.convertToType(typeCodes.DateTime, value, formatProvider || null);
        },

        toString: function (value, formatProvider, valueTypeCode) {
            if (value && value.$boxed) {
                return value.toString();
            }

            var typeCodes = scope.convert.typeCodes,
                type = typeof (value);

            switch (type) {
                case "boolean":
                    return value ? "True" : "False";

                case "number":
                    if ((valueTypeCode || null) === typeCodes.Char) {
                        return String.fromCharCode(value);
                    }

                    if (isNaN(value)) {
                        return "NaN";
                    }

                    if (value % 1 !== 0) {
                        value = parseFloat(value.toPrecision(15));
                    }

                    return value.toString();

                case "string":
                    return value;

                case "object":
                    if (value == null) {
                        return "";
                    }

                    // If the object has an override to the toString() method,
                    // then just return its result
                    if (value.toString !== Object.prototype.toString) {
                        return value.toString();
                    }

                    if (H5.isDate(value)) {
                        return System.DateTime.format(value, null, formatProvider || null);
                    }

                    if (value instanceof System.Decimal) {
                        if (value.isInteger()) {
                            return value.toFixed(0, 4);
                        }
                        return value.toPrecision(value.precision());
                    }

                    if (System.Int64.is64Bit(value)) {
                        return value.toString();
                    }

                    if (value.format) {
                        return value.format(null, formatProvider || null);
                    }

                    var typeName = H5.getTypeName(value);

                    return typeName;
            }

            // try converting using IConvertible
            return scope.convert.convertToType(scope.convert.typeCodes.String, value, formatProvider || null);
        },

        toNumberInBase: function (str, fromBase, typeCode) {
            if (fromBase !== 2 && fromBase !== 8 && fromBase !== 10 && fromBase !== 16) {
                throw new System.ArgumentException.$ctor1("Invalid Base.");
            }

            var typeCodes = scope.convert.typeCodes;

            if (str == null) {
                if (typeCode === typeCodes.Int64) {
                    return System.Int64.Zero;
                }

                if (typeCode === typeCodes.UInt64) {
                    return System.UInt64.Zero;
                }

                return 0;
            }

            if (str.length === 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("length", "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            // Let's process the string in lower case.
            str = str.toLowerCase();

            var minValue = scope.internal.getMinValue(typeCode),
                maxValue = scope.internal.getMaxValue(typeCode);

            // Calculate offset (start index)
            var isNegative = false,
                startIndex = 0;

            if (str[startIndex] === "-") {
                if (fromBase !== 10) {
                    throw new System.ArgumentException.$ctor1("String cannot contain a minus sign if the base is not 10.");
                }

                if (minValue >= 0) {
                    throw new System.OverflowException.$ctor1("The string was being parsed as an unsigned number and could not have a negative sign.");
                }

                isNegative = true;
                ++startIndex;
            } else if (str[startIndex] === "+") {
                ++startIndex;
            }

            if (fromBase === 16 && str.length >= 2 && str[startIndex] === "0" && str[startIndex + 1] === "x") {
                startIndex += 2;
            }

            // Fill allowed codes for the specified base:
            var allowedCodes;

            if (fromBase === 2) {
                allowedCodes = scope.internal.charsToCodes("01");
            } else if (fromBase === 8) {
                allowedCodes = scope.internal.charsToCodes("01234567");
            } else if (fromBase === 10) {
                allowedCodes = scope.internal.charsToCodes("0123456789");
            } else if (fromBase === 16) {
                allowedCodes = scope.internal.charsToCodes("0123456789abcdef");
            } else {
                throw new System.ArgumentException.$ctor1("Invalid Base.");
            }

            // Create charCode-to-Value map
            var codeValues = {};

            for (var i = 0; i < allowedCodes.length; i++) {
                var allowedCode = allowedCodes[i];

                codeValues[allowedCode] = i;
            }

            var firstAllowed = allowedCodes[0],
                lastAllowed = allowedCodes[allowedCodes.length - 1],
                res,
                totalMax,
                code,
                j;

            if (typeCode === typeCodes.Int64 || typeCode === typeCodes.UInt64) {
                for (j = startIndex; j < str.length; j++) {
                    code = str[j].charCodeAt(0);

                    if (!(code >= firstAllowed && code <= lastAllowed)) {
                        if (j === startIndex) {
                            throw new System.FormatException.$ctor1("Could not find any recognizable digits.");
                        } else {
                            throw new System.FormatException.$ctor1("Additional non-parsable characters are at the end of the string.");
                        }
                    }
                }

                var isSign = typeCode === typeCodes.Int64;

                if (isSign) {
                    res = new System.Int64(H5.$Long.fromString(str, false, fromBase));
                } else {
                    res = new System.UInt64(H5.$Long.fromString(str, true, fromBase));
                }

                if (res.toString(fromBase) !== System.String.trimStartZeros(str)) {
                    throw new System.OverflowException.$ctor1("Value was either too large or too small.");
                }

                return res;
            } else {
                // Parse the number:
                res = 0;
                totalMax = maxValue - minValue + 1;

                for (j = startIndex; j < str.length; j++) {
                    code = str[j].charCodeAt(0);

                    if (code >= firstAllowed && code <= lastAllowed) {
                        res *= fromBase;
                        res += codeValues[code];

                        if (res > scope.internal.typeRanges.Int64_MaxValue) {
                            throw new System.OverflowException.$ctor1("Value was either too large or too small.");
                        }
                    } else {
                        if (j === startIndex) {
                            throw new System.FormatException.$ctor1("Could not find any recognizable digits.");
                        } else {
                            throw new System.FormatException.$ctor1("Additional non-parsable characters are at the end of the string.");
                        }
                    }
                }

                if (isNegative) {
                    res *= -1;
                }

                if (res > maxValue && fromBase !== 10 && minValue < 0) {
                    // Assume that the value is negative, transform it:
                    res = res - totalMax;
                }

                if (res < minValue || res > maxValue) {
                    throw new System.OverflowException.$ctor1("Value was either too large or too small.");
                }

                return res;
            }
        },

        toStringInBase: function (value, toBase, typeCode) {
            var typeCodes = scope.convert.typeCodes;

            value = H5.unbox(value, true);

            if (toBase !== 2 && toBase !== 8 && toBase !== 10 && toBase !== 16) {
                throw new System.ArgumentException.$ctor1("Invalid Base.");
            }

            var minValue = scope.internal.getMinValue(typeCode),
                maxValue = scope.internal.getMaxValue(typeCode),
                special = System.Int64.is64Bit(value);

            if (special) {
                if (value.lt(minValue) || value.gt(maxValue)) {
                    throw new System.OverflowException.$ctor1("Value was either too large or too small for an unsigned byte.");
                }
            } else if (value < minValue || value > maxValue) {
                throw new System.OverflowException.$ctor1("Value was either too large or too small for an unsigned byte.");
            }

            // Handle negative numbers:
            var isNegative = false;

            if (special) {
                if (toBase === 10) {
                    return value.toString();
                } else {
                    return value.value.toUnsigned().toString(toBase);
                }
            } else if (value < 0) {
                if (toBase === 10) {
                    isNegative = true;
                    value *= -1;
                } else {
                    value = (maxValue + 1 - minValue) + value;
                }
            }

            // Fill allowed codes for the specified base:
            var allowedChars;

            if (toBase === 2) {
                allowedChars = "01";
            } else if (toBase === 8) {
                allowedChars = "01234567";
            } else if (toBase === 10) {
                allowedChars = "0123456789";
            } else if (toBase === 16) {
                allowedChars = "0123456789abcdef";
            } else {
                throw new System.ArgumentException.$ctor1("Invalid Base.");
            }

            // Fill Value-To-Char map:
            var charByValues = {},
                allowedCharArr = allowedChars.split(""),
                allowedChar;

            for (var i = 0; i < allowedCharArr.length; i++) {
                allowedChar = allowedCharArr[i];

                charByValues[i] = allowedChar;
            }

            // Parse the number:
            var res = "";

            if (value === 0 || (special && value.eq(0))) {
                res = "0";
            } else {
                var mod, char;

                if (special) {
                    while (value.gt(0)) {
                        mod = value.mod(toBase);
                        value = value.sub(mod).div(toBase);

                        char = charByValues[mod.toNumber()];
                        res += char;
                    }
                } else {
                    while (value > 0) {
                        mod = value % toBase;
                        value = (value - mod) / toBase;

                        char = charByValues[mod];
                        res += char;
                    }
                }
            }

            if (isNegative) {
                res += "-";
            }

            res = res.split("").reverse().join("");

            return res;
        },

        toBase64String: function (inArray, offset, length, options) {
            if (inArray == null) {
                throw new System.ArgumentNullException.$ctor1("inArray");
            }

            offset = offset || 0;
            length = length != null ? length : inArray.length;
            options = options || 0; // 0 - means "None", 1 - stands for "InsertLineBreaks"

            if (length < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("length", "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            if (offset < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offset", "Value must be positive.");
            }

            if (options < 0 || options > 1) {
                throw new System.ArgumentException.$ctor1("Illegal enum value.");
            }

            var inArrayLength = inArray.length;

            if (offset > (inArrayLength - length)) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offset", "Offset and length must refer to a position in the string.");
            }

            if (inArrayLength === 0) {
                return "";
            }

            var insertLineBreaks = (options === 1),
                strArrayLen = scope.internal.toBase64_CalculateAndValidateOutputLength(length, insertLineBreaks);

            var strArray = [];
            strArray.length = strArrayLen;

            scope.internal.convertToBase64Array(strArray, inArray, offset, length, insertLineBreaks);

            var str = strArray.join("");

            return str;
        },

        toBase64CharArray: function (inArray, offsetIn, length, outArray, offsetOut, options) {
            if (inArray == null) {
                throw new System.ArgumentNullException.$ctor1("inArray");
            }

            if (outArray == null) {
                throw new System.ArgumentNullException.$ctor1("outArray");
            }

            if (length < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("length", "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            if (offsetIn < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offsetIn", "Value must be positive.");
            }

            if (offsetOut < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offsetOut", "Value must be positive.");
            }

            options = options || 0; // 0 - means "None", 1 - stands for "InsertLineBreaks"

            if (options < 0 || options > 1) {
                throw new System.ArgumentException.$ctor1("Illegal enum value.");
            }

            var inArrayLength = inArray.length;

            if (offsetIn > inArrayLength - length) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offsetIn", "Offset and length must refer to a position in the string.");
            }

            if (inArrayLength === 0) {
                return 0;
            }

            var insertLineBreaks = options === 1,
                outArrayLength = outArray.length; //This is the maximally required length that must be available in the char array

            // Length of the char buffer required
            var numElementsToCopy = scope.internal.toBase64_CalculateAndValidateOutputLength(length, insertLineBreaks);

            if (offsetOut > (outArrayLength - numElementsToCopy)) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offsetOut", "Either offset did not refer to a position in the string, or there is an insufficient length of destination character array.");
            }

            var charsArr = [],
                charsArrLength = scope.internal.convertToBase64Array(charsArr, inArray, offsetIn, length, insertLineBreaks);

            scope.internal.charsToCodes(charsArr, outArray, offsetOut);

            return charsArrLength;
        },

        fromBase64String: function (s) {
            // "s" is an unfortunate parameter name, but we need to keep it for backward compat.

            if (s == null) {
                throw new System.ArgumentNullException.$ctor1("s");
            }

            var sChars = s.split(""),
                bytes = scope.internal.fromBase64CharPtr(sChars, 0, sChars.length);

            return bytes;
        },

        fromBase64CharArray: function (inArray, offset, length) {
            if (inArray == null) {
                throw new System.ArgumentNullException.$ctor1("inArray");
            }

            if (length < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("length", "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            if (offset < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offset", "Value must be positive.");
            }

            if (offset > (inArray.length - length)) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offset", "Offset and length must refer to a position in the string.");
            }

            var chars = scope.internal.codesToChars(inArray),
                bytes = scope.internal.fromBase64CharPtr(chars, offset, length);

            return bytes;
        },

        getTypeCode: function (t) {
            if (t == null) {
                return System.TypeCode.Object;
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
        },

        changeConversionType: function (value, conversionType, provider) {
            if (conversionType == null) {
                throw new System.ArgumentNullException.$ctor1("conversionType");
            }

            if (value == null) {
                if (H5.Reflection.isValueType(conversionType)) {
                    throw new System.InvalidCastException.$ctor1("Null object cannot be converted to a value type.");
                }
                return null;
            }

            var fromTypeCode = scope.convert.getTypeCode(H5.getType(value)),
                ic = H5.as(value, System.IConvertible);

            if (ic == null && fromTypeCode == System.TypeCode.Object) {
                if (H5.referenceEquals(H5.getType(value), conversionType)) {
                    return value;
                }
                throw new System.InvalidCastException.$ctor1("Cannot convert to IConvertible");
            }

            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Boolean, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toBoolean(value, provider) : ic.System$IConvertible$ToBoolean(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Char, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toChar(value, provider, fromTypeCode) : ic.System$IConvertible$ToChar(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.SByte, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toSByte(value, provider, fromTypeCode) : ic.System$IConvertible$ToSByte(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Byte, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toByte(value, provider) : ic.System$IConvertible$ToByte(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Int16, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toInt16(value, provider) : ic.System$IConvertible$ToInt16(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.UInt16, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toUInt16(value, provider) : ic.System$IConvertible$ToUInt16(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Int32, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toInt32(value, provider) : ic.System$IConvertible$ToInt32(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.UInt32, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toUInt32(value, provider) : ic.System$IConvertible$ToUInt32(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Int64, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toInt64(value, provider) : ic.System$IConvertible$ToInt64(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.UInt64, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toUInt64(value, provider) : ic.System$IConvertible$ToUInt64(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Single, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toSingle(value, provider) : ic.System$IConvertible$ToSingle(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Double, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toDouble(value, provider) : ic.System$IConvertible$ToDouble(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Decimal, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toDecimal(value, provider) : ic.System$IConvertible$ToDecimal(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.DateTime, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toDateTime(value, provider) : ic.System$IConvertible$ToDateTime(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.String, scope.convert.convertTypes)])) {
                return ic == null ? scope.convert.toString(value, provider, fromTypeCode) : ic.System$IConvertible$ToString(provider);
            }
            if (H5.referenceEquals(conversionType, scope.convert.convertTypes[System.Array.index(System.TypeCode.Object, scope.convert.convertTypes)])) {
                return value;
            }

            if (ic == null) {
                throw new System.InvalidCastException.$ctor1("Cannot convert to IConvertible");
            }

            return ic.System$IConvertible$ToType(conversionType, provider);
        },

        changeType: function (value, typeCode, formatProvider) {
            if (H5.isFunction(typeCode)) {
                return scope.convert.changeConversionType(value, typeCode, formatProvider);
            }

            if (value == null && (typeCode === System.TypeCode.Empty || typeCode === System.TypeCode.String || typeCode === System.TypeCode.Object)) {
                return null;
            }

            var fromTypeCode = scope.convert.getTypeCode(H5.getType(value)),
                v = H5.as(value, System.IConvertible);

            if (v == null && fromTypeCode == System.TypeCode.Object) {
                throw new System.InvalidCastException.$ctor1("Cannot convert to IConvertible");
            }

            switch (typeCode) {
                case System.TypeCode.Boolean:
                    return v == null ? scope.convert.toBoolean(value, formatProvider) : v.System$IConvertible$ToBoolean(provider);
                case System.TypeCode.Char:
                    return v == null ? scope.convert.toChar(value, formatProvider, fromTypeCode) : v.System$IConvertible$ToChar(provider);
                case System.TypeCode.SByte:
                    return v == null ? scope.convert.toSByte(value, formatProvider, fromTypeCode) : v.System$IConvertible$ToSByte(provider);
                case System.TypeCode.Byte:
                    return v == null ? scope.convert.toByte(value, formatProvider, fromTypeCode) : v.System$IConvertible$ToByte(provider);
                case System.TypeCode.Int16:
                    return v == null ? scope.convert.toInt16(value, formatProvider) : v.System$IConvertible$ToInt16(provider);
                case System.TypeCode.UInt16:
                    return v == null ? scope.convert.toUInt16(value, formatProvider) : v.System$IConvertible$ToUInt16(provider);
                case System.TypeCode.Int32:
                    return v == null ? scope.convert.toInt32(value, formatProvider) : v.System$IConvertible$ToInt32(provider);
                case System.TypeCode.UInt32:
                    return v == null ? scope.convert.toUInt32(value, formatProvider) : v.System$IConvertible$ToUInt32(provider);
                case System.TypeCode.Int64:
                    return v == null ? scope.convert.toInt64(value, formatProvider) : v.System$IConvertible$ToInt64(provider);
                case System.TypeCode.UInt64:
                    return v == null ? scope.convert.toUInt64(value, formatProvider) : v.System$IConvertible$ToUInt64(provider);
                case System.TypeCode.Single:
                    return v == null ? scope.convert.toSingle(value, formatProvider) : v.System$IConvertible$ToSingle(provider);
                case System.TypeCode.Double:
                    return v == null ? scope.convert.toDouble(value, formatProvider) : v.System$IConvertible$ToDouble(provider);
                case System.TypeCode.Decimal:
                    return v == null ? scope.convert.toDecimal(value, formatProvider) : v.System$IConvertible$ToDecimal(provider);
                case System.TypeCode.DateTime:
                    return v == null ? scope.convert.toDateTime(value, formatProvider) : v.System$IConvertible$ToDateTime(provider);
                case System.TypeCode.String:
                    return v == null ? scope.convert.toString(value, formatProvider, fromTypeCode) : v.System$IConvertible$ToString(provider);
                case System.TypeCode.Object:
                    return value;
                case System.TypeCode.DBNull:
                    throw new System.InvalidCastException.$ctor1("Cannot convert DBNull values");
                case System.TypeCode.Empty:
                    throw new System.InvalidCastException.$ctor1("Cannot convert Empty values");
                default:
                    throw new System.ArgumentException.$ctor1("Unknown type code");
            }
        }
    };

    scope.internal = {
        base64Table: [
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
            "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d",
            "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s",
            "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7",
            "8", "9", "+", "/", "="
        ],

        typeRanges: {
            Char_MinValue: 0,
            Char_MaxValue: 65535,

            Byte_MinValue: 0,
            Byte_MaxValue: 255,

            SByte_MinValue: -128,
            SByte_MaxValue: 127,

            Int16_MinValue: -32768,
            Int16_MaxValue: 32767,

            UInt16_MinValue: 0,
            UInt16_MaxValue: 65535,

            Int32_MinValue: -2147483648,
            Int32_MaxValue: 2147483647,

            UInt32_MinValue: 0,
            UInt32_MaxValue: 4294967295,

            Int64_MinValue: System.Int64.MinValue,
            Int64_MaxValue: System.Int64.MaxValue,

            UInt64_MinValue: System.UInt64.MinValue,
            UInt64_MaxValue: System.UInt64.MaxValue,

            Single_MinValue: -3.40282347e+38,
            Single_MaxValue: 3.40282347e+38,

            Double_MinValue: -1.7976931348623157e+308,
            Double_MaxValue: 1.7976931348623157e+308,

            Decimal_MinValue: System.Decimal.MinValue,
            Decimal_MaxValue: System.Decimal.MaxValue
        },

        base64LineBreakPosition: 76,

        getTypeCodeName: function (typeCode) {
            var typeCodes = scope.convert.typeCodes;

            if (scope.internal.typeCodeNames == null) {
                var names = {};

                for (var codeName in typeCodes) {
                    if (!typeCodes.hasOwnProperty(codeName)) {
                        continue;
                    }

                    var codeValue = typeCodes[codeName];

                    names[codeValue] = codeName;
                }
                scope.internal.typeCodeNames = names;
            }

            var name = scope.internal.typeCodeNames[typeCode];

            if (name == null) {
                throw System.ArgumentOutOfRangeException("typeCode", "The specified typeCode is undefined.");
            }

            return name;
        },

        suggestTypeCode: function (value) {
            var typeCodes = scope.convert.typeCodes,
                type = typeof (value);

            switch (type) {
                case "boolean":
                    return typeCodes.Boolean;

                case "number":
                    if (value % 1 !== 0) {
                        return typeCodes.Double;
                    }

                    return typeCodes.Int32;

                case "string":
                    return typeCodes.String;

                case "object":
                    if (H5.isDate(value)) {
                        return typeCodes.DateTime;
                    }

                    if (value != null) {
                        return typeCodes.Object;
                    }

                    break;
            }
            return null;
        },

        getMinValue: function (typeCode) {
            var typeCodes = scope.convert.typeCodes;

            switch (typeCode) {
                case typeCodes.Char:
                    return scope.internal.typeRanges.Char_MinValue;
                case typeCodes.SByte:
                    return scope.internal.typeRanges.SByte_MinValue;
                case typeCodes.Byte:
                    return scope.internal.typeRanges.Byte_MinValue;
                case typeCodes.Int16:
                    return scope.internal.typeRanges.Int16_MinValue;
                case typeCodes.UInt16:
                    return scope.internal.typeRanges.UInt16_MinValue;
                case typeCodes.Int32:
                    return scope.internal.typeRanges.Int32_MinValue;
                case typeCodes.UInt32:
                    return scope.internal.typeRanges.UInt32_MinValue;
                case typeCodes.Int64:
                    return scope.internal.typeRanges.Int64_MinValue;
                case typeCodes.UInt64:
                    return scope.internal.typeRanges.UInt64_MinValue;
                case typeCodes.Single:
                    return scope.internal.typeRanges.Single_MinValue;
                case typeCodes.Double:
                    return scope.internal.typeRanges.Double_MinValue;
                case typeCodes.Decimal:
                    return scope.internal.typeRanges.Decimal_MinValue;
                case typeCodes.DateTime:
                    return System.DateTime.getMinValue();

                default:
                    return null;
            }
        },

        getMaxValue: function (typeCode) {
            var typeCodes = scope.convert.typeCodes;

            switch (typeCode) {
                case typeCodes.Char:
                    return scope.internal.typeRanges.Char_MaxValue;
                case typeCodes.SByte:
                    return scope.internal.typeRanges.SByte_MaxValue;
                case typeCodes.Byte:
                    return scope.internal.typeRanges.Byte_MaxValue;
                case typeCodes.Int16:
                    return scope.internal.typeRanges.Int16_MaxValue;
                case typeCodes.UInt16:
                    return scope.internal.typeRanges.UInt16_MaxValue;
                case typeCodes.Int32:
                    return scope.internal.typeRanges.Int32_MaxValue;
                case typeCodes.UInt32:
                    return scope.internal.typeRanges.UInt32_MaxValue;
                case typeCodes.Int64:
                    return scope.internal.typeRanges.Int64_MaxValue;
                case typeCodes.UInt64:
                    return scope.internal.typeRanges.UInt64_MaxValue;
                case typeCodes.Single:
                    return scope.internal.typeRanges.Single_MaxValue;
                case typeCodes.Double:
                    return scope.internal.typeRanges.Double_MaxValue;
                case typeCodes.Decimal:
                    return scope.internal.typeRanges.Decimal_MaxValue;
                case typeCodes.DateTime:
                    return System.DateTime.getMaxValue();
                default:
                    throw new System.ArgumentOutOfRangeException.$ctor4("typeCode", "The specified typeCode is undefined.");
            }
        },

        isFloatingType: function (typeCode) {
            var typeCodes = scope.convert.typeCodes,
                isFloatingType =
                    typeCode === typeCodes.Single ||
                    typeCode === typeCodes.Double ||
                    typeCode === typeCodes.Decimal;

            return isFloatingType;
        },

        toNumber: function (value, formatProvider, typeCode, valueTypeCode) {
            value = H5.unbox(value, true);

            var typeCodes = scope.convert.typeCodes,
                type = typeof (value),
                isFloating = scope.internal.isFloatingType(typeCode);

            if (valueTypeCode === typeCodes.String) {
                type = "string";
            }

            if (System.Int64.is64Bit(value) || value instanceof System.Decimal) {
                type = "number";
            }

            switch (type) {
                case "boolean":
                    return value ? 1 : 0;

                case "number":
                    if (typeCode === typeCodes.Decimal) {
                        scope.internal.validateNumberRange(value, typeCode, true);

                        return new System.Decimal(value, formatProvider);
                    }

                    if (typeCode === typeCodes.Int64) {
                        scope.internal.validateNumberRange(value, typeCode, true);

                        return new System.Int64(value);
                    }

                    if (typeCode === typeCodes.UInt64) {
                        scope.internal.validateNumberRange(value, typeCode, true);

                        return new System.UInt64(value);
                    }

                    if (System.Int64.is64Bit(value)) {
                        value = value.toNumber();
                    } else if (value instanceof System.Decimal) {
                        value = value.toFloat();
                    }

                    if (!isFloating && (value % 1 !== 0)) {
                        value = scope.internal.roundToInt(value, typeCode);
                    }

                    if (isFloating) {
                        var minValue = scope.internal.getMinValue(typeCode),
                            maxValue = scope.internal.getMaxValue(typeCode);

                        if (value > maxValue) {
                            value = Infinity;
                        } else if (value < minValue) {
                            value = -Infinity;
                        }
                    }

                    scope.internal.validateNumberRange(value, typeCode, false);
                    return value;

                case "string":
                    if (value == null) {
                        if (formatProvider != null) {
                            throw new System.ArgumentNullException.$ctor3("String", "Value cannot be null.");
                        }

                        return 0;
                    }

                    if (isFloating) {
                        var nfInfo = (formatProvider || System.Globalization.CultureInfo.getCurrentCulture()).getFormat(System.Globalization.NumberFormatInfo),
                            point = nfInfo.numberDecimalSeparator;

                        if (typeCode === typeCodes.Decimal) {
                            if (!new RegExp("^[+-]?(\\d+|\\d+.|\\d*\\" + point +"\\d+)$").test(value)) {
                                if (!/^[+-]?[0-9]+$/.test(value)) {
                                    throw new System.FormatException.$ctor1("Input string was not in a correct format.");
                                }
                            }

                            value = new System.Decimal(value, formatProvider);
                        } else {
                            if (!new RegExp("^[-+]?[0-9]*\\" + point +"?[0-9]+([eE][-+]?[0-9]+)?$").test(value)) {
                                throw new System.FormatException.$ctor1("Input string was not in a correct format.");
                            }

                            value = H5.Int.parseFloat(value, formatProvider);
                        }
                    } else {
                        if (!/^[+-]?[0-9]+$/.test(value)) {
                            throw new System.FormatException.$ctor1("Input string was not in a correct format.");
                        }

                        var str = value;

                        if (typeCode === typeCodes.Int64) {
                            value = new System.Int64(value);

                            if (System.String.trimStartZeros(str) !== value.toString()) {
                                this.throwOverflow(scope.internal.getTypeCodeName(typeCode));
                            }
                        } else if (typeCode === typeCodes.UInt64) {
                            value = new System.UInt64(value);

                            if (System.String.trimStartZeros(str) !== value.toString()) {
                                this.throwOverflow(scope.internal.getTypeCodeName(typeCode));
                            }
                        } else {
                            value = parseInt(value, 10);
                        }
                    }

                    if (isNaN(value)) {
                        throw new System.FormatException.$ctor1("Input string was not in a correct format.");
                    }

                    scope.internal.validateNumberRange(value, typeCode, true);

                    return value;

                case "object":
                    if (value == null) {
                        return 0;
                    }

                    if (H5.isDate(value)) {
                        scope.internal.throwInvalidCastEx(scope.convert.typeCodes.DateTime, typeCode);
                    }

                    break;
            }

            // TODO: #822 When IConvertible is implemented, try it before throwing InvalidCastEx
            valueTypeCode = valueTypeCode || scope.internal.suggestTypeCode(value);
            scope.internal.throwInvalidCastEx(valueTypeCode, typeCode);

            // try converting using IConvertible
            return scope.convert.convertToType(typeCode, value, formatProvider);
        },

        validateNumberRange: function (value, typeCode, denyInfinity) {
            var typeCodes = scope.convert.typeCodes,
                minValue = scope.internal.getMinValue(typeCode),
                maxValue = scope.internal.getMaxValue(typeCode),
                typeName = scope.internal.getTypeCodeName(typeCode);

            if (typeCode === typeCodes.Single ||
                typeCode === typeCodes.Double) {
                if (!denyInfinity && (value === Infinity || value === -Infinity)) {
                    return;
                }
            }

            if (typeCode === typeCodes.Decimal || typeCode === typeCodes.Int64 || typeCode === typeCodes.UInt64) {
                if (typeCode === typeCodes.Decimal) {
                    if (!System.Int64.is64Bit(value)) {
                        if (minValue.gt(value) || maxValue.lt(value)) {
                            this.throwOverflow(typeName);
                        }
                    }

                    value = new System.Decimal(value);
                } else if (typeCode === typeCodes.Int64) {
                    if (value instanceof System.UInt64) {
                        if (value.gt(System.Int64.MaxValue)) {
                            this.throwOverflow(typeName);
                        }
                    } else if (value instanceof System.Decimal) {
                        if ((value.gt(new System.Decimal(maxValue)) || value.lt(new System.Decimal(minValue)))) {
                            this.throwOverflow(typeName);
                        }
                    } else if (!(value instanceof System.Int64)) {
                        if (minValue.toNumber() > value || maxValue.toNumber() < value) {
                            this.throwOverflow(typeName);
                        }
                    }

                    value = new System.Int64(value);
                } else if (typeCode === typeCodes.UInt64) {
                    if (value instanceof System.Int64) {
                        if (value.isNegative()) {
                            this.throwOverflow(typeName);
                        }
                    } else if (value instanceof System.Decimal) {
                        if ((value.gt(new System.Decimal(maxValue)) || value.lt(new System.Decimal(minValue)))) {
                            this.throwOverflow(typeName);
                        }
                    } else if (!(value instanceof System.UInt64)) {
                        if (minValue.toNumber() > value || maxValue.toNumber() < value) {
                            this.throwOverflow(typeName);
                        }
                    }

                    value = new System.UInt64(value);
                }
            } else if (value < minValue || value > maxValue) {
                this.throwOverflow(typeName);
            }
        },

        throwOverflow: function (typeName) {
            throw new System.OverflowException.$ctor1("Value was either too large or too small for '" + typeName + "'.");
        },

        roundToInt: function (value, typeCode) {
            if (value % 1 === 0) {
                return value;
            }

            var intPart;

            if (value >= 0) {
                intPart = Math.floor(value);
            } else {
                intPart = -1 * Math.floor(-value);
            }

            var floatPart = value - intPart,
                minValue = scope.internal.getMinValue(typeCode),
                maxValue = scope.internal.getMaxValue(typeCode);

            if (value >= 0.0) {
                if (value < (maxValue + 0.5)) {
                    if (floatPart > 0.5 || floatPart === 0.5 && (intPart & 1) !== 0) {
                        ++intPart;
                    }

                    return intPart;
                }
            } else if (value >= (minValue - 0.5)) {
                if (floatPart < -0.5 || floatPart === -0.5 && (intPart & 1) !== 0) {
                    --intPart;
                }

                return intPart;
            }

            var typeName = scope.internal.getTypeCodeName(typeCode);

            throw new System.OverflowException.$ctor1("Value was either too large or too small for an '" + typeName + "'.");
        },

        toBase64_CalculateAndValidateOutputLength: function (inputLength, insertLineBreaks) {
            var base64LineBreakPosition = scope.internal.base64LineBreakPosition,
                outlen = ~~(inputLength / 3) * 4; // the base length - we want integer division here.

            outlen += ((inputLength % 3) !== 0) ? 4 : 0; // at most 4 more chars for the remainder

            if (outlen === 0) {
                return 0;
            }

            if (insertLineBreaks) {
                var newLines = ~~(outlen / base64LineBreakPosition);

                if ((outlen % base64LineBreakPosition) === 0) {
                    --newLines;
                }

                outlen += newLines * 2; // the number of line break chars we'll add, "\r\n"
            }

            // If we overflow an int then we cannot allocate enough
            // memory to output the value so throw
            if (outlen > 2147483647) {
                throw new System.OutOfMemoryException();
            }

            return outlen;
        },

        convertToBase64Array: function (outChars, inData, offset, length, insertLineBreaks) {
            var base64Table = scope.internal.base64Table,
                base64LineBreakPosition = scope.internal.base64LineBreakPosition,
                lengthmod3 = length % 3,
                calcLength = offset + (length - lengthmod3),
                charCount = 0,
                j = 0;

            // Convert three bytes at a time to base64 notation.  This will consume 4 chars.
            var i;

            for (i = offset; i < calcLength; i += 3) {
                if (insertLineBreaks) {
                    if (charCount === base64LineBreakPosition) {
                        outChars[j++] = "\r";
                        outChars[j++] = "\n";
                        charCount = 0;
                    }

                    charCount += 4;
                }

                outChars[j] = base64Table[(inData[i] & 0xfc) >> 2];
                outChars[j + 1] = base64Table[((inData[i] & 0x03) << 4) | ((inData[i + 1] & 0xf0) >> 4)];
                outChars[j + 2] = base64Table[((inData[i + 1] & 0x0f) << 2) | ((inData[i + 2] & 0xc0) >> 6)];
                outChars[j + 3] = base64Table[(inData[i + 2] & 0x3f)];
                j += 4;
            }

            //Where we left off before
            i = calcLength;

            if (insertLineBreaks && (lengthmod3 !== 0) && (charCount === scope.internal.base64LineBreakPosition)) {
                outChars[j++] = "\r";
                outChars[j++] = "\n";
            }

            switch (lengthmod3) {
                case 2: //One character padding needed
                    outChars[j] = base64Table[(inData[i] & 0xfc) >> 2];
                    outChars[j + 1] = base64Table[((inData[i] & 0x03) << 4) | ((inData[i + 1] & 0xf0) >> 4)];
                    outChars[j + 2] = base64Table[(inData[i + 1] & 0x0f) << 2];
                    outChars[j + 3] = base64Table[64]; //Pad
                    j += 4;

                    break;

                case 1: // Two character padding needed
                    outChars[j] = base64Table[(inData[i] & 0xfc) >> 2];
                    outChars[j + 1] = base64Table[(inData[i] & 0x03) << 4];
                    outChars[j + 2] = base64Table[64]; //Pad
                    outChars[j + 3] = base64Table[64]; //Pad
                    j += 4;

                    break;
            }

            return j;
        },

        fromBase64CharPtr: function (input, offset, inputLength) {
            if (inputLength < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("inputLength", "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            if (offset < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("offset", "Value must be positive.");
            }

            // We need to get rid of any trailing white spaces.
            // Otherwise we would be rejecting input such as "abc= ":
            while (inputLength > 0) {
                var lastChar = input[offset + inputLength - 1];

                if (lastChar !== " " && lastChar !== "\n" && lastChar !== "\r" && lastChar !== "\t") {
                    break;
                }

                inputLength--;
            }

            // Compute the output length:
            var resultLength = scope.internal.fromBase64_ComputeResultLength(input, offset, inputLength);

            if (0 > resultLength) {
                throw new System.InvalidOperationException.$ctor1("Contract voilation: 0 <= resultLength.");
            }

            // resultLength can be zero. We will still enter FromBase64_Decode and process the input.
            // It may either simply write no bytes (e.g. input = " ") or throw (e.g. input = "ab").

            // Create result byte blob:
            var decodedBytes = [];
            decodedBytes.length = resultLength;

            // Convert Base64 chars into bytes:
            scope.internal.fromBase64_Decode(input, offset, inputLength, decodedBytes, 0, resultLength);

            // We are done:
            return decodedBytes;
        },

        fromBase64_Decode: function (input, inputIndex, inputLength, dest, destIndex, destLength) {
            var startDestIndex = destIndex;

            // You may find this method weird to look at. Its written for performance, not aesthetics.
            // You will find unrolled loops label jumps and bit manipulations.

            var intA = "A".charCodeAt(0),
                inta = "a".charCodeAt(0),
                int0 = "0".charCodeAt(0),
                intEq = "=".charCodeAt(0),
                intPlus = "+".charCodeAt(0),
                intSlash = "/".charCodeAt(0),
                intSpace = " ".charCodeAt(0),
                intTab = "\t".charCodeAt(0),
                intNLn = "\n".charCodeAt(0),
                intCRt = "\r".charCodeAt(0),
                intAtoZ = ("Z".charCodeAt(0) - "A".charCodeAt(0)),
                int0To9 = ("9".charCodeAt(0) - "0".charCodeAt(0));

            var endInputIndex = inputIndex + inputLength,
                endDestIndex = destIndex + destLength;

            // Current char code/value:
            var currCode;

            // This 4-byte integer will contain the 4 codes of the current 4-char group.
            // Eeach char codes for 6 bits = 24 bits.
            // The remaining byte will be FF, we use it as a marker when 4 chars have been processed.
            var currBlockCodes = 0x000000FF;

            var allInputConsumed = false,
                equalityCharEncountered = false;

            while (true) {
                // break when done:
                if (inputIndex >= endInputIndex) {
                    allInputConsumed = true;

                    break;
                }

                // Get current char:
                currCode = input[inputIndex].charCodeAt(0);
                inputIndex++;

                // Determine current char code (unsigned Int comparison):
                if (((currCode - intA) >>> 0) <= intAtoZ) {
                    currCode -= intA;
                } else if (((currCode - inta) >>> 0) <= intAtoZ) {
                    currCode -= (inta - 26);
                } else if (((currCode - int0) >>> 0) <= int0To9) {
                    currCode -= (int0 - 52);
                } else {
                    // Use the slower switch for less common cases:
                    switch (currCode) {
                        // Significant chars:
                        case intPlus:
                            currCode = 62;

                            break;

                        case intSlash:
                            currCode = 63;

                            break;

                            // Legal no-value chars (we ignore these):
                        case intCRt:
                        case intNLn:
                        case intSpace:
                        case intTab:
                            continue;

                            // The equality char is only legal at the end of the input.
                            // Jump after the loop to make it easier for the JIT register predictor to do a good job for the loop itself:
                        case intEq:
                            equalityCharEncountered = true;

                            break;

                            // Other chars are illegal:
                        default:
                            throw new System.FormatException.$ctor1("The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.");
                    }
                }

                if (equalityCharEncountered) {
                    break;
                }

                // Ok, we got the code. Save it:
                currBlockCodes = (currBlockCodes << 6) | currCode;

                // Last bit in currBlockCodes will be on after in shifted right 4 times:
                if ((currBlockCodes & 0x80000000) !== 0) {
                    if ((endDestIndex - destIndex) < 3) {
                        return -1;
                    }

                    dest[destIndex] = 0xFF & (currBlockCodes >> 16);
                    dest[destIndex + 1] = 0xFF & (currBlockCodes >> 8);
                    dest[destIndex + 2] = 0xFF & (currBlockCodes);
                    destIndex += 3;

                    currBlockCodes = 0x000000FF;
                }
            } // end of while

            if (!allInputConsumed && !equalityCharEncountered) {
                throw new System.InvalidOperationException.$ctor1("Contract violation: should never get here.");
            }

            if (equalityCharEncountered) {
                if (currCode !== intEq) {
                    throw new System.InvalidOperationException.$ctor1("Contract violation: currCode == intEq.");
                }

                // Recall that inputIndex is now one position past where '=' was read.
                // '=' can only be at the last input pos:
                if (inputIndex === endInputIndex) {
                    // Code is zero for trailing '=':
                    currBlockCodes <<= 6;

                    // The '=' did not complete a 4-group. The input must be bad:
                    if ((currBlockCodes & 0x80000000) === 0) {
                        throw new System.FormatException.$ctor1("Invalid length for a Base-64 char array or string.");
                    }

                    if ((endDestIndex - destIndex) < 2) {
                        // Autch! We underestimated the output length!
                        return -1;
                    }

                    // We are good, store bytes form this past group. We had a single "=", so we take two bytes:
                    dest[destIndex] = 0xFF & (currBlockCodes >> 16);
                    dest[destIndex + 1] = 0xFF & (currBlockCodes >> 8);
                    destIndex += 2;

                    currBlockCodes = 0x000000FF;
                } else { // '=' can also be at the pre-last position iff the last is also a '=' excluding the white spaces:
                    // We need to get rid of any intermediate white spaces.
                    // Otherwise we would be rejecting input such as "abc= =":
                    while (inputIndex < (endInputIndex - 1)) {
                        var lastChar = input[inputIndex];

                        if (lastChar !== " " && lastChar !== "\n" && lastChar !== "\r" && lastChar !== "\t") {
                            break;
                        }

                        inputIndex++;
                    }

                    if (inputIndex === (endInputIndex - 1) && input[inputIndex] === "=") {
                        // Code is zero for each of the two '=':
                        currBlockCodes <<= 12;

                        // The '=' did not complete a 4-group. The input must be bad:
                        if ((currBlockCodes & 0x80000000) === 0) {
                            throw new System.FormatException.$ctor1("Invalid length for a Base-64 char array or string.");
                        }

                        if ((endDestIndex - destIndex) < 1) {
                            // Autch! We underestimated the output length!
                            return -1;
                        }

                        // We are good, store bytes form this past group. We had a "==", so we take only one byte:
                        dest[destIndex] = 0xFF & (currBlockCodes >> 16);
                        destIndex++;

                        currBlockCodes = 0x000000FF;
                    } else {
                        // '=' is not ok at places other than the end:
                        throw new System.FormatException.$ctor1("The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.");
                    }
                }
            }

            // We get here either from above or by jumping out of the loop:
            // The last block of chars has less than 4 items
            if (currBlockCodes !== 0x000000FF) {
                throw new System.FormatException.$ctor1("Invalid length for a Base-64 char array or string.");
            }

            // Return how many bytes were actually recovered:
            return (destIndex - startDestIndex);
        },

        fromBase64_ComputeResultLength: function (input, startIndex, inputLength) {
            var intEq = "=",
                intSpace = " ";

            if (inputLength < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("inputLength", "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            var endIndex = startIndex + inputLength,
                usefulInputLength = inputLength,
                padding = 0;

            while (startIndex < endIndex) {
                var c = input[startIndex];

                startIndex++;

                // We want to be as fast as possible and filter out spaces with as few comparisons as possible.
                // We end up accepting a number of illegal chars as legal white-space chars.
                // This is ok: as soon as we hit them during actual decode we will recognise them as illegal and throw.
                if (c <= intSpace) {
                    usefulInputLength--;
                } else if (c === intEq) {
                    usefulInputLength--;
                    padding++;
                }
            }

            if (0 > usefulInputLength) {
                throw new System.InvalidOperationException.$ctor1("Contract violation: 0 <= usefulInputLength.");
            }

            if (0 > padding) {
                // For legal input, we can assume that 0 <= padding < 3. But it may be more for illegal input.
                // We will notice it at decode when we see a '=' at the wrong place.
                throw new System.InvalidOperationException.$ctor1("Contract violation: 0 <= padding.");
            }

            // Perf: reuse the variable that stored the number of '=' to store the number of bytes encoded by the
            // last group that contains the '=':
            if (padding !== 0) {
                if (padding === 1) {
                    padding = 2;
                } else if (padding === 2) {
                    padding = 1;
                } else {
                    throw new System.FormatException.$ctor1("The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.");
                }
            }

            // Done:
            return ~~(usefulInputLength / 4) * 3 + padding;
        },

        charsToCodes: function (chars, codes, codesOffset) {
            if (chars == null) {
                return null;
            }

            codesOffset = codesOffset || 0;

            if (codes == null) {
                codes = [];
                codes.length = chars.length;
            }

            for (var i = 0; i < chars.length; i++) {
                codes[i + codesOffset] = chars[i].charCodeAt(0);
            }

            return codes;
        },

        codesToChars: function (codes, chars) {
            if (codes == null) {
                return null;
            }

            chars = chars || [];

            for (var i = 0; i < codes.length; i++) {
                var code = codes[i];

                chars[i] = String.fromCharCode(code);
            }

            return chars;
        },

        throwInvalidCastEx: function (fromTypeCode, toTypeCode) {
            var fromType = scope.internal.getTypeCodeName(fromTypeCode),                toType = scope.internal.getTypeCodeName(toTypeCode);

            throw new System.InvalidCastException.$ctor1("Invalid cast from '" + fromType + "' to '" + toType + "'.");
        }
    };

    System.Convert = scope.convert;
