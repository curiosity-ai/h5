namespace Bridge.Translator
{
    public class RawValue
    {
        public string Value
        {
            get;
            private set;
        }

        public RawValue(string value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}