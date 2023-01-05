    H5.define("System.Collections.Generic.CollectionExtensions", {
        statics: {
            methods: {
                GetValueOrDefault$1: function (TKey, TValue, dictionary, key) {
                    return System.Collections.Generic.CollectionExtensions.GetValueOrDefault(TKey, TValue, dictionary, key, H5.getDefaultValue(TValue));
                },
                GetValueOrDefault: function (TKey, TValue, dictionary, key, defaultValue) {
                    if (dictionary == null) {
                        throw new System.ArgumentNullException.$ctor1("dictionary");
                    }

                    var value = { };
                    return dictionary["System$Collections$Generic$IReadOnlyDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$tryGetValue"](key, value) ? value.v : defaultValue;
                },
                TryAdd: function (TKey, TValue, dictionary, key, value) {
                    if (dictionary == null) {
                        throw new System.ArgumentNullException.$ctor1("dictionary");
                    }

                    if (!dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$containsKey"](key)) {
                        dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$add"](key, value);
                        return true;
                    }

                    return false;
                },
                Remove: function (TKey, TValue, dictionary, key, value) {
                    if (dictionary == null) {
                        throw new System.ArgumentNullException.$ctor1("dictionary");
                    }

                    if (dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$tryGetValue"](key, value)) {
                        dictionary["System$Collections$Generic$IDictionary$2$" + H5.getTypeAlias(TKey) + "$" + H5.getTypeAlias(TValue) + "$remove"](key);
                        return true;
                    }

                    value.v = H5.getDefaultValue(TValue);
                    return false;
                }
            }
        }
    });
