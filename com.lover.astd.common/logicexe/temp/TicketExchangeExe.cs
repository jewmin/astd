using com.lover.astd.common.model;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.temp
{
	public class TicketExchangeExe : TempExeBase, ITempExe, IExecute
	{
        /// <summary>
        /// 兑换物品id
        /// </summary>
		private int _exchangeId;
        /// <summary>
        /// 兑换物品名称
        /// </summary>
		private string _exchangeName = "";
        /// <summary>
        /// 要兑换次数
        /// </summary>
		private int _exchangeCount;
        /// <summary>
        /// 已经兑换次数
        /// </summary>
		private int _countExchanged;
        /// <summary>
        /// 一次兑换上限
        /// </summary>
        private int _oneTimeCount = 1000;
        /// <summary>
        /// 已完成兑换
        /// </summary>
		private bool _finished;
        /// <summary>
        /// 兵器兑换价格
        /// </summary>
		private int _weapon_price = 1000;

		public TicketExchangeExe()
		{
			this._name = "temp";
			this._readable = "临时任务";
		}

        public override long execute()
        {
            if (this._user.Level < 131)
            {
                this.logInfo("您级别小于131, 将关闭临时任务");
                this._finished = true;
                return base.an_hour_later();
            }
            else
            {
                int total_ticket = 0;
                int result = this._factory.getMiscManager().ticketGetInfo(this._proto, this._logger, ref total_ticket);
                if (result == 1)
                {
                    return 60000L;
                }
                else if (result == 10)
                {
                    this.logInfo("获取点券信息出错, 将关闭临时任务");
                    this._finished = true;
                    return base.an_hour_later();
                }
                else if (total_ticket < this._weapon_price)
                {
                    this.logInfo("点券不足, 将关闭临时任务");
                    this._finished = true;
                    return base.an_hour_later();
                }
                else
                {
                    //List<Equipment> weaponInfo = this._factory.getEquipManager().getWeaponInfo(this._proto, this._logger, this._user);
                    //if (weaponInfo == null || weaponInfo.Count == 0)
                    //{
                    //    this.logInfo("未获取到兵器信息, 将关闭临时任务");
                    //    this._finished = true;
                    //    return base.an_hour_later();
                    //}
                    //else
                    //{
                    //    bool find = false;
                    //    foreach (Equipment current in weaponInfo)
                    //    {
                    //        if (current.Name.Equals(this._exchangeName))
                    //        {
                    //            find = true;
                    //            if (!current.CanUpgrade)
                    //            {
                    //                this.logInfo("您要兑换的兵器已经不能再升级, 将关闭临时任务");
                    //                this._finished = true;
                    //                return base.an_hour_later();
                    //            }
                    //        }
                    //    }
                    //    if (!find)
                    //    {
                    //        this.logInfo("您的兵器列表中未找到要兑换的兵器, 将关闭临时任务");
                    //        this._finished = true;
                    //        return base.an_hour_later();
                    //    }
                    //    else
                    //    {
                    //        base.notifySingleExe("store");
                    //        base.notifySingleExe("weapon");
                    //        int storeTotalSize = this._user._storeTotalSize;
                    //        int storeUsedSize = this._user._storeUsedSize;
                    //        int count = this._exchangeCount - this._countExchanged;
                    //        if (count * this._weapon_price > total_ticket)
                    //        {
                    //            count = total_ticket / this._weapon_price;
                    //        }
                    //        if (count * 90 > this._user.Stone)
                    //        {
                    //            count = this._user.Stone / 90;
                    //        }
                    //        this.logInfo(string.Format("点券剩余{0}, 玉石剩余{1}, 将兑换{2}个{3}", total_ticket, this._user.Stone, count, this._exchangeName));
                    //        if (count == 0)
                    //        {
                    //            this.logInfo("已经不应该再兑换物品, 将关闭临时任务");
                    //            this._finished = true;
                    //            return base.an_hour_later();
                    //        }
                    //        else
                    //        {
                    //            result = this._factory.getMiscManager().ticketExchangeWeapon(this._proto, this._logger, this._exchangeId, this._exchangeName, count);
                    //            if (result == 1)
                    //            {
                    //                return 60000L;
                    //            }
                    //            else if (result == 10)
                    //            {
                    //                this.logInfo("兑换物品出错, 将关闭临时任务");
                    //                this._finished = true;
                    //                return base.an_hour_later();
                    //            }
                    //            else
                    //            {
                    //                this._countExchanged += count;
                    //                base.notifySingleExe("weapon");
                    //                return base.immediate();
                    //            }
                    //        }
                    //    }
                    //}
                    int count = this._exchangeCount - this._countExchanged;
                    if (count * this._weapon_price > total_ticket)
                    {
                        count = total_ticket / this._weapon_price;
                    }
                    if (count > this._oneTimeCount)
                    {
                        count = this._oneTimeCount;
                    }
                    this.logInfo(string.Format("点券剩余{0}, 玉石剩余{1}, 将兑换{2}个{3}", total_ticket, this._user.Stone, count, this._exchangeName));
                    if (count == 0)
                    {
                        this.logInfo("已经不应该再兑换物品, 将关闭临时任务");
                        this._finished = true;
                        return base.an_hour_later();
                    }
                    else
                    {
                        result = this._factory.getMiscManager().ticketExchangeWeapon(this._proto, this._logger, this._exchangeId, this._exchangeName, count);
                        if (result == 1)
                        {
                            return 60000L;
                        }
                        else if (result == 10)
                        {
                            this.logInfo("兑换物品出错, 将关闭临时任务");
                            this._finished = true;
                            return base.an_hour_later();
                        }
                        else
                        {
                            this._countExchanged += count;
                            base.notifySingleExe("weapon");
                            return base.immediate();
                        }
                    }
                }
            }
        }

		public bool isFinished()
		{
			return this._finished || this._exchangeCount == this._countExchanged;
		}

		public void setTarget(Dictionary<string, string> conf)
		{
            if (conf == null || !conf.ContainsKey("exchange_id") || !conf.ContainsKey("exchange_id") || !conf.ContainsKey("exchange_name")) return;
            int.TryParse(conf["exchange_id"], out this._exchangeId);
            int.TryParse(conf["exchange_count"], out this._exchangeCount);
            this._exchangeName = conf["exchange_name"];
		}

		public string getStatus()
		{
			return string.Format("{0} / {1}", this._countExchanged, this._exchangeCount);
		}
	}
}
