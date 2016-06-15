using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.lover.astd.common.model.battle;
using com.lover.astd.common.logic;
using com.lover.common;

namespace com.lover.astd.game.ui.ui
{
    public partial class NewResCampaignSelector : Form
    {
        private NewMainForm _frm;

        private List<ResCampaign> _campaigns;

        private ResCampaign _selectCampaign;

        public ResCampaign getSelectCampaign()
        {
            return _selectCampaign;
        }

        public void setMainForm(NewMainForm frm)
        {
            _frm = frm;
        }

        public NewResCampaignSelector()
        {
            InitializeComponent();
            btn_ok.Enabled = false;
        }

        private void btn_init_Click(object sender, EventArgs e)
        {
            ProtocolMgr protocol = new ProtocolMgr(_frm.GameUser, _frm, _frm, _frm.Gameurl, _frm.JSessionId, _frm.Factory);
            _campaigns = _frm.Factory.getBattleManager().getResCampaigns(protocol, _frm);
            combo_campaigns.DataSource = _campaigns;
            combo_campaigns.DisplayMember = "Name";
            if (_campaigns.Count > 0)
            {
                btn_ok.Enabled = true;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (_campaigns == null || _campaigns.Count == 0)
            {
                UiUtils.getInstance().info("还未选择任何副本");
                return;
            }
            if (combo_campaigns.SelectedIndex < 0)
            {
                UiUtils.getInstance().info("还未选择任何副本");
                return;
            }
            int selectedIndex = combo_campaigns.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= _campaigns.Count)
            {
                UiUtils.getInstance().info("还未选择任何副本");
                return;
            }
            _selectCampaign = _campaigns[selectedIndex];
            Close();
        }
    }
}
