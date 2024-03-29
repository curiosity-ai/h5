H5.merge(new System.Globalization.CultureInfo("lt-LT", true), {
    englishName: "Lithuanian (Lithuania)",
    nativeName: "lietuvių (Lietuva)",

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
        currencySymbol: "€",
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
        abbreviatedDayNames: ["sk","pr","an","tr","kt","pn","št"],
        abbreviatedMonthGenitiveNames: ["saus.","vas.","kov.","bal.","geg.","birž.","liep.","rugp.","rugs.","spal.","lapkr.","gruod.",""],
        abbreviatedMonthNames: ["saus.","vas.","kov.","bal.","geg.","birž.","liep.","rugp.","rugs.","spal.","lapkr.","gruod.",""],
        amDesignator: "priešpiet",
        dateSeparator: "-",
        dayNames: ["sekmadienis","pirmadienis","antradienis","trečiadienis","ketvirtadienis","penktadienis","šeštadienis"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "yyyy 'm'. MMMM d 'd'., dddd HH:mm:ss",
        longDatePattern: "yyyy 'm'. MMMM d 'd'., dddd",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "MMMM d 'd'.",
        monthGenitiveNames: ["sausio","vasario","kovo","balandžio","gegužės","birželio","liepos","rugpjūčio","rugsėjo","spalio","lapkričio","gruodžio",""],
        monthNames: ["sausis","vasaris","kovas","balandis","gegužė","birželis","liepa","rugpjūtis","rugsėjis","spalis","lapkritis","gruodis",""],
        pmDesignator: "popiet",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "yyyy-MM-dd",
        shortestDayNames: ["Sk","Pr","An","Tr","Kt","Pn","Št"],
        shortTimePattern: "HH:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "yyyy 'm'. MMMM",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 1257,
        CultureName: "lt-LT",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 1063,
        listSeparator: ";",
        MacCodePage: 10029,
        OEMCodePage: 775,
        IsReadOnly: true
    })
});
