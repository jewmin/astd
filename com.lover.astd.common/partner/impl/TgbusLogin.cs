using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class TgbusLogin : LoginImplBase
	{
		public TgbusLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = string.Format("http://pay.tgbus.com/api/userlogin.ashx?u={0}&p={1}&fn=LOGINSTATE&r=0&rand={2}", this._username, this._password, this._rand.NextDouble());
			HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
			bool flag = httpResult == null;
			LoginResult result;
			if (flag)
			{
				loginResult.StatusCode = LoginStatusCode.FailInLogin;
				result = loginResult;
			}
			else
			{
				string content = httpResult.getContent();
				bool flag2 = content.IndexOf("LOGINSTATE(1)") >= 0;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = "http://pay.tgbus.com/api/game/8/servers.aspx";
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					string content2 = httpResult.getContent();
					string arg = string.Format("双线{0}服", this._acc.ServerId);
					Regex regex = new Regex(string.Format("<a.*onclick=\"goServer\\('([^\"']*?)'\\)\".*?>{0}.*</a>", arg));
					Match match = regex.Match(content2);
					bool flag3 = match == null || match.Groups == null || match.Groups.Count < 2;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
						result = loginResult;
					}
					else
					{
						string text = match.Groups[1].Value;
						match = Regex.Match(text, "^.*appid=(\\d+).*$");
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							text = string.Format("http://pay.tgbus.com/api/game/8/gamelogin.ashx?appid={0}", match.Groups[1].Value);
							httpResult = TransferMgr.doGetPure(text, ref cookies, "", null);
							bool flag5 = httpResult == null;
							if (flag5)
							{
								loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
								result = loginResult;
							}
							else
							{
								string text2 = httpResult.getExtraHeader("Location");
								text2 = "http://pay.tgbus.com/api/game/8/" + text2;
								httpResult = TransferMgr.doGetPure(text2, ref cookies, "", null);
								bool flag6 = httpResult == null;
								if (flag6)
								{
									loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
									result = loginResult;
								}
								else
								{
									content2 = httpResult.getContent();
									match = Regex.Match(content2, ".*?iframe.src.*?=.*?['\"]+(.*?)['\"]+.*");
									bool flag7 = match == null || match.Groups == null || match.Groups.Count < 2;
									if (flag7)
									{
										loginResult.StatusCode = LoginStatusCode.FailInGotoGameUrl;
										result = loginResult;
									}
									else
									{
										text2 = match.Groups[1].Value;
										base.processStartGame(text2, loginResult, ref cookies, "");
										result = loginResult;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
	}
}
