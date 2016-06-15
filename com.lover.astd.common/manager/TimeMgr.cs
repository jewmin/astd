using System;

namespace com.lover.astd.common.manager
{
	public class TimeMgr
	{
		private long _timeSpan;

		public long TimeStamp
		{
			get
			{
				return decimal.ToInt64(decimal.Divide(DateTime.Now.Ticks - 621355968000000000L, 10000m)) - this._timeSpan;
			}
			set
			{
				this._timeSpan = decimal.ToInt64(decimal.Divide(DateTime.Now.Ticks - 621355968000000000L, 10000m)) - value;
			}
		}

		public DateTime DateTimeNow
		{
			get
			{
				return new DateTime(this.TimeStamp * 10000L + 621355968000000000L);
			}
		}
	}
}
