using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace com.lover.common
{
	public class LocMgr
	{
		private static ResourceManager _resMgr;

		private static CultureInfo _culture;

		public static void initManager(string resName, string loc)
		{
			bool flag = resName == null || resName == "";
			if (flag)
			{
				throw new Exception("必须指定语言资源名");
			}
			bool flag2 = loc == null || loc == "";
			if (flag2)
			{
				loc = "zh_CN";
			}
			LocMgr._resMgr = new ResourceManager(resName, Assembly.GetExecutingAssembly());
			LocMgr._culture = new CultureInfo(loc);
		}

		public static string getString(string key)
		{
			bool flag = LocMgr._resMgr == null;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				string text;
				try
				{
					text = LocMgr._resMgr.GetString(key, LocMgr._culture);
				}
				catch
				{
					text = "";
				}
				result = text;
			}
			return result;
		}
	}
}
