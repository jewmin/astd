using System;

namespace com.lover.astd.common.model.misc
{
	public class KfzbItemConfig
	{
		public int item_type;

		public int max_pos;

		public void fill(string str)
		{
			string[] array = str.Split(new char[]
			{
				':'
			});
			bool flag = array.Length < 2;
			if (!flag)
			{
				int.TryParse(array[0], out this.item_type);
				int.TryParse(array[1], out this.max_pos);
			}
		}
	}
}
