    // module export
    if (typeof define === "function" && define.amd) {
        // AMD
        define("highfive", [], function () { return HighFive; });
    } else if (typeof module !== "undefined" && module.exports) {
        // Node
        module.exports = HighFive;
    }
