using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using com.lover.astd.common.model.building;
using System;
using System.Collections.Generic;
using com.lover.astd.common.model.enumer;

namespace com.lover.astd.common.logicexe.economy
{
    public class MovableExe : ExeBase
    {
        private int _weave_price = 130;

        private string _refine_weapone_name = "";

        private string _movable_order = "";

        private int _refine_reserve = 10;

        private List<int> _movableSerial = new List<int>();

        public MovableExe()
        {
            this._name = "movable";
            this._readable = "行动力";
        }

        //public override long execute()
        //{
        //    Dictionary<string, string> config = base.getConfig();
        //    long result;
        //    if (!config.ContainsKey("enabled") || !config["enabled"].ToLower().Equals("true"))
        //    {
        //        result = base.next_hour();
        //    }
        //    else
        //    {
        //        if (config.ContainsKey("weave_price"))
        //        {
        //            int.TryParse(config["weave_price"], out this._weave_price);
        //        }
        //        if (config.ContainsKey("refine_reserve"))
        //        {
        //            int.TryParse(config["refine_reserve"], out this._refine_reserve);
        //        }
        //        if (config.ContainsKey("refine_item"))
        //        {
        //            this._refine_weapone_name = config["refine_item"];
        //        }
        //        if (this._refine_weapone_name == null || this._refine_weapone_name == "")
        //        {
        //            this._refine_weapone_name = "无敌将军炮";
        //        }
        //        if (config.ContainsKey("order"))
        //        {
        //            this._movable_order = config["order"];
        //        }
        //        else
        //        {
        //            this._movable_order = "2341";
        //        }
        //        bool winter_trade = config.ContainsKey("winter_trade") && config["winter_trade"].ToLower().Equals("true");
        //        bool arch_trade = false;
        //        bool arch_trade_max = false;
        //        bool arch_refill = false;
        //        if (this._user.isActivityRunning(ActivityType.ArchEvent))
        //        {
        //            arch_trade = (config.ContainsKey("arch_trade") && config["arch_trade"].ToLower().Equals("true"));
        //            arch_trade_max = (config.ContainsKey("arch_trade_max") && config["arch_trade_max"].ToLower().Equals("true"));
        //            arch_refill = (config.ContainsKey("arch_refill") && config["arch_refill"].ToLower().Equals("true"));
        //        }
        //        string visit_merchants = "";
        //        int max_fail_count = 0;
        //        if (config.ContainsKey("trade_visit"))
        //        {
        //            visit_merchants = config["trade_visit"];
        //        }
        //        if (config.ContainsKey("trade_visit_fail"))
        //        {
        //            int.TryParse(config["trade_visit_fail"], out max_fail_count);
        //        }
        //        int reserve = 0;
        //        if (config.ContainsKey("reserve"))
        //        {
        //            int.TryParse(config["reserve"], out reserve);
        //        }
        //        int weave_count = 0;
        //        if (config.ContainsKey("weave_count"))
        //        {
        //            int.TryParse(config["weave_count"], out weave_count);
        //        }
        //        int refine_gold_limit = 0;
        //        if (config.ContainsKey("gold_refine_limit"))
        //        {
        //            int.TryParse(config["gold_refine_limit"], out refine_gold_limit);
        //        }
        //        int refine_stone_limit = 0;
        //        if (config.ContainsKey("stone_refine_limit"))
        //        {
        //            int.TryParse(config["stone_refine_limit"], out refine_stone_limit);
        //        }
        //        bool is_new_trade = this._user._is_new_trade;
        //        MiscMgr miscManager = this._factory.getMiscManager();
        //        bool trade = false;
        //        bool weave = false;
        //        if (this._user.Level >= 40 && ((this._movable_order.Contains("1") || this._movable_order.Contains("5") || (this._user.Season == 4 & winter_trade)) | arch_trade))
        //        {
        //            trade = true;
        //        }
        //        if (this._user.Level >= 82 && (this._movable_order.Contains("2") || this._movable_order.Contains("6")))
        //        {
        //            weave = true;
        //        }
        //        int state = 0;
        //        int weave_state = 0;
        //        if (trade && !is_new_trade)
        //        {
        //            miscManager.handleTradeInfo(this._proto, this._logger, this._user, out state, false, true);
        //        }
        //        if (weave)
        //        {
        //            miscManager.handleWeaveInfo(this._proto, this._logger, this._user, this._weave_price, weave_count, out weave_state, false, true);
        //        }
        //        if (this._user.CurMovable < reserve)
        //        {
        //            this.logInfo("已经达到保留行动力下限, 下个小时再检测");
        //            result = base.next_hour();
        //        }
        //        else
        //        {
        //            if ((this._user.Season == 4 & winter_trade) && !is_new_trade)
        //            {
        //                long num4 = (long)miscManager.handleTradeInfo(this._proto, this._logger, this._user, out state, false, true);
        //            }
        //            if (arch_trade)
        //            {
        //                if (is_new_trade)
        //                {
        //                    int map_count = 0;
        //                    int boxnum = 0;
        //                    long num6 = (long)miscManager.handleNewTradeInfo(this._proto, this._logger, this._user, visit_merchants, max_fail_count, base.getSilverAvailable(), base.getGoldAvailable(), out map_count, out boxnum);
        //                    while (boxnum > 0)
        //                    {
        //                        miscManager.openNewTradeBox(this._proto, this._logger, out boxnum);
        //                    }
        //                    if (map_count == 0 || map_count == 20)
        //                    {
        //                        this._user._arch_have_got_all_pieces = true;
        //                    }
        //                    else
        //                    {
        //                        this._user._arch_have_got_all_pieces = false;
        //                    }
        //                    if (!this._user._arch_have_got_all_pieces)
        //                    {
        //                        if (num6 == 10L)
        //                        {
        //                            result = base.next_halfhour();
        //                            return result;
        //                        }
        //                        if (num6 == 1L && this._isRunningServer)
        //                        {
        //                            result = 60000L;
        //                            return result;
        //                        }
        //                        if (num6 == 2L)
        //                        {
        //                            if (arch_refill && this._user._can_buy_active_count > 0)
        //                            {
        //                                int goldAvailable = base.getGoldAvailable();
        //                                int num7 = (this._user._max_active - this._user.CurMovable) / 10;
        //                                num7 = (3 - this._user._can_buy_active_count + 1) * num7;
        //                                if (goldAvailable >= num7)
        //                                {
        //                                    int num8 = miscManager.refillActive(this._proto, this._logger);
        //                                    if (num8 == 0)
        //                                    {
        //                                        this._user._can_buy_active_count--;
        //                                        result = base.immediate();
        //                                        return result;
        //                                    }
        //                                    if (num8 == 1)
        //                                    {
        //                                        result = 300000L;
        //                                        return result;
        //                                    }
        //                                    result = base.next_hour();
        //                                    return result;
        //                                }
        //                            }
        //                            result = base.next_hour();
        //                            return result;
        //                        }
        //                        if (num6 == 0L)
        //                        {
        //                            result = base.immediate();
        //                            return result;
        //                        }
        //                        result = base.next_halfhour();
        //                        return result;
        //                    }
        //                }
        //                else
        //                {
        //                    if (arch_trade_max)
        //                    {
        //                        long num10 = (long)miscManager.handleTradeInfo(this._proto, this._logger, this._user, out state, false, false);
        //                        if (num10 == 10L)
        //                        {
        //                            result = base.next_halfhour();
        //                            return result;
        //                        }
        //                        bool flag40 = num10 == 1L && this._isRunningServer;
        //                        if (flag40)
        //                        {
        //                            result = 60000L;
        //                            return result;
        //                        }
        //                        bool flag41 = num10 == 3L;
        //                        if (flag41)
        //                        {
        //                            result = base.next_halfhour();
        //                            return result;
        //                        }
        //                        bool flag42 = num10 == 2L && state == 2;
        //                        if (flag42)
        //                        {
        //                            Building building = this._factory.getBuildingManager().getBuilding(this._user._buildings, "驿站");
        //                            bool flag43 = building != null && building.Level < this._user.Level;
        //                            if (flag43)
        //                            {
        //                                bool flag44 = this._factory.getBuildingManager().upgradeBuilding(this._proto, this._logger, building, this._user._buildingLines);
        //                                bool flag45 = flag44;
        //                                if (flag45)
        //                                {
        //                                    this._factory.getMiscManager().getExtraInfo2(this._proto, this._logger, this._user);
        //                                    result = base.immediate();
        //                                    return result;
        //                                }
        //                                this.logInfo("升级驿站失败, 等待半小时");
        //                                result = 1800000L;
        //                                return result;
        //                            }
        //                            else
        //                            {
        //                                num10 = (long)miscManager.handleTradeInfo(this._proto, this._logger, this._user, out state, true, false);
        //                                bool flag46 = num10 == 10L;
        //                                if (flag46)
        //                                {
        //                                    result = base.next_halfhour();
        //                                    return result;
        //                                }
        //                                bool flag47 = num10 == 1L && this._isRunningServer;
        //                                if (flag47)
        //                                {
        //                                    result = 60000L;
        //                                    return result;
        //                                }
        //                                bool flag48 = num10 == 3L;
        //                                if (flag48)
        //                                {
        //                                    result = base.next_halfhour();
        //                                    return result;
        //                                }
        //                            }
        //                        }
        //                        bool flag49 = num10 == 0L;
        //                        if (flag49)
        //                        {
        //                            result = base.immediate();
        //                            return result;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        long num11 = (long)miscManager.handleTradeInfo(this._proto, this._logger, this._user, out state, false, false);
        //                        bool flag50 = num11 == 10L;
        //                        if (flag50)
        //                        {
        //                            result = base.next_halfhour();
        //                            return result;
        //                        }
        //                        bool flag51 = num11 == 1L && this._isRunningServer;
        //                        if (flag51)
        //                        {
        //                            result = 60000L;
        //                            return result;
        //                        }
        //                        bool flag52 = num11 == 3L;
        //                        if (flag52)
        //                        {
        //                            result = base.next_halfhour();
        //                            return result;
        //                        }
        //                        bool flag53 = num11 == 0L;
        //                        if (flag53)
        //                        {
        //                            result = base.immediate();
        //                            return result;
        //                        }
        //                    }
        //                }
        //            }
        //            this._movableSerial.Clear();
        //            int length = this._movable_order.Length;
        //            for (int i = 0; i < length; i++)
        //            {
        //                char c = this._movable_order[i];
        //                if (c.Equals('1'))
        //                {
        //                    if (!this._movableSerial.Contains(1))
        //                    {
        //                        this._movableSerial.Add(1);
        //                    }
        //                }
        //                else if (c.Equals('2'))
        //                {
        //                    if (!this._movableSerial.Contains(2))
        //                    {
        //                        this._movableSerial.Add(2);
        //                    }
        //                }
        //                else if (c.Equals('3'))
        //                {
        //                    if (!this._movableSerial.Contains(3))
        //                    {
        //                        this._movableSerial.Add(3);
        //                    }
        //                }
        //                else if (c.Equals('4'))
        //                {
        //                    if (!this._movableSerial.Contains(4))
        //                    {
        //                        this._movableSerial.Add(4);
        //                    }
        //                }
        //                else if (c.Equals('5'))
        //                {
        //                    if (!this._movableSerial.Contains(5))
        //                    {
        //                        this._movableSerial.Add(5);
        //                    }
        //                }
        //                else if (c.Equals('6'))
        //                {
        //                    if (!this._movableSerial.Contains(6))
        //                    {
        //                        this._movableSerial.Add(6);
        //                    }
        //                }
        //                else if (c.Equals('7'))
        //                {
        //                    if (!this._movableSerial.Contains(7))
        //                    {
        //                        this._movableSerial.Add(7);
        //                    }
        //                }
        //                else if (c.Equals('8'))
        //                {
        //                    if (!this._movableSerial.Contains(8))
        //                    {
        //                        this._movableSerial.Add(8);
        //                    }
        //                }
        //                if (this._movableSerial.Count >= 4)
        //                {
        //                    break;
        //                }
        //            }
        //            foreach (int current in this._movableSerial)
        //            {
        //                if (current == 2)
        //                {
        //                    long num12 = (long)miscManager.handleWeaveInfo(this._proto, this._logger, this._user, this._weave_price, weave_count, out weave_state, false, false);
        //                    if (num12 == 0L)
        //                    {
        //                        result = base.immediate();
        //                        return result;
        //                    }
        //                    if (num12 == 3L)
        //                    {
        //                        result = base.next_halfhour();
        //                        return result;
        //                    }
        //                    if (num12 == 4L)
        //                    {
        //                        result = base.next_halfhour();
        //                        return result;
        //                    }
        //                    if (num12 == 10L)
        //                    {
        //                        result = base.next_halfhour();
        //                        return result;
        //                    }
        //                    if (num12 != 2L && num12 == 1L && this._isRunningServer)
        //                    {
        //                        result = 60000L;
        //                        return result;
        //                    }
        //                }
        //                else if (current == 3)
        //                {
        //                    long num18 = (long)miscManager.handleRefineFactoryInfo(this._proto, this._logger, this._user, refine_gold_limit, refine_stone_limit, base.getGoldAvailable(), base.getStoneAvailable(), this._refine_weapone_name, this._user.Silver, false);
        //                    bool flag77 = num18 == 0L;
        //                    if (flag77)
        //                    {
        //                        long num19 = base.immediate();
        //                        result = num19;
        //                        return result;
        //                    }
        //                    bool flag78 = num18 == 3L;
        //                    if (flag78)
        //                    {
        //                        long num20 = base.next_halfhour();
        //                        result = num20;
        //                        return result;
        //                    }
        //                    bool flag79 = num18 == 4L;
        //                    if (flag79)
        //                    {
        //                        long num21 = base.next_halfhour();
        //                        result = num21;
        //                        return result;
        //                    }
        //                    bool flag80 = num18 == 10L;
        //                    if (flag80)
        //                    {
        //                        long num22 = base.next_halfhour();
        //                        result = num22;
        //                        return result;
        //                    }
        //                    bool flag81 = num18 != 2L && num18 == 1L && this._isRunningServer;
        //                    if (flag81)
        //                    {
        //                        long num23 = 60000L;
        //                        result = num23;
        //                        return result;
        //                    }
        //                }
        //                else if (current == 4)
        //                {
        //                    long num24 = (long)miscManager.handleRefineInfo(this._factory.getEquipManager(), this._proto, this._logger, this._user, base.getGemPrice(), base.getGoldAvailable(), this._refine_reserve, false);
        //                    if (num24 == 0L)
        //                    {
        //                        return base.immediate();
        //                    }
        //                    if (num24 == 3L)
        //                    {
        //                        return base.next_halfhour();
        //                    }
        //                    if (num24 == 4L)
        //                    {
        //                        return 60000L;
        //                    }
        //                    if (num24 == 10L)
        //                    {
        //                        return base.next_halfhour();
        //                    }
        //                    if (num24 != 2L && num24 == 1L && this._isRunningServer)
        //                    {
        //                        return 60000L;
        //                    }
        //                }
        //                else if (current == 5)
        //                {
        //                    long num24 = (long)miscManager.gamblingStone(this._proto, this._logger, this._user);
        //                    if (num24 == 0L)
        //                    {
        //                        return base.immediate();
        //                    }
        //                    if (num24 == 3L)
        //                    {
        //                        return base.next_halfhour();
        //                    }
        //                    if (num24 == 4L)
        //                    {
        //                        return 60000L;
        //                    }
        //                    if (num24 == 10L)
        //                    {
        //                        return base.next_halfhour();
        //                    }
        //                    if (num24 != 2L && num24 == 1L && this._isRunningServer)
        //                    {
        //                        return 60000L;
        //                    }
        //                }
        //                else if (current == 1)
        //                {
        //                    if (is_new_trade)
        //                    {
        //                        int map_count = 0;
        //                        int boxnum = 0;
        //                        long num31 = (long)miscManager.handleNewTradeInfo(this._proto, this._logger, this._user, visit_merchants, max_fail_count, base.getSilverAvailable(), base.getGoldAvailable(), out map_count, out boxnum);
        //                        while (boxnum > 0)
        //                        {
        //                            miscManager.openNewTradeBox(this._proto, this._logger, out boxnum);
        //                        }
        //                        if (num31 == 10L)
        //                        {
        //                            result = base.next_halfhour();
        //                            return result;
        //                        }
        //                        if (num31 == 1L && this._isRunningServer)
        //                        {
        //                            result = 60000L;
        //                            return result;
        //                        }
        //                        if (num31 == 2L)
        //                        {
        //                            result = base.next_hour();
        //                            return result;
        //                        }
        //                        if (num31 == 0L)
        //                        {
        //                            result = base.immediate();
        //                            return result;
        //                        }
        //                        result = base.next_halfhour();
        //                        return result;
        //                    }
        //                    else
        //                    {
        //                        long num33 = (long)miscManager.handleTradeInfo(this._proto, this._logger, this._user, out state, false, false);
        //                        if (num33 == 0L)
        //                        {
        //                            result = base.immediate();
        //                            return result;
        //                        }
        //                        if (num33 == 3L)
        //                        {
        //                            result = base.next_halfhour();
        //                            return result;
        //                        }
        //                        if (num33 == 10L)
        //                        {
        //                            result = base.next_halfhour();
        //                            return result;
        //                        }
        //                        if (num33 != 2L && num33 == 1L && this._isRunningServer)
        //                        {
        //                            result = 60000L;
        //                            return result;
        //                        }
        //                    }
        //                }
        //            }
        //            result = base.next_hour();
        //        }
        //    }
        //    return result;
        //}

        public override long execute()
        {
            object[] result = this._lua.CallFunction("movable_execute");
            if (result != null)
            {
                if (result[0].GetType().IsEnum)
                {
                    return getTimeByExeCode((ExeCode)result[0]);
                }
                return (long)result[0];
            }
            return this.next_hour();
        }
    }
}
