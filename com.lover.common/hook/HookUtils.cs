using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace com.lover.common.hook
{
	public class HookUtils
	{
		public enum HookType
		{
			WH_KEYBOARD = 2,
			WH_KEYBOARD_LL = 13
		}

		public struct KeyBoardHookStruct
		{
			public int vkCode;

			public int ScanCode;

			public int Flags;

			public int Time;

			public int DwExtraInfo;
		}

		public struct KBDLLHOOKSTRUCT
		{
			public uint vkCode;

			public uint scanCode;

			public uint flags;

			public uint time;

			public IntPtr extraInfo;
		}

		private static bool _hookShift = true;

		private static bool _hookCtrl = true;

		private static bool _hookAlt = true;

		private static Keys _hookKey = Keys.None;

		private static int _hookHandle = 0;

		private static HOOKPROC _keyProc = new HOOKPROC(HookUtils.HookKeyboardProc);

		private static DoKeyHook _hookTrueFunc;

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		private static extern int SetWindowsHookEx(HookUtils.HookType idHook, HOOKPROC lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		private static extern bool UnhookWindowsHookEx(int idHook);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		private static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32")]
		private static extern int GetCurrentThreadId();

		public static void removeKeyHook()
		{
			if (HookUtils._hookHandle != 0)
			{
				HookUtils.UnhookWindowsHookEx(HookUtils._hookHandle);
				HookUtils._hookHandle = 0;
			}
		}

		public static void setKeyHook(bool doShift, bool doCtrl, bool doAlt, Keys keycode, DoKeyHook func)
		{
			HookUtils._hookCtrl = doCtrl;
			HookUtils._hookAlt = doAlt;
			HookUtils._hookShift = doShift;
			HookUtils._hookKey = keycode;
			HookUtils._hookTrueFunc = func;
			if (HookUtils._hookHandle != 0)
			{
				HookUtils.UnhookWindowsHookEx(HookUtils._hookHandle);
				HookUtils._hookHandle = 0;
			}
			HookUtils._hookHandle = HookUtils.SetWindowsHookEx(HookUtils.HookType.WH_KEYBOARD_LL, HookUtils._keyProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
		}

		public static int HookKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			int result;
			if (HookUtils._hookHandle == 0)
			{
				result = HookUtils.CallNextHookEx(HookUtils._hookHandle, nCode, wParam, lParam);
			}
            else if (nCode < 0 || !(((long)lParam.ToInt32() & -2147483648L) == 0L))
            {
                result = HookUtils.CallNextHookEx(HookUtils._hookHandle, nCode, wParam, lParam);
            }
            else
            {
                KeyStateInfo keyState = KeyboardInfo.GetKeyState(Keys.ControlKey);
                KeyStateInfo keyState2 = KeyboardInfo.GetKeyState(Keys.Alt);
                KeyStateInfo keyState3 = KeyboardInfo.GetKeyState(Keys.ShiftKey);
                if (KeyboardInfo.GetKeyState(HookUtils._hookKey).IsPressed && ((HookUtils._hookCtrl && keyState.IsPressed) || !HookUtils._hookCtrl) && ((HookUtils._hookAlt && keyState2.IsPressed) || !HookUtils._hookAlt) && ((HookUtils._hookShift && keyState3.IsPressed) || !HookUtils._hookShift) && HookUtils._hookTrueFunc != null)
                {
                    HookUtils._hookTrueFunc();
                }
                result = HookUtils.CallNextHookEx(HookUtils._hookHandle, nCode, wParam, lParam);
            }
			return result;
		}
	}
}
