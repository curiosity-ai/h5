    Bridge.define("System.Collections.SortedList.SortedListDebugView", {
        $kind: "nested class",
        fields: {
            sortedList: null
        },
        props: {
            Items: {
                get: function () {
                    return this.sortedList.ToKeyValuePairsArray();
                }
            }
        },
        ctors: {
            ctor: function (sortedList) {
                this.$initialize();
                if (sortedList == null) {
                    throw new System.ArgumentNullException.$ctor1("sortedList");
                }

                this.sortedList = sortedList;
            }
        }
    });
