using System;

namespace com.lover.astd.common.model.misc
{
	public class TicketItem
	{
		public int id;

		public int playerlevel;

		public int tickets;

		public string itemname;

		public int itemcount;

        public TicketItem()
        {
            id = 0;
            playerlevel = 0;
            tickets = 0;
            itemname = "";
            itemcount = 0;
        }
	}
}
