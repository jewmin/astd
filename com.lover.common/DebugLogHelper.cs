using System;
using System.IO;
using System.Text;

namespace com.lover.common
{
	public class DebugLogHelper
	{
		private static DebugLogHelper _instance = new DebugLogHelper();

		private bool _enable = true;

		private string _filename = "command.log";

		public static DebugLogHelper getInstance()
		{
			return DebugLogHelper._instance;
		}

		public void log(string text)
		{
            if (!this._enable) return;

            FileInfo fileInfo = new FileInfo(this._filename);
            FileStream fileStream;
            if (!fileInfo.Exists)
            {
                fileStream = fileInfo.Create();
            }
            else
            {
                fileStream = fileInfo.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            }
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            DateTime now = DateTime.Now;
            streamWriter.WriteLine(string.Format("[{0:G}]:{1}", now, text));
            streamWriter.Close();
            fileStream.Close();
		}
	}
}
