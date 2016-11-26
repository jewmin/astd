using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    public class AstdObject
    {
        private string value_;
        public AstdObject()
        {
            value_ = null;
        }
        public AstdObject(string value)
        {
            value_ = value;
        }
        public int IntValue
        {
            get { return int.Parse(value_); }
        }
        public double DoubleValue
        {
            get { return double.Parse(value_); }
        }
        public string StringValue
        {
            get { return value_; }
        }
        public override string ToString()
        {
            return value_;
        }

        public static object ParseXmlNode(XmlNode node)
        {
            if (node == null) return null;
            if (node.HasChildNodes)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Text)
                    {
                        return new AstdObject(child.InnerText);
                    }

                    if (result.ContainsKey(child.Name))
                    {
                        (result[child.Name] as List<object>).Add(ParseXmlNode(child));
                    }
                    else
                    {
                        List<object> value = new List<object>();
                        value.Add(ParseXmlNode(child));
                        result[child.Name] = value;
                    }
                }
                return result;
            }
            else
            {
                return new AstdObject(node.InnerText);
            }
        }
    }
}
