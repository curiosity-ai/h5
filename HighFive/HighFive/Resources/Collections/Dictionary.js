    System.Collections.Generic.Dictionary$2.getTypeParameters = function (type) {
        var interfaceType;

        if (System.String.startsWith(type.$$name, "System.Collections.Generic.IDictionary")) {
            interfaceType = type;
        } else {
            var interfaces = HighFive.Reflection.getInterfaces(type);

            for (var j = 0; j < interfaces.length; j++) {
                if (System.String.startsWith(interfaces[j].$$name, "System.Collections.Generic.IDictionary")) {
                    interfaceType = interfaces[j];

                    break;
                }
            }
        }

        var typesGeneric = interfaceType ? HighFive.Reflection.getGenericArguments(interfaceType) : null;
        var typeKey = typesGeneric ? typesGeneric[0] : null;
        var typeValue = typesGeneric ? typesGeneric[1] : null;

        return [typeKey, typeValue];
    };