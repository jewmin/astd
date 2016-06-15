using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.hero
{
	public class HeroTrainExe : ExeBase
	{
		public HeroTrainExe()
		{
			this._name = "hero";
			this._readable = "武将突飞";
		}

		public void getAllHeroes()
		{
			this._factory.getHeroManager().getAllHeros(this._proto, this._logger, ref this._user);
			this.refreshUi();
		}

		protected override void refreshUi()
		{
			this._user.addUiToQueue(this._name);
			this._user.addUiToQueue("wash");
		}

		public override void init_data()
		{
			this.getAllHeroes();
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
			{
				if (!this._user.isActivityRunning(ActivityType.GiftEvent))
				{
					return base.an_hour_later();
				}
				if (!config.ContainsKey("giftevent") || !config["giftevent"].ToLower().Equals("true"))
				{
					return base.an_hour_later();
				}
			}
            bool has_hero = false;
			this.getAllHeroes();
			int creditAvailable = base.getCreditAvailable();
			if (this._user.TufeiNumber == 0 && this._user.TufeiCredit > creditAvailable)
			{
				return base.next_hour();
			}
            else if (config.ContainsKey("train_heros") && config["train_heros"] != "")
            {
                int level = this._user.Level;
                string text = config["train_heros"];
                string[] array = text.Split(new char[] { ',' });
                if (array.Length != 0)
                {
                    List<Hero> heroes = this._user.Heroes;
                    List<Hero> list = new List<Hero>();
                    foreach (Hero current in heroes)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i].Equals(current.Id.ToString()))
                            {
                                if (current.TrainPositionId > 0)
                                {
                                    list.Add(current);
                                }
                                break;
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        has_hero = true;
                        bool trainHeroResult = true;
                        int failtimes = 0;
                        while ((!this._user.TuFeiCdFlag || this._user.TuFeiCd == 0L) && trainHeroResult && failtimes < 20)
                        {
                            failtimes++;
                            for (int j = 0; j < list.Count; j++)
                            {
                                if (list[j].TrainPositionId <= 0 || list[j].Level >= level)
                                {
                                    list.RemoveAt(j);
                                    j--;
                                }
                            }
                            for (int k = 0; k < list.Count - 1; k++)
                            {
                                for (int l = k + 1; l < list.Count; l++)
                                {
                                    if (list[k].Level > list[l].Level)
                                    {
                                        Hero value = list[k];
                                        list[k] = list[l];
                                        list[l] = value;
                                    }
                                }
                            }
                            if (this._user.TufeiNumber == 0 && this._user.TufeiCredit > creditAvailable)
                            {
                                return base.next_hour();
                            }
                            if (list != null && list.Count > 0)
                            {
                                trainHeroResult = this._factory.getHeroManager().trainHero(this._proto, this._logger, this._user, list[0].Id);
                            }
                        }
                    }
                }
            }
            if (this._user.TuFeiCdFlag && this._user.TuFeiCd > 0L)
            {
                if (config.ContainsKey("nocd") && config["nocd"].ToLower().Equals("true") && base.getGoldAvailable() >= 3)
                {
                    if (this._factory.getHeroManager().removeTrainCd(this._proto, this._logger))
                    {
                        return base.immediate();
                    }
                    else
                    {
                        return base.next_halfhour();
                    }
                }
                else
                {
                    return this._user.TuFeiCd;
                }
            }
            else if (has_hero)
            {
                return base.immediate();
            }
            else
            {
                return base.next_halfhour();
            }
		}
	}
}
