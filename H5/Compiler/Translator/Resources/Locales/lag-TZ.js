H5.merge(new System.Globalization.CultureInfo("lag-TZ", true), {
    englishName: "Langi (Tanzania)",
    nativeName: "Kɨlaangi (Taansanía)",

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
        currencySymbol: "TSh",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 0,
        currencyDecimalSeparator: ".",
        currencyGroupSeparator: ",",
        currencyNegativePattern: 9,
        currencyPositivePattern: 2,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ".",
        numberGroupSeparator: ",",
        numberNegativePattern: 1
    }),

    dateTimeFormat: H5.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["Píili","Táatu","Íne","Táano","Alh","Ijm","Móosi"],
        abbreviatedMonthGenitiveNames: ["Fúngatɨ","Naanɨ","Keenda","Ikúmi","Inyambala","Idwaata","Mʉʉnchɨ","Vɨɨrɨ","Saatʉ","Inyi","Saano","Sasatʉ",""],
        abbreviatedMonthNames: ["Fúngatɨ","Naanɨ","Keenda","Ikúmi","Inyambala","Idwaata","Mʉʉnchɨ","Vɨɨrɨ","Saatʉ","Inyi","Saano","Sasatʉ",""],
        amDesignator: "TOO",
        dateSeparator: "/",
        dayNames: ["Jumapíiri","Jumatátu","Jumaíne","Jumatáano","Alamíisi","Ijumáa","Jumamóosi"],
        firstDayOfWeek: 1,
        fullDateTimePattern: "dddd, d MMMM yyyy HH:mm:ss",
        longDatePattern: "dddd, d MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "MMMM d",
        monthGenitiveNames: ["Kʉfúngatɨ","Kʉnaanɨ","Kʉkeenda","Kwiikumi","Kwiinyambála","Kwiidwaata","Kʉmʉʉnchɨ","Kʉvɨɨrɨ","Kʉsaatʉ","Kwiinyi","Kʉsaano","Kʉsasatʉ",""],
        monthNames: ["Kʉfúngatɨ","Kʉnaanɨ","Kʉkeenda","Kwiikumi","Kwiinyambála","Kwiidwaata","Kʉmʉʉnchɨ","Kʉvɨɨrɨ","Kʉsaatʉ","Kwiinyi","Kʉsaano","Kʉsasatʉ",""],
        pmDesignator: "MUU",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd/MM/yyyy",
        shortestDayNames: ["Píili","Táatu","Íne","Táano","Alh","Ijm","Móosi"],
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
        CultureName: "lag-TZ",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 4096,
        listSeparator: ";",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});
