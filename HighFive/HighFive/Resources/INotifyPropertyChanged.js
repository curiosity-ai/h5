    HighFive.define("System.ComponentModel.INotifyPropertyChanged", {
        $kind: "interface"
    });

    HighFive.define("System.ComponentModel.PropertyChangedEventArgs", {
        ctor: function (propertyName, newValue, oldValue) {
            this.$initialize();
            this.propertyName = propertyName;
            this.newValue = newValue;
            this.oldValue = oldValue;
        }
    });
