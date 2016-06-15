using System;
using System.Collections.Generic;
using System.Text;

namespace com.lover.astd.common.logicexe.equip
{
    public class WarChariotExe : ExeBase
    {
        public WarChariotExe()
		{
			this._name = ConfigStrings.S_War_Chariot;
			this._readable = ConfigStrings.SR_War_Chariot;
		}

        public override long execute()
        {
            if (this._user.Level < 288)
            {
                return base.next_day();
            }
            else
            {
                Dictionary<string, string> config = base.getConfig();
                if (config.ContainsKey(ConfigStrings.enabled) && config[ConfigStrings.enabled].ToLower().Equals("true"))
                {
                    while (this._factory.getEquipManager().handleWarChariotUpgrade(this._proto, this._logger, this._user) == 0) ;
                }
                return base.an_hour_later();
            }
        }
    }
}
