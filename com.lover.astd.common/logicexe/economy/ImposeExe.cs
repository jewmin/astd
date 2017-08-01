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
            if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
            {
                return base.next_day();
            }

            MiscMgr miscManager = this._factory.getMiscManager();
            if (this._user.isActivityRunning(ActivityType.ImposeEvent))
            {
                int imposeEventInfo = miscManager.getImposeEventInfo(this._proto, this._logger, this._user);
                this._user._impose_isImposeEvent = (imposeEventInfo == 0);
            }
            if (this._user.isActivityRunning(ActivityType.RepayEvent))
            {
                int repayEventInfo = miscManager.getRepayEventInfo(this._proto, this._logger, this._user);
                this._user._impose_isRepayEvent = (repayEventInfo == 0);
            }
            if (!this._user._impose_isImposeEvent && !this._user._impose_isRepayEvent)
            {
                return this.doNormalImpose(miscManager);
            }

            if (this._user._impose_isImposeEvent)
            {
                string[] array = this._user._impose_imposeEvent_rewardStatus.Split(new char[] { ',' });
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != null && array[i].Length != 0 && !array[i].Equals("0") && array[i].Equals("1"))
                    {
                        miscManager.getImposeEventReward(this._proto, this._logger, i);
                    }
                }
            }
            if (this._user._impose_isRepayEvent)
            {
                string[] array2 = this._user._impose_repayEvent_rewardStatus.Split(new char[] { ',' });
                for (int j = 0; j < array2.Length; j++)
                {
                    if (array2[j] != null && array2[j].Length != 0 && !array2[j].Equals("0") && array2[j].Equals("1"))
                    {
                        miscManager.getRepayEventReward(this._proto, this._logger, j);
                    }
                }
            }

            int min_loyalty = 90;
            if (config.ContainsKey("impose_loyalty"))
            {
                int.TryParse(config["impose_loyalty"], out min_loyalty);
            }
            if ((!this._user._impose_isImposeEvent || this._user._impose_imposeEvent_numNow >= this._user._impose_imposeEvent_numMax) && (!this._user._impose_isRepayEvent || this._user._impose_repayEvent_numNow >= this._user._impose_repayEvent_numMax))
            {
                return this.doNormalImpose(miscManager);
            }

            miscManager.getImposeInfo(this._proto, this._logger, this._user, min_loyalty);
            if (this._user._impose_count > 0)
            {
                if (this._user._impose_hasCd && this._user._impose_cdtime > 0L)
                {
                    return this._user._impose_cdtime;
                }
                else if (miscManager.impose(this._proto, this._logger, this._user, false, min_loyalty))
                {
                    return base.immediate();
                }
                else
                {
                    return base.next_halfhour();
                }
            }

            int goldAvailable = base.getGoldAvailable();
            int force_impose_gold = 0;
            if (config.ContainsKey("force_impose_gold"))
            {
                int.TryParse(config["force_impose_gold"], out force_impose_gold);
            }
            if (this._user._impose_force_gold > force_impose_gold || this._user._impose_force_gold > goldAvailable)
            {
                return base.next_day();
            }
            else if (miscManager.impose(this._proto, this._logger, this._user, true, min_loyalty))
            {
                return base.immediate();
            }
            else
            {
                return base.next_halfhour();
            }
        }

        private long doNormalImpose(MiscMgr mgr)
        {
            if (this._user.Silver >= this._user.MaxSilver)
            {
                return base.next_hour();
            }

            Dictionary<string, string> config = base.getConfig();
            int goldAvailable = base.getGoldAvailable();
            int force_impose_gold = 0;
            int impose_reserve = 35;
            int min_loyalty = 90;
            if (config.ContainsKey("impose_reserve"))
            {
                int.TryParse(config["impose_reserve"], out impose_reserve);
            }
            if (config.ContainsKey("force_impose_gold"))
            {
                int.TryParse(config["force_impose_gold"], out force_impose_gold);
            }
            if (config.ContainsKey("impose_loyalty"))
            {
                int.TryParse(config["impose_loyalty"], out min_loyalty);
            }
            mgr.getImposeInfo(this._proto, this._logger, this._user, min_loyalty);
            if (this._user._impose_force_task_num > 0)
            {
                if (mgr.impose(this._proto, this._logger, this._user, true, min_loyalty))
                {
                    this._user._impose_force_task_num--;
                    this._user._impose_task_num--;
                    return base.immediate();
                }
                else
                {
                    return base.next_halfhour();
                }
            }
            else if (this._user._impose_task_num > 0)
            {
                bool force = false;
                if (this._user._impose_count == 0) force = true;
                if (mgr.impose(this._proto, this._logger, this._user, force, min_loyalty))
                {
                    this._user._impose_task_num--;
                    return base.immediate();
                }
                else
                {
                    return base.next_halfhour();
                }
            }
            if (this._user._impose_count <= impose_reserve && (this._user._impose_force_gold > force_impose_gold || this._user._impose_force_gold > goldAvailable))
            {
                return base.next_day();
            }
            else if (this._user._impose_hasCd && this._user._impose_cdtime > 0L)
            {
                return this._user._impose_cdtime;
            }
            else if (this._user._impose_count > impose_reserve)
            {
                if (mgr.impose(this._proto, this._logger, this._user, false, min_loyalty))
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
                if (this._user._impose_force_gold > force_impose_gold || this._user._impose_force_gold > goldAvailable)
                {
                    return base.next_day();
                }
                else if (mgr.impose(this._proto, this._logger, this._user, true, min_loyalty))
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
}
