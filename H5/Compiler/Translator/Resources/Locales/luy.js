H5.merge(new System.Globalization.CultureInfo("luy", true), {
    englishName: "Luyia",
    nativeName: "Luluhia",

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
        currencyNegativePattern: 2,
        currencyPositivePattern: 0,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ".",
        numberGroupSeparator: ",",
        numberNegativePattern: 1
    }),

    dateTimeFormat: H5.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["J2","J3","J4","J5","Al","Ij","J1"],
        abbreviatedMonthGenitiveNames: ["Jan","Feb","Mar","Apr","Mei","Jun","Jul","Ago","Sep","Okt","Nov","Des",""],
        abbreviatedMonthNames: ["Jan","Feb","Mar","Apr","Mei","Jun","Jul","Ago","Sep","Okt","Nov","Des",""],
        amDesignator: "a.m.",
        dateSeparator: "/",
        dayNames: ["Jumapiri","Jumatatu","Jumanne","Jumatano","Murwa wa Kanne","Murwa wa Katano","Jumamosi"],
        firstDayOfWeek: 0,
        fullDateTimePattern: "dddd, d MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd, d MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "MMMM d",
        monthGenitiveNames: ["Januari","Februari","Machi","Aprili","Mei","Juni","Julai","Agosti","Septemba","Oktoba","Novemba","Desemba",""],
        monthNames: ["Januari","Februari","Machi","Aprili","Mei","Juni","Julai","Agosti","Septemba","Oktoba","Novemba","Desemba",""],
        pmDesignator: "p.m.",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd/MM/yyyy",
        shortestDayNames: ["J2","J3","J4","J5","Al","Ij","J1"],
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
        CultureName: "luy-KE",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 4096,
        listSeparator: ";",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});
