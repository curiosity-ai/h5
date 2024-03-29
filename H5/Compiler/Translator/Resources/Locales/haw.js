H5.merge(new System.Globalization.CultureInfo("haw", true), {
    englishName: "Hawaiian",
    nativeName: "ʻŌlelo Hawaiʻi",

    numberFormat: H5.merge(new System.Globalization.NumberFormatInfo(), {
        nanSymbol: "NaN",
        negativeSign: "-",
        positiveSign: "+",
        negativeInfinitySymbol: "-∞",
        positiveInfinitySymbol: "∞",
        percentSymbol: "%",
        percentGroupSizes: [3],
        percentDecimalDigits: 2,
        percentDecimalSeparator: ".",
        percentGroupSeparator: ",",
        percentPositivePattern: 1,
        percentNegativePattern: 1,
        currencySymbol: "$",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 2,
        currencyDecimalSeparator: ".",
        currencyGroupSeparator: ",",
        currencyNegativePattern: 1,
        currencyPositivePattern: 0,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ".",
        numberGroupSeparator: ",",
        numberNegativePattern: 1
    }),

    dateTimeFormat: H5.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["LP","P1","P2","P3","P4","P5","P6"],
        abbreviatedMonthGenitiveNames: ["Ian.","Pep.","Mal.","ʻAp.","Mei","Iun.","Iul.","ʻAu.","Kep.","ʻOk.","Now.","Kek.",""],
        abbreviatedMonthNames: ["Ian.","Pep.","Mal.","ʻAp.","Mei","Iun.","Iul.","ʻAu.","Kep.","ʻOk.","Now.","Kek.",""],
        amDesignator: "AM",
        dateSeparator: "/",
        dayNames: ["Lāpule","Poʻakahi","Poʻalua","Poʻakolu","Poʻahā","Poʻalima","Poʻaono"],
        firstDayOfWeek: 0,
        fullDateTimePattern: "dddd, d MMMM yyyy h:mm:ss tt",
        longDatePattern: "dddd, d MMMM yyyy",
        longTimePattern: "h:mm:ss tt",
        monthDayPattern: "MMMM d",
        monthGenitiveNames: ["Ianuali","Pepeluali","Malaki","ʻApelila","Mei","Iune","Iulai","ʻAukake","Kepakemapa","ʻOkakopa","Nowemapa","Kekemapa",""],
        monthNames: ["Ianuali","Pepeluali","Malaki","ʻApelila","Mei","Iune","Iulai","ʻAukake","Kepakemapa","ʻOkakopa","Nowemapa","Kekemapa",""],
        pmDesignator: "PM",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "d/M/yyyy",
        shortestDayNames: ["LP","P1","P2","P3","P4","P5","P6"],
        shortTimePattern: "h:mm tt",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "yyyy MMMM",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 1252,
        CultureName: "haw-US",
        EBCDICCodePage: 37,
        IsRightToLeft: false,
        LCID: 1141,
        listSeparator: ";",
        MacCodePage: 10000,
        OEMCodePage: 437,
        IsReadOnly: true
    })
});
