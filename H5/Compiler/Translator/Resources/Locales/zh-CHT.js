H5.merge(new System.Globalization.CultureInfo("zh-CHT", true), {
    englishName: "Chinese (Traditional) Legacy",
    nativeName: "中文(繁體) 舊版",

    numberFormat: H5.merge(new System.Globalization.NumberFormatInfo(), {
        nanSymbol: "非數值",
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
        currencySymbol: "HK$",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 2,
        currencyDecimalSeparator: ".",
        currencyGroupSeparator: ",",
        currencyNegativePattern: 0,
        currencyPositivePattern: 0,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ".",
        numberGroupSeparator: ",",
        numberNegativePattern: 1
    }),

    dateTimeFormat: H5.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["週日","週一","週二","週三","週四","週五","週六"],
        abbreviatedMonthGenitiveNames: ["一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月",""],
        abbreviatedMonthNames: ["一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月",""],
        amDesignator: "上午",
        dateSeparator: "/",
        dayNames: ["星期日","星期一","星期二","星期三","星期四","星期五","星期六"],
        firstDayOfWeek: 0,
        fullDateTimePattern: "yyyy'年'M'月'd'日' H:mm:ss",
        longDatePattern: "yyyy'年'M'月'd'日'",
        longTimePattern: "H:mm:ss",
        monthDayPattern: "M月d日",
        monthGenitiveNames: ["一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月",""],
        monthNames: ["一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月",""],
        pmDesignator: "下午",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "d/M/yyyy",
        shortestDayNames: ["日","一","二","三","四","五","六"],
        shortTimePattern: "H:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "yyyy'年'M'月'",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 950,
        CultureName: "zh-HK",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 3076,
        listSeparator: ",",
        MacCodePage: 10002,
        OEMCodePage: 950,
        IsReadOnly: true
    })
});
