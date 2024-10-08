H5.merge(new System.Globalization.CultureInfo("wae", true), {
    englishName: "Walser",
    nativeName: "Walser",

    numberFormat: H5.merge(new System.Globalization.NumberFormatInfo(), {
        nanSymbol: "NaN",
        negativeSign: "-",
        positiveSign: "+",
        negativeInfinitySymbol: "-∞",
        positiveInfinitySymbol: "∞",
        percentSymbol: "%",
        percentGroupSizes: [3],
        percentDecimalDigits: 2,
        percentDecimalSeparator: ",",
        percentGroupSeparator: "’",
        percentPositivePattern: 1,
        percentNegativePattern: 1,
        currencySymbol: "CHF",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 2,
        currencyDecimalSeparator: ",",
        currencyGroupSeparator: "’",
        currencyNegativePattern: 9,
        currencyPositivePattern: 2,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ",",
        numberGroupSeparator: "’",
        numberNegativePattern: 1
    }),

    dateTimeFormat: H5.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["Sun","Män","Ziš","Mit","Fró","Fri","Sam"],
        abbreviatedMonthGenitiveNames: ["Jen","Hor","Mär","Abr","Mei","Brá","Hei","Öig","Her","Wím","Win","Chr",""],
        abbreviatedMonthNames: ["Jen","Hor","Mär","Abr","Mei","Brá","Hei","Öig","Her","Wím","Win","Chr",""],
        amDesignator: "AM",
        dateSeparator: "-",
        dayNames: ["Sunntag","Mäntag","Zištag","Mittwuč","Fróntag","Fritag","Samštag"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "dddd, d. MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd, d. MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "MMMM d",
        monthGenitiveNames: ["Jenner","Hornig","Märze","Abrille","Meije","Bráčet","Heiwet","Öigšte","Herbštmánet","Wímánet","Wintermánet","Chrištmánet",""],
        monthNames: ["Jenner","Hornig","Märze","Abrille","Meije","Bráčet","Heiwet","Öigšte","Herbštmánet","Wímánet","Wintermánet","Chrištmánet",""],
        pmDesignator: "PM",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "yyyy-MM-dd",
        shortestDayNames: ["Sun","Män","Ziš","Mit","Fró","Fri","Sam"],
        shortTimePattern: "HH:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "yyyy MMMM",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 0,
        CultureName: "wae-CH",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 4096,
        listSeparator: ";",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});
