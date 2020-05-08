    HighFive.define("HighFive.Collections.EnumerableHelpers", {
        statics: {
            methods: {
                ToArray: function (T, source) {
                    var count = { };
                    var results = { v : HighFive.Collections.EnumerableHelpers.ToArray$1(T, source, count) };
                    System.Array.resize(results, count.v, function () {
                        return HighFive.getDefaultValue(T);
                    }, T);
                    return results.v;
                },
                ToArray$1: function (T, source, length) {
                    var en = HighFive.getEnumerator(source, T);
                    try {
                        if (en.System$Collections$IEnumerator$moveNext()) {
                            var DefaultCapacity = 4;
                            var arr = { v : System.Array.init(DefaultCapacity, function (){
                                return HighFive.getDefaultValue(T);
                            }, T) };
                            arr.v[System.Array.index(0, arr.v)] = en[HighFive.geti(en, "System$Collections$Generic$IEnumerator$1$" + HighFive.getTypeAlias(T) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")];
                            var count = 1;

                            while (en.System$Collections$IEnumerator$moveNext()) {
                                if (count === arr.v.length) {
                                    var MaxArrayLength = 2146435071;

                                    var newLength = count << 1;
                                    if ((newLength >>> 0) > MaxArrayLength) {
                                        newLength = MaxArrayLength <= count ? ((count + 1) | 0) : MaxArrayLength;
                                    }

                                    System.Array.resize(arr, newLength, function () {
                                        return HighFive.getDefaultValue(T);
                                    }, T);
                                }

                                arr.v[System.Array.index(HighFive.identity(count, ((count = (count + 1) | 0))), arr.v)] = en[HighFive.geti(en, "System$Collections$Generic$IEnumerator$1$" + HighFive.getTypeAlias(T) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1")];
                            }

                            length.v = count;
                            return arr.v;
                        }
                    }
                    finally {
                        if (HighFive.hasValue(en)) {
                            en.System$IDisposable$Dispose();
                        }
                    }

                    length.v = 0;
                    return System.Array.init(0, function (){
                        return HighFive.getDefaultValue(T);
                    }, T);
                }
            }
        }
    });
