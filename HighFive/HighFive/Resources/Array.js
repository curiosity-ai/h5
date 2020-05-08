    var array = {
        toIndex: function (arr, indices) {
            if (indices.length !== (arr.$s ? arr.$s.length : 1)) {
                throw new System.ArgumentException.$ctor1("Invalid number of indices");
            }

            if (indices[0] < 0 || indices[0] >= (arr.$s ? arr.$s[0] : arr.length)) {
                throw new System.IndexOutOfRangeException.$ctor1("Index 0 out of range");
            }

            var idx = indices[0],
                i;

            if (arr.$s) {
                for (i = 1; i < arr.$s.length; i++) {
                    if (indices[i] < 0 || indices[i] >= arr.$s[i]) {
                        throw new System.IndexOutOfRangeException.$ctor1("Index " + i + " out of range");
                    }

                    idx = idx * arr.$s[i] + indices[i];
                }
            }

            return idx;
        },

        index: function (index, arr) {
            if (index < 0 || index >= arr.length) {
                throw new System.IndexOutOfRangeException();
            }
            return index;
        },

        $get: function (indices) {
            var r = this[System.Array.toIndex(this, indices)];

            return typeof r !== "undefined" ? r : this.$v;
        },

        get: function (arr) {
            if (arguments.length < 2) {
                throw new System.ArgumentNullException.$ctor1("indices");
            }

            var idx = Array.prototype.slice.call(arguments, 1);

            for (var i = 0; i < idx.length; i++) {
                if (!HighFive.hasValue(idx[i])) {
                    throw new System.ArgumentNullException.$ctor1("indices");
                }
            }

            var r = arr[System.Array.toIndex(arr, idx)];

            return typeof r !== "undefined" ? r : arr.$v;
        },

        $set: function (indices, value) {
            this[System.Array.toIndex(this, indices)] = value;
        },

        set: function (arr, value) {
            var indices = Array.prototype.slice.call(arguments, 2);

            arr[System.Array.toIndex(arr, indices)] = value;
        },

        getLength: function (arr, dimension) {
            if (dimension < 0 || dimension >= (arr.$s ? arr.$s.length : 1)) {
                throw new System.IndexOutOfRangeException();
            }

            return arr.$s ? arr.$s[dimension] : arr.length;
        },

        getRank: function (arr) {
            return arr.$type ? arr.$type.$rank : (arr.$s ? arr.$s.length : 1);
        },

        getLower: function (arr, d) {
            System.Array.getLength(arr, d);

            return 0;
        },

        create: function (defvalue, initValues, T, sizes) {
            if (sizes === null) {
                throw new System.ArgumentNullException.$ctor1("length");
            }

            var arr = [],
                length = arguments.length > 3 ? 1 : 0,
                i, s, v, j,
                idx,
                indices,
                flatIdx;

            arr.$v = defvalue;
            arr.$s = [];
            arr.get = System.Array.$get;
            arr.set = System.Array.$set;

            if (sizes && HighFive.isArray(sizes)) {
                for (i = 0; i < sizes.length; i++) {
                    j = sizes[i];

                    if (isNaN(j) || j < 0) {
                        throw new System.ArgumentOutOfRangeException.$ctor1("length");
                    }

                    length *= j;
                    arr.$s[i] = j;
                }
            } else {
                for (i = 3; i < arguments.length; i++) {
                    j = arguments[i];

                    if (isNaN(j) || j < 0) {
                        throw new System.ArgumentOutOfRangeException.$ctor1("length");
                    }

                    length *= j;
                    arr.$s[i - 3] = j;
                }
            }

            arr.length = length;
            var isFn = HighFive.isFunction(defvalue);

            if (isFn) {
                var v = defvalue();

                if (!v || (!v.$kind && typeof v !== "object")) {
                    isFn = false;
                    defvalue = v;
                }
            }

            for (var k = 0; k < length; k++) {
                arr[k] = isFn ? defvalue() : defvalue;
            }

            if (initValues) {
                for (i = 0; i < arr.length; i++) {
                    indices = [];
                    flatIdx = i;

                    for (s = arr.$s.length - 1; s >= 0; s--) {
                        idx = flatIdx % arr.$s[s];
                        indices.unshift(idx);
                        flatIdx = HighFive.Int.div(flatIdx - idx, arr.$s[s]);
                    }

                    v = initValues;

                    for (idx = 0; idx < indices.length; idx++) {
                        v = v[indices[idx]];
                    }

                    arr[i] = v;
                }
            }

            System.Array.init(arr, T, arr.$s.length);

            return arr;
        },

        init: function (length, value, T, addFn) {
            if (length == null) {
                throw new System.ArgumentNullException.$ctor1("length");
            }

            if (HighFive.isArray(length)) {
                var elementType = value,
                    rank = T || 1;

                System.Array.type(elementType, rank, length);

                return length;
            }

            if (isNaN(length) || length < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor1("length");
            }

            var arr = new Array(length),
                isFn = addFn !== true && HighFive.isFunction(value);

            if (isFn) {
                var v = value();

                if (!v || (!v.$kind && typeof v !== "object")) {
                    isFn = false;
                    value = v;
                }
            }

            for (var i = 0; i < length; i++) {
                arr[i] = isFn ? value() : value;
            }

            return System.Array.init(arr, T, 1);
        },

        toEnumerable: function (array) {
            return new HighFive.ArrayEnumerable(array);
        },

        toEnumerator: function (array, T) {
            return new HighFive.ArrayEnumerator(array, T);
        },

        _typedArrays: {
            Float32Array: System.Single,
            Float64Array: System.Double,
            Int8Array: System.SByte,
            Int16Array: System.Int16,
            Int32Array: System.Int32,
            Uint8Array: System.Byte,
            Uint8ClampedArray: System.Byte,
            Uint16Array: System.UInt16,
            Uint32Array: System.UInt32
        },

        is: function (obj, type) {
            if (obj instanceof HighFive.ArrayEnumerator) {
                if ((obj.constructor === type) || (obj instanceof type) ||
                    type === HighFive.ArrayEnumerator ||
                    type.$$name && System.String.startsWith(type.$$name, "System.Collections.IEnumerator") ||
                    type.$$name && System.String.startsWith(type.$$name, "System.Collections.Generic.IEnumerator")) {
                    return true;
                }

                return false;
            }

            if (!HighFive.isArray(obj)) {
                return false;
            }

            if (type.$elementType && type.$isArray) {
                var et = HighFive.getType(obj).$elementType;

                if (et) {

                    if (HighFive.Reflection.isValueType(et) !== HighFive.Reflection.isValueType(type.$elementType)) {
                        return false;
                    }

                    return System.Array.getRank(obj) === type.$rank && HighFive.Reflection.isAssignableFrom(type.$elementType, et);
                }

                type = Array;
            }

            if ((obj.constructor === type) || (obj instanceof type)) {
                return true;
            }

            if (type === System.Collections.IEnumerable ||
                type === System.Collections.ICollection ||
                type === System.ICloneable ||
                type === System.Collections.IList ||
                type.$$name && System.String.startsWith(type.$$name, "System.Collections.Generic.IEnumerable$1") ||
                type.$$name && System.String.startsWith(type.$$name, "System.Collections.Generic.ICollection$1") ||
                type.$$name && System.String.startsWith(type.$$name, "System.Collections.Generic.IList$1") ||
                type.$$name && System.String.startsWith(type.$$name, "System.Collections.Generic.IReadOnlyCollection$1") ||
                type.$$name && System.String.startsWith(type.$$name, "System.Collections.Generic.IReadOnlyList$1")) {
                return true;
            }

            var isTypedArray = !!System.Array._typedArrays[String.prototype.slice.call(Object.prototype.toString.call(obj), 8, -1)];

            if (isTypedArray && !!System.Array._typedArrays[type.name]) {
                return obj instanceof type;
            }

            return isTypedArray;
        },

        clone: function (arr) {
            var newArr;

            if (arr.length === 1) {
                newArr = [arr[0]];
            } else {
                newArr = arr.slice(0);
            }

            newArr.$type = arr.$type;
            newArr.$v = arr.$v;
            newArr.$s = arr.$s;
            newArr.get = System.Array.$get;
            newArr.set = System.Array.$set;

            return newArr;
        },

        getCount: function (obj, T) {
            var name,
                v;

            if (HighFive.isArray(obj)) {
                return obj.length;
            } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$getCount"])) {
                return obj[name]();
            } else if (HighFive.isFunction(obj[name = "System$Collections$ICollection$getCount"])) {
                return obj[name]();
            } else if (T && (v = obj["System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$Count"]) !== undefined) {
                return v;
            } else if ((v = obj["System$Collections$ICollection$Count"]) !== undefined) {
                return v;
            } else if ((v = obj.Count) !== undefined) {
                return v;
            } else if (HighFive.isFunction(obj.getCount)) {
                return obj.getCount();
            }

            return 0;
        },

        getIsReadOnly: function (obj, T) {
            var name,
                v;

            if (HighFive.isArray(obj)) {
                return T ? true : false;
            } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$getIsReadOnly"])) {
                return obj[name]();
            } else if (T && (v = obj["System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$IsReadOnly"]) !== undefined) {
                return v;
            } else if (HighFive.isFunction(obj[name = "System$Collections$IList$getIsReadOnly"])) {
                return obj[name]();
            } else if ((v = obj["System$Collections$IList$IsReadOnly"]) !== undefined) {
                return v;
            } else if ((v = obj.IsReadOnly) !== undefined) {
                return v;
            } else if (HighFive.isFunction(obj.getIsReadOnly)) {
                return obj.getIsReadOnly();
            }

            return false;
        },

        checkReadOnly: function (obj, T, msg) {
            if (HighFive.isArray(obj)) {
                if (T) {
                    throw new System.NotSupportedException.$ctor1(msg || "Collection was of a fixed size.");
                }
            } else if (System.Array.getIsReadOnly(obj, T)) {
                throw new System.NotSupportedException.$ctor1(msg || "Collection is read-only.");
            }
        },

        add: function (obj, item, T) {
            var name;

            System.Array.checkReadOnly(obj, T);

            if (T) {
                item = System.Array.checkNewElementType(item, T);
            }

            if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$add"])) {
                return obj[name](item);
            } else if (HighFive.isFunction(obj[name = "System$Collections$IList$add"])) {
                return obj[name](item);
            } else if (HighFive.isFunction(obj.add)) {
                return obj.add(item);
            }

            return -1;
        },

        checkNewElementType: function (v, type) {
            var unboxed = HighFive.unbox(v, true);

            if (HighFive.isNumber(unboxed)) {
                if (type === System.Decimal) {
                    return new System.Decimal(unboxed);
                }

                if (type === System.Int64) {
                    return new System.Int64(unboxed);
                }

                if (type === System.UInt64) {
                    return new System.UInt64(unboxed);
                }
            }

            var is = HighFive.is(v, type);

            if (!is) {
                if (v == null && HighFive.getDefaultValue(type) == null) {
                    return null;
                }

                throw new System.ArgumentException.$ctor1("The value " + unboxed + "is not of type " + HighFive.getTypeName(type) + " and cannot be used in this generic collection.");
            }

            return unboxed;
        },

        clear: function (obj, T) {
            var name;

            System.Array.checkReadOnly(obj, T, "Collection is read-only.");

            if (HighFive.isArray(obj)) {
                System.Array.fill(obj, T ? (T.getDefaultValue || HighFive.getDefaultValue(T)) : null, 0, obj.length);
            } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$clear"])) {
                obj[name]();
            } else if (HighFive.isFunction(obj[name = "System$Collections$IList$clear"])) {
                obj[name]();
            } else if (HighFive.isFunction(obj.clear)) {
                obj.clear();
            }
        },

        fill: function (dst, val, index, count) {
            if (!HighFive.hasValue(dst)) {
                throw new System.ArgumentNullException.$ctor1("dst");
            }

            if (index < 0 || count < 0 || (index + count) > dst.length) {
                throw new System.IndexOutOfRangeException();
            }

            var isFn = HighFive.isFunction(val);

            if (isFn) {
                var v = val();

                if (!v || (!v.$kind && typeof v !== "object")) {
                    isFn = false;
                    val = v;
                }
            }

            while (--count >= 0) {
                dst[index + count] = isFn ? val() : val;
            }
        },

        copy: function (src, spos, dest, dpos, len) {
            if (!dest) {
                throw new System.ArgumentNullException.$ctor3("dest", "Value cannot be null");
            }

            if (!src) {
                throw new System.ArgumentNullException.$ctor3("src", "Value cannot be null");
            }

            if (spos < 0 || dpos < 0 || len < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor1("bound", "Number was less than the array's lower bound in the first dimension");
            }

            if (len > (src.length - spos) || len > (dest.length - dpos)) {
                throw new System.ArgumentException.$ctor1("Destination array was not long enough. Check destIndex and length, and the array's lower bounds");
            }

            if (spos < dpos && src === dest) {
                while (--len >= 0) {
                    dest[dpos + len] = src[spos + len];
                }
            } else {
                for (var i = 0; i < len; i++) {
                    dest[dpos + i] = src[spos + i];
                }
            }
        },

        copyTo: function (obj, dest, index, T) {
            var name;

            if (HighFive.isArray(obj)) {
                System.Array.copy(obj, 0, dest, index, obj ? obj.length : 0);
            } else if (HighFive.isFunction(obj.copyTo)) {
                obj.copyTo(dest, index);
            } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$copyTo"])) {
                obj[name](dest, index);
            } else if (HighFive.isFunction(obj[name = "System$Collections$ICollection$copyTo"])) {
                obj[name](dest, index);
            } else {
                throw new System.NotImplementedException.$ctor1("copyTo");
            }
        },

        indexOf: function (arr, item, startIndex, count, T) {
            var name;

            if (HighFive.isArray(arr)) {
                var i,
                    el,
                    endIndex;

                startIndex = startIndex || 0;
                count = HighFive.isNumber(count) ? count : arr.length;
                endIndex = startIndex + count;

                for (i = startIndex; i < endIndex; i++) {
                    el = arr[i];

                    if (el === item || System.Collections.Generic.EqualityComparer$1.$default.equals2(el, item)) {
                        return i;
                    }
                }
            } else if (T && HighFive.isFunction(arr[name = "System$Collections$Generic$IList$1$" + HighFive.getTypeAlias(T) + "$indexOf"])) {
                return arr[name](item);
            } else if (HighFive.isFunction(arr[name = "System$Collections$IList$indexOf"])) {
                return arr[name](item);
            } else if (HighFive.isFunction(arr.indexOf)) {
                return arr.indexOf(item);
            }

            return -1;
        },

        contains: function (obj, item, T) {
            var name;

            if (HighFive.isArray(obj)) {
                return System.Array.indexOf(obj, item) > -1;
            } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$contains"])) {
                return obj[name](item);
            } else if (HighFive.isFunction(obj[name = "System$Collections$IList$contains"])) {
                return obj[name](item);
            } else if (HighFive.isFunction(obj.contains)) {
                return obj.contains(item);
            }

            return false;
        },

        remove: function (obj, item, T) {
            var name;

            System.Array.checkReadOnly(obj, T);

            if (HighFive.isArray(obj)) {
                var index = System.Array.indexOf(obj, item);

                if (index > -1) {
                    obj.splice(index, 1);

                    return true;
                }
            } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$ICollection$1$" + HighFive.getTypeAlias(T) + "$remove"])) {
                return obj[name](item);
            } else if (HighFive.isFunction(obj[name = "System$Collections$IList$remove"])) {
                return obj[name](item);
            } else if (HighFive.isFunction(obj.remove)) {
                return obj.remove(item);
            }

            return false;
        },

        insert: function (obj, index, item, T) {
            var name;

            System.Array.checkReadOnly(obj, T);

            if (T) {
                item = System.Array.checkNewElementType(item, T);
            }

            if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$IList$1$" + HighFive.getTypeAlias(T) + "$insert"])) {
                obj[name](index, item);
            } else if (HighFive.isFunction(obj[name = "System$Collections$IList$insert"])) {
                obj[name](index, item);
            } else if (HighFive.isFunction(obj.insert)) {
                obj.insert(index, item);
            }
        },

        removeAt: function (obj, index, T) {
            var name;

            System.Array.checkReadOnly(obj, T);

            if (HighFive.isArray(obj)) {
                obj.splice(index, 1);
            } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$IList$1$" + HighFive.getTypeAlias(T) + "$removeAt"])) {
                obj[name](index);
            } else if (HighFive.isFunction(obj[name = "System$Collections$IList$removeAt"])) {
                obj[name](index);
            } else if (HighFive.isFunction(obj.removeAt)) {
                obj.removeAt(index);
            }
        },

        getItem: function (obj, idx, T) {
            var name,
                v;

            if (HighFive.isArray(obj)) {
                v = obj[idx];
                if (T) {
                    return v;
                }

                return (obj.$type && (HighFive.isNumber(v) || HighFive.isBoolean(v) || HighFive.isDate(v))) ? HighFive.box(v, obj.$type.$elementType) : v;
            } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$IList$1$" + HighFive.getTypeAlias(T) + "$getItem"])) {
                v = obj[name](idx);
                return v;
            } else if (HighFive.isFunction(obj.get)) {
                v = obj.get(idx);
            } else if (HighFive.isFunction(obj.getItem)) {
                v = obj.getItem(idx);
            } else if (HighFive.isFunction(obj[name = "System$Collections$IList$$getItem"])) {
                v = obj[name](idx);
            } else if (HighFive.isFunction(obj.get_Item)) {
                v = obj.get_Item(idx);
            }

            return T && HighFive.getDefaultValue(T) != null ? HighFive.box(v, T) : v;
        },

        setItem: function (obj, idx, value, T) {
            var name;

            if (HighFive.isArray(obj)) {
                if (obj.$type) {
                    value = System.Array.checkElementType(value, obj.$type.$elementType);
                }

                obj[idx] = value;
            } else {
                if (T) {
                    value = System.Array.checkElementType(value, T);
                }

                if (HighFive.isFunction(obj.set)) {
                    obj.set(idx, value);
                } else if (HighFive.isFunction(obj.setItem)) {
                    obj.setItem(idx, value);
                } else if (T && HighFive.isFunction(obj[name = "System$Collections$Generic$IList$1$" + HighFive.getTypeAlias(T) + "$setItem"])) {
                    return obj[name](idx, value);
                } else if (T && HighFive.isFunction(obj[name = "System$Collections$IList$setItem"])) {
                    return obj[name](idx, value);
                } else if (HighFive.isFunction(obj.set_Item)) {
                    obj.set_Item(idx, value);
                }
            }
        },

        checkElementType: function (v, type) {
            var unboxed = HighFive.unbox(v, true);

            if (HighFive.isNumber(unboxed)) {
                if (type === System.Decimal) {
                    return new System.Decimal(unboxed);
                }

                if (type === System.Int64) {
                    return new System.Int64(unboxed);
                }

                if (type === System.UInt64) {
                    return new System.UInt64(unboxed);
                }
            }

            var is = HighFive.is(v, type);

            if (!is) {
                if (v == null) {
                    return HighFive.getDefaultValue(type);
                }

                throw new System.ArgumentException.$ctor1("Cannot widen from source type to target type either because the source type is a not a primitive type or the conversion cannot be accomplished.");
            }

            return unboxed;
        },

        resize: function (arr, newSize, val, T) {
            if (newSize < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor3("newSize", newSize, "newSize cannot be less than 0.");
            }

            var oldSize = 0,
                isFn = HighFive.isFunction(val),
                ref = arr.v;

            if (isFn) {
                var v = val();

                if (!v || (!v.$kind && typeof v !== "object")) {
                    isFn = false;
                    val = v;
                }
            }

            if (!ref) {
                ref = System.Array.init(new Array(newSize), T);
            } else {
                oldSize = ref.length;
                ref.length = newSize;
            }

            for (var i = oldSize; i < newSize; i++) {
                ref[i] = isFn ? val() : val;
            }

            ref.$s = [ref.length];

            arr.v = ref;
        },

        reverse: function (arr, index, length) {
            if (!array) {
                throw new System.ArgumentNullException.$ctor1("arr");
            }

            if (!index && index !== 0) {
                index = 0;
                length = arr.length;
            }

            if (index < 0 || length < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4((index < 0 ? "index" : "length"), "Non-negative number required.");
            }

            if ((array.length - index) < length) {
                throw new System.ArgumentException.$ctor1("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }

            if (System.Array.getRank(arr) !== 1) {
                throw new System.Exception("Only single dimension arrays are supported here.");
            }

            var i = index,
                j = index + length - 1;

            while (i < j) {
                var temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
                i++;
                j--;
            }
        },

        binarySearch: function (array, index, length, value, comparer) {
            if (!array) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            var lb = 0;

            if (index < lb || length < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4(index < lb ? "index" : "length", "Non-negative number required.");
            }

            if (array.length - (index - lb) < length) {
                throw new System.ArgumentException.$ctor1("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }

            if (System.Array.getRank(array) !== 1) {
                throw new System.RankException.$ctor1("Only single dimensional arrays are supported for the requested action.");
            }

            if (!comparer) {
                comparer = System.Collections.Generic.Comparer$1.$default;
            }

            var lo = index,
                hi = index + length - 1,
                i,
                c;

            while (lo <= hi) {
                i = lo + ((hi - lo) >> 1);

                try {
                    c = System.Collections.Generic.Comparer$1.get(comparer)(array[i], value);
                } catch (e) {
                    throw new System.InvalidOperationException.$ctor2("Failed to compare two elements in the array.", e);
                }

                if (c === 0) {
                    return i;
                }

                if (c < 0) {
                    lo = i + 1;
                } else {
                    hi = i - 1;
                }
            }

            return ~lo;
        },

        sortDict: function (keys, values, index, length, comparer) {
            if (!comparer) {
                comparer = System.Collections.Generic.Comparer$1.$default;
            }

            var list = [],
                fn = HighFive.fn.bind(comparer, System.Collections.Generic.Comparer$1.get(comparer));

            if (length == null) {
                length = keys.length;
            }

            for (var j = 0; j < keys.length; j++) {
                list.push({ key: keys[j], value: values[j] });
            }

            if (index === 0 && length === list.length) {
                list.sort(function (x, y) {
                    return fn(x.key, y.key);
                });
            } else {
                var newarray = list.slice(index, index + length);

                newarray.sort(function (x, y) {
                    return fn(x.key, y.key);
                });

                for (var i = index; i < (index + length); i++) {
                    list[i] = newarray[i - index];
                }
            }

            for (var k = 0; k < list.length; k++) {
                keys[k] = list[k].key;
                values[k] = list[k].value;
            }
        },

        sort: function (array, index, length, comparer) {
            if (!array) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (arguments.length === 2 && typeof index === "function") {
                array.sort(index);
                return;
            }

            if (arguments.length === 2 && typeof index === "object") {
                comparer = index;
                index = null;
            }

            if (!HighFive.isNumber(index)) {
                index = 0;
            }

            if (!HighFive.isNumber(length)) {
                length = array.length;
            }

            if (!comparer) {
                comparer = System.Collections.Generic.Comparer$1.$default;
            }

            if (index === 0 && length === array.length) {
                array.sort(HighFive.fn.bind(comparer, System.Collections.Generic.Comparer$1.get(comparer)));
            } else {
                var newarray = array.slice(index, index + length);

                newarray.sort(HighFive.fn.bind(comparer, System.Collections.Generic.Comparer$1.get(comparer)));

                for (var i = index; i < (index + length) ; i++) {
                    array[i] = newarray[i - index];
                }
            }
        },

        min: function (arr, minValue) {
            var min = arr[0],
                len = arr.length;

            for (var i = 0; i < len; i++) {
                if ((arr[i] < min || min < minValue) && !(arr[i] < minValue)) {
                    min = arr[i];
                }
            }

            return min;
        },

        max: function (arr, maxValue) {
            var max = arr[0],
                len = arr.length;

            for (var i = 0; i < len; i++) {
                if ((arr[i] > max || max > maxValue) && !(arr[i] > maxValue)) {
                    max = arr[i];
                }
            }

            return max;
        },

        addRange: function (arr, items) {
            if (HighFive.isArray(items)) {
                arr.push.apply(arr, items);
            } else {
                var e = HighFive.getEnumerator(items);

                try {
                    while (e.moveNext()) {
                        arr.push(e.Current);
                    }
                } finally {
                    if (HighFive.is(e, System.IDisposable)) {
                        e.Dispose();
                    }
                }
            }
        },

        convertAll: function (array, converter) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (!HighFive.hasValue(converter)) {
                throw new System.ArgumentNullException.$ctor1("converter");
            }

            var array2 = array.map(converter);

            return array2;
        },

        find: function (T, array, match) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (!HighFive.hasValue(match)) {
                throw new System.ArgumentNullException.$ctor1("match");
            }

            for (var i = 0; i < array.length; i++) {
                if (match(array[i])) {
                    return array[i];
                }
            }

            return HighFive.getDefaultValue(T);
        },

        findAll: function (array, match) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (!HighFive.hasValue(match)) {
                throw new System.ArgumentNullException.$ctor1("match");
            }

            var list = [];

            for (var i = 0; i < array.length; i++) {
                if (match(array[i])) {
                    list.push(array[i]);
                }
            }

            return list;
        },

        findIndex: function (array, startIndex, count, match) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (arguments.length === 2) {
                match = startIndex;
                startIndex = 0;
                count = array.length;
            } else if (arguments.length === 3) {
                match = count;
                count = array.length - startIndex;
            }

            if (startIndex < 0 || startIndex > array.length) {
                throw new System.ArgumentOutOfRangeException.$ctor1("startIndex");
            }

            if (count < 0 || startIndex > array.length - count) {
                throw new System.ArgumentOutOfRangeException.$ctor1("count");
            }

            if (!HighFive.hasValue(match)) {
                throw new System.ArgumentNullException.$ctor1("match");
            }

            var endIndex = startIndex + count;

            for (var i = startIndex; i < endIndex; i++) {
                if (match(array[i])) {
                    return i;
                }
            }

            return -1;
        },

        findLast: function (T, array, match) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (!HighFive.hasValue(match)) {
                throw new System.ArgumentNullException.$ctor1("match");
            }

            for (var i = array.length - 1; i >= 0; i--) {
                if (match(array[i])) {
                    return array[i];
                }
            }

            return HighFive.getDefaultValue(T);
        },

        findLastIndex: function (array, startIndex, count, match) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (arguments.length === 2) {
                match = startIndex;
                startIndex = array.length - 1;
                count = array.length;
            } else if (arguments.length === 3) {
                match = count;
                count = startIndex + 1;
            }

            if (!HighFive.hasValue(match)) {
                throw new System.ArgumentNullException.$ctor1("match");
            }

            if (array.length === 0) {
                if (startIndex !== -1) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("startIndex");
                }
            } else {
                if (startIndex < 0 || startIndex >= array.length) {
                    throw new System.ArgumentOutOfRangeException.$ctor1("startIndex");
                }
            }

            if (count < 0 || startIndex - count + 1 < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor1("count");
            }

            var endIndex = startIndex - count;

            for (var i = startIndex; i > endIndex; i--) {
                if (match(array[i])) {
                    return i;
                }
            }

            return -1;
        },

        forEach: function (array, action) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (!HighFive.hasValue(action)) {
                throw new System.ArgumentNullException.$ctor1("action");
            }

            for (var i = 0; i < array.length; i++) {
                action(array[i], i, array);
            }
        },

        indexOfT: function (array, value, startIndex, count) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (arguments.length === 2) {
                startIndex = 0;
                count = array.length;
            } else if (arguments.length === 3) {
                count = array.length - startIndex;
            }

            if (startIndex < 0 || (startIndex >= array.length && array.length > 0)) {
                throw new System.ArgumentOutOfRangeException.$ctor4("startIndex", "out of range");
            }

            if (count < 0 || count > array.length - startIndex) {
                throw new System.ArgumentOutOfRangeException.$ctor4("count", "out of range");
            }

            return System.Array.indexOf(array, value, startIndex, count);
        },

        isFixedSize: function (array) {
            if (HighFive.isArray(array)) {
                return true;
            } else if (array["System$Collections$IList$isFixedSize"] != null) {
                return array["System$Collections$IList$isFixedSize"];
            } else if(array["System$Collections$IList$IsFixedSize"] != null) {
                return array["System$Collections$IList$IsFixedSize"];
            } else if (array.isFixedSize != null) {
                return array.isFixedSize;
            } else if (array.IsFixedSize != null) {
                return array.IsFixedSize;
            }

            return true;
        },

        isSynchronized: function (array) {
            return false;
        },

        lastIndexOfT: function (array, value, startIndex, count) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (arguments.length === 2) {
                startIndex = array.length - 1;
                count = array.length;
            } else if (arguments.length === 3) {
                count = (array.length === 0) ? 0 : (startIndex + 1);
            }

            if (startIndex < 0 || (startIndex >= array.length && array.length > 0)) {
                throw new System.ArgumentOutOfRangeException.$ctor4("startIndex", "out of range");
            }

            if (count < 0 || startIndex - count + 1 < 0) {
                throw new System.ArgumentOutOfRangeException.$ctor4("count", "out of range");
            }

            var endIndex = startIndex - count + 1;

            for (var i = startIndex; i >= endIndex; i--) {
                var el = array[i];

                if (el === value || System.Collections.Generic.EqualityComparer$1.$default.equals2(el, value)) {
                    return i;
                }
            }

            return -1;
        },

        syncRoot: function (array) {
            return array;
        },

        trueForAll: function (array, match) {
            if (!HighFive.hasValue(array)) {
                throw new System.ArgumentNullException.$ctor1("array");
            }

            if (!HighFive.hasValue(match)) {
                throw new System.ArgumentNullException.$ctor1("match");
            }

            for (var i = 0; i < array.length; i++) {
                if (!match(array[i])) {
                    return false;
                }
            }

            return true;
        },

        type: function (t, rank, arr) {
            rank = rank || 1;

            var typeCache = System.Array.$cache[rank],
                result,
                name;

            if (!typeCache) {
                typeCache = [];
                System.Array.$cache[rank] = typeCache;
            }

            for (var i = 0; i < typeCache.length; i++) {
                if (typeCache[i].$elementType === t) {
                    result = typeCache[i];
                    break;
                }
            }

            if (!result) {
                name = HighFive.getTypeName(t) + "[" + System.String.fromCharCount(",".charCodeAt(0), rank - 1) + "]";

                var old = HighFive.Class.staticInitAllow;

                result = HighFive.define(name, {
                    $inherits: [System.Array, System.Collections.ICollection, System.ICloneable, System.Collections.Generic.IList$1(t), System.Collections.Generic.IReadOnlyCollection$1(t)],
                    $noRegister: true,
                    statics: {
                        $elementType: t,
                        $rank: rank,
                        $isArray: true,
                        $is: function (obj) {
                            return System.Array.is(obj, this);
                        },
                        getDefaultValue: function () {
                            return null;
                        },
                        createInstance: function () {
                            var arr;

                            if (this.$rank === 1) {
                                arr = [];
                            } else {
                                var args = [HighFive.getDefaultValue(this.$elementType), null, this.$elementType];

                                for (var j = 0; j < this.$rank; j++) {
                                    args.push(0);
                                }

                                arr = System.Array.create.apply(System.Array, args);
                            }

                            arr.$type = this;

                            return arr;
                        }
                    }
                });

                typeCache.push(result);

                HighFive.Class.staticInitAllow = true;

                if (result.$staticInit) {
                    result.$staticInit();
                }

                HighFive.Class.staticInitAllow = old;
            }

            if (arr) {
                arr.$type = result;
            }

            return arr || result;
        },
        getLongLength: function (array) {
            return System.Int64(array.length);
        }
    };

    HighFive.define("System.Array", {
        statics: array
    });

    System.Array.$cache = {};
