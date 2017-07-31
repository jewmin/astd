using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.battle
{
	public class BattleExe : ExeBase
	{
		public BattleExe()
		{
			this._name = "battle";
			this._readable = "征战";
		}

		public override void init_data()
		{
			if (!this._isRunningServer)
			{
				this._factory.getBattleManager().getAllPowerInfo(this._proto, this._logger, this._user);
				this.refreshUi();
			}
		}

		public override long execute()
		{
			BattleMgr battleManager = this._factory.getBattleManager();
			if (this._user.isActivityRunning(ActivityType.WeaponEvent) && !this._user._battle_got_weapon_event_free_token)
			{
				battleManager.getWeaponEventInfo(this._proto, this._logger, this._user);
			}
			int num = this._factory.getTroopManager().makeSureForce(this._proto, this._logger, 0.5);
			long result;
			if (num != 0)
			{
				this.logInfo("征兵失败, 暂停征战至下个半小时");
				result = base.next_halfhour();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
                bool npc_enabled = config.ContainsKey("npc_enabled") && config["npc_enabled"].ToLower().Equals("true");
                int npc_reserve_token = 0;
				if (config.ContainsKey("npc_reserve_token") && config["npc_reserve_token"] != "")
				{
                    int.TryParse(config["npc_reserve_token"], out npc_reserve_token);
				}
				string npc_ids = "";
				if (config.ContainsKey("npc_ids") && config["npc_ids"] != "")
				{
					npc_ids = config["npc_ids"];
				}
				string npc_force_ids = "";
				if (config.ContainsKey("npc_force_ids") && config["npc_force_ids"] != "")
				{
					npc_force_ids = config["npc_force_ids"];
				}
				bool army_enabled = config.ContainsKey("army_enabled") && config["army_enabled"].ToLower().Equals("true");
				int army_reserve_token = 0;
				if (config.ContainsKey("army_reserve_token") && config["army_reserve_token"] != "")
				{
					int.TryParse(config["army_reserve_token"], out army_reserve_token);
				}
				string[] armyIds = new string[0];
				string[] firstArmyIds = new string[0];
				if (config.ContainsKey("army_ids") && config["army_ids"] != "")
				{
					armyIds = config["army_ids"].Split(new char[]{ ',' });
				}
				if (config.ContainsKey("first_army_ids") && config["first_army_ids"] != "")
				{
					firstArmyIds = config["first_army_ids"].Split(new char[]{ ',' });
				}
				bool attack_army_only_jingyingtime = config.ContainsKey("attack_army_only_jingyingtime") && config["attack_army_only_jingyingtime"].ToLower().Equals("true");
				bool attack_first = config.ContainsKey("attack_first") && config["attack_first"].ToLower().Equals("true");
				int attack_first_interval = 0;
				if (config.ContainsKey("attack_first_interval") && config["attack_first_interval"] != "")
				{
					int.TryParse(config["attack_first_interval"], out attack_first_interval);
				}
				bool enabled = config.ContainsKey("enabled") && config["enabled"].ToLower().Equals("true");
                if (!npc_enabled && !army_enabled && !enabled)
				{
					result = base.next_halfhour();
				}
				else
				{
					string eventId = "";
					if (config.ContainsKey("battle_event_id"))
					{
						eventId = config["battle_event_id"];
					}
					bool do_event = config.ContainsKey("battle_event_enable") && config["battle_event_enable"].ToLower().Equals("true");
					if (this._user.TokenCdFlag && this._user.TokenCdTime > 2000L)
					{
						result = this._user.TokenCdTime;
					}
					else
					{
						if (this._user.Token <= 0)
						{
							result = base.next_hour();
						}
						else
						{
							if (this._user._battle_current_army_id != "" && battleManager.tryFindArmy(this._proto, this._logger, this._user, this._user._battle_current_army_id, false) == 0)
							{
								result = base.immediate();
							}
							else
							{
								string formation = battleManager.getFormation(this._proto, this._logger);
								bool flag20 = false;
								int token = this._user.Token;
								if (npc_ids.Length > 0 || npc_force_ids.Length > 0 || armyIds.Length != 0)
								{
									if (army_enabled && armyIds.Length != 0 && token > army_reserve_token && (!attack_army_only_jingyingtime || base.inJingyingArmyTime()))
									{
                                        for (int i = 0; i < armyIds.Length; i++)
										{
                                            string armyId = armyIds[i];
											if (!(armyId == ""))
											{
												string[] array4 = armyId.Split(new char[]{ ':' });
												int num5 = battleManager.tryFindArmy(this._proto, this._logger, this._user, array4[0], false);
												if (num5 == 0)
												{
													flag20 = true;
													break;
												}
												else if (num5 == 1)
												{
													result = 60000L;
													return result;
												}
												else if (num5 == 2)
												{
													if (this._user.TokenCdTime > 0L)
													{
														result = this._user.TokenCdTime;
														return result;
													}
													result = 600000L;
													return result;
												}
                                                else if (num5 == 3)
                                                {
                                                    result = base.next_hour();
                                                    return result;
                                                }
											}
										}
									}
									if (!flag20)
									{
										battleManager.getBattleInfo(this._proto, this._logger, this._user, base.getGoldAvailable(), do_event, eventId);
                                        if (npc_enabled && (npc_ids.Length > 0 || npc_force_ids.Length > 0) && token > npc_reserve_token)
										{
											int num8 = 0;
											string text5 = formation;
											string text6 = "";
											string text7 = "";
											string text8 = "";
											string text9 = "";
											string[] array5 = npc_ids.Split(new char[]{ ':' });
											if (array5.Length >= 2)
											{
												text6 = array5[0];
												text7 = array5[1];
											}
											array5 = npc_force_ids.Split(new char[]{ ':' });
											if (array5.Length >= 2)
											{
												text8 = array5[0];
												text9 = array5[1];
											}
											bool flag33 = this._user._battle_free_force_token > 0;
											string npcid;
											string text10;
											if (flag33 && text8.Length > 0)
											{
												npcid = text8;
												text10 = text9;
											}
											else
											{
												npcid = text6;
												text10 = text7;
											}
											if (!text5.Equals(text10))
											{
												if (battleManager.changeFormation(this._proto, this._logger, text10))
												{
													num8 = battleManager.attackNpc(this._proto, this._logger, this._user, npcid, flag33, text10);
													text5 = text10;
													flag20 = true;
												}
											}
											else
											{
												num8 = battleManager.attackNpc(this._proto, this._logger, this._user, npcid, flag33, text10);
												flag20 = true;
											}
											if (num8 == 2)
											{
												this.changeDefenseFormation(formation);
												result = this._user.TokenCdTime;
												return result;
											}
											if (num8 == 3)
											{
												this.changeDefenseFormation(formation);
												result = base.next_hour();
												return result;
											}
											formation = text5;
										}
									}
								}
								if (flag20)
								{
									result = 25000L;
								}
								else
								{
									this.changeDefenseFormation(formation);
									if (attack_first)
									{
										string[] array6 = firstArmyIds;
										int num7;
										for (int j = 0; j < array6.Length; j = num7 + 1)
										{
											string text11 = array6[j];
											if (!(text11 == ""))
											{
                                                string[] array7 = text11.Split(new char[] { ':' });
												int num9 = battleManager.tryFindArmy(this._proto, this._logger, this._user, array7[0], true);
												if (num9 == 0)
												{
													flag20 = true;
													break;
												}
												if (num9 == 1)
												{
													long num6 = 60000L;
													result = num6;
													return result;
												}
												if (num9 == 2)
												{
													long num6;
													if (this._user.TokenCdTime > 0L)
													{
														num6 = this._user.TokenCdTime;
														result = num6;
														return result;
													}
													num6 = 600000L;
													result = num6;
													return result;
												}
												else
												{
													if (num9 == 3)
													{
														long num6 = base.next_hour();
														result = num6;
														return result;
													}
													if (num9 == 4)
													{
														long num6 = 120000L;
														result = num6;
														return result;
													}
												}
											}
											num7 = j;
										}
										if (flag20)
										{
											result = base.immediate();
										}
										else
										{
											result = (long)(attack_first_interval * 1000);
										}
									}
									else
									{
										if (armyIds.Length == 0)
										{
											result = base.next_halfhour();
										}
										else
										{
											if (!attack_army_only_jingyingtime || base.inJingyingArmyTime())
											{
												result = 60000L;
											}
											else
											{
												result = base.next_halfhour();
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		private void changeDefenseFormation(string current_formation)
		{
			Dictionary<string, string> config = base.getConfig();
			if (config.ContainsKey("defense_enabled") && config["defense_enabled"].ToLower().Equals("true") && config.ContainsKey("defense_formation") && !current_formation.Equals(config["defense_formation"]))
			{
				this._factory.getBattleManager().changeFormation(this._proto, this._logger, config["defense_formation"]);
			}
		}
	}
}
