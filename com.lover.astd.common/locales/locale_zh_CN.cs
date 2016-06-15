using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace com.lover.astd.common.locales
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class locale_zh_CN
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = locale_zh_CN.resourceMan == null;
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("com.lover.astd.common.locales.locale.zh_CN", typeof(locale_zh_CN).Assembly);
					locale_zh_CN.resourceMan = resourceManager;
				}
				return locale_zh_CN.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return locale_zh_CN.resourceCulture;
			}
			set
			{
				locale_zh_CN.resourceCulture = value;
			}
		}

		internal locale_zh_CN()
		{
		}
	}
}
