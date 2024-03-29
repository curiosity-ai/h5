H5.merge(new System.Globalization.CultureInfo("ebu", true), {
    englishName: "Embu",
    nativeName: "Kĩembu",

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
        currencySymbol: "Ksh",
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
        abbreviatedDayNames: ["Kma","Tat","Ine","Tan","Arm","Maa","NMM"],
        abbreviatedMonthGenitiveNames: ["Mbe","Kai","Kat","Kan","Gat","Gan","Mug","Knn","Ken","Iku","Imw","Igi",""],
        abbreviatedMonthNames: ["Mbe","Kai","Kat","Kan","Gat","Gan","Mug","Knn","Ken","Iku","Imw","Igi",""],
        amDesignator: "KI",
        dateSeparator: "/",
        dayNames: ["Kiumia","Njumatatu","Njumaine","Njumatano","Aramithi","Njumaa","NJumamothii"],
        firstDayOfWeek: 0,
        fullDateTimePattern: "dddd, d MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd, d MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "MMMM d",
        monthGenitiveNames: ["Mweri wa mbere","Mweri wa kaĩri","Mweri wa kathatũ","Mweri wa kana","Mweri wa gatano","Mweri wa gatantatũ","Mweri wa mũgwanja","Mweri wa kanana","Mweri wa kenda","Mweri wa ikũmi","Mweri wa ikũmi na ũmwe","Mweri wa ikũmi na Kaĩrĩ",""],
        monthNames: ["Mweri wa mbere","Mweri wa kaĩri","Mweri wa kathatũ","Mweri wa kana","Mweri wa gatano","Mweri wa gatantatũ","Mweri wa mũgwanja","Mweri wa kanana","Mweri wa kenda","Mweri wa ikũmi","Mweri wa ikũmi na ũmwe","Mweri wa ikũmi na Kaĩrĩ",""],
        pmDesignator: "UT",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd/MM/yyyy",
        shortestDayNames: ["Kma","Tat","Ine","Tan","Arm","Maa","NMM"],
        shortTimePattern: "HH:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "MMMM yyyy",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 0,
        CultureName: "ebu-KE",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 4096,
        listSeparator: ";",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});
