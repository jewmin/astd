using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.economy
{
	public class FeteExe : ExeBase
	{
		public FeteExe()
		{
			this._name = "fete";
			this._readable = "祭祀";
		}

		public override long execute()
		{
			if (this._user.Level < 30)
			{
				return base.next_day();
			}
            int goldAvailable = base.getGoldAvailable();
            Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
            {
                return base.next_day();
            }
            string[] fete_gold_array = config["fete_gold"].Split(new char[] { ',' });
            for (int i = 0; i < fete_gold_array.Length && i < this._user._fete_gold.Length; i++)
            {
                this._user._fete_gold[i] = int.Parse(fete_gold_array[i]);
            }
            MiscMgr miscManager = this._factory.getMiscManager();
            if (!miscManager.getFeteInfo(this._proto, this._logger, this._user))
            {
                return base.next_halfhour();
            }
            //免费祭祀
            for (int i = 4; i >= 0; i--)
            {
                int times = 10;
                while (this._user._fete_now_gold[i] <= 0 && this._user._fete_min_levels[i] <= this._user.Level && times > 0)
                {
                    times--;
                    miscManager.doFete(this._proto, this._logger, this._user, i + 1, true);
                    if (!miscManager.getFeteInfo(this._proto, this._logger, this._user))
                    {
                        this.logInfo("获取祭祀信息出错, 半小时后继续");
                        return base.next_halfhour();
                    }
                }
            }
            //祭祀活动期间, 金币祭祀
            if (_user.isActivityRunning(ActivityType.FeteEvent))
            {
                for (int i = 4; i >= 0; i--)
                {
                    int times = 10;
                    while (this._user._fete_now_gold[i] <= this._user._fete_gold[i] && this._user._fete_now_gold[i] <= goldAvailable && this._user._fete_min_levels[i] <= this._user.Level && times > 0)
                    {
                        times--;
                        miscManager.doFete(this._proto, this._logger, this._user, i + 1, false);
                        if (!miscManager.getFeteInfo(this._proto, this._logger, this._user))
                        {
                            this.logInfo("获取祭祀信息出错, 半小时后继续");
                            return base.next_halfhour();
                        }
                        goldAvailable = base.getGoldAvailable();
                    }
                }
            }
            //免费大祭祀
            while (this._user._fete_gold[5] > 0 && this._user._fete_now_free_times[5] > 0)
            {
                miscManager.doFete(this._proto, this._logger, this._user, 6, true);
                if (!miscManager.getFeteInfo(this._proto, this._logger, this._user))
                {
                    this.logInfo("获取祭祀信息出错, 半小时后继续");
                    return base.next_halfhour();
                }
            }
            if (this._user.isActivityRunning(ActivityType.FeteEvent))
            {
                if (miscManager.getFeteEventInfo(this._proto, this._logger, this._user) == 2)
                {
                    return base.next_day();
                }
            }
            return base.next_day();
		}
	}
}
