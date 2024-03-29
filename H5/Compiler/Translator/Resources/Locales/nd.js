H5.merge(new System.Globalization.CultureInfo("nd", true), {
    englishName: "North Ndebele",
    nativeName: "isiNdebele",

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
        currencySymbol: "$",
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
        abbreviatedDayNames: ["Son","Mvu","Sib","Sit","Sin","Sih","Mgq"],
        abbreviatedMonthGenitiveNames: ["Zib","Nhlo","Mbi","Mab","Nkw","Nhla","Ntu","Ncw","Mpan","Mfu","Lwe","Mpal",""],
        abbreviatedMonthNames: ["Zib","Nhlo","Mbi","Mab","Nkw","Nhla","Ntu","Ncw","Mpan","Mfu","Lwe","Mpal",""],
        amDesignator: "AM",
        dateSeparator: "/",
        dayNames: ["Sonto","Mvulo","Sibili","Sithathu","Sine","Sihlanu","Mgqibelo"],
        firstDayOfWeek: 0,
        fullDateTimePattern: "dddd, d MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd, d MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "MMMM d",
        monthGenitiveNames: ["Zibandlela","Nhlolanja","Mbimbitho","Mabasa","Nkwenkwezi","Nhlangula","Ntulikazi","Ncwabakazi","Mpandula","Mfumfu","Lwezi","Mpalakazi",""],
        monthNames: ["Zibandlela","Nhlolanja","Mbimbitho","Mabasa","Nkwenkwezi","Nhlangula","Ntulikazi","Ncwabakazi","Mpandula","Mfumfu","Lwezi","Mpalakazi",""],
        pmDesignator: "PM",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd/MM/yyyy",
        shortestDayNames: ["Son","Mvu","Sib","Sit","Sin","Sih","Mgq"],
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
        CultureName: "nd-ZW",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 4096,
        listSeparator: ";",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});
