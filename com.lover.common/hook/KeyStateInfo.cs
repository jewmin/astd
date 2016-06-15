using System;
using System.Windows.Forms;

namespace com.lover.common.hook
{
	public struct KeyStateInfo
	{
		private Keys m_Key;

		private bool m_IsPressed;

		private bool m_IsToggled;

		public static KeyStateInfo Default
		{
			get
			{
				return new KeyStateInfo(Keys.None, false, false);
			}
		}

		public Keys Key
		{
			get
			{
				return this.m_Key;
			}
		}

		public bool IsPressed
		{
			get
			{
				return this.m_IsPressed;
			}
		}

		public bool IsToggled
		{
			get
			{
				return this.m_IsToggled;
			}
		}

		public KeyStateInfo(Keys key, bool ispressed, bool istoggled)
		{
			this.m_Key = key;
			this.m_IsPressed = ispressed;
			this.m_IsToggled = istoggled;
		}
	}
}
