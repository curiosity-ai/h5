namespace H5.Translator
{
    public class RawValue
    {
        public string Value { get; private set; }

        public RawValue(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}