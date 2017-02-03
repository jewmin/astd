using com.lover.astd.common;
using com.lover.common;
using System;
using System.IO;
using System.Windows.Forms;

namespace com.lover.astd.game.ui
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new NewMainForm());
            }
            catch
            {
                DateTime now = DateTime.Now;
                MiniDump.TryDump(string.Format("astd{0}_{1}_{2}_{3}_{4}_{5}.dmp", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second), MiniDump.MiniDumpType.WithFullMemory);
            }
		}
	}
}
