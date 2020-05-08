HighFive.merge(new System.Globalization.CultureInfo("da-DK", true), {
    englishName: "Danish (Denmark)",
    nativeName: "dansk (Danmark)",

    numberFormat: HighFive.merge(new System.Globalization.NumberFormatInfo(), {
        nanSymbol: "NaN",
        negativeSign: "-",
        positiveSign: "+",
        negativeInfinitySymbol: "-∞",
        positiveInfinitySymbol: "∞",
        percentSymbol: "%",
        percentGroupSizes: [3],
        percentDecimalDigits: 2,
        percentDecimalSeparator: ",",
        percentGroupSeparator: ".",
        percentPositivePattern: 0,
        percentNegativePattern: 0,
        currencySymbol: "kr.",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 2,
        currencyDecimalSeparator: ",",
        currencyGroupSeparator: ".",
        currencyNegativePattern: 8,
        currencyPositivePattern: 3,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ",",
        numberGroupSeparator: ".",
        numberNegativePattern: 1
    }),

    dateTimeFormat: HighFive.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["sø","ma","ti","on","to","fr","lø"],
        abbreviatedMonthGenitiveNames: ["jan","feb","mar","apr","maj","jun","jul","aug","sep","okt","nov","dec",""],
        abbreviatedMonthNames: ["jan","feb","mar","apr","maj","jun","jul","aug","sep","okt","nov","dec",""],
        amDesignator: "",
        dateSeparator: "-",
        dayNames: ["søndag","mandag","tirsdag","onsdag","torsdag","fredag","lørdag"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "d. MMMM yyyy HH:mm:ss",
        longDatePattern: "d. MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "d. MMMM",
        monthGenitiveNames: ["januar","februar","marts","april","maj","juni","juli","august","september","oktober","november","december",""],
        monthNames: ["januar","februar","marts","april","maj","juni","juli","august","september","oktober","november","december",""],
        pmDesignator: "",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd-MM-yyyy",
        shortestDayNames: ["sø","ma","ti","on","to","fr","lø"],
        shortTimePattern: "HH:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "MMMM yyyy",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: HighFive.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 1252,
        CultureName: "da-DK",
        EBCDICCodePage: 20277,
        IsRightToLeft: false,
        LCID: 1030,
        listSeparator: ";",
        MacCodePage: 10000,
        OEMCodePage: 850,
        IsReadOnly: true
    })
});