namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Sets the display column, the sort column, and the sort order for when a table is used as a parent table in FK
    /// relationships.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
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
