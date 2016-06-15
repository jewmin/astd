using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.model;

namespace com.lover.astd.common.logicexe.hero
{
    public class BigHeroTrainExe : ExeBase
    {
        private int max_big_level_;
        public BigHeroTrainExe()
		{
			this._name = ConfigStrings.S_BigHeroTrain;
            this._readable = ConfigStrings.SR_BigHeroTrain;
		}

        public void getAllBigGenerals()
        {
            _factory.getBigHeroManager().getAllBigGenerals(_proto, _logger);
            _factory.getBigHeroManager().getBigTrainInfo(_proto, _logger, out max_big_level_);
        }

        public override void init_data()
        {
            getAllBigGenerals();
        }

        public override long execute()
        {
            Dictionary<string, string> config = getConfig();
            if (!config.ContainsKey(ConfigStrings.enabled) || !config[ConfigStrings.enabled].ToLower().Equals("true"))
            {
                return an_hour_later();
            }
            getAllBigGenerals();
            List<BigHero> heros = _factory.getBigHeroManager().heros_;
            heros.Sort();
            foreach (BigHero hero in heros)
            {
                if (hero.Big == 0)
                {
                    if (_factory.getBigHeroManager().toBigGeneral(_proto, _logger, hero.Id))
                    {
                        logInfo(string.Format("{0} 转成大将", hero.Name));
                    }
                    continue;
                }
                if (hero.BigLevel >= max_big_level_)
                {
                    continue;
                }
                if (hero.TuFei <= 0)
                {
                    continue;
                }
                if (!_factory.getBigHeroManager().startBigTrain(_proto, _logger, 1, hero.Id))
                {
                    continue;
                }
                logInfo(string.Format("开始训练大将 {0}", hero.Name));
                int use_tu_fei = 0;
                while (hero.TuFei > 0)
                {
                    if (_factory.getBigHeroManager().fastTrainBigGeneral(_proto, _logger, hero.Id))
                    {
                        use_tu_fei++;
                        hero.TuFei--;
                    }
                    else
                    {
                        break;
                    }
                }
                if (use_tu_fei > 0)
                {
                    logInfo(string.Format("使用{0}个大将令突飞大将 {1}", use_tu_fei, hero.Name));
                }
            }
            getAllBigGenerals();
            heros = _factory.getBigHeroManager().heros_;
            heros.Sort();
            int pos = 1;
            foreach (BigHero hero in heros)
            {
                if (pos > 3)
                {
                    break;
                }
                if (hero.Big == 1 && hero.BigLevel < max_big_level_ && _factory.getBigHeroManager().startBigTrain(_proto, _logger, pos, hero.Id))
                {
                    logInfo(string.Format("开始训练大将 {0}", hero.Name));
                    pos++;
                }
            }
            return next_day();
        }
    }
}
