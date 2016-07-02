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
			if (resName == null || resName == "")
			{
				throw new Exception("必须指定语言资源名");
			}
			if (loc == null || loc == "")
			{
				loc = "zh_CN";
			}
			LocMgr._resMgr = new ResourceManager(resName, Assembly.GetExecutingAssembly());
			LocMgr._culture = new CultureInfo(loc);
		}

		public static string getString(string key)
		{
            if (LocMgr._resMgr == null) return "";

            string text;
            try
            {
                text = LocMgr._resMgr.GetString(key, LocMgr._culture);
            }
            catch
            {
                text = "";
            }
            return text;
		}
	}
}
