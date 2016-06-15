using com.lover.astd.common.logic;
using System;
using System.Collections.Generic;

namespace com.lover.astd.common.logicexe.temp
{
	public class CreditWashExe : TempExeBase, ITempExe, IExecute
	{
		private bool _finished;

		private List<string> _wash_configs = new List<string>();

		private int _wash_id;

		private int _now_index = -1;

		private string _wash_name = "";

		private string _wash_what = "";

		private int[] _now_attrib;

		public CreditWashExe()
		{
			int[] now_attrib = new int[3];
			this._now_attrib = now_attrib;
			this._name = "temp";
			this._readable = "临时任务";
		}

		public override long execute()
		{
			HeroMgr heroManager = this._factory.getHeroManager();
			int num = heroManager.startWash(this._proto, this._logger, this._user, this._wash_id, this._wash_what, false, base.getCreditAvailable(), 20, ref this._now_attrib);
			bool flag = num == 1;
			long result;
			if (flag)
			{
				this._finished = !this.getNextWash();
			}
			else
			{
				bool flag2 = num == 3;
				if (flag2)
				{
					this._finished = false;
					result = base.next_day();
					return result;
				}
			}
			result = base.immediate();
			return result;
		}

		private bool getNextWash()
		{
			bool flag = this._now_index >= this._wash_configs.Count - 1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<string> arg_3C_0 = this._wash_configs;
				int num = this._now_index + 1;
				this._now_index = num;
				string text = arg_3C_0[num];
				string[] array = text.Split(new char[]
				{
					':'
				});
				bool flag2 = array.Length < 3;
				if (flag2)
				{
					result = false;
				}
				else
				{
					this._wash_id = int.Parse(array[0]);
					this._wash_what = array[1];
					this._wash_name = array[2];
					result = true;
				}
			}
			return result;
		}

		public bool isFinished()
		{
			return this._finished;
		}

		public void setTarget(Dictionary<string, string> conf)
		{
			bool flag = conf == null || !conf.ContainsKey("wash_info");
			if (!flag)
			{
				string text = conf["wash_info"];
				string[] array = text.Split(new char[]
				{
					','
				});
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					bool flag2 = array[i] != null && array[i] != "";
					if (flag2)
					{
						this._wash_configs.Add(array[i]);
					}
					int num2 = i;
					i = num2 + 1;
				}
				this.getNextWash();
			}
		}

		public string getStatus()
		{
			bool flag = !this._finished;
			string result;
			if (flag)
			{
				result = string.Format("洗将{0}, 进度[{1}/{2}], 当前:统{3},勇{4},智{5}", new object[]
				{
					this._wash_name,
					this._now_index + 1,
					this._wash_configs.Count,
					this._now_attrib[0],
					this._now_attrib[1],
					this._now_attrib[2]
				});
			}
			else
			{
				result = "完成";
			}
			return result;
		}
	}
}
