using System;
using System.IO;
using System.Net;
using System.Text;

namespace com.lover.common
{
	public class HttpUtils
	{
		private static HttpUtils _instance = new HttpUtils();

		public static HttpUtils getInstance()
		{
			return HttpUtils._instance;
		}

		private HttpUtils()
		{
		}

		public string doGet(string url)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			httpWebRequest.Method = "GET";
			HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
			bool flag = httpWebResponse.StatusCode != HttpStatusCode.OK;
			string result;
			if (flag)
			{
				UiUtils.getInstance().error("请求失败, 请重试或联系管理员, code=" + httpWebResponse.StatusCode.ToString());
				result = null;
			}
			else
			{
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream);
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
				result = text;
			}
			return result;
		}

		public string doPost(string url, string data)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(data);
				requestStream.Write(bytes, 0, bytes.Length);
			}
			HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
			bool flag = httpWebResponse.StatusCode != HttpStatusCode.OK;
			string result;
			if (flag)
			{
				UiUtils.getInstance().error("请求失败, 请重试或联系管理员, code=" + httpWebResponse.StatusCode.ToString());
				result = null;
			}
			else
			{
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream);
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
				result = text;
			}
			return result;
		}
	}
}
