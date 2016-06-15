using com.lover.astd.common.logic;
using com.lover.astd.common.model.enumer;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.economy
{
	public class ImposeExe : ExeBase
	{
		public ImposeExe()
		{
			this._name = "impose";
			this._readable = "征收";
		}

		public override long execute()
		{
			Dictionary<string, string> config = base.getConfig();
			bool flag = !config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true");
			long result;
			if (flag)
			{
				result = base.next_day();
			}
			else
			{
				MiscMgr miscManager = this._factory.getMiscManager();
				bool flag2 = this._user.isActivityRunning(ActivityType.ImposeEvent);
				if (flag2)
				{
					int imposeEventInfo = miscManager.getImposeEventInfo(this._proto, this._logger, this._user);
					this._user._impose_isImposeEvent = (imposeEventInfo == 0);
				}
				bool flag3 = this._user.isActivityRunning(ActivityType.RepayEvent);
				if (flag3)
				{
					int repayEventInfo = miscManager.getRepayEventInfo(this._proto, this._logger, this._user);
					this._user._impose_isRepayEvent = (repayEventInfo == 0);
				}
				bool flag4 = !this._user._impose_isImposeEvent && !this._user._impose_isRepayEvent;
				if (flag4)
				{
					result = this.doNormalImpose(miscManager);
				}
				else
				{
					bool impose_isImposeEvent = this._user._impose_isImposeEvent;
					if (impose_isImposeEvent)
					{
						string[] array = this._user._impose_imposeEvent_rewardStatus.Split(new char[]
						{
							','
						});
						int num;
						for (int i = 0; i < array.Length; i = num + 1)
						{
							bool flag5 = array[i] != null && array[i].Length != 0 && !array[i].Equals("0") && array[i].Equals("1");
							if (flag5)
							{
								miscManager.getImposeEventReward(this._proto, this._logger, i);
							}
							num = i;
						}
					}
					bool impose_isRepayEvent = this._user._impose_isRepayEvent;
					if (impose_isRepayEvent)
					{
						string[] array2 = this._user._impose_repayEvent_rewardStatus.Split(new char[]
						{
							','
						});
						int num;
						for (int j = 0; j < array2.Length; j = num + 1)
						{
							bool flag6 = array2[j] != null && array2[j].Length != 0 && !array2[j].Equals("0") && array2[j].Equals("1");
							if (flag6)
							{
								miscManager.getRepayEventReward(this._proto, this._logger, j);
							}
							num = j;
						}
					}
					int min_loyalty = 90;
					bool flag7 = config.ContainsKey("impose_loyalty");
					if (flag7)
					{
						int.TryParse(config["impose_loyalty"], out min_loyalty);
					}
					bool flag8 = (!this._user._impose_isImposeEvent || this._user._impose_imposeEvent_numNow >= this._user._impose_imposeEvent_numMax) && (!this._user._impose_isRepayEvent || this._user._impose_repayEvent_numNow >= this._user._impose_repayEvent_numMax);
					if (flag8)
					{
						result = this.doNormalImpose(miscManager);
					}
					else
					{
						miscManager.getImposeInfo(this._proto, this._logger, this._user, min_loyalty);
						bool flag9 = this._user._impose_count > 0;
						if (flag9)
						{
							bool flag10 = this._user._impose_hasCd && this._user._impose_cdtime > 0L;
							if (flag10)
							{
								result = this._user._impose_cdtime;
							}
							else
							{
								bool flag11 = miscManager.impose(this._proto, this._logger, this._user, false, min_loyalty);
								if (flag11)
								{
									result = base.immediate();
								}
								else
								{
									result = base.next_halfhour();
								}
							}
						}
						else
						{
							int goldAvailable = base.getGoldAvailable();
							int num2 = 0;
							bool flag12 = config.ContainsKey("force_impose_gold");
							if (flag12)
							{
								int.TryParse(config["force_impose_gold"], out num2);
							}
							bool flag13 = this._user._impose_force_gold > num2 || this._user._impose_force_gold > goldAvailable;
							if (flag13)
							{
								result = base.next_day();
							}
							else
							{
								bool flag14 = miscManager.impose(this._proto, this._logger, this._user, true, min_loyalty);
								if (flag14)
								{
									result = base.immediate();
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
			return result;
		}

		private long doNormalImpose(MiscMgr mgr)
		{
			bool flag = this._user.Silver >= this._user.MaxSilver;
			long result;
			if (flag)
			{
				result = base.next_hour();
			}
			else
			{
				Dictionary<string, string> config = base.getConfig();
				int goldAvailable = base.getGoldAvailable();
				int num = 0;
				int num2 = 35;
				int min_loyalty = 90;
				bool flag2 = config.ContainsKey("impose_reserve");
				if (flag2)
				{
					int.TryParse(config["impose_reserve"], out num2);
				}
				bool flag3 = config.ContainsKey("force_impose_gold");
				if (flag3)
				{
					int.TryParse(config["force_impose_gold"], out num);
				}
				bool flag4 = config.ContainsKey("impose_loyalty");
				if (flag4)
				{
					int.TryParse(config["impose_loyalty"], out min_loyalty);
				}
				mgr.getImposeInfo(this._proto, this._logger, this._user, min_loyalty);
				bool flag5 = this._user._impose_count <= num2 && (this._user._impose_force_gold > num || this._user._impose_force_gold > goldAvailable);
				if (flag5)
				{
					result = base.next_day();
				}
				else
				{
					bool flag6 = this._user._impose_hasCd && this._user._impose_cdtime > 0L;
					if (flag6)
					{
						result = this._user._impose_cdtime;
					}
					else
					{
						bool flag7 = this._user._impose_count > num2;
						if (flag7)
						{
							bool flag8 = mgr.impose(this._proto, this._logger, this._user, false, min_loyalty);
							if (flag8)
							{
								result = base.immediate();
							}
							else
							{
								result = base.next_halfhour();
							}
						}
						else
						{
							bool flag9 = this._user._impose_force_gold > num || this._user._impose_force_gold > goldAvailable;
							if (flag9)
							{
								result = base.next_day();
							}
							else
							{
								bool flag10 = mgr.impose(this._proto, this._logger, this._user, true, min_loyalty);
								if (flag10)
								{
									result = base.immediate();
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
			return result;
		}
	}
}
