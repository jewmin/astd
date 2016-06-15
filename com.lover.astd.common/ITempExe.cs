using System;
using System.Collections.Generic;

namespace com.lover.astd.common
{
	public interface ITempExe : IExecute
	{
		bool isFinished();

		void setTarget(Dictionary<string, string> conf);

		string getStatus();
	}
}
