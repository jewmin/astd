using System;
using System.Collections.Generic;
using System.Text;
using LuaInterface;
using System.Collections.Specialized;
using System.Xml;

namespace com.lover.astd.common.model
{
    public class AstdLuaObject
    {
        private Lua lua_state_;
        private string root_;

        public AstdLuaObject()
        {
            lua_state_ = new Lua();
            root_ = "";
        }

        public int GetIntValue(string fullpath)
        {
            string value = GetStringValue(fullpath);
            if (value.Equals("")) return 0;
            else return int.Parse(value);
        }

        static public int GetIntValue(LuaTable table, string field)
        {
            string value = GetStringValue(table, field);
            if (value.Equals("")) return 0;
            else return int.Parse(value);
        }

        public double GetDoubleValue(string fullpath)
        {
            return lua_state_.GetNumber(fullpath);
        }

        public string GetStringValue(string fullpath)
        {
            string value = lua_state_.GetString(fullpath);
            if (value == null) return "";
            else return value;
        }

        static public string GetStringValue(LuaTable table, string field)
        {
            object value = table[field];
            if (value == null) return "";
            else return (string)value;
        }

        public ListDictionary GetListValue(string fullpath)
        {
            LuaTable table = lua_state_.GetTable(fullpath);
            if (table == null)
            {
                return null;
            }
            return lua_state_.GetTableDict(table);
        }

        public void DumpLuaStack(ILogger logger)
        {
            LuaTable table = lua_state_.GetTable(root_);
            DumpLuaStack(table, root_, logger);
        }

        private void DumpLuaStack(LuaTable table, string name, ILogger logger)
        {
            if (table == null)
            {
                return;
            }

            foreach (object k in table.Keys)
            {
                object v = table[k];
                if (v is LuaTable)
                {
                    DumpLuaStack((LuaTable)v, string.Format("{0}.{1}", name, k), logger);
                }
                else
                {
                    logger.log(string.Format("{0}.{1} = {2}", name, k, v), System.Drawing.Color.Firebrick);
                }
            }
        }

        public void ParseXml(XmlNode node)
        {
            root_ = node.Name;
            ParseXml(node, node.Name, lua_state_);
        }

        private void ParseXml(XmlNode xml_node, string name, Lua lua_state, int index = 0)
        {
            if (xml_node == null)
            {
                return;
            }

            LuaTable table = lua_state.GetTable(name);
            if (table == null)
            {
                lua_state.NewTable(name);
                table = lua_state.GetTable(name);
            }
            if (index > 0)
            {
                name = string.Format("{0}.{1}", name, index);
                table = lua_state.GetTable(name);
                if (table == null)
                {
                    lua_state.NewTable(name);
                    table = lua_state.GetTable(name);
                }
            }

            Dictionary<string, int> indexs = new Dictionary<string, int>();
            foreach (XmlNode node in xml_node.ChildNodes)
            {
                if (node.Value != null)
                {
                    table[node.Name] = node.Value;
                }
                else if (node.HasChildNodes && node.FirstChild.Value != null)
                {
                    table[node.Name] = node.FirstChild.Value;
                }
                else
                {
                    if (xml_node.SelectNodes(node.Name).Count > 1)
                    {
                        if (indexs.ContainsKey(node.Name))
                        {
                            indexs[node.Name]++;
                        }
                        else
                        {
                            indexs[node.Name] = 1;
                        }
                        ParseXml(node, string.Format("{0}.{1}", name, node.Name), lua_state, indexs[node.Name]);
                    }
                    else
                    {
                        ParseXml(node, string.Format("{0}.{1}", name, node.Name), lua_state);
                    }
                }
            }
        }
    }
}
