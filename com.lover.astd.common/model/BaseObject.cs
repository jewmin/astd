using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;

namespace com.lover.astd.common.model
{
    public class BaseObject : XmlObject
    {
        public override void Parse(XmlNode node)
        {
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == pi.Name)
                    {
                        if (pi.PropertyType.Name == "Int32")
                        {
                            object value = Convert.ToInt32(item.InnerText);
                            pi.SetValue(this, value, null);
                        }
                        else if (pi.PropertyType.Name == "String")
                        {
                            object value = item.InnerText;
                            pi.SetValue(this, value, null);
                        }
                        else if (pi.PropertyType.Name == "Single")
                        {
                            object value = Convert.ToSingle(item.InnerText);
                            pi.SetValue(this, value, null);
                        }
                        break;
                    }
                }
            }
        }

        public override bool CanAdd()
        {
            return true;
        }
    }
}
