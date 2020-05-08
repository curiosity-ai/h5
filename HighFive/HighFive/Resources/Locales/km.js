HighFive.merge(new System.Globalization.CultureInfo("km", true), {
    englishName: "Khmer",
    nativeName: "ភាសាខ្មែរ",

    numberFormat: HighFive.merge(new System.Globalization.NumberFormatInfo(), {
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
        currencySymbol: "៛",
        currencyGroupSizes: [3],
        currencyDecimalDigits: 2,
        currencyDecimalSeparator: ".",
        currencyGroupSeparator: ",",
        currencyNegativePattern: 5,
        currencyPositivePattern: 1,
        numberGroupSizes: [3],
        numberDecimalDigits: 2,
        numberDecimalSeparator: ".",
        numberGroupSeparator: ",",
        numberNegativePattern: 2
    }),

    dateTimeFormat: HighFive.merge(new System.Globalization.DateTimeFormatInfo(), {
        abbreviatedDayNames: ["អាទិ.","ច.","អ.","ពុ","ព្រហ.","សុ.","ស."],
        abbreviatedMonthGenitiveNames: ["១","២","៣","៤","៥","៦","៧","៨","៩","១០","១១","១២",""],
        abbreviatedMonthNames: ["១","២","៣","៤","៥","៦","៧","៨","៩","១០","១១","១២",""],
        amDesignator: "ព្រឹក",
        dateSeparator: "/",
        dayNames: ["ថ្ងៃអាទិត្យ","ថ្ងៃច័ន្ទ","ថ្ងៃអង្គារ","ថ្ងៃពុធ","ថ្ងៃព្រហស្បតិ៍","ថ្ងៃសុក្រ","ថ្ងៃសៅរ៍"],
        firstDayOfWeek: 0,
        fullDateTimePattern: "d MMMM yyyy HH:mm:ss",
        longDatePattern: "d MMMM yyyy",
        longTimePattern: "HH:mm:ss",
        monthDayPattern: "d MMMM",
        monthGenitiveNames: ["មករា","កុម្ភៈ","មិនា","មេសា","ឧសភា","មិថុនា","កក្កដា","សីហា","កញ្ញា","តុលា","វិច្ឆិកា","ធ្នូ",""],
        monthNames: ["មករា","កុម្ភៈ","មិនា","មេសា","ឧសភា","មិថុនា","កក្កដា","សីហា","កញ្ញា","តុលា","វិច្ឆិកា","ធ្នូ",""],
        pmDesignator: "ល្ងាច",
        rfc1123: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
        shortDatePattern: "dd/MM/yy",
        shortestDayNames: ["អា","ច","អ","ពុ","ព","សុ","ស"],
        shortTimePattern: "H:mm",
        sortableDateTimePattern: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
        sortableDateTimePattern1: "yyyy'-'MM'-'dd",
        timeSeparator: ":",
        universalSortableDateTimePattern: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
        yearMonthPattern: "'ខែ' MM 'ឆ្នាំ' yyyy",
        roundtripFormat: "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffzzz"
    }),

    TextInfo: HighFive.merge(new System.Globalization.TextInfo(), {
        ANSICodePage: 0,
        CultureName: "km-KH",
        EBCDICCodePage: 500,
        IsRightToLeft: false,
        LCID: 1107,
        listSeparator: ",",
        MacCodePage: 2,
        OEMCodePage: 1,
        IsReadOnly: true
    })
});