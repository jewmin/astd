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
            //string text = "updater1.exe";
            //string text2 = "updater.exe";
            //if (File.Exists(text))
            //{
            //    if (File.Exists(text2))
            //    {
            //        File.Delete(text2);
            //    }
            //    File.Move(text, text2);
            //}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            //bool accountFromArgs = false;
            //if (args.Length >= 3)
            //{
            //    string s = args[0];
            //    string userAuth = args[1];
            //    string arg_5C_0 = args[2];
            //    int num = 0;
            //    int num2 = 0;
            //    int.TryParse(s, out num);
            //    int.TryParse(arg_5C_0, out num2);
            //    if (num > 0 && num2 > 0)
            //    {
            //        UserSessionMgr.instance.UserId = num;
            //        UserSessionMgr.instance.UserAuth = userAuth;
            //        UserSessionMgr.instance.AccId = num2;
            //        bool flag = false;
            //        if (args.Length >= 4)
            //        {
            //            flag = (args[3] == "sf");
            //        }
            //        string text3;
            //        if (flag)
            //        {
            //            text3 = UserSessionMgr.instance.getSfConfig();
            //        }
            //        else
            //        {
            //            text3 = UserSessionMgr.instance.getSession();
            //        }
            //        if (text3.Length > 0)
            //        {
            //            UiUtils.getInstance().info("获取用户会话出错, " + text3);
            //            Environment.Exit(-1);
            //        }
            //        accountFromArgs = true;
            //    }
            //}
            //Application.Run(new MainForm
            //{
            //    AccountFromArgs = accountFromArgs
            //});
            try
            {
                Application.Run(new NewMainForm());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Application ERROR::{0}::{1}", ex.Message, ex.StackTrace);
            }
		}
	}
}
