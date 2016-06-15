using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace com.lover.astd.game.ui
{
    public partial class NewAttackTargetSelector : Form
    {
        private string _cityname;

        public string getCityname()
        {
            return _cityname;
        }

        public void setCityInfo(Dictionary<int, int> cityinfo, int selfCityId)
        {
            foreach (int current in cityinfo.Keys)
            {
                string key = "lb_" + current;
                Control[] array = Controls.Find(key, true);
                if (Controls != null && array.Length != 0)
                {
                    LinkLabel linkLabel = array[0] as LinkLabel;
                    if (linkLabel != null)
                    {
                        if (current == selfCityId)
                        {
                            linkLabel.BackColor = Color.Plum;
                        }
                        else
                        {
                            linkLabel.BackColor = SystemColors.Control;
                        }
                        if (cityinfo[current] == 0)
                        {
                            linkLabel.LinkColor = Color.Gray;
                        }
                        else if (cityinfo[current] == 1)
                        {
                            linkLabel.LinkColor = Color.Black;
                        }
                        else if (cityinfo[current] == 2)
                        {
                            linkLabel.LinkColor = Color.Red;
                        }
                        else if (cityinfo[current] == 3)
                        {
                            linkLabel.LinkColor = Color.Blue;
                        }
                    }
                }
            }
        }

        public NewAttackTargetSelector()
        {
            InitializeComponent();
        }

        private void lnk_clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel linkLabel = sender as LinkLabel;
            _cityname = linkLabel.Text;
            Close();
        }
    }
}
