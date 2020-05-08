namespace System.ComponentModel.DataAnnotations
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public class CompareAttribute : ValidationAttribute
    {
        public extern CompareAttribute(string otherProperty);

        public extern string OtherProperty { get; private set; }

        public extern string OtherPropertyDisplayName { get; internal set; }
    }
}
