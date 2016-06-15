using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.lover.astd.common.model.misc;
using com.lover.astd.common.logic;
using com.lover.common;

namespace com.lover.astd.game.ui.ui
{
    public partial class NewDailyTreasureGameSelector : Form
    {
        private NewMainForm _frm;

        private List<TGame> _games;

        private TGame _selectGame;

        public TGame getSelectCampaign()
        {
            return _selectGame;
        }

        public void setMainForm(NewMainForm frm)
        {
            _frm = frm;
        }

        public NewDailyTreasureGameSelector()
        {
            InitializeComponent();
            btn_ok.Enabled = false;
        }

        private void btn_init_Click(object sender, EventArgs e)
        {
            ProtocolMgr protocol = new ProtocolMgr(_frm.GameUser, _frm, _frm, _frm.Gameurl, _frm.JSessionId, _frm.Factory);
            _games = _frm.Factory.getActivityManager().getAllDailyTreasureGames(protocol, _frm);
            combo_games.DataSource = _games;
            combo_games.DisplayMember = "Desc";
            if (_games.Count > 0)
            {
                btn_ok.Enabled = true;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (_games == null || _games.Count == 0)
            {
                UiUtils.getInstance().info("还未选择任何副本");
                return;
            }
            if (combo_games.SelectedIndex < 0)
            {
                UiUtils.getInstance().info("还未选择任何副本");
                return;
            }
            int selectedIndex = combo_games.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= _games.Count)
            {
                UiUtils.getInstance().info("还未选择任何副本");
                return;
            }
            _selectGame = _games[selectedIndex];
            Close();
        }
    }
}
