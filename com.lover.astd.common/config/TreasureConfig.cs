using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.config
{
    public class TreasureConfig
    {
        private int _pos;
        private int _use_dice_type;
        public int Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public int DiceType
        {
            get { return _use_dice_type; }
            set
            {
                if (value == 1)
                {
                    _use_dice_type = 1;
                }
                else if (value == 2)
                {
                    _use_dice_type = 2;
                }
                else
                {
                    _use_dice_type = 0;
                }
            }
        }
        public TreasureConfig()
        {
            _pos = 0;
            _use_dice_type = 0;
        }
        public TreasureConfig(int pos, int dice_type)
        {
            Pos = pos;
            DiceType = dice_type;
        }
    }
}
