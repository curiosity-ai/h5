using System;
using Jint;
using H5.Compiler.Service.Next;
using Xunit;

namespace H5.Compiler.Next.Tests
{
    public class IntegrationTestRunner
    {
        private readonly H5Compiler _compiler = new H5Compiler();

        private string GetH5Mock()
        {
            return @"
            var globalThis = this;
            var H5 = {
                types: {},
                define: function (name, config) {
                    var parts = name.split('.');
                    var current = globalThis;
                    for (var i = 0; i < parts.length - 1; i++) {
                        if (!current[parts[i]]) current[parts[i]] = {};
                        current = current[parts[i]];
                    }
                    var typeName = parts[parts.length - 1];

                    var isGeneric = typeof config === 'function';

                    var createType = function(configObj) {
                        var constructor = function() {
                            if (configObj.fields) {
                                for (var k in configObj.fields) {
                                    this[k] = configObj.fields[k];
                                }
                            }
                            if (configObj.props) {
                                for (var k in configObj.props) {
                                    this[k] = configObj.props[k];
                                }
                            }
                            if (configObj.ctors && configObj.ctors.init) {
                                configObj.ctors.init.apply(this, arguments);
                            }
                        };

                        if (configObj.methods) {
                            for (var k in configObj.methods) {
                                constructor.prototype[k] = configObj.methods[k];
                            }
                        }

                        if (configObj.statics) {
                            if (configObj.statics.fields) {
                                for (var k in configObj.statics.fields) {
                                    constructor[k] = configObj.statics.fields[k];
                                }
                            }
                            if (configObj.statics.methods) {
                                for (var k in configObj.statics.methods) {
                                    constructor[k] = configObj.statics.methods[k];
                                }
                            }
                        }

                        return constructor;
                    };

                    if (isGeneric) {
                        var wrapper = function() {
                             return createType(config.apply(null, arguments));
                        };
                        current[typeName] = wrapper;
                        H5.types[name] = wrapper;
                    } else {
                        var ctor = createType(config);
                        current[typeName] = ctor;
                        H5.types[name] = ctor;
                    }
                }
            };
            ";
        }

        public void AssertExecution(string csharpSource, string executionScript, object expectedValue)
        {
            var compiledJs = _compiler.Compile(csharpSource);

            var engine = new Engine(options => {
                 options.Strict(false);
            });
            engine.Execute(GetH5Mock());
            try
            {
                engine.Execute(compiledJs);
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing compiled JS:\n" + compiledJs, ex);
            }

            var result = engine.Evaluate(executionScript).ToObject();

            // Jint often returns Double for all numbers, cast for assertion ease if we passed an int
            if (expectedValue is int i && result is double d)
            {
                Assert.Equal((double)i, d);
            }
            else
            {
                Assert.Equal(expectedValue, result);
            }
        }
    }
}
