H5.merge(new System.Globalization.CultureInfo("mr-IN", true), {
    englishName: "Marathi (India)",
    nativeName: "मराठी (भारत)",

    numberFormat: H5.merge(new System.Globalization.NumberFormatInfo(), {
        nanSymbol: "NaN",
        negativeSign: "-",
        positiveSign: "+",
        negativeInfinitySymbol: "-∞",
        positiveInfinitySymbol: "∞",
        percentSymbol: "%",
        percentGroupSizes: [3,2],
        percentDecimalDigits: 2,
        percentDecimalSeparator: ".",
        percentGroupSeparator: ",",
        percentPositivePattern: 1,
        percentNegativePattern: 1,
        currencySymbol: "₹",
        currencyGroupSizes: [3,2],
        currencyDecimalDigits: 2,
        currencyDecimalSeparator: "`",
        currencyGroupSeparator: ",",
        currencyNegativePattern: 12,
        currencyPositivePattern: 0,
        numberGroupSizes: [3,2],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ".",
        numberGroupSeparator: ",",
        numberNegativePattern: 1
    }),

    dateTimeFormat: H5.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["रवि.","सोम.","मंगळ.","बुध.","गुरु.","शुक्र.","शनि."],
        abbreviatedMonthGenitiveNames: ["जाने.","फेब्रु.","मार्च","एप्रि","मे","जून","जुलै","ऑग.","सप्टें.","ऑक्टो.","नोव्हें.","डिसें.",""],
        abbreviatedMonthNames: ["जाने.","फेब्रु.","मार्च","एप्रि","मे","जून","जुलै","ऑग.","सप्टें.","ऑक्टो.","नोव्हें.","डिसें.",""],
        amDesignator: "म.पू.",
        dateSeparator: "-",
        dayNames: ["रविवार","सोमवार","मंगळवार","बुधवार","गुरुवार","शुक्रवार","शनिवार"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "dd MMMM yyyy HH:mm:ss",
        longDatePattern: "dd MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "dd MMMM",
        monthGenitiveNames: ["जानेवारी","फेब्रुवारी","मार्च","एप्रिल","मे","जून","जुलै","ऑगस्ट","सप्टेंबर","ऑक्टोबर","नोव्हेंबर","डिसेंबर",""],
        monthNames: ["जानेवारी","फेब्रुवारी","मार्च","एप्रिल","मे","जून","जुलै","ऑगस्ट","सप्टेंबर","ऑक्टोबर","नोव्हेंबर","डिसेंबर",""],
        pmDesignator: "म.नं.",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd-MM-yyyy",
        shortestDayNames: ["र","सो","मं","बु","गु","शु","श"],
        shortTimePattern: "HH:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "MMMM, yyyy",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 0,
        CultureName: "mr-IN",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 1102,
        listSeparator: ",",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});
