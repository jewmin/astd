using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.temp
{
	public class EquipMeltExe : TempExeBase, ITempExe, IExecute
	{
		private int _equipId;

		private bool _finished;

		private Equipment _eqp;

		private int _equipLevel = -1;

		private int _equipMh = -1;

		public EquipMeltExe()
		{
			this._name = "temp";
			this._readable = "临时任务";
		}

		public override long execute()
		{
			EquipMgr equipManager = this._factory.getEquipManager();
			bool flag = this._equipLevel < 0;
			long result;
			if (flag)
			{
				List<Equipment> storeCanMelt = equipManager.getStoreCanMelt(this._proto, this._logger, this._user);
				foreach (Equipment current in storeCanMelt)
				{
					bool flag2 = current.Id == this._equipId;
					if (flag2)
					{
						this._eqp = current;
						break;
					}
				}
				bool flag3 = this._eqp == null;
				if (flag3)
				{
					this._finished = true;
					result = base.immediate();
					return result;
				}
				this._finished = false;
				this._equipLevel = this._eqp.Level;
				this._equipMh = this._eqp.EnchantLevel;
			}
			bool flag4 = this._equipLevel > 1;
			if (flag4)
			{
				while (this._equipLevel > 1)
				{
					int num = equipManager.degradeEquip(this._proto, this._logger, this._equipId);
					bool flag5 = num > 0;
					if (flag5)
					{
						this._finished = true;
						result = base.immediate();
						return result;
					}
					this._eqp.Level = this._eqp.Level - 1;
					int num2 = this._equipLevel;
					this._equipLevel = num2 - 1;
				}
			}
			else
			{
				bool flag6 = this._equipMh > 0;
				if (flag6)
				{
					while (this._equipMh > 0)
					{
						int num3 = equipManager.degradeMh(this._proto, this._logger, this._equipId);
						bool flag7 = num3 > 0;
						if (flag7)
						{
							this._finished = true;
							result = base.immediate();
							return result;
						}
						this._eqp.EnchantLevel = this._eqp.EnchantLevel - 1;
						int num2 = this._equipMh;
						this._equipMh = num2 - 1;
					}
				}
				else
				{
					bool flag8 = this._eqp.Quality >= EquipmentQuality.Yellow;
					if (flag8)
					{
						int magic = this._user.Magic;
						int num4 = equipManager.meltEquip(this._proto, this._logger, this._equipId, magic);
						bool flag9 = num4 > 0;
						if (flag9)
						{
							this._finished = true;
							result = base.immediate();
							return result;
						}
					}
					else
					{
						int num5 = equipManager.sellEquip(this._proto, this._logger, this._equipId);
						bool flag10 = num5 > 0;
						if (flag10)
						{
							this._finished = true;
							result = base.immediate();
							return result;
						}
					}
					this._finished = true;
				}
			}
			result = base.immediate();
			return result;
		}

		public bool isFinished()
		{
			return this._finished;
		}

		public void setTarget(Dictionary<string, string> conf)
		{
			bool flag = conf == null || !conf.ContainsKey("equipId");
			if (!flag)
			{
				int.TryParse(conf["equipId"], out this._equipId);
			}
		}

		public string getStatus()
		{
			bool flag = !this._finished;
			string result;
			if (flag)
			{
				result = "降级/融化进行中";
			}
			else
			{
				result = "完成";
			}
			return result;
		}
	}
}
