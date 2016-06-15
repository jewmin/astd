using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class KugouLogin : LoginImplBase
	{
		public KugouLogin(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			string url = string.Format("http://kggapi.kugou.com/index.php?r=NewLogin/Web&callback=loginedCallBack&UserName={0}&UserPwd={1}&SaveCookie=on&loginsource=&checkcode={2}&t={3}&_={4}", new object[]
			{
				this._username,
				this._password,
				verify_code,
				new Random().NextDouble(),
				new Random().NextDouble(),
				CommonUtils.getMSecondsNow()
			});
			HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "http://youxi.kugou.com/", null);
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
				Match match = new Regex("\"code\":(?<code>\\d+),", RegexOptions.IgnoreCase).Match(content);
				bool flag2 = !match.Success || int.Parse(match.Groups["code"].Value) != 1;
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string redirecturl = string.Format("http://games.kugou.com/gotogames.aspx?gameid=49&ServiceID={0}", this._acc.ServerId);
					base.processRedirect(redirecturl, loginResult, ref cookies);
					result = loginResult;
				}
			}
			return result;
		}
	}
}
