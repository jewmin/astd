using System;

namespace com.lover.astd.common.model.building
{
	public class BuildingLine
	{
		public int Id;

		public long CdTime;

		public int CdFlag;

		public bool AlreadyCd
		{
			get
			{
				return this.CdTime > 0L && this.CdFlag > 0;
			}
		}
	}
}
