using com.lover.astd.common.config;
using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using System;

namespace com.lover.astd.common
{
	public interface IExecute
	{
		bool IsRunningServer();

		void setRunningServer(bool isserver);

		void setName(string name);

		string getName();

		void setReadableName(string rname);

		string getReadableName();

		void setNextExeTime(long timestamp);

		long getNextExeTime();

        void setVariables(ProtocolMgr proto, ILogger logger, IServer server, User u, GameConfig conf, ServiceFactory factory);

        void setOtherConf(OtherConfig conf);

		void refreshConfig(GameConfig conf);

		void init_data();

		long execute();

		void clearTmpVariables();
	}
}
