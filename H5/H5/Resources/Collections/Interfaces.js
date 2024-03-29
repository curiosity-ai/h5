    H5.define("System.Collections.IEnumerable", {
        $kind: "interface"
    });
    H5.define("System.Collections.ICollection", {
        inherits: [System.Collections.IEnumerable],
        $kind: "interface"
    });
    H5.define("System.Collections.IList", {
        inherits: [System.Collections.ICollection],
        $kind: "interface"
    });
    H5.define("System.Collections.IDictionary", {
        inherits: [System.Collections.ICollection],
        $kind: "interface"
    });

    H5.define("System.Collections.Generic.IEnumerable$1", function (T) {
        return {
            inherits: [System.Collections.IEnumerable],
            $kind: "interface",
            $variance: [1]
        };
    });

    H5.define("System.Collections.Generic.ICollection$1", function (T) {
        return {
            inherits: [System.Collections.Generic.IEnumerable$1(T)],
            $kind: "interface"
        };
    });

    H5.define("System.Collections.Generic.IEqualityComparer$1", function (T) {
        return {
            $kind: "interface",
            $variance: [2]
        };
    });

    H5.define("System.Collections.Generic.IDictionary$2", function (TKey, TValue) {
        return {
            inherits: [System.Collections.Generic.ICollection$1(System.Collections.Generic.KeyValuePair$2(TKey, TValue))],
            $kind: "interface"
        };
    });

    H5.define("System.Collections.Generic.IList$1", function (T) {
        return {
            inherits: [System.Collections.Generic.ICollection$1(T)],
            $kind: "interface"
        };
    });

    H5.define("System.Collections.Generic.ISet$1", function (T) {
        return {
            inherits: [System.Collections.Generic.ICollection$1(T)],
            $kind: "interface"
        };
    });

    H5.define("System.Collections.Generic.IReadOnlyCollection$1", function (T) {
        return {
            inherits: [System.Collections.Generic.IEnumerable$1(T)],
            $kind: "interface"
        };
    });

    H5.define("System.Collections.Generic.IReadOnlyList$1", function (T) {
        return {
            inherits: [System.Collections.Generic.IReadOnlyCollection$1(T)],
            $kind: "interface",
            $variance: [1]
        };
    });

    H5.define("System.Collections.Generic.IReadOnlyDictionary$2", function (TKey, TValue) {
        return {
            inherits: [System.Collections.Generic.IReadOnlyCollection$1(System.Collections.Generic.KeyValuePair$2(TKey, TValue))],
            $kind: "interface"
        };
    });
