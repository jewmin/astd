using System.Net.Json;
using System;
using System.Text;

namespace com.lover.common.transfer
{
	public class ConnectorEvent : EventArgs
	{
		private JsonObjectCollection _jsonObj;

		private int _cmdId;

		private bool _cmdSuccess;

		private JsonObjectCollection _body;

		private string _rawResponse;

		public JsonObjectCollection Root
		{
			get
			{
				return this._jsonObj;
			}
		}

		public int CmdId
		{
			get
			{
				return this._cmdId;
			}
		}

		public bool CmdSuccess
		{
			get
			{
				return this._cmdSuccess;
			}
		}

		public JsonObjectCollection CmdBody
		{
			get
			{
				return this._body;
			}
		}

		public ConnectorEvent(string rawResponse)
		{
			this._rawResponse = rawResponse;
			this.generateResponse();
		}

		private void generateResponse()
		{
			string s = Encoding.UTF8.GetString(Convert.FromBase64String(this._rawResponse));
			JsonTextParser jsonTextParser = new JsonTextParser();
			this._jsonObj = (JsonObjectCollection)jsonTextParser.Parse(s);
			JsonNumericValue jsonNumericValue = this._jsonObj["CMD_ID"] as JsonNumericValue;
			this._cmdId = (int)jsonNumericValue.Value;
			JsonBooleanValue jsonBooleanValue = this._jsonObj["STATUS"] as JsonBooleanValue;
			this._cmdSuccess = jsonBooleanValue.Value.Value;
			this._body = (this._jsonObj["CMD_BODY"] as JsonObjectCollection);
		}
	}
}
