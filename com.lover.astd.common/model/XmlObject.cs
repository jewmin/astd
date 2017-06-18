using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model
{
    public abstract class XmlObject
    {
        /// <summary>
        /// 解析xml
        /// </summary>
        /// <param name="node"></param>
        public abstract void Parse(XmlNode node);
        /// <summary>
        /// 是否能插入到列表
        /// </summary>
        /// <returns></returns>
        public abstract bool CanAdd();
    }
}
