using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace com.lover.common.hook
{
	public class KeyboardInfo
	{
		private KeyboardInfo()
		{
		}

		[DllImport("user32")]
		private static extern short GetKeyState(int vKey);

		public static KeyStateInfo GetKeyState(Keys key)
		{
			int vKey = (int)key;
			bool flag = key == Keys.Alt;
			if (flag)
			{
				vKey = 18;
			}
			short keyState = KeyboardInfo.GetKeyState(vKey);
			int num = KeyboardInfo.Low((int)keyState);
			int num2 = KeyboardInfo.High((int)keyState);
			bool istoggled = num == 1;
			bool ispressed = num2 == 1;
			return new KeyStateInfo(key, ispressed, istoggled);
		}

		private static int High(int keyState)
		{
			bool flag = keyState > 0;
			int result;
			if (flag)
			{
				result = keyState >> 16;
			}
			else
			{
				result = (keyState >> 16 & 1);
			}
			return result;
		}

		private static int Low(int keyState)
		{
			return keyState & 65535;
		}
	}
}
