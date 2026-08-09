using System.Text;

namespace H5.Translator.Roslyn;

/// <summary>
/// Indentation-aware writer for emitting JavaScript.
/// </summary>
public sealed class JsWriter
{
    private readonly StringBuilder _sb = new();
    private int _indent;
    private bool _atLineStart = true;

    public const string IndentUnit = "    ";

    public JsWriter Indent()
    {
        _indent++;
        return this;
    }

    public JsWriter Outdent()
    {
        if (_indent > 0) _indent--;
        return this;
    }

    private void WriteIndentIfNeeded()
    {
        if (_atLineStart)
        {
            for (var i = 0; i < _indent; i++) _sb.Append(IndentUnit);
            _atLineStart = false;
        }
    }

    public JsWriter Write(string text)
    {
        if (string.IsNullOrEmpty(text)) return this;
        WriteIndentIfNeeded();
        _sb.Append(text);
        return this;
    }

    public JsWriter Write(char c)
    {
        WriteIndentIfNeeded();
        _sb.Append(c);
        return this;
    }

    public JsWriter WriteLine(string text = "")
    {
        if (text.Length > 0)
        {
            WriteIndentIfNeeded();
            _sb.Append(text);
        }
        _sb.Append('\n');
        _atLineStart = true;
        return this;
    }

    /// <summary>Opens a "{" block, indents, runs body, outdents and closes.</summary>
    public JsWriter Block(System.Action body, string open = "{", string close = "}")
    {
        WriteLine(open);
        Indent();
        body();
        Outdent();
        Write(close);
        return this;
    }

    public override string ToString() => _sb.ToString();
}
