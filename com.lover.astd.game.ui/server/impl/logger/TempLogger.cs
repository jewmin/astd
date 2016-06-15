using com.lover.astd.common;
using com.lover.astd.common.model;
using System;
using System.Drawing;

namespace com.lover.astd.game.ui.server.impl.logger
{
	internal class TempLogger : ILogger
	{
		private NewMainForm _partner;

		public TempLogger(NewMainForm p)
		{
			_partner = p;
		}

		public void logDebug(string text)
		{
			_partner.LogTemp(text, LogLevel.Debug, Color.Black);
		}

		public void logError(string text)
		{
			_partner.LogTemp(text, LogLevel.Error, Color.Black);
		}

		public void log(string text, Color color)
		{
			_partner.LogTemp(text, LogLevel.Info, Color.Black);
		}

		public void logSingle(string text)
		{

		}

		public void logSurprise(string text)
		{
			throw new NotImplementedException();
		}

		public void logTemp(string text)
		{
			throw new NotImplementedException();
		}
	}
}
