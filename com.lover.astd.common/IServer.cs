using com.lover.astd.common.model.enumer;
using System;

namespace com.lover.astd.common
{
	public interface IServer
	{
		void init_completed();

		void startServer();

		void stopServer(bool is_user_operate = false);

		void notifyState(AccountStatus status);

		void startReLoginTimer();

		void refreshPlayerSafe();

		void notifySingleExe(string exe_name);
	}
}
