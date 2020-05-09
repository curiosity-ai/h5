namespace System.ComponentModel
{
    [H5.External]
    public interface INotifyPropertyChanged : H5.IH5Class
    {
        event PropertyChangedEventHandler PropertyChanged;
    }

    [H5.Name("Function")]
    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public class PropertyChangedEventArgs : H5.IH5Class
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