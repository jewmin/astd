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
    public partial class NewTrainHeroCalc : Form
    {
        private int _curLevel = 1;

        private int _targetLevel = 1;

        private int _curReTrainLevel = 1;

        private int _starPerReTrain = 6;

        private int _expPerTufei;

        private int _trainLevel;

        private bool _useReTrain;

        private bool _useTufei;

        private double[] _trainLevelFactor = new double[] { 1.0, 1.5, 2.0, 2.5, 3.0, 4.0 };

        private bool _trainer1;

        private bool _trainer2;

        private bool _trainer3;

        private bool _trainer4;

        private NewMainForm _frm;

        private string getTrainInfo(long exp)
        {
            long useTufei = exp / _expPerTufei + 1;
            long tufei = 0;
            double hours;
            if (_useTufei)
            {
                double fact = 1.0;
                if (_trainer1)
                {
                    fact *= 1.25;
                }
                if (_trainer2)
                {
                    if (_trainer3)
                    {
                        fact = fact * 0.6 * 4.0;
                    }
                    else
                    {
                        fact = fact * 0.6 * 2.0;
                    }
                }
                if (_trainer4)
                {
                    fact *= 1.2;
                }
                hours = (double)useTufei / (_trainLevelFactor[_trainLevel] + fact * 10.0);
                tufei = useTufei - (long)(hours * _trainLevelFactor[_trainLevel]);
            }
            else
            {
                hours = (double)useTufei / _trainLevelFactor[_trainLevel];
            }
            return string.Format("总计需要经验值:\t{0:d}\r\n突飞次数:\t{1:d}\r\n需要小时数:\t{2:f2} ({3:f2}天)\r\n", exp, tufei, hours, hours / 24.0);
        }

        private long getTotalExpToStar(int curLevel, int curRetrainLevel, int targetLevel, int starPerRetrain)
        {
            long exp = getTotalExpToLevel(curLevel, curRetrainLevel);
            while (curRetrainLevel + starPerRetrain <= targetLevel + 1)
            {
                curRetrainLevel += starPerRetrain;
                exp += getTotalExpToLevel(1, curRetrainLevel);
            }
            return exp;
        }

        private long getTotalExpToLevel(int fromLevel, int toLevel)
        {
            long exp = 0;
            for (int i = fromLevel; i < toLevel; i++)
            {
                exp += getLevelExp(i);
            }
            return exp;
        }

        private long getLevelExp(int level)
        {
            if (level == 1)
            {
                return 45;
            }
            else if (level == 2)
            {
                return 180;
            }
            else if (level == 3)
            {
                return 450;
            }
            else if (level == 4)
            {
                return 720;
            }
            else if (level == 5)
            {
                return 900;
            }
            else if (level > 5 && level < 16)
            {
                return 900 + (level - 6) * 900;
            }
            else if (level >= 16 && level < 57)
            {
                return 10800 + (level - 16) * 1800;
            }
            else if (level == 57)
            {
                return 87300;
            }
            else if (level >= 58 && level < 79)
            {
                return 90000 + (level - 58) * 9000;
            }
            else if (level >= 79 && level < 89)
            {
                return 297000 + (level - 79) * 27000;
            }
            else if (level == 89)
            {
                return 576000;
            }
            else if (level == 90)
            {
                return 594000;
            }
            else if (level == 91)
            {
                return 621000;
            }
            else if (level == 92)
            {
                return 648000;
            }
            else if (level >= 93)
            {
                return 684000 + (level - 93) * 36000;
            }
            return 0;
        }

        public NewTrainHeroCalc(NewMainForm frm)
        {
            InitializeComponent();
            _frm = frm;
            combo_trainLevel.SelectedIndex = 0;
        }

        private void btn_calc_Click(object sender, EventArgs e)
        {
            _curLevel = (int)num_curLevel.Value;
            _curReTrainLevel = (int)num_curReTrainLevel.Value;
            _targetLevel = (int)num_targetLevel.Value;
            _starPerReTrain = (int)num_starPerReTrain.Value;
            _expPerTufei = (int)num_expPerTufei.Value;
            _trainLevel = combo_trainLevel.SelectedIndex;
            _useReTrain = chk_reTrain.Checked;
            _useTufei = chk_tufei.Checked;
            _trainer1 = chk_trainer1.Checked;
            _trainer2 = chk_trainer2.Checked;
            _trainer3 = chk_trainer3.Checked;
            _trainer4 = chk_trainer4.Checked;
            long exp;
            if (!_useReTrain)
            {
                exp = getTotalExpToLevel(_curLevel, _targetLevel);
            }
            else
            {
                exp = getTotalExpToStar(_curLevel, _curReTrainLevel, _targetLevel, _starPerReTrain);
            }
            txt_result.Text = getTrainInfo(exp);
        }

        private void btn_open_all_trainer_Click(object sender, EventArgs e)
        {
            int trainer_times = (int)num_open_all_trainer_times.Value;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("open_trainer_times", trainer_times.ToString());
            _frm.addTempServer("open_all_trainer", dictionary);
            Close();
        }
    }
}
