using System;

namespace com.lover.astd.common.model.battle
{
	public class CampaignItem
	{
		public int id;

		public string name;

		public int armynum;

		public int times;

		public int remain6num;

		public int ID
		{
			get
			{
				return this.id;
			}
		}

		public string Name
		{
			get
			{
				return string.Format("{0} (已打{1}次)", this.name, this.times);
			}
		}
	}
}
