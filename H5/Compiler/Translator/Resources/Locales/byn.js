H5.merge(new System.Globalization.CultureInfo("byn", true), {
    englishName: "Blin",
    nativeName: "ብሊን",

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
        currencySymbol: "Nfk",
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
        abbreviatedDayNames: ["ሰ/ቅ","ሰኑ","ሰሊጝ","ለጓ","ኣምድ","ኣርብ","ሰ/ሽ"],
        abbreviatedMonthGenitiveNames: ["ልደት","ካብኽ","ክብላ","ፋጅኺ","ክቢቅ","ም/ት","ኰር","ማርያ","ያኸኒ","መተሉ","ም/ም","ተሕሳ",""],
        abbreviatedMonthNames: ["ልደት","ካብኽ","ክብላ","ፋጅኺ","ክቢቅ","ም/ት","ኰር","ማርያ","ያኸኒ","መተሉ","ም/ም","ተሕሳ",""],
        amDesignator: "ፋዱስ ጃብ",
        dateSeparator: "/",
        dayNames: ["ሰንበር ቅዳዅ","ሰኑ","ሰሊጝ","ለጓ ወሪ ለብዋ","ኣምድ","ኣርብ","ሰንበር ሽጓዅ"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "dddd፡ dd MMMM ግርጋ yyyy gg h:mm:ss tt",
        longDatePattern: "dddd፡ dd MMMM ግርጋ yyyy gg",
        longTimePattern: "h:mm:ss tt",
        monthDayPattern: "MMMM d",
        monthGenitiveNames: ["ልደትሪ","ካብኽብቲ","ክብላ","ፋጅኺሪ","ክቢቅሪ","ምኪኤል ትጟኒሪ","ኰርኩ","ማርያም ትሪ","ያኸኒ መሳቅለሪ","መተሉ","ምኪኤል መሽወሪ","ተሕሳስሪ",""],
        monthNames: ["ልደትሪ","ካብኽብቲ","ክብላ","ፋጅኺሪ","ክቢቅሪ","ምኪኤል ትጟኒሪ","ኰርኩ","ማርያም ትሪ","ያኸኒ መሳቅለሪ","መተሉ","ምኪኤል መሽወሪ","ተሕሳስሪ",""],
        pmDesignator: "ፋዱስ ደምቢ",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd/MM/yyyy",
        shortestDayNames: ["ሰ/ቅ","ሰኑ","ሰሊጝ","ለጓ","ኣምድ","ኣርብ","ሰ/ሽ"],
        shortTimePattern: "h:mm tt",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "yyyy MMMM",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: H5.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 0,
        CultureName: "byn-ER",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 4096,
        listSeparator: ";",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});
