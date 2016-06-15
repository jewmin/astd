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
        private Dictionary<string, int> indexs_ = new Dictionary<string, int>();
        public List<BigHero> heros_ = new List<BigHero>();

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
            indexs_["夏桀"] = 15;
            indexs_["商汤"] = 16;
            indexs_["周文王"] = 17;
            indexs_["姜子牙"] = 18;
            indexs_["王翦"] = 19;
            indexs_["秦始皇"] = 20;
            indexs_["李牧"] = 21;
            indexs_["张飞"] = 22;
            indexs_["荆轲"] = 23;
            indexs_["鬼谷子"] = 24;
            indexs_["蒙恬"] = 25;
            indexs_["诸葛亮"] = 26;
            indexs_["曹操"] = 27;
            indexs_["墨子"] = 28;
            indexs_["典韦"] = 29;
            indexs_["文丑"] = 30;
            indexs_["西施"] = 31;
            indexs_["兀突骨"] = 32;
            indexs_["刘备"] = 33;
            indexs_["吕布"] = 34;
            indexs_["黄忠"] = 35;
            indexs_["太史慈"] = 36;
            indexs_["赵云"] = 37;
            indexs_["关羽"] = 38;
            indexs_["许褚"] = 39;
            indexs_["孙膑"] = 40;
            indexs_["白起"] = 41;
            indexs_["郭嘉"] = 42;
            indexs_["司马懿"] = 43;
            indexs_["张辽"] = 44;
            indexs_["徐庶"] = 45;
            indexs_["周瑜"] = 46;
            indexs_["甘宁"] = 47;
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
        public void getBigTrainInfo(ProtocolMgr protocol, ILogger logger, out int max_big_level)
        {
            max_big_level = 0;
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
                        }
                    }
                }
            }
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
    }
}
