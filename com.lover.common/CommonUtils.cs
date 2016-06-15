using System.Net.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace com.lover.common
{
	public class CommonUtils
	{
		private static List<string> _md5ExceptDir = null;

		private static string COOKIE_SPLITER = "; ";

		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        public static void setCookie(string url, string name, string value, string path, string domain)
        {
            string lpszCookieData = string.Format("{0}={1};path={2};domain={3};", name, value, path, domain);
            CommonUtils.InternetSetCookie(url, null, lpszCookieData);
        }

        public static void setCookie(string url, string name, string value, string path, string domain, DateTime expires)
        {
            string lpszCookieData = string.Format("{0}={1};path={2};domain={3};expires={4}", name, value, path, domain, expires.ToString("R"));
            CommonUtils.InternetSetCookie(url, null, lpszCookieData);
        }

		public static List<Cookie> getCookieByUrl(List<Cookie> all_cookies, string url)
		{
			List<Cookie> list = new List<Cookie>();
			List<Cookie> result;
			if (all_cookies == null || all_cookies.Count == 0)
			{
				result = list;
			}
			else
			{
				Uri uri = new Uri(url);
				string absolutePath = uri.AbsolutePath;
				string text = "." + uri.Host;
				foreach (Cookie current in all_cookies)
				{
					if (absolutePath.StartsWith(current.Path) && text.EndsWith(current.Domain))
					{
						list.Add(current);
					}
				}
				result = list;
			}
			return result;
		}

		public static void updateCookies(ref List<Cookie> base_cookies, ref List<Cookie> new_cookies)
		{
            if (new_cookies == null || new_cookies.Count == 0)
            {
                return;
            }
            foreach (Cookie current in new_cookies)
            {
                Cookie cookie = null;
                foreach (Cookie current2 in base_cookies)
                {
                    if (current2.Name.Equals(current.Name) && current2.Path.Equals(current.Path) && current2.Domain.Equals(current.Domain))
                    {
                        cookie = current2;
                        break;
                    }
                }
                if (cookie != null)
                {
                    base_cookies.Remove(cookie);
                }
                base_cookies.Add(current);
            }
		}

		public static long getMSecondsNow()
		{
			double totalMilliseconds = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
			return (long)totalMilliseconds;
		}

		public static long getMSeconds(DateTime dt)
		{
			double totalMilliseconds = dt.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
			return (long)totalMilliseconds;
		}

		public static string DESEncrypt(string strPlain, string strDESKey, string strDESIV)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(strDESKey);
			byte[] bytes2 = Encoding.ASCII.GetBytes(strDESIV);
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write);
			StreamWriter streamWriter = new StreamWriter(cryptoStream);
			streamWriter.WriteLine(strPlain);
			streamWriter.Close();
			cryptoStream.Close();
			byte[] bytes3 = memoryStream.ToArray();
			memoryStream.Close();
			return Encoding.Unicode.GetString(bytes3);
		}

		public static string DESDecrypt(string strCipher, string strDESKey, string strDESIV)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(strDESKey);
			byte[] bytes2 = Encoding.ASCII.GetBytes(strDESIV);
			byte[] bytes3 = Encoding.Unicode.GetBytes(strCipher);
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream(bytes3);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, bytes2), CryptoStreamMode.Read);
			StreamReader streamReader = new StreamReader(cryptoStream);
			string result = streamReader.ReadLine();
			streamReader.Close();
			cryptoStream.Close();
			memoryStream.Close();
			return result;
		}

		public static List<Point> getAStarPath(int[,] grids, Point start, Point target)
		{
			List<Point> list = new List<Point>();
			bool flag = grids[target.X, target.Y] == -1;
			List<Point> result;
			if (flag)
			{
				result = list;
			}
			else
			{
				int length = grids.GetLength(0);
				int length2 = grids.GetLength(1);
				int[,] array = new int[length, length2];
				int[,] array2 = new int[length, length2];
				int[,] array3 = new int[length, length2];
				List<Point> list2 = new List<Point>();
				List<Point> list3 = new List<Point>();
				List<Point> list4 = new List<Point>();
				List<Point> list5 = new List<Point>();
				bool flag2 = false;
				Point point = new Point(start.X, start.Y);
				array2[start.X, start.Y] = 0;
				array3[start.X, start.Y] = Math.Abs(target.X - start.X) + Math.Abs(target.Y - start.Y);
				array[start.X, start.Y] = array2[start.X, start.Y] + array3[start.X, start.Y];
				list4.Add(new Point(start.X, start.Y));
				list5.Add(new Point(start.X, start.Y));
				while (!flag2)
				{
					bool flag3 = point.X == target.X && point.Y == target.Y;
					if (flag3)
					{
						flag2 = true;
						break;
					}
					Point item = new Point(-1, -1);
					int num6;
					for (int i = 0; i < 4; i = num6 + 1)
					{
						Point point2 = default(Point);
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						int num4 = 99999999;
						bool flag4 = i == 0;
						if (flag4)
						{
							point2.X = point.X;
							point2.Y = point.Y - 1;
						}
						else
						{
							bool flag5 = i == 1;
							if (flag5)
							{
								point2.X = point.X;
								point2.Y = point.Y + 1;
							}
							else
							{
								bool flag6 = i == 2;
								if (flag6)
								{
									point2.X = point.X - 1;
									point2.Y = point.Y;
								}
								else
								{
									bool flag7 = i == 3;
									if (flag7)
									{
										point2.X = point.X + 1;
										point2.Y = point.Y;
									}
								}
							}
						}
						bool flag8 = point2.X >= 0 && point2.Y >= 0 && point2.Y < length2 && point2.X < length && grids[point2.X, point2.Y] != -1;
						if (flag8)
						{
							bool flag9 = false;
							foreach (Point current in list4)
							{
								bool flag10 = current.X == point2.X && current.Y == point2.Y;
								if (flag10)
								{
									flag9 = true;
									break;
								}
							}
							bool flag11 = !flag9;
							if (flag11)
							{
								num = array2[point.X, point.Y] + 1;
								num2 = Math.Abs(target.X - point2.X) + Math.Abs(target.Y - point2.Y);
								num3 = num + num2;
								bool flag12 = false;
								int index = -1;
								int num5 = -1;
								foreach (Point current2 in list2)
								{
									num6 = num5;
									num5 = num6 + 1;
									bool flag13 = current2.X == point2.X && current2.Y == point2.Y;
									if (flag13)
									{
										flag12 = true;
										index = num5;
										break;
									}
								}
								bool flag14 = flag12;
								if (flag14)
								{
									bool flag15 = array2[point2.X, point2.Y] < num;
									if (flag15)
									{
										num = array2[point2.X, point2.Y];
										num2 = array3[point2.X, point2.Y];
										num3 = array[point2.X, point2.Y];
										bool flag16 = num3 <= num4;
										if (flag16)
										{
											item.X = point2.X;
											item.Y = point2.Y;
										}
									}
									else
									{
										array2[point2.X, point2.Y] = num;
										array3[point2.X, point2.Y] = num2;
										array[point2.X, point2.Y] = num3;
										list3[index] = new Point(point.X, point.Y);
										bool flag17 = num3 <= num4;
										if (flag17)
										{
											item.X = point2.X;
											item.Y = point2.Y;
										}
									}
								}
								else
								{
									list2.Add(new Point(point2.X, point2.Y));
									list3.Add(new Point(point.X, point.Y));
									array2[point2.X, point2.Y] = num;
									array3[point2.X, point2.Y] = num2;
									array[point2.X, point2.Y] = num3;
									bool flag18 = num3 <= num4;
									if (flag18)
									{
										item.X = point2.X;
										item.Y = point2.Y;
									}
								}
							}
						}
						num6 = i;
					}
					bool flag19 = item.X < 0 || item.Y < 0;
					if (flag19)
					{
						bool flag20 = list4.Count == 1;
						if (flag20)
						{
							break;
						}
						int num7 = -1;
						int num8 = -1;
						foreach (Point current3 in list4)
						{
							num6 = num7;
							num7 = num6 + 1;
							bool flag21 = current3.X == point.X && current3.Y == point.Y;
							if (flag21)
							{
								num8 = num7;
								break;
							}
						}
						bool flag22 = num8 < 0;
						if (flag22)
						{
							point = list4[list4.Count - 1];
						}
						else
						{
							bool flag23 = num8 == 0;
							if (flag23)
							{
								break;
							}
							point = list4[num8 - 1];
						}
					}
					else
					{
						int num9 = -1;
						int num10 = -1;
						foreach (Point current4 in list2)
						{
							num6 = num9;
							num9 = num6 + 1;
							bool flag24 = current4.X == item.X && current4.Y == item.Y;
							if (flag24)
							{
								num10 = num9;
								break;
							}
						}
						Point item2 = point;
						bool flag25 = num10 >= 0;
						if (flag25)
						{
							list2.RemoveAt(num10);
							item2 = list3[num10];
							list3.RemoveAt(num10);
						}
						list4.Add(item);
						list5.Add(item2);
						point.X = item.X;
						point.Y = item.Y;
					}
				}
				bool flag26 = flag2;
				if (flag26)
				{
					list.Insert(0, new Point(point.X, point.Y));
					int num11 = 0;
					int num12 = length * length2;
					while ((point.X != start.X || point.Y != start.Y) && num11 < num12)
					{
						int num13 = -1;
						int num6;
						for (int j = 0; j < list4.Count; j = num6 + 1)
						{
							bool flag27 = list4[j].X == point.X && list4[j].Y == point.Y;
							if (flag27)
							{
								num13 = j;
								break;
							}
							num6 = j;
						}
						bool flag28 = num13 >= 0;
						if (flag28)
						{
							point = list5[num13];
						}
						list.Insert(0, new Point(point.X, point.Y));
						num6 = num11;
						num11 = num6 + 1;
					}
				}
				result = list;
			}
			return result;
		}

		public static string getReadable(long number)
		{
			bool flag = number > 1000000L;
			string result;
			if (flag)
			{
				result = string.Format("{0}万", (int)(number / 10000L));
			}
			else
			{
				result = number.ToString();
			}
			return result;
		}

		public static string getShortReadable(long number)
		{
			bool flag = number > 10000L;
			string result;
			if (flag)
			{
				result = string.Format("{0}万", (int)(number / 10000L));
			}
			else
			{
				result = number.ToString();
			}
			return result;
		}

		public static string generateBytesMd5(byte[] raw_bytes)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] value = mD5CryptoServiceProvider.ComputeHash(raw_bytes);
			string text = BitConverter.ToString(value);
			return text.Replace("-", "");
		}

		public static string generateStringMd5(string raw, Encoding encoding = null)
		{
			Encoding encoding2 = Encoding.UTF8;
			bool flag = encoding != null;
			if (flag)
			{
				encoding2 = encoding;
			}
			byte[] bytes = encoding2.GetBytes(raw);
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] value = mD5CryptoServiceProvider.ComputeHash(bytes);
			string text = BitConverter.ToString(value);
			return text.Replace("-", "");
		}

		public static JsonObjectCollection getLocalMd5(List<string> md5ExceptDir)
		{
			CommonUtils._md5ExceptDir = md5ExceptDir;
			string applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			DirectoryInfo dirinfo = new DirectoryInfo(applicationBase);
			return CommonUtils.generateDirMd5(dirinfo, "");
		}

		private static string generateFileMd5(string filePath)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			bool flag = !fileInfo.Exists;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				FileStream fileStream = fileInfo.OpenRead();
				byte[] value = mD5CryptoServiceProvider.ComputeHash(fileStream);
				fileStream.Close();
				string text = BitConverter.ToString(value);
				result = text.Replace("-", "");
			}
			return result;
		}

		public static string generateCookieString(List<Cookie> cookies)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int count = cookies.Count;
			while (i < count)
			{
				Cookie cookie = cookies[i];
				stringBuilder.AppendFormat("{0}={1};path={2};domain={3};expires={4}{5}", new object[]
				{
					cookie.Name,
					cookie.Value,
					cookie.Path,
					cookie.Domain,
					cookie.Expires,
					CommonUtils.COOKIE_SPLITER
				});
				int num = i;
				i = num + 1;
			}
			return stringBuilder.ToString();
		}

		public static List<Cookie> generateCookies(string confString)
		{
			List<Cookie> list = new List<Cookie>();
			string[] array = confString.Split(new string[]
			{
				CommonUtils.COOKIE_SPLITER
			}, StringSplitOptions.RemoveEmptyEntries);
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				string text = array[i];
				bool flag = text != null && !(text == "");
				if (flag)
				{
					Cookie cookie = CommonUtils.parseCookie(array[i]);
					bool flag2 = cookie != null;
					if (flag2)
					{
						list.Add(cookie);
					}
				}
				int num2 = i;
				i = num2 + 1;
			}
			return list;
		}

		public static Cookie parseCookie(string cookie_string)
		{
			bool flag = cookie_string == null || cookie_string.Length == 0;
			Cookie result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string[] array = cookie_string.Split(new char[]
				{
					';'
				});
				Cookie cookie = new Cookie();
				int num = 0;
				string[] array2 = array;
				int num3;
				for (int i = 0; i < array2.Length; i = num3 + 1)
				{
					string text = array2[i];
					bool flag2 = text != null && text.Length != 0;
					if (flag2)
					{
						int num2 = text.IndexOf('=');
						bool flag3 = num2 >= 0;
						if (flag3)
						{
							string text2 = text.Substring(0, num2).Trim();
							string text3 = text.Substring(num2 + 1).Trim();
							num3 = num;
							num = num3 + 1;
							bool flag4 = num == 1;
							if (flag4)
							{
								cookie.Name = text2;
								cookie.Value = text3;
							}
							else
							{
								bool flag5 = text2.ToLower().Equals("path");
								if (flag5)
								{
									cookie.Path = text3;
								}
								else
								{
									bool flag6 = text2.ToLower().Equals("domain");
									if (flag6)
									{
										cookie.Domain = text3;
									}
									else
									{
										bool flag7 = text2.ToLower().Equals("expires");
										if (flag7)
										{
											DateTime expires;
											bool flag8 = DateTime.TryParse(text3, out expires);
											bool flag9 = flag8;
											if (flag9)
											{
												cookie.Expires = expires;
											}
										}
									}
								}
							}
						}
					}
					num3 = i;
				}
				result = cookie;
			}
			return result;
		}

		private static JsonObjectCollection generateDirMd5(DirectoryInfo dirinfo, string dirPath)
		{
			JsonObjectCollection jsonObjectCollection = new JsonObjectCollection();
			jsonObjectCollection.Name = dirPath;
			string str = (dirPath == "") ? "" : (dirPath + "/");
			DirectoryInfo[] directories = dirinfo.GetDirectories();
			int i = 0;
			int num = directories.Length;
			while (i < num)
			{
				DirectoryInfo directoryInfo = directories[i];
				bool flag = !(directoryInfo.Name == ".") && !(directoryInfo.Name == "..") && !CommonUtils._md5ExceptDir.Contains(directoryInfo.Name);
				if (flag)
				{
					JsonObjectCollection jsonObjectCollection2 = CommonUtils.generateDirMd5(directoryInfo, str + directoryInfo.Name);
					IEnumerator<JsonObject> enumerator = jsonObjectCollection2.GetEnumerator();
					while (enumerator.MoveNext())
					{
						JsonObject current = enumerator.Current;
						jsonObjectCollection.Add(current);
					}
				}
				int num2 = i;
				i = num2 + 1;
			}
			FileInfo[] files = dirinfo.GetFiles();
			int j = 0;
			int num3 = files.Length;
			while (j < num3)
			{
				FileInfo fileInfo = files[j];
				string text = CommonUtils.generateFileMd5(fileInfo.FullName);
				text = text.ToLower();
				jsonObjectCollection.Add(new JsonStringValue
				{
					Name = str + fileInfo.Name,
					Value = text
				});
				int num2 = j;
				j = num2 + 1;
			}
			return jsonObjectCollection;
		}
	}
}
