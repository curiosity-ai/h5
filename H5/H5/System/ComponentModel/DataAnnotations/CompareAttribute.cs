namespace System.ComponentModel.DataAnnotations
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class CompareAttribute : ValidationAttribute
    {
        public extern CompareAttribute(string otherProperty);

        public extern string OtherProperty { get; private set; }

        public extern string OtherPropertyDisplayName { get; internal set; }
    }
}
