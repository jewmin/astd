using System;
using System.Collections.Generic;
using System.Text;
using com.lover.astd.common.logic;
using com.lover.astd.common.model.attack;

namespace com.lover.astd.common.logicexe.temp
{
    public class JuedouExe : TempExeBase, ITempExe, IExecute
    {
        private bool _finished = false;
        private int _juedou = 0;
        //private List<string> _juedou_blacklist = new List<string>();

        public JuedouExe()
        {
            _name = ConfigStrings.S_Temp;
            _readable = ConfigStrings.SR_Temp;
        }

        private void loadBlackList()
        {
            logInfo("加载决斗黑名单");
            //_juedou_blacklist.Clear();
            //Dictionary<string, string> config = _otherConf.getConfig(ConfigStrings.S_Attack);
            //if (config.ContainsKey(ConfigStrings.black_list))
            //{
            //    string[] list = config[ConfigStrings.black_list].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (list != null && list.Length > 0)
            //    {
            //        foreach (string item in list)
            //        {
            //            _juedou_blacklist.Add(item);
            //            logInfo(item);
            //        }
            //    }
            //}
            foreach (string item in _otherConf.BlackList)
            {
                logInfo(item);
            }
        }

        //private void saveBlackList()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (string item in _juedou_blacklist)
        //    {
        //        if (sb.Length > 0)
        //        {
        //            sb.Append(";");
        //        }
        //        sb.Append(item);
        //    }
        //    _otherConf.setConfig(ConfigStrings.S_Attack, ConfigStrings.black_list, sb.ToString());
        //    _otherConf.saveSettings();
        //}

        public override void init_data()
        {
            base.init_data();
            loadBlackList();
        }

        private void pk(PkInfo pkInfo)
        {
            Dictionary<string, string> config = _conf.getConfig(ConfigStrings.S_Attack);
            bool use_token = config.ContainsKey(ConfigStrings.use_token) && config[ConfigStrings.use_token].ToLower().Equals("true");
            BattleMgr battleManager = _factory.getBattleManager();
            MiscMgr miscManager = _factory.getMiscManager();
            if (use_token)
            {
                battleManager.useToken(_proto, _logger, _user, 10000);
            }
            int run_times = 100;
            bool find_spy = false;
            do
            {
                run_times--;
                _factory.getTroopManager().makeSureForce(_proto, _logger, 0.5);
                int worldevent;
                int seniorslaves;
                battleManager.attackPlayer(_proto, _logger, _user, pkInfo.areaId, pkInfo.scopeid, pkInfo.cityid, out worldevent, out seniorslaves);
                if (seniorslaves == 1)
                {
                    miscManager.getSeniorJailInfo(_proto, _logger);
                }
                if (worldevent == 2)
                {
                    miscManager.handleTradeFriend(_proto, _logger, _user);
                }
                else if (worldevent == 1)
                {
                    find_spy = true;
                }
                pkInfo = battleManager.getPkInfo(_proto, _logger);
            }
            while ((pkInfo.result < 0 || pkInfo.stage == 1) && run_times > 0);
            if (pkInfo.result >= 0 && pkInfo.stage == 2 && pkInfo.pkresult == 1)
            {
                logInfo(string.Format("决斗胜利, 获得战绩+{0}, 宝石+{1}", pkInfo.score, pkInfo.baoshi));
            }
            if (find_spy)
            {
                battleManager.AttackSpy(_proto, _logger, _user);
            }
        }

        public override long execute()
        {
            Dictionary<string, string> config = _conf.getConfig(ConfigStrings.S_Attack);
            int min_level = 0;
            int max_level = 0;
            if (config.ContainsKey(ConfigStrings.level_min))
            {
                int.TryParse(config[ConfigStrings.level_min], out min_level);
            }
            if (config.ContainsKey(ConfigStrings.level_max))
            {
                int.TryParse(config[ConfigStrings.level_max], out max_level);
            }
            BattleMgr battleManager = _factory.getBattleManager();
            battleManager.getNewAreaInfo(_proto, _logger, _user);
            _juedou = _user._attack_daojuFlag;
            if (_juedou > 0)
            {
                AreaInfo areaInfo = battleManager.getNearCapital(_user);
                if (areaInfo == null)
                {
                    logInfo("不在敌国首都外, 不能进行决斗");
                    _finished = true;
                }
                else if (_user._arrest_state == 100)
                {
                    battleManager.escapeFromJail(_proto, _logger);
                    _user._arrest_state = 0;
                    _finished = true;
                }
                else if (_user._arrest_state > 10)
                {
                    _finished = true;
                }
                else if (_user._attack_cityHp <= 80)
                {
                    logInfo("城防低于80, 不能进行决斗");
                    _finished = true;
                }
                else
                {
                    if (battleManager.handleDaoju(_proto, _logger, _user, areaInfo.areaid, 2, min_level, max_level, _otherConf.BlackList) == 0)
                    {
                        if (battleManager.handleDaoju(_proto, _logger, _user, areaInfo.areaid, 1, min_level, max_level, _otherConf.BlackList) == 0)
                        {
                            PkInfo pkInfo = battleManager.getPkInfo(_proto, _logger);
                            if (pkInfo.result < 0)
                            {
                                _finished = true;
                            }
                            else if (pkInfo.stage == 1)
                            {
                                if (pkInfo.fang_maxcityhp < 350)
                                {
                                    logInfo(string.Format("{0}最大城防低于350, 加入黑名单", pkInfo.fang_name));
                                    //_juedou_blacklist.Add(pkInfo.fang_name);
                                    //saveBlackList();
                                    _otherConf.addBlackList(pkInfo.fang_name);
                                }
                                pk(pkInfo);
                            }
                        }
                        else
                        {
                            logInfo("使用决斗战旗出错");
                            _finished = true;
                        }
                    }
                    else
                    {
                        logInfo("使用诱敌锦囊出错");
                        _finished = true;
                    }
                }
            }
            return immediate();
        }

        public bool isFinished()
        {
            return _finished || _user._attack_daojuFlag == 0;
        }

        public void setTarget(Dictionary<string, string> conf)
        {
            
        }

        public string getStatus()
        {
            return string.Format("决斗战旗: {0}", _juedou);
        }
    }
}
