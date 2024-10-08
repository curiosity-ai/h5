H5.merge(new System.Globalization.CultureInfo("nn-NO", true), {
    englishName: "Norwegian Nynorsk (Norway)",
    nativeName: "nynorsk (Noreg)",

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
        percentGroupSeparator: " ",
        percentPositivePattern: 0,
        percentNegativePattern: 0,
        currencySymbol: "kr",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 2,
        currencyDecimalSeparator: ",",
        currencyGroupSeparator: " ",
        currencyNegativePattern: 8,
        currencyPositivePattern: 3,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ",",
        numberGroupSeparator: " ",
        numberNegativePattern: 1
    }),

    dateTimeFormat: H5.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["sø.","må.","ty.","on.","to.","fr.","la."],
        abbreviatedMonthGenitiveNames: ["jan.","feb.","mars","apr.","mai","juni","juli","aug.","sep.","okt.","nov.","des.",""],
        abbreviatedMonthNames: ["jan","feb","mar","apr","mai","jun","jul","aug","sep","okt","nov","des",""],
        amDesignator: "f.m.",
        dateSeparator: ".",
        dayNames: ["søndag","måndag","tysdag","onsdag","torsdag","fredag","laurdag"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "dddd d. MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd d. MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "d. MMMM",
        monthGenitiveNames: ["januar","februar","mars","april","mai","juni","juli","august","september","oktober","november","desember",""],
        monthNames: ["januar","februar","mars","april","mai","juni","juli","august","september","oktober","november","desember",""],
        pmDesignator: "e.m.",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd.MM.yyyy",
        shortestDayNames: ["sø.","må.","ty.","on.","to.","fr.","la."],
        shortTimePattern: "HH:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "MMMM yyyy",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 1252,
        CultureName: "nn-NO",
        EBCDICCodePage: 20277,
        IsRightToLeft: false,
        LCID: 2068,
        listSeparator: ";",
        MacCodePage: 10000,
        OEMCodePage: 850,
        IsReadOnly: true
    })
});
