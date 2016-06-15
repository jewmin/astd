using com.lover.astd.common.logic;
using com.lover.astd.common.model.battle;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.temp
{
	public class CampaignExe : TempExeBase, ITempExe, IExecute
	{
		private int _fromId;

		private int _targetCount;

		private int _doneCount;

		private List<CampaignItem> _campaigns;

		private CampaignItem _todoCampaign;

		public CampaignExe()
		{
			this._name = "temp";
			this._readable = "临时任务";
		}

		public override long execute()
		{
			CampaignMgr campaignManager = this._factory.getCampaignManager();
			bool flag = this._campaigns == null || this._campaigns.Count == 0;
			long result;
			if (flag)
			{
				this._campaigns = campaignManager.getCampaigns(this._proto, this._logger);
				bool flag2 = this._campaigns == null;
				if (flag2)
				{
					this.logInfo("获取战役失败, 将重试, 如果一直出现, 请停止临时任务");
					result = base.immediate();
					return result;
				}
			}
			this._todoCampaign = null;
			foreach (CampaignItem current in this._campaigns)
			{
				bool flag3 = current.id >= this._fromId && current.times < 100;
				if (flag3)
				{
					this._todoCampaign = current;
					break;
				}
			}
			bool flag4 = this._todoCampaign == null;
			if (flag4)
			{
				result = base.next_halfhour();
			}
			else
			{
				int num = campaignManager.startCampaign(this._proto, this._logger, this._todoCampaign);
				bool flag5 = num == 1;
				if (flag5)
				{
					result = base.next_halfhour();
				}
				else
				{
					bool flag6 = num != 2;
					if (flag6)
					{
						bool flag7 = num == 3;
						if (flag7)
						{
							result = this._user.TokenCdTime;
							return result;
						}
						bool flag8 = num == 4;
						if (flag8)
						{
							result = base.next_hour();
							return result;
						}
						bool flag9 = num == 5;
						if (flag9)
						{
							result = base.next_hour();
							return result;
						}
						bool flag10 = num == 6;
						if (flag10)
						{
							result = base.next_hour();
							return result;
						}
						bool flag11 = num == 7;
						if (flag11)
						{
							result = base.next_hour();
							return result;
						}
						bool flag12 = num == 10;
						if (flag12)
						{
							result = base.next_hour();
							return result;
						}
					}
					int doneCount = this._doneCount;
					this._doneCount = doneCount + 1;
					result = base.immediate();
				}
			}
			return result;
		}

		public bool isFinished()
		{
			return this._doneCount >= this._targetCount;
		}

		public void setTarget(Dictionary<string, string> conf)
		{
			bool flag = conf == null || !conf.ContainsKey("count");
			if (!flag)
			{
				int.TryParse(conf["count"], out this._targetCount);
				bool flag2 = conf.ContainsKey("from");
				if (flag2)
				{
					int.TryParse(conf["from"], out this._fromId);
				}
			}
		}

		public string getStatus()
		{
			return string.Format("单人战役 {0}/{1}", this._doneCount, this._targetCount);
		}
	}
}
