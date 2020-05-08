    HighFive.define("System.Reflection.TypeAttributes", {
        $kind: "enum",
        statics: {
            fields: {
                VisibilityMask: 7,
                NotPublic: 0,
                Public: 1,
                NestedPublic: 2,
                NestedPrivate: 3,
                NestedFamily: 4,
                NestedAssembly: 5,
                NestedFamANDAssem: 6,
                NestedFamORAssem: 7,
                LayoutMask: 24,
                AutoLayout: 0,
                SequentialLayout: 8,
                ExplicitLayout: 16,
                ClassSemanticsMask: 32,
                Class: 0,
                Interface: 32,
                Abstract: 128,
                Sealed: 256,
                SpecialName: 1024,
                Import: 4096,
                Serializable: 8192,
                WindowsRuntime: 16384,
                StringFormatMask: 196608,
                AnsiClass: 0,
                UnicodeClass: 65536,
                AutoClass: 131072,
                CustomFormatClass: 196608,
                CustomFormatMask: 12582912,
                BeforeFieldInit: 1048576,
                RTSpecialName: 2048,
                HasSecurity: 262144,
                ReservedMask: 264192
            }
        },
        $flags: true
    });