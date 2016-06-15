using com.lover.astd.common.config;
using com.lover.astd.common.manager;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

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

        public int handlePolishInfo(ProtocolMgr protocol, ILogger logger, int gold_available, int magic_now, int reserve_count, int reserve_item_count, int gold_merge_attrib, int melt_failcount)
        {
            string url = "/root/polish!getBaowuPolishInfo.action";
            ServerResult xml = protocol.getXml(url, "获取炼化信息");
            if (xml == null)
            {
                return 1;
            }
            if (!xml.CmdSucceed)
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
                }
                if (decoration.polishtimes < 10)
                {
                    list.Add(decoration);
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
                    result = 2;
                }
                else
                {
                    XmlDocument cmdResult = serverResult.CmdResult;
                    string[] array = new string[]
					{
						"白色",
						"蓝色",
						"绿色",
						"黄色",
						"红色",
						"紫色"
					};
                    int num = 0;
                    int num2;
                    for (int i = 0; i < array.Length; i = num2 + 1)
                    {
                        bool flag3 = array[i].Equals(sell_quality);
                        if (flag3)
                        {
                            num = i + 1;
                            break;
                        }
                        num2 = i;
                    }
                    string tradeSN = "";
                    string text = "";
                    int num3 = 0;
                    int num4 = 0;
                    bool flag4 = false;
                    XmlNode xmlNode = cmdResult.SelectSingleNode("/results/tradesn");
                    bool flag5 = xmlNode != null;
                    if (flag5)
                    {
                        tradeSN = xmlNode.InnerText;
                    }
                    XmlNode xmlNode2 = cmdResult.SelectSingleNode("/results/merchandise");
                    XmlNodeList childNodes = xmlNode2.ChildNodes;
                    foreach (XmlNode xmlNode3 in childNodes)
                    {
                        bool flag6 = xmlNode3.Name == "merchandisename";
                        if (flag6)
                        {
                            text = xmlNode3.InnerText;
                        }
                        else
                        {
                            bool flag7 = xmlNode3.Name == "merchandisequality";
                            if (flag7)
                            {
                                num3 = int.Parse(xmlNode3.InnerText);
                            }
                            else
                            {
                                bool flag8 = xmlNode3.Name == "cost";
                                if (flag8)
                                {
                                    num4 = int.Parse(xmlNode3.InnerText);
                                }
                                else
                                {
                                    bool flag9 = xmlNode3.Name == "chip";
                                    if (flag9)
                                    {
                                        flag4 = true;
                                    }
                                }
                            }
                        }
                    }
                    string text2 = string.Format("委派[{0}], 花费{1}银币, 获得[{2}{3}], 售价{4}银币", new object[]
					{
						merchant.merchantname,
						merchant.cost,
						text,
						flag4 ? "碎片" : "",
						num4
					});
                    base.logInfo(logger, text2);
                    bool flag10 = num3 <= num;
                    if (flag10)
                    {
                        this.sellMerchantGoods(protocol, logger, tradeSN, text);
                    }
                    result = 0;
                }
            }
            return result;
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
                    if (user.Stone >= warChariot.Needbowlder)
                    {
                        if (warChariot.Needtofull > 0 && warChariot.Equipitemnum >= warChariot.Needequipitem && (warChariot.Chuizi > 0 || notChuizi))
                        {
                            if (upgradeWarChariot(protocol, logger, warChariot.Chuizi))
                            {
                                return 0;
                            }
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
    }
}
