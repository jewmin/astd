using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    public class Technology
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
        public int consumebintie_;
        /// <summary>
        /// 效果
        /// </summary>
        public string effectvalue_;
        /// <summary>
        /// 解析xml
        /// </summary>
        /// <param name="node"></param>
        public void handle(XmlNode node)
        {
            if (node == null) return;
            AstdLuaObject lua = new AstdLuaObject();
            lua.ParseXml(node);
            techid_ = lua.GetIntValue("technology.techid");
            techname_ = lua.GetStringValue("technology.techname");
            intro_ = lua.GetStringValue("technology.intro");
            techlevel_ = lua.GetIntValue("technology.techlevel");
            progress_ = lua.GetIntValue("technology.progress");
            requireprogress_ = lua.GetIntValue("technology.requireprogress");
            consumebintie_ = lua.GetIntValue("technology.consumebintie");
            effectvalue_ = lua.GetStringValue("technology.effectvalue");
        }
    }
}
