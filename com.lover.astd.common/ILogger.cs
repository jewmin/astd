using System;
using System.Drawing;

namespace com.lover.astd.common
{
	public interface ILogger
	{
		void logDebug(string text);

		void logError(string text);

		void log(string text, Color color);

		void logSingle(string text);

		void logSurprise(string text);
	}
}
