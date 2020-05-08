    H5.define("System.Attribute", {
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
                    var baseType = H5.Reflection.getBaseType(o.td),
                        baseAttrs = [],
                        baseMember = null;

                    while (baseType != null && baseMember == null) {
                        baseMember = H5.Reflection.getMembers(baseType, 31, 28, o.n);

                        if (baseMember.length == 0) {
                            var newBaseType = H5.Reflection.getBaseType(baseType);

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
                            attrType = H5.getType(baseAttr),
                            meta = H5.getMetadata(attrType);

                        if (meta && meta.am || !r.some(function (a) { return H5.is(a, t); })) {
                            r.push(baseAttr);
                        }
                    }
                }

                if (!t) {
                    return r;
                }

                return r.filter(function (a) { return H5.is(a, t); });
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
