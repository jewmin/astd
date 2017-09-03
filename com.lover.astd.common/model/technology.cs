using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    public class Technology : XmlObject
    {
        /// <summary>
        /// 科技id
        /// </summary>
        public int techid_;
        /// <summary>
        /// 科技名称
        /// </summary>
        public string techname_;
        /// <summary>
        /// 科技简介
        /// </summary>
        public string intro_;
        /// <summary>
        /// 科技等级
        /// </summary>
        public int techlevel_;
        /// <summary>
        /// 研究进度
        /// </summary>
        public int progress_;
        /// <summary>
        /// 要求进度
        /// </summary>
        public int requireprogress_;
        /// <summary>
        /// 需要消耗镔铁
        /// </summary>
        public int consumenum_;
        /// <summary>
        /// 效果
        /// </summary>
        public string effectvalue_;
        /// <summary>
        /// 消耗类型
        /// </summary>
        public string consumerestype_;
        /// <summary>
        /// 解析xml
        /// </summary>
        /// <param name="node"></param>
        //public void handle(XmlNode node)
        //{
        //    if (node == null) return;
        //    AstdLuaObject lua = new AstdLuaObject();
        //    lua.ParseXml(node);
        //    techid_ = lua.GetIntValue("technology.techid");
        //    techname_ = lua.GetStringValue("technology.techname");
        //    intro_ = lua.GetStringValue("technology.intro");
        //    techlevel_ = lua.GetIntValue("technology.techlevel");
        //    progress_ = lua.GetIntValue("technology.progress");
        //    requireprogress_ = lua.GetIntValue("technology.requireprogress");
        //    consumebintie_ = lua.GetIntValue("technology.consumebintie");
        //    effectvalue_ = lua.GetStringValue("technology.effectvalue");
        //}
        /// <summary>
        /// <techid>8</techid>
        /// <techname>雁行阵</techname>
        /// <intro>升级增加部队防御能力</intro>
        /// <pic>new_tech_8</pic>
        /// <consumerestype>bintie</consumerestype>
        /// <techlevel>4</techlevel>
        /// <progress>214</progress>
        /// <requireprogress>500</requireprogress>
        /// <consumenum>500</consumenum>
        /// <effectvalue>0.015</effectvalue>
        /// </summary>
        /// <param name="node"></param>
        public override void Parse(XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "techid") techid_ = int.Parse(item.InnerText);
                else if (item.Name == "techname") techname_ = item.InnerText;
                else if (item.Name == "intro") intro_ = item.InnerText;
                else if (item.Name == "techlevel") techlevel_ = int.Parse(item.InnerText);
                else if (item.Name == "progress") progress_ = int.Parse(item.InnerText);
                else if (item.Name == "requireprogress") requireprogress_ = int.Parse(item.InnerText);
                else if (item.Name == "consumenum") consumenum_ = int.Parse(item.InnerText);
                else if (item.Name == "effectvalue") effectvalue_ = item.InnerText;
                else if (item.Name == "consumerestype") consumerestype_ = item.InnerText;
            }
        }

        public override bool CanAdd()
        {
            return true;
        }
    }
}
