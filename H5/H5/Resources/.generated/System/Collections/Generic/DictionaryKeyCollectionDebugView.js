    H5.define("System.Collections.Generic.DictionaryKeyCollectionDebugView$2", function (TKey, TValue) { return {
        fields: {
            _collection: null
        },
        props: {
            Items: {
                get: function () {
                    var items = System.Array.init(System.Array.getCount(this._collection, TKey), function (){
                        return H5.getDefaultValue(TKey);
                    }, TKey);
                    System.Array.copyTo(this._collection, items, 0, TKey);
                    return items;
                }
            }
        },
        ctors: {
            ctor: function (collection) {
                this.$initialize();
                if (collection == null) {
                    throw new System.ArgumentNullException.$ctor1("collection");
                }

                this._collection = collection;
            }
        }
    }; });
