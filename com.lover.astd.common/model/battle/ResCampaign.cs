using System;

namespace com.lover.astd.common.model.battle
{
	public class ResCampaign
	{
		public int id;

		public string name;

		public string armies;

		public int status;

		public string reward;

		public string finishreward;

		public string Name
		{
			get
			{
				return string.Format("{0} ({1})", this.name, this.getRewardByNpcIndex(0));
			}
		}

		public string getRewardByNpcIndex(int npcIndex)
		{
			string str = "";
			string[] array = this.reward.Split(new char[]
			{
				';'
			});
			bool flag = array.Length < npcIndex;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				result = str + this.generateReward(array[npcIndex]);
			}
			return result;
		}

		private string generateReward(string rewardString)
		{
			bool flag = rewardString == null || rewardString == "";
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				string[] array = rewardString.Split(new char[]
				{
					':'
				});
				bool flag2 = array.Length < 3;
				if (flag2)
				{
					result = "";
				}
				else
				{
					string a = array[1];
					string arg = array[2];
					string arg2 = "";
					bool flag3 = a == "copper";
					if (flag3)
					{
						arg2 = "银币";
					}
					else
					{
						bool flag4 = a == "baoshi";
						if (flag4)
						{
							arg2 = "宝石";
							bool flag5 = array.Length >= 4;
							if (flag5)
							{
								arg = array[3];
							}
							else
							{
								arg = "<<未知的错误>>";
							}
						}
					}
					result = string.Format("{0}+{1}", arg2, arg);
				}
			}
			return result;
		}
	}
}
