using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using com.lover.common;
using System.Xml;
using System.IO;
using System.Timers;

namespace com.lover.astd.game.ui
{
    public partial class NewReportViewer : Form
    {
        private class Buff
        {
            public List<string> list;

            public Buff()
            {
                list = new List<string>();
            }

            public Buff Clone()
            {
                Buff buff = new Buff();
                foreach(string item in list)
                {
                    buff.list.Add(item);
                }
                return buff;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                foreach (string item in list)
                {
                    sb.AppendFormat("{0}  ", item);
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 军队信息
        /// </summary>
        private class ArmyInfo
        {
            /// <summary>
            /// 武将
            /// </summary>
            public string name;
            /// <summary>
            /// 等级
            /// </summary>
            public int level;
            /// <summary>
            /// 兵力
            /// </summary>
            public int armyNow;
            /// <summary>
            /// 最大兵力
            /// </summary>
            public int armyMax;
            /// <summary>
            /// 士气
            /// </summary>
            public int moracle;
            /// <summary>
            /// 士气状态
            /// </summary>
            public string moracle_status;
            /// <summary>
            /// 状态
            /// </summary>
            public string status;
            /// <summary>
            /// 目标
            /// </summary>
            public int action_target;
            /// <summary>
            /// buff
            /// </summary>
            public Buff buff;

            public ArmyInfo()
            {
                name = "";
                level = 0;
                armyNow = 0;
                armyMax = 0;
                moracle = 0;
                status = "";
                action_target = 0;
                buff = new Buff();
            }

            public ArmyInfo Clone()
            {
                return new ArmyInfo
                {
                    name = this.name,
                    level = this.level,
                    armyNow = this.armyNow,
                    armyMax = this.armyMax,
                    moracle = this.moracle,
                    moracle_status = this.moracle_status,
                    status = this.status,
                    action_target = this.action_target,
                    buff = this.buff.Clone()
                };
            }
        }

        private string _rawText = "";

        private List<List<ArmyInfo>> _steps = new List<List<ArmyInfo>>();

        private int _currentStep;

        private string _gameUrl = "";

        private System.Timers.Timer _timer;

        delegate void NextCallBack();

        public void setReport(string url)
        {
            txt_url.Text = url;
            openUrl(url);
        }

        private void openUrl(string url)
        {
            if (url.IndexOf("?fid=") >= 0)
            {
                url = url.Replace("?fid=", "root/fight.action?id=");
            }
            CookieContainer cookieContainer = new CookieContainer(10000, 1000, 409600);
            if (web_view != null && web_view.Document != null)
            {
                string cookie = web_view.Document.Cookie;
                if (cookie != null)
                {
                    cookie = cookie.Replace(';', ',');
                    cookieContainer.SetCookies(new Uri(txt_url.Text), cookie);
                }
            }
            List<Cookie> list = new List<Cookie>();
            foreach (Cookie item in cookieContainer.GetCookies(new Uri(url)))
            {
                list.Add(item);
            }
            _rawText = TransferMgr.doGet(url, ref list);
            initContents();
        }

        private void initContents()
        {
            if (_rawText == null || _rawText == "")
            {
                UiUtils.getInstance().info("打开失败, 请检查是否有误");
                return;
            }
            _currentStep = 0;
            _steps.Clear();
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(_rawText);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("/results/battlereport/report");
                if (xmlNode == null)
                {
                    UiUtils.getInstance().info("您所查看的战报不存在或已经过期");
                }
                else
                {
                    string[] report_info = xmlNode.InnerText.Split(new char[] { ';' });
                    int report_step = report_info.Length - 2;
                    List<ArmyInfo> army_list = new List<ArmyInfo>();
                    //Gong|鱼鳞阵|2|0|0|0|0|0|典韦|314|1001417|812436|812436|张飞|314|1419|811867|811867|荆轲|314|1420|787786|787786|0|0|0|0|0|秦始皇|314|5415|811691|811691|鬼谷子|314|4420|809436|809436|0|0|0|0|0|0|0|0|0|0;Fang|鱼鳞阵|2|鬼谷子|314|4420|783325|783325|0|0|0|0|0|典韦|314|1001417|803380|803380|张飞|314|1419|810836|810836|0|0|0|0|0|秦始皇|314|5415|809740|809740|0|0|0|0|0|荆轲|314|1420|806860|806860|0|0|0|0|0;Init|12|士气|100|13|士气|100|14|士气|100|23|士气|100|24|士气|100|28|士气|100;ZhancheInit|18|2;ZhancheInit|27|2;战车|18|18|战车攻击2|21|伤害|96000|21|削弱|0.33|23|失败|0|24|伤害|96000|24|削弱|0.33|26|伤害|96000|26|削弱|0.33|28|伤害|96000|28|削弱|0.33;战车|27|27|战车攻击2|12|失败|0|13|伤害|94000|13|削弱|0.33|14|伤害|94000|14|削弱|0.33|16|伤害|94000|16|削弱|0.33|17|伤害|94000|17|削弱|0.33;战法|12|28|118|12|士兵|0|12|士气|100;状态|21|削弱|消失;攻击|21|14|荆棘|14|伤害|162074;状态|13|削弱|消失;状态|13|暴击|0;战法|13|23|123|26|伤害|178623|23|伤害|78095|13|士气|100;战法|23|13|118|23|士兵|78095|23|士气|100;状态|14|削弱|消失;战法|14|21|125|21|伤害|21143|14|士气|100;状态|24|削弱|消失;战法|24|14|123|14|回避|0|24|士气|100;状态|16|削弱|消失;攻击|16|23|暴击|21|伤害|220140|28|伤害|314995|23|伤害|123093|23|士气|34;状态|26|削弱|消失;攻击|26|13|普通|14|伤害|163742|12|伤害|80225|13|伤害|145146|13|士气|34;状态|17|削弱|消失;攻击|17|21|荆棘|21|伤害|125428;状态|28|削弱|消失;状态|28|暴击|0;战法|28|12|125|12|伤害|26331|28|士气|100;战法|12|28|118|12|士兵|106556|12|士气|100;攻击|21|14|荆棘|14|伤害|319361;战法|13|23|123|26|伤害|206920|26|混乱|产生|23|伤害|91677|13|士气|100;战法|23|13|118|23|士兵|214770|23|士气|100;战法|14|21|125|21|伤害|20356|14|士气|100;战法|24|14|123|17|伤害|130801|14|伤害|48609|24|士气|100;攻击|16|23|普通|23|抵挡|0|16|伤害|98618;状态|26|混乱|消失;攻击|26|13|普通|17|伤害|189221|12|伤害|99977|13|伤害|180175|13|士气|34;攻击|17|21|荆棘|21|伤害|259398;战法|28|12|125|12|伤害|31686|28|士气|100;状态|12|暴击|0;战法|12|28|118|12|士兵|131663|12|士气|100;攻击|21|17|荆棘|17|伤害|207340;战法|13|23|123|26|伤害|194622|23|伤害|83507|13|士气|100;战法|23|13|118|23|士兵|83507|23|士气|100;状态|14|复活|出现|14|士兵|787786|14|士气|100;状态|14|暴击|0;战法|14|21|125|21|伤害|40860|14|士气|100;战法|24|14|123|17|伤害|132820|14|伤害|311627|24|士气|100;攻击|16|23|暴击|24|伤害|360411|28|伤害|395865|23|伤害|181464|23|士气|34;攻击|26|13|普通|14|伤害|180373|12|伤害|85810|13|伤害|158670|13|士气|34;攻击|17|24|荆棘|24|失败|0;状态|28|复活|出现|28|士兵|806860|28|士气|100;状态|28|暴击|0;战法|28|12|125|12|伤害|53156|28|士气|100;战法|12|28|118|28|回避|0|12|士气|100;战法|23|13|118|23|士兵|181464|23|士气|100;战法|13|23|123|26|伤害|133575|23|伤害|74173|13|士气|100;战法|24|14|123|17|伤害|55254|14|伤害|231387|24|士气|100;状态|14|暴击|0;战法|14|24|125|24|伤害|54869|14|士气|100;战法|28|12|125|12|伤害|42544|28|士气|100;攻击|16|23|普通|23|抵挡|0|16|伤害|97625;战法|12|28|118|12|士兵|181510|12|士气|100;战法|23|13|118|23|士兵|74173|23|士气|100;战法|13|23|123|23|伤害|65018|23|混乱|产生|13|士气|100;战法|24|14|123|14|伤害|64399|14|混乱|产生|24|士气|100;攻击|16|23|暴击|24|伤害|299556|28|伤害|436301|23|伤害|165399|23|士气|34;战法|28|12|125|12|伤害|32043|28|士气|100;战法|12|28|118|12|士兵|32043|12|士气|100;状态|23|混乱|消失;战法|13|23|123|23|伤害|67839|13|士气|100;战法|28|12|125|12|伤害|32241|28|士气|100;攻击|16|23|普通|28|伤害|324719|23|伤害|136384|23|士气|34;战法|12|28|118|28|回避|0|12|士气|100;战法|23|13|118|23|士兵|238679|23|士气|100;状态|13|暴击|0;战法|13|23|123|23|伤害|91515|13|士气|100;状态|28|暴击|0;战法|28|12|125|12|伤害|31741|28|士气|100;攻击|16|23|普通|28|伤害|45840|23|伤害|134789|23|士气|34;战法|12|23|118|12|士兵|63982|12|士气|100;战法|23|13|118|23|士兵|225446|23|士气|100;战法|13|23|123|23|伤害|70532|23|混乱|产生|13|士气|100;攻击|16|23|普通|23|伤害|137396|23|士气|34;战法|12|23|118|12|士兵|0|12|士气|100;状态|23|混乱|消失;战法|13|23|123|23|伤害|75101|13|士气|100;攻击|16|23|普通|23|伤害|146901|23|士气|34;战法|12|23|118|12|士兵|0|12|士气|100;战法|23|13|118|23|士兵|223026|23|士气|100;状态|13|暴击|0;战法|13|23|123|23|伤害|101510|13|士气|100;攻击|16|23|普通|23|伤害|154826|23|士气|34;战法|12|23|118|12|士兵|0|12|士气|100;战法|23|13|118|23|士兵|193643|23|士气|100;状态|13|暴击|0;战法|13|23|123|23|伤害|105338|13|士气|100;攻击|16|23|普通|23|伤害|164974|23|士气|34;战法|12|23|118|12|士兵|0|12|士气|100;战法|23|13|118|23|士兵|192511|23|士气|100;战法|13|23|123|23|伤害|83202|23|混乱|产生|13|士气|100;攻击|16|23|普通|23|伤害|164160|23|士气|34;战法|12|23|118|12|士兵|0|12|士气|100;状态|23|混乱|消失;战法|13|23|123|23|伤害|11801|13|士气|100;
                    string[] gong = report_info[0].Split(new char[] { '|' });
                    for (int i = 3; i < gong.Length; i = i + 5)
                    {
                        ArmyInfo armyInfo = new ArmyInfo();
                        armyInfo.name = gong[i];
                        armyInfo.level = int.Parse(gong[i + 1]);
                        armyInfo.armyNow = int.Parse(gong[i + 3]);
                        armyInfo.armyMax = int.Parse(gong[i + 4]);
                        army_list.Add(armyInfo);
                    }
                    //Fang|鱼鳞阵|2|太史慈|238|3413|129004|129004|0|0|0|0|0|0|0|0|0|0|项羽|238|1415|129201|129201|赵云|175|2417|122974|122974|0|0|0|0|0|0|0|0|0|0|吕布|238|2415|128442|128442|张良|240|4414|127435|127435
                    string[] fang = report_info[1].Split(new char[] { '|' });
                    for (int i = 3; i < fang.Length; i = i + 5)
                    {
                        ArmyInfo armyInfo = new ArmyInfo();
                        armyInfo.name = fang[i];
                        armyInfo.level = int.Parse(fang[i + 1]);
                        armyInfo.armyNow = int.Parse(fang[i + 3]);
                        armyInfo.armyMax = int.Parse(fang[i + 4]);
                        army_list.Add(armyInfo);
                    }
                    string attZhanqi = "";
                    string defZhanqi = "";
                    for (int i = 0; i < report_step; i++)
                    {
                        string report = report_info[i + 2];
                        if (report == "")
                        {
                            break;
                        }
                        for (int j = 0; j < 18; j++)
                        {
                            army_list[j].status = "";
                            army_list[j].moracle_status = "";
                            army_list[j].action_target = 0;
                        }
                        string[] report_content = report.Split(new char[] { '|' });
                        string report_type = report_content[0];
                        ArmyInfo armyInfo = null;
                        int position = 0;
                        if (report_type == "Init")
                        {
                            //Init|13|士气|100|14|士气|100|18|士气|100|21|士气|100|23|士气|100|24|士气|100
                            for (int j = 1; j < report_content.Length; j = j + 3)
                            {
                                position = int.Parse(report_content[j]);
                                armyInfo = army_list[getArmyIndex(position)];
                                handleReport(armyInfo, report_content[j + 1], report_content[j + 2]);
                            }
                        }
                        else if (report_type == "attZhanqi")
                        {
                            //attZhanqi|参将旗,140000,5
                            string[] zhanqi = report_content[1].Split(new char[] { ',' });
                            attZhanqi = string.Format("{0}Lv{1}", zhanqi[0], zhanqi[2]);
                            continue;
                        }
                        else if (report_type == "defZhanqi")
                        {
                            string[] zhanqi = report_content[1].Split(new char[] { ',' });
                            defZhanqi = string.Format("{0}Lv{1}", zhanqi[0], zhanqi[2]);
                            continue;
                        }
                        else if (report_type == "ZhancheInit")
                        {
                            //ZhancheInit|17|2
                            //1:破阵战车,2:霹雳战车,3:神武战车
                            position = int.Parse(report_content[1]);
                            armyInfo = army_list[getArmyIndex(position)];
                            if (report_content[2] == "1")
                            {
                                armyInfo.name = "破阵战车";
                            }
                            else if (report_content[2] == "2")
                            {
                                armyInfo.name = "霹雳战车";
                            }
                            else if (report_content[2] == "3")
                            {
                                armyInfo.name = "神武战车";
                            }
                            else
                            {
                                armyInfo.name = report_content[2];
                            }
                            continue;
                        }
                        else if (report_type == "战车")
                        {
                            //战车|17|17|战车攻击2|21|伤害|96000|21|削弱|0.33|23|伤害|96000|23|削弱|0.33|24|伤害|96000|24|削弱|0.33|26|伤害|96000|26|削弱|0.33|28|伤害|96000|28|削弱|0.33
                            position = int.Parse(report_content[1]);
                            armyInfo = army_list[getArmyIndex(position)];
                            armyInfo.action_target = 1;
                            armyInfo.status += string.Format("{0}  ", report_content[3]);
                            for (int j = 4; j < report_content.Length; j = j + 3)
                            {
                                position = int.Parse(report_content[j]);
                                armyInfo = army_list[getArmyIndex(position)];
                                if (armyInfo.action_target == 0)
                                {
                                    armyInfo.action_target = 2;
                                }
                                handleReport(armyInfo, report_content[j + 1], report_content[j + 2]);
                            }
                        }
                        else if (report_type == "状态")
                        {
                            //状态|11|削弱|消失
                            //状态|21|复活|出现|21|士兵|678787|21|士气|100
                            //状态|14|混乱|消失
                            //状态|14|暴击|0
                            for (int j = 1; j < report_content.Length; j = j + 3)
                            {
                                position = int.Parse(report_content[j]);
                                armyInfo = army_list[getArmyIndex(position)];
                                armyInfo.action_target = 1;
                                handleReport(armyInfo, report_content[j + 1], report_content[j + 2]);
                            }
                        }
                        else if (report_type == "战法")
                        {
                            //战法|21|11|125|11|伤害|19891|21|士气|100
                            //战法|13|23|118|13|士兵|90000|13|士气|100
                            //战法|14|21|123|24|伤害|179266|21|伤害|185687|14|士气|100
                            //战法|24|11|123|14|伤害|51706|11|伤害|27595|24|士气|100
                            //战法|24|11|123|14|伤害|53464|14|混乱|产生|11|伤害|28912|24|士气|100
                            //战法|14|21|9|21|伤害|1396|14|坚守|产生
                            //战法|11|21|128|激光|21|24
                            position = int.Parse(report_content[1]);
                            armyInfo = army_list[getArmyIndex(position)];
                            armyInfo.action_target = 1;
                            armyInfo.status += string.Format("{0}  ", hanldeTactics(report_content[3]));
                            armyInfo.moracle = 0;
                            armyInfo.moracle_status = "↓";
                            if (report_content[4] == "激光")
                            {
                                for (int j = 5; j < report_content.Length; j++)
                                {
                                    position = int.Parse(report_content[j]);
                                    armyInfo = army_list[getArmyIndex(position)];
                                    if (armyInfo.action_target == 0)
                                    {
                                        armyInfo.action_target = 2;
                                    }
                                    handleReport(armyInfo, "激光", "0");
                                }
                            }
                            else
                            {
                                for (int j = 4; j < report_content.Length; j = j + 3)
                                {
                                    position = int.Parse(report_content[j]);
                                    armyInfo = army_list[getArmyIndex(position)];
                                    if (armyInfo.action_target == 0)
                                    {
                                        armyInfo.action_target = 2;
                                    }
                                    handleReport(armyInfo, report_content[j + 1], report_content[j + 2]);
                                }
                            }
                        }
                        else if (report_type == "攻击")
                        {
                            //攻击|11|21|荆棘|21|伤害|204374
                            //攻击|16|23|普通|21|伤害|192726|28|伤害|206587|23|伤害|116997|23|士气|34
                            //攻击|26|13|普通|13|抵挡|0|26|伤害|83081
                            //攻击|16|23|暴击|23|伤害|185839|23|士气|34
                            //攻击|21|21|鼓舞|11|士气减少|5|22|士气|34|23|士气|34
                            position = int.Parse(report_content[1]);
                            armyInfo = army_list[getArmyIndex(position)];
                            armyInfo.action_target = 1;
                            armyInfo.status += string.Format("{0}  ", report_content[3]);
                            for (int j = 4; j < report_content.Length; j = j + 3)
                            {
                                position = int.Parse(report_content[j]);
                                armyInfo = army_list[getArmyIndex(position)];
                                if (armyInfo.action_target == 0)
                                {
                                    armyInfo.action_target = 2;
                                }
                                handleReport(armyInfo, report_content[j + 1], report_content[j + 2]);
                            }
                        }
                        else if (report_type == "激光伤害")
                        {
                            //激光伤害|11|21|伤害|121949|24|伤害|423283|11|士气|100
                            position = int.Parse(report_content[1]);
                            armyInfo = army_list[getArmyIndex(position)];
                            armyInfo.action_target = 1;
                            for (int j = 2; j < report_content.Length; j = j + 3)
                            {
                                position = int.Parse(report_content[j]);
                                armyInfo = army_list[getArmyIndex(position)];
                                if (armyInfo.action_target == 0)
                                {
                                    armyInfo.action_target = 2;
                                }
                                handleReport(armyInfo, report_content[j + 1], report_content[j + 2]);
                            }
                        }
                        else
                        {
                            UiUtils.getInstance().info(string.Format("您所查看的战报存在未知步骤 {0}", report));
                            return;
                        }
                        List<ArmyInfo> step_army_list = new List<ArmyInfo>();
                        for (int j = 0; j < army_list.Count; j++)
                        {
                            step_army_list.Add(army_list[j].Clone());
                        }
                        _steps.Add(step_army_list);
                    }
                    lbl_attacker.Text = string.Format("{0} ({1}级)", xmlDocument.SelectSingleNode("/results/attacker/playername").InnerText, xmlDocument.SelectSingleNode("/results/attacker/playerlevel").InnerText);
                    lbl_defender.Text = string.Format("{0} ({1}级)", xmlDocument.SelectSingleNode("/results/defender/playername").InnerText, xmlDocument.SelectSingleNode("/results/defender/playerlevel").InnerText);
                    lbl_result.Text = ((xmlDocument.SelectSingleNode("/results/battlereport/winside").InnerText == "1") ? "攻方胜" : "守方胜");
                    lbl_result.Visible = true;
                    XmlNode xmlNode2 = xmlDocument.SelectSingleNode("/results/battlereport/troopequipinfofrom");
                    if (xmlNode2 != null)
                    {
                        lbl_attackerWeapon.Text = getWeaponInfo(xmlNode2.InnerText);
                    }
                    XmlNode xmlNode3 = xmlDocument.SelectSingleNode("/results/battlereport/troopequipinfoto");
                    if (xmlNode3 != null)
                    {
                        lbl_defenderWeapon.Text = getWeaponInfo(xmlNode3.InnerText);
                    }
                    XmlNode xmlNode4 = xmlDocument.SelectSingleNode("/results/battlereport/attack");
                    if (xmlNode4 != null)
                    {
                        //<attack>3,神武战车,5</attack>
                        string[] pairs = xmlNode4.InnerText.Split(new char[] { ',' });
                        if (pairs.Length == 3)
                        {
                            lbl_attackerWeapon.Text = string.Format("{0}Lv{1} {2}", pairs[1], pairs[2], attZhanqi);
                        }
                    }
                    XmlNode xmlNode5 = xmlDocument.SelectSingleNode("/results/battlereport/defend");
                    if (xmlNode5 != null)
                    {
                        //<defend>2,雷霆战车,19</defend>
                        string[] pairs = xmlNode5.InnerText.Split(new char[] { ',' });
                        if (pairs.Length == 3)
                        {
                            lbl_defenderWeapon.Text = string.Format("{0}Lv{1} {2}", pairs[1], pairs[2], defZhanqi);
                        }
                    }
                    btn_init.Enabled = btn_prev.Enabled = btn_next.Enabled = btn_result.Enabled = btn_auto.Enabled = btn_stop.Enabled = true;
                    renderCurrentStep();
                }
            }
            catch (Exception ex)
            {
                UiUtils.getInstance().info("解析数据出错, 请检查是否有误, " + ex.Message);
            }
        }

        private void handleReport(ArmyInfo army, string key, string value)
        {
            if (key == "士气")
            {
                army.moracle += int.Parse(value);
                army.moracle_status = "↑";
            }
            else if (key == "士气减少")
            {
                army.moracle -= int.Parse(value);
                army.moracle_status = "↓";
            }
            else if (key == "伤害")
            {
                army.armyNow -= int.Parse(value);
                army.status += string.Format("兵力减少{0}↓  ", value);
            }
            else if (key == "士兵")
            {
                army.armyNow += int.Parse(value);
                army.status += string.Format("兵力增加{0}↑  ", value);
            }
            else
            {
                if (value == "0" || value == "出现")
                {
                    army.status += string.Format("{0}  ", key);
                }
                else if (value == "消失")
                {
                    army.buff.list.Remove(key);
                }
                else if (key == "削弱2")
                {
                    army.buff.list.Add("削弱");
                }
                else
                {
                    army.buff.list.Add(key);
                }
            }
        }

        private string hanldeTactics(string value)
        {
            int tactics = int.Parse(value);
            switch (tactics)
            {
                case 1:
                    return "擒贼擒王";
                case 3:
                    return "声东击西";
                case 11:
                    return "拨乱攻击";
                case 12:
                    return "反客为主";
                case 13:
                    return "打草惊蛇";
                case 20:
                    return "号令天下";
                case 101:
                    return "马踏千里";
                case 104:
                    return "起义攻击";
                case 109:
                    return "连续狂怒";
                case 110:
                    return "横扫千军";
                case 111:
                    return "连续玉石";
                case 117:
                    return "战斗咆哮";
                case 118:
                    return "金刚神力";
                case 122:
                    return "乱箭穿心";
                case 123:
                    return "震天咆哮";
                case 124:
                    return "号令天下";
                case 125:
                    return "英魂不灭";
                case 126:
                    return "人屠";
                case 128:
                    return "百步穿杨";
                default:
                    return "未知战法: " + value;
            }
        }
        private string getWeaponInfo(string raw)
        {
            StringBuilder sb = new StringBuilder();
            string[] weapons = raw.Split(new char[] { ',' });
            for (int i = 0;i < weapons.Length;i++)
            {
                if (weapons[i] == "")
                {
                    continue;
                }
                string[] weapon = weapons[i].Split(new char[] { '|' });
                sb.AppendFormat("{0}({1})  ", weapon[3].Substring(weapon[3].Length - 1), weapon[1]);
            }
            return sb.ToString();
        }

        private int getArmyIndex(int armyPostion)
        {
            if (armyPostion < 20)
            {
                return armyPostion - 11;
            }
            return armyPostion - 21 + 9;
        }

        private void renderCurrentStep()
        {
            lbl_steps.Text = string.Format("{0}/{1}", _currentStep + 1, _steps.Count);
            if (_currentStep > _steps.Count - 1)
            {
                return;
            }
            List<ArmyInfo> list = _steps[_currentStep];
            for (int i = 0; i < list.Count; i++)
            {
                if (i <= 8)
                {
                    renderArmy(11 + i, list[i]);
                }
                else
                {
                    renderArmy(21 + i - 9, list[i]);
                }
            }
        }

        private void renderArmy(int armyPosition, ArmyInfo info)
        {
            Panel panel = Controls.Find("panel_" + armyPosition, true)[0] as Panel;
            if (info.armyNow == 0 && info.action_target == 0)
            {
                panel.Visible = false;
                return;
            }
            panel.Visible = true;
            if (info.action_target == 0)
            {
                panel.BackColor = SystemColors.Control;
            }
            else if (info.action_target == 1)
            {
                panel.BackColor = Color.PeachPuff;
            }
            else
            {
                panel.BackColor = Color.Silver;
            }
            Label label = Controls.Find("lbl_name_" + armyPosition, true)[0] as Label;
            Label label2 = Controls.Find("lbl_level_" + armyPosition, true)[0] as Label;
            Label label3 = Controls.Find("lbl_army_" + armyPosition, true)[0] as Label;
            Label label4 = Controls.Find("lbl_moracle_" + armyPosition, true)[0] as Label;
            Label label5 = Controls.Find("lbl_status_" + armyPosition, true)[0] as Label;
            label.Text = info.name;
            label2.Text = string.Format("级:{0}", info.level);
            label3.Text = string.Format("兵:{0}/{1}", info.armyNow, info.armyMax);
            label4.Text = string.Format("士:{0}{1}", info.moracle, info.moracle_status);
            label5.Text = info.buff.ToString() + info.status;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.lbl_name_11.InvokeRequired)
                {
                    NextCallBack cb = new NextCallBack(next);
                    this.Invoke(cb);
                }
                else
                {
                    next();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("timer_Elapsed:{0}", ex.Message);
            }
        }

        public NewReportViewer(string gameUrl)
        {
            _gameUrl = gameUrl;
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Enabled = false;
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            InitializeComponent();
        }

        private void btn_openUrl_Click(object sender, EventArgs e)
        {
            if (txt_url.Text == "")
            {
                UiUtils.getInstance().info("还未输入网址");
                return;
            }
            else if (txt_url.Text.IndexOf("fight|") >= 0)
            {
                txt_url.Text = string.Format("{0}?fid={1}", _gameUrl, txt_url.Text.Substring(txt_url.Text.IndexOf("fight|") + 6));
            }
            openUrl(txt_url.Text);
        }

        private void btn_openFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
            try
            {
                string text = new StreamReader(fileStream).ReadToEnd();
                _rawText = text.Substring(text.IndexOf("<results>"));
                initContents();
            }
            catch (Exception ex)
            {
                UiUtils.getInstance().info("出错了, " + ex.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }

        private void btn_saveFile_Click(object sender, EventArgs e)
        {
            if (_rawText == null || _rawText == "")
            {
                UiUtils.getInstance().info("还没输入文件或网址呢");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CreatePrompt = true;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            StreamWriter sw = new StreamWriter(saveFileDialog.OpenFile());
            sw.Write(_rawText);
            sw.Flush();
            sw.Close();
        }

        private void btn_init_Click(object sender, EventArgs e)
        {
            _currentStep = 0;
            renderCurrentStep();
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (_currentStep > 0)
            {
                _currentStep--;
                renderCurrentStep();
            }
        }

        private void next()
        {
            if (_currentStep < _steps.Count - 1)
            {
                _currentStep++;
                renderCurrentStep();
            }
            else
            {
                _timer.Enabled = false;
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            next();
        }

        private void btn_result_Click(object sender, EventArgs e)
        {
            _currentStep = _steps.Count - 1;
            renderCurrentStep();
        }

        private void btn_auto_Click(object sender, EventArgs e)
        {
            _timer.Enabled = true;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            _timer.Enabled = false;
        }
    }
}
