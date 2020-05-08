HighFive.merge(new System.Globalization.CultureInfo("tg-Cyrl", true), {
    englishName: "Tajik (Cyrillic)",
    nativeName: "тоҷикӣ",

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
        percentGroupSeparator: " ",
        percentPositivePattern: 1,
        percentNegativePattern: 1,
        currencySymbol: "смн",
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

    dateTimeFormat: HighFive.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["пкш","дшб","сшб","чшб","пшб","ҷум","шнб"],
        abbreviatedMonthGenitiveNames: ["янв","фев","мар","апр","май","июн","июл","авг","сен","окт","ноя","дек",""],
        abbreviatedMonthNames: ["янв","фев","мар","апр","май","июн","июл","авг","сен","окт","ноя","дек",""],
        amDesignator: "",
        dateSeparator: ".",
        dayNames: ["якшанбе","душанбе","сешанбе","чоршанбе","панҷшанбе","ҷумъа","шанбе"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "d MMMM yyyy' с.' HH:mm:ss",
        longDatePattern: "d MMMM yyyy' с.'",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "dd MMMM",
        monthGenitiveNames: ["январ","феврал","март","апрел","май","июн","июл","август","сентябр","октябр","ноябр","декабр",""],
        monthNames: ["январ","феврал","март","апрел","май","июн","июл","август","сентябр","октябр","ноябр","декабр",""],
        pmDesignator: "",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd.MM.yyyy",
        shortestDayNames: ["яш","дш","сш","чш","пш","ҷм","шб"],
        shortTimePattern: "HH:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "MMMM yyyy",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: HighFive.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 1251,
        CultureName: "tg-Cyrl-TJ",
        EBCDICCodePage: 20880,
        IsRightToLeft: false,
        LCID: 1064,
        listSeparator: ";",
        MacCodePage: 10007,
        OEMCodePage: 866,
        IsReadOnly: true
    })
});