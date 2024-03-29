H5.merge(new System.Globalization.CultureInfo("gd", true), {
    englishName: "Scottish Gaelic",
    nativeName: "Gàidhlig",

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
        currencySymbol: "£",
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
        abbreviatedDayNames: ["DiD","DiL","DiM","DiC","Dia","Dih","DiS"],
        abbreviatedMonthGenitiveNames: ["Faoi","Gearr","Màrt","Gibl","Cèit","Ògmh","Iuch","Lùna","Sult","Dàmh","Samh","Dùbh",""],
        abbreviatedMonthNames: ["Faoi","Gearr","Màrt","Gibl","Cèit","Ògmh","Iuch","Lùna","Sult","Dàmh","Samh","Dùbh",""],
        amDesignator: "m",
        dateSeparator: "/",
        dayNames: ["DiDòmhnaich","DiLuain","DiMàirt","DiCiadain","DiarDaoin","DihAoine","DiSathairne"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "dddd, d'mh' MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd, d'mh' MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "d'mh' MMMM",
        monthGenitiveNames: ["dhen Fhaoilleach","dhen Ghearran","dhen Mhàrt","dhen Ghiblean","dhen Chèitean","dhen Ògmhios","dhen Iuchar","dhen Lùnastal","dhen t-Sultain","dhen Dàmhair","dhen t-Samhain","dhen Dùbhlachd",""],
        monthNames: ["Am Faoilleach","An Gearran","Am Màrt","An Giblean","An Cèitean","An t-Ògmhios","An t-Iuchar","An Lùnastal","An t-Sultain","An Dàmhair","An t-Samhain","An Dùbhlachd",""],
        pmDesignator: "f",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd/MM/yyyy",
        shortestDayNames: ["Dò","Lu","Mà","Ci","Da","hA","Sa"],
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
        CultureName: "gd-GB",
        EBCDICCodePage: 20285,
        IsRightToLeft: false,
        LCID: 1169,
        listSeparator: ";",
        MacCodePage: 10000,
        OEMCodePage: 850,
        IsReadOnly: true
    })
});
