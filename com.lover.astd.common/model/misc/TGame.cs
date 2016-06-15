using com.lover.common;
using System;

namespace com.lover.astd.common.model.misc
{
	public class TGame
	{
		public int tid;

		public string cardreward;

		public string powername;

		public int coppercost;

		public int open;

		public int maxpos;

		public int pos;

		public string Desc
		{
			get
			{
				return string.Format("{0}-{1}银币-{2}", this.powername, CommonUtils.getShortReadable((long)this.coppercost), this.cardreward);
			}
		}
	}
}
