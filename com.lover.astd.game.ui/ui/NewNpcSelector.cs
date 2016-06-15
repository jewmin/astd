using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.lover.astd.common.model.battle;
using System.Globalization;

namespace com.lover.astd.game.ui.ui
{
    public partial class NewNpcSelector : Form
    {
        private List<Npc> _npcs;

        private List<string> _itemNames;

        private List<string> _itemColors;

        private List<Npc> _itemNpcs = new List<Npc>();

        private Npc _selectedNpc;

        private string _formation;

        public Npc getSelectedNpc()
        {
            return _selectedNpc;
        }

        public string getFormation()
        {
            return _formation;
        }

        public void setNpcs(List<Npc> npcs)
        {
            _npcs = npcs;
            _itemNames = new List<string>();
            _itemColors = new List<string>();
            foreach (Npc current in npcs)
            {
                string itemNamePure = current.ItemNamePure;
                if (!_itemNames.Contains(itemNamePure))
                {
                    _itemNames.Add(itemNamePure);
                    _itemColors.Add(current.ItemColor);
                }
            }
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = _itemNames;
            lst_reason.DataSource = bindingSource;
            lst_reason.SelectedIndex = -1;
        }

        private void renderNpcByItem(string itemname)
        {
            _itemNpcs.Clear();
            if (itemname != null && itemname.Length > 0)
            {
                foreach (Npc current in _npcs)
                {
                    if (current.ItemNamePure.Equals(itemname))
                    {
                        _itemNpcs.Add(current);
                    }
                }
            }
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = _itemNpcs;
            lst_npc.DataSource = bindingSource;
            lst_npc.SelectedIndex = -1;
        }

        public NewNpcSelector()
        {
            InitializeComponent();
        }

        private void lst_reason_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            e.DrawBackground();
            Brush brush = Brushes.Black;
            int index = e.Index;
            if (index < 0)
            {
                return;
            }
            string text = _itemColors[index].TrimStart(new char[] { '#' });
            if (text != "")
            {
                brush = new SolidBrush(Color.FromArgb(int.Parse(text.Substring(0, 2), NumberStyles.AllowHexSpecifier), int.Parse(text.Substring(2, 2), NumberStyles.AllowHexSpecifier), int.Parse(text.Substring(4, 2), NumberStyles.AllowHexSpecifier)));
                e.Graphics.DrawString(listBox.Items[e.Index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        private void lst_reason_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemname = lst_reason.SelectedItem as string;
            renderNpcByItem(itemname);
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (lst_npc.SelectedIndex >= 0)
            {
                _selectedNpc = lst_npc.SelectedItem as Npc;
            }
            if (combo_formation.SelectedIndex < 0)
            {
                _formation = "不变阵";
            }
            else
            {
                _formation = combo_formation.SelectedItem as string;
            }
            Close();
        }
    }
}
