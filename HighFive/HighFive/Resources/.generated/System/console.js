    HighFive.define("System.Console", {
        statics: {
            methods: {
                Write: function (value) {
                    var con = HighFive.global.console;

                    if (con && con.log) {
                        con.log(!HighFive.isDefined(HighFive.unbox(value)) ? "" : HighFive.unbox(value));
                    }
                },
                WriteLine: function (value) {
                    var con = HighFive.global.console;

                    if (con && con.log) {
                        con.log(!HighFive.isDefined(HighFive.unbox(value)) ? "" : HighFive.unbox(value));
                    }
                },
                TransformChars: function (buffer, all, index, count) {
                    if (all !== 1) {
                        if (buffer == null) {
                            throw new System.ArgumentNullException.$ctor1("buffer");
                        }

                        if (index < 0) {
                            throw new System.ArgumentOutOfRangeException.$ctor4("index", "less than zero");
                        }

                        if (count < 0) {
                            throw new System.ArgumentOutOfRangeException.$ctor4("count", "less than zero");
                        }

                        if (((index + count) | 0) > buffer.length) {
                            throw new System.ArgumentException.$ctor1("index plus count specify a position that is not within buffer.");
                        }
                    }

                    var s = "";
                    if (buffer != null) {
                        if (all === 1) {
                            index = 0;
                            count = buffer.length;
                        }

                        for (var i = index; i < ((index + count) | 0); i = (i + 1) | 0) {
                            s = (s || "") + String.fromCharCode(buffer[System.Array.index(i, buffer)]);
                        }
                    }

                    return s;
                },
                Clear: function () {
                    var con = HighFive.global.console;

                    if (con && con.clear) {
                        con.clear();
                    }
                }
            }
        }
    });
