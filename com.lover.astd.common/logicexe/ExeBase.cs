using com.lover.astd.common.config;
using com.lover.astd.common.logic;
using com.lover.astd.common.model;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace com.lover.astd.common.logicexe
{
	public abstract class ExeBase : IExecute
	{
		protected bool _isRunningServer;

		protected ServiceFactory _factory;

		protected bool _ready;

		protected ILogger _logger;

		protected ProtocolMgr _proto;

		protected IServer _server;

		protected User _user;

		protected GameConfig _conf;

        protected OtherConfig _otherConf;

		private long _nextExeTimeStamp;

		protected string _name = "";

		protected string _readable = "";

		protected Color _logColor = Color.Black;

		public bool Ready
		{
			get
			{
				return this._ready;
			}
		}

		public bool IsRunningServer()
		{
			return this._isRunningServer;
		}

		public void setRunningServer(bool isserver)
		{
			this._isRunningServer = isserver;
		}

		public virtual void clearTmpVariables()
		{
		}

		public void setNextExeTime(long timestamp)
		{
			this._nextExeTimeStamp = timestamp + this._factory.TmrMgr.TimeStamp;
		}

		public long getNextExeTime()
		{
			return this._nextExeTimeStamp;
		}

		public void setName(string name)
		{
			this._name = name;
		}

		public string getName()
		{
			return this._name;
		}

		public void setReadableName(string rname)
		{
			this._readable = rname;
		}

		public string getReadableName()
		{
			return this._readable;
		}

		protected virtual void refreshUi()
		{
			this._user.addUiToQueue(this._name);
		}

		public void setVariables(ProtocolMgr proto, ILogger logger, IServer server, User u, GameConfig conf, ServiceFactory factory)
		{
			this._proto = proto;
			this._logger = logger;
			this._server = server;
			this._user = u;
			this._conf = conf;
			this._factory = factory;
		}

		public void refreshConfig(GameConfig conf)
		{
			this._conf = conf;
		}

		public virtual void init_data()
		{
			this.refreshUi();
		}

		public abstract long execute();

		protected virtual void logInfo(string text)
		{
			this._logger.log(text, this._logColor);
		}

		protected Dictionary<string, string> getConfig()
		{
			return this._conf.getConfig(this._name);
		}

        protected Dictionary<string, string> getOtherConfig()
        {
            return _otherConf.getConfig(_name);
        }

        /// <summary>
        /// 精英军团加成时间
        /// </summary>
        /// <returns></returns>
		protected bool inJingyingArmyTime()
		{
			int hour = this._factory.TmrMgr.DateTimeNow.Hour;
			return (hour == 13) || (hour >= 20 && hour < 23);
		}

        /// <summary>
        /// 马上
        /// </summary>
        /// <returns></returns>
		protected long immediate()
		{
			return 2000;
		}

        /// <summary>
        /// 瞬间
        /// </summary>
        /// <returns></returns>
        protected long instant()
        {
            return 100;
        }

        /// <summary>
        /// 下半个小时
        /// </summary>
        /// <returns></returns>
		protected long next_halfhour()
		{
			if (this._isRunningServer)
			{
				Random random = new Random();
				return (long)((random.Next(30, 45) * 60 + random.Next(59)) * 1000);
			}
			else
			{
				DateTime dateTimeNow = this._factory.TmrMgr.DateTimeNow;
				int minute;
				if (dateTimeNow.Minute < 30)
				{
					minute = 30 - dateTimeNow.Minute;
				}
				else
				{
					minute = 60 - dateTimeNow.Minute;
				}
				return (long)(minute * 60 * 1000);
			}
		}

        /// <summary>
        /// 下个小时
        /// </summary>
        /// <returns></returns>
		protected long next_hour()
		{
			if (this._isRunningServer)
			{
				Random random = new Random();
				return (long)((random.Next(60, 80) * 60 + random.Next(59)) * 1000);
			}
			else
			{
				return (long)((60 - this._factory.TmrMgr.DateTimeNow.Minute) * 60 * 1000);
			}
		}

        /// <summary>
        /// 龙舟时间
        /// </summary>
        /// <returns></returns>
        protected long next_boat()
        {
            int hour = this._factory.TmrMgr.DateTimeNow.Hour;
            int minute = this._factory.TmrMgr.DateTimeNow.Minute;
            int second = this._factory.TmrMgr.DateTimeNow.Second;
            if (hour < 10)
            {
                return (long)((60 - second + (59 - minute) * 60 + (9 - hour) * 60 * 60 + 10) * 1000);
            }
            else if (hour < 15)
            {
                return (long)((60 - second + (59 - minute) * 60 + (14 - hour) * 60 * 60 + 10) * 1000);
            }
            else if (hour < 21)
            {
                return (long)((60 - second + (59 - minute) * 60 + (20 - hour) * 60 * 60 + 10) * 1000);
            }
            else
            {
                return this.next_day();
            }
        }

		protected long next_dash()
		{
			int second = this._factory.TmrMgr.DateTimeNow.Second;
			if (second >= 0 && second < 10)
            {
                return (long)((10 - second) * 1000);
            }
            else if (second >= 10 && second < 40)
			{
				return (long)((40 - second) * 1000);
			}
            else if (second >= 40 && second < 60)
            {
                return (long)((70 - second) * 1000);
            }
            else
            {
                return this.next_day();
            }
		}

        /// <summary>
        /// 一个小时
        /// </summary>
        /// <returns></returns>
		protected long an_hour_later()
		{
			return 3600000L;
		}

        /// <summary>
        /// 下一天
        /// </summary>
        /// <returns></returns>
		protected long next_day()
		{
			DateTime dateTimeNow = this._factory.TmrMgr.DateTimeNow;
			int hour;
			if (dateTimeNow.Hour < 5)
			{
				hour = 5 - dateTimeNow.Hour - 1;
			}
			else
			{
				hour = 29 - dateTimeNow.Hour - 1;
			}
			int minute = 60 - dateTimeNow.Minute;
			if (this._isRunningServer)
			{
				Random random = new Random();
				long second = (long)((hour * 60 * 60 + (minute + 30) * 60) * 1000);
				return second + (long)((random.Next(20) * 60 + random.Next(59)) * 1000);
			}
			else
			{
				return (long)((hour * 60 * 60 + (minute + 30) * 60) * 1000);
			}
		}

        /// <summary>
        /// 宴会时间
        /// </summary>
        /// <returns></returns>
		protected long next_dinner()
		{
			DateTime dateTimeNow = this._factory.TmrMgr.DateTimeNow;
			int hour;
			if (dateTimeNow.Hour < 10)
			{
				hour = 10 - dateTimeNow.Hour - 1;
			}
            else if (dateTimeNow.Hour < 19)
            {
                hour = 19 - dateTimeNow.Hour - 1;
            }
            else
            {
                hour = 34 - dateTimeNow.Hour - 1;
            }
			return (long)((hour * 60 * 60 + (60 - dateTimeNow.Minute + 1) * 60) * 1000);
		}

        /// <summary>
        /// 集结时间
        /// </summary>
        /// <returns></returns>
		protected bool in_gongjian_time()
		{
			DateTime dateTimeNow = this._factory.TmrMgr.DateTimeNow;
			int hour = dateTimeNow.Hour;
			int minute = dateTimeNow.Minute;
			return (hour == 10 && minute < 30) || (hour == 15 && minute < 30) || (hour == 20 && minute > 30);
		}

        /// <summary>
        /// 攻坚时间
        /// </summary>
        /// <returns></returns>
        protected long next_gongjian_time()
        {
            DateTime dateTimeNow = this._factory.TmrMgr.DateTimeNow;
            DateTime dateTime_10 = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 10, 0, 0, 0);
            DateTime dateTime_15 = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 15, 0, 0, 0);
            DateTime dateTime_20 = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 20, 30, 0, 0);
            if (dateTimeNow < dateTime_10)
            {
                return (long)(dateTime_10 - dateTimeNow).TotalMilliseconds;
            }
            else if (dateTimeNow < dateTime_15)
            {
                return (long)(dateTime_15 - dateTimeNow).TotalMilliseconds;
            }
            else if (dateTimeNow < dateTime_20)
            {
                return (long)(dateTime_20 - dateTimeNow).TotalMilliseconds;
            }
            else
            {
                return (long)(dateTime_10.AddDays(1.0) - dateTimeNow).TotalMilliseconds;
            }
        }

        /// <summary>
        /// 比较时间
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns>返回小的</returns>
        protected long smaller_time(long time1, long time2)
        {
            if (time1 < time2)
            {
                return time1;
            }
            else
            {
                return time2;
            }
        }

        /// <summary>
        /// 比较时间
        /// </summary>
        /// <param name="times"></param>
        /// <returns>返回最小的</returns>
		protected long smallest_time(List<long> times)
		{
            long min_time = 0;
            for (int i = 0; i < times.Count; i++)
            {
                if (min_time == 0 || min_time > times[i])
                {
                    min_time = times[i];
                }
            }
            return min_time;
		}

        /// <summary>
        /// 可用金币
        /// </summary>
        /// <returns></returns>
		protected int getGoldAvailable()
		{
			return this.getAvailable("gold_reserve", this._user.Gold);
		}

        /// <summary>
        /// 可用银币
        /// </summary>
        /// <returns></returns>
		protected int getSilverAvailable()
		{
			return this.getAvailable("silver_reserve", this._user.Silver);
		}

        /// <summary>
        /// 可用军工
        /// </summary>
        /// <returns></returns>
		protected int getCreditAvailable()
		{
			return this.getAvailable("credit_reserve", this._user.Credit);
		}

        /// <summary>
        /// 可用玉石
        /// </summary>
        /// <returns></returns>
		protected int getStoneAvailable()
		{
			return this.getAvailable("stone_reserve", this._user.Stone);
		}

		private int getAvailable(string global_key, int valueTotal)
		{
			Dictionary<string, string> config = this._conf.getConfig("global");
			int value = 0;
			if (config.ContainsKey(global_key))
			{
				int.TryParse(config[global_key], out value);
			}
            int reserve_value = valueTotal - value;
			if (reserve_value < 0)
			{
                reserve_value = 0;
			}
            return reserve_value;
		}

        /// <summary>
        /// 宝石性价比
        /// </summary>
        /// <returns>返回1级宝石等价金币</returns>
		protected double getGemPrice()
		{
			double price = 100.0;
			Dictionary<string, string> config = this._conf.getConfig("global");
			if (config.ContainsKey("gem_price"))
			{
				double.TryParse(config["gem_price"], out price);
			}
			if (price > 0.0)
			{
				price = 10.0 / price;
			}
			else
			{
				price = 0.1;
			}
			return price;
		}

		protected List<int> generateIds(string id_string)
		{
			List<int> list = new List<int>();
			if (id_string == null || id_string == "")
			{
				return list;
			}
			else
			{
				string[] ids = id_string.Split(',');
				for (int i = 0; i < ids.Length; i++)
				{
					string id = ids[i];
					if (id != null && id != "")
					{
						int nId = 0;
						int.TryParse(id, out nId);
						if (nId != 0)
						{
							list.Add(nId);
						}
					}
				}
                return list;
			}
		}

		protected void notifySingleExe(string exe_name)
		{
			this._server.notifySingleExe(exe_name);
		}

        public void setOtherConf(OtherConfig conf)
        {
            _otherConf = conf;
        }
    }
}
