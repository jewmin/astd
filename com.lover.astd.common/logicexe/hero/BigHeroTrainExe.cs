using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.model;
using com.lover.astd.common.logic;

namespace com.lover.astd.common.logicexe.hero
{
    public class BigHeroTrainExe : ExeBase
    {
        private const int change_big_level_ = 100;
        private int max_big_level_;
        private int total_pos_;
        private int freenum_;
        private BigHeroMgr mgr_;
        public BigHeroTrainExe()
		{
			this._name = ConfigStrings.S_BigHeroTrain;
            this._readable = ConfigStrings.SR_BigHeroTrain;
		}

        public void getAllBigGenerals()
        {
            mgr_ = _factory.getBigHeroManager();
            mgr_.getAllBigGenerals(_proto, _logger);
            mgr_.getBigTrainInfo(_proto, _logger, out max_big_level_, out total_pos_, out freenum_);
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
            List<BigHero> heros = mgr_.heros_;
            heros.Sort();
            foreach (BigHero hero in heros)
            {
                bool canChange = max_big_level_ >= change_big_level_ && hero.BigLevel >= max_big_level_;
                if (hero.Big == 0)
                {
                    if (mgr_.toBigGeneral(_proto, _logger, hero.Id))
                    {
                        logInfo(string.Format("{0} 转成大将", hero.Name));
                    }
                    continue;
                }
                if (hero.TuFei <= 0)
                {
                    continue;
                }
                if (!mgr_.startBigTrain(_proto, _logger, 1, hero.Id))
                {
                    continue;
                }
                if (canChange && hero.Change == 0)
                {
                    if (mgr_.bigGeneralChange(_proto, _logger, hero.Id))
                    {
                        logInfo(string.Format("大将【{0}】Lv.{1}晋升", hero.Name, hero.BigLevel));
                    }
                }
                logInfo(string.Format("开始大将{0} {1}", canChange ? "突破" : "突飞", hero.Name));
                int use_free_num = 0;
                while (freenum_ > 0)
                {
                    if (canChange) break;
                    if (mgr_.fastTrainBigGeneral(_proto, _logger, hero.Id))
                    {
                        use_free_num++;
                        freenum_--;
                    }
                }
                if (use_free_num > 0)
                {
                    logInfo(string.Format("使用{0}次免费对大将【{1}】进行{2}", freenum_, hero.Name, canChange ? "突破" : "突飞"));
                }
                int use_tu_fei = 0;
                int cost = 0;
                while (hero.TuFei > 0)
                {
                    if (canChange)
                    {
                        if (mgr_.newTrainBigGeneral(_proto, _logger, hero.Id, out cost))
                        {
                            use_tu_fei += cost;
                            hero.TuFei -= cost;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (mgr_.fastTrainBigGeneral(_proto, _logger, hero.Id))
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
                    logInfo(string.Format("使用{0}个大将令对大将【{1}】进行{2}", use_tu_fei, hero.Name, canChange ? "突破" : "突飞"));
                }
            }
            getAllBigGenerals();
            heros = mgr_.heros_;
            heros.Sort();
            int pos = 1;
            foreach (BigHero hero in heros)
            {
                if (pos > total_pos_) break;
                if (hero.Big == 1 && hero.BigLevel < max_big_level_ && _factory.getBigHeroManager().startBigTrain(_proto, _logger, pos, hero.Id))
                {
                    logInfo(string.Format("开始训练大将 {0}", hero.Name));
                    pos++;
                }
            }
            getAllBigGenerals();
            heros = mgr_.heros_;
            heros.Sort();
            pos = 1;
            foreach (BigHero hero in heros)
            {
                if (hero.TrainPos == 0) continue;
                if (pos > total_pos_) break;
                foreach (BigHeroExpBook item in mgr_.expbooks_)
                {
                    if (item.type == hero.GeneralType)
                    {
                        while (item.num > 0)
                        {
                            item.num--;
                            if (!mgr_.useExpBook(_proto, _logger, hero.Id)) break;
                        }
                        break;
                    }
                }
                pos++;
            }
            return next_day();
        }
    }
}
