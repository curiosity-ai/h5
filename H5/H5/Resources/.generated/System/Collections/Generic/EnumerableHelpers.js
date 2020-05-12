    H5.define("H5.Collections.EnumerableHelpers", {
        statics: {
            methods: {
                ToArray: function (T, source) {
                    var count = { };
                    var results = { v : H5.Collections.EnumerableHelpers.ToArray$1(T, source, count) };
                    System.Array.resize(results, count.v, function () {
                        return H5.getDefaultValue(T);
                    }, T);
                    return results.v;
                },
                ToArray$1: function (T, source, length) {
                    var en = H5.getEnumerator(source, T);
                    try {
                        if (en.System$Collections$IEnumerator$moveNext()) {
                            var DefaultCapacity = 4;
                            var arr = { v : System.Array.init(DefaultCapacity, function (){
                                return H5.getDefaultValue(T);
                            }, T) };
                            arr.v[System.Array.index(0, arr.v)] = en[H5.geti(en, "System$Collections$Generic$IEnumerator$1$" + H5.getTypeAlias(T) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")];
                            var count = 1;

                            while (en.System$Collections$IEnumerator$moveNext()) {
                                if (count === arr.v.length) {
                                    var MaxArrayLength = 2146435071;

                                    var newLength = count << 1;
                                    if ((newLength >>> 0) > MaxArrayLength) {
                                        newLength = MaxArrayLength <= count ? ((count + 1) | 0) : MaxArrayLength;
                                    }

                                    System.Array.resize(arr, newLength, function () {
                                        return H5.getDefaultValue(T);
                                    }, T);
                                }

                                arr.v[System.Array.index(H5.identity(count, ((count = (count + 1) | 0))), arr.v)] = en[H5.geti(en, "System$Collections$Generic$IEnumerator$1$" + H5.getTypeAlias(T) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")];
                            }

                            length.v = count;
                            return arr.v;
                        }
                    }
                    finally {
                        if (H5.hasValue(en)) {
                            en.System$IDisposable$Dispose();
                        }
                    }

                    length.v = 0;
                    return System.Array.init(0, function (){
                        return H5.getDefaultValue(T);
                    }, T);
                }
            }
        }
    });
