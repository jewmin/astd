using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Json
{
    public class JsonNumericValue : JsonObject
    {
        public override object GetValue()
        {
            return Value;
        }

        private double _value;
        public double Value
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

        public JsonNumericValue()
        {

        }
        public JsonNumericValue(string name)
        {
            Name = name;
        }

        public JsonNumericValue(string name, double value)
        {
            Name = name;
            Value = value;
        }
        public JsonNumericValue(double value)
        {
            Value = value;
        }

        public JsonNumericValue(int value)
        {
            Value = (double)value;
        }

        public JsonNumericValue(string name, int value)
        {
            Name = name;
            Value = (double)value;
        }

        public JsonNumericValue(float value)
        {
            Value = (double)value;
        }

        public JsonNumericValue(string name, float value)
        {
            Name = name;
            Value = (double)value;
        }

        public JsonNumericValue(long value)
        {
            Value = (double)value;
        }

        public JsonNumericValue(string name, long value)
        {
            Name = name;
            Value = (double)value;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            JsonNumericValue other = obj as JsonNumericValue;
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

            writer.Write(Value.ToString("g", JsonUtility.CultureInfo));
        }
    }
}
