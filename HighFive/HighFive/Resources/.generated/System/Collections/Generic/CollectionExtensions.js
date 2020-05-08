    HighFive.define("System.Collections.Generic.CollectionExtensions", {
        statics: {
            methods: {
                GetValueOrDefault: function (TKey, TValue, dictionary, key) {
                    return System.Collections.Generic.CollectionExtensions.GetValueOrDefault$1(TKey, TValue, dictionary, key, HighFive.getDefaultValue(TValue));
                },
                GetValueOrDefault$1: function (TKey, TValue, dictionary, key, defaultValue) {
                    if (dictionary == null) {
                        throw new System.ArgumentNullException.$ctor1("dictionary");
                    }

                    var value = { };
                    return dictionary["System$Collections$Generic$IReadOnlyDictionary$2$" + HighFive.getTypeAlias(TKey) + "$" + HighFive.getTypeAlias(TValue) + "$tryGetValue"](key, value) ? value.v : defaultValue;
                },
                TryAdd: function (TKey, TValue, dictionary, key, value) {
                    if (dictionary == null) {
                        throw new System.ArgumentNullException.$ctor1("dictionary");
                    }

                    if (!dictionary["System$Collections$Generic$IDictionary$2$" + HighFive.getTypeAlias(TKey) + "$" + HighFive.getTypeAlias(TValue) + "$containsKey"](key)) {
                        dictionary["System$Collections$Generic$IDictionary$2$" + HighFive.getTypeAlias(TKey) + "$" + HighFive.getTypeAlias(TValue) + "$add"](key, value);
                        return true;
                    }

                    return false;
                },
                Remove: function (TKey, TValue, dictionary, key, value) {
                    if (dictionary == null) {
                        throw new System.ArgumentNullException.$ctor1("dictionary");
                    }

                    if (dictionary["System$Collections$Generic$IDictionary$2$" + HighFive.getTypeAlias(TKey) + "$" + HighFive.getTypeAlias(TValue) + "$tryGetValue"](key, value)) {
                        dictionary["System$Collections$Generic$IDictionary$2$" + HighFive.getTypeAlias(TKey) + "$" + HighFive.getTypeAlias(TValue) + "$remove"](key);
                        return true;
                    }

                    value.v = HighFive.getDefaultValue(TValue);
                    return false;
                }
            }
        }
    });
