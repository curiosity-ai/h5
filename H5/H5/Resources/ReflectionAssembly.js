    H5.assembly = function (assemblyName, res, callback, restore) {
        if (!callback) {
            callback = res;
            res = {};
        }

        assemblyName = assemblyName || "H5.$Unknown";

        var asm = System.Reflection.Assembly.assemblies[assemblyName];

        if (!asm) {
            asm = new System.Reflection.Assembly(assemblyName, res);
        } else {
            H5.apply(asm.res, res || {});
        }

        var oldAssembly = H5.$currentAssembly;

        H5.$currentAssembly = asm;

        if (callback) {
            var old = H5.Class.staticInitAllow;
            H5.Class.staticInitAllow = false;

            callback.call(H5.global, asm, H5.global);

            H5.Class.staticInitAllow = old;
        }

        H5.init();

        if (restore) {
            H5.$currentAssembly = oldAssembly;
        }
    };

    H5.define("System.Reflection.Assembly", {
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
                name = H5.Reflection.getTypeNamespace(type) + "." + name;
            }

            return this.res[name] || null;
        },

        getManifestResourceData: function (type, name) {
            if (arguments.length === 1) {
                name = type;
                type = null;
            }

            if (type) {
                name = H5.Reflection.getTypeNamespace(type) + '.' + name;
            }

            var r = this.res[name];

            return r ? System.Convert.fromBase64String(r) : null;
        },

        getCustomAttributes: function (attributeType) {
            if (this.attr && attributeType && !H5.isBoolean(attributeType)) {
                return this.attr.filter(function (a) {
                    return H5.is(a, attributeType);
                });
            }

            return this.attr || [];
        }
    });

    H5.$currentAssembly = new System.Reflection.Assembly("mscorlib");
    H5.SystemAssembly = H5.$currentAssembly;
    H5.SystemAssembly.$types["System.Reflection.Assembly"] = System.Reflection.Assembly;
    System.Reflection.Assembly.$assembly = H5.SystemAssembly;

    var $asm = H5.$currentAssembly;
