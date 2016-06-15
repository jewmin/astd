using System;
using System.Collections.Generic;
using System.Text;

using System.Globalization;

namespace System.Net.Json
{
    public static class JsonUtility
    {
        public const string MimeType = "application/json";

        public static int MaxTextLength = -1;
        public static int MaxDepthNesting = -1;
        public static int MaxStringLength = 1024;


        internal const char begin_array =       '[';
        internal const char end_array =         ']';
        internal const char begin_object =      '{';
        internal const char end_object =        '}';
        internal const char name_separator =    ':';
        internal const char value_separator =   ',';
        internal const char quote =             '"';

        internal static readonly CultureInfo CultureInfo = new CultureInfo("en-US", false);


        

        internal static string EscapeNonPrintCharacter(char c)
        {
            
            const string start = "\\u";
            return start + ((int)c).ToString("x4");
        }

        internal static string EscapeString(string text)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append('"');
            foreach (char c in text)
            {
                //bool hex = false;

                if (c == '\"') sb.Append("\\\"");
                else if (c == '\\') sb.Append("\\\\");
                else if (c == '\b') sb.Append("\\b");
                else if (c == '\f') sb.Append("\\f");
                else if (c == '\n') sb.Append("\\n");
                else if (c == '\r') sb.Append("\\r");
                else if (c == '\t') sb.Append("\\t");
                else if (char.IsLetterOrDigit(c)) sb.Append(c);
                else if (char.IsPunctuation(c)) sb.Append(c);
                else if (char.IsSeparator(c)) sb.Append(c);
                else if (char.IsWhiteSpace(c)) sb.Append(c);
                else if (char.IsSymbol(c)) sb.Append(c);
                else
                {
                    sb.Append(EscapeNonPrintCharacter(c));
                }
                //else if (c >= 0x0020 && c <= 0x0021) hex = true;
                //else if (c >= 0x0023 && c <= 0x005b) hex = true;
                //else if (c >= 0x005d) hex = true;
            }
            sb.Append('"');

            return sb.ToString();
        }

        internal static string UnEscapeString(string text)
        {
            text = text.Trim();

            if (text.StartsWith("\"")) text = text.Remove(0, 1);
            if (text.EndsWith("\"")) text = text.Remove(text.Length - 1, 1);

            StringBuilder sb = new StringBuilder();

            try
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];

                    switch (c)
                    {
                        case '\\':
                            {
                                i++;
                                if (text[i] == 'u' || text[i] == 'U')
                                {
                                    string hex = text.Substring(i + 1, 4);
                                    char u = (char)int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

                                    i += 4;
                                    sb.Append(u);
                                    continue;
                                }

                                if (text[i] == 'n')
                                {
                                    sb.Append('\n');
                                    continue;
                                }

                                if (text[i] == 'r')
                                {
                                    sb.Append('\r');
                                    continue;
                                }

                                if (text[i] == 't')
                                {
                                    sb.Append('\t');
                                    continue;
                                }

                                if (text[i] == 'f')
                                {
                                    sb.Append('\f');
                                    continue;
                                }

                                if (text[i] == 'b')
                                {
                                    sb.Append('\b');
                                    continue;
                                }

                                if (text[i] == '\\')
                                {
                                    sb.Append('\\');
                                    continue;
                                }

                                if (text[i] == '/')
                                {
                                    sb.Append('/');
                                    continue;
                                }

                                if (text[i] == '"')
                                {
                                    sb.Append('"');
                                    continue;
                                }

                                throw new FormatException("Unrecognized escape sequence '\\" + text[i] +
                                    "' in position: " + i + ".");
                            }

                        default:
                            sb.Append(c);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sb.ToString();
        }


        #region Indentation

        public static bool GenerateIndentedJsonText = true;

        internal const char indent = '\t';
        internal const char space = ' ';
        internal static int ThreadId { get { return System.Threading.Thread.CurrentThread.ManagedThreadId; } }
        internal static readonly SortedDictionary<int, int> IndentDepthCollection = new SortedDictionary<int, int>();
        internal static int IndentDepth
        {
            get
            {
                int tid = ThreadId;
                try
                {
                    return IndentDepthCollection[tid];
                }
                catch (KeyNotFoundException)
                {
                    return 0;
                }
            }
            set
            {
                IndentDepthCollection[ThreadId] = value;
            }
        }

        internal static string GetIndentString()
        {
            int len = IndentDepth;
            if (len <= 0)
            {
                return string.Empty;
            }
            else
            {
                return new string(indent, len);
            }
        }

        internal static void WriteIndent(System.IO.TextWriter writer)
        {
            if (GenerateIndentedJsonText)
            {
                writer.Write(GetIndentString());
            }
        }

        internal static void WriteSpace(System.IO.TextWriter writer)
        {
            if (GenerateIndentedJsonText)
            {
                writer.Write(space);
            }
        }

        internal static void WriteLine(System.IO.TextWriter writer)
        {
            if (GenerateIndentedJsonText)
            {
                writer.Write(Environment.NewLine);
            }
        }

        #endregion
    }
}