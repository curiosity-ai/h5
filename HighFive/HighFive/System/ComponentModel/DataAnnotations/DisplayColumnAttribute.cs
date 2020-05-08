namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Sets the display column, the sort column, and the sort order for when a table is used as a parent table in FK
    /// relationships.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public class DisplayColumnAttribute : Attribute
    {
        public extern DisplayColumnAttribute(string displayColumn);

        public extern DisplayColumnAttribute(string displayColumn, string sortColumn);

        public extern DisplayColumnAttribute(string displayColumn, string sortColumn, bool sortDescending);

        public extern string DisplayColumn { get; }

        public extern string SortColumn { get; }

        public extern bool SortDescending { get; }
    }
}
