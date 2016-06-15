using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using com.lover.astd.common.model.attack;
using System;
using System.Collections.Generic;
using com.lover.astd.common.config;
using System.Text;

namespace com.lover.astd.common.logicexe.battle
{
	public class AttackExe : ExeBase
	{
        /// <summary>
        /// 下次攻坚执行时间
        /// </summary>
		private long _next_gongjian_exetime;
        /// <summary>
        /// 下次攻击执行时间
        /// </summary>
		private long _next_attack_exetime;
        /// <summary>
        /// 下次移动执行时间
        /// </summary>
		private long _next_move_exetime;
        /// <summary>
        /// 下次悬赏执行时间
        /// </summary>
		private long _next_cityevent_exetime;
        /// <summary>
        /// 下次杂七杂八执行时间
        /// </summary>
		private long _next_misc_exetime;
        /// <summary>
        /// 下次决斗执行时间
        /// </summary>
        private long _next_daoju_exetime;
        /// <summary>
        /// 悬赏剩余时间
        /// </summary>
        private long _cityevent_remain_time;

        //private List<string> _juedou_blacklist = new List<string>();

		public override void clearTmpVariables()
		{
			this._next_gongjian_exetime = 0;
			this._next_attack_exetime = 0;
			this._next_move_exetime = 0;
			this._next_cityevent_exetime = 0;
			this._next_misc_exetime = 0;
            this._next_daoju_exetime = 0;
            this._cityevent_remain_time = 0;
		}

		public AttackExe()
		{
            this._name = ConfigStrings.S_Attack;
            this._readable = ConfigStrings.SR_Attack;
		}

		public override void init_data()
		{
			if (this._user._inNewArea)
			{
				this._factory.getBattleManager().getNewAreaInfo(this._proto, this._logger, this._user);
			}
            this.refreshUi();
            loadBlackList();
		}

        /// <summary>
        /// 天降奇兵0:有NPC,-5已领取奖励
        /// </summary>
        /// <returns></returns>
		private long do_nationevent()
		{
			BattleMgr battleManager = this._factory.getBattleManager();
			Dictionary<string, string> config = base.getConfig();
			if (config.ContainsKey("nation_event"))
			{
				config["nation_event"].ToLower().Equals("true");
			}
			long remain_time = 0;
			int nationEventRet = battleManager.handleNationEventInfo(this._proto, this._logger, this._user, out remain_time);
			if (nationEventRet == 0)//有目标
			{
				if (this._user._attack_selfCityId == 112)//许都
				{
					battleManager.newMoveToArea(this._proto, this._logger, this._user, 111, out remain_time);
				}
				if (this._user._attack_selfCityId == 113)//成都
				{
					battleManager.newMoveToArea(this._proto, this._logger, this._user, 114, out remain_time);
				}
				if (this._user._attack_selfCityId == 134)//武昌
				{
					battleManager.newMoveToArea(this._proto, this._logger, this._user, 128, out remain_time);
				}
                return this.attack_nationevent_player();
            }
            else if (nationEventRet == 1)//空值
            {
                return base.immediate();
            }
            else if (nationEventRet == 2)
            {
                return base.immediate();
            }
            else if (nationEventRet == 10)//出错
            {
                return -4;
            }
            else if (nationEventRet == 9)//领取奖励
            {
                return -5;
            }
            else
            {
                return base.immediate();
            }
		}

        /// <summary>
        /// 悬赏 -1:移动, -2:已移动到指定位置, -3:大于星级, -4:出错
        /// </summary>
        /// <returns></returns>
        private long do_cityevent(bool is_doing_nation)
		{
			BattleMgr battleManager = this._factory.getBattleManager();
			Dictionary<string, string> config = base.getConfig();
			if (config.ContainsKey("city_event"))
			{
				config["city_event"].ToLower().Equals("true");
			}
			int max_star = 3;
			if (config.ContainsKey("city_event_maxstar"))
			{
				int.TryParse(config["city_event_maxstar"], out max_star);
			}
			int reserved_num = 0;
			if (config.ContainsKey("city_event_reserved_num"))
			{
				int.TryParse(config["city_event_reserved_num"], out reserved_num);
			}
			string move_target = "";
			if (config.ContainsKey("move_target"))
			{
				move_target = config["move_target"];
			}
			int cityEventRet = battleManager.handleNewCityEventInfo(this._proto, this._logger, this._user, max_star, reserved_num, move_target, out this._cityevent_remain_time);
			if (cityEventRet == 0)
			{
                AreaInfo areaById = this._user.getAreaById(this._user._attack_cityevent_target_areaid);
                if (areaById == null)
                {
                    return -1;
                }
                this.logInfo(string.Format("目前处于悬赏任务期间, 悬赏目标位于为[{0}], 将忽略设置的移动目标", areaById.areaname));
                long attackRet = this.attack_cityevent_player();
                if (attackRet == 0)//距离太远
                {
                    AreaInfo nextMoveArea = battleManager.getNextMoveArea(this._proto, this._logger, this._user, areaById, true, is_doing_nation);
                    if (nextMoveArea == null)
                    {
                        return this.attack_cityevent_player();
                    }
                    int moveRet = battleManager.moveToArea(this._proto, this._logger, this._user, nextMoveArea.areaid);
                    if (moveRet == 0)
                    {
                        if (nextMoveArea.areaid != this._user._attack_cityevent_target_areaid)
                        {
                            return 60100;
                        }
                        attackRet = this.attack_cityevent_player();
                        if (attackRet >= 0)
                        {
                            return attackRet;
                        }
                        return -3;
                    }
                    else if (moveRet == 2)
                    {
                        this.logInfo("城防正在恢复, 等6分钟");
                        return 300100;
                    }
                    else
                    {
                        return 60100;
                    }
                }
                else
                {
                    return attackRet;
                }
            }
            else if (cityEventRet == 1)
            {
                return base.immediate();
            }
            else if (cityEventRet == 2)//完成
            {
                return base.immediate();
            }
            else if (cityEventRet == 3)//完成,移动
            {
                return -1;
            }
            else if (cityEventRet == 4)//完成
            {
                return -2;
            }
            else if (cityEventRet == 5)//大于星级
            {
                return -3;
            }
            else if (cityEventRet == 15)//达到保留次数
            {
                return -2;
            }
            else
            {
                return -4;
            }
		}

        /// <summary>
        /// 攻击悬赏玩家
        /// </summary>
        /// <returns></returns>
		private long attack_cityevent_player()
		{
			BattleMgr battleManager = this._factory.getBattleManager();
			List<ScopeCity> areaScopeInfo = battleManager.getAreaScopeInfo(this._proto, this._logger, this._user, this._user._attack_cityevent_target_areaid, this._user._attack_cityevent_target_scopeid);
			ScopeCity scopeCity = null;
			foreach (ScopeCity current in areaScopeInfo)
			{
				if (current.playerid == this._user._attack_cityevent_targetid)
				{
					scopeCity = current;
					break;
				}
			}
			if (scopeCity == null)
			{
				return base.immediate();
			}
            else if (scopeCity.protectcd > 0)
            {
                return (long)scopeCity.protectcd;
            }
            else
            {
                int worldevent;
                int attackRet = battleManager.attackPlayer(this._proto, this._logger, this._user, this._user._attack_cityevent_target_areaid, this._user._attack_cityevent_target_scopeid, scopeCity.cityid, out worldevent);
                if (worldevent == 2)
                {
                    _factory.getMiscManager().handleTradeFriend(_proto, _logger, _user);
                }
                else if (worldevent == 1)
                {
                    battleManager.AttackSpy(this._proto, this._logger, this._user);
                }
                if (attackRet == 0)
                {
                    return base.immediate();
                }
                else if (attackRet == 4)
                {
                    return 0;
                }
                else if (attackRet < 0)
                {
                    this.logInfo("没有打败敌人, 肿么办?? 木办法, 过5分钟继续打吧, 说不定就...");
                    return base.immediate();
                }
                else
                {
                    return 120000;
                }
            }
		}

        /// <summary>
        /// 攻击天降奇兵
        /// </summary>
        /// <returns></returns>
		private long attack_nationevent_player()
		{
			BattleMgr battleManager = this._factory.getBattleManager();
			int attackRet = battleManager.attackNationDayNpc(this._proto, this._logger, this._user, this._user._attack_nationevent_target_areaid, 0);
			if (attackRet == 0)
			{
				return base.immediate();
			}
			else
			{
				return 120000;
			}
		}

        /// <summary>
        /// 攻击敌对玩家
        /// </summary>
        /// <param name="is_moving"></param>
        /// <returns></returns>
		private long do_normal_attack(bool is_moving)
		{
			BattleMgr battleManager = this._factory.getBattleManager();
			User user = this._user;
			Dictionary<string, string> config = base.getConfig();
			bool attack_npc = config.ContainsKey("attack_npc") && config["attack_npc"].ToLower().Equals("true");
			bool use_token = config.ContainsKey("use_token") && config["use_token"].ToLower().Equals("true");
			bool do_jail_tech = config.ContainsKey("jail_tech") && config["jail_tech"].ToLower().Equals("true");
			bool attack_only_not_injail = config.ContainsKey("not_injail") && config["not_injail"].ToLower().Equals("true");
			if (config.ContainsKey("gongjian"))
			{
				config["gongjian"].ToLower().Equals("true");
			}
			int level_min = 0;
			int level_max = 0;
			int min_score = 10000;
			int attack_reserve_token = 0;
			if (config.ContainsKey("level_min"))
			{
				int.TryParse(config["level_min"], out level_min);
			}
			if (config.ContainsKey("level_max"))
			{
				int.TryParse(config["level_max"], out level_max);
			}
			if (config.ContainsKey("min_score"))
			{
				int.TryParse(config["min_score"], out min_score);
			}
			if (min_score == 0)
			{
				min_score = 10000;
			}
			if (config.ContainsKey("attack_reserve_token"))
			{
				int.TryParse(config["attack_reserve_token"], out attack_reserve_token);
			}
			if (!config.ContainsKey("attack_reserve_token_enable") || !config["attack_reserve_token_enable"].ToLower().Equals("true"))
			{
				attack_reserve_token = 0;
			}
			int attack_filter_type = 0;
			string attack_filter_content = "";
			if (config.ContainsKey("attack_filter_type"))
			{
				int.TryParse(config["attack_filter_type"], out attack_filter_type);
				if (attack_filter_type < 0 || attack_filter_type > 4)
				{
					attack_filter_type = 0;
				}
				if (config.ContainsKey("attack_filter_content"))
				{
					attack_filter_content = config["attack_filter_content"].Trim();
				}
			}
			if (user._attack_battleScore < min_score || user.AttackOrders > attack_reserve_token)
			{
				if (use_token)
				{
					battleManager.useToken(this._proto, this._logger, this._user, min_score);
				}
				long findAndAttackRet = battleManager.find_and_attack(this._proto, this._logger, this._user, attack_reserve_token, attack_only_not_injail, attack_npc, level_min, level_max, attack_filter_type, attack_filter_content);                
				battleManager.doJail(this._proto, this._logger, this._user, do_jail_tech, base.getGoldAvailable());
				if (user.AttackOrders <= attack_reserve_token || user.TokenCdFlag)
				{
					if (is_moving)
					{
						return 60100;
					}
					else
					{
						return base.an_hour_later();
					}
				}
				else
				{
                    if (findAndAttackRet > 60100)
					{
                        findAndAttackRet = 60100;
					}
                    if (findAndAttackRet < 0)
					{
						if (is_moving)
						{
                            return 60100;
						}
						else
						{
							return base.an_hour_later();
						}
					}
                    else if (findAndAttackRet > 0)
                    {
                        return findAndAttackRet;
                    }
                    else
                    {
                        return 60100;
                    }
				}
			}
			else if (is_moving)
			{
                return 60100;
			}
			else
			{
				return base.an_hour_later();
			}
		}

        /// <summary>
        /// 决斗
        /// </summary>
        /// <returns></returns>
        public long do_juedou_attack(int min_level, int max_level)
        {
            BattleMgr battleManager = _factory.getBattleManager();
            PkInfo pkInfo = battleManager.getPkInfo(_proto, _logger);
            if (pkInfo.result < 0)
            {
                return 60000;
            }
            else if (pkInfo.stage == 1)
            {
                if (pkInfo.fang_name == _user.Username)
                {
                    logInfo(string.Format("正在被[{0}]决斗", pkInfo.gong_name));
                    return an_hour_later();
                }
                else
                {
                    return pk(pkInfo);
                }
            }
            else if (_user._attack_daojuFlag > 0)
            {
                AreaInfo areaInfo = battleManager.getNearCapital(_user);
                if (areaInfo == null)
                {
                    logInfo("不在敌国首都外, 不能进行决斗");
                    return next_halfhour();
                }
                else if (_user._attack_cityHpRecoverCd > 0)
                {
                    logInfo("正在补充城防, 不能进行决斗");
                    return _user._attack_cityHpRecoverCd;
                }
                else if (_user._arrest_state == 100)
                {
                    battleManager.escapeFromJail(_proto, _logger);
                    _user._arrest_state = 0;
                    return 60000;
                }
                else if (_user._arrest_state > 10)
                {
                    return 60000;
                }
                else if (_user._attack_cityHp <= 80)
                {
                    logInfo("城防低于80, 取消决斗");
                    return 300000;
                }
                long daojuRet = battleManager.handleDaoju(_proto, _logger, _user, areaInfo.areaid, 2, min_level, max_level, _otherConf.BlackList);
                if (daojuRet == 0)
                {
                    daojuRet = battleManager.handleDaoju(_proto, _logger, _user, areaInfo.areaid, 1, min_level, max_level, _otherConf.BlackList);
                    if (daojuRet == 0)
                    {
                        pkInfo = battleManager.getPkInfo(_proto, _logger);
                        if (pkInfo.result < 0)
                        {
                            return immediate();
                        }
                        else if (pkInfo.stage == 1)
                        {
                            if (pkInfo.fang_maxcityhp < 350)
                            {
                                logInfo(string.Format("{0}最大城防低于350, 加入黑名单", pkInfo.fang_name));
                                _logger.logSurprise(string.Format("{0}最大城防低于350, 加入黑名单", pkInfo.fang_name));
                                //_juedou_blacklist.Add(pkInfo.fang_name);
                                //saveBlackList();
                                _otherConf.addBlackList(pkInfo.fang_name);
                            }
                            return pk(pkInfo);
                        }
                    }
                }
                if (daojuRet == -1)
                {
                    return an_hour_later();
                }
                else if (daojuRet == -2 || daojuRet == -4)
                {
                    return 300000;
                }
                else if (daojuRet == -3 || daojuRet == -6)
                {
                    return 300000;
                }
                else if (daojuRet == -5 || daojuRet == -8)
                {
                    return 60000;
                }
                else if (daojuRet == -7)
                {
                    return 120000;
                }
            }
            return an_hour_later();
        }

        private long pk(PkInfo pkInfo)
        {
            Dictionary<string, string> config = base.getConfig();
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
                battleManager.attackPlayer(_proto, _logger, _user, pkInfo.areaId, pkInfo.scopeid, pkInfo.cityid, out worldevent);
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
            return 60000;
        }

        /// <summary>
        /// 计算目标移动地区
        /// </summary>
        /// <returns></returns>
		private AreaInfo calcTargetMoveArea()
		{
			Dictionary<string, string> config = base.getConfig();
			string areaName = "";
			if (config.ContainsKey("move_target"))
			{
				areaName = config["move_target"];
			}
            if (this._user._attack_spy_city != null && this._user._attack_spy_city.Length > 0)
            {
                this.logInfo(string.Format("发现间谍, 间谍目标为[{0}], 将忽略设置的移动目标", this._user._attack_spy_city));
                areaName = this._user._attack_spy_city;
            }
			else if (this._user._attack_nation_battle_city != null && this._user._attack_nation_battle_city.Length > 0)
			{
				this.logInfo(string.Format("目前处于攻坚战集结期间, 集结目标为[{0}], 将忽略设置的移动目标", this._user._attack_nation_battle_city));
				areaName = this._user._attack_nation_battle_city;
			}
			AreaInfo areaInfo = this._user.getAreaByName(areaName);
			int areaid = 0;
			if (areaInfo != null)
			{
				areaid = areaInfo.areaid;
			}
			int nationInt = this._user.NationInt;
			if (areaid == 0)
			{
				if (nationInt == 1)
				{
                    areaid = 108;//剑阁
				}
                else if (nationInt == 2)
                {
                    areaid = 117;//隆中
                }
                else if (nationInt == 3)
                {
                    areaid = 125;//涪陵
                }
			}
			if (this._user._attack_selfCityId == areaid)
			{
				return null;
			}
			else
			{
				if (areaInfo == null)
				{
					areaInfo = this._user.getAreaById(areaid);
				}
				return areaInfo;
			}
		}

		public override long execute()
		{
			BattleMgr battleManager = this._factory.getBattleManager();
			Dictionary<string, string> config = base.getConfig();
            if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))//没有开启功能
			{
				return base.an_hour_later();
			}
            bool city_event = config.ContainsKey("city_event") && config["city_event"].ToLower().Equals("true");
            bool nation_event = config.ContainsKey("nation_event") && config["nation_event"].ToLower().Equals("true");
            bool gongjian = config.ContainsKey("gongjian") && config["gongjian"].ToLower().Equals("true");
            bool enable_attack = config.ContainsKey("enable_attack") && config["enable_attack"].ToLower().Equals("true");
            bool get_extra_order = config.ContainsKey("get_extra_order") && config["get_extra_order"].ToLower().Equals("true");
            bool attack_npc = config.ContainsKey("attack_npc");
            if (attack_npc)
            {
                config["attack_npc"].ToLower().Equals("true");
            }
            if (config.ContainsKey("use_token"))
            {
                config["use_token"].ToLower().Equals("true");
            }
            bool auto_move = config.ContainsKey("auto_move") && config["auto_move"].ToLower().Equals("true");
            bool do_jail_tech = config.ContainsKey("jail_tech") && config["jail_tech"].ToLower().Equals("true");
            bool not_injail = config.ContainsKey("not_injail");
            if (not_injail)
            {
                config["not_injail"].ToLower().Equals("true");
            }
            int min_level = 0;
            int max_level = 0;
            int min_score = 10000;
            int attack_reserve_token = 0;
            if (config.ContainsKey("level_min"))
            {
                int.TryParse(config["level_min"], out min_level);
            }
            if (config.ContainsKey("level_max"))
            {
                int.TryParse(config["level_max"], out max_level);
            }
            if (config.ContainsKey("min_score"))
            {
                int.TryParse(config["min_score"], out min_score);
            }
            if (min_score == 0)
            {
                min_score = 10000;
            }
            if (config.ContainsKey("attack_reserve_token"))
            {
                int.TryParse(config["attack_reserve_token"], out attack_reserve_token);
            }
            if (!config.ContainsKey("attack_reserve_token_enable") || !config["attack_reserve_token_enable"].ToLower().Equals("true"))
            {
                attack_reserve_token = 0;
            }
            if (config.ContainsKey("move_target"))
            {
                string move_target = config["move_target"];
            }
            bool auto_juedou = config.ContainsKey(ConfigStrings.juedou) && config[ConfigStrings.juedou].ToLower().Equals("true");
            if (!this._user._inNewArea)//不在新区
            {
                return base.an_hour_later();
            }
            else if (this._user._attack_selfCityId == 0)//所在地区未加载
            {
                return base.an_hour_later();
            }

            //加载新区信息
            battleManager.getNewAreaInfo(this._proto, this._logger, this._user);

            //加载个人令信息
            battleManager.getUserTokens(this._proto, this._logger, this._user);

            if (this._user._attack_cityHpRecoverCd > 0)//补充城防
            {
                return this._user._attack_cityHpRecoverCd;
            }

            //杂七杂八
            long misc_exetime = 3600000;//一个小时
            if (this._factory.TmrMgr.TimeStamp >= this._next_misc_exetime)
            {
                //领取攻击令
                if (get_extra_order)
                {
                    battleManager.handleTransferInfo(this._proto, this._logger, this._user);
                }
                //技术研究
                if (this._user.Level >= User.Level_Jail)
                {
                    battleManager.getJailInfo(this._proto, this._logger, this._user, do_jail_tech, base.getGoldAvailable());
                }
                //获取战绩信息
                battleManager.getBattleScoreInfo(this._proto, this._logger, this._user);
                //领取战绩奖励
                for (int i = 0; i < this._user._attack_battleScore_awardGot.Length && this._user._attack_battleScore_awardGot[i] != 0; i++)
                {
                    if (this._user._attack_battleScore_awardGot[i] == 1)
                    {
                        battleManager.getBattleScoreAward(this._proto, this._logger, i + 1);
                    }
                }
                //领取排名奖励
                if (this._user._attack_last_awardGot == 1)
                {
                    battleManager.getRankAward(_proto, _logger);
                }
                //领取战绩宝箱
                if (this._user._attack_battleScore_box > 0)
                {
                    battleManager.openScoreBox(this._proto, this._logger, this._user._attack_battleScore_box);
                }
                this._next_misc_exetime = this._factory.TmrMgr.TimeStamp + misc_exetime;
            }
            else
            {
                misc_exetime = this._next_misc_exetime - this._factory.TmrMgr.TimeStamp;
            }

            //搜索间谍
            battleManager.AttackSpy(this._proto, this._logger, this._user);

            //天降奇兵
            bool is_doing_nation = false;
            if (nation_event)
            {
                long nationRet = this.do_nationevent();
                if (nationRet > 0)
                {
                    return nationRet;
                }
                else if (nationRet == -5)
                {
                    is_doing_nation = true;
                }
            }

            //悬赏
            bool is_doing_cityevent = false;
            long city_exetime = 300000;//5分钟
            if (city_event && this._user.AttackOrders > 0)//悬赏
            {
                if (this._factory.TmrMgr.TimeStamp >= this._next_cityevent_exetime)
                {
                    long cityRet = this.do_cityevent(is_doing_nation);
                    if (cityRet == -1)
                    {
                        is_doing_cityevent = true;
                        city_exetime = 600000;
                    }
                    else if (cityRet == -2)
                    {
                        city_exetime = 600000;
                    }
                    else if (cityRet == -3)
                    {
                        is_doing_cityevent = true;
                        city_exetime = 600000;
                    }
                    else
                    {
                        city_exetime = cityRet;
                    }
                    this._next_cityevent_exetime = this._factory.TmrMgr.TimeStamp + city_exetime;
                }
                else
                {
                    city_exetime = this._next_cityevent_exetime - this._factory.TmrMgr.TimeStamp;
                }
            }
            if (this._cityevent_remain_time > 0)
            {
                is_doing_cityevent = true;
            }

            //攻坚战
            long gongjian_exetime = 300000;//5分钟
            this._user._attack_gongjian_status = -1;
            if (gongjian)
            {
                if (this._factory.TmrMgr.TimeStamp >= this._next_gongjian_exetime)
                {
                    battleManager.getNationBattleInfo(this._proto, this._logger, this._user);
                    if (this._user._attack_gongjian_status == 3)
                    {
                        battleManager.getNationBattleReward(this._proto, this._logger);//攻坚奖励
                        battleManager.getNationBattleInfo(this._proto, this._logger, this._user);//攻坚战信息
                        gongjian_exetime = base.next_gongjian_time();
                    }
                    else if (this._user._attack_gongjian_status == 0)
                    {
                        gongjian_exetime = base.next_gongjian_time();
                    }
                    else if (this._user._attack_gongjian_status == 1)
                    {
                        AreaInfo areaByName = this._user.getAreaByName(this._user._attack_nation_battle_city);
                        if (areaByName == null)
                        {
                            gongjian_exetime = 60000;
                        }
                        else if (this._user._attack_selfCityId == areaByName.areaid)
                        {
                            gongjian_exetime = this._user._attack_nationBattleRemainTime;
                        }
                        else
                        {
                            gongjian_exetime = base.immediate();
                        }
                    }
                    else
                    {
                        gongjian_exetime = this._user._attack_nationBattleRemainTime;
                    }
                    this._next_gongjian_exetime = this._factory.TmrMgr.TimeStamp + gongjian_exetime;
                }
                else
                {
                    gongjian_exetime = this._next_gongjian_exetime - this._factory.TmrMgr.TimeStamp;
                }
            }
            else
            {
                this._user._attack_nation_battle_city = "";
                this._user._attack_nationBattleRemainTime = base.next_gongjian_time();
            }

            //自动移动
            AreaInfo areaInfo = null;
            long transfer_cd;
            if (this._factory.TmrMgr.TimeStamp >= this._next_move_exetime)
            {
                if (auto_move)
                {
                    areaInfo = this.calcTargetMoveArea();
                    if (areaInfo != null)
                    {
                        areaInfo = battleManager.getNextMoveArea(this._proto, this._logger, this._user, areaInfo, is_doing_cityevent, is_doing_nation);
                    }
                }
                if (this._user._attack_transfer_cd > 0)
                {
                    transfer_cd = this._user._attack_transfer_cd;
                }
                else if (areaInfo != null)
                {
                    int moveRet = battleManager.moveToArea(this._proto, this._logger, this._user, areaInfo.areaid);
                    if (moveRet == 0)
                    {
                        transfer_cd = 60000;
                    }
                    else if (moveRet == 2)
                    {
                        transfer_cd = 600000;
                    }
                    else
                    {
                        transfer_cd = 60000;
                    }
                }
                else
                {
                    transfer_cd = 1800000;
                }
                this._next_move_exetime = this._factory.TmrMgr.TimeStamp + transfer_cd;
            }
            else
            {
                transfer_cd = this._next_move_exetime - this._factory.TmrMgr.TimeStamp;
            }

            //决斗
            long juedou_exetime = 300000;//5分钟
            if (auto_juedou)
            {
                if (this._factory.TmrMgr.TimeStamp >= this._next_daoju_exetime)
                {
                    juedou_exetime = do_juedou_attack(min_level, max_level);
                    this._next_daoju_exetime = this._factory.TmrMgr.TimeStamp + juedou_exetime;
                }
                else
                {
                    juedou_exetime = this._next_daoju_exetime - this._factory.TmrMgr.TimeStamp;
                }
            }

            //普通攻击
            long attack_exetime = 300000;//5分钟
            bool is_moving = areaInfo != null;
            if (enable_attack)
            {
                if (this._factory.TmrMgr.TimeStamp >= this._next_attack_exetime)
                {
                    attack_exetime = this.do_normal_attack(is_moving);
                    this._next_attack_exetime = this._factory.TmrMgr.TimeStamp + attack_exetime;
                }
                else
                {
                    attack_exetime = this._next_attack_exetime - this._factory.TmrMgr.TimeStamp;
                }
            }

            return base.smallest_time(new List<long>
							{
								misc_exetime,
								city_exetime,
								transfer_cd,
								gongjian_exetime,
								attack_exetime,
                                juedou_exetime
							});
		}

        private void loadBlackList()
        {
            _logger.logSurprise("加载决斗黑名单");
            foreach (string item in _otherConf.BlackList)
            {
                _logger.logSurprise(item);
            }
        }
	}
}
