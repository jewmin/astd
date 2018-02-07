using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.model
{
    public class SpecialEquip : BaseObject
    {
        public int storeid { get; set; }
        public int equiplevel { get; set; }
        public string equipname { get; set; }
        public int equiptype { get; set; }
        public int quality { get; set; }
        public SpecialEquip()
        {
            storeid = 0;
            equiplevel = 0;
            equipname = "";
            equiptype = 0;
            quality = 0;
        }
        public string GetEquipName()
        {
            return string.Format("{0}星{1}", equiplevel, equipname);
        }
    }
}
