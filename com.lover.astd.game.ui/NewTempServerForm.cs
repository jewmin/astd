﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.lover.astd.common;
using com.lover.astd.game.ui.server.impl.logger;
using com.lover.astd.common.logic;
using com.lover.astd.common.model.battle;
using com.lover.common;
using com.lover.astd.common.model;

namespace com.lover.astd.game.ui
{
    public partial class NewTempServerForm : Form
    {
        private class HeroWashInfo
        {
            public int hero_id;

            public string hero_name;

            public int hero_level;

            public string wash_what;

            public string desc
            {
                get
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat("{0}-{1}级 [", hero_name, hero_level);
                    if (wash_what.Length >= 1 && wash_what[0] == '1')
                    {
                        stringBuilder.Append("统");
                    }
                    if (wash_what.Length >= 2 && wash_what[1] == '1')
                    {
                        stringBuilder.Append("勇");
                    }
                    if (wash_what.Length >= 3 && wash_what[2] == '1')
                    {
                        stringBuilder.Append("智");
                    }
                    stringBuilder.Append("]");
                    return stringBuilder.ToString();
                }
            }
        }

        private NewMainForm _frm;

        private List<HeroWashInfo> _heroes_to_wash = new List<HeroWashInfo>();

        public NewTempServerForm(NewMainForm frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void btn_init_campaign_Click(object sender, EventArgs e)
        {
            ILogger logger = new TempLogger(_frm);
            ProtocolMgr protocol = new ProtocolMgr(_frm.GameUser, logger, _frm, _frm.Gameurl, _frm.JSessionId, _frm.Factory);
            List<CampaignItem> campaigns = _frm.Factory.getCampaignManager().getCampaigns(protocol, logger);
            if (campaigns == null)
            {
                UiUtils.getInstance().info("未找到战役");
                return;
            }
            combo_campains.DataSource = campaigns;
            combo_campains.DisplayMember = "Name";
            combo_campains.ValueMember = "ID";
            btn_campaign.Enabled = true;
        }

        private void btn_campaign_Click(object sender, EventArgs e)
        {
            if (combo_campains.SelectedValue == null)
            {
                UiUtils.getInstance().info("还没有选择打哪个");
                return;
            }
            int count = (int)num_campaign_count.Value;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string from = combo_campains.SelectedValue.ToString();
            dictionary.Add("from", from);
            dictionary.Add("count", count.ToString());
            _frm.addTempServer("campaign", dictionary);
            Close();
        }

        private void btn_ticketWeapon_Click(object sender, EventArgs e)
        {
            if (_frm.GameUser.Level < 131)
            {
                UiUtils.getInstance().info("必须131级以上才能兑换紫兵器, so... 你还是呆着吧");
                return;
            }
            if (combo_ticketWeapons.SelectedIndex < 0)
            {
                UiUtils.getInstance().info("你什么都木有选择呢");
                return;
            }
            int exchange_id = combo_ticketWeapons.SelectedIndex + 12;
            int exchange_count = (int)num_ticketWeapons.Value;
            string exchange_name = combo_ticketWeapons.SelectedItem.ToString();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("exchange_id", exchange_id.ToString());
            dictionary.Add("exchange_count", exchange_count.ToString());
            dictionary.Add("exchange_name", exchange_name);
            _frm.addTempServer("ticket_exchange", dictionary);
            Close();
        }

        private void btn_init_melt_Click(object sender, EventArgs e)
        {
            ILogger logger = new TempLogger(_frm);
            ProtocolMgr protocol = new ProtocolMgr(_frm.GameUser, logger, _frm, _frm.Gameurl, _frm.JSessionId, _frm.Factory);
            List<Equipment> storeCanMelt = _frm.Factory.getEquipManager().getStoreCanMelt(protocol, logger, _frm.GameUser);
            combo_store_equips.DataSource = storeCanMelt;
            combo_store_equips.DisplayMember = "Name";
            combo_store_equips.ValueMember = "Id";
            btn_melt.Enabled = true;
        }

        private void btn_melt_Click(object sender, EventArgs e)
        {
            if (combo_store_equips.SelectedValue == null)
            {
                UiUtils.getInstance().info("还没有选择装备");
                return;
            }
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("equipId", combo_store_equips.SelectedValue.ToString());
            _frm.addTempServer("equip_melt", dictionary);
            Close();
        }

        private void btn_init_wash_Click(object sender, EventArgs e)
        {
            ILogger logger = new TempLogger(_frm);
            ProtocolMgr protocol = new ProtocolMgr(_frm.GameUser, logger, _frm, _frm.Gameurl, _frm.JSessionId, _frm.Factory);
            _frm.Factory.getHeroManager().getWashInfo(protocol, logger, _frm.GameUser);
            List<Hero> heroes = _frm.GameUser.Heroes;
            if (heroes == null)
            {
                UiUtils.getInstance().info("未找到武将");
                return;
            }
            combo_wash_heroes.DataSource = heroes;
            combo_wash_heroes.DisplayMember = "Desc";
            combo_wash_heroes.ValueMember = "Id";
            btn_start_wash.Enabled = true;
            _heroes_to_wash.Clear();
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = _heroes_to_wash;
            lst_wash.DataSource = bindingSource;
            lst_wash.DisplayMember = "desc";
        }

        private void btn_add_wash_Click(object sender, EventArgs e)
        {
            if (combo_wash_heroes.SelectedValue == null)
            {
                UiUtils.getInstance().info("还没有选择洗哪个");
                return;
            }
            bool tong = chk_wash_tong.Checked;
            bool yong = chk_wash_yong.Checked;
            bool zhi = chk_wash_zhi.Checked;
            if (!tong && !yong && !zhi)
            {
                UiUtils.getInstance().info("大哥, 你洗啥都不选, 你想干啥, 你这是想洗将??");
                return;
            }
            string text = "";
            text += (tong ? "1" : "0");
            text += (yong ? "1" : "0");
            text += (zhi ? "1" : "0");
            HeroWashInfo heroWashInfo = new HeroWashInfo();
            heroWashInfo.wash_what = text;
            Hero hero = this.combo_wash_heroes.SelectedItem as Hero;
            if (hero == null)
            {
                UiUtils.getInstance().info("读取武将数据失败, 请关闭此窗口重试");
                return;
            }
            heroWashInfo.hero_id = hero.Id;
            heroWashInfo.hero_name = hero.Name;
            heroWashInfo.hero_level = hero.Level;
            _heroes_to_wash.Add(heroWashInfo);
            (lst_wash.DataSource as BindingSource).ResetBindings(false);
        }

        private void btn_del_wash_Click(object sender, EventArgs e)
        {
            if (lst_wash.SelectedIndex < 0)
            {
                UiUtils.getInstance().info("没有选择要删除的洗将!");
                return;
            }
            HeroWashInfo heroWashInfo = lst_wash.SelectedItem as HeroWashInfo;
            if (heroWashInfo == null)
            {
                UiUtils.getInstance().info("没有选择要删除的洗将!!");
                return;
            }
            _heroes_to_wash.Remove(heroWashInfo);
            (lst_wash.DataSource as BindingSource).ResetBindings(false);
        }

        private void btn_start_wash_Click(object sender, EventArgs e)
        {
            if (_heroes_to_wash.Count == 0)
            {
                UiUtils.getInstance().info("还没有选择洗哪个");
                return;
            }
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _heroes_to_wash.Count; i++)
            {
                HeroWashInfo heroWashInfo = _heroes_to_wash[i];
                stringBuilder.AppendFormat("{0}:{1}:{2}-{3}级,", heroWashInfo.hero_id, heroWashInfo.wash_what, heroWashInfo.hero_name, heroWashInfo.hero_level);
            }
            dictionary.Add("wash_info", stringBuilder.ToString());
            _frm.addTempServer("wash", dictionary);
            Close();
        }

        private void btn_rawstone_Click(object sender, EventArgs e)
        {
            int target_amount = (int)num_rawstone_amount.Value;
            string sell_raw_to_sys = chk_rawstone_sellsys.Checked.ToString().ToLower();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("target_amount", target_amount.ToString());
            dictionary.Add("sell_raw_to_sys", sell_raw_to_sys);
            _frm.addTempServer("cut_rawstone", dictionary);
            Close();
        }

        private void btn_doWeave_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> conf = new Dictionary<string, string>();
            conf.Add(ConfigStrings.weave_item, _frm.combo_movable_weave_like.SelectedIndex.ToString());
            _frm.addTempServer("weave", conf);
            Close();
        }

        private void btn_doStore_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> conf = new Dictionary<string, string>();
            _frm.addTempServer("dump_store", conf);
            Close();
        }

        private void btn_juedou_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> conf = new Dictionary<string, string>();
            _frm.addTempServer("juedou", conf);
            Close();
        }

        private void btn_hongbao_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> conf = new Dictionary<string, string>();
            _frm.addTempServer("hongbao", conf);
            Close();
        }
    }
}
