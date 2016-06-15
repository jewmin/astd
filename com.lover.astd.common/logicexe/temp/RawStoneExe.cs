using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.temp
{
	public class RawStoneExe : TempExeBase, ITempExe, IExecute
	{
		private int _target_amount;

		private int _already_amount;

		private bool _sell_raw_to_sys;

		private bool _finished;

		public RawStoneExe()
		{
			this._name = "temp";
			this._readable = "临时任务";
		}

		public override long execute()
		{
			MiscMgr miscManager = this._factory.getMiscManager();
			int num = 0;
			int num2 = miscManager.handleRawStoneInfo(this._proto, this._logger, base.getSilverAvailable(), this._sell_raw_to_sys, this._target_amount - this._already_amount, out num);
			this._already_amount += num;
			long result;
			if (num2 == 3)
			{
				this._finished = true;
				this.logInfo("已经木有石头了, 将关闭任务");
				result = base.next_hour();
			}
            else if (num2 == 1)
            {
                this.logInfo("网络连接失败, 稍等几秒钟");
                result = 5000L;
            }
            else if (num2 == 2)
            {
                this._finished = true;
                this.logInfo("银币已达到设置下限, 将关闭任务");
                result = base.next_hour();
            }
            else if (num2 == 10)
            {
                this._finished = true;
                this.logInfo("出现错误, 将关闭任务");
                result = base.next_hour();
            }
            else if (this._already_amount >= this._target_amount)
            {
                this._finished = true;
                result = base.next_hour();
            }
            else
            {
                result = base.immediate();
            }
			return result;
		}

		public bool isFinished()
		{
			return this._finished || this._target_amount <= this._already_amount;
		}

		public void setTarget(Dictionary<string, string> conf)
		{
            if (conf == null || !conf.ContainsKey("target_amount") || !conf.ContainsKey("sell_raw_to_sys"))
            {
                return;
            }
            int.TryParse(conf["target_amount"], out this._target_amount);
            this._sell_raw_to_sys = conf["sell_raw_to_sys"].ToLower().Equals("true");
		}

		public string getStatus()
		{
			return string.Format("{0} / {1}", this._already_amount, this._target_amount);
		}
	}
}
