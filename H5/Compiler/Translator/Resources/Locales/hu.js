H5.merge(new System.Globalization.CultureInfo("hu", true), {
    englishName: "Hungarian",
    nativeName: "magyar",

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
        percentPositivePattern: 1,
        percentNegativePattern: 1,
        currencySymbol: "Ft",
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
        abbreviatedDayNames: ["V","H","K","Sze","Cs","P","Szo"],
        abbreviatedMonthGenitiveNames: ["jan.","febr.","márc.","ápr.","máj.","jún.","júl.","aug.","szept.","okt.","nov.","dec.",""],
        abbreviatedMonthNames: ["jan.","febr.","márc.","ápr.","máj.","jún.","júl.","aug.","szept.","okt.","nov.","dec.",""],
        amDesignator: "de.",
        dateSeparator: ". ",
        dayNames: ["vasárnap","hétfő","kedd","szerda","csütörtök","péntek","szombat"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "yyyy. MMMM d., dddd H:mm:ss",
        longDatePattern: "yyyy. MMMM d., dddd",
        longTimePattern: "H:mm:ss",
        monthDayPattern: "MMMM d.",
        monthGenitiveNames: ["január","február","március","április","május","június","július","augusztus","szeptember","október","november","december",""],
        monthNames: ["január","február","március","április","május","június","július","augusztus","szeptember","október","november","december",""],
        pmDesignator: "du.",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "yyyy. MM. dd.",
        shortestDayNames: ["V","H","K","Sze","Cs","P","Szo"],
        shortTimePattern: "H:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "yyyy. MMMM",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 1250,
        CultureName: "hu-HU",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 1038,
        listSeparator: ";",
        MacCodePage: 10029,
        OEMCodePage: 852,
        IsReadOnly: true
    })
});
