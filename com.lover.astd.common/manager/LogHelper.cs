using System;
using System.IO;

namespace com.lover.astd.common.manager
{
	public class LogHelper
	{
		private string _dirName = "";

		private string _logFileName = "astd.log";

		public string LogFilePath
		{
			get
			{
				return this._dirName + "/logs/" + this._logFileName;
			}
		}

		public LogHelper(string partner, int serverid, string rolename, string _userDirName = "")
		{
			string dirName = "accounts";
			if (_userDirName != "")
			{
				dirName = dirName + "/" + _userDirName;
			}
            this._dirName = string.Format("{0}/{1}_{2}_{3}", dirName, partner, serverid, rolename);
            if (this._dirName == null || this._dirName.Trim().Length == 0)
            {
                return;
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(this._dirName);
            if (!directoryInfo.Exists)
            {
                if (Directory.Exists(rolename))
                {
                    if (!Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(dirName);
                    }
                    Directory.Move(rolename, this._dirName);
                }
                string userDirName = string.Format("{0}/{1}", dirName, rolename);
                if (Directory.Exists(userDirName))
                {
                    Directory.Move(userDirName, this._dirName);
                }
                if (!Directory.Exists(this._dirName))
                {
                    directoryInfo.Create();
                }
                if (!Directory.Exists(this._dirName + "/logs"))
                {
                    directoryInfo = new DirectoryInfo(this._dirName + "/logs");
                    directoryInfo.Create();
                }
            }
            else
            {
                directoryInfo = new DirectoryInfo(this._dirName + "/logs");
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }
		}

		public void log(string logtext)
		{
            if (this._dirName == null || this._dirName.Trim().Length == 0)
            {
                return;
            }
            string logFileName = "logs/" + this._logFileName;
            if (this._dirName.Trim().Length > 0)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(this._dirName);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                    directoryInfo = new DirectoryInfo(this._dirName + "/logs");
                    directoryInfo.Create();
                }
                else
                {
                    directoryInfo = new DirectoryInfo(this._dirName + "/logs");
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                }
                logFileName = this._dirName + "/" + logFileName;
            }
            FileInfo fileInfo = new FileInfo(logFileName);
            FileStream fileStream;
            if (fileInfo.Exists)
            {
                if (fileInfo.LastWriteTime.Date < DateTime.Now.Date)
                {
                    fileInfo.Delete();
                    fileStream = fileInfo.Create();
                }
                else
                {
                    fileStream = fileInfo.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                }
            }
            else
            {
                fileStream = fileInfo.Create();
            }
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(logtext);
            streamWriter.Close();
            fileStream.Close();
		}
	}
}
