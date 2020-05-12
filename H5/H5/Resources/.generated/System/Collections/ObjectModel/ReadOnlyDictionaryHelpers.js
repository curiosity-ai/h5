    H5.define("System.Collections.ObjectModel.ReadOnlyDictionaryHelpers", {
        statics: {
            methods: {
                CopyToNonGenericICollectionHelper: function (T, collection, array, index) {
                    var $t;
                    if (array == null) {
                        throw new System.ArgumentNullException.$ctor1("array");
                    }

                    if (System.Array.getRank(array) !== 1) {
                        throw new System.ArgumentException.$ctor1("Only single dimensional arrays are supported for the requested action.");
                    }

                    if (System.Array.getLower(array, 0) !== 0) {
                        throw new System.ArgumentException.$ctor1("The lower bound of target array must be zero.");
                    }

                    if (index < 0) {
                        throw new System.ArgumentOutOfRangeException.$ctor4("index", "Index is less than zero.");
                    }

                    if (((array.length - index) | 0) < System.Array.getCount(collection, T)) {
                        throw new System.ArgumentException.$ctor1("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
                    }

                    var nonGenericCollection = H5.as(collection, System.Collections.ICollection);
                    if (nonGenericCollection != null) {
                        System.Array.copyTo(nonGenericCollection, array, index);
                        return;
                    }

                    var items = H5.as(array, System.Array.type(T));
                    if (items != null) {
                        System.Array.copyTo(collection, items, index, T);
                    } else {
                        /* 
                           FxOverRh: Type.IsAssignableNot() not an api on that platform.

                        //
                        // Catch the obvious case assignment will fail.
                        // We can found all possible problems by doing the check though.
                        // For example, if the element type of the Array is derived from T,
                        // we can't figure out if we can successfully copy the element beforehand.
                        //
                        Type targetType = array.GetType().GetElementType();
                        Type sourceType = typeof(T);
                        if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType))) {
                           throw new ArgumentException(SR.Argument_InvalidArrayType);
                        }
                        */

                        var objects = H5.as(array, System.Array.type(System.Object));
                        if (objects == null) {
                            throw new System.ArgumentException.$ctor1("Target array type is not compatible with the type of items in the collection.");
                        }

                        try {
                            $t = H5.getEnumerator(collection, T);
                            try {
                                while ($t.moveNext()) {
                                    var item = $t.Current;
                                    objects[System.Array.index(H5.identity(index, ((index = (index + 1) | 0))), objects)] = item;
                                }
                            } finally {
                                if (H5.is($t, System.IDisposable)) {
                                    $t.System$IDisposable$Dispose();
                                }
                            }
                        } catch ($e1) {
                            $e1 = System.Exception.create($e1);
                            if (H5.is($e1, System.ArrayTypeMismatchException)) {
                                throw new System.ArgumentException.$ctor1("Target array type is not compatible with the type of items in the collection.");
                            } else {
                                throw $e1;
                            }
                        }
                    }
                }
            }
        }
    });
