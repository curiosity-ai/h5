    H5.define("System.Console", {
        statics: {
            methods: {
                write: function (value) {
                    System.Console.Write(System.DateTime.format(value));
                },
                write$1: function (value) {
                    System.Console.Write(value.toString());
                },
                Write: function (value) {
                    var con = H5.global.console;

                    if (con && con.log) {
                        con.log(!H5.isDefined(H5.unbox(value)) ? "" : H5.unbox(value));
                    }
                },
                writeLine: function (value) {
                    System.Console.WriteLine(System.DateTime.format(value));
                },
                writeLine$1: function (value) {
                    System.Console.WriteLine(value.toString());
                },
                WriteLine: function (value) {
                    var con = H5.global.console;

                    if (con && con.log) {
                        con.log(!H5.isDefined(H5.unbox(value)) ? "" : H5.unbox(value));
                    }
                },
                Log: function (value) {
                    var con = H5.global.console;

                    if (con && con.log) {
                        con.log(H5.unbox(value));
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
                    var con = H5.global.console;

                    if (con && con.clear) {
                        con.clear();
                    }
                }
            }
        }
    });
