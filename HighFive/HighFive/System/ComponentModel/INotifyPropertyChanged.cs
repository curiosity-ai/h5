namespace System.ComponentModel
{
    [HighFive.External]
    public interface INotifyPropertyChanged : HighFive.IHighFiveClass
    {
        event PropertyChangedEventHandler PropertyChanged;
    }

    [HighFive.Name("Function")]
    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public class PropertyChangedEventArgs : HighFive.IHighFiveClass
    {
        public PropertyChangedEventArgs(string propertyName)
        {
        }

        public PropertyChangedEventArgs(string propertyName, object newValue)
        {
        }

        public PropertyChangedEventArgs(string propertyName, object newValue, object oldValue)
        {
        }

        public readonly string PropertyName;
        public readonly object OldValue;
        public readonly object NewValue;
    }
}