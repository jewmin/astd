using System;

namespace com.lover.common.command
{
	public class CommandID
	{
		public const int Login = 1;

		public const int Register = 2;

		public const int GetUserInfo = 3;

		public const int GetSession = 4;

		public const int PushSession = 5;

		public const int AddAccount = 6;

		public const int RemoveAccount = 7;

		public const int ChangeAccountInfo = 8;

		public const int SuspendSingle = 9;

		public const int ResumeSingle = 10;

		public const int UpdateConfig = 11;

		public const int FeeAccount = 12;

		public const int HeartBeat = 1000;
	}
}
