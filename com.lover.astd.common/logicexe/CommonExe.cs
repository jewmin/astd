using System;
using System.Collections.Generic;
using com.lover.astd.common.model.misc;

namespace com.lover.astd.common.logicexe
{
	public class CommonExe : ExeBase
	{
		public CommonExe()
		{
			this._name = "global";
			this._readable = "通用";
		}

		public override void init_data()
		{
			this._factory.getMiscManager().getServerTime(this._proto, this._logger);
			this._factory.getMiscManager().getPlayerInfo(this._proto, this._logger, "", this._user);
			this._factory.getMiscManager().getExtraInfo(this._proto, this._logger, this._user);
			this._factory.getMiscManager().getExtraInfo2(this._proto, this._logger, this._user);
			this._factory.getBuildingManager().getBuildingMainCity(this._proto, this._logger, this._user);
			Dictionary<string, string> config = base.getConfig();
			bool auto_logon_award = config.ContainsKey("auto_logon_award") && config["auto_logon_award"].ToLower().Equals("true");
			bool auto_cai = config.ContainsKey("auto_cai") && config["auto_cai"].ToLower().Equals("true");
			bool auto_buy_credit = config.ContainsKey("auto_buy_credit") && config["auto_buy_credit"].ToLower().Equals("true");
			if (auto_logon_award && !this._user.GotLoginReward)
			{
                this._factory.getMiscManager().getPerDayReward(this._proto, this._logger, this._user);
                this._factory.getMiscManager().getLoginReward(this._proto, this._logger, this._user);
                this._factory.getMiscManager().handleChampionInfo(this._proto, this._logger);
			}
			this._factory.getMiscManager().getNewGiftReward(this._proto, this._logger, this._user);
            this._factory.getMiscManager().getNewPerDayTask(this._proto, this._logger, this._user);
            this._factory.getMiscManager().getSeniorJailInfo(this._proto, this._logger);
			if (auto_cai)
			{
				this._factory.getMiscManager().gamblingStone(this._proto, this._logger, this._user);
			}
			if (auto_buy_credit)
			{
				this._factory.getMiscManager().autoBuyCredit(this._proto, this._logger, this._user);
			}
            this._factory.getMiscManager().getGeneralTowerInfo(this._proto, this._logger);
            this._factory.getActivityManager().getPayHongbaoEventInfo(this._proto, this._logger);
            if (this._user.Silver < 10000000)
            {
                this._factory.getMiscManager().ticketExchangeMoney(this._proto, this._logger);
            }
            int ticket = 0;
            int total_ticket = 0;
            List<TicketItem> itemList = new List<TicketItem>();
            if (config.ContainsKey(ConfigStrings.ticket_bighero) && config[ConfigStrings.ticket_bighero] != "")
            {
                ticket = int.Parse(config[ConfigStrings.ticket_bighero]);
            }
            this._factory.getMiscManager().ticketGetInfo(this._proto, this._logger, ref total_ticket, ref itemList);
            foreach (TicketItem item in itemList)
            {
                if (item.tickets <= ticket && item.playerlevel <= this._user.Level)
                {
                    this._factory.getMiscManager().ticketExchangeBigHero(this._proto, this._logger, item);
                }
            }
            if (this._user.Stone <= 1000000)
            {
                this._factory.getMiscManager().ticketExchangeWeapon(this._proto, this._logger, 33, "玉石", 1);
            }
            this._factory.getTroopManager().makeSureForce(this._proto, this._logger, 0.5);
			this.refreshUi();
		}

		public override long execute()
		{
			this.init_data();
			return base.next_halfhour();
		}
	}
}
