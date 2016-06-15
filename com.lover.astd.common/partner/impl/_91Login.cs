using com.lover.astd.common.manager;
using com.lover.common;
using com.lover.common.http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace com.lover.astd.common.partner.impl
{
	public class _91Login : LoginImplBase
	{
		public _91Login(LoginMgr loginMgr) : base(loginMgr)
		{
		}

		public override LoginResult login(ref List<Cookie> cookies, string verify_code = null, string extra = null)
		{
			LoginResult loginResult = new LoginResult();
			base.logging();
			byte[] bytes = Encoding.UTF8.GetBytes(this._acc.Password);
			int num = bytes.Length;
			byte[] array = new byte[num + 15];
			bytes.CopyTo(array, 0);
			array[num] = 163;
			array[num + 1] = 172;
			array[num + 2] = 161;
			array[num + 3] = 163;
			byte[] bytes2 = Encoding.UTF8.GetBytes("fdjf,jkgfkl");
			bytes2.CopyTo(array, num + 4);
			string text = CommonUtils.generateBytesMd5(array);
			string arg = text.ToLower();
			string url = string.Format("https://aq.91.com/AjaxAction/AC_userlogin.ashx?CallBack=jsonp{0}&siteflag=221&nduseraction=login&txtUserName={1}&txtPassword={2}&checkcode=", CommonUtils.getMSecondsNow(), this._username, arg);
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
				string content = httpResult.getContent(Encoding.GetEncoding("GBK"));
				bool flag2 = !content.Contains("登陆成功");
				if (flag2)
				{
					loginResult.StatusCode = LoginStatusCode.FailInLogin;
					result = loginResult;
				}
				else
				{
					string url2 = string.Format("http://web.91.com/AS/LoginGame.ashx?ServerID={0}&Channel=2&LoginBack=http://as.91.com/", this._acc.ServerId);
					httpResult = TransferMgr.doGetPure(url2, ref cookies, "", null);
					bool flag3 = httpResult == null;
					if (flag3)
					{
						loginResult.StatusCode = LoginStatusCode.FailInGetServerList;
						result = loginResult;
					}
					else
					{
						string content2 = httpResult.getContent();
						Regex regex = new Regex("href=\"([^\"']*?)\"");
						Match match = regex.Match(content2);
						bool flag4 = match == null || match.Groups == null || match.Groups.Count < 2;
						if (flag4)
						{
							loginResult.StatusCode = LoginStatusCode.FailInFindingGameUrl;
							result = loginResult;
						}
						else
						{
							string value = match.Groups[1].Value;
							base.processStartGame(value, loginResult, ref cookies, "");
							result = loginResult;
						}
					}
				}
			}
			return result;
		}
	}
}
