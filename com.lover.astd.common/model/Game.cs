using System;

namespace com.lover.astd.common.model
{
	public class Game
	{
		private string _gameId;

		public string GameId
		{
			get
			{
				return this._gameId;
			}
			set
			{
				this._gameId = value;
			}
		}

		public void handleEvent(GameEvent evt)
		{
		}

		public void dispose()
		{
		}
	}
}
