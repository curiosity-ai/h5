H5.merge(new System.Globalization.CultureInfo("sg", true), {
    englishName: "Sango",
    nativeName: "Sängö",

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
        percentGroupSeparator: ".",
        percentPositivePattern: 1,
        percentNegativePattern: 1,
        currencySymbol: "FCFA",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 0,
        currencyDecimalSeparator: ",",
        currencyGroupSeparator: ".",
        currencyNegativePattern: 2,
        currencyPositivePattern: 0,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ",",
        numberGroupSeparator: ".",
        numberNegativePattern: 1
    }),

    dateTimeFormat: H5.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["Bk1","Bk2","Bk3","Bk4","Bk5","Lâp","Lây"],
        abbreviatedMonthGenitiveNames: ["Nye","Ful","Mbä","Ngu","Bêl","Fön","Len","Kük","Mvu","Ngb","Nab","Kak",""],
        abbreviatedMonthNames: ["Nye","Ful","Mbä","Ngu","Bêl","Fön","Len","Kük","Mvu","Ngb","Nab","Kak",""],
        amDesignator: "ND",
        dateSeparator: "/",
        dayNames: ["Bikua-ôko","Bïkua-ûse","Bïkua-ptâ","Bïkua-usïö","Bïkua-okü","Lâpôsö","Lâyenga"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "dddd d MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd d MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "d MMMM",
        monthGenitiveNames: ["Nyenye","Fulundïgi","Mbängü","Ngubùe","Bêläwü","Föndo","Lengua","Kükürü","Mvuka","Ngberere","Nabändüru","Kakauka",""],
        monthNames: ["Nyenye","Fulundïgi","Mbängü","Ngubùe","Bêläwü","Föndo","Lengua","Kükürü","Mvuka","Ngberere","Nabändüru","Kakauka",""],
        pmDesignator: "LK",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "d/M/yyyy",
        shortestDayNames: ["Bk1","Bk2","Bk3","Bk4","Bk5","Lâp","Lây"],
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
        CultureName: "sg-CF",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 4096,
        listSeparator: ";",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});
