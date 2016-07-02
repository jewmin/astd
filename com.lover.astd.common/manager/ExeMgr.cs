using com.lover.astd.common.config;
using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Text;
using LuaInterface;

namespace com.lover.astd.common.manager
{
	public class ExeMgr
	{
		private bool _isRunningServer;

		public StringBuilder _sb_process = new StringBuilder();

		private string _status = "";

		private long _nextFireTime;

		private GameConfig _conf;

        private OtherConfig _otherConf;

		private User _user;

		private ServiceFactory _factory;

		private ProtocolMgr _proto;

        private LuaMgr _lua;

		private ILogger _logger;

		private List<IExecute> _exes = new List<IExecute>();

		public bool IsRunningServer
		{
			get
			{
				return this._isRunningServer;
			}
			set
			{
				this._isRunningServer = value;
                for (int i = 0; i < this._exes.Count; i++)
                {
                    IExecute exe = this._exes[i];
                    if (exe != null)
                    {
                        exe.setRunningServer(this._isRunningServer);
                    }
                }
			}
		}

		public string Status
		{
			get
			{
				return this._status;
			}
		}

		public long nextFireTime
		{
			get
			{
				return this._nextFireTime;
			}
		}

		public ExeMgr(GameConfig conf, User u)
		{
			this._conf = conf;
			this._user = u;
		}

        private IExecute findExe(string exe_name)
        {
            for (int i = 0; i < this._exes.Count; i++)
            {
                IExecute exe = this._exes[i];
                if (exe.getName().Equals(exe_name))
                {
                    return exe;
                }
            }
            return null;
        }

		public void addExe(IExecute exe)
		{
            if (exe == null || exe.getName() == null)
            {
                return;
            }
            if (this.findExe(exe.getName()) != null)
            {
                return;
            }
            this._exes.Add(exe);
		}

        public void refreshConfig(GameConfig conf)
        {
            for (int i = 0; i < this._exes.Count; i++)
            {
                IExecute exe = this._exes[i];
                if (exe != null)
                {
                    exe.refreshConfig(conf);
                }
            }
            this.clear_runtime();
        }

		public void setExeVariables(ProtocolMgr proto, ILogger logger, IServer server, User user, GameConfig conf, OtherConfig otherConf, ServiceFactory factory, LuaMgr lua)
		{
			_factory = factory;
			_proto = proto;
            _lua = lua;
			_logger = logger;
            _otherConf = otherConf;
            for (int i = 0; i < _exes.Count; i++)
            {
                IExecute exe = _exes[i];
                if (exe != null)
                {
                    exe.setVariables(proto, logger, server, user, conf, factory, lua);
                    exe.setOtherConf(_otherConf);
                }
            }
		}

		public void clear_runtime()
		{
            for (int i = 0; i < this._exes.Count; i++)
            {
                IExecute exe = this._exes[i];
                if (exe != null)
                {
                    exe.setNextExeTime(0);
                    exe.clearTmpVariables();
                }
            }
            this._nextFireTime = 0;
		}

		public void fireSingleForce(string exe_name)
		{
			IExecute exe = this.findExe(exe_name);
            if (exe == null)
            {
                return;
            }
            long nextExeTime = exe.execute();
            exe.setNextExeTime(nextExeTime);
            if (this._nextFireTime == 0 || this._nextFireTime > exe.getNextExeTime())
            {
                this._nextFireTime = exe.getNextExeTime();
            }
		}

		public string fire(bool save_log = false)
		{
			this._sb_process.Remove(0, this._sb_process.Length);
			this._nextFireTime = 0;
			long timeStamp = this._factory.TmrMgr.TimeStamp;
			if (this._user._arrest_state == 100)
			{
				this._factory.getBattleManager().escapeFromJail(this._proto, this._logger);
				this._user._arrest_state = 0;
			}
			for (int i = 0; i < this._exes.Count; i++)
			{
				IExecute exe = this._exes[i];
				if (exe != null)
				{
					if (exe.getNextExeTime() <= timeStamp)
					{
						this._sb_process.AppendFormat("-------{0}: user:{1} --> START do exe:{2}", this._factory.TmrMgr.TimeStamp, this._user.Username, exe.getReadableName());
						long nextExeTime = exe.execute();
						exe.setNextExeTime(nextExeTime);
						this._sb_process.AppendFormat("-------{0}: user:{1} --> END do exe:{2}", this._factory.TmrMgr.TimeStamp, this._user.Username, exe.getReadableName());
					}
                    if (this._nextFireTime == 0 || this._nextFireTime > exe.getNextExeTime())
                    {
                        this._nextFireTime = exe.getNextExeTime();
                    }
				}
			}
            for (int i = 0; i < this._exes.Count - 1; i++)
            {
                for (int j = i + 1; j < this._exes.Count; j++)
                {
                    if (this._exes[i].getNextExeTime() > this._exes[j].getNextExeTime())
                    {
                        IExecute var = this._exes[i];
                        this._exes[i] = this._exes[j];
                        this._exes[j] = var;
                    }
                }
            }
			StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this._exes.Count; i++)
            {
                IExecute exe = this._exes[i];
                if (exe != null)
                {
                    DateTime dt = new DateTime(exe.getNextExeTime() * 10000);
                    sb.AppendFormat("{0} : {1}\r\n", dt.ToString("HH:mm:ss"), exe.getReadableName());
                }
            }
			this._status = sb.ToString();
            if (save_log)
            {
                return this._sb_process.ToString();
            }
            return "";
		}

		public void init_data()
		{
            for (int i = 0; i < this._exes.Count; i++)
            {
                IExecute exe = this._exes[i];
                if (exe != null)
                {
                    exe.init_data();
                }
            }
		}
	}
}
