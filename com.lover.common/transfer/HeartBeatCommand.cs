using System.Net.Json;
using System;
using System.Text;

namespace com.lover.common.transfer
{
	public class HeartBeatCommand : BaseCommand
	{
		private JsonObjectCollection _jsonObj;

		public HeartBeatCommand()
		{
			this._jsonObj = new JsonObjectCollection();
			this._jsonObj.Add(new JsonNumericValue("CMD_ID", 1000));
		}

		public override string toRawString()
		{
			return this._jsonObj.ToString();
		}

		public override string toString()
		{
			string s = this.toRawString();
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		}
	}
}
