using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.lover.astd.common;
using com.lover.astd.common.model;
using com.lover.astd.common.config;
using com.lover.astd.common.manager;
using com.lover.astd.common.logic;
using com.lover.common.hook;
using System.Threading;
using com.lover.astd.common.partner;
using System.Net;
using Microsoft.Win32;
using com.lover.astd.common.model.enumer;
using com.lover.common;
using mshtml;
using com.lover.astd.common.model.battle;
using System.Globalization;
using com.lover.astd.common.model.building;
using com.lover.astd.weaponcalc;
using com.lover.astd.game.ui.ui;
using com.lover.astd.common.model.misc;
using com.lover.astd.common.logicexe.temp;
using com.lover.astd.common.logicexe.battle;
using com.lover.astd.common.logicexe.equip;
using com.lover.astd.game.ui.server.impl;
using com.lover.astd.game.ui.server.impl.troop;
using com.lover.astd.game.ui.server.impl.secretary;
using com.lover.astd.game.ui.server.impl.equipment;
using com.lover.astd.game.ui.server.impl.economy;
using com.lover.astd.game.ui.server.impl.battle;
using com.lover.astd.game.ui.server.impl.activities;
using com.lover.astd.common.logicexe;
using com.lover.astd.common.logicexe.building;
using com.lover.astd.common.logicexe.hero;
using com.lover.astd.common.logicexe.secretary;
using com.lover.astd.common.logicexe.economy;
using com.lover.astd.common.logicexe.activities;
using System.Timers;
using LuaInterface;

namespace com.lover.astd.game.ui
{
    public partial class NewMainForm : Form, ILogger, IServer
    {
        private delegate void SetLablePlayerInfoDelegate();

        private delegate void logMeDelegate(string logtext);

        private delegate void LogSingleDelegate(string logtext);

        private delegate void LogDelegate(string logtext, LogLevel level, Color logColor);

        private delegate void LogSurpriseDelegate(string logtext, Color logColor);

        private delegate void logTempDelegate(string text, LogLevel level, Color logColor);

        private delegate void renderUIDelegate();

        private class TempLogger : ILogger
        {
            private NewMainForm _frm;

            public TempLogger(NewMainForm frm)
            {
                _frm = frm;
            }

            public void logDebug(string text)
            {
                _frm.logTempSafe(text, LogLevel.Debug, Color.Red);
            }

            public void logError(string text)
            {
                _frm.logTempSafe(text, LogLevel.Error, Color.Red);
            }

            public void log(string text, Color color)
            {
                _frm.logTempSafe(text, LogLevel.Info, color);
            }

            public void logSingle(string text)
            {
                
            }

            public void logSurprise(string text)
            {
                
            }
            
            public void logInfo(string text)
            {
                _frm.logTempSafe(text, LogLevel.Info, Color.DodgerBlue);
            }
        }

        //private string _initialUrl = GlobalConfig.HomeUrl + "/astd/update";
        private string _initialUrl = GlobalConfig.HomeUrl;

        protected LogHelper _logger;

        private ServiceFactory _factory;

        private LuaMgr lua_mgr_;

        private LoginMgr _loginMgr;

        private bool _accountFromArgs;

        private DoKeyHook _keyHook;

        private LogLevel _logLevel = LogLevel.Info;

        private bool _intentExit;

        private System.Windows.Forms.Timer _serverTimeTimer;

        private System.Windows.Forms.Timer _refreshUiTimer;

        private ExeMgr _exeMgr;

        private SettingsMgr _settingMgr;

        private System.Timers.Timer _relogin_timer;

        private Thread _init_thread;

        private Thread _exe_thread;

        private int _serverStatus;

        private string _custome_session = "";

        private AccountData _account;

        private bool _auto_hide_flash;

        private User _gameUser;

        private Dictionary<string, int> _selectIndexes = new Dictionary<string, int>();

        private Thread _tempThread;

        private bool _doingTemp;

        private System.Windows.Forms.Timer _refreshTempTimer;

        private ITempExe _tempExe;

        private bool _isRelogin;

        private LoginResult _login_result;

        private string _jsessionid = "";

        private string _gameurl = "";

        private List<Cookie> _cookies = new List<Cookie>();

        private LogSingleDelegate _logSingleDelegate;

        private LogDelegate _logDelegate;

        private LogSurpriseDelegate _logSurpriseDelegate;

        private logTempDelegate _logTempSafe;

        private bool _server_running;

        private int _width;

        private int _height;

        public ServiceFactory Factory
        {
            get
            {
                return _factory;
            }
            set
            {
                _factory = value;
            }
        }

        public bool AccountFromArgs
        {
            get
            {
                return _accountFromArgs;
            }
            set
            {
                _accountFromArgs = value;
            }
        }

        public AccountData Account
        {
            get
            {
                return _account;
            }
        }

        public bool Auto_hide_flash
        {
            get
            {
                return _auto_hide_flash;
            }
            set
            {
                _auto_hide_flash = value;
            }
        }

        public User GameUser
        {
            get
            {
                return _gameUser;
            }
            set
            {
                _gameUser = value;
            }
        }

        public string JSessionId
        {
            get
            {
                return _jsessionid;
            }
        }

        public string Gameurl
        {
            get
            {
                return _gameurl;
            }
        }

        public GameConfig getConfig()
        {
            if (this._account != null)
            {
                return this._account.GameConf;
            }
            return null;
        }

        public void removeKeyHook()
        {
            HookUtils.removeKeyHook();
        }

        public void setKeyHook(bool shift, bool ctrl, bool alt, Keys key)
        {
            HookUtils.setKeyHook(shift, ctrl, alt, key, _keyHook);
        }

        private void toggleHide()
        {
            if (notify_icon.Visible)
            {
                hideMe();
                notify_icon.Visible = false;
                return;
            }
            showMe();
            notify_icon.Visible = true;
        }

        public void refreshPlayerInfo()
        {
            Text = string.Format("[{0}] <{1}>", string.Format("{0:s}({1:d}) - {2}", _gameUser.Username, _gameUser.Level, _account.getServerName()), GlobalConfig.Version);
            setPlayerInfoLabelSafe();
            setPlayerInfoNotify();
        }

        private void setPlayerInfoLabelSafe()
        {
            if (lbl_playerinfo.InvokeRequired)
            {
                lbl_playerinfo.Invoke(new SetLablePlayerInfoDelegate(setPlayerInfoLabel));
                return;
            }
            setPlayerInfoLabel();
        }

        private void setPlayerInfoLabel()
        {
            lbl_playerinfo.Text = string.Format("{0}[{1}级 {2} {3}{4}] 金币[{5}] 银币[{6}] 玉石[{7}] 军功[{8}] 行动力[{9}] 军令[{10}/50] 攻击令[{11}/50] 城防[{12}] 战绩[{13}] 决斗[{14}]", new object[]
			{
				_gameUser.Username,
				_gameUser.Level,
				_gameUser.Nation,
				_gameUser.Year,
				_gameUser.SeasonName,
				_gameUser.Gold,
				CommonUtils.getReadable((long)_gameUser.Silver),
				CommonUtils.getReadable((long)_gameUser.Stone),
				CommonUtils.getReadable((long)_gameUser.Credit),
				CommonUtils.getReadable((long)_gameUser.CurMovable),
				_gameUser.Token,
				_gameUser.AttackOrders,
				_gameUser._attack_cityHp,
				_gameUser._attack_battleScore,
                _gameUser._attack_daojuFlag
			});
        }

        private void setPlayerInfoNotify()
        {
            notify_icon.Text = string.Format("{0}{1}区-{2}", EnumString.getString(_account.Server_type), _account.ServerId, _gameUser.Username);
        }

        private int getDataGridRowIndexFromPoint(DataGridView dg, int px, int py)
        {
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                Rectangle rowDisplayRectangle = dg.GetRowDisplayRectangle(i, false);
                if (dg.RectangleToScreen(rowDisplayRectangle).Contains(px, py))
                {
                    return i;
                }
            }
            return -1;
        }

        public void addTempServer(string type, Dictionary<string, string> conf)
        {
            if (_tempExe != null)
            {
                UiUtils.getInstance().info("还有临时任务没有完成, 不能再开始新的临时任务");
                return;
            }
            if (type.Equals("campaign"))
            {
                _tempExe = new CampaignExe();
            }
            else if (type.Equals("equip_melt"))
            {
                _tempExe = new EquipMeltExe();
            }
            else if (type.Equals("wash"))
            {
                _tempExe = new CreditWashExe();
            }
            else if (type.Equals("ticket_exchange"))
            {
                _tempExe = new TicketExchangeExe();
            }
            else if (type.Equals("cut_rawstone"))
            {
                _tempExe = new RawStoneExe();
            }
            else if (type.Equals("dump_store"))
            {
                _tempExe = new DumpStoreExe();
            }
            else if (type.Equals("weave"))
            {
                _tempExe = new WeaveExe();
            }
            else if (type.Equals("juedou"))
            {
                _tempExe = new JuedouExe();
            }
            else if (type.Equals("hongbao"))
            {
                _tempExe = new HongbaoExe();
            }
            else if (type.Equals("open_all_trainer"))
            {
                _tempExe = new OpenAllTrainerExe();
            }
            if (_tempExe == null)
            {
                return;
            }
            _tempExe.setTarget(conf);
            lbl_tempStatus.Text = _tempExe.getStatus();
            ILogger logger = new TempLogger(this);
            ProtocolMgr proto = new ProtocolMgr(_gameUser, logger, this, _gameurl, _jsessionid, _factory);
            _tempExe.setVariables(proto, logger, this, _gameUser, _account.GameConf, _factory);
            _tempExe.setOtherConf(_account.OtherConf);
            _tempExe.init_data();
            startTempServer();
        }

        private void startTempServer()
        {
            stopTempServer();
            if (_tempExe == null)
            {
                return;
            }
            _refreshTempTimer = new System.Windows.Forms.Timer();
            _refreshTempTimer.Interval = 100;
            _refreshTempTimer.Tick += new EventHandler(_refreshTempTimer_Tick);
            _refreshTempTimer.Start();
            _doingTemp = true;
            _tempThread = new Thread(new ThreadStart(_temp_thread_proc));
            _tempThread.Start();
            btn_tempAdd.Enabled = false;
            btn_tempStop.Enabled = true;
        }

        private void stopTempServer()
        {
            _doingTemp = false;
            if (_refreshTempTimer != null && _refreshTempTimer.Enabled)
            {
                _refreshTempTimer.Stop();
                _refreshTempTimer.Dispose();
                _refreshTempTimer = null;
            }
            _tempThread = null;
            lbl_tempStatus.Text = "";
            btn_tempAdd.Enabled = true;
            btn_tempStop.Enabled = false;
        }

        public void hideMe()
        {
            Hide();
            if (_auto_hide_flash)
            {
                gameBrowser.Navigate(_initialUrl);
                log(string.Format("browser.navigate({0})", _initialUrl), Color.Green);
            }
        }

        public void showMe()
        {
            Show();
            BringToFront();
        }

        public void closeMe()
        {
            _intentExit = true;
            stopServer(true);
            _account = null;
            Close();
        }

        public void startRelogin()
        {
            _isRelogin = true;
            stop_relogin_timer();
            login_worker.CancelAsync();
            login_worker.RunWorkerAsync();
        }

        private void getCustomSession()
        {
            if (_account == null || _account.Server_type != ServerType.Custom)
            {
                return;
            }
            string gameurl = _account.CustomGameUrl;
            if (gameurl == null)
            {
                return;
            }
            if (gameurl.EndsWith("/"))
            {
                gameurl = gameurl.Substring(0, gameurl.Length - 1);
            }
            string url = gameurl + "/root/";
            subBrowser.Navigate(url);
            log(string.Format("browser.navigate({0})", url), Color.Green);
        }

        private void logMeSafe(string logtext)
        {
            if (logText.InvokeRequired && !logText.IsDisposed)
            {
                logText.Invoke(new logMeDelegate(logMe), new object[] { logtext });
                return;
            }
            logMe(logtext);
        }

        private void logMe(string logtext)
        {
            if (logText == null || logText.IsDisposed)
            {
                return;
            }
            string text = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", _factory.TmrMgr.DateTimeNow, logtext);
            logText.AppendText(text);
            logText.Select(logText.Text.Length - text.Length, text.Length);
            logText.SelectionColor = Color.Red;
            logText.Select(logText.Text.Length, 0);
            logText.AppendText("\n");
            logText.ScrollToCaret();
        }

        public void LogSingleSafe(string logtext)
        {
            if (_logSingleDelegate == null)
            {
                _logSingleDelegate = new LogSingleDelegate(LogSingle);
            }
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(_logSingleDelegate, new object[] { logtext });
                return;
            }
            LogSingle(logtext);
        }

        internal void LogSingle(string logtext)
        {
            lblStatus.Text = logtext;
            if (logtext == "挂机就绪")
            {
                menu_startServer.Enabled = true;
                menu_stopServer.Enabled = false;
            }
        }

        protected void LogSafe(string logtext, LogLevel level, Color logColor)
        {
            if (_logDelegate == null)
            {
                _logDelegate = new LogDelegate(Log);
            }
            if (logText.InvokeRequired && !logText.IsDisposed)
            {
                if (IsHandleCreated)
                {
                    logText.Invoke(_logDelegate, new object[] { logtext, level, logColor });
                }
                return;
            }
            Log(logtext, level, logColor);
        }

        protected void Log(string logtext, LogLevel level, Color logColor)
        {
            if (level < _logLevel)
            {
                return;
            }
            string text = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", _factory.TmrMgr.DateTimeNow, logtext);
            if (_logger != null)
            {
                _logger.log(text);
            }
            if (level >= LogLevel.Debug)
            {
                if (logText == null || logText.IsDisposed)
                {
                    return;
                }
                logText.AppendText(text);
                logText.Select(logText.Text.Length - text.Length, text.Length);
                logText.SelectionColor = logColor;
                logText.Select(logText.Text.Length, 0);
                logText.AppendText("\n");
                logText.ScrollToCaret();
            }
        }

        protected void LogFile(string logtext)
        {
            string text = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", _factory.TmrMgr.DateTimeNow, logtext);
            if (_logger != null)
            {
                _logger.log(text);
            }
        }

        protected void logSurpriseSafe(string logtext, Color logColor)
        {
            if (_logSurpriseDelegate == null)
            {
                _logSurpriseDelegate = new LogSurpriseDelegate(LogSurprise);
            }
            if (logSurpise.InvokeRequired)
            {
                logSurpise.Invoke(_logSurpriseDelegate, new object[] { logtext, logColor });
                return;
            }
            LogSurprise(logtext, logColor);
        }

        public void LogSurprise(string logtext, Color logColor)
        {
            if (logSurpise == null)
            {
                return;
            }
            string text = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", DateTime.Now, logtext);
            logSurpise.AppendText(text);
            logSurpise.Select(logSurpise.Text.Length - text.Length, text.Length);
            logSurpise.SelectionColor = logColor;
            logSurpise.Select(logSurpise.Text.Length, 0);
            logSurpise.AppendText("\n");
            logSurpise.ScrollToCaret();
        }

        public void LogTemp(string logtext, LogLevel level, Color logColor)
        {
            logTempSafe(logtext, level, logColor);
        }

        private void logTempSafe(string text, LogLevel level, Color logColor)
        {
            if (_logTempSafe == null)
            {
                _logTempSafe = new logTempDelegate(logTemperory);
            }
            if (logTemp.InvokeRequired)
            {
                logTemp.Invoke(_logTempSafe, new object[] { text, level, logColor });
                return;
            }
            logTemperory(text, level, logColor);
        }

        private void logTemperory(string logtext, LogLevel level, Color logColor)
        {
            if (level < _logLevel)
            {
                return;
            }
            string text = string.Format("[{0:MM-dd HH:mm:ss}]: {1}", this._factory.TmrMgr.DateTimeNow, logtext);
            if (_logger != null)
            {
                _logger.log(text);
            }
            if (level >= LogLevel.Debug)
            {
                if (logTemp == null)
                {
                    return;
                }
                logTemp.AppendText(text);
                logTemp.Select(logTemp.Text.Length - text.Length, text.Length);
                logTemp.SelectionColor = logColor;
                logTemp.Select(logText.Text.Length, 0);
                logTemp.AppendText("\n");
                logTemp.ScrollToCaret();
            }
        }

        public void initGameFromArgs(AccountData acc)
        {
            _account = acc;
            _exeMgr = new ExeMgr(_account.GameConf, _gameUser);
            _settingMgr = new SettingsMgr(_account.GameConf, _gameUser);
            _account.Status = AccountStatus.STA_not_start;
            if (_account.Server_type == ServerType.Custom)
            {
                gameBrowser.Navigate(_account.CustomLoginUrl);
                log(string.Format("browser.navigate({0})", _account.CustomLoginUrl), Color.Green);
                return;
            }
            string gameUrl = acc.GameUrl;
            foreach (Cookie current in CommonUtils.getCookieByUrl(acc.Cookies, gameUrl))
            {
                if (current != null)
                {
                    CommonUtils.setCookie(gameUrl, current.Name, current.Value, current.Path, current.Domain, current.Expires);
                }
            }
            Uri uri = new Uri(gameUrl);
            CommonUtils.setCookie(string.Format("{0}://{1}/", uri.Scheme, uri.Host), "JSESSIONID", acc.JsessionId, "/root/", uri.Host);
            _jsessionid = acc.JsessionId;
            _gameurl = acc.GameUrl;
            gameBrowser.Navigate(gameUrl);
            log(string.Format("browser.navigate({0})", gameUrl), Color.Green);
            start_delay_init(false);
        }

        public void initGame(AccountData acc, LoginResult state)
        {
            _account = acc;
            _exeMgr = new ExeMgr(_account.GameConf, _gameUser);
            _settingMgr = new SettingsMgr(_account.GameConf, _gameUser);
            _account.Status = AccountStatus.STA_not_start;
            if (_account.Server_type == ServerType.Custom)
            {
                gameBrowser.Navigate(_account.CustomLoginUrl);
                log(string.Format("browser.navigate({0})", _account.CustomLoginUrl), Color.Green);
                return;
            }
            string gameUrl = state.GameUrl;
            foreach (Cookie current in CommonUtils.getCookieByUrl(state.WebCookies, gameUrl))
            {
                if (current != null)
                {
                    CommonUtils.setCookie(gameUrl, current.Name, current.Value, current.Path, current.Domain, current.Expires);
                }
            }
            Uri uri = new Uri(gameUrl);
            CommonUtils.setCookie(string.Format("{0}://{1}/", uri.Scheme, uri.Host), "JSESSIONID", state.JSessionID, "/root/", uri.Host);
            _account.Cookies = state.WebCookies;
            _account.JsessionId = state.JSessionID;
            _account.GameUrl = state.GameUrl;
            _jsessionid = state.JSessionID;
            _gameurl = state.GameUrl;
            //object flags = null;
            //object targetFrameName = null;
            //object postData = null;
            //object headers = "Cache-Control: max-age=0";
            gameBrowser.Navigate(gameUrl);
            log(string.Format("browser.navigate({0})", gameUrl), Color.Green);
            start_delay_init(false);
        }

        private void init_session()
        {
            try
            {
                ProtocolMgr protocolMgr = new ProtocolMgr(_gameUser, this, this, _gameurl, _jsessionid, _factory);
                _factory.getMiscManager().getServerTime(protocolMgr, this);
                int playerInfo = _factory.getMiscManager().getPlayerInfo(protocolMgr, this, _account.RoleName, _gameUser);
                if (playerInfo == 2)
                {
                    UiUtils.getInstance().error("您选择的角色不存在");
                }
                else if (playerInfo == 3)
                {
                    UiUtils.getInstance().error("切换角色失败");
                }
                else if (playerInfo == 4)
                {
                    UiUtils.getInstance().error("角色被封号");
                }
                else if (playerInfo == 1 || playerInfo == 10)
                {
                    UiUtils.getInstance().error("获取用户信息失败, 请重试");
                }
                else
                {
                    if (!_accountFromArgs || UserSessionMgr.instance.isSf)
                    {
                        _account.GameConf = new GameConfig(EnumString.getString(_account.Server_type), _account.ServerId, _gameUser.Username);
                        _account.GameConf.loadSettings();
                        _account.OtherConf = new OtherConfig(EnumString.getString(_account.Server_type), _account.ServerId, _gameUser.Username);
                        _account.OtherConf.loadSettings();
                    }
                    if (_logger == null)
                    {
                        _logger = new LogHelper(EnumString.getString(_account.Server_type), _account.ServerId, _gameUser.Username, "");
                    }
                    buildServers();
                    lua_mgr_ = new LuaMgr(protocolMgr, this, _factory, _account.GameConf, _account.OtherConf, _gameUser);
                    _exeMgr.setExeVariables(protocolMgr, this, this, _gameUser, _account.GameConf, _account.OtherConf, _factory);
                    _exeMgr.init_data();
                    lua_mgr_.CreateVM(_exeMgr);
                    DbHelper.CreateTable(EnumString.getString(_account.Server_type), _account.ServerId, _gameUser.Username);
                    DbHelper.InsertUser((int)_account.Server_type, _account.ServerId, _gameUser.Id, _gameUser.Username);
                    _gameUser._db_userid = DbHelper.GetUserId((int)_account.Server_type, _account.ServerId, _gameUser.Id);
                    init_completed();
                }
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                _init_thread = null;
            }
        }

        private void init_data()
        {
            try
            {
                if (_exeMgr != null)
                {
                    _exeMgr.init_data();
                }
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                _init_thread = null;
            }
        }

        private void start_delay_init(bool is_only_init_data = false)
        {
            if (_init_thread == null)
            {
                if (is_only_init_data)
                {
                    _init_thread = new Thread(new ThreadStart(init_data));
                }
                else
                {
                    _init_thread = new Thread(new ThreadStart(init_session));
                }
                _init_thread.Start();
            }
        }

        private void buildServers()
        {
            _settingMgr.addSetting(new CommonServer(this));
            _settingMgr.addSetting(new BuildingServer(this));
            _settingMgr.addSetting(new HeroTrainServer(this));
            _settingMgr.addSetting(new HeroWashServer(this));
            _settingMgr.addSetting(new SecretaryServer(this));
            _settingMgr.addSetting(new QhServer(this));
            _settingMgr.addSetting(new QiFuServer(this));
            _settingMgr.addSetting(new BaiShenServer(this));
            _settingMgr.addSetting(new ShenHuoServer(this));
            _settingMgr.addSetting(new MhServer(this));
            _settingMgr.addSetting(new MerchantServer(this));
            _settingMgr.addSetting(new DailyWeaponServer(this));
            _settingMgr.addSetting(new WeaponServer(this));
            _settingMgr.addSetting(new StoreServer(this));
            _settingMgr.addSetting(new PolishServer(this));
            _settingMgr.addSetting(new DinnerServer(this));
            _settingMgr.addSetting(new FeteServer(this));
            _settingMgr.addSetting(new ImposeServer(this));
            _settingMgr.addSetting(new MarketServer(this));
            _settingMgr.addSetting(new MovableServer(this));
            _settingMgr.addSetting(new StockServer(this));
            _settingMgr.addSetting(new StoneServer(this));
            _settingMgr.addSetting(new BattleServer(this));
            _settingMgr.addSetting(new AttackServer(this));
            _settingMgr.addSetting(new ResCampaignServer(this));
            _settingMgr.addSetting(new ArchServer(this));
            _settingMgr.addSetting(new BoatServer(this));
            _settingMgr.addSetting(new YueBingServer(this));
            _settingMgr.addSetting(new CrossPlatformCompeteServer(this));
            _settingMgr.addSetting(new DailyTreasureGameServer(this));
            _settingMgr.addSetting(new GemFlopServer(this));
            _settingMgr.addSetting(new SilverFlopServer(this));
            _settingMgr.addSetting(new JailEventServer(this));
            _settingMgr.addSetting(new KfBanquetServer(this));
            _settingMgr.addSetting(new GiftEventServer(this));
            _settingMgr.addSetting(new PlayerCompeteServer(this));
            _settingMgr.addSetting(new TreasureGameServer(this));
            _settingMgr.addSetting(new WorldArmyServer(this));
            _settingMgr.addSetting(new FestivalEventServer(this));
            _settingMgr.addSetting(new TroopFeedbackServer(this));
            _settingMgr.addSetting(new SuperFanpaiServer(this));
            _settingMgr.addSetting(new TowerServer(this));
            _settingMgr.addSetting(new TroopTurntableServer(this));
            _settingMgr.addSetting(new CakeServer(this));
            _settingMgr.addSetting(new GemDumpServer(this));
            _settingMgr.addSetting(new NewDayTreasureGameServer(this));
            _settingMgr.addSetting(new BaoshiStoneServer(this));
            _settingMgr.addSetting(new WarChariotServer(this));
            _settingMgr.addSetting(new BGServer(this));
            _settingMgr.addSetting(new KfRankServer(this));
            _settingMgr.addSetting(new QingmingServer(this));
            _exeMgr.addExe(new CommonExe());
            _exeMgr.addExe(new PingExe());
            _exeMgr.addExe(new BuildingExe());
            _exeMgr.addExe(new HeroTrainExe());
            _exeMgr.addExe(new HeroWashExe());
            _exeMgr.addExe(new SecretaryExe());
            //_exeMgr.addExe(new MovableExe());
            _exeMgr.addExe(new QhExe());
            _exeMgr.addExe(new MhExe());
            _exeMgr.addExe(new QiFuExe());
            _exeMgr.addExe(new BaiShenExe());
            _exeMgr.addExe(new ShenHuoExe());
            _exeMgr.addExe(new MerchantExe());
            _exeMgr.addExe(new DailyWeaponExe());
            _exeMgr.addExe(new WeaponExe());
            _exeMgr.addExe(new StoreExe());
            _exeMgr.addExe(new PolishExe());
            _exeMgr.addExe(new DinnerExe());
            _exeMgr.addExe(new FeteExe());
            _exeMgr.addExe(new ImposeExe());
            _exeMgr.addExe(new MarketExe());
            _exeMgr.addExe(new StockExe());
            _exeMgr.addExe(new StoneExe());
            _exeMgr.addExe(new BattleExe());
            _exeMgr.addExe(new AttackExe());
            _exeMgr.addExe(new ResCampaignExe());
            _exeMgr.addExe(new ArchExe());
            _exeMgr.addExe(new BoatExe());
            _exeMgr.addExe(new YueBingExe());
            _exeMgr.addExe(new CrossplatformCompeteExe());
            _exeMgr.addExe(new DailyTreasureGameExe());
            _exeMgr.addExe(new GemFlopExe());
            _exeMgr.addExe(new SilverFlopExe());
            _exeMgr.addExe(new JailEventExe());
            _exeMgr.addExe(new KfBanquetExe());
            _exeMgr.addExe(new GiftEventExe());
            _exeMgr.addExe(new PlayerCompeteExe());
            _exeMgr.addExe(new TreasureGameExe());
            _exeMgr.addExe(new WorldArmyExe());
            _exeMgr.addExe(new FestivalEventExe());
            _exeMgr.addExe(new TroopFeedbackExe());
            _exeMgr.addExe(new SuperFanpaiExe());
            _exeMgr.addExe(new TowerExe());
            _exeMgr.addExe(new TroopTurntableExe());
            _exeMgr.addExe(new CakeExe());
            _exeMgr.addExe(new GemDumpExe());
            _exeMgr.addExe(new NewDayTreasureGameExe());
            _exeMgr.addExe(new BaoshiStoneExe());
            _exeMgr.addExe(new WarChariotExe());
            _exeMgr.addExe(new BGExe());
            _exeMgr.addExe(new KFRankExe());
            _exeMgr.addExe(new QingmingExe());
            _exeMgr.addExe(new BigHeroTrainExe());
        }

        public void startReLoginTimerImpl(int waitSeconds = 1800)
        {
            if (waitSeconds == 0)
            {
                startRelogin();
                return;
            }
            if (_relogin_timer != null)
            {
                return;
            }
            logMeSafe(string.Format("将在{0}后开始重新登录", (waitSeconds >= 60) ? string.Format("{0}分钟", waitSeconds / 60) : string.Format("{0}秒", waitSeconds)));
            start_relogin_timer(waitSeconds);
        }

        private void start_relogin_timer(int waitSeconds)
        {
            stop_relogin_timer();
            _relogin_timer = new System.Timers.Timer();
            _relogin_timer.Interval = (double)(waitSeconds * 1000);
            _relogin_timer.Elapsed += new ElapsedEventHandler(tmr_relogin_Tick);
            _relogin_timer.Start();
        }

        private void stop_relogin_timer()
        {
            if (_relogin_timer != null)
            {
                _relogin_timer.Stop();
                _relogin_timer = null;
            }
        }

        private void _exe_proc()
        {
            int num = 0;
            while (_server_running)
            {
                if (_exeMgr == null)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    try
                    {
                        if (num < 20)
                        {
                            num++;
                            if (_exe_thread != null)
                            {
                                Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            num = 0;
                            _exeMgr.fire(false);
                            LogSingleSafe("挂机中...");
                        }
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        logError(string.Format("error:{0}:{1}", ex.Message, ex.StackTrace));
                    }
                    catch (Exception ex)
                    {
                        logError(string.Format("error:{0}:{1}", ex.Message, ex.StackTrace));
                    }
                }
            }
        }

        public NewMainForm()
        {
            InitializeComponent();
            _width = Width;
            _height = Height;
        }

        public void logDebug(string text)
        {
            LogSafe(text, LogLevel.Debug, Color.Blue);
        }

        public void logError(string text)
        {
            LogSafe(text, LogLevel.Error, Color.Red);
        }

        public void log(string text, Color color)
        {
            LogSafe(text, LogLevel.Info, color);
        }

        public void logSingle(string text)
        {
            LogSingleSafe(text);
        }

        public void logSurprise(string text)
        {
            logSurpriseSafe(text, Color.Black);
        }

        public void logInfo(string text)
        {
            LogSafe(text, LogLevel.Info, Color.DodgerBlue);
        }

        public void init_completed()
        {
            if (_isRelogin)
            {
                startServer();
                return;
            }
            LogSingleSafe("挂机就绪");
            _serverStatus = 1;
        }

        public void startServer()
        {
            stopServer(false);
            _server_running = true;
            _exeMgr.clear_runtime();
            _exe_thread = new Thread(new ThreadStart(_exe_proc));
            _exe_thread.Start();
            _serverStatus = 2;
        }

        public void stopServer(bool is_user_operate = false)
        {
            _server_running = false;
            if (_exe_thread != null)
            {
                try
                {
                    _exe_thread.Join(0);
                    _exe_thread.Interrupt();
                    _exe_thread = null;
                }
                catch (Exception ex)
                {
                    logError(string.Format("error:{0}:{1}", ex.Message, ex.StackTrace));
                }
            }
            if (is_user_operate)
            {
                _serverStatus = 3;
                _isRelogin = false;
                return;
            }
            _serverStatus = 3;
            _isRelogin = true;
        }

        public void notifyState(common.model.enumer.AccountStatus status)
        {
            _account.Status = status;
        }

        public void startReLoginTimer()
        {
            startReLoginTimerImpl(1800);
        }

        public void refreshPlayerSafe()
        {
            _gameUser.addUiToQueue("global");
        }

        public void notifySingleExe(string exe_name)
        {
            if (_exeMgr == null)
            {
                return;
            }
            _exeMgr.fireSingleForce(exe_name);
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            SystemEvents.SessionEnding -= new SessionEndingEventHandler(this.SystemEvents_SessionEnding);
            closeMe();
        }

        private void refreshUiTimer()
        {
            try
            {
                if (_serverStatus == 0)
                {
                    menu_startServer.Enabled = false;
                    menu_stopServer.Enabled = false;
                    if (_account != null && _account.Server_type == ServerType.Custom)
                    {
                        menu_init.Enabled = true;
                    }
                    else
                    {
                        menu_init.Enabled = false;
                    }
                    menu_refresh.Enabled = false;
                    menu_default.Enabled = false;
                }
                else if (_serverStatus == 1)
                {
                    menu_startServer.Enabled = true;
                    menu_stopServer.Enabled = false;
                    menu_init.Enabled = true;
                    menu_refresh.Enabled = true;
                    menu_default.Enabled = true;
                }
                else if (_serverStatus == 2)
                {
                    menu_startServer.Enabled = false;
                    menu_stopServer.Enabled = true;
                    menu_init.Enabled = true;
                    menu_refresh.Enabled = true;
                    menu_default.Enabled = true;
                }
                else if (_serverStatus == 3)
                {
                    menu_startServer.Enabled = true;
                    menu_stopServer.Enabled = false;
                    menu_init.Enabled = true;
                    menu_refresh.Enabled = true;
                    menu_default.Enabled = true;
                }
                if (_settingMgr != null)
                {
                    _refreshUiTimer.Stop();
                    _settingMgr.renderSettings();
                    if (_exeMgr != null)
                    {
                        txt_servers.Text = _exeMgr.Status;
                    }
                    _refreshUiTimer.Start();
                }
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
        }

        private void _refreshUiTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                renderUIDelegate cb = new renderUIDelegate(refreshUiTimer);
                Invoke(cb);
            }
            catch (Exception ex)
            {
                Console.WriteLine("_refreshUiTimer_Tick::ERROR::{0}::{1}", ex.Message, ex.StackTrace);
            }
        }

        private void serverTimeTimer()
        {
            try
            {
                if (_factory == null)
                {
                    return;
                }
                DateTime dateTimeNow = _factory.TmrMgr.DateTimeNow;
                lbl_time.Text = dateTimeNow.ToString("G");
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
        }

        private void _serverTimeTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                renderUIDelegate cb = new renderUIDelegate(serverTimeTimer);
                Invoke(cb);
            }
            catch (Exception ex)
            {
                Console.WriteLine("_serverTimeTimer_Tick::ERROR::{0}::{1}", ex.Message, ex.StackTrace);
            }
        }

        private void _temp_thread_proc()
        {
            try
            {
                if (_tempExe != null)
                {
                    int num = 0;
                    long time = _tempExe.execute();
                    time += _factory.TmrMgr.TimeStamp;
                    while (_doingTemp && !_tempExe.isFinished())
                    {
                        if (num < 20)
                        {
                            Thread.Sleep(100);
                            num++;
                        }
                        else
                        {
                            num = 0;
                            if (time < _factory.TmrMgr.TimeStamp)
                            {
                                time = _tempExe.execute();
                                time += _factory.TmrMgr.TimeStamp;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
        }

        private void refreshTempTimer()
        {
            try
            {
                if (_tempExe == null)
                {
                    stopTempServer();
                    return;
                }
                lbl_tempStatus.Text = _tempExe.getStatus();
                if (_tempExe.isFinished())
                {
                    stopTempServer();
                    _tempExe = null;
                }
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
        }

        private void _refreshTempTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                renderUIDelegate cb = new renderUIDelegate(refreshTempTimer);
                Invoke(cb);
            }
            catch (Exception ex)
            {
                Console.WriteLine("_refreshTempTimer_Tick::ERROR::{0}::{1}", ex.Message, ex.StackTrace);
            }
        }

        private void tmr_relogin_Tick(object sender, EventArgs e)
        {
            try
            {
                stop_relogin_timer();
                startRelogin();
            }
            catch (Exception ex)
            {
                Console.WriteLine("tmr_relogin_Tick::ERROR::{0}::{1}", ex.Message, ex.StackTrace);
            }
        }

        private void subBrowser_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
        {
            try
            {
                if (_account == null || _account.Server_type != ServerType.Custom)
                {
                    return;
                }
                HTMLDocument hTMLDocument = (HTMLDocument)subBrowser.Document;
                if (hTMLDocument == null)
                {
                    return;
                }
                string cookie = hTMLDocument.cookie;
                if (cookie == null)
                {
                    return;
                }
                string[] array = cookie.Split(new char[] { ';' });
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i];
                    if (text != null)
                    {
                        string[] array2 = text.Split(new char[] { '=' });
                        if (array2.Length >= 2 && array2[0].Equals("JSESSIONID"))
                        {
                            _custome_session = array2[1];
                            _jsessionid = _custome_session;
                            _gameurl = _account.CustomGameUrl;
                            start_delay_init(false);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
        }

        private void notify_icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Visible)
            {
                hideMe();
                return;
            }
            showMe();
        }

        private void login_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CommonReceiver loginClient = new CommonReceiver(_account, login_worker);
            if (_login_result != null && _login_result.StatusCode == LoginStatusCode.NeedVerifyCode)
            {
                logMeSafe("重新登录需要验证码呀大哥, 貌似你还是关了再重新打开才行");
                return;
            }
            _cookies = new List<Cookie>();
            _login_result = _loginMgr.doLogin(loginClient, ref _cookies, null, null);
            e.Result = _login_result;
        }

        private void login_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string text = e.UserState as string;
            if (text == null)
            {
                return;
            }
            logMeSafe(text);
        }

        private void login_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoginResult loginResult = e.Result as LoginResult;
            if (loginResult == null)
            {
                logMeSafe("重新登录失败");
                startReLoginTimerImpl(30);
                return;
            }
            string desc = loginResult.StatusDesc;
            if (loginResult.StatusCode == LoginStatusCode.NeedVerifyCode)
            {
                this.logMeSafe("重新登录需要验证码呀大哥, 貌似你还是关了再重新打开才行");
                return;
            }
            if (loginResult.StatusCode == LoginStatusCode.Success)
            {
                initGame(_account, loginResult);
            }
            else
            {
                logMeSafe("重新登录失败");
                startReLoginTimerImpl(30);
            }
        }

        private void menu_init_Click(object sender, EventArgs e)
        {
            if (_account == null)
            {
                return;
            }
            if (_account.Server_type == ServerType.Custom && (_custome_session == null || _custome_session.Length == 0))
            {
                getCustomSession();
                return;
            }
            start_delay_init(true);
        }

        private void menu_saveSettings_Click(object sender, EventArgs e)
        {
            Validate();
            if (_settingMgr == null || _account == null || _account.GameConf == null)
            {
                UiUtils.getInstance().info("还未初始化成功");
                return;
            }
            _account.GameConf.clearSettingDict();
            _settingMgr.saveSettings();
            _account.GameConf.saveSettings();
            _account.GameConf.loadSettings();
            _account.OtherConf.saveSettings();
            _account.OtherConf.loadSettings();
            if (UserSessionMgr.instance.is_init_from_args)
            {
                string text = UserSessionMgr.instance.pushConfig();
                if (text.Length > 0)
                {
                    UiUtils.getInstance().info("上传配置到服务器失败, 请重试, " + text);
                }
            }
            UiUtils.getInstance().info("保存设置成功");
        }

        private void logText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            NewReportViewer viewer = new NewReportViewer(_gameurl);
            viewer.Show();
            viewer.BringToFront();
            viewer.setReport(e.LinkText);
        }

        private void menu_relogin_Click(object sender, EventArgs e)
        {
            if (_settingMgr == null || _account == null || _account.GameConf == null)
            {
                UiUtils.getInstance().info("还未初始化成功");
                return;
            }
            if (MessageBox.Show("你确定要刷新立即重新登录吗?", "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                startRelogin();
            }
        }

        private void menu_refresh_Click(object sender, EventArgs e)
        {
            if (_account == null || _account.GameConf == null)
            {
                UiUtils.getInstance().info("还未初始化成功");
                return;
            }
            gameBrowser.Navigate(_gameurl);
            log(string.Format("browser.navigate({0})", _gameurl), Color.Green);
        }

        private void menu_about_Click(object sender, EventArgs e)
        {
            UiUtils.getInstance().info("本程序仅用于内部学习与交流，仅供爱好者测试，请于测试后24小时内自行删除！");
        }

        private void menu_protocol_viewer_Click(object sender, EventArgs e)
        {
            if (_gameurl == null || _gameurl == "" || _jsessionid == null || _jsessionid == "")
            {
                UiUtils.getInstance().info("请先登录");
                return;
            }
            NewTranslator translator = new NewTranslator(_gameurl, _jsessionid);
            translator.Show();
            translator.BringToFront();
        }

        private void menu_clearLog_Click(object sender, EventArgs e)
        {
            logText.Text = "";
            logSurpise.Text = "";
            logTemp.Text = "";
        }

        private void menu_reportViewer_Click(object sender, EventArgs e)
        {
            NewReportViewer viewer = new NewReportViewer(_gameurl);
            viewer.Show();
            viewer.BringToFront();
        }

        private void menu_stopServer_Click(object sender, EventArgs e)
        {
            if (_account == null || _account.GameConf == null)
            {
                UiUtils.getInstance().info("还未初始化成功");
                return;
            }
            menu_startServer.Enabled = true;
            menu_stopServer.Enabled = false;
            stopServer(true);
        }

        private void menu_exit_Click(object sender, EventArgs e)
        {
            if ((_accountFromArgs && !UserSessionMgr.instance.isSf) || GlobalConfig.isManager)
            {
                Close();
                return;
            }
            if (MessageBox.Show("确认退出吗?", "确认退出", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                closeMe();
            }
        }

        private void menu_startServer_Click(object sender, EventArgs e)
        {
            if (_account == null || _account.GameConf == null)
            {
                UiUtils.getInstance().info("还未初始化成功");
                return;
            }
            menu_startServer.Enabled = false;
            menu_stopServer.Enabled = true;
            startServer();
        }

        private void menu_hideGame_Click(object sender, EventArgs e)
        {
            if (_gameurl == null || _gameurl == "")
            {
                UiUtils.getInstance().info("还未初始化");
                return;
            }
            if (gameBrowser.LocationURL.Equals(_initialUrl))
            {
                gameBrowser.Navigate(_gameurl);
                log(string.Format("browser.navigate({0})", _gameurl), Color.Green);
                return;
            }
            gameBrowser.Navigate(_initialUrl);
            log(string.Format("browser.navigate({0})", _initialUrl), Color.Green);
        }

        private void menu_test_Click(object sender, EventArgs e)
        {
            //ProtocolMgr protocolMgr = new ProtocolMgr(_gameUser, this, this, _gameurl, _jsessionid, _factory);
            //object[] result = lua_mgr_.CallFunction("movable_execute");
        }

        private void menu_default_Click(object sender, EventArgs e)
        {
            if (_settingMgr == null || _account == null || _account.GameConf == null)
            {
                UiUtils.getInstance().info("还未初始化成功");
                return;
            }
            if (MessageBox.Show("你确定要恢复默认设置吗?", "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                _settingMgr.loadDefaultSettings();
                _account.GameConf.saveSettings();
                _account.GameConf.loadSettings();
                _account.OtherConf.saveSettings();
                _account.OtherConf.loadSettings();
                UiUtils.getInstance().info("保存设置成功");
            }
        }

        private void btn_selForceNpc_Click(object sender, EventArgs e)
        {
            NewNpcSelector selector = new NewNpcSelector();
            selector.StartPosition = FormStartPosition.CenterParent;
            selector.setNpcs(_gameUser._all_npcs);
            selector.ShowDialog();
            Npc selectedNpc = selector.getSelectedNpc();
            if (selectedNpc == null)
            {
                return;
            }
            lbl_attackForceNpc.Text = string.Format("{0}:{1}:{2}:{3}", selectedNpc.Name, selectedNpc.ItemName, selector.getFormation(), selectedNpc.Id);
        }

        private void btn_selNpc_Click(object sender, EventArgs e)
        {
            NewNpcSelector selector = new NewNpcSelector();
            selector.StartPosition = FormStartPosition.CenterParent;
            selector.setNpcs(_gameUser._all_npcs);
            selector.ShowDialog();
            Npc selectedNpc = selector.getSelectedNpc();
            if (selectedNpc == null)
            {
                return;
            }
            lbl_attackNpc.Text = string.Format("{0}:{1}:{2}:{3}", selectedNpc.Name, selectedNpc.ItemName, selector.getFormation(), selectedNpc.Id);
        }

        private void btn_attack_target_Click(object sender, EventArgs e)
        {
            NewAttackTargetSelector selector = new NewAttackTargetSelector();
            selector.StartPosition = FormStartPosition.Manual;
            selector.Location = new Point(500, 400);
            selector.setCityInfo(_gameUser.getNewAreaCityInfo(), _gameUser._attack_selfCityId);
            selector.ShowDialog();
            lbl_attack_target.Text = selector.getCityname();
        }

        private void btn_sel_resCampaign_Click(object sender, EventArgs e)
        {
            if (_gameurl == null || _gameurl == "")
            {
                UiUtils.getInstance().info("还未初始化");
                return;
            }
            NewResCampaignSelector selector = new NewResCampaignSelector();
            selector.StartPosition = FormStartPosition.Manual;
            selector.Location = new Point(500, 400);
            selector.setMainForm(this);
            selector.ShowDialog();
            ResCampaign selectCampaign = selector.getSelectCampaign();
            if (selectCampaign == null)
            {
                return;
            }
            lbl_attack_resCampaign.Text = string.Format("{0}:{1}", selectCampaign.id, selectCampaign.name);
        }

        private void btn_weaponCalc_Click(object sender, EventArgs e)
        {
            new WeaponCalc { StartPosition = FormStartPosition.CenterParent }.ShowDialog();
        }

        private void gameBrowser_NewWindow3(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow3Event e)
        {
            try
            {
                gameBrowser.Navigate(e.bstrUrl);
                log(string.Format("browser.navigate({0})", e.bstrUrl), Color.Green);
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
            e.cancel = true;
        }

        private void gameBrowser_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
        {
            try
            {
                if (_account == null || _account.Server_type != ServerType.Custom)
                {
                    return;
                }
                HTMLDocument hTMLDocument = (HTMLDocument)gameBrowser.Document;
                if (hTMLDocument == null)
                {
                    return;
                }
                Uri uri = new Uri(hTMLDocument.url);
                Uri gameuri = new Uri(_account.CustomGameUrl);
                if (uri.Host.Equals(gameuri.Host) && uri.AbsolutePath.Equals(gameuri.AbsolutePath))
                {
                    getCustomSession();
                }
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
        }

        private void chk_selAll_building_CheckedChanged(object sender, EventArgs e)
        {
            if (dg_buildings.DataSource == null)
            {
                return;
            }
            BindingSource bindingSource = dg_buildings.DataSource as BindingSource;
            if (bindingSource == null)
            {
                return;
            }
            List<Building> list = bindingSource.DataSource as List<Building>;
            if (list == null)
            {
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                Building building = list[i];
                if (building != null)
                {
                    building.IsChecked = chk_selAll_building.Checked;
                }
            }
            dg_buildings.Refresh();
        }

        private void menu_login_Click(object sender, EventArgs e)
        {
            if (_account != null)
            {
                MessageBox.Show("小伙伴, 你已经登录了, 如果要更换账号的话, 还是关了再开吧, 要不就重新打开一个好了");
                return;
            }
            try
            {
                new NewLoginForm(this).ShowDialog();
            }
            catch (Exception ex)
            {
                logError(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
            }
        }

        private void btn_day_treasure_game_select_Click(object sender, EventArgs e)
        {
            if (_gameurl == null || _gameurl == "")
            {
                UiUtils.getInstance().info("还未初始化");
                return;
            }
            NewDailyTreasureGameSelector selector = new NewDailyTreasureGameSelector();
            selector.StartPosition = FormStartPosition.Manual;
            selector.Location = new Point(430, 350);
            selector.setMainForm(this);
            selector.ShowDialog();
            TGame selectCampaign = selector.getSelectCampaign();
            if (selectCampaign == null)
            {
                return;
            }
            lbl_day_treasure_game_sel.Text = string.Format("{0}:{1}", selectCampaign.tid, selectCampaign.powername);
        }

        private void dg_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            int idx = -1;
            if (_selectIndexes.ContainsKey(dataGridView.Name))
            {
                idx = this._selectIndexes[dataGridView.Name];
            }
            if (dataGridView.Rows.Count > 0 && dataGridView.SelectedRows.Count > 0 && dataGridView.SelectedRows[0].Index != idx)
            {
                if (dataGridView.Rows.Count <= idx)
                {
                    idx = dataGridView.Rows.Count - 1;
                }
                dataGridView.Rows[idx].Selected = true;
                dataGridView.CurrentCell = dataGridView.Rows[idx].Cells[0];
            }
        }

        private void dg_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dg_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridView dataGridView = sender as DataGridView;
                if (!_selectIndexes.ContainsKey(dataGridView.Name))
                {
                    _selectIndexes.Add(dataGridView.Name, e.RowIndex);
                    return;
                }
                _selectIndexes[dataGridView.Name] = e.RowIndex;
            }
        }

        private void dg_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Clicks < 2 && e.Button == MouseButtons.Left && e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                DataGridView dataGridView = sender as DataGridView;
                dataGridView.DoDragDrop(dataGridView.Rows[e.RowIndex], DragDropEffects.Move);
            }
        }

        private void dg_DragDrop(object sender, DragEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            int idx = -1;
            if (_selectIndexes.ContainsKey(dataGridView.Name))
            {
                idx = _selectIndexes[dataGridView.Name];
            }
            if (idx == -1)
            {
                return;
            }
            int dataGridRowIndexFromPoint = getDataGridRowIndexFromPoint(dataGridView, e.X, e.Y);
            if (dataGridRowIndexFromPoint == -1)
            {
                return;
            }
            BindingSource bs = dataGridView.DataSource as BindingSource;
            object value = bs[idx];
            bs.RemoveAt(idx);
            bs.Insert(dataGridRowIndexFromPoint, value);
        }

        private void dg_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dg_rowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView == null)
            {
                return;
            }
            BindingSource bindingSource = dataGridView.DataSource as BindingSource;
            if (bindingSource == null)
            {
                return;
            }
            List<Equipment> EquipmentList = bindingSource.DataSource as List<Equipment>;
            List<Npc> NpcList = bindingSource.DataSource as List<Npc>;
            if (EquipmentList == null && NpcList == null)
            {
                return;
            }
            if (EquipmentList != null && EquipmentList.Count <= e.RowIndex)
            {
                return;
            }
            if (NpcList != null && NpcList.Count <= e.RowIndex)
            {
                return;
            }
            DataGridViewRow dataGridViewRow = dataGridView.Rows[e.RowIndex];
            if (EquipmentList == null)
            {
                if (NpcList != null)
                {
                    string text = NpcList[e.RowIndex].ItemColor.TrimStart(new char[] { '#' });
                    if (text != "")
                    {
                        Color foreColor = Color.FromArgb(int.Parse(text.Substring(0, 2), NumberStyles.AllowHexSpecifier), int.Parse(text.Substring(2, 2), NumberStyles.AllowHexSpecifier), int.Parse(text.Substring(4, 2), NumberStyles.AllowHexSpecifier));
                        dataGridViewRow.DefaultCellStyle.ForeColor = foreColor;
                    }
                }
                return;
            }
            Equipment equipment = EquipmentList[e.RowIndex];
            if (equipment.Quality == EquipmentQuality.White)
            {
                dataGridViewRow.DefaultCellStyle.ForeColor = Color.Black;
                return;
            }
            if (equipment.Quality == EquipmentQuality.Blue)
            {
                dataGridViewRow.DefaultCellStyle.ForeColor = Color.Blue;
                return;
            }
            if (equipment.Quality == EquipmentQuality.Green)
            {
                dataGridViewRow.DefaultCellStyle.ForeColor = Color.Green;
                return;
            }
            if (equipment.Quality == EquipmentQuality.Yellow)
            {
                dataGridViewRow.DefaultCellStyle.ForeColor = Color.Yellow;
                return;
            }
            if (equipment.Quality == EquipmentQuality.Red)
            {
                dataGridViewRow.DefaultCellStyle.ForeColor = Color.Red;
                return;
            }
            dataGridViewRow.DefaultCellStyle.ForeColor = Color.Purple;
        }

        private void btn_tempAdd_Click(object sender, EventArgs e)
        {
            if (_settingMgr == null)
            {
                UiUtils.getInstance().error("还未初始化服务");
                return;
            }
            new NewTempServerForm(this).ShowDialog();
        }

        private void btn_tempStop_Click(object sender, EventArgs e)
        {
            if (_settingMgr == null)
            {
                UiUtils.getInstance().error("还未初始化服务");
                return;
            }
            stopTempServer();
            _tempExe = null;
        }

        private void NewMainForm_Load(object sender, EventArgs e)
        {
            _keyHook = new DoKeyHook(toggleHide);
            if (GlobalConfig.isDebug)
            {
                _logLevel = LogLevel.Debug;
            }
            _factory = new ServiceFactory();
            _gameUser = new User(_factory);
            _loginMgr = new LoginMgr();
            Text = GlobalConfig.Version;
            gameBrowser.Navigate(_initialUrl);
            log(string.Format("browser.navigate({0})", _initialUrl), Color.Green);
            //if (!GlobalConfig.isDebug)
            //{
            //    menu_test.Visible = false;
            //}
            if (_accountFromArgs && !UserSessionMgr.instance.isSf)
            {
                menu_login.Visible = false;
                menu_about.Visible = false;
                menu_more.Visible = false;
                menu_relogin.Visible = false;
                menu_exit.Visible = false;
                notify_icon.Visible = false;
            }
            if (GlobalConfig.isManager)
            {
                menu_login.Visible = false;
                menu_about.Visible = false;
                menu_more.Visible = false;
                menu_relogin.Visible = false;
                menu_exit.Visible = false;
                menu_startServer.Visible = false;
                menu_stopServer.Visible = false;
                menu_init.Visible = false;
                setPage.Parent = null;
                setPage2.Parent = null;
                notify_icon.Visible = false;
                txt_servers.Visible = false;
            }
            menu_refresh.Enabled = false;
            menu_startServer.Enabled = false;
            menu_stopServer.Enabled = false;
            menu_default.Enabled = false;
            SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
            _serverTimeTimer = new System.Windows.Forms.Timer();
            _serverTimeTimer.Interval = 1000;
            _serverTimeTimer.Tick += new EventHandler(_serverTimeTimer_Tick);
            _serverTimeTimer.Start();
            _refreshUiTimer = new System.Windows.Forms.Timer();
            _refreshUiTimer.Interval = 500;
            _refreshUiTimer.Tick += new EventHandler(_refreshUiTimer_Tick);
            _refreshUiTimer.Start();
            if (_accountFromArgs)
            {
                initGameFromArgs(UserSessionMgr.instance.Account);
            }
        }

        private void NewMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((_accountFromArgs && !UserSessionMgr.instance.isSf) || GlobalConfig.isManager)
            {
                if (MessageBox.Show("确认退出吗?", "确认退出", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (!this._intentExit)
            {
                hideMe();
                e.Cancel = true;
            }
        }

        private void NewMainForm_Resize(object sender, EventArgs e)
        {
            int diff_width = _width - Width;
            int diff_height = _height - Height;
            lblStatus.Location = new Point(lblStatus.Location.X, lblStatus.Location.Y - diff_height);
            lbl_playerinfo.Location = new Point(lbl_playerinfo.Location.X, lbl_playerinfo.Location.Y - diff_height);
            lbl_playerinfo.Width = lbl_playerinfo.Width - diff_width;
            mainTab.Width = mainTab.Width - diff_width;
            mainTab.Height = mainTab.Height - diff_height;
            gameBrowser.Width = gameBrowser.Width - diff_width;
            gameBrowser.Height = gameBrowser.Height - diff_height;
            _width = Width;
            _height = Height;
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeMe();
        }

        private void menu_lua_Click(object sender, EventArgs e)
        {
            menu_stopServer_Click(null, null);
            lua_mgr_.CreateVM(_exeMgr);
        }

        private void menu_train_hero_Click(object sender, EventArgs e)
        {
            if (_gameurl == null || _gameurl == "" || _jsessionid == null || _jsessionid == "")
            {
                UiUtils.getInstance().info("请先登录");
                return;
            }
            NewTrainHeroCalc trainHeroCalc = new NewTrainHeroCalc(this);
            trainHeroCalc.Show();
            trainHeroCalc.BringToFront();
        }
    }
}
