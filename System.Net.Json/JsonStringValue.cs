using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Json
{
    public class JsonStringValue : JsonObject
    {
        public override object GetValue()
        {
            return Value;
        }

        private string _value;
        public string Value
        {
            get
            {
                if (_value != null)
                {
                    return _value;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _value = value;
            }
        }

        public JsonStringValue()
        {

        }

        public JsonStringValue(string name)
        {
            Name = name;
        }

        public JsonStringValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            JsonStringValue other = obj as JsonStringValue;
            if (other == null)
            {
                return false;
            }

            return (this.Value == other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override void WriteTo(System.IO.TextWriter writer)
        {
            if (Name != string.Empty)
            {
                writer.Write(JsonUtility.quote);
                writer.Write(Name);
                writer.Write(JsonUtility.quote);
                writer.Write(JsonUtility.name_separator);
                JsonUtility.WriteSpace(writer);
            }

            writer.Write(JsonUtility.EscapeString(Value));
        }
    }
}
