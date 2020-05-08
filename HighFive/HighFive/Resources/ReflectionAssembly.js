    HighFive.assembly = function (assemblyName, res, callback, restore) {
        if (!callback) {
            callback = res;
            res = {};
        }

        assemblyName = assemblyName || "HighFive.$Unknown";

        var asm = System.Reflection.Assembly.assemblies[assemblyName];

        if (!asm) {
            asm = new System.Reflection.Assembly(assemblyName, res);
        } else {
            HighFive.apply(asm.res, res || {});
        }

        var oldAssembly = HighFive.$currentAssembly;

        HighFive.$currentAssembly = asm;

        if (callback) {
            var old = HighFive.Class.staticInitAllow;
            HighFive.Class.staticInitAllow = false;

            callback.call(HighFive.global, asm, HighFive.global);

            HighFive.Class.staticInitAllow = old;
        }

        HighFive.init();

        if (restore) {
            HighFive.$currentAssembly = oldAssembly;
        }
    };

    HighFive.define("System.Reflection.Assembly", {
        statics: {
            assemblies: {}
        },

        ctor: function (name, res) {
            this.$initialize();
            this.name = name;
            this.res = res || {};
            this.$types = {};
            this.$ = {};

            System.Reflection.Assembly.assemblies[name] = this;
        },

        toString: function () {
            return this.name;
        },

        getManifestResourceNames: function () {
            return Object.keys(this.res);
        },

        getManifestResourceDataAsBase64: function (type, name) {
            if (arguments.length === 1) {
                name = type;
                type = null;
            }

            if (type) {
                name = HighFive.Reflection.getTypeNamespace(type) + "." + name;
            }

            return this.res[name] || null;
        },

        getManifestResourceData: function (type, name) {
            if (arguments.length === 1) {
                name = type;
                type = null;
            }

            if (type) {
                name = HighFive.Reflection.getTypeNamespace(type) + '.' + name;
            }

            var r = this.res[name];

            return r ? System.Convert.fromBase64String(r) : null;
        },

        getCustomAttributes: function (attributeType) {
            if (this.attr && attributeType && !HighFive.isBoolean(attributeType)) {
                return this.attr.filter(function (a) {
                    return HighFive.is(a, attributeType);
                });
            }

            return this.attr || [];
        }
    });

    HighFive.$currentAssembly = new System.Reflection.Assembly("mscorlib");
    HighFive.SystemAssembly = HighFive.$currentAssembly;
    HighFive.SystemAssembly.$types["System.Reflection.Assembly"] = System.Reflection.Assembly;
    System.Reflection.Assembly.$assembly = HighFive.SystemAssembly;

    var $asm = HighFive.$currentAssembly;
