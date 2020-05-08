    // module export
    if (typeof define === "function" && define.amd) {
        // AMD
        define("h5", [], function () { return H5; });
    } else if (typeof module !== "undefined" && module.exports) {
        // Node
        module.exports = H5;
    }
