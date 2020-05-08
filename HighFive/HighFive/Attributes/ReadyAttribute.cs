namespace H5
{
    /// <summary>
    /// Makes the method to be called once the page is loaded. If using jQuery2, triggers jQuery's event,
    /// otherwise, uses DOMContentReady event from HTML5.
    /// </summary>
    [NonScriptable]
    public class ReadyAttribute : AdapterAttribute
    {
        public const string Format = "H5.ready(this.{2});";
        public const string FormatScope = "H5.ready(this.{2}, this);";
        public const string Event = "ready";
        public const bool StaticOnly = true;

        public ReadyAttribute()
        {
        }
    }
}