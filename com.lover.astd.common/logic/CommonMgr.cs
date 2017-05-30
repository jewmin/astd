using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.manager;
using System.Drawing;
using com.lover.astd.common.model;
using System.Xml;
using System.Data;
using System.IO;
using com.lover.astd.common.model.misc;

namespace com.lover.astd.common.logic
{
    public class CommonMgr : MgrBase
    {
        Dictionary<string, string> equipaddinfo = new Dictionary<string, string>();
        Dictionary<string, string> equipattr = new Dictionary<string, string>();

        public CommonMgr(TimeMgr tmrMgr, ServiceFactory factory)
        {
            this._logColor = Color.SlateBlue;
            this._tmrMgr = tmrMgr;
            this._factory = factory;
            InitValue();
        }

        private void InitValue()
        {
            equipaddinfo.Add("att", "普攻");
            equipaddinfo.Add("def", "普防");
            equipaddinfo.Add("satt", "战攻");
            equipaddinfo.Add("sdef", "战防");
            equipaddinfo.Add("stgatt", "策攻");
            equipaddinfo.Add("stgdef", "策防");
            equipaddinfo.Add("hp", "兵力");

            equipattr.Add("1", "统");
            equipattr.Add("2", "勇");
            equipattr.Add("3", "智");
        }

        private int GetValue(XmlDocument doc, string xpath, int def_value)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node != null)
            {
                int value;
                if (int.TryParse(node.InnerText, out value))
                    return value;
            }
            return def_value;
        }

        private long GetLongValue(XmlDocument doc, string xpath, long def_value)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node != null)
            {
                long value;
                if (long.TryParse(node.InnerText, out value))
                    return value;
            }
            return def_value; 
        }

        private string GetStringValue(XmlDocument doc, string xpath, string def_value)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node != null) return node.InnerText;
            return def_value;
        }

        private DataTable GetValue(XmlDocument doc, string xpath, string table_name)
        {
            DataTable dt = new DataTable(table_name);
            XmlNode node = doc.SelectSingleNode(string.Format("{0}[1]", xpath));
            if (node != null)
            {
                string column;
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    column = node.ChildNodes.Item(i).Name;
                    dt.Columns.Add(column);
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            string xml_string = doc.InnerXml;
            XmlTextReader xr = new XmlTextReader(new StringReader(xml_string));
            ds.ReadXml(xr);

            return dt;
        }

        #region 屠城嘉奖
        public void world_getTuCityInfo(ProtocolMgr proto, ILogger logger)
        {
            string url = "/root/world!getTuCityInfo.action";
            ServerResult xml = proto.getXml(url, "屠城嘉奖");
            if (xml == null || !xml.CmdSucceed) return;

            int maxrecvednum = GetValue(xml.CmdResult, "/results/maxrecvednum", 0);
            int recvednum = GetValue(xml.CmdResult, "/results/recvednum", 0);
            if (recvednum >= maxrecvednum) return;

            DataTable dt = GetValue(xml.CmdResult, "/results/info", "info");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!world_getTuCityReward(proto, logger, Convert.ToInt32(dt.Rows[i]["playerid"]), Convert.ToInt32(dt.Rows[i]["areaid"])))
                        return;
                }
            }
        }

        public bool world_getTuCityReward(ProtocolMgr proto, ILogger logger, int playerId, int areaId)
        {
            string url = "/root/world!getTuCityReward.action";
            string data = string.Format("playerId={0}&areaId={1}", playerId, areaId);
            ServerResult xml = proto.postXml(url, data, "屠城嘉奖-搜刮");
            if (xml == null || !xml.CmdSucceed) return false;

            RewardInfo info = new RewardInfo();
            info.handleXmlNode(xml.CmdResult.SelectSingleNode("/results/rewardinfo"));
            logInfo(logger, string.Format("屠城嘉奖, 获得 %s !", info.ToString()));
            return true;
        }
        #endregion

        #region 猴子套装
        public long equip_handleMonkeyTao(ProtocolMgr proto, ILogger logger, long leave_tickets, int use_tickets)
        {
            long ticketnumber = 0;
            int maxtaozhuanglv = 0;
            DataTable dt = null;
            int result = equip_getUpgradeInfo(proto, logger, ref ticketnumber, ref maxtaozhuanglv, ref dt);
            if (result != 0) return next_halfhour();

            if (ticketnumber < leave_tickets)
                logInfo(logger, string.Format("当前拥有点券 {0}, 保留点券 {1}", ticketnumber, leave_tickets));

            if (dt != null && dt.Rows.Count > 0)
            {
                bool upgraded = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string name = string.Format("{0}({1})", dr["equipname"], dr["generalname"]);

                    if (Convert.ToInt32(dr["xuli"]) >= Convert.ToInt32(dr["maxxuli"]))
                        equip_useXuli(proto, logger, name, Convert.ToInt32(dr["composite"]));

                    if (Convert.ToInt32(dr["monkeylv"]) >= maxtaozhuanglv)
                    {
                        logInfo(logger, string.Format("{0} 已升级到顶级!", name));
                        continue;
                    }

                    if (Convert.ToInt32(dr["tickets"]) <= use_tickets)
                    {
                        equip_upgradeMonkeyTao(proto, logger, name, Convert.ToInt32(dr["composite"]), 0);
                        upgraded = true;
                    }
                }
                if (upgraded) return immediate();
            }

            return next_hour();
        }

        public int equip_getUpgradeInfo(ProtocolMgr proto, ILogger logger, ref long ticketnumber, ref int maxtaozhuanglv, ref DataTable dt)
        {
            string url = "/root/equip!getUpgradeInfo.action";
            ServerResult xml = proto.getXml(url, "强化");
            if (xml == null || !xml.CmdSucceed) return 10;

            ticketnumber = GetLongValue(xml.CmdResult, "/results/ticketnumber", 0);
            maxtaozhuanglv = GetValue(xml.CmdResult, "/results/taozhuang/maxtaozhuanglv", 0);
            dt = GetValue(xml.CmdResult, "/results/playerequipdto", "playerequipdto");
            return 0;
        }

        public int equip_upgradeMonkeyTao(ProtocolMgr proto, ILogger logger, string name, int composite, int num)
        {
            string url = "/root/equip!upgradeMonkeyTao.action";
            string data = string.Format("composite={0}&num={1}", composite, num);
            ServerResult xml = proto.postXml(url, data, "猴子套装-强化");
            if (xml == null || !xml.CmdSucceed) return 10;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("强化 {0} 成功", name);
            int baoji = GetValue(xml.CmdResult, "/results/baoji", 0);
            if (baoji > 0) sb.AppendFormat(", {0}倍暴击", baoji);
            //DataTable dt = GetValue(xml.CmdResult, "/results/addinfo", "addinfo");
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        sb.AppendFormat(", {0}+{1}", equipaddinfo[Convert.ToString(dt.Rows[i]["name"])], dt.Rows[i]["val"]);
            //    }
            //}
            logInfo(logger, sb.ToString());
            return 0;
        }

        public int equip_useXuli(ProtocolMgr proto, ILogger logger, string name, int composite)
        {
            string url = "/root/equip!useXuli.action";
            string data = string.Format("composite={0}", composite);
            ServerResult xml = proto.postXml(url, data, "猴子套装-蓄力");
            if (xml == null || !xml.CmdSucceed) return 10;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}蓄力", name);
            //int getevent = GetValue(xml.CmdResult, "/results/xuliinfo/getevent", 0);
            //switch (getevent)
            //{
            //    case 1:
            //        {
            //            int gethighnum = GetValue(xml.CmdResult, "/results/xuliinfo/gethighnum", 0);
            //            sb.AppendFormat(", 普通强化次数+{0}", gethighnum);
            //        }
            //        break;

            //    case 2:
            //        {
            //            DataTable dt = GetValue(xml.CmdResult, "/results/xuliinfo/addinfo", "xuliinfo");
            //            if (dt.Rows.Count > 0)
            //            {
            //                for (int i = 0; i < dt.Rows.Count; i++)
            //                {
            //                    sb.AppendFormat(", {0}+{1}", equipaddinfo[Convert.ToString(dt.Rows[i]["name"])], dt.Rows[i]["val"]);
            //                }
            //            }
            //        }
            //        break;

            //    case 3:
            //        {
            //            string newattr = GetStringValue(xml.CmdResult, "/results/xuliinfo/newattr", "");
            //            sb.AppendFormat(", {0}", newattr);
            //        }
            //        break;
            //}
            logInfo(logger, sb.ToString());
            return 0;
        }
        #endregion
    }
}
