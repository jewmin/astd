using System;

namespace com.lover.common.transfer
{
	public abstract class BaseCommand
	{
		public abstract string toRawString();

		public abstract string toString();
	}
}
