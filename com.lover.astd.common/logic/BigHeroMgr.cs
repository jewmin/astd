using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.manager;
using System.Drawing;
using com.lover.astd.common.model;
using System.Xml;

namespace com.lover.astd.common.logic
{
    public class BigHeroMgr : MgrBase
    {
        public Dictionary<string, int> indexs_ = new Dictionary<string, int>();
        public List<BigHero> heros_ = new List<BigHero>();
        public List<BigHeroExpBook> expbooks_ = new List<BigHeroExpBook>();

        public BigHeroMgr(TimeMgr tmrMgr, ServiceFactory factory)
		{
            _logColor = Color.LimeGreen;
            _tmrMgr = tmrMgr;
            _factory = factory;
            initIndex();
		}
        /// <summary>
        /// 初始化优先级
        /// </summary>
        private void initIndex()
        {
            indexs_["李白"] = 1000;
            indexs_["黄帝"] = 1300;
            indexs_["后羿"] = 1400;
            indexs_["夏桀"] = 1500;
            indexs_["王翦"] = 1700;
            indexs_["鬼谷子"] = 1702;
            indexs_["荆轲"] = 1703;
            indexs_["曹操"] = 1704;
            indexs_["成吉思汗"] = 1705;
            indexs_["蚩尤"] = 1706;
            indexs_["炎帝"] = 1707;
            indexs_["姜子牙"] = 1708;
            indexs_["秦始皇"] = 1800;
            indexs_["李牧"] = 1900;
            indexs_["张飞"] = 2000;
            indexs_["戚继光"] = 2201;
            indexs_["诸葛亮"] = 2202;
            indexs_["大禹"] = 2204;
            indexs_["商汤"] = 2300;
            indexs_["周文王"] = 2400;
            indexs_["蒙恬"] = 2500;
            indexs_["墨子"] = 2800;
            indexs_["典韦"] = 2900;
            indexs_["文丑"] = 3000;
            indexs_["西施"] = 3100;
            indexs_["兀突骨"] = 3200;
            indexs_["刘备"] = 3300;
            indexs_["吕布"] = 3400;
            indexs_["黄忠"] = 3500;
            indexs_["太史慈"] = 3600;
            indexs_["赵云"] = 3700;
            indexs_["关羽"] = 3800;
            indexs_["许褚"] = 3900;
            indexs_["孙膑"] = 4000;
            indexs_["白起"] = 4100;
            indexs_["郭嘉"] = 4200;
            indexs_["司马懿"] = 4300;
            indexs_["张辽"] = 4400;
            indexs_["徐庶"] = 4500;
            indexs_["周瑜"] = 4600;
            indexs_["甘宁"] = 4700;
        }
        /// <summary>
        /// 获得大将信息
        /// </summary>
        /// <param name="heroId"></param>
        /// <returns></returns>
        private BigHero getBigHero(int heroId)
        {
            foreach (BigHero hero in heros_)
            {
                if (hero.Id == heroId) return hero;
            }
            return null;
        }
        /// <summary>
        /// 获得所有大将信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        public void getAllBigGenerals(ProtocolMgr protocol, ILogger logger)
        {
            string url = "/root/general!getAllBigGenerals.action";
            ServerResult xml = protocol.getXml(url, "获取所有大将信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNodeList childNodes = cmdResult.SelectNodes("/results/general");
            if (childNodes != null)
            {
                heros_.Clear();
                foreach (XmlNode xmlNode in childNodes)
                {
                    BigHero hero = new BigHero();
                    hero.fillValues(xmlNode.ChildNodes);
                    if (indexs_.ContainsKey(hero.Name)) hero.Index = indexs_[hero.Name];
                    heros_.Add(hero);
                }
            }
        }
        /// <summary>
        /// 获取大将训练信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="max_big_level"></param>
        public void getBigTrainInfo(ProtocolMgr protocol, ILogger logger, out int max_big_level, out int totalpos, out int freenum)
        {
            max_big_level = 0;
            totalpos = 0;
            freenum = 0;
            string url = "/root/general!getBigTrainInfo.action";
            ServerResult xml = protocol.getXml(url, "获取大将训练信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/maxbglv");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out max_big_level);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/totalpos");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out totalpos);
            }
            xmlNode = cmdResult.SelectSingleNode("/results/freenum");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out freenum);
            }
            XmlNodeList childNodes = cmdResult.SelectNodes("/results/traininfo");
            if (childNodes != null)
            {
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    int generalId = 0;
                    XmlNode xmlNode3 = xmlNode2.SelectSingleNode("generalid");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out generalId);
                    }
                    if (generalId != 0)
                    {
                        BigHero hero = getBigHero(generalId);
                        if (hero != null)
                        {
                            XmlNode xmlNode4 = xmlNode2.SelectSingleNode("pos");
                            if (xmlNode4 != null)
                            {
                                hero.TrainPos = int.Parse(xmlNode4.InnerText);
                            }
                            xmlNode4 = xmlNode2.SelectSingleNode("exp");
                            if (xmlNode4 != null)
                            {
                                hero.Exp = int.Parse(xmlNode4.InnerText);
                            }
                            xmlNode4 = xmlNode2.SelectSingleNode("maxexp");
                            if (xmlNode4 != null)
                            {
                                hero.MaxExp = int.Parse(xmlNode4.InnerText);
                            }
                            xmlNode4 = xmlNode2.SelectSingleNode("biglv");
                            if (xmlNode4 != null)
                            {
                                hero.BigLevel = int.Parse(xmlNode4.InnerText);
                            }
                            xmlNode4 = xmlNode2.SelectSingleNode("num");
                            if (xmlNode4 != null)
                            {
                                hero.TuFei = int.Parse(xmlNode4.InnerText);
                            }
                            xmlNode4 = xmlNode2.SelectSingleNode("change");
                            if (xmlNode4 != null)
                            {
                                hero.Change = int.Parse(xmlNode4.InnerText);
                            }
                            xmlNode4 = xmlNode2.SelectSingleNode("generaltype");
                            if (xmlNode4 != null)
                            {
                                hero.GeneralType = int.Parse(xmlNode4.InnerText);
                            }
                        }
                    }
                }
            }
            expbooks_ = XmlHelper.GetClassList<BigHeroExpBook>(cmdResult.SelectNodes("/results/expbook"));
        }
        /// <summary>
        /// 训练大将
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="trainPosId"></param>
        /// <param name="generalId"></param>
        /// <returns></returns>
        public bool startBigTrain(ProtocolMgr protocol, ILogger logger, int trainPosId, int generalId)
        {
            string url = "/root/general!startBigTrain.action";
            string data = string.Format("trainPosId={0}&generalId={1}", trainPosId, generalId);
            ServerResult xml = protocol.postXml(url, data, "训练大将");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 突飞大将
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="generalId"></param>
        /// <returns></returns>
        public bool fastTrainBigGeneral(ProtocolMgr protocol, ILogger logger, int generalId)
        {
            string url = "/root/general!fastTrainBigGeneral.action";
            string data = string.Format("generalId={0}", generalId);
            ServerResult xml = protocol.postXml(url, data, "突飞大将");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 转成大将
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="generalId"></param>
        /// <returns></returns>
        public bool toBigGeneral(ProtocolMgr protocol, ILogger logger, int generalId)
        {
            string url = "/root/general!toBigGeneral.action";
            string data = string.Format("generalId={0}", generalId);
            ServerResult xml = protocol.postXml(url, data, "转成大将");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 大将晋升
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="generalId"></param>
        /// <returns></returns>
        public bool bigGeneralChange(ProtocolMgr protocol, ILogger logger, int generalId)
        {
            string url = "/root/general!bigGeneralChange.action";
            string data = string.Format("generalId={0}", generalId);
            ServerResult xml = protocol.postXml(url, data, "大将晋升");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 大将突破
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="generalId"></param>
        /// <returns></returns>
        public bool newTrainBigGeneral(ProtocolMgr protocol, ILogger logger, int generalId, out int cost)
        {
            cost = 0;
            string url = "/root/general!newTrainBigGeneral.action";
            string data = string.Format("generalId={0}", generalId);
            ServerResult xml = protocol.postXml(url, data, "大将突破");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            XmlNode xmlNode = xml.CmdResult.SelectSingleNode("/results/newtufei/cost");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out cost);
            }
            return true;
        }
        /// <summary>
        /// 大将使用经验书
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="generalId"></param>
        /// <returns></returns>
        public bool useExpBook(ProtocolMgr protocol, ILogger logger, int generalId)
        {
            string url = "/root/general!useExpBook.action";
            string data = string.Format("generalId={0}", generalId);
            ServerResult xml = protocol.postXml(url, data, "使用经验书");
            if (xml == null || !xml.CmdSucceed) return false;

            XmlDocument cmdResult = xml.CmdResult;
            int expreward = XmlHelper.GetValue<int>(cmdResult.SelectSingleNode("/results/bookreward/expreward"));
            logInfo(logger, string.Format("使用经验书, 经验+{0}", expreward));
            return true;
        }
    }
}
