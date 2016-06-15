using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using com.lover.astd.common.model.enumer;
using System;

namespace com.lover.astd.common.logicexe.activities
{
	public class JailEventExe : ExeBase
	{
		public JailEventExe()
		{
			this._name = "jail_event";
			this._readable = "典狱活动";
		}

		public override long execute()
		{
			bool flag = this._user.Level < User.Level_Jail;
			long result;
			if (flag)
			{
				result = base.next_hour();
			}
			else
			{
				ActivityMgr activityManager = this._factory.getActivityManager();
				bool flag2 = !this._user.isActivityRunning(ActivityType.JailEvent);
				if (flag2)
				{
					result = base.next_hour();
				}
				else
				{
					int num = activityManager.handleJailEventInfo(this._proto, this._logger, this._user);
					bool flag3 = num == 1;
					if (flag3)
					{
						bool isRunningServer = this._isRunningServer;
						if (isRunningServer)
						{
							result = 60000L;
						}
						else
						{
							result = base.next_hour();
						}
					}
					else
					{
						bool flag4 = num == 10;
						if (flag4)
						{
							this._user.removeActivity(ActivityType.JailEvent);
							result = base.next_day();
						}
						else
						{
							this._user.addActivity(ActivityType.JailEvent);
							result = base.next_hour();
						}
					}
				}
			}
			return result;
		}
	}
}
