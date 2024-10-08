H5.merge(new System.Globalization.CultureInfo("ja-JP", true), {
    englishName: "Japanese (Japan)",
    nativeName: "日本語 (日本)",

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
        currencySymbol: "¥",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 0,
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
        abbreviatedDayNames: ["日","月","火","水","木","金","土"],
        abbreviatedMonthGenitiveNames: ["1","2","3","4","5","6","7","8","9","10","11","12",""],
        abbreviatedMonthNames: ["1","2","3","4","5","6","7","8","9","10","11","12",""],
        amDesignator: "午前",
        dateSeparator: "/",
        dayNames: ["日曜日","月曜日","火曜日","水曜日","木曜日","金曜日","土曜日"],
        firstDayOfWeek: 0,
        fullDateTimePattern: "yyyy'年'M'月'd'日' H:mm:ss",
        longDatePattern: "yyyy'年'M'月'd'日'",
        longTimePattern: "H:mm:ss",
        monthDayPattern: "M月d日",
        monthGenitiveNames: ["1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月",""],
        monthNames: ["1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月",""],
        pmDesignator: "午後",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "yyyy/MM/dd",
        shortestDayNames: ["日","月","火","水","木","金","土"],
        shortTimePattern: "H:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "yyyy'年'M'月'",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 932,
        CultureName: "ja-JP",
        EBCDICCodePage: 20290,
        IsRightToLeft: false,
        LCID: 1041,
        listSeparator: ",",
        MacCodePage: 10001,
        OEMCodePage: 932,
        IsReadOnly: true
    })
});
