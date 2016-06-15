using com.lover.common.http;
using ComponentAce.Compression.Libs.zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace com.lover.common
{
	public class TransferMgr
	{
		public static string _proxy_host = "";

		public static int _proxy_port = 0;

		public static int _proxy_type = 0;

		public static string doGet(string url, ref List<Cookie> cookies)
		{
			string text = "";
			HttpResult httpResult = TransferMgr.doGetPure(url, ref cookies, "", null);
			string result;
			if (httpResult == null || httpResult.StatusCode == 0)
			{
				text = "code:-1";
				result = text;
			}
            else if (httpResult.StatusCode != 200 && httpResult.StatusCode != 304)
            {
                text = "code:" + httpResult.StatusCode;
                result = text;
            }
            else
            {
                Stream decodedStream = httpResult.getDecodedStream();
                MemoryStream memoryStream = new MemoryStream();
                ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
                try
                {
                    TransferMgr.CopyStream(decodedStream, zOutputStream);
                    memoryStream.Position = 0L;
                    StreamReader streamReader = new StreamReader(memoryStream);
                    text = streamReader.ReadToEnd();
                    int num = text.IndexOf("<results>");
                    if (num < 0)
                    {
                        text = "";
                    }
                    else
                    {
                        text = text.Substring(num);
                    }
                }
                finally
                {
                    decodedStream.Close();
                    memoryStream.Close();
                    zOutputStream.Close();
                }
                result = text;
            }
			return result;
		}

		public static string doPost(string url, string data, ref List<Cookie> cookies)
		{
			string text = "";
			HttpResult httpResult = TransferMgr.doPostPure(url, data, ref cookies, "", null);
			string result;
			if (httpResult == null || httpResult.StatusCode == 0)
			{
				text = "code:-1";
				result = text;
			}
            else if (httpResult.StatusCode != 200 && httpResult.StatusCode != 304)
            {
                text = "code:" + httpResult.StatusCode;
                result = text;
            }
            else
            {
                Stream decodedStream = httpResult.getDecodedStream();
                MemoryStream memoryStream = new MemoryStream();
                ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
                try
                {
                    TransferMgr.CopyStream(decodedStream, zOutputStream);
                    memoryStream.Position = 0L;
                    StreamReader streamReader = new StreamReader(memoryStream);
                    text = streamReader.ReadToEnd();
                    int num = text.IndexOf("<results>");
                    if (num < 0)
                    {
                        text = "";
                    }
                    else
                    {
                        text = text.Substring(num);
                    }
                }
                finally
                {
                    decodedStream.Close();
                    memoryStream.Close();
                    zOutputStream.Close();
                }
                result = text;
            }
			return result;
		}

		public static void CopyStream(Stream input, Stream output)
		{
			byte[] buffer = new byte[2000];
			int count;
			while ((count = input.Read(buffer, 0, 2000)) > 0)
			{
				output.Write(buffer, 0, count);
			}
			output.Flush();
		}

		public static HttpResult doGetPure(string url, ref List<Cookie> cookies, string referer = "", Dictionary<string, string> headers = null)
		{
			HttpResult result;
			try
			{
				bool flag = !url.StartsWith("http://") && !url.StartsWith("https://");
				if (flag)
				{
					url = "http://" + url;
				}
				HttpSocket httpSocket = new HttpSocket();
				httpSocket._proxy_host = TransferMgr._proxy_host;
				httpSocket._proxy_port = TransferMgr._proxy_port;
				httpSocket.Method = "GET";
				httpSocket.Url = url;
				bool flag2 = referer != null && referer != "";
				if (flag2)
				{
					httpSocket.Referer = referer;
				}
				bool flag3 = headers != null && headers.Count > 0;
				if (flag3)
				{
					IEnumerator<string> enumerator = headers.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						httpSocket.addExtraHeader(current, headers[current]);
					}
				}
				httpSocket.setCookies(cookies);
				HttpResult httpResult = httpSocket.execute();
				bool flag4 = cookies != null;
				if (flag4)
				{
					List<Cookie> cookies2 = httpResult.getCookies();
					CommonUtils.updateCookies(ref cookies, ref cookies2);
				}
				result = httpResult;
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("ERROR:{0}::{1}", ex.Message, ex.StackTrace));
			}
			result = null;
			return result;
		}

		public static HttpResult doPostPure(string url, string data, ref List<Cookie> cookies, string referer = "", Dictionary<string, string> headers = null)
		{
			HttpResult result;
			try
			{
				if (!url.StartsWith("http://") && !url.StartsWith("https://"))
				{
					url = "http://" + url;
				}
				HttpSocket httpSocket = new HttpSocket();
				httpSocket._proxy_host = TransferMgr._proxy_host;
				httpSocket._proxy_port = TransferMgr._proxy_port;
				httpSocket.Method = "POST";
				httpSocket.Url = url;
				if (referer != null && referer != "")
				{
					httpSocket.Referer = referer;
				}
				if (headers != null && headers.Count > 0)
				{
					IEnumerator<string> enumerator = headers.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						httpSocket.addExtraHeader(current, headers[current]);
					}
				}
				httpSocket.setCookies(cookies);
				httpSocket.Data = data;
				HttpResult httpResult = httpSocket.execute();
				if (cookies != null)
				{
					List<Cookie> cookies2 = httpResult.getCookies();
					CommonUtils.updateCookies(ref cookies, ref cookies2);
				}
				result = httpResult;
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("ERROR:{0}::{1}", ex.Message, ex.StackTrace));
			}
			result = null;
			return result;
		}
	}
}
