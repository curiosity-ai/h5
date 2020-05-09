    H5.define("System.MissingMethodException", {
        inherits: [System.Exception],
        ctors: {
            ctor: function () {
                this.$initialize();
                System.Exception.ctor.call(this, "Attempted to access a missing method.");
            },
            $ctor1: function (message) {
                this.$initialize();
                System.Exception.ctor.call(this, message);
            },
            $ctor2: function (message, inner) {
                this.$initialize();
                System.Exception.ctor.call(this, message, inner);
            },
            $ctor3: function (className, methodName) {
                this.$initialize();
                System.Exception.ctor.call(this, (className || "") + "." + (methodName || "") + " Due to: Attempted to access a missing member.");
            }
        }
    });
