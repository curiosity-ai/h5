    Bridge.define("System.Attribute", {
        statics: {
            getCustomAttributes: function (o, t, b) {
                if (o == null) {
                    throw new System.ArgumentNullException.$ctor1("element");
                }

                if (t == null) {
                    throw new System.ArgumentNullException.$ctor1("attributeType");
                }

                var r = o.at || [];

                if (o.ov === true) {
                    var baseType = Bridge.Reflection.getBaseType(o.td),
                        baseAttrs = [],
                        baseMember = null;

                    while (baseType != null && baseMember == null) {
                        baseMember = Bridge.Reflection.getMembers(baseType, 31, 28, o.n);

                        if (baseMember.length == 0) {
                            var newBaseType = Bridge.Reflection.getBaseType(baseType);

                            if (newBaseType != baseType) {
                                baseType = newBaseType;
                            }

                            baseMember = null;
                        } else {
                            baseMember = baseMember[0];
                        }
                    }

                    if (baseMember != null) {
                        baseAttrs = System.Attribute.getCustomAttributes(baseMember, t);
                    }

                    for (var i = 0; i < baseAttrs.length; i++) {
                        var baseAttr = baseAttrs[i],
                            attrType = Bridge.getType(baseAttr),
                            meta = Bridge.getMetadata(attrType);

                        if (meta && meta.am || !r.some(function (a) { return Bridge.is(a, t); })) {
                            r.push(baseAttr);
                        }
                    }
                }

                if (!t) {
                    return r;
                }

                return r.filter(function (a) { return Bridge.is(a, t); });
            },

            getCustomAttributes$1: function (a, t, b) {
                if (a == null) {
                    throw new System.ArgumentNullException.$ctor1("element");
                }

                if (t == null) {
                    throw new System.ArgumentNullException.$ctor1("attributeType");
                }

                return a.getCustomAttributes(t || b);
            },

            isDefined: function (o, t, b) {
                var attrs = System.Attribute.getCustomAttributes(o, t, b);

                return attrs.length > 0;
            }
        }
    });
