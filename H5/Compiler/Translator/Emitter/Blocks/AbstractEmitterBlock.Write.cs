using H5.Contract;
using H5.Contract.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.CSharp;
using Newtonsoft.Json;

namespace H5.Translator
{
    public partial class AbstractEmitterBlock
    {
        public virtual int Level
        {
            get
            {
                return Emitter.Level;
            }
        }

        public virtual string RemoveTokens(string code)
        {
            return Emitter.AssemblyInfo.SourceMap.Enabled
                        ? SourceMapGenerator.tokenRegex.Replace(code, "")
                        : code;
        }

        public virtual void Indent()
        {
            Emitter.ResetLevel(Emitter.Level + 1);
        }

        public virtual void Outdent()
        {
            Emitter.ResetLevel(Emitter.Level - 1);
        }

        public virtual void ResetLevel(int level)
        {
            Emitter.ResetLevel(level);
        }

        public virtual void WriteSourceMapName(string name)
        {
            if (Emitter.AssemblyInfo.SourceMap.Enabled && !Emitter.EmitterOutput.Names.Contains(name))
            {
                Emitter.EmitterOutput.Names.Add(name);
            }
        }

        public virtual void WriteSequencePoint(DomRegion region)
        {
            if (Emitter.AssemblyInfo.SourceMap.Enabled)
            {
                var line = region.BeginLine;
                var column = region.BeginColumn;
                var idx = Emitter.SourceFileNameIndex;

                if (Emitter.TypeInfo.TypeDeclaration.HasModifier(Modifiers.Partial))
                {
                    var fn = Emitter.Translator.EmitNode.GetParent<SyntaxTree>().FileName;

                    if (fn != null && fn.Length > 0)
                    {
                        idx = Emitter.SourceFiles.IndexOf(fn);

                        if (idx == -1)
                        {
                            idx = Emitter.SourceFileNameIndex;
                        }
                    }
                }
                var point = string.Format("/*##|{0},{1},{2}|##*/", idx, line, column);

                if (Emitter.LastSequencePoint != point)
                {
                    Emitter.LastSequencePoint = point;
                    Write(point);
                }
            }
        }

        public virtual void WriteIndent()
        {
            if (!Emitter.IsNewLine)
            {
                return;
            }

            for (var i = 0; i < Level; i++)
            {
                Emitter.Output.Append(H5.Translator.Emitter.INDENT);
            }

            Emitter.IsNewLine = false;
        }

        public virtual void WriteNewLine()
        {
            Emitter.Output.Append(H5.Translator.Emitter.NEW_LINE);
            Emitter.IsNewLine = true;
        }

        public virtual void BeginBlock()
        {
            WriteOpenBrace();
            WriteNewLine();
            Indent();
        }

        public virtual void EndBlock()
        {
            Outdent();
            WriteCloseBrace();
        }

        /// <summary>
        /// Used to write simple blocks. Optionally doesn't end the block with a newline.
        /// </summary>
        /// <param name="what"></param>
        /// <param name="newline"></param>
        public virtual void WriteBlock(string what, bool newline = true)
        {
            BeginBlock();
            Write(what);
            WriteNewLine();
            EndBlock();

            if (newline)
            {
                WriteNewLine();
            }
        }
        public void Write(string value)
        {
            WriteIndent();
            Emitter.Output.Append(value);
        }

        public void Write(object value)
        {
            WriteIndent();
            Emitter.Output.Append(value);
        }

        public void Write(params string[] values)
        {
            foreach (var item in values)
            {
                Write(item);
            }
        }

        public void Write(params object[] values)
        {
            foreach (var item in values)
            {
                Write(item);
            }
        }

        public void WriteScript(string value)
        {
            WriteIndent();

            var s = Emitter.ToJavaScript(value);

            Emitter.Output.Append(s);
        }

        public void WriteScript<T>(T value)
        {
            WriteIndent();

            var s = ToJavaScript(value, Emitter);

            Emitter.Output.Append(s);
        }

        public static string ToJavaScript<T>(T value, IEmitter emitter)
        {
            string s = null;

            switch (value)
            {
                case double d:
                    if (double.IsNaN(d))
                    {
                        s = JS.Types.Number.NaN;
                    }
                    else if (double.IsPositiveInfinity(d))
                    {
                        s = JS.Types.Number.Infinity;
                    }
                    else if (double.IsNegativeInfinity(d))
                    {
                        s = JS.Types.Number.InfinityNegative;
                    }
                    else
                    {
                        s = JsonConvert.ToString(d);
                        //s = emitter.ToJavaScript(value);
                    }
                    break;
                case float f:
                    if (float.IsNaN(f))
                    {
                        s = JS.Types.Number.NaN;
                    }
                    else if (float.IsPositiveInfinity(f))
                    {
                        s = JS.Types.Number.Infinity;
                    }
                    else if (float.IsNegativeInfinity(f))
                    {
                        s = JS.Types.Number.InfinityNegative;
                    }
                    else
                    {
                        s = JsonConvert.ToString(f);
                        //s = emitter.ToJavaScript(value);
                    }
                    break;
                case char ch:
                    s = JsonConvert.ToString((int)ch);
                    //s = emitter.ToJavaScript((int)ch);
                    break;
                case decimal dec:
                {
                    var tmp = dec.ToString(CultureInfo.InvariantCulture);
                    s = JS.Types.SYSTEM_DECIMAL + "(" + DecimalConstant(dec, emitter);

                    int dot;
                    if ((dot = tmp.IndexOf(".")) >= 0)
                    {
                        s += ", " + tmp.Substring(dot + 1).Length;
                    }

                    s += ")";
                    break;
                }

                case long l:
                    s = JS.Types.System.Int64.NAME + "(" + LongConstant(l, emitter) + ")";
                    break;
                case ulong ul:
                    s = JS.Types.SYSTEM_UInt64 + "(" + ULongConstant(ul, emitter) + ")";
                    break;
                case int i:
                    s = JsonConvert.ToString(i);
                    //s = emitter.ToJavaScript((int)ch);
                    break;
                case uint ui:
                    s = JsonConvert.ToString(ui);
                    //s = emitter.ToJavaScript((int)ch);
                    break;
                case bool b:
                    s = b ? "true" : "false";
                    //s = emitter.ToJavaScript((int)ch);
                    break;
                case string str:
                    s = JsonConvert.ToString(str, '"', StringEscapeHandling.EscapeNonAscii);
                    //s = emitter.ToJavaScript((int)ch);
                    break;
                default:
                    s = emitter.ToJavaScript(value);
                    break;
            }
            return s;
        }

        public virtual void WriteLines(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                Write(line.Replace(H5.Translator.Emitter.CRLF, H5.Translator.Emitter.NEW_LINE));
                WriteNewLine();
            }
        }

        public virtual void WriteLines(params string[] lines)
        {
            WriteLines((IEnumerable<string>)lines);
        }

        /// <summary>
        /// Takes an array of strings to put into Emitter's output.
        /// Each array element is considered as a separate line (i.e. new line character appended) and not expected to contain new line character at the end.
        /// Set lineStartOffset to trim first lineStartOffset whitespaces.
        /// Use wrappedStart to add it before the first line and use wrappedEnd to add it after the last line.
        /// Use alignedIndent to align whitespace to match indent levels (i.e. first one whitespaces gets three extra ones, six became eight whitespaces etc).
        /// </summary>
        /// <param name="lines">Strings to put into Emitter's output, line by line.</param>
        /// <param name="lineStartOffset">Offset in lines to trim whitespaces.</param>
        /// <param name="wrappedStart">Suffix for the first line.</param>
        /// <param name="wrappedEnd">Postfix for the last line.</param>
        /// <param name="alignedIndent">Aligns each line to match indent levels (i.e. first one whitespaces gets three extra ones, six became eight whitespaces etc).</param>
        public virtual void WriteLinesIndented(string[] lines, int lineStartOffset = 0, string wrappedStart = null, string wrappedEnd = null, bool alignedIndent = false)
        {
            if (lines == null)
            {
                return;
            }

            for (int j = 0; j < lines.Length; j++)
            {
                var line = lines[j];

                line = IndentLine(line, lineStartOffset, alignedIndent);

                if (line == null)
                {
                    line = string.Empty;
                }

                if (j == 0 && !string.IsNullOrEmpty(wrappedStart))
                {
                    line = wrappedStart + line;
                }

                if (j == lines.Length - 1 && !string.IsNullOrEmpty(wrappedEnd))
                {
                    line = line + wrappedEnd;
                }

                if (!string.IsNullOrEmpty(line))
                {
                    WriteIndent();

                    Emitter.Output.Append(line);
                }

                WriteNewLine();
            }
        }

        /// <summary>
        /// First whitespace symbols get removed (i.e shifts the line on startOffset whitespace symbols (or less if not present) to the left.
        /// Then aligns remaining whitespaces to match indent level if alignedIndent is true.
        /// </summary>
        /// <param name="line">The string to process.</param>
        /// <param name="startOffset">The number of whitespaces to remove from the beginning, ignored if negative or zero.</param>
        /// <param name="alignedIndent">Specifies if whitespaces should be aligned by indent level.</param>
        /// <returns>The string with removed first startOffset whitespace symbols and aligned by indent level if required.</returns>
        private static string IndentLine(string line, int startOffset, bool alignedIndent)
        {
            if (string.IsNullOrEmpty(line))
            {
                return line;
            }

            if (startOffset <= 0 && !alignedIndent)
            {
                return line;
            }

            if (startOffset < 0)
            {
                startOffset = 0;
            }

            int firstWhitespaceCount = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    firstWhitespaceCount++;
                }
                else
                {
                    break;
                }
            }

            int trimIndex = firstWhitespaceCount > startOffset
                ? startOffset
                : firstWhitespaceCount;

            if (trimIndex > 0)
            {
                line = line.Substring(trimIndex);
            }

            if (alignedIndent)
            {
                int indentIndex = firstWhitespaceCount - trimIndex;

                var indentLength = H5.Translator.Emitter.INDENT.Length;

                if (indentIndex % indentLength > 0)
                {
                    var level = indentIndex / indentLength;
                    var fullLevelLength = (level + 1) * indentLength;
                    var restLevelLength = fullLevelLength - indentIndex;

                    line = line.PadLeft(line.Length + restLevelLength);
                }
            }

            return line;
        }

        public static string DecimalConstant(decimal value, IEmitter emitter)
        {
            string s = null;
            bool similar = false;

            try
            {
                similar = (decimal)(double)value == value;
            }
            catch
            {
            }

            if (similar)
            {
                s = emitter.ToJavaScript(value);
                if (CultureInfo.InstalledUICulture.CompareInfo.IndexOf(s, "e", CompareOptions.IgnoreCase) > -1)
                {
                    s = emitter.ToJavaScript(s);
                }
            }
            else
            {
                s = emitter.ToJavaScript(value.ToString(CultureInfo.InvariantCulture));
            }

            return s;
        }

        public static string LongConstant(long value, IEmitter emitter)
        {
            if (value > Int32.MaxValue || value < Int32.MinValue)
            {
                int l1 = (int)(value & uint.MaxValue);
                int l2 = (int)(value >> 32);

                return emitter.ToJavaScript(new int[] { l1, l2 });
            }

            return emitter.ToJavaScript(value);
        }

        public static string ULongConstant(ulong value, IEmitter emitter)
        {
            if (value > UInt32.MaxValue)
            {
                int l1 = (int)(value & uint.MaxValue);
                int l2 = (int)(value >> 32);

                return emitter.ToJavaScript(new int[] { l1, l2 });
            }

            return emitter.ToJavaScript(value);
        }

        public virtual void WriteCall(object callee = null)
        {
            WriteDot();

            if (callee == null)
            {
                Write(JS.Funcs.CALL);
            }
            else
            {
                Write(JS.Funcs.CALL);
                WriteOpenParentheses();
                Write(callee);
                WriteCloseParentheses();
            }
        }

        public virtual void WriteComma()
        {
            WriteComma(false);
        }

        public virtual void WriteComma(bool newLine)
        {
            Write(",");

            if (newLine)
            {
                WriteNewLine();
            }
            else
            {
                WriteSpace();
            }
        }

        public static string GetThisAlias(IEmitter emitter)
        {
            return "this";
        }

        public virtual void WriteThis()
        {
            Write(GetThisAlias(Emitter));
            Emitter.ThisRefCounter++;
        }

        public virtual void WriteSpace()
        {
            WriteSpace(true);
        }

        public virtual void WriteSpace(bool addSpace)
        {
            if (addSpace)
            {
                Write(" ");
            }
        }

        public virtual void WriteDot()
        {
            Write(".");
        }

        public virtual void WriteColon()
        {
            Write(": ");
        }

        public virtual void WriteSemiColon()
        {
            WriteSemiColon(false);
        }

        public virtual void WriteSemiColon(bool newLine)
        {
            if (Emitter.SkipSemiColon)
            {
                Emitter.SkipSemiColon = false;
                return;
            }

            Write(";");

            if (newLine)
            {
                WriteNewLine();
            }
        }

        public virtual void WriteNew()
        {
            Write("new ");
        }

        public virtual void WriteVar(bool ignoreAsync = false)
        {
            if (!Emitter.IsAsync || ignoreAsync || Emitter.IsNativeAsync)
            {
                Write("var ");
            }
        }

        public virtual void WriteIf()
        {
            Write("if ");
        }

        public virtual void WriteElse()
        {
            Write("else ");
        }

        public virtual void WriteWhile()
        {
            Write("while ");
        }

        public virtual void WriteFor()
        {
            Write("for ");
        }

        public virtual void WriteThrow()
        {
            Write("throw ");
        }

        public virtual void WriteTry()
        {
            Write("try ");
        }

        public virtual void WriteCatch()
        {
            Write("catch ");
        }

        public virtual void WriteFinally()
        {
            Write("finally ");
        }

        public virtual void WriteDo()
        {
            Write("do");
        }

        public virtual void WriteSwitch()
        {
            Write("switch ");
        }

        public virtual void WriteReturn(bool addSpace)
        {
            Write("return");
            WriteSpace(addSpace);
        }

        public virtual void WriteOpenBracket()
        {
            WriteOpenBracket(false);
        }

        public virtual void WriteOpenBracket(bool addSpace)
        {
            Write("[");
            WriteSpace(addSpace);
        }

        public virtual void WriteCloseBracket()
        {
            WriteCloseBracket(false);
        }

        public virtual void WriteCloseBracket(bool addSpace)
        {
            WriteSpace(addSpace);
            Write("]");
        }

        public virtual void WriteOpenParentheses()
        {
            WriteOpenParentheses(false);
        }

        public virtual void WriteOpenParentheses(bool addSpace)
        {
            Write("(");
            WriteSpace(addSpace);
        }

        public virtual void WriteCloseParentheses()
        {
            WriteCloseParentheses(false);
        }

        public virtual void WriteCloseParentheses(bool addSpace)
        {
            WriteSpace(addSpace);
            Write(")");
        }

        public virtual void WriteOpenCloseParentheses()
        {
            WriteOpenCloseParentheses(false);
        }

        public virtual void WriteOpenCloseParentheses(bool addSpace)
        {
            Write("()");
            WriteSpace(addSpace);
        }

        public virtual void WriteOpenBrace()
        {
            WriteOpenBrace(false);
        }

        public virtual void WriteOpenBrace(bool addSpace)
        {
            Write("{");
            WriteSpace(addSpace);
        }

        public virtual void WriteCloseBrace()
        {
            WriteCloseBrace(false);
        }

        public virtual void WriteCloseBrace(bool addSpace)
        {
            WriteSpace(addSpace);
            Write("}");
        }

        public virtual void WriteOpenCloseBrace()
        {
            Write("{ }");
        }

        public virtual void WriteFunction()
        {
            Write("function ");
        }

        public virtual void PushWriter(string format, Action callback = null, string thisArg = null, int[] ignoreRange = null)
        {
            Emitter.Writers.Push(new Writer { InlineCode = format, Output = Emitter.Output, IsNewLine = Emitter.IsNewLine, Callback = callback, ThisArg = thisArg, IgnoreRange = ignoreRange });
            Emitter.IsNewLine = false;
            Emitter.Output = new StringBuilder();
        }

        public virtual string PopWriter(bool preventWrite = false)
        {
            string result = Emitter.Output.ToString();
            var writer = Emitter.Writers.Pop();
            Emitter.Output = writer.Output;
            result = writer.InlineCode != null ? string.Format(writer.InlineCode, result) : result;
            Emitter.IsNewLine = writer.IsNewLine;

            if (!preventWrite)
            {
                Write(result);
            }

            if (writer.Callback != null)
            {
                writer.Callback.Invoke();
            }

            return result;
        }

        public virtual string WriteIndentToString(string value)
        {
            return WriteIndentToString(value, Level);
        }

        public static string UpdateIndentsInString(string value, int level)
        {
            StringBuilder output = new StringBuilder();

            for (var i = 0; i < level; i++)
            {
                output.Append(H5.Translator.Emitter.INDENT);
            }

            string indent = output.ToString();

            return Regex.Replace(value, "^(\\s*)(.*)$", (m) =>
            {
                return indent + m.Groups[2].Value;
            }, RegexOptions.Multiline);
        }

        public static string WriteIndentToString(string value, int level)
        {
            StringBuilder output = new StringBuilder();

            for (var i = 0; i < level; i++)
            {
                output.Append(H5.Translator.Emitter.INDENT);
            }

            string indent = output.ToString();

            return Regex.Replace(value, H5.Translator.Emitter.NEW_LINE + "(?!\\s*$)(.+)", (m) =>
            {
                return H5.Translator.Emitter.NEW_LINE + indent + m.Groups[1].Value;
            }, RegexOptions.Multiline);
        }

        public virtual void EnsureComma(bool newLine = true)
        {
            if (Emitter.Comma)
            {
                WriteComma(newLine);
                Emitter.Comma = false;
            }
        }

        public IWriterInfo SaveWriter()
        {
            /*if (this.Emitter.LastSavedWriter != null && this.Emitter.LastSavedWriter.Output == this.Emitter.Output)
            {
                this.Emitter.LastSavedWriter.IsNewLine = this.Emitter.IsNewLine;
                this.Emitter.LastSavedWriter.Level = this.Emitter.Level;
                this.Emitter.LastSavedWriter.Comma = this.Emitter.Comma;
                return this.Emitter.LastSavedWriter;
            }*/

            var info = new WriterInfo
            {
                Output = Emitter.Output,
                IsNewLine = Emitter.IsNewLine,
                Level = Emitter.Level,
                Comma = Emitter.Comma
            };

            Emitter.LastSavedWriter = info;

            return info;
        }

        public bool RestoreWriter(IWriterInfo writer)
        {
            if (Emitter.Output != writer.Output)
            {
                Emitter.Output = writer.Output;
                Emitter.IsNewLine = writer.IsNewLine;
                Emitter.ResetLevel(writer.Level);
                Emitter.Comma = writer.Comma;

                return true;
            }

            return false;
        }

        public StringBuilder NewWriter()
        {
            Emitter.Output = new StringBuilder();
            Emitter.IsNewLine = false;
            Emitter.ResetLevel();
            Emitter.Comma = false;

            return Emitter.Output;
        }

        public int GetNumberOfEmptyLinesAtEnd()
        {
            return GetNumberOfEmptyLinesAtEnd(Emitter.Output);
        }

        public static int GetNumberOfEmptyLinesAtEnd(StringBuilder buffer)
        {
            int count = 0;
            bool lastNewLineFound = false;
            int i = buffer.Length - 1;
            var charArray = buffer.ToString().ToCharArray();

            while (i >= 0)
            {
                char c = charArray[i];

                if (!Char.IsWhiteSpace(c))
                {
                    return count;
                }

                if (c == H5.Translator.Emitter.NEW_LINE_CHAR)
                {
                    if (!lastNewLineFound)
                    {
                        lastNewLineFound = true;
                    }
                    else
                    {
                        count++;
                        ;
                    }
                }
                i--;
            }

            return count;
        }

        public bool IsOnlyWhitespaceOnPenultimateLine(bool lastTwoLines = true, string output = null)
        {
            return IsOnlyWhitespaceOnPenultimateLine(output ?? Emitter.Output.ToString(), lastTwoLines);
        }

        public static bool IsOnlyWhitespaceOnPenultimateLine(string buffer, bool lastTwoLines = true)
        {
            int i = buffer.Length - 1;
            var charArray = buffer.ToCharArray();

            while (i >= 0)
            {
                char c = charArray[i];

                if (!Char.IsWhiteSpace(c))
                {
                    return false;
                }

                if (c == H5.Translator.Emitter.NEW_LINE_CHAR)
                {
                    if (lastTwoLines)
                    {
                        lastTwoLines = false;
                    }
                    else
                    {
                        return true;
                    }
                }

                i--;
            }

            return true;
        }

        public bool RemovePenultimateEmptyLines(bool withLast = false)
        {
            if (Emitter.Output != null)
            {
                return RemovePenultimateEmptyLines(Emitter.Output, withLast);
            }

            return false;
        }

        /// <summary>
        /// Splits the input string into lines by CRLF and LF,
        /// replaces lines containing only whitespaces into empty lines and
        /// optionally replaces first (ignoring whitespaces) asterisk symbol in each line with whitespace.
        /// </summary>
        /// <param name="s">Input string.</param>
        /// <param name="removeFirstAsterisk">Specifies whether to replace first (ignoring whitespaces) asterisk symbol in each line with whitespace</param>
        /// <returns>String array representing the input line splittted by lines, first asterisk symbol removed if required.</returns>
        public string[] GetNormalizedWhitespaceAndAsteriskLines(string s, bool removeFirstAsterisk)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new[] { s };
            }

            var lines = s.Split(new string[] { H5.Translator.Emitter.CRLF, H5.Translator.Emitter.NEW_LINE }, StringSplitOptions.None);

            lines = lines.Select(
                x =>
                    {
                        if (string.IsNullOrEmpty(x))
                        {
                            return string.Empty;
                        }

                        if (removeFirstAsterisk)
                        {
                            var asteriskIndex = -1;

                            for (int i = 0; i < x.Length; i++)
                            {
                                if (x[i] != ' ')
                                {
                                    if (x[i] == '*')
                                    {
                                        asteriskIndex = i;
                                    }

                                    break;
                                }
                            }

                            if (asteriskIndex >= 0)
                            {
                                var ie = asteriskIndex + 1 > x.Length ? x.Length : asteriskIndex + 1;
                                x = x.Substring(0, asteriskIndex) + " " + x.Substring(ie);
                            }
                        }

                        return x.All(c => c == ' ') ? string.Empty : x;
                    }).ToArray();

            return lines;
        }

        public static bool RemovePenultimateEmptyLines(StringBuilder buffer, bool withLast = false)
        {
            bool removed = false;
            if (buffer.Length != 0)
            {
                int length = buffer.Length;
                int i = length - 1;
                var charArray = buffer.ToString().ToCharArray();
                int start = -1;
                int end = -1;
                bool firstCR = true;

                while (Char.IsWhiteSpace(charArray[i]) && (i > -1))
                {
                    if (charArray[i] == H5.Translator.Emitter.NEW_LINE_CHAR)
                    {
                        if (firstCR)
                        {
                            firstCR = false;
                            end = i;

                            if (withLast)
                            {
                                start = i;
                            }
                        }
                        else
                        {
                            start = i;
                        }
                    }

                    i--;
                }

                if (start > -1 && end > -1)
                {
                    buffer.Remove(start, end - start + 1);
                    removed = true;
                }
            }
            return removed;
        }

        public static bool IsReturnLast(string str)
        {
            str = str.TrimEnd();
            return str.EndsWith("return;") || Regex.IsMatch(str, "(?m:^)return\\s(.*)?;$");
        }

        public static bool IsContinueLast(string str)
        {
            str = str.TrimEnd();
            return str.EndsWith("continue;");
        }

        public static bool IsJumpStatementLast(string str)
        {
            str = str.TrimEnd();
            return str.EndsWith("continue;") || str.EndsWith("break;") || IsReturnLast(str);
        }
    }

    public class WriterInfo : IWriterInfo
    {
        public StringBuilder Output { get; set; }

        public bool IsNewLine { get; set; }

        public int Level { get; set; }

        public bool Comma { get; set; }
    }

    public class Writer : IWriter
    {
        public StringBuilder Output { get; set; }

        public bool IsNewLine { get; set; }

        public string InlineCode { get; set; }

        public Action Callback { get; set; }

        public string ThisArg { get; set; }

        public int[] IgnoreRange { get; set; }
    }
}