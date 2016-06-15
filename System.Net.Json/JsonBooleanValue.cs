using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Json
{
    public class JsonBooleanValue : JsonObject
    {
        private bool? _value = new bool?();
        public bool? Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public JsonBooleanValue()
        {

        }

        public JsonBooleanValue(string name)
        {
        }

        public JsonBooleanValue(string name, string value)
        {
            this.Name = name;

            _value = null;

            if (value != null)
            {
                value = value.Trim().ToLower();
                if (value != string.Empty)
                {
                    switch (value.Trim().ToLower())
                    {
                        case "null":
                            _value = null;
                            break;

                        case "true":
                            _value = true;
                            break;

                        case "false":
                            _value = false;
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }



        public JsonBooleanValue(string name, bool? value)
        {
            this.Name = name;
            this.Value = value;
        }

        public JsonBooleanValue(bool? value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            JsonBooleanValue other = obj as JsonBooleanValue;
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

            if (Value.HasValue)
            {
                writer.Write(Value.ToString().ToLower());
            }
            else
            {
                writer.Write("null");
            }
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}