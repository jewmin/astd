using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;
using com.lover.astd.common.model;

namespace com.lover.astd.common.logicexe.hero
{
	public class HeroWashExe : ExeBase
	{
		public HeroWashExe()
		{
			this._name = "wash";
			this._readable = "武将洗属性";
		}

        public override long execute()
        {
            Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
            {
                return base.an_hour_later();
            }

            int creditCostToWash = 0;
            int goldWashCount = 0;
            int superWashCount = 0;
            int canawaken = 0;
            HeroMgr heroManager = this._factory.getHeroManager();
            this._user.BigHeroList = heroManager.getWashModes(this._proto, this._logger, ref creditCostToWash, ref goldWashCount, ref superWashCount, ref canawaken);
            this._user.BigHeroList.Sort();

            if (canawaken == 1)
            {
                if (config.ContainsKey("open_awaken") && config["open_awaken"].ToLower().Equals("true"))
                {
                    string bh_ids = "";
                    if (config.ContainsKey("bh_ids")) bh_ids = config["bh_ids"];
                    List<int> ids = base.generateIds(bh_ids);

                    bool onlyawaken = config.ContainsKey("only_awaken") && config["only_awaken"].ToLower().Equals("true");
                    bool useawakenwine = config.ContainsKey("use_awaken_wine") && config["use_awaken_wine"].ToLower().Equals("true");

                    foreach (com.lover.astd.common.model.BigHero general in this._user.BigHeroList)
                    {
                        if (ids.Contains(general.Id) && general.CanAwaken == 1)
                        {
                            GeneralAwakeInfo info = heroManager.getAwakenGeneralInfo(this._proto, this._logger, general);
                            if (info != null)
                            {
                                if (!onlyawaken || info.isawaken == 0)
                                {
                                    while (info.freeliquornum >= info.needliquornum)
                                    {
                                        heroManager.awakenGeneral(this._proto, this._logger, general, true, ref info.needliquornum);
                                        info.freeliquornum -= info.needliquornum;
                                    }

                                    if (useawakenwine)
                                    {
                                        while (info.liquornum >= info.needliquornum)
                                        {
                                            heroManager.awakenGeneral(this._proto, this._logger, general, false, ref info.needliquornum);
                                            info.liquornum -= info.needliquornum;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (goldWashCount == 0 && superWashCount == 0)
            {
                this.logInfo("条件不足, 今天不再洗属性");
                return next_day();
            }

            string wash_attrib = config["wash_attrib"];
            if (wash_attrib.Length < 3)
            {
                for (int i = 0; i < 3 - wash_attrib.Length; i++)
                {
                    wash_attrib += "0";
                }
            }

            bool finish = true;
            foreach (com.lover.astd.common.model.BigHero general in this._user.BigHeroList)
            {
                int[] old_attrib = new int[3];
                int result = heroManager.startWash(this._proto, this._logger, this._user, general.Id, wash_attrib, true, base.getCreditAvailable(), 20, ref old_attrib);
                if (result == 3)
                {
                    this.logInfo("条件不足, 今天不再洗属性");
                    return next_day();
                }
                else if (result != 1)
                {
                    finish = false;
                }
            }

            if (finish)
            {
                this.logInfo("武将已经洗属性完毕");
                return next_day();
            }

            return base.immediate();
        }

        public override void init_data()
        {
            int creditCostToWash = 0;
            int goldWashCount = 0;
            int superWashCount = 0;
            int canawaken = 0;
            this._user.BigHeroList = this._factory.getHeroManager().getWashModes(this._proto, this._logger, ref creditCostToWash, ref goldWashCount, ref superWashCount, ref canawaken);
            this.refreshUi();
        }

        //public override long execute()
        //{
        //    Dictionary<string, string> config = base.getConfig();
        //    if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
        //    {
        //        return base.an_hour_later();
        //    }

        //    if (config.ContainsKey("wash_hero") && config["wash_hero"] != "")
        //    {
        //        int heroId = 0;
        //        if (int.TryParse(config["wash_hero"], out heroId))
        //        {
        //            string wash_attrib = config["wash_attrib"];
        //            if (wash_attrib.Length < 3)
        //            {
        //                for (int i = 0; i < 3 - wash_attrib.Length; i++)
        //                {
        //                    wash_attrib += "0";
        //                }
        //            }
        //            HeroMgr heroManager = this._factory.getHeroManager();
        //            if (heroId == 0 || wash_attrib == "")
        //            {
        //                this.logInfo("没有可洗的武将");
        //                return base.next_hour();
        //            }
        //            int[] old_attrib = new int[3];
        //            int result = heroManager.startWash(this._proto, this._logger, this._user, heroId, wash_attrib, true, base.getCreditAvailable(), 20, ref old_attrib);
        //            if (result == 1)
        //            {
        //                this.logInfo("武将已经洗属性完毕, 将在1小时候再次检查");
        //                return 3600000L;
        //            }
        //            else if (result == 2)
        //            {
        //                this.logInfo("洗将出错, 半小时后继续");
        //                return 1800000L;
        //            }
        //            else if (result == 3)
        //            {
        //                this.logInfo("条件不足, 今天不再洗属性");
        //                return base.next_day();
        //            }
        //            this.refreshUi();
        //            return base.next_day();
        //        }
        //    }
        //    return base.immediate();
        //}
	}
}
