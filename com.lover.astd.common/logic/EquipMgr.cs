using com.lover.astd.common.config;
using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Text;

namespace com.lover.astd.common.logic
{
    public class EquipMgr : MgrBase
    {
        private class CrystalInfo
        {
            public int curbaoshinum;

            public int totalbaoshinum;

            public int storeId;

            public int baoshilevel;

            public string desc;

            public int baoshitype;

            public int lack_gemcount
            {
                get
                {
                    return this.totalbaoshinum - this.curbaoshinum;
                }
            }
        }

        public class Decoration
        {
            public int storeid;

            public int attribute_base;

            public int polishtimes;

            public string name;

            public int gold;

            public int upgradestate;

            public double succprob;

            public int eventsrc;

            public int quality;

            public string generalname;
        }

        private class Officer
        {
            public int Id;

            public string Name;

            public int Pos;

            public int Level;

            public float Ratio;
        }

        private class AnswerOption
        {
            public string index;

            public string name;
        }

        private class Answer
        {
            private List<EquipMgr.AnswerOption> _options = new List<EquipMgr.AnswerOption>();

            public void addOption(EquipMgr.AnswerOption opt)
            {
                this._options.Add(opt);
            }

            public string getAnswerIndex(string question)
            {
                string result = "0";
                int num = 0;
                foreach (EquipMgr.AnswerOption current in this._options)
                {
                    bool flag = current.name.IndexOf(question) >= 0;
                    if (flag)
                    {
                        result = current.index;
                        break;
                    }
                    int num2 = num;
                    num = num2 + 1;
                }
                return result;
            }
        }

        private class Reward
        {
            public string type;

            public string name;

            public string goodsid;

            public string quality;
        }

        public class Merchant
        {
            public int merchantid;

            public string merchantname;

            public int cost;
        }

        public EquipMgr(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.Crimson;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
        }

        public int handleStore(ProtocolMgr protocol, ILogger logger, User user, bool merge_gem, int merge_gem_level, bool open_box_for_yupei, bool upgrade_crystal, int upgrade_crystal_level, bool auto_melt_yupei, int yupei_attr)
        {
            bool flag = false;
            EquipMgr equipManager = this._factory.getEquipManager();
            equipManager.getStore(protocol, logger, user);
            List<Equipment> storeEquips = user._storeEquips;
            int gemMinLevel = 0;
            int storeGems = 0;
            for (int i = 0; i < storeEquips.Count; i++)
            {
                Equipment equipment = storeEquips[i];
                if (equipment.goodstype == GoodsType.Gem && equipment.FragmentCount > 1)
                {
                    storeGems += GlobalConfig.getGemCount(equipment.Level) * equipment.FragmentCount;
                    if (gemMinLevel == 0)
                    {
                        gemMinLevel = equipment.Level;
                    }
                    else if (gemMinLevel > equipment.Level)
                    {
                        gemMinLevel = equipment.Level;
                    }
                }
            }
            user._storeGems = storeGems;
            if (merge_gem && gemMinLevel > 0 && gemMinLevel < merge_gem_level)
            {
                int ret = 0, times = 50;
                do
                {
                    times--;
                    ret = equipManager.mergeSameLevelGem(protocol, logger, gemMinLevel);
                    gemMinLevel++;
                    flag = true;
                } while (ret > 1 && gemMinLevel < merge_gem_level && times > 0);
            }
            int storeUsedSize = user._storeUsedSize;
            int storeTotalSize = user._storeTotalSize;
            for (int j = 0; j < storeEquips.Count; j++)
            {
                Equipment equipment2 = storeEquips[j];
                if (equipment2.goodstype != GoodsType.Weapon && equipment2.RemainTime > 0 && storeUsedSize < storeTotalSize)
                {
                    int equip = equipManager.getEquip(protocol, logger, equipment2);
                    if (equip == 2)
                    {
                        storeUsedSize = storeTotalSize;
                        break;
                    }
                    storeUsedSize++;
                    flag = true;
                }
            }
            if (user.Level >= 240 & upgrade_crystal)
            {
                this.handleCrystalInfo(protocol, logger, upgrade_crystal_level);
            }
            if (open_box_for_yupei)
            {
                bool hasYuPei = false;
                for (int k = 0; k < storeEquips.Count; k++)
                {
                    Equipment equipment3 = storeEquips[k];
                    if (equipment3.Name == "家传玉佩")
                    {
                        hasYuPei = true;
                        break;
                    }
                }
                if (!hasYuPei)
                {
                    base.logInfo(logger, "玉佩已经炼化完了, 再开几个箱子来玩玩呗 =.=");
                    this._factory.getMiscManager().openNationTreasure(protocol, logger);
                }
            }
            user._storeUsedSize = storeUsedSize;
            if (auto_melt_yupei)
            {
                for (int i = 0; i < storeEquips.Count; i++)
                {
                    Equipment equip = storeEquips[i];
                    if (equip.Name == "家传玉佩" && equip.Baowulea <= yupei_attr)
                    {
                        Decoration dec = new Decoration();
                        dec.storeid = equip.Id;
                        dec.name = equip.Name;
                        dec.attribute_base = equip.Baowulea;
                        destroy(protocol, logger, user.Magic, dec, false);
                    }
                }
            }
            if (flag)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public List<Equipment> getStoreCanMelt(ProtocolMgr protocol, ILogger logger, User user)
        {
            this.getStore(protocol, logger, user);
            List<Equipment> storeEquips = user._storeEquips;
            List<Equipment> list = new List<Equipment>();
            int num;
            for (int i = 0; i < storeEquips.Count; i = num + 1)
            {
                Equipment equipment = storeEquips[i];
                bool flag = equipment.goodstype != GoodsType.Decoration && equipment.goodstype != GoodsType.Gem && equipment.goodstype != GoodsType.Weapon;
                if (flag)
                {
                    list.Add(equipment);
                }
                num = i;
            }
            return list;
        }

        public void getStore(ProtocolMgr protocol, ILogger logger, User user)
        {
            int storeUsedSize = 0;
            int storeTotalSize = 0;
            List<Equipment> list = new List<Equipment>();
            string url = "/root/goods!openStorehouse.action";
            ServerResult xml = protocol.getXml(url, "获取仓库信息");
            if (xml == null || !xml.CmdSucceed)
            {
                return;
            }
            XmlDocument cmdResult = xml.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/usesize");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out storeUsedSize);
            }
            else
            {
                storeUsedSize = 0;
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/storesize");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out storeTotalSize);
            }
            else
            {
                storeTotalSize = 0;
            }
            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode3.ChildNodes;
            foreach (XmlNode xmlNode4 in childNodes)
            {
                if (xmlNode4.Name == "storehousedto")
                {
                    Equipment equipment = new Equipment();
                    XmlNodeList childNodes2 = xmlNode4.ChildNodes;
                    equipment.fillValues(childNodes2);
                    list.Add(equipment);
                }
            }
            user._storeEquips = list;
            user._storeUsedSize = storeUsedSize;
            user._storeTotalSize = storeTotalSize;
        }

        public int degradeEquip(ProtocolMgr protocol, ILogger logger, int equipId)
        {
            string url = "/root/equip!degradeEquipByStore.action";
            string data = "storeId=" + equipId;
            ServerResult serverResult = protocol.postXml(url, data, "降级装备");
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool cmdSucceed = serverResult.CmdSucceed;
                if (cmdSucceed)
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cost");
                    bool flag2 = xmlNode != null;
                    if (flag2)
                    {
                        base.logInfo(logger, string.Format("降级成功, 银币+{0}", xmlNode.InnerText));
                    }
                    result = 0;
                }
                else
                {
                    string cmdError = serverResult.CmdError;
                    bool flag3 = cmdError.Contains("上限");
                    if (flag3)
                    {
                        result = 2;
                    }
                    else
                    {
                        bool flag4 = cmdError.Contains("最低");
                        if (flag4)
                        {
                            result = 3;
                        }
                        else
                        {
                            result = 4;
                        }
                    }
                }
            }
            return result;
        }

        public int degradeMh(ProtocolMgr protocol, ILogger logger, int equipId)
        {
            string url = "/root/enchant!reduce.action";
            string data = "storeId=" + equipId;
            ServerResult serverResult = protocol.postXml(url, data, "降魔装备");
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool cmdSucceed = serverResult.CmdSucceed;
                if (cmdSucceed)
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/addbowlder");
                    bool flag2 = xmlNode != null;
                    if (flag2)
                    {
                        base.logInfo(logger, string.Format("装备降魔成功, 玉石+{0}", xmlNode.InnerText));
                    }
                    result = 0;
                }
                else
                {
                    string cmdError = serverResult.CmdError;
                    bool flag3 = cmdError.IndexOf("上限") >= 0;
                    if (flag3)
                    {
                        result = 2;
                    }
                    else
                    {
                        bool flag4 = cmdError.IndexOf("最低") >= 0;
                        if (flag4)
                        {
                            result = 3;
                        }
                        else
                        {
                            result = 4;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 熔化装备
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="equipId"></param>
        /// <param name="magic"></param>
        /// <returns></returns>
        public int meltEquip(ProtocolMgr protocol, ILogger logger, int equipId, int magic)
        {
            string url = "/root/stoneMelt!melt.action";
            string data = string.Format("magic={0}&gold=0&meltGold=0&storeId={1}", magic, equipId);
            ServerResult serverResult = protocol.postXml(url, data, "融化装备");
            if (serverResult == null)
            {
                return 1;
            }
            else if (serverResult.CmdSucceed)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/gainbowlder");
                if (xmlNode != null)
                {
                    base.logInfo(logger, string.Format("装备融化成功, 玉石+{0}", xmlNode.InnerText));
                }
                return 0;
            }
            else if (serverResult.CmdError.IndexOf("上限") >= 0)
            {
                return 2;
            }
            else if (serverResult.CmdError.IndexOf("最低") >= 0)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        /// <summary>
        /// 卖出装备
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="equipId"></param>
        /// <returns></returns>
        public int sellEquip(ProtocolMgr protocol, ILogger logger, int equipId)
        {
            string url = "/root/goods!sellGoods.action";
            string data = string.Format("goodsId={0}&count=1", equipId);
            ServerResult serverResult = protocol.postXml(url, data, "卖出装备");
            if (serverResult == null)
            {
                return 1;
            }
            else if (serverResult.CmdSucceed)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cost");
                if (xmlNode != null)
                {
                    base.logInfo(logger, string.Format("卖出装备成功, 银币+{0}", xmlNode.InnerText));
                }
                return 0;
            }
            else if (serverResult.CmdError.IndexOf("上限") >= 0)
            {
                return 2;
            }
            else if (serverResult.CmdError.IndexOf("最低") >= 0)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        /// <summary>
        /// 取装备
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="equip"></param>
        /// <returns></returns>
        public int getEquip(ProtocolMgr protocol, ILogger logger, Equipment equip)
        {
            string url = "/root/goods!draw.action";
            string data;
            if (equip.goodstype == GoodsType.Gem)
            {
                data = string.Format("baoshiLv={0}&count=1&goodsId=0", equip.Level);
            }
            else
            {
                data = string.Format("baoshiLv=0&count=1&goodsId={0}", equip.Id);
            }
            ServerResult serverResult = protocol.postXml(url, data, "拿取装备");
            if (serverResult == null)
            {
                return 1;
            }
            else if (serverResult.CmdSucceed)
            {
                base.logInfo(logger, string.Format("从临时仓库拿取装备[{0}]", equip.Name));
                return 0;
            }
            else if (serverResult.CmdError.IndexOf("仓库没有足够的位置") >= 0)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 合成同级宝石
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="gemLevel"></param>
        /// <returns></returns>
        public int mergeSameLevelGem(ProtocolMgr protocol, ILogger logger, int gemLevel)
        {
            string url = "/root/equip!updateBaoshiWholeLevel.action";
            string data = string.Format("baoshiId={0}", gemLevel);
            ServerResult serverResult = protocol.postXml(url, data, "合成同级宝石" + gemLevel.ToString());
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return 0;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                int num = 0;
                int numup = 0;
                int baoshilevelup = 0;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    if (xmlNode2.Name == "num")
                    {
                        num = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "baoshilevelup")
                    {
                        baoshilevelup = int.Parse(xmlNode2.InnerText);
                    }
                    else if (xmlNode2.Name == "numup")
                    {
                        numup = int.Parse(xmlNode2.InnerText);
                    }
                }
                string text = string.Format("使用{0}级宝石{1}个, 合成{2}级宝石{3}个", gemLevel, num, baoshilevelup, numup);
                base.logInfo(logger, text);
                return numup;
            }
        }

        /// <summary>
        /// 进阶水晶石
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private int handleCrystalInfo(ProtocolMgr protocol, ILogger logger, int upgrade_crystal_level)
        {
            string url = "/root/equip!getCrystal.action";
            ServerResult xml = protocol.getXml(url, "获取水晶石信息");
            int result;
            if (xml == null)
            {
                result = 1;
            }
            else if (!xml.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                List<int> gemList;
                int equipGems = this.getEquipGems(protocol, logger, out gemList);
                if (equipGems > 0 || gemList.Count == 0)
                {
                    result = 2;
                }
                else
                {
                    List<EquipMgr.CrystalInfo> crystalInfoList = new List<EquipMgr.CrystalInfo>();
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/baoshidto");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        int storeId = 0;
                        string generalname = "";
                        string goodsname = "";
                        string desc = " ";
                        XmlNode xmlNode2 = xmlNode.SelectSingleNode("istop");
                        if (xmlNode2 == null || !(xmlNode2.InnerText == "1"))
                        {
                            XmlNode xmlNode3 = xmlNode.SelectSingleNode("storeid");
                            if (xmlNode3 != null)
                            {
                                int.TryParse(xmlNode3.InnerText, out storeId);
                            }
                            XmlNode xmlNode4 = xmlNode.SelectSingleNode("generalname");
                            if (xmlNode4 != null)
                            {
                                generalname = xmlNode4.InnerText;
                            }
                            XmlNode xmlNode5 = xmlNode.SelectSingleNode("goodsname");
                            if (xmlNode5 != null)
                            {
                                goodsname = xmlNode5.InnerText;
                            }
                            if (generalname != "")
                            {
                                desc += generalname + " ";
                            }
                            if (goodsname != "")
                            {
                                desc += goodsname;
                            }
                            int totalbaoshinum = 0;
                            int curbaoshinum = 0;
                            XmlNode xmlNode6 = xmlNode.SelectSingleNode("curbaoshinum");
                            if (xmlNode6 != null)
                            {
                                int.TryParse(xmlNode6.InnerText, out curbaoshinum);
                            }
                            XmlNode xmlNode7 = xmlNode.SelectSingleNode("totalbaoshinum");
                            if (xmlNode7 != null)
                            {
                                int.TryParse(xmlNode7.InnerText, out totalbaoshinum);
                            }
                            int baoshilevel = 0;
                            XmlNode xmlNode8 = xmlNode.SelectSingleNode("baoshilevel");
                            if (xmlNode8 != null)
                            {
                                int.TryParse(xmlNode8.InnerText, out baoshilevel);
                            }
                            int baoshitype = 0;
                            XmlNode xmlNode9 = xmlNode.SelectSingleNode("baoshitype");
                            if (xmlNode9 != null)
                            {
                                int.TryParse(xmlNode9.InnerText, out baoshitype);
                            }
                            if (totalbaoshinum != 0)
                            {
                                if (totalbaoshinum - curbaoshinum <= 0)
                                {
                                    this.upgradeCrystal(protocol, logger, storeId);
                                }
                                else if (baoshilevel >= upgrade_crystal_level)
                                {
                                    crystalInfoList.Add(new EquipMgr.CrystalInfo
                                    {
                                        storeId = storeId,
                                        totalbaoshinum = totalbaoshinum,
                                        curbaoshinum = curbaoshinum,
                                        baoshilevel = baoshilevel,
                                        desc = desc,
                                        baoshitype = baoshitype
                                    });
                                }
                            }
                        }
                    }
                    for (int i = 0; i < crystalInfoList.Count - 1; i++)
                    {
                        for (int j = i + 1; j < crystalInfoList.Count; j++)
                        {
                            if (crystalInfoList[i].baoshilevel < crystalInfoList[j].baoshilevel || (crystalInfoList[i].baoshilevel == crystalInfoList[j].baoshilevel && crystalInfoList[i].curbaoshinum < crystalInfoList[j].curbaoshinum))
                            {
                                EquipMgr.CrystalInfo value = crystalInfoList[i];
                                crystalInfoList[i] = crystalInfoList[j];
                                crystalInfoList[j] = value;
                            }
                        }
                    }
                    for (int i = 0; i < crystalInfoList.Count; i++)
                    {
                        EquipMgr.CrystalInfo crystalInfo = crystalInfoList[i];
                        bool upgrade = false;
                        while (!upgrade && gemList.Count > 0)
                        {
                            for (int j = 0; j < gemList.Count; j++)
                            {
                                int gem = GlobalConfig.getGemCount(gemList[j]);
                                if (gem >= crystalInfo.lack_gemcount || j == gemList.Count - 1)
                                {
                                    int curbaoshinum = crystalInfo.curbaoshinum;
                                    if (this.meltCrystal(protocol, logger, gemList[j], crystalInfo.storeId, ref curbaoshinum) > 0)
                                    {
                                        result = 2;
                                        return result;
                                    }
                                    crystalInfo.curbaoshinum = curbaoshinum;
                                    base.logInfo(logger, string.Format("熔炼[{0}]{1}阶水晶石, 当前进度{2}/{3}", crystalInfo.desc, crystalInfo.baoshilevel, crystalInfo.curbaoshinum, crystalInfo.totalbaoshinum));
                                    gemList.RemoveAt(j);
                                    if (crystalInfo.lack_gemcount <= 0)
                                    {
                                        this.upgradeCrystal(protocol, logger, crystalInfo.storeId);
                                        upgrade = true;
                                        break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    result = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// 仓库宝石列表
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="logger"></param>
        /// <param name="gems"></param>
        /// <returns></returns>
        private int getEquipGems(ProtocolMgr protocol, ILogger logger, out List<int> gems)
        {
            gems = new List<int>();
            string url = "/root/equip!getBaoshiForinset.action";
            ServerResult xml = protocol.getXml(url, "获取装备宝石信息");
            int result;
            if (xml == null)
            {
                result = 1;
            }
            else if (!xml.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/baoshilist/baoshi");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    int lv = 0;
                    int num = 0;
                    XmlNode xmlNode2 = xmlNode.SelectSingleNode("lv");
                    if (xmlNode2 != null)
                    {
                        int.TryParse(xmlNode2.InnerText, out lv);
                    }
                    XmlNode xmlNode3 = xmlNode.SelectSingleNode("num");
                    if (xmlNode3 != null)
                    {
                        int.TryParse(xmlNode3.InnerText, out num);
                    }
                    if (num > 0 && lv >= 15)
                    {
                        for (int i = 0; i < num; i++)
                        {
                            gems.Add(lv);
                        }
                    }
                }
                result = 0;
            }
            return result;
        }

        private int meltCrystal(ProtocolMgr protocol, ILogger logger, int baoshiId, int storeId, ref int curbaoshinum)
        {
            string url = "/root/equip!meltCrystal.action";
            string data = string.Format("baoshiId={0}&storeId={1}", baoshiId, storeId);
            ServerResult serverResult = protocol.postXml(url, data, "熔炼水晶石");
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/baoshidto");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    XmlNode xmlNode2 = xmlNode.SelectSingleNode("storeid");
                    if (xmlNode2 != null && int.Parse(xmlNode2.InnerText) == storeId)
                    {
                        XmlNode xmlNode3 = xmlNode.SelectSingleNode("curbaoshinum");
                        if (xmlNode3 != null)
                        {
                            int.TryParse(xmlNode3.InnerText, out curbaoshinum);
                        }
                    }
                }
                return 0;
            }
        }

        private int upgradeCrystal(ProtocolMgr protocol, ILogger logger, int storeId)
        {
            string url = "/root/equip!upgradeCrystal.action";
            string data = string.Format("storeId={0}", storeId);
            ServerResult serverResult = protocol.postXml(url, data, "进阶水晶石");
            int result;
            if (serverResult == null)
            {
                result = 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                base.logInfo(logger, "进阶水晶石成功");
                result = 0;
            }
            return result;
        }

        public bool getBaowuPolishInfo(ProtocolMgr protocol, ILogger logger, User user, string lh_ids)
        {
            string url = "/root/polish!getBaowuPolishInfo.action";
            ServerResult xml = protocol.getXml(url, "获取炼化信息");
            if (xml == null || !xml.CmdSucceed) return false;

            user.SpecialTreasureList.Clear();
            XmlNodeList nodeList = xml.CmdResult.SelectNodes("/results/specialtreasure");
            foreach (XmlNode item in nodeList)
            {
                Specialtreasure treasure = new Specialtreasure();
                XmlNodeList childNodes = item.ChildNodes;
                foreach (XmlNode child in childNodes)
                {
                    if (child.Name == "storeid") treasure.Id = int.Parse(child.InnerText);
                    else if (child.Name == "upgradestate") treasure.upgradestate_ = int.Parse(child.InnerText);
                    else if (child.Name == "consecratestatus") treasure.consecratestatus_ = int.Parse(child.InnerText);
                    else if (child.Name == "generalname") treasure.generalname_ = child.InnerText;
                    else if (child.Name == "attribute_lea") treasure.attribute_lea_ = int.Parse(child.InnerText);
                    else if (child.Name == "attribute_str") treasure.attribute_str_ = int.Parse(child.InnerText);
                    else if (child.Name == "attribute_int") treasure.attribute_int_ = int.Parse(child.InnerText);
                    else if (child.Name == "additionalattributelvmax") treasure.additionalattributelvmax_ = int.Parse(child.InnerText);
                    else if (child.Name == "maxadd") treasure.maxadd_ = int.Parse(child.InnerText);
                    else if (child.Name == "intro") treasure.intro_ = child.InnerText;
                    else if (child.Name == "quality") treasure.quality_ = int.Parse(child.InnerText);
                    else if (child.Name == "name") treasure.Name = child.InnerText;
                    else if (child.Name == "succprob") treasure.succprob_ = float.Parse(child.InnerText);
                    else if (child.Name == "canconsecrate") treasure.canconsecrate_ = int.Parse(child.InnerText);
                    
                }
                if (treasure.Id > 0 && treasure.generalname_ != "")
                {
                    user.SpecialTreasureList.Add(treasure);
                }
            }

            List<int> ids = base.generateIds(lh_ids);
            foreach (Specialtreasure current in user.SpecialTreasureList)
            {
                current.IsChecked = (ids.IndexOf(current.Id) >= 0);
            }

            return true;
        }

        public int handlePolishInfo(ProtocolMgr protocol, ILogger logger, User user, int gold_available, string lh_ids, int magic_now, int reserve_count, int reserve_item_count, int gold_merge_attrib, int melt_failcount)
        {
            string url = "/root/polish!getBaowuPolishInfo.action";
            ServerResult xml = protocol.getXml(url, "获取炼化信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            XmlDocument cmdResult = xml.CmdResult;
            int num = 0;
            int level = 0;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/num");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out num);
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/level");
            if (xmlNode2 != null)
            {
                int.TryParse(xmlNode2.InnerText, out level);
            }
            List<EquipMgr.Decoration> list = new List<EquipMgr.Decoration>();
            List<Decoration> equipList = new List<Decoration>();
            List<Decoration> upgradeList = new List<Decoration>();
            XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/baowu");
            foreach (XmlNode xmlNode3 in xmlNodeList)
            {
                EquipMgr.Decoration decoration = new EquipMgr.Decoration();
                XmlNodeList childNodes = xmlNode3.ChildNodes;
                foreach (XmlNode xmlNode4 in childNodes)
                {
                    if (xmlNode4.Name == "storeid")
                    {
                        decoration.storeid = int.Parse(xmlNode4.InnerText);
                    }
                    else if (xmlNode4.Name == "attribute_base")
                    {
                        decoration.attribute_base = int.Parse(xmlNode4.InnerText);
                    }
                    else if (xmlNode4.Name == "polishtimes")
                    {
                        decoration.polishtimes = int.Parse(xmlNode4.InnerText);
                    }
                    else if (xmlNode4.Name == "gold")
                    {
                        decoration.gold = int.Parse(xmlNode4.InnerText);
                    }
                    else if (xmlNode4.Name == "name")
                    {
                        decoration.name = xmlNode4.InnerText;
                    }
                    else if (xmlNode4.Name == "quality")
                    {
                        decoration.quality = int.Parse(xmlNode4.InnerText);
                    }
                    else if (xmlNode4.Name == "upgradestate")
                    {
                        decoration.upgradestate = int.Parse(xmlNode4.InnerText);
                    }
                    else if (xmlNode4.Name == "eventsrc")
                    {
                        decoration.eventsrc = int.Parse(xmlNode4.InnerText);
                    }
                    else if (xmlNode4.Name == "succprob")
                    {
                        decoration.succprob = double.Parse(xmlNode4.InnerText);
                    }
                    else if (xmlNode4.Name == "generalname")
                    {
                        decoration.generalname = xmlNode4.InnerText;
                    }
                }
                if (decoration.polishtimes < 10)
                {
                    list.Add(decoration);
                }
                else if (decoration.upgradestate == 2 && decoration.succprob > 0.1)
                {
                    equipList.Add(decoration);
                }
                else if (decoration.upgradestate == 4 && decoration.succprob > 0.01)
                {
                    equipList.Add(decoration);
                }
                else if (decoration.eventsrc == 1)
                {
                    upgradeList.Add(decoration);
                }
            }

            //专属玉佩
            if (!getBaowuPolishInfo(protocol, logger, user, lh_ids)) return 1;
            List<Specialtreasure> treasure_list = user.SpecialTreasureList;
            //专属开光
            foreach (Specialtreasure current3 in treasure_list)
            {
                if (current3.CanConsecrate)
                {
                    if (!consecrateSpecialTreasure(protocol, logger, current3)) return 10;
                }
            }
            //专属升级
            List<int> ids = base.generateIds(lh_ids);
            foreach (int current in ids)
            {
                Specialtreasure treasure = null;
                foreach (Specialtreasure current2 in treasure_list)
                {
                    if (current2.Id == current)
                    {
                        treasure = current2;
                        break;
                    }
                }

                if (treasure != null && treasure.CanUpgrade && upgradeList.Count > 0)
                {
                    while (treasure.CanUpgrade && upgradeList.Count > 0)
                    {
                        if (!upgradeBaowu(protocol, logger, treasure, upgradeList[0])) return 10;
                        upgradeList.RemoveAt(0);
                    }
                }
            }
            
            //宝物升级
            if (upgradeList.Count > 0 && equipList.Count > 0)
            {
                while (upgradeList.Count > 0)
                {
                    upgradeBaowu(protocol, logger, equipList[0], upgradeList[0]);
                    upgradeList.RemoveAt(0);
                }
            }
            //保留炼化次数
            if (num <= reserve_count)
            {
                return 2;
            }
            //没有宝物
            if (list.Count == 0)
            {
                return 2;
            }
            //保留宝物个数
            int idx = 0;
            while (list.Count > 0 && list.Count > reserve_item_count)
            {
                idx = 0;
                foreach (EquipMgr.Decoration current in list)
                {
                    if (current.polishtimes == 0)
                    {
                        break;
                    }
                    idx++;
                }
                if (idx < list.Count)
                {
                    EquipMgr.Decoration dec = list[idx];
                    this.destroy(protocol, logger, magic_now, dec, true);
                    list.RemoveAt(idx);
                }
            }
            //炼化宝物
            foreach (EquipMgr.Decoration dec in list)
            {
                if (dec.polishtimes >= 5)
                {
                    if (dec.attribute_base < gold_merge_attrib)
                    {
                        this.destroy(protocol, logger, magic_now, dec, false);
                        return 0;
                    }
                    while (num > 0)
                    {
                        if (gold_available <= dec.gold)
                        {
                            break;
                        }
                        gold_available -= dec.gold;
                        this.polish(protocol, logger, dec, out num);
                    }
                }
                else
                {
                    int polishnum = 5 - dec.polishtimes;
                    int failcount = 0;
                    while (num > 0 && polishnum > 0)
                    {
                        polishnum--;
                        dec.polishtimes++;
                        int add_attr = this.polish(protocol, logger, dec, out num);
                        if (add_attr == -1)
                        {
                            return 3;
                        }
                        else if (add_attr == 0)
                        {
                            if (failcount >= melt_failcount)
                            {
                                this.destroy(protocol, logger, magic_now, dec, false);
                                return 0;
                            }
                            else
                            {
                                failcount++;
                            }
                        }
                        else
                        {
                            dec.attribute_base += add_attr;
                        }
                    }
                    return 4;
                }
            }
            return 2;
        }

        public bool consecrateSpecialTreasure(ProtocolMgr protocol, ILogger logger, Specialtreasure src)
        {
            string url = "/root/polish!consecrateSpecialTreasure.action";
            string data = string.Format("storeId={0}", src.Id);
            ServerResult xml = protocol.postXml(url, data, "宝物开光");
            if (xml == null || !xml.CmdSucceed) return false;

            logInfo(logger, string.Format("{0}开光成功", src.NameWithGeneral));
            return true;
        }

        public bool upgradeBaowu(ProtocolMgr protocol, ILogger logger, Decoration dst, Decoration src)
        {
            string url = "/root/polish!upgradeBaowu.action";
            string data = string.Format("storeId={0}&storeId2={1}", dst.storeid, src.storeid);
            ServerResult xml = protocol.postXml(url, data, "宝物升级");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            AstdLuaObject lua = new AstdLuaObject();
            lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
            int succlea = lua.GetIntValue("results.baowu.succlea");
            int succstr = lua.GetIntValue("results.baowu.succstr");
            int succint = lua.GetIntValue("results.baowu.succint");
            logInfo(logger, string.Format("升级宝物[{0}({1})],统+{2},勇+{3},智+{4}", dst.name, dst.generalname, succlea, succstr, succint));
            return true;
        }

        public bool upgradeBaowu(ProtocolMgr protocol, ILogger logger, Specialtreasure dst, Decoration src)
        {
            string url = "/root/polish!upgradeBaowu.action";
            string data = string.Format("storeId={0}&storeId2={1}&type=2", dst.Id, src.storeid);
            ServerResult xml = protocol.postXml(url, data, "专属玉佩升级");
            if (xml == null || !xml.CmdSucceed) return false;

            int upgraderesult = 0;
            int succlea = 0;
            int succstr = 0;
            int succint = 0;
            XmlNode node = xml.CmdResult.SelectSingleNode("/results/upgraderesult");
            if (node != null) int.TryParse(node.InnerText, out upgraderesult);
            node = xml.CmdResult.SelectSingleNode("/results/baowu/succlea");
            if (node != null) int.TryParse(node.InnerText, out succlea);
            node = xml.CmdResult.SelectSingleNode("/results/baowu/succstr");
            if (node != null) int.TryParse(node.InnerText, out succstr);
            node = xml.CmdResult.SelectSingleNode("/results/baowu/succint");
            if (node != null) int.TryParse(node.InnerText, out succint);
            
            StringBuilder sb = new StringBuilder();
            sb.Append("专属玉佩升级");
            if (upgraderesult == 1)
            {
                sb.Append("成功");
                if (succlea > 0) sb.AppendFormat(", 统+{0}", succlea);
                if (succstr > 0) sb.AppendFormat(", 勇+{0}", succstr);
                if (succint > 0) sb.AppendFormat(", 智+{0}", succint);
                dst.attribute_lea_ += succlea;
                dst.attribute_str_ += succstr;
                dst.attribute_int_ += succint;
            }
            else
            {
                sb.Append("失败");
            }

            logInfo(logger, sb.ToString());

            return true;
        }

        public void qiling()
        {
        }

        public int polish(ProtocolMgr protocol, ILogger logger, EquipMgr.Decoration dec, out int _remain_melt_number)
        {
            _remain_melt_number = 0;
            string url = "/root/polish!polish.action";
            string data = "storeId=" + dec.storeid;
            ServerResult serverResult = protocol.postXml(url, data, "炼化");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return -1;
            }
            XmlDocument cmdResult = serverResult.CmdResult;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/num");
            if (xmlNode != null)
            {
                int.TryParse(xmlNode.InnerText, out _remain_melt_number);
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/baowu");
            EquipMgr.Decoration decoration = new EquipMgr.Decoration();
            XmlNodeList childNodes = xmlNode2.ChildNodes;
            foreach (XmlNode xmlNode3 in childNodes)
            {
                if (xmlNode3.Name == "storeid")
                {
                    decoration.storeid = int.Parse(xmlNode3.InnerText);
                }
                else if (xmlNode3.Name == "attribute_base")
                {
                    decoration.attribute_base = int.Parse(xmlNode3.InnerText);
                }
                else if (xmlNode3.Name == "polishtimes")
                {
                    decoration.polishtimes = int.Parse(xmlNode3.InnerText);
                }
                else if (xmlNode3.Name == "gold")
                {
                    decoration.gold = int.Parse(xmlNode3.InnerText);
                }
                else if (xmlNode3.Name == "name")
                {
                    decoration.name = xmlNode3.InnerText;
                }
            }
            int add_attr = decoration.attribute_base - dec.attribute_base;
            base.logInfo(logger, string.Format("炼化[{0}], 属性+{1}", decoration.name, add_attr));
            return add_attr;
        }

        public void destroy(ProtocolMgr protocol, ILogger logger, int magic, EquipMgr.Decoration dec, bool is_extra_item)
        {
            string url = "/root/stoneMelt!melt.action";
            string data = string.Format("magic={0}&meltGold=0&gold=0&storeId={1}", magic, dec.storeid);
            ServerResult serverResult = protocol.postXml(url, data, "融化");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            if (!flag)
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                int num = 0;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/gainbowlder");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    int.TryParse(xmlNode.InnerText, out num);
                }
                if (is_extra_item)
                {
                    base.logInfo(logger, string.Format("融化多余的[{0}], 玉石+{1}", dec.name, num));
                }
                else
                {
                    base.logInfo(logger, string.Format("融化[{0}, 属性+{1}], 玉石+{2}", dec.name, dec.attribute_base, num));
                }
            }
        }

        public int getEquipmentsCanQh(ProtocolMgr protocol, ILogger logger, User user, string qh_idstr)
        {
            string url = "/root/equip!getUpgradeInfo.action";
            ServerResult xml = protocol.getXml(url, "获取装备强化信息");
            bool flag = xml == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !xml.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    List<EquipMgr.Officer> list;
                    this.renderQhInfo(protocol, logger, cmdResult, user, out list, qh_idstr, false, 0);
                    result = 0;
                }
            }
            return result;
        }

        public int handleEquipmentsCanQh(ProtocolMgr protocol, ILogger logger, User user, int silver_available, string qh_idstr, int use_assist_silver, bool use_gold, int use_gold_silver, bool qh_toukui, int toukui_ticket)
        {
            string url = "/root/equip!getUpgradeInfo.action";
            ServerResult xml = protocol.getXml(url, "获取装备强化信息");
            int result;
            if (xml == null)
            {
                result = 1;
            }
            else if (!xml.CmdSucceed)
            {
                result = 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                List<EquipMgr.Officer> officers;
                this.renderQhInfo(protocol, logger, cmdResult, user, out officers, qh_idstr, qh_toukui, toukui_ticket);
                List<Equipment> qhEquips = user._qhEquips;
                bool flag3 = false;
                List<int> list = base.generateIds(qh_idstr);
                foreach (int current in list)
                {
                    Equipment equipment = null;
                    foreach (Equipment current2 in qhEquips)
                    {
                        if (current2.Id == current)
                        {
                            equipment = current2;
                            break;
                        }
                    }
                    if (equipment != null && equipment.CanUpgrade && equipment.UpgradeCost <= silver_available)
                    {
                        if (!this.upgradeEquip(protocol, logger, user, officers, equipment, use_gold && use_gold_silver <= equipment.UpgradeCost, use_assist_silver <= equipment.UpgradeCost, qh_idstr))
                        {
                            result = 10;
                            return result;
                        }
                        flag3 = true;
                        break;
                    }
                }
                if (flag3)
                {
                    result = 0;
                }
                else
                {
                    result = 2;
                }
            }
            return result;
        }

        private void renderQhInfo(ProtocolMgr protocol, ILogger logger, XmlDocument xml, User user, out List<EquipMgr.Officer> _officers, string qh_idstr, bool qh_toukui, int toukui_ticket)
        {
            _officers = new List<EquipMgr.Officer>();
            new List<Equipment>();
            base.tryUseCard(protocol, logger, xml);
            XmlNode xmlNode = xml.SelectSingleNode("/results/magic");
            bool flag = xmlNode != null;
            if (flag)
            {
                int num = 0;
                int.TryParse(xmlNode.InnerText, out num);
                bool flag2 = num > 0;
                if (flag2)
                {
                    user.Magic = num;
                }
            }
            XmlNode xmlNode2 = xml.SelectSingleNode("/results/canusegold");
            bool flag3 = xmlNode2 != null;
            if (flag3)
            {
                user.CanUseGoldQh = (xmlNode2.InnerText == "1");
            }
            XmlNode xmlNode3 = xml.SelectSingleNode("/results/officerlist");
            bool flag4 = xmlNode3 != null;
            if (flag4)
            {
                XmlNodeList childNodes = xmlNode3.ChildNodes;
                foreach (XmlNode xmlNode4 in childNodes)
                {
                    EquipMgr.Officer officer = new EquipMgr.Officer();
                    foreach (XmlNode xmlNode5 in xmlNode4.ChildNodes)
                    {
                        bool flag5 = xmlNode5.Name == "playerid";
                        if (flag5)
                        {
                            officer.Id = int.Parse(xmlNode5.InnerText);
                        }
                        else
                        {
                            bool flag6 = xmlNode5.Name == "playername";
                            if (flag6)
                            {
                                officer.Name = xmlNode5.InnerText;
                            }
                            else
                            {
                                bool flag7 = xmlNode5.Name == "pos";
                                if (flag7)
                                {
                                    officer.Pos = int.Parse(xmlNode5.InnerText);
                                }
                                else
                                {
                                    bool flag8 = xmlNode5.Name == "playerlevel";
                                    if (flag8)
                                    {
                                        officer.Level = int.Parse(xmlNode5.InnerText);
                                    }
                                    else
                                    {
                                        bool flag9 = xmlNode5.Name == "ratio";
                                        if (flag9)
                                        {
                                            officer.Ratio = float.Parse(xmlNode5.InnerText);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    _officers.Add(officer);
                }
            }
            XmlNode xmlNode6 = xml.SelectSingleNode("/results");
            XmlNodeList childNodes2 = xmlNode6.ChildNodes;
            List<int> list = new List<int>();
            foreach (XmlNode xmlNode7 in childNodes2)
            {
                bool flag10 = !(xmlNode7.Name != "equip");
                if (flag10)
                {
                    XmlNode xmlNode8 = xmlNode7.SelectSingleNode("storeid");
                    bool flag11 = xmlNode8 != null;
                    if (flag11)
                    {
                        int num2 = 0;
                        int.TryParse(xmlNode8.InnerText, out num2);
                        bool flag12 = num2 != 0;
                        if (flag12)
                        {
                            Equipment equipment = this.findEquip(user._qhEquips, num2);
                            bool flag13 = equipment == null;
                            if (flag13)
                            {
                                equipment = new Equipment();
                                XmlNodeList childNodes3 = xmlNode7.ChildNodes;
                                equipment.fillValues(childNodes3);
                                user._qhEquips.Add(equipment);
                            }
                            else
                            {
                                XmlNodeList childNodes4 = xmlNode7.ChildNodes;
                                equipment.fillValues(childNodes4);
                            }
                            list.Add(num2);
                        }
                    }
                }
            }
            int num3;
            for (int i = 0; i < user._qhEquips.Count; i = num3 + 1)
            {
                int id = user._qhEquips[i].Id;
                bool flag14 = !list.Contains(id);
                if (flag14)
                {
                    user._qhEquips.RemoveAt(i);
                    num3 = i;
                    i = num3 - 1;
                }
                num3 = i;
            }
            List<int> list2 = base.generateIds(qh_idstr);
            foreach (Equipment current in user._qhEquips)
            {
                current.IsChecked = (list2.IndexOf(current.Id) >= 0);
            }
            if (qh_toukui)
            {
                int num4 = 0;
                XmlNode xmlNode9 = xml.SelectSingleNode("/results/ticketnumber");
                bool flag15 = xmlNode9 != null;
                if (flag15)
                {
                    int.TryParse(xmlNode9.InnerText, out num4);
                }
                XmlNodeList xmlNodeList = xml.SelectNodes("/results/toukui");
                foreach (XmlNode xmlNode10 in xmlNodeList)
                {
                    bool flag16 = xmlNode10 != null;
                    if (flag16)
                    {
                        int toukui_id = 0;
                        int num5 = 0;
                        string arg = "";
                        string text = "";
                        bool flag17 = false;
                        XmlNode xmlNode11 = xmlNode10.SelectSingleNode("achievfree");
                        bool flag18 = xmlNode11 != null;
                        if (flag18)
                        {
                            flag17 = (xmlNode11.InnerText != "0");
                        }
                        XmlNode xmlNode12 = xmlNode10.SelectSingleNode("needdianquan");
                        bool flag19 = xmlNode12 != null;
                        if (flag19)
                        {
                            int.TryParse(xmlNode12.InnerText, out num5);
                        }
                        bool flag20 = flag17 || num5 <= toukui_ticket;
                        if (flag20)
                        {
                            XmlNode xmlNode13 = xmlNode10.SelectSingleNode("toukuiid");
                            bool flag21 = xmlNode13 != null;
                            if (flag21)
                            {
                                int.TryParse(xmlNode13.InnerText, out toukui_id);
                            }
                            bool flag22 = !flag17 && num4 < num5;
                            if (flag22)
                            {
                                break;
                            }
                            string a = "0";
                            XmlNode xmlNode14 = xmlNode10.SelectSingleNode("ticketstatus");
                            bool flag23 = xmlNode14 != null;
                            if (flag23)
                            {
                                a = xmlNode14.InnerText;
                            }
                            bool flag24 = !(a == "0");
                            if (flag24)
                            {
                                XmlNode xmlNode15 = xmlNode10.SelectSingleNode("generalname");
                                bool flag25 = xmlNode15 != null;
                                if (flag25)
                                {
                                    text = xmlNode15.InnerText;
                                }
                                bool flag26 = text == "";
                                if (flag26)
                                {
                                    text = "无人使用";
                                }
                                XmlNode xmlNode16 = xmlNode10.SelectSingleNode("name");
                                bool flag27 = xmlNode16 != null;
                                if (flag27)
                                {
                                    arg = xmlNode16.InnerText;
                                }
                                int num6;
                                do
                                {
                                    num6 = this.upgradeToukui(protocol, logger, user, flag17, false, toukui_id, string.Format("{0}({1})", arg, text), toukui_ticket);
                                }
                                while (num6 == 4);
                            }
                        }
                    }
                }
            }
        }

        private bool upgradeEquip(ProtocolMgr protocol, ILogger logger, User user, List<EquipMgr.Officer> _officers, Equipment equip, bool useGold, bool useAssist, string qh_ids)
        {
            bool flag = equip == null;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                int num = -1;
                int num2 = 0;
                int magic = user.Magic;
                EquipMgr.Officer officer = null;
                bool flag2 = useAssist && _officers.Count > 0;
                if (flag2)
                {
                    officer = _officers[0];
                    num = officer.Id;
                }
                if (useGold)
                {
                    num2 = 100 - magic;
                }
                bool flag3 = !user.CanUseGoldQh;
                if (flag3)
                {
                    num2 = 0;
                }
                int gold = user.Gold;
                bool flag4 = gold < num2;
                if (flag4)
                {
                    base.logInfo(logger, "你太穷了!!!!!!! 连保证强化装备成功的金币都没有, 等1小时再说强化的事吧");
                    result = false;
                }
                else
                {
                    string url = "/root/equip!upgradeMyEquip.action";
                    string data = string.Format("assPlayerId={0}&gold={1}&magic={2}&storeId={3}", new object[]
					{
						num,
						num2,
						magic,
						equip.Id
					});
                    int level = equip.Level;
                    bool flag5 = equip.FragmentCount > 0;
                    if (flag5)
                    {
                        base.logInfo(logger, string.Format("装备[{0}]是碎片, 忽略", equip.Name));
                        result = false;
                    }
                    else
                    {
                        int upgradeCost = equip.UpgradeCost;
                        ServerResult serverResult = protocol.postXml(url, data, "强化装备");
                        bool flag6 = serverResult == null;
                        if (flag6)
                        {
                            result = false;
                        }
                        else
                        {
                            bool flag7 = !serverResult.CmdSucceed;
                            if (flag7)
                            {
                                result = false;
                            }
                            else
                            {
                                XmlDocument cmdResult = serverResult.CmdResult;
                                bool flag8 = true;
                                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/flag");
                                bool flag9 = xmlNode != null;
                                if (flag9)
                                {
                                    flag8 = (xmlNode.InnerText == "1");
                                }
                                bool flag10 = !flag8;
                                if (flag10)
                                {
                                    result = false;
                                }
                                else
                                {
                                    int num3 = 0;
                                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/officerlist");
                                    bool flag11 = xmlNode2 != null;
                                    if (flag11)
                                    {
                                        this.renderQhInfo(protocol, logger, cmdResult, user, out _officers, qh_ids, true, 10000);
                                        Equipment equipment = null;
                                        foreach (Equipment current in user._qhEquips)
                                        {
                                            bool flag12 = current.Id == equip.Id;
                                            if (flag12)
                                            {
                                                equipment = current;
                                                break;
                                            }
                                        }
                                        bool flag13 = equipment != null;
                                        if (flag13)
                                        {
                                            int level2 = equipment.Level;
                                            num3 = level2 - level;
                                        }
                                    }
                                    else
                                    {
                                        XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/magic");
                                        bool flag14 = xmlNode3 != null;
                                        if (flag14)
                                        {
                                            int.TryParse(xmlNode3.InnerText, out magic);
                                            bool flag15 = magic > 0;
                                            if (flag15)
                                            {
                                                user.Magic = magic;
                                            }
                                        }
                                        XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/canusegold");
                                        bool flag16 = xmlNode4 != null;
                                        if (flag16)
                                        {
                                            user.CanUseGoldQh = (xmlNode4.InnerText == "1");
                                        }
                                        XmlNodeList childNodes = cmdResult.SelectSingleNode("/results").ChildNodes;
                                        bool flag17 = officer != null;
                                        if (flag17)
                                        {
                                            _officers.Remove(officer);
                                        }
                                        foreach (XmlNode xmlNode5 in childNodes)
                                        {
                                            bool flag18 = xmlNode5.Name == "flag" && xmlNode5.InnerText == "1";
                                            if (flag18)
                                            {
                                                num3 = 1;
                                            }
                                        }
                                    }
                                    string text = "";
                                    bool flag19 = num > 0;
                                    if (flag19)
                                    {
                                        text += string.Format("使用强化辅助: {0}({1}), 降低费用百分比: {2:f2}", officer.Name, officer.Level, officer.Ratio);
                                        bool flag20 = _officers.Contains(officer);
                                        if (flag20)
                                        {
                                            _officers.Remove(officer);
                                        }
                                    }
                                    text += string.Format("强化装备 [{0}] {1}, 花费 [{2}]银币", equip.EquipNameWithGeneral, (num3 > 0) ? ("成功, 级别+" + num3) : "失败", upgradeCost);
                                    bool flag21 = num2 > 0;
                                    if (flag21)
                                    {
                                        text += string.Format(", 花费金币 [{0}]个", num2);
                                    }
                                    base.logInfo(logger, text);
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int upgradeToukui(ProtocolMgr protocol, ILogger logger, User user, bool is_free, bool useGold, int toukui_id, string toukui_desc, int toukui_ticket)
        {
            string url = "/root/toukui!upgrade.action";
            string data = string.Format("consumeType={0}&useShenhuo=0&toukuiId={1}", useGold ? 1 : 0, toukui_id);
            string text;
            if (is_free)
            {
                text = "免费强化";
            }
            else if (useGold)
            {
                text = "金币强化";
            }
            else
            {
                text = "点券强化";
            }
            text += toukui_desc;
            ServerResult serverResult = protocol.postXml(url, data, text);
            bool flag = serverResult == null;
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = !serverResult.CmdSucceed;
                if (flag2)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/updatetype");
                    bool flag3 = xmlNode != null;
                    if (flag3)
                    {
                        string innerText = xmlNode.InnerText;
                        string code = "";
                        string arg = "";
                        XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/updateattributetype");
                        bool flag4 = xmlNode2 != null;
                        if (flag4)
                        {
                            code = xmlNode2.InnerText;
                        }
                        XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/updateattributevalue");
                        bool flag5 = xmlNode3 != null;
                        if (flag5)
                        {
                            arg = xmlNode3.InnerText;
                        }
                        string toukuiAttName = this.getToukuiAttName(code);
                        bool flag6 = innerText == "base";
                        if (flag6)
                        {
                            text += string.Format(", 基础属性增加, {0}增加{1}", toukuiAttName, arg);
                        }
                        else
                        {
                            bool flag7 = innerText == "spacial";
                            if (flag7)
                            {
                                text += string.Format(", 专有属性升级, {0}升级", toukuiAttName);
                            }
                            else
                            {
                                bool flag8 = innerText == "qualitySymbol";
                                if (flag8)
                                {
                                    int num = 1;
                                    XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/quality");
                                    bool flag9 = xmlNode4 != null;
                                    if (flag9)
                                    {
                                        int.TryParse(xmlNode4.InnerText, out num);
                                    }
                                    string[] array = new string[]
									{
										"白色",
										"蓝色",
										"绿色",
										"黄色",
										"红色",
										"紫色"
									};
                                    bool flag10 = num < 0 || num > array.Length - 1;
                                    if (flag10)
                                    {
                                        num = 1;
                                    }
                                    text += string.Format(", 头盔品质升级为{0}", array[num - 1]);
                                }
                            }
                        }
                        base.logInfo(logger, text);
                    }
                    int num2 = 0;
                    XmlNode xmlNode5 = cmdResult.SelectSingleNode("/results/needdianquan");
                    bool flag11 = xmlNode5 != null;
                    if (flag11)
                    {
                        int.TryParse(xmlNode5.InnerText, out num2);
                    }
                    bool flag12 = num2 <= toukui_ticket;
                    if (flag12)
                    {
                        result = 4;
                    }
                    else
                    {
                        string a = "0";
                        XmlNode xmlNode6 = cmdResult.SelectSingleNode("/results/ticketstatus");
                        bool flag13 = xmlNode6 != null;
                        if (flag13)
                        {
                            a = xmlNode6.InnerText;
                        }
                        bool flag14 = a == "0";
                        if (flag14)
                        {
                            result = 3;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                }
            }
            return result;
        }

        private string getToukuiAttName(string code)
        {
            bool flag = code == "att";
            string result;
            if (flag)
            {
                result = "普通攻击";
            }
            else
            {
                bool flag2 = code == "def";
                if (flag2)
                {
                    result = "普通防御";
                }
                else
                {
                    bool flag3 = code == "satt";
                    if (flag3)
                    {
                        result = "战法攻击";
                    }
                    else
                    {
                        bool flag4 = code == "sdef";
                        if (flag4)
                        {
                            result = "战法防御";
                        }
                        else
                        {
                            bool flag5 = code == "stgatt";
                            if (flag5)
                            {
                                result = "策略攻击";
                            }
                            else
                            {
                                bool flag6 = code == "stgdef";
                                if (flag6)
                                {
                                    result = "策略防御";
                                }
                                else
                                {
                                    result = "可带兵数";
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool enchantEquip(ProtocolMgr protocol, ILogger logger, User user, Equipment equip, int mhtype, string mh_ids)
        {
            if (equip == null)
            {
                return false;
            }
            string url = "/root/enchant!enchant.action";
            string data = string.Format("magic={0}&quantity={1}&storeId={2}&halfBowlder=false&useGold=false", user.Magic, mhtype, equip.Id);
            if (equip.FragmentCount > 0)
            {
                base.logInfo(logger, string.Format("装备[{0}]是碎片, 忽略", equip.Name));
                return false;
            }
            int num = 0;
            if (mhtype == 1)
            {
                num = equip.EnchantCost1;
            }
            else if (mhtype == 2)
            {
                num = equip.EnchantCost2;
            }
            else if (mhtype == 3)
            {
                num = equip.EnchantCost3;
            }
            ServerResult serverResult = protocol.postXml(url, data, "魔化装备");
            if (serverResult == null || !serverResult.CmdSucceed)
            {
                return false;
            }
            if (!serverResult.CmdSucceed)
            {
                return false;
            }
            XmlDocument cmdResult = serverResult.CmdResult;
            this.renderMhInfo(cmdResult, user, mh_ids);
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/enchantingchange");
            if (xmlNode == null)
            {
                return false;
            }
            int num2 = 0;
            if (int.TryParse(xmlNode.InnerText, out num2))
            {
                string[] array = new string[] { "少量", "大量", "海量" };
                string text = string.Format("[{0}]方式魔化化装备[{1}] [{2}{3}], 花费 [{4}] 玉石", array[mhtype - 1], equip.EquipNameWithGeneral, (num2 > 0) ? "成功+" : "失败", num2, num);
                base.logInfo(logger, text);
                return num2 > 0;
            }
            return false;
        }

        private Equipment findEquip(List<Equipment> equips, int id)
        {
            Equipment result;
            foreach (Equipment current in equips)
            {
                bool flag = current.Id == id;
                if (flag)
                {
                    result = current;
                    return result;
                }
            }
            result = null;
            return result;
        }

        public int getEquipmentsCanMh(ProtocolMgr protocol, ILogger logger, User user, string mh_idstr)
        {
            string url = "/root/enchant!getEnchantEquipsInfo.action";
            ServerResult xml = protocol.getXml(url, "获取装备魔化信息");
            if (xml == null)
            {
                return 1;
            }
            else if (!xml.CmdSucceed)
            {
                return 10;
            }
            else
            {
                XmlDocument cmdResult = xml.CmdResult;
                this.renderMhInfo(cmdResult, user, mh_idstr);
                return 0;
            }
        }

        public int handleEquipmentsCanMh(ProtocolMgr protocol, ILogger logger, User user, string mh_idstr, int fail_count_max, string mh_type, int stone_available)
        {
            this.getEquipmentsCanMh(protocol, logger, user, mh_idstr);
            int num = 1;
            if (mh_type == "少量")
            {
                num = 1;
            }
            else if (mh_type == "大量")
            {
                num = 2;
            }
            else if (mh_type == "海量")
            {
                num = 3;
            }
            List<int> list = base.generateIds(mh_idstr);
            if (list.Count > 0)
            {
                int i = 0;
                while (i < fail_count_max)
                {
                    Equipment equipment = null;
                    foreach (int current in list)
                    {
                        Equipment equipment2 = this.findEquip(user._mhEquips, current);
                        if (equipment2 != null && equipment2.CanEnchant && equipment2.EnchantCost1 <= stone_available && (num != 2 || equipment2.EnchantCost2 <= stone_available) && (num != 3 || equipment2.EnchantCost3 <= stone_available))
                        {
                            equipment = equipment2;
                            break;
                        }
                    }
                    if (equipment == null)
                    {
                        return 2;
                    }
                    if (this.enchantEquip(protocol, logger, user, equipment, num, mh_idstr))
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                    int num3 = 0;
                    if (num == 1)
                    {
                        num3 = equipment.EnchantCost1;
                    }
                    else if (num == 2)
                    {
                        num3 = equipment.EnchantCost2;
                    }
                    else if (num == 3)
                    {
                        num3 = equipment.EnchantCost3;
                    }
                    stone_available -= num3;
                }
            }
            return 0;
        }

        private void renderMhInfo(XmlDocument xml, User user, string mh_ids)
        {
            List<Equipment> list = new List<Equipment>();
            XmlNode xmlNode = xml.SelectSingleNode("/results");
            XmlNodeList childNodes = xmlNode.ChildNodes;
            List<int> list2 = new List<int>();
            foreach (XmlNode xmlNode2 in childNodes)
            {
                bool flag = !(xmlNode2.Name != "enchant");
                if (flag)
                {
                    XmlNode xmlNode3 = xmlNode2.SelectSingleNode("storeid");
                    bool flag2 = xmlNode3 != null;
                    if (flag2)
                    {
                        int num = 0;
                        int.TryParse(xmlNode3.InnerText, out num);
                        bool flag3 = num != 0;
                        if (flag3)
                        {
                            Equipment equipment = this.findEquip(user._mhEquips, num);
                            bool flag4 = equipment == null;
                            if (flag4)
                            {
                                equipment = new Equipment();
                                XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                                equipment.fillValues(childNodes2);
                                user._mhEquips.Add(equipment);
                            }
                            else
                            {
                                XmlNodeList childNodes3 = xmlNode2.ChildNodes;
                                equipment.fillValues(childNodes3);
                            }
                            list2.Add(num);
                        }
                    }
                }
            }
            int num2;
            for (int i = 0; i < user._mhEquips.Count; i = num2 + 1)
            {
                int id = user._mhEquips[i].Id;
                bool flag5 = !list2.Contains(id);
                if (flag5)
                {
                    user._mhEquips.RemoveAt(i);
                    num2 = i;
                    i = num2 - 1;
                }
                num2 = i;
            }
            List<int> list3 = base.generateIds(mh_ids);
            foreach (Equipment current in list)
            {
                current.IsChecked = (list3.IndexOf(current.Id) >= 0);
            }
        }

        public List<Equipment> getWeaponInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            List<Equipment> list = new List<Equipment>();
            bool flag = user.Level < 120;
            List<Equipment> result;
            if (flag)
            {
                result = list;
            }
            else
            {
                string url = "/root/troopEquip!getEquip.action";
                ServerResult xml = protocol.getXml(url, "获取兵器信息");
                bool flag2 = xml == null;
                if (flag2)
                {
                    result = list;
                }
                else
                {
                    bool flag3 = !xml.CmdSucceed;
                    if (flag3)
                    {
                        result = list;
                    }
                    else
                    {
                        XmlDocument cmdResult = xml.CmdResult;
                        XmlNode xmlNode = cmdResult.SelectSingleNode("/results/bowlder");
                        bool flag4 = xmlNode != null;
                        if (flag4)
                        {
                            int num = 0;
                            int.TryParse(xmlNode.InnerText, out num);
                            bool flag5 = num > 0;
                            if (flag5)
                            {
                                user.Stone = num;
                            }
                        }
                        XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results");
                        XmlNodeList childNodes = xmlNode2.ChildNodes;
                        foreach (XmlNode xmlNode3 in childNodes)
                        {
                            bool flag6 = !(xmlNode3.Name != "equip");
                            if (flag6)
                            {
                                Equipment equipment = new Equipment();
                                XmlNodeList childNodes2 = xmlNode3.ChildNodes;
                                equipment.fillValues(childNodes2);
                                equipment.goodstype = GoodsType.Weapon;
                                list.Add(equipment);
                            }
                        }
                        result = list;
                    }
                }
            }
            return result;
        }

        public int handleWeaponsUpgrade(ProtocolMgr protocol, ILogger logger, User user, bool only_getInfo)
        {
            string text = "";
            bool flag = user.Level < 120;
            int result;
            if (flag)
            {
                result = 10;
            }
            else
            {
                string url = "/root/troopEquip!getEquip.action";
                ServerResult xml = protocol.getXml(url, "获取兵器信息");
                bool flag2 = xml == null;
                if (flag2)
                {
                    result = 1;
                }
                else
                {
                    bool flag3 = !xml.CmdSucceed;
                    if (flag3)
                    {
                        result = 10;
                    }
                    else
                    {
                        XmlDocument cmdResult = xml.CmdResult;
                        base.tryUseCard(protocol, logger, cmdResult);
                        List<Equipment> list = new List<Equipment>();
                        XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                        XmlNodeList childNodes = xmlNode.ChildNodes;
                        int num = 0;
                        int num2 = 0;
                        double num3 = 0.0;
                        foreach (XmlNode xmlNode2 in childNodes)
                        {
                            bool flag4 = !(xmlNode2.Name != "equip");
                            if (flag4)
                            {
                                Equipment equipment = new Equipment();
                                XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                                equipment.fillValues(childNodes2);
                                equipment.goodstype = GoodsType.Weapon;
                                list.Add(equipment);
                                string arg2;
                                string arg = this.renderWeaponInfo(equipment, ref num, ref num2, ref num3, out arg2);
                                text += string.Format("{0}|{1}/", arg2, arg);
                            }
                        }
                        int mozi = -1;
                        XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/mozichuizi");
                        bool flag5 = xmlNode3 != null;
                        if (flag5)
                        {
                            int.TryParse(xmlNode3.InnerText, out mozi);
                        }
                        text += string.Format("black|总星级:{0}, 平均星级:{1:f2}/", num2, (double)num2 * 1.0 / (double)num);
                        text += string.Format("black|总坚韧:{0:f2}%, 平均坚韧:{1:f2}%", num3, num3 / (double)num);
                        user._weapon_info = text;
                        if (only_getInfo)
                        {
                            result = 0;
                        }
                        else
                        {
                            int stone = user.Stone;
                            bool flag6 = stone >= 90;
                            if (flag6)
                            {
                                bool flag7 = false;
                                foreach (Equipment current in list)
                                {
                                    bool flag8 = current.CanUpgrade && ((!current._isSuperWeapon && ((current.Level < current.CurWeaponLevelLimit && current.ZeroStarNum > 0) || (current.Level == current.CurWeaponLevelLimit && current.Quality == EquipmentQuality.Purple && current.ZeroStarNum >= 3 && stone >= 270))) || (current._isSuperWeapon && current.ZeroStarNum >= current.OnceNum && stone >= current.OnceNum * 90));
                                    if (flag8)
                                    {
                                        flag7 = this.upgradeWeapon(protocol, logger, current, mozi);
                                        break;
                                    }
                                }
                                bool flag9 = flag7;
                                if (flag9)
                                {
                                    result = 0;
                                    return result;
                                }
                            }
                            result = 2;
                        }
                    }
                }
            }
            return result;
        }

        public bool upgradeWeapon(ProtocolMgr protocol, ILogger logger, Equipment weapon, int mozi)
        {
            string url = "/root/troopEquip!upgradeMyEquip.action";
            string data = string.Format("storeId={0}&halfBowlder=false", weapon.Id);
            bool flag = weapon == null;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = weapon.FragmentCount > 0;
                if (flag2)
                {
                    base.logInfo(logger, string.Format("兵器[{0}]是碎片, 忽略", weapon.Name));
                    result = false;
                }
                else
                {
                    bool isSuperWeapon = weapon._isSuperWeapon;
                    if (isSuperWeapon)
                    {
                        url = "/root/troopEquip!cuilianEquip.action";
                        bool flag3 = mozi == -1;
                        if (flag3)
                        {
                            data = string.Format("storeId={0}", weapon.Id);
                        }
                        else
                        {
                            bool flag4 = mozi == 0;
                            if (flag4)
                            {
                                data = string.Format("chuizi=false&storeId={0}", weapon.Id);
                            }
                            else
                            {
                                data = string.Format("chuizi=true&storeId={0}", weapon.Id);
                            }
                        }
                    }
                    string text = "强化兵器";
                    bool flag5 = weapon.Level == weapon.CurWeaponLevelLimit;
                    if (flag5)
                    {
                        text = "补充兵器坚韧度";
                    }
                    bool isSuperWeapon2 = weapon._isSuperWeapon;
                    if (isSuperWeapon2)
                    {
                        text = "淬炼神兵";
                    }
                    ServerResult serverResult = protocol.postXml(url, data, text);
                    bool flag6 = serverResult == null;
                    if (flag6)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag7 = !serverResult.CmdSucceed;
                        if (flag7)
                        {
                            result = false;
                        }
                        else
                        {
                            XmlDocument cmdResult = serverResult.CmdResult;
                            bool flag8 = false;
                            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/successupgrade");
                            bool flag9 = xmlNode != null;
                            if (flag9)
                            {
                                flag8 = (xmlNode.InnerText == "1");
                            }
                            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/cuilian/upgrade");
                            bool flag10 = xmlNode2 != null;
                            if (flag10)
                            {
                                flag8 = (xmlNode2.InnerText == "1");
                            }
                            string text2 = string.Format("{0} [{1}] {2}", text, weapon.Name, flag8 ? "成功" : "失败");
                            base.logInfo(logger, text2);
                            bool flag11 = flag8;
                            if (flag11)
                            {
                                logger.logSurprise(text2);
                            }
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        private string renderWeaponInfo(Equipment weapon, ref int weapon_count, ref int weapon_total_stars, ref double weapon_total_tonghness, out string color)
        {
            int num;
            double num2;
            double num3;
            int num4;
            int num5;
            WeaponHelper.getWeaponInfo(weapon, 0, out num, out num2, out num3, out num4, out num5);
            string result = string.Format("{0}: {1}星  {2}, 失败{3}次", new object[]
			{
				weapon.Name.PadLeft(5, '一'),
				weapon.Level.ToString().PadLeft(3),
				weapon.ValueNow.ToString().PadLeft(6),
				num4.ToString().PadLeft(4)
			});
            bool flag = weapon.Quality == EquipmentQuality.White;
            if (flag)
            {
                color = "gray";
            }
            else
            {
                bool flag2 = weapon.Quality == EquipmentQuality.Blue;
                if (flag2)
                {
                    color = "blue";
                }
                else
                {
                    bool flag3 = weapon.Quality == EquipmentQuality.Green;
                    if (flag3)
                    {
                        color = "green";
                    }
                    else
                    {
                        bool flag4 = weapon.Quality == EquipmentQuality.Yellow;
                        if (flag4)
                        {
                            color = "yellow";
                        }
                        else
                        {
                            bool flag5 = weapon.Quality == EquipmentQuality.Red;
                            if (flag5)
                            {
                                color = "red";
                            }
                            else
                            {
                                color = "purple";
                                bool isSuperWeapon = weapon._isSuperWeapon;
                                if (isSuperWeapon)
                                {
                                    result = string.Format("{0}:{1}阶 {2}, 淬炼进度 {3}次, 还差{4}次", new object[]
									{
										weapon.Name.PadLeft(5, '一'),
										weapon._curCuilianLevel.ToString().PadLeft(3),
										weapon.ValueNow.ToString().PadLeft(6),
										weapon._curCuilianCount,
										weapon._cuilianNeedCount - weapon._curCuilianCount
									});
                                }
                                else
                                {
                                    result = string.Format("{0}:{1}星 {2}, 坚韧{3},满{4},差{5}", new object[]
									{
										weapon.Name.PadLeft(5, '一'),
										weapon.Level.ToString().PadLeft(3),
										weapon.ValueNow.ToString().PadLeft(6),
										string.Format("{0:f2}%", num2 * 100.0).PadLeft(7),
										string.Format("{0:f2}%", num3 * 100.0).PadLeft(7),
										string.Format("{0:f2}%", (num3 - num2) * 100.0).PadLeft(7)
									});
                                }
                                int num6 = weapon_count;
                                weapon_count = num6 + 1;
                                weapon_total_stars += weapon.Level;
                                weapon_total_tonghness += num2 * 100.0;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int handleDailyWeapon(ProtocolMgr protocol, ILogger logger, out bool hasGotToday, string target_weapon_name)
        {
            hasGotToday = false;
            Dictionary<string, string> dictionary;
            List<string> list;
            this.getDailyWeaponDatabase(out dictionary, out list);
            bool flag = list.IndexOf(target_weapon_name) < 0;
            int result;
            if (flag)
            {
                result = 3;
            }
            else
            {
                string url = "/root/playerAnswer!getQuestion2.action";
                ServerResult xml = protocol.getXml(url, "获取每日兵器信息");
                bool flag2 = xml == null;
                if (flag2)
                {
                    result = 1;
                }
                else
                {
                    bool flag3 = !xml.CmdSucceed;
                    if (flag3)
                    {
                        result = 10;
                    }
                    else
                    {
                        XmlDocument cmdResult = xml.CmdResult;
                        XmlNode xmlNode = cmdResult.SelectSingleNode("/results/isend");
                        bool flag4 = xmlNode != null;
                        if (flag4)
                        {
                            hasGotToday = true;
                            result = 2;
                        }
                        else
                        {
                            int num = 0;
                            int num2 = 0;
                            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/maxrewardtimes");
                            bool flag5 = xmlNode2 != null;
                            if (flag5)
                            {
                                int.TryParse(xmlNode2.InnerText, out num);
                            }
                            XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/rewardtimes");
                            bool flag6 = xmlNode3 != null;
                            if (flag6)
                            {
                                int.TryParse(xmlNode3.InnerText, out num2);
                            }
                            bool flag7 = num > 0 && num == num2;
                            if (flag7)
                            {
                                hasGotToday = true;
                                result = 2;
                            }
                            else
                            {
                                hasGotToday = false;
                                XmlNode xmlNode4 = cmdResult.SelectSingleNode("/results/canreceive");
                                bool flag8 = xmlNode4 == null || !(xmlNode4.InnerText == "1");
                                if (flag8)
                                {
                                    List<string> list2 = new List<string>();
                                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/question/pic");
                                    bool flag9 = xmlNodeList != null && xmlNodeList.Count > 0;
                                    if (flag9)
                                    {
                                        foreach (XmlNode xmlNode5 in xmlNodeList)
                                        {
                                            bool flag10 = dictionary.ContainsKey(xmlNode5.InnerText);
                                            if (flag10)
                                            {
                                                list2.Add(dictionary[xmlNode5.InnerText]);
                                            }
                                            else
                                            {
                                                list2.Add("");
                                                base.logInfo(logger, "找不到每日兵器问题的key, 记录在此, key=" + xmlNode5.InnerText);
                                            }
                                        }
                                    }
                                    List<EquipMgr.Answer> list3 = new List<EquipMgr.Answer>();
                                    XmlNodeList xmlNodeList2 = cmdResult.SelectNodes("/results/answer");
                                    foreach (XmlNode xmlNode6 in xmlNodeList2)
                                    {
                                        EquipMgr.Answer answer = new EquipMgr.Answer();
                                        XmlNodeList childNodes = xmlNode6.ChildNodes;
                                        foreach (XmlNode xmlNode7 in childNodes)
                                        {
                                            EquipMgr.AnswerOption answerOption = new EquipMgr.AnswerOption();
                                            XmlNodeList childNodes2 = xmlNode7.ChildNodes;
                                            foreach (XmlNode xmlNode8 in childNodes2)
                                            {
                                                bool flag11 = xmlNode8.Name == "index";
                                                if (flag11)
                                                {
                                                    answerOption.index = xmlNode8.InnerText;
                                                }
                                                else
                                                {
                                                    bool flag12 = xmlNode8.Name == "name";
                                                    if (flag12)
                                                    {
                                                        answerOption.name = xmlNode8.InnerText;
                                                    }
                                                }
                                            }
                                            answer.addOption(answerOption);
                                        }
                                        list3.Add(answer);
                                    }
                                    List<string> list4 = new List<string>();
                                    int num3 = 0;
                                    foreach (string current in list2)
                                    {
                                        bool flag13 = list3.Count - 1 >= num3;
                                        if (flag13)
                                        {
                                            list4.Add(list3[num3].getAnswerIndex(current));
                                        }
                                        int num4 = num3;
                                        num3 = num4 + 1;
                                    }
                                    this.answerDailyWeaponQuestion(protocol, logger, list4);
                                    result = 0;
                                }
                                else
                                {
                                    string text = "";
                                    XmlNodeList xmlNodeList3 = cmdResult.SelectNodes("/results/reward");
                                    foreach (XmlNode xmlNode9 in xmlNodeList3)
                                    {
                                        EquipMgr.Reward reward = new EquipMgr.Reward();
                                        XmlNodeList childNodes3 = xmlNode9.ChildNodes;
                                        foreach (XmlNode xmlNode10 in childNodes3)
                                        {
                                            bool flag14 = xmlNode10.Name == "type";
                                            if (flag14)
                                            {
                                                reward.type = xmlNode10.InnerText;
                                            }
                                            else
                                            {
                                                bool flag15 = xmlNode10.Name == "name";
                                                if (flag15)
                                                {
                                                    reward.name = xmlNode10.InnerText;
                                                }
                                                else
                                                {
                                                    bool flag16 = xmlNode10.Name == "quality";
                                                    if (flag16)
                                                    {
                                                        reward.quality = xmlNode10.InnerText;
                                                    }
                                                    else
                                                    {
                                                        bool flag17 = xmlNode10.Name == "goodsid";
                                                        if (flag17)
                                                        {
                                                            reward.goodsid = xmlNode10.InnerText;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        bool flag18 = reward.name.IndexOf(target_weapon_name) >= 0;
                                        if (flag18)
                                        {
                                            text = reward.goodsid;
                                            break;
                                        }
                                    }
                                    bool flag19 = text != "" && this.getDailyWeaponReward(protocol, logger, text);
                                    if (flag19)
                                    {
                                        result = 2;
                                    }
                                    else
                                    {
                                        result = 10;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool answerDailyWeaponQuestion(ProtocolMgr protocol, ILogger logger, List<string> weaponIds)
        {
            string url = "/root/playerAnswer!answer2.action";
            string text = "";
            int i = 0;
            int count = weaponIds.Count;
            while (i < count)
            {
                text += weaponIds[i];
                bool flag = i < count - 1;
                if (flag)
                {
                    text += ",";
                }
                int num = i;
                i = num + 1;
            }
            string data = string.Format("answer={0}", text);
            ServerResult serverResult = protocol.postXml(url, data, "回答每日兵器问题");
            bool flag2 = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag2)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/totalcorrect");
                bool flag3 = false;
                bool flag4 = xmlNode != null;
                if (flag4)
                {
                    flag3 = xmlNode.InnerText.Equals("true");
                }
                string text2 = string.Format("回答每日兵器问题 {0}", flag3 ? "成功" : "失败");
                base.logInfo(logger, text2);
                result = true;
            }
            return result;
        }

        public bool getDailyWeaponReward(ProtocolMgr protocol, ILogger logger, string weaponId)
        {
            string url = "/root/playerAnswer!receiveFinalReward2.action";
            string data = "giftIndex=" + weaponId;
            ServerResult serverResult = protocol.postXml(url, data, "获取每日兵器奖励");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/reward");
                XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/rewardnum");
                string arg = "";
                string arg2 = "";
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    arg = xmlNode.InnerText;
                }
                bool flag3 = xmlNode2 != null;
                if (flag3)
                {
                    arg2 = xmlNode2.InnerText;
                }
                string text = string.Format("获取每日兵器奖励 {0}*{1}", arg, arg2);
                base.logInfo(logger, text);
                result = true;
            }
            return result;
        }

        private void getDailyWeaponDatabase(out Dictionary<string, string> _weaponImages, out List<string> _weaponDedicated)
        {
            _weaponImages = new Dictionary<string, string>();
            _weaponImages.Add("sunbinbingfa", "孙膑兵法");
            _weaponImages.Add("guiguzi", "鬼谷子");
            _weaponImages.Add("sunzibingfa", "孙子兵法");
            _weaponImages.Add("luoshenfu", "洛神赋");
            _weaponImages.Add("baiyuqilinbingfu", "白玉麒麟兵符");
            _weaponImages.Add("liulongfeipei", "六龙飞辔");
            _weaponImages.Add("zhangbashemao", "丈八蛇矛");
            _weaponImages.Add("longlinzijindao", "龙鳞紫金刀");
            _weaponImages.Add("taihejian", "太和剑");
            _weaponImages.Add("qilinshuangqiang", "麒麟双枪");
            _weaponImages.Add("jiuxiaoshennu", "九霄神怒");
            _weaponImages.Add("daxialongque", "大夏龙雀");
            _weaponImages.Add("16101", "雪霁");
            _weaponImages.Add("danxiabiri", "丹霞蔽日");
            _weaponImages.Add("tongyunjiao", "彤云角");
            _weaponImages.Add("hongyijiangjunpao", "红衣将军炮");
            _weaponImages.Add("huagucongji", "化骨丛棘");
            _weaponImages.Add("shilvyan", "弑履岩");
            _weaponImages.Add("yulinjinghui", "玉麟旌麾");
            _weaponImages.Add("tongrenjiliche", "铜人计里车");
            _weaponImages.Add("huaguxuandeng", "化骨悬灯");
            _weaponImages.Add("xuantingjiao", "玄霆角");
            _weaponImages.Add("wudijiangjunpao", "无敌将军炮");
            _weaponImages.Add("wuduwenxinding", "五毒问心钉");
            _weaponImages.Add("qilufeng", "七戮锋");
            _weaponImages.Add("panlonghuagai", "蟠龙华盖");
            _weaponImages.Add("xuanyuanzhinanche", "轩辕指南车");
            _weaponImages.Add("luohunmingdeng", "落魂冥灯");
            _weaponImages.Add("dayelonglinjia", "大叶龙鳞甲");
            _weaponImages.Add("xuanwukai", "玄武铠");
            _weaponImages.Add("26101", "三味纯阳铠");
            _weaponImages.Add("yulvjindaijia", "玉镂金带甲");
            _weaponImages.Add("xianglong", "象龙");
            _weaponImages.Add("menghu", "猛虎");
            _weaponImages.Add("xiongshi", "雄狮");
            _weaponImages.Add("zhanxiang", "战象");
            _weaponImages.Add("qibu", "七步");
            _weaponImages.Add("qilin", "麒麟");
            _weaponImages.Add("fengyulongfeipifeng", "凤羽龙飞披风");
            _weaponImages.Add("45101", "千手叶檀披风");
            _weaponImages.Add("46101", "蝶凤舞阳");
            _weaponDedicated = new List<string>();
            _weaponDedicated.Add("彤云角");
            _weaponDedicated.Add("红衣将军炮");
            _weaponDedicated.Add("化骨丛棘");
            _weaponDedicated.Add("弑履岩");
            _weaponDedicated.Add("玉麟旌麾");
            _weaponDedicated.Add("铜人计里车");
            _weaponDedicated.Add("化骨悬灯");
            _weaponDedicated.Add("玄霆角");
            _weaponDedicated.Add("无敌将军炮");
            _weaponDedicated.Add("五毒问心钉");
            _weaponDedicated.Add("七戮锋");
            _weaponDedicated.Add("蟠龙华盖");
            _weaponDedicated.Add("轩辕指南车");
            _weaponDedicated.Add("落魂冥灯");
        }

        public int handleMerchants(ProtocolMgr protocol, ILogger logger, int silver_available, bool only_free, string merchant_type, string sell_quality)
        {
            List<EquipMgr.Merchant> list = new List<EquipMgr.Merchant>();
            int num = 0;
            bool flag = false;
            bool flag2 = false;
            string url = "/root/market!getPlayerMerchant.action";
            ServerResult xml = protocol.getXml(url, "获取委派商人信息");
            bool flag3 = xml == null;
            int result;
            if (flag3)
            {
                result = 1;
            }
            else
            {
                bool flag4 = !xml.CmdSucceed;
                if (flag4)
                {
                    result = 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cd");
                    bool flag5 = xmlNode != null;
                    if (flag5)
                    {
                        num = int.Parse(xmlNode.InnerText);
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/cdflag");
                    bool flag6 = xmlNode2 != null;
                    if (flag6)
                    {
                        flag = (xmlNode2.InnerText == "1");
                    }
                    XmlNode xmlNode3 = cmdResult.SelectSingleNode("/results/free");
                    bool flag7 = xmlNode3 != null;
                    if (flag7)
                    {
                        flag2 = (xmlNode3.InnerText == "1");
                    }
                    XmlNodeList xmlNodeList = cmdResult.SelectNodes("/results/merchant");
                    foreach (XmlNode xmlNode4 in xmlNodeList)
                    {
                        EquipMgr.Merchant merchant = new EquipMgr.Merchant();
                        XmlNodeList childNodes = xmlNode4.ChildNodes;
                        foreach (XmlNode xmlNode5 in childNodes)
                        {
                            bool flag8 = xmlNode5.Name == "merchantid";
                            if (flag8)
                            {
                                merchant.merchantid = int.Parse(xmlNode5.InnerText);
                            }
                            else
                            {
                                bool flag9 = xmlNode5.Name == "merchantname";
                                if (flag9)
                                {
                                    merchant.merchantname = xmlNode5.InnerText;
                                }
                                else
                                {
                                    bool flag10 = xmlNode5.Name == "cost";
                                    if (flag10)
                                    {
                                        merchant.cost = int.Parse(xmlNode5.InnerText);
                                    }
                                }
                            }
                        }
                        list.Add(merchant);
                    }
                    bool flag11 = only_free && !flag2;
                    if (flag11)
                    {
                        result = 3;
                    }
                    else
                    {
                        bool flag12 = flag && num > 2000;
                        if (flag12)
                        {
                            result = num;
                        }
                        else
                        {
                            EquipMgr.Merchant merchant2 = null;
                            bool flag13 = merchant_type.Equals("马");
                            if (flag13)
                            {
                                using (List<EquipMgr.Merchant>.Enumerator enumerator3 = list.GetEnumerator())
                                {
                                    while (enumerator3.MoveNext())
                                    {
                                        EquipMgr.Merchant current = enumerator3.Current;
                                        bool flag14 = current.merchantid <= 4;
                                        if (flag14)
                                        {
                                            bool flag15 = merchant2 == null;
                                            if (flag15)
                                            {
                                                merchant2 = current;
                                            }
                                            else
                                            {
                                                bool flag16 = merchant2.merchantid < current.merchantid;
                                                if (flag16)
                                                {
                                                    merchant2 = current;
                                                }
                                            }
                                        }
                                    }
                                    goto IL_34D;
                                }
                            }
                            foreach (EquipMgr.Merchant current2 in list)
                            {
                                bool flag17 = current2.merchantid > 4;
                                if (flag17)
                                {
                                    bool flag18 = merchant2 == null;
                                    if (flag18)
                                    {
                                        merchant2 = current2;
                                    }
                                    else
                                    {
                                        bool flag19 = merchant2.merchantid < current2.merchantid;
                                        if (flag19)
                                        {
                                            merchant2 = current2;
                                        }
                                    }
                                }
                            }
                        IL_34D:
                            bool flag20 = merchant2.cost > silver_available;
                            if (flag20)
                            {
                                result = 2;
                            }
                            else
                            {
                                bool flag21 = this.tradeMerchant(protocol, logger, merchant2, sell_quality) == 0;
                                if (flag21)
                                {
                                    result = 0;
                                }
                                else
                                {
                                    result = 10;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int tradeMerchant(ProtocolMgr protocol, ILogger logger, EquipMgr.Merchant merchant, string sell_quality)
        {
            string url = "/root/market!trade.action";
            string data = string.Format("gold=0&merchantId={0}", merchant.merchantid);
            ServerResult serverResult = protocol.postXml(url, data, "委派" + merchant.merchantname);
            if (serverResult == null)
            {
                return 1;
            }
            else if (!serverResult.CmdSucceed)
            {
                return 2;
            }

            XmlDocument cmdResult = serverResult.CmdResult;
            string[] array = new string[] { "白色", "蓝色", "绿色", "黄色", "红色", "紫色" };
            int sell_index = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(sell_quality))
                {
                    sell_index = i + 1;
                    break;
                }
            }
            string tradeSN = "";
            string merchandisename = "";
            int merchandisequality = 0;
            int cost = 0;
            bool chip = false;
            XmlNode xmlNode = cmdResult.SelectSingleNode("/results/tradesn");
            if (xmlNode != null)
            {
                tradeSN = xmlNode.InnerText;
            }
            XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/merchandise");
            XmlNodeList childNodes = xmlNode2.ChildNodes;
            foreach (XmlNode xmlNode3 in childNodes)
            {
                if (xmlNode3.Name == "merchandisename")
                {
                    merchandisename = xmlNode3.InnerText;
                }
                else if (xmlNode3.Name == "merchandisequality")
                {
                    merchandisequality = int.Parse(xmlNode3.InnerText);
                }
                else if (xmlNode3.Name == "cost")
                {
                    cost = int.Parse(xmlNode3.InnerText);
                }
                else if (xmlNode3.Name == "chip")
                {
                    chip = true;
                }
            }
            string text2 = string.Format("委派[{0}], 花费{1}银币, 获得[{2}{3}], 售价{4}银币", new object[] { merchant.merchantname, merchant.cost, merchandisename, chip ? "碎片" : "", cost });
            base.logInfo(logger, text2);
            if (merchandisequality <= sell_index && !merchandisename.Contains("赤兔马") && !merchandisename.Contains("锦缎虎纹披风"))
            {
                this.sellMerchantGoods(protocol, logger, tradeSN, merchandisename);
            }
            return 0;
        }

        public bool sellMerchantGoods(ProtocolMgr protocol, ILogger logger, string tradeSN, string name)
        {
            string url = "/root/market!confirm.action";
            string data = string.Format("tradeSN={0}", tradeSN);
            ServerResult serverResult = protocol.postXml(url, data, "卖出委派物品");
            bool flag = serverResult == null || !serverResult.CmdSucceed;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results/cost");
                int num = int.Parse(xmlNode.InnerText);
                string text = string.Format("卖出委派物品 [{0}], 银币+{1}", name, num);
                base.logInfo(logger, text);
                result = true;
            }
            return result;
        }

        public int handleWarChariotUpgrade(ProtocolMgr protocol, ILogger logger, User user, bool notChuizi = false)
        {
            if (user.Level < 288)
            {
                return 10;
            }
            else
            {
                string url = "/root/warChariot!getWarChariotInfo.action";
                ServerResult xml = protocol.getXml(url, "获取战车信息");
                if (xml == null)
                {
                    return 1;
                }
                else if (!xml.CmdSucceed)
                {
                    return 10;
                }
                else
                {
                    XmlDocument cmdResult = xml.CmdResult;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                    XmlNodeList childNodes = xmlNode.ChildNodes;
                    WarChariot warChariot = new WarChariot();
                    warChariot.fillValues(childNodes);
                    user.Stone = warChariot.Bowlder;
                    while (user.Stone < warChariot.Needbowlder)
                    {
                        if (this._factory.getMiscManager().ticketExchangeWeapon(protocol, logger, 3, "玉石", 10) != 0)
                        {
                            return 3;
                        }
                        user.Stone += 30000;
                    }
                    while (warChariot.Equipitemnum < warChariot.Needequipitem)
                    {
                        if (this._factory.getMiscManager().ticketExchangeWeapon(protocol, logger, 17, "蟠龙华盖", 10000) != 0)
                        {
                            return 3;
                        }
                        warChariot.Equipitemnum += 50000;
                    }
                    if (warChariot.Needtofull > 0 && (warChariot.Chuizi > 0 || notChuizi))
                    {
                        if (upgradeWarChariot(protocol, logger, warChariot.Chuizi))
                        {
                            return 0;
                        }
                    }
                    return 2;
                }
            }
        }

        public bool upgradeWarChariot(ProtocolMgr protocol, ILogger logger, int chuiziCri)
        {
            string url = "/root/warChariot!strengthenWarChariot.action";
            string data = string.Format("chuiziCri={0}", chuiziCri);
            string text = "强化战车";
            if (chuiziCri == 1)
            {
                text = "墨家铁锤强化战车";
            }
            else if (chuiziCri == 2)
            {
                text = "中级铁锤强化战车";
            }
            else if (chuiziCri == 3)
            {
                text = "高级铁锤强化战车";
            }
            ServerResult serverResult = protocol.postXml(url, data, text);
            if (serverResult == null)
            {
                return false;
            }
            else if (!serverResult.CmdSucceed)
            {
                return false;
            }
            else
            {
                XmlDocument cmdResult = serverResult.CmdResult;
                XmlNode xmlNode = cmdResult.SelectSingleNode("/results");
                WarChariot warChariot = new WarChariot();
                warChariot.fillValues(xmlNode.ChildNodes);
                if (warChariot.Islaststrengthen)
                {
                    string log_string = string.Format("战车 [{0}] 升级到 {1} {1}", warChariot.Name, warChariot.Equiplevel, warChariot.toString());
                    base.logInfo(logger, log_string);
                    logger.logSurprise(log_string);
                }
                else
                {
                    base.logInfo(logger, string.Format("{0} [{1}] 成功 {2}倍暴击 获得余料+{3} {4}", text, warChariot.Name, warChariot.Isbaoji, warChariot.Surplus, warChariot.toString()));
                }
                return true;
            }
        }

        #region 专属
        public long handleSpecialEquipInfo(ProtocolMgr protocol, ILogger logger, User user, double prob)
        {
            List<SheetInfo> list = getSpecialEquipInfo(protocol, logger, user);
            list.Sort();
            if (list.Count == 0)
            {
                return next_day();
            }
            if (user._specialEquipSkillInfo.makestate == 1)
            {
                if (!judgeSpecialEquip(protocol, logger, user))
                {
                    return next_halfhour();
                }
            }
            if (user._specialEquipSkillInfo.makenum <= 0)
            {
                return user._specialEquipSkillInfo.makecd > 0 ? user._specialEquipSkillInfo.makecd : next_hour();
            }
            foreach (SheetInfo item in list)
            {
                if (item.succprob + user._specialEquipSkillInfo.addprob >= prob && item.material2num <= user._specialEquipSkillInfo.material2num && item.material1num <= item.goodsnum)
                {
                    if (!makeSpecialEquip(protocol, logger, user, item))
                    {
                        return next_halfhour();
                    }
                    break;
                }
            }
            return user._specialEquipSkillInfo.endcd;
        }

        public List<SheetInfo> getSpecialEquipInfo(ProtocolMgr protocol, ILogger logger, User user)
        {
            List<SheetInfo> list = new List<SheetInfo>();
            if (user.Level < 300)
            {
                return list;
            }

            string url = "/root/equip!getSpecialEquipInfo.action";
            ServerResult xml = protocol.getXml(url, "获取专属");
            if (xml == null || !xml.CmdSucceed)
            {
                return list;
            }

            user._specialEquipSkillInfo.handle(xml.CmdResult.SelectSingleNode("/results/skillinfo"));

            XmlNode node = xml.CmdResult.SelectSingleNode("/results");
            XmlNodeList nodeList = node.ChildNodes;
            foreach (XmlNode item in nodeList)
            {
                if (item.Name == "sheetinfo")
                {
                    SheetInfo sheet = new SheetInfo();
                    sheet.handle(item);
                    if (sheet.id > 0 && sheet.quality >= 5)
                    {
                        list.Add(sheet);
                    }
                }
            }
            return list;
        }

        public bool judgeSpecialEquip(ProtocolMgr protocol, ILogger logger, User user)
        {
            string url = "/root/equip!judgeSpecialEquip.action";
            string text = "淬火";
            ServerResult xml = protocol.getXml(url, text);
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            XmlNode node = xml.CmdResult.SelectSingleNode("/results/succ");
            if (node != null)
            {
                if (node.InnerText == "1")
                {
                    logInfo(logger, string.Format("{0}成功", text));
                }
                else
                {
                    logInfo(logger, string.Format("{0}失败", text));
                }
            }
            return true;
        }

        public bool makeSpecialEquip(ProtocolMgr protocol, ILogger logger, User user, SheetInfo sheetinfo)
        {
            string url = "/root/equip!makeSpecialEquip.action";
            string data = string.Format("specialId={0}", sheetinfo.id);
            ServerResult xml = protocol.postXml(url, data, "铸造");
            if (xml == null || !xml.CmdSucceed)
            {
                return false;
            }
            user._specialEquipSkillInfo.handle2(xml.CmdResult.SelectSingleNode("/results"));
            logInfo(logger, string.Format("铸造{0}lv.{1}, 消耗镔铁*{2}, {3}*{4}", sheetinfo.name, sheetinfo.lv, sheetinfo.material2num, sheetinfo.goodsname, sheetinfo.material1num));
            return true;
        }
        #endregion

        #region 新科技
        public long handleNewTech(ProtocolMgr protocol, ILogger logger, User user, int availablebintie, int consumebintie)
        {
            if (user._specialEquipSkillInfo.material2num <= availablebintie)
            {
                return next_hour();
            }
            bool action = false;
            List<Technology> list = getNewTech(protocol, logger);
            foreach (Technology item in list)
            {
                if (item.progress_ < item.requireprogress_ && item.consumebintie_ <= consumebintie)
                {
                    researchNewTech(protocol, logger, item);
                    action = true;
                }
            }
            if (action)
            {
                return immediate();
            }
            return next_hour();
        }

        public List<Technology> getNewTech(ProtocolMgr protocol, ILogger logger)
        {
            List<Technology> list = new List<Technology>();
            string url = "/root/tech!getNewTech.action";
            ServerResult xml = protocol.getXml(url, "新科技信息");
            if (xml != null && xml.CmdSucceed)
            {
                XmlNodeList xmlNodeList = xml.CmdResult.SelectNodes("/results/technology");
                foreach (XmlNode item in xmlNodeList)
                {
                    Technology tech = new Technology();
                    tech.handle(item);
                    if (tech.techid_ > 0)
                    {
                        list.Add(tech);
                    }
                }
            }
            return list;
        }

        public bool researchNewTech(ProtocolMgr protocol, ILogger logger, Technology tech)
        {
            bool result = false;
            string url = "/root/tech!researchNewTech.action";
            string data = string.Format("techId={0}", tech.techid_);
            ServerResult xml = protocol.postXml(url, data, "研究新科技");
            if (xml != null && xml.CmdSucceed)
            {
                result = true;
                AstdLuaObject lua = new AstdLuaObject();
                lua.ParseXml(xml.CmdResult.SelectSingleNode("/results"));
                int currentconsume = lua.GetIntValue("results.currentconsume");
                int addprogress = lua.GetIntValue("results.addprogress");
                int progress = lua.GetIntValue("results.progress");
                tech.progress_ = progress;
                logInfo(logger, string.Format("研究新科技[{0} lv.{1}], 消耗镔铁*{2}, 进度+{3}, 当前进度{4}/{5}", tech.techname_, tech.techlevel_, currentconsume, addprogress, tech.progress_, tech.requireprogress_));
            }
            return result;
        }
        #endregion

        #region 猴子套装
        /*
         * <results>
         *  <state>1</state>
         *  <taozhuang>
         *      <zhugenum>5</zhugenum>
         *      <toukuinum>5</toukuinum>
         *      <toukui>
         *          <have>1</have>
         *          <name>无双·百军令</name>
         *          <pic>baijunling6</pic>
         *          <intro>军令出，兵戈起，血漂橹，尸填海！</intro>
         *          <type>1</type>
         *      </toukui>
         *      <toukui>
         *          <have>1</have>
         *          <name>无双·万军行</name>
         *          <pic>wanjunxing6</pic>
         *          <intro>刀剑风中铮铮鸣，飚骑猎猎万军行！万军出，谁争锋？</intro>
         *          <type>2</type>
         *      </toukui>
         *      <toukui>
         *          <have>1</have>
         *          <name>无双·春秋史</name>
         *          <pic>chunqiushi6</pic>
         *          <intro>文王拘而演周易，仲尼厄而作春秋。</intro>
         *          <type>3</type>
         *      </toukui>
         *      <toukui>
         *          <have>1</have>
         *          <name>无双·杀破狼</name>
         *          <pic>shapolang6</pic>
         *          <intro>紫薇命格之一，有该命格者多为征战沙场的大将军。</intro>
         *          <type>4</type>
         *      </toukui>
         *      <toukui>
         *          <have>1</have>
         *          <name>无双·磐石盔</name>
         *          <pic>panshikui6</pic>
         *          <intro>磐石方且厚，千年当不朽。</intro>
         *          <type>5</type>
         *      </toukui>
         *      <tunum>5</tunum>
         *      <cantomonkey>3</cantomonkey>
         *      <elitemassacrenum>0,0,0,0,0</elitemassacrenum>
         *      <tucitydam>40</tucitydam>
         *      <maxtaozhuanglv>7</maxtaozhuanglv>
         *  </taozhuang>
         *  <playerequipdto>
         *      <equipname>传奇·大圣套装</equipname>
         *      <canmoli>0</canmoli>
         *      <generalname>王翦</generalname>
         *      <enchanting>0</enchanting>
         *      <bindstatus>0</bindstatus>
         *      <powerstr>0;0</powerstr>
         *      <attfull>100000</attfull>
         *      <deffull>50000</deffull>
         *      <cbtime>0</cbtime>
         *      <status>0</status>
         *      <reallevel>0</reallevel>
         *      <holelevel>0</holelevel>
         *      <holeenchanting>0</holeenchanting>
         *      <attr1>att=27936;def=19658;satt=52954;sdef=33434;stgatt=16584;stgdef=6000;hp=95926</attr1>
         *      <attr2>att=72040;def=48000;satt=115100;sdef=72020;stgatt=48000;stgdef=24000;hp=120000</attr2>
         *      <attr3>att=132500;def=95000;satt=195000;sdef=132500;stgatt=95000;stgdef=57500;hp=207500</attr3>
         *      <composite>908</composite>
         *      <leaadd>45</leaadd>
         *      <stradd>42</stradd>
         *      <intadd>37</intadd>
         *      <monkeylv>6</monkeylv>
         *      <lveffect>att=192000;def=128000;satt=320000;sdef=192000;stgatt=128000;stgdef=64000;hp=320000;alldame=0.06;allddame=0.06</lveffect>
         *      <lvfull>att=72000;def=48000;satt=120000;sdef=72000;stgatt=48000;stgdef=24000;hp=120000</lvfull>
         *      <canupgrade>0</canupgrade>
         *      <tickets>400000</tickets>
         *      <ticketsstatus>3</ticketsstatus>
         *      <freenum>0</freenum>
         *      <xuli>2</xuli>
         *      <maxxuli>3</maxxuli>
         *      <highnum>0</highnum>
         *  </playerequipdto>
         *  <playerequipdto>
         *      <equipname>史诗·大圣套装</equipname>
         *      <canmoli>0</canmoli>
         *      <generalname>荆轲</generalname>
         *      <enchanting>0</enchanting>
         *      <bindstatus>0</bindstatus>
         *      <powerstr>0;0</powerstr>
         *      <attfull>100000</attfull>
         *      <deffull>50000</deffull>
         *      <cbtime>0</cbtime>
         *      <status>0</status>
         *      <reallevel>0</reallevel>
         *      <holelevel>0</holelevel>
         *      <holeenchanting>0</holeenchanting>
         *      <attr1>att=27936;def=19658;satt=52954;sdef=33434;stgatt=16584;stgdef=6000;hp=95926</attr1>
         *      <attr3>att=132500;def=95000;satt=195000;sdef=132500;stgatt=95000;stgdef=57500;hp=207500</attr3>
         *      <composite>770</composite>
         *      <leaadd>45</leaadd>
         *      <stradd>36</stradd>
         *      <intadd>40</intadd>
         *      <monkeylv>7</monkeylv>
         *      <lveffect>att=264000;def=176000;satt=440000;sdef=264000;stgatt=176000;stgdef=88000;hp=440000;alldame=0.07;allddame=0.07</lveffect>
         *      <lvfull>att=96000;def=64000;satt=160000;sdef=96000;stgatt=64000;stgdef=32000;hp=160000</lvfull>
         *      <canupgrade>0</canupgrade>
         *      <tickets>200000</tickets>
         *      <ticketsstatus>1</ticketsstatus>
         *      <freenum>3</freenum>
         *      <xuli>2</xuli>
         *      <maxxuli>3</maxxuli>
         *      <highnum>3</highnum>
         *  </playerequipdto>
         *  <playerequipdto>
         *      <equipname>史诗·大圣套装</equipname>
         *      <canmoli>0</canmoli>
         *      <generalname>夏桀</generalname>
         *      <enchanting>0</enchanting>
         *      <bindstatus>0</bindstatus>
         *      <powerstr>0;0</powerstr>
         *      <attfull>100000</attfull>
         *      <deffull>50000</deffull>
         *      <cbtime>0</cbtime>
         *      <status>0</status>
         *      <reallevel>0</reallevel>
         *      <holelevel>0</holelevel>
         *      <holeenchanting>0</holeenchanting>
         *      <attr1>att=27936;def=19658;satt=52954;sdef=33434;stgatt=16584;stgdef=6000;hp=95926</attr1>
         *      <attr3>att=132500;def=95000;satt=195000;sdef=132500;stgatt=95000;stgdef=57500;hp=207500</attr3>
         *      <composite>771</composite>
         *      <leaadd>47</leaadd>
         *      <stradd>40</stradd>
         *      <intadd>39</intadd>
         *      <monkeylv>7</monkeylv>
         *      <lveffect>att=264000;def=176000;satt=440000;sdef=264000;stgatt=176000;stgdef=88000;hp=440000;alldame=0.07;allddame=0.07</lveffect>
         *      <lvfull>att=96000;def=64000;satt=160000;sdef=96000;stgatt=64000;stgdef=32000;hp=160000</lvfull>
         *      <canupgrade>0</canupgrade>
         *      <tickets>200000</tickets>
         *      <ticketsstatus>1</ticketsstatus>
         *      <freenum>3</freenum>
         *      <xuli>0</xuli>
         *      <maxxuli>3</maxxuli>
         *      <highnum>20</highnum>
         *  </playerequipdto>
         *  <playerequipdto>
         *      <equipname>史诗·大圣套装</equipname>
         *      <canmoli>0</canmoli>
         *      <generalname>秦始皇</generalname>
         *      <enchanting>0</enchanting>
         *      <bindstatus>0</bindstatus>
         *      <powerstr>0;0</powerstr>
         *      <attfull>100000</attfull>
         *      <deffull>50000</deffull>
         *      <cbtime>0</cbtime>
         *      <status>0</status>
         *      <reallevel>0</reallevel>
         *      <holelevel>0</holelevel>
         *      <holeenchanting>0</holeenchanting>
         *      <attr1>att=27936;def=19658;satt=52954;sdef=33434;stgatt=16584;stgdef=6000;hp=95926</attr1>
         *      <attr3>att=132500;def=95000;satt=195000;sdef=132500;stgatt=95000;stgdef=57500;hp=207500</attr3>
         *      <composite>769</composite>
         *      <leaadd>47</leaadd>
         *      <stradd>43</stradd>
         *      <intadd>40</intadd>
         *      <monkeylv>7</monkeylv>
         *      <lveffect>att=264000;def=176000;satt=440000;sdef=264000;stgatt=176000;stgdef=88000;hp=440000;alldame=0.07;allddame=0.07</lveffect>
         *      <lvfull>att=96000;def=64000;satt=160000;sdef=96000;stgatt=64000;stgdef=32000;hp=160000</lvfull>
         *      <canupgrade>0</canupgrade>
         *      <tickets>200000</tickets>
         *      <ticketsstatus>1</ticketsstatus>
         *      <freenum>3</freenum>
         *      <xuli>0</xuli>
         *      <maxxuli>3</maxxuli>
         *      <highnum>20</highnum>
         *  </playerequipdto>
         *  <playerequipdto>
         *      <equipname>史诗·大圣套装</equipname>
         *      <canmoli>0</canmoli>
         *      <generalname>姜子牙</generalname>
         *      <enchanting>0</enchanting>
         *      <bindstatus>0</bindstatus>
         *      <powerstr>0;0</powerstr>
         *      <attfull>100000</attfull>
         *      <deffull>50000</deffull>
         *      <cbtime>0</cbtime>
         *      <status>0</status>
         *      <reallevel>0</reallevel>
         *      <holelevel>0</holelevel>
         *      <holeenchanting>0</holeenchanting>
         *      <attr1>att=27936;def=19658;satt=52954;sdef=33434;stgatt=16584;stgdef=6000;hp=95926</attr1>
         *      <attr3>att=132500;def=95000;satt=195000;sdef=132500;stgatt=95000;stgdef=57500;hp=207500</attr3>
         *      <composite>846</composite>
         *      <leaadd>35</leaadd>
         *      <stradd>44</stradd>
         *      <intadd>44</intadd>
         *      <monkeylv>7</monkeylv>
         *      <lveffect>att=264000;def=176000;satt=440000;sdef=264000;stgatt=176000;stgdef=88000;hp=440000;alldame=0.07;allddame=0.07</lveffect>
         *      <lvfull>att=96000;def=64000;satt=160000;sdef=96000;stgatt=64000;stgdef=32000;hp=160000</lvfull>
         *      <canupgrade>0</canupgrade>
         *      <tickets>200000</tickets>
         *      <ticketsstatus>1</ticketsstatus>
         *      <freenum>3</freenum>
         *      <xuli>2</xuli>
         *      <maxxuli>3</maxxuli>
         *      <highnum>12</highnum>
         *  </playerequipdto>
         *  <ticketnumber>1892914385</ticketnumber>
         *  <officerlist>
         *      <officer><playerid>270431</playerid><playername>﹏遊龍天瀑</playername><pos>0</pos><nation>3</nation><playerlevel>438</playerlevel><ratio>46.00</ratio></officer>
         *      <officer><playerid>333140</playerid><playername>pong</playername><pos>1</pos><nation>3</nation><playerlevel>436</playerlevel><ratio>36.60</ratio></officer>
         *      <officer><playerid>120537</playerid><playername>烈长空</playername><pos>1</pos><nation>3</nation><playerlevel>430</playerlevel><ratio>36.00</ratio></officer>
         *      <officer><playerid>377699</playerid><playername>我靠你是猪</playername><pos>1</pos><nation>3</nation><playerlevel>430</playerlevel><ratio>36.00</ratio></officer>
         *      <officer><playerid>135576</playerid><playername>佩恩</playername><pos>1</pos><nation>3</nation><playerlevel>440</playerlevel><ratio>37.00</ratio></officer>
         *      <officer><playerid>277444</playerid><playername>卍丶乱世</playername><pos>2</pos><nation>3</nation><playerlevel>418</playerlevel><ratio>26.10</ratio></officer>
         *      <officer><playerid>318904</playerid><playername>宣战。风云</playername><pos>2</pos><nation>3</nation><playerlevel>410</playerlevel><ratio>25.50</ratio></officer>
         *      <officer><playerid>120666</playerid><playername>无双☆圣经</playername><pos>2</pos><nation>3</nation><playerlevel>422</playerlevel><ratio>26.40</ratio></officer>
         *      <officer><playerid>106793</playerid><playername>蓝黑军团</playername><pos>2</pos><nation>3</nation><playerlevel>414</playerlevel><ratio>25.80</ratio></officer>
         *      <officer><playerid>25244</playerid><playername>都梁王</playername><pos>2</pos><nation>3</nation><playerlevel>412</playerlevel><ratio>25.65</ratio></officer>
         *      <officer><playerid>273772</playerid><playername>江天一色</playername><pos>2</pos><nation>3</nation><playerlevel>424</playerlevel><ratio>26.55</ratio></officer>
         *      <officer><playerid>358911</playerid><playername>黯之利刃</playername><pos>2</pos><nation>3</nation><playerlevel>418</playerlevel><ratio>26.10</ratio></officer>
         *      <officer><playerid>294750</playerid><playername>权倾乄君临天下</playername><pos>2</pos><nation>3</nation><playerlevel>408</playerlevel><ratio>25.35</ratio></officer>
         *      <officer><playerid>219091</playerid><playername>龍皇至尊</playername><pos>2</pos><nation>3</nation><playerlevel>408</playerlevel><ratio>25.35</ratio></officer>
         *      <officer><playerid>277363</playerid><playername>无双★柳叶刀</playername><pos>2</pos><nation>3</nation><playerlevel>408</playerlevel><ratio>25.35</ratio></officer>
         *      <officer><playerid>292371</playerid><playername>风飞乐</playername><pos>2</pos><nation>3</nation><playerlevel>394</playerlevel><ratio>24.30</ratio></officer>
         *      <officer><playerid>96133</playerid><playername>無聊的人</playername><pos>2</pos><nation>3</nation><playerlevel>414</playerlevel><ratio>25.80</ratio></officer>
         *      <officer><playerid>25813</playerid><playername>大卫</playername><pos>3</pos><nation>3</nation><playerlevel>396</playerlevel><ratio>16.30</ratio></officer>
         *      <officer><playerid>200494</playerid><playername>真英雄</playername><pos>3</pos><nation>3</nation><playerlevel>408</playerlevel><ratio>16.90</ratio></officer>
         *      <officer><playerid>225182</playerid><playername>柠乐乐</playername><pos>3</pos><nation>3</nation><playerlevel>380</playerlevel><ratio>15.50</ratio></officer>
         *      <officer><playerid>22550</playerid><playername>小将</playername><pos>3</pos><nation>3</nation><playerlevel>400</playerlevel><ratio>16.50</ratio></officer>
         *      <officer><playerid>209458</playerid><playername>水中月</playername><pos>3</pos><nation>3</nation><playerlevel>402</playerlevel><ratio>16.60</ratio></officer>
         *      <officer><playerid>358820</playerid><playername>红尘ャ诸葛曹操</playername><pos>3</pos><nation>3</nation><playerlevel>394</playerlevel><ratio>16.20</ratio></officer>
         *      <officer><playerid>344041</playerid><playername>莫追燕</playername><pos>3</pos><nation>3</nation><playerlevel>398</playerlevel><ratio>16.40</ratio></officer>
         *      <officer><playerid>378576</playerid><playername>渴望传奇</playername><pos>3</pos><nation>3</nation><playerlevel>408</playerlevel><ratio>16.90</ratio></officer>
         *      <officer><playerid>378459</playerid><playername>上穷碧落</playername><pos>3</pos><nation>3</nation><playerlevel>408</playerlevel><ratio>16.90</ratio></officer>
         *      <officer><playerid>61818</playerid><playername>太上老君</playername><pos>3</pos><nation>3</nation><playerlevel>388</playerlevel><ratio>15.90</ratio></officer>
         *      <officer><playerid>282112</playerid><playername>﹏焦嚸★瑝镞三哥</playername><pos>3</pos><nation>3</nation><playerlevel>400</playerlevel><ratio>16.50</ratio></officer>
         *      <officer><playerid>75087</playerid><playername>和平年代</playername><pos>3</pos><nation>3</nation><playerlevel>386</playerlevel><ratio>15.80</ratio></officer>
         *      <officer><playerid>199591</playerid><playername>无双★噁貫懑盈</playername><pos>3</pos><nation>3</nation><playerlevel>354</playerlevel><ratio>14.20</ratio></officer>
         *      <officer><playerid>82468</playerid><playername>龍魂</playername><pos>3</pos><nation>3</nation><playerlevel>406</playerlevel><ratio>16.80</ratio></officer>
         *      <officer><playerid>84358</playerid><playername>赤发鬼刘唐</playername><pos>3</pos><nation>3</nation><playerlevel>406</playerlevel><ratio>16.80</ratio></officer>
         *      <officer><playerid>81906</playerid><playername>意绵绵静日玉生香</playername><pos>3</pos><nation>3</nation><playerlevel>386</playerlevel><ratio>15.80</ratio></officer>
         *      <officer><playerid>122149</playerid><playername>灰诚勿鸟</playername><pos>3</pos><nation>3</nation><playerlevel>384</playerlevel><ratio>15.70</ratio></officer>
         *      <officer><playerid>282200</playerid><playername>天下无我</playername><pos>3</pos><nation>3</nation><playerlevel>406</playerlevel><ratio>16.80</ratio></officer>
         *      <officer><playerid>17094</playerid><playername>狮子王</playername><pos>3</pos><nation>3</nation><playerlevel>394</playerlevel><ratio>16.20</ratio></officer>
         *      <officer><playerid>226494</playerid><playername>咔咔几下</playername><pos>3</pos><nation>3</nation><playerlevel>400</playerlevel><ratio>16.50</ratio></officer>
         *      <officer><playerid>378542</playerid><playername>主宰メ佑手</playername><pos>3</pos><nation>3</nation><playerlevel>402</playerlevel><ratio>16.60</ratio></officer>
         *      <officer><playerid>10684</playerid><playername>幻影</playername><pos>3</pos><nation>3</nation><playerlevel>400</playerlevel><ratio>16.50</ratio></officer>
         *      <officer><playerid>292397</playerid><playername>风飞乐6</playername><pos>3</pos><nation>3</nation><playerlevel>394</playerlevel><ratio>16.20</ratio></officer>
         *      <officer><playerid>271797</playerid><playername>乱世的英雄</playername><pos>3</pos><nation>3</nation><playerlevel>384</playerlevel><ratio>15.70</ratio></officer>
         *  </officerlist>
         *  <hasking>1</hasking>
         *  <achievement><achid>129</achid><name>灵猴暴击</name><intro>猴套装强化有几率2倍暴击</intro><pic>s126</pic></achievement>
         *  <achievement><achid>139</achid><name>灵猴进阶·二</name><intro>猴套装开放7级</intro><pic>s136</pic></achievement>
         *  <achievement><achid>14</achid><name>天铸神兵</name><intro>给诸葛套装打孔，可镶嵌第二颗宝石</intro><pic>s10</pic></achievement>
         *  <achievement><achid>127</achid><name>免费强化</name><intro>每件猴套装每天可免费强化3次</intro><pic>s124</pic></achievement>
         *  <achievement><achid>3</achid><name>名匠传说</name><intro>强化装备暴击几率提升(未达到vip3也能获得)</intro><pic>sa2</pic></achievement>
         *  <achievement><achid>117</achid><name>积少成多·五</name><intro>头盔每天免费强化5次(次日生效)</intro><pic>s114</pic></achievement>
         *  <card><cardname>强化打折卡</cardname><cardtype>3</cardtype><cardintro>使用后下次强化所需银币减少20%，最多同时拥有5张。</cardintro><effect>equipcopper:0.8</effect><maxnum>5</maxnum><cardnum>0</cardnum><active>0</active></card>
         *  <card><cardname>强化暴击卡</cardname><cardtype>2</cardtype><cardintro>使用后下次强化成功必定暴击，最多同时拥有5张。</cardintro><effect>equipcrit:2</effect><maxnum>5</maxnum><cardnum>0</cardnum><active>0</active></card>
         *  <xilian>
         *      <xilian>2</xilian>
         *      <colorstate>0</colorstate>
         *      <baoshixilian>10</baoshixilian>
         *      <maxlv>5</maxlv>
         *  </xilian>
         *  <magic>97</magic>
         *  <holecopper>3000000</holecopper>
         *  <magicstate>1</magicstate>
         *  <cd>-1</cd>
         *  <cdflag>0</cdflag>
         *  <canusegold>0</canusegold>
         *  <cancrip>1</cancrip>
         *  <cripconsumelv>4</cripconsumelv>
         *  <isautoass>true</isautoass>
         *  <limitlv>300</limitlv>
         *  <canactive>1</canactive>
         * </results>
         */
        //public int getUpgradeInfo(ProtocolMgr protocol, ILogger logger, User user)
        //{
        //    string url = "/root/equip!getUpgradeInfo.action";
        //    ServerResult
        //    return 0;
        //}
        #endregion
    }
}
