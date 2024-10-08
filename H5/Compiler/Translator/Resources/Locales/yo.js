H5.merge(new System.Globalization.CultureInfo("yo", true), {
    englishName: "Yoruba",
    nativeName: "Èdè Yorùbá",

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
        currencySymbol: "₦",
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
        abbreviatedDayNames: ["Àìkú","Ajé","Ìsẹ́gun","Ọjọ́rú","Ọjọ́bọ","Ẹtì","Àbámẹ́ta"],
        abbreviatedMonthGenitiveNames: ["Ṣẹ́rẹ́","Èrèlè","Ẹrẹ̀nà","Ìgbé","Ẹ̀bibi","Òkúdu","Agẹmọ","Ògún","Owewe","Ọ̀wàrà","Bélú","Ọ̀pẹ̀",""],
        abbreviatedMonthNames: ["Ṣẹ́rẹ́","Èrèlè","Ẹrẹ̀nà","Ìgbé","Ẹ̀bibi","Òkúdu","Agẹmọ","Ògún","Owewe","Ọ̀wàrà","Bélú","Ọ̀pẹ̀",""],
        amDesignator: "Àárọ̀",
        dateSeparator: "/",
        dayNames: ["Ọjọ́ Àìkú","Ọjọ́ Ajé","Ọjọ́ Ìsẹ́gun","Ọjọ́rú","Ọjọ́bọ","Ọjọ́ Ẹtì","Ọjọ́ Àbámẹ́ta"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "dddd, d MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd, d MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "MMMM d",
        monthGenitiveNames: ["Oṣù Ṣẹ́rẹ́","Oṣù Èrèlè","Oṣù Ẹrẹ̀nà","Oṣù Ìgbé","Oṣù Ẹ̀bibi","Oṣù Òkúdu","Oṣù Agẹmọ","Oṣù Ògún","Oṣù Owewe","Oṣù Ọ̀wàrà","Oṣù Bélú","Oṣù Ọ̀pẹ̀",""],
        monthNames: ["Oṣù Ṣẹ́rẹ́","Oṣù Èrèlè","Oṣù Ẹrẹ̀nà","Oṣù Ìgbé","Oṣù Ẹ̀bibi","Oṣù Òkúdu","Oṣù Agẹmọ","Oṣù Ògún","Oṣù Owewe","Oṣù Ọ̀wàrà","Oṣù Bélú","Oṣù Ọ̀pẹ̀",""],
        pmDesignator: "Ọ̀sán",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd/MM/yyyy",
        shortestDayNames: ["Àìkú","Ajé","Ìsẹ́gun","Ọjọ́rú","Ọjọ́bọ","Ẹtì","Àbámẹ́ta"],
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
        CultureName: "yo-NG",
        EBCDICCodePage: 37,
        IsRightToLeft: false,
        LCID: 1130,
        listSeparator: ";",
        MacCodePage: 10000,
        OEMCodePage: 437,
        IsReadOnly: true
    })
});
