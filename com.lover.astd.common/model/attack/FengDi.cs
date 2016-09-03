using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace com.lover.astd.common.model.attack
{
    public class FengDi
    {
        /// <summary>
        /// 是否完成，0：否，1：是
        /// </summary>
        public int finish;
        /// <summary>
        /// 类型
        /// </summary>
        public int type;
        /// <summary>
        /// 生产资源，1：宝石，2：镔铁，3：兵器，4：大将令
        /// </summary>
        public int resid;
        /// <summary>
        /// 地区id
        /// </summary>
        public int areaid;
        /// <summary>
        /// 地区名
        /// </summary>
        public string areaname;
        /// <summary>
        /// 奖励数量
        /// </summary>
        public int rewardnum;
        /// <summary>
        /// 到期时间
        /// </summary>
        public int nextcd;
        /// <summary>
        /// 免费借兵次数
        /// </summary>
        public int freejiebinnum;
        /// <summary>
        /// 借兵花费金币
        /// </summary>
        public int jiebincost;
        /// <summary>
        /// 剩余生产次数
        /// </summary>
        public int remainnum;
        /// <summary>
        /// 初始化
        /// </summary>
        public FengDi()
        {
            finish = 0;
            type = 0;
            areaid = 0;
            resid = 0;
            areaname = "";
            rewardnum = 0;
            nextcd = 0;
            freejiebinnum = 0;
            jiebincost = 0;
            remainnum = 0;
        }
        /// <summary>
        /// 解析Xml
        /// </summary>
        /// <param name="fengdi"></param>
        public void fillXmlNode(XmlNode fengdi)
        {
            if (fengdi == null) return;
            XmlNodeList fengdiNode = fengdi.ChildNodes;
            foreach (XmlNode fengdiChildNode in fengdiNode)
            {
                if (fengdiChildNode.Name == "finish")
                {
                    finish = int.Parse(fengdiChildNode.InnerText);
                }
                else if (fengdiChildNode.Name == "type")
                {
                    type = int.Parse(fengdiChildNode.InnerText);
                }
                else if (fengdiChildNode.Name == "resid")
                {
                    resid = int.Parse(fengdiChildNode.InnerText);
                }
                else if (fengdiChildNode.Name == "areaid")
                {
                    areaid = int.Parse(fengdiChildNode.InnerText);
                }
                else if (fengdiChildNode.Name == "areaname")
                {
                    areaname = fengdiChildNode.InnerText;
                }
                else if (fengdiChildNode.Name == "rewardnum")
                {
                    rewardnum = int.Parse(fengdiChildNode.InnerText);
                }
                else if (fengdiChildNode.Name == "nextcd")
                {
                    nextcd = int.Parse(fengdiChildNode.InnerText);
                }
                else if (fengdiChildNode.Name == "freejiebinnum")
                {
                    freejiebinnum = int.Parse(fengdiChildNode.InnerText);
                }
                else if (fengdiChildNode.Name == "jiebincost")
                {
                    jiebincost = int.Parse(fengdiChildNode.InnerText);
                }
                else if (fengdiChildNode.Name == "remainnum")
                {
                    remainnum = int.Parse(fengdiChildNode.InnerText);
                }
            }
        }
    }
}
