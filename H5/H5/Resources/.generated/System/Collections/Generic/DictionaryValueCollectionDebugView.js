    H5.define("System.Collections.Generic.DictionaryValueCollectionDebugView$2", function (TKey, TValue) { return {
        fields: {
            _collection: null
        },
        props: {
            Items: {
                get: function () {
                    var items = System.Array.init(System.Array.getCount(this._collection, TValue), function (){
                        return H5.getDefaultValue(TValue);
                    }, TValue);
                    System.Array.copyTo(this._collection, items, 0, TValue);
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
