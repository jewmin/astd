using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;
using System.Globalization;

namespace System.Net.Json
{
    public sealed class JsonTextParser
    {
        /// <summary>
        /// current position.
        /// </summary>
        private int c = 0;

        /// <summary>
        /// json text to parse
        /// </summary>
        private string s = string.Empty;

        /// <summary>
        /// sync object for thread locking.
        /// </summary>
        private object SyncObject = new object();

        /// <summary>
        /// Parses JSON text into JSonObject.
        /// </summary>
        /// <param name="text">JSON text to parse.</param>
        /// <returns>Json object parsed from text.</returns>
        public JsonObject Parse(string text)
        {
            lock (SyncObject)
            {
                c = 0;

                if (text == null)
                {
                    throw new FormatException();
                }

                s = text.Trim();
                if (s == string.Empty)
                {
                    throw new FormatException();
                }
#if !DEBUG
                try
#endif
                {
                    return ParseSomethingWithoutName();
                }
#if !DEBUG
                catch (Exception)
                {
                    throw;
                }
#endif
            }
        }

        private bool IsEOS { get { return c >= s.Length; } }
        private char cur { get { return s[c]; } }

        private void SkipWhiteSpace()
        {
            for (; ; )
            {
                if (IsEOS)
                {
                    break;
                }

                if (char.IsWhiteSpace(s[c]))
                {
                    c++;
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (IsEOS)
            {
                throw new FormatException();
            }
        }

        private static readonly Regex _regexNumber = new Regex(
                @"(?<minus>[-])?(?<int>(0)|([1-9])[0-9]*)(?<frac>.[0-9]+)?(?<exp>(e|E)([-]|[+])[0-9]+)?",
                RegexOptions.Compiled);

        private JsonObject ParseSomethingWithoutName()
        {
            SkipWhiteSpace();

            // collection?
            if (s[c] == '{' | s[c] == '[')
            {
                return ParseCollection();
            }

            // string?
            if (s[c] == '"')
            {
                return ParseStringValue();
            }

            // number?
            if (char.IsDigit(s[c]) || s[c] == '-')
            {
                return ParseNumericValue();
            }

            // literal?
            if (s[c] == 't' || s[c] == 'f' || s[c] == 'n')
            {
                return ParseLiteralValue();
            }

            throw new FormatException("Cannot parse a value.");
        }

        private JsonStringValue ParseStringValue()
        {
            if (s[c] != '"')
            {
                throw new FormatException();
            }

            // skip open quote
            c++;

            StringBuilder value = new StringBuilder();

            for (; ; c++)
            {
                if (IsEOS)
                {
                    throw new FormatException();
                }

                if (s[c] == '"')
                {
                    if (s[c - 1] != '\\')
                    {
                        break;
                    }
                }
                
                value.Append(s[c]);
            }

            if (s[c] != '"')
            {
                throw new FormatException();
            }

            c++;

            JsonStringValue result = new JsonStringValue();
            result.Value = JsonUtility.UnEscapeString(value.ToString());
            return result;
        }

        private static readonly Regex _regexLiteral = new Regex(
                @"(?<value>false|true|null)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private JsonBooleanValue ParseLiteralValue()
        {
            Match m = _regexLiteral.Match(s, c);
            if (!m.Success)
            {
                throw new FormatException("Cannot parse a literal value.");
            }

            string captured = m.Captures[0].Value;
            c += captured.Length;

            return new JsonBooleanValue(null, captured);
        }

        private JsonNumericValue ParseNumericValue()
        {
            Match mnum = _regexNumber.Match(s, c);
            if (!mnum.Success)
            {
                throw new FormatException("Cannot parse a number value.");
            }

            string captured = mnum.Captures[0].Value;
            c += captured.Length;

            double value = double.Parse(captured,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                JsonUtility.CultureInfo);

            return new JsonNumericValue(value);
        }

        private JsonCollection ParseCollection()
        {
            SkipWhiteSpace();

            JsonCollection result;
            bool is_array = false;

            if (s[c] == '{')
            {
                is_array = false;
                result = new JsonObjectCollection();
            }
            else if (s[c] == '[')
            {
                is_array = true;
                result = new JsonArrayCollection();
            }
            else
            {
                throw new FormatException();
            }

            // skip open bracket
            c++;
            SkipWhiteSpace();

            // parse collection items
            for (; ; )
            {
                string name = string.Empty;
                if (!is_array)
                {
                    name = ParseName();
                }

                JsonObject obj = ParseSomethingWithoutName();

                if (obj == null)
                {
                    throw new Exception();
                }

                // add name to item, if object.
                if (!is_array)
                {
                    obj.Name = name;
                }

                result.Add(obj);

                SkipWhiteSpace();

                if (s[c] == ',')
                {
                    c++;

                    SkipWhiteSpace();
                }
                else
                {
                    break;
                }
            }

            SkipWhiteSpace();
            
            if (is_array)
            {
                if (s[c] != ']')
                {
                    throw new FormatException();
                }
            }
            else
            {
                if (s[c] != '}')
                {
                    throw new FormatException();
                }
            }

            c++;

            return result;
        }

        string ParseName()
        {
            if (IsEOS)
            {
                throw new FormatException("Cannot find object item's name.");
            }

            if (s[c] != '"')
            {
                throw new FormatException();
            }

            // skip open quote
            c++;

            StringBuilder name = new StringBuilder();
            
            for (; ; c++)
            {
                if (IsEOS)
                {
                    throw new FormatException();
                }

                if (s[c] == '"')
                {
                    if (s[c-1] != '\\')
                    {
                        break;
                    }
                }

                name.Append(s[c]);
            }
            c ++;

            SkipWhiteSpace();
            if (IsEOS)
            {
                throw new FormatException();
            }
            if (s[c] != ':')
            {
                throw new FormatException();
            }
            c++;
            return name.ToString();
        }
    }
}