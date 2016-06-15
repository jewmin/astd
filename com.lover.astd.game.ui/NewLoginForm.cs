using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.lover.astd.common.model.enumer;
using com.lover.astd.common.model;
using com.lover.astd.common.partner;
using com.lover.astd.common.manager;
using com.lover.astd.common.config;
using com.lover.common;
using System.Net;
using com.lover.astd.common;

namespace com.lover.astd.game.ui
{
    public partial class NewLoginForm : Form
    {
        private class SMeta
        {
            private ServerType _stype;

            private string _desc;

            public ServerType Stype
            {
                get
                {
                    return _stype;
                }
                set
                {
                    _stype = value;
                }
            }

            public string Desc
            {
                get
                {
                    return _desc;
                }
                set
                {
                    _desc = value;
                }
            }

            public SMeta(ServerType tp, string desc)
            {
                _stype = tp;
                _desc = desc;
            }
        }

        private NewMainForm _form;

        private List<SMeta> _serverMetas;

        private Dictionary<ServerType, List<AccountData>> _users;

        private AccountData _acc_to_login;

        private LoginResult _login_result;

        private LoginMgr _loginMgr;

        private void loadUsers()
        {
            GlobalConfig.getInstance().loadSettings();
            _users = GlobalConfig.getInstance().getAllAccountByType();
            AccountData lastLoginAccount = GlobalConfig.getInstance().getLastLoginAccount();
            int selectedIndex = -1;
            if (_users.Count > 0)
            {
                selectedIndex = 0;
            }
            if (lastLoginAccount != null)
            {
                for (int i = 0; i < _serverMetas.Count; i++)
                {
                    if (_serverMetas[i].Stype == lastLoginAccount.Server_type)
                    {
                        selectedIndex = i;
                        break;
                    }
                }
            }
            comboServerList.SelectedIndex = selectedIndex;
        }

        private void getServerUsers()
        {
            SMeta sMeta = comboServerList.SelectedItem as SMeta;
            if (sMeta == null)
            {
                return;
            }
            if (!_users.ContainsKey(sMeta.Stype))
            {
                return;
            }
            List<AccountData> list = _users[sMeta.Stype];
            if (sMeta.Stype == ServerType.Custom)
            {
                txt_customLoginUrl.Enabled = true;
                txt_customGameUrl.Enabled = true;
                combo_username.Enabled = false;
                txt_serverId.Enabled = false;
                txt_password.Enabled = false;
                txt_roleName.Enabled = true;
                combo_username.Text = "";
                txt_serverId.Value = decimal.Zero;
                txt_password.Text = "";
                if (list.Count == 0)
                {
                    return;
                }
                AccountData accountData = list[0];
                txt_roleName.Text = accountData.RoleName;
                txt_customLoginUrl.Text = accountData.CustomLoginUrl;
                txt_customGameUrl.Text = accountData.CustomGameUrl;
            }
            else
            {
                txt_customLoginUrl.Enabled = false;
                txt_customGameUrl.Enabled = false;
                combo_username.Enabled = true;
                txt_serverId.Enabled = true;
                txt_password.Enabled = true;
                txt_roleName.Enabled = true;
                txt_roleName.Text = "";
                txt_customLoginUrl.Text = "";
                txt_customGameUrl.Text = "";
                combo_username.DataSource = list;
            }
            AccountData lastLoginAccount = GlobalConfig.getInstance().getLastLoginAccount();
            if (list.Count > 0)
            {
                int selectedIndex = 0;
                if (lastLoginAccount != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].UserName == lastLoginAccount.UserName && list[i].RoleName == lastLoginAccount.RoleName)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }
                }
                if (sMeta.Stype != ServerType.Custom)
                {
                    combo_username.SelectedIndex = selectedIndex;
                    return;
                }
            }
            else
            {
                combo_username.Text = "";
                txt_serverId.Value = decimal.Zero;
                txt_password.Text = "";
                txt_roleName.Text = "";
            }
        }

        private void getUserInfo()
        {
            AccountData accountData = combo_username.SelectedItem as AccountData;
            if (accountData == null)
            {
                txt_serverId.Value = decimal.Zero;
                txt_password.Text = "";
                txt_roleName.Text = "";
                return;
            }
            txt_serverId.Value = accountData.ServerId;
            txt_password.Text = accountData.Password;
            txt_roleName.Text = accountData.RoleName;
        }

        private AccountData collectUserInfo()
        {
            if (this.comboServerList.SelectedIndex < 0)
            {
                UiUtils.getInstance().info("大虾，表糊弄我了，表还没填完呢，想进去免谈！！");
                return null;
            }
            SMeta sMeta = comboServerList.SelectedItem as SMeta;
            if (sMeta.Stype == ServerType.Custom)
            {
                if (txt_customLoginUrl.Text == "" || txt_customGameUrl.Text == "")
                {
                    UiUtils.getInstance().info("大虾，表糊弄我了，表还没填完呢，想进去免谈！！");
                    return null;
                }
            }
            else if (txt_serverId.Value <= decimal.Zero || combo_username.Text == "" || txt_password.Text == "")
            {
                UiUtils.getInstance().info("大虾，表糊弄我了，表还没填完呢，想进去免谈！！");
                return null;
            }
            int serverId = (int)txt_serverId.Value;
            string rolename = txt_roleName.Text;
            string username = combo_username.Text;
            string password = txt_password.Text;
            string loginurl = txt_customLoginUrl.Text;
            if (loginurl.IndexOf("http://") < 0 && loginurl.IndexOf("https://") < 0)
            {
                loginurl = "http://" + loginurl;
            }
            string gameurl = txt_customGameUrl.Text;
            if (gameurl.IndexOf("http://") < 0 && gameurl.IndexOf("https://") < 0)
            {
                gameurl = "http://" + gameurl;
            }
            AccountData accountData = null;
            if (combo_username.DataSource != null)
            {
                foreach (AccountData current in (combo_username.DataSource as List<AccountData>))
                {
                    if (current.UserName.Equals(username) && current.RoleName == rolename)
                    {
                        accountData = current;
                        break;
                    }
                }
            }
            if (accountData == null)
            {
                accountData = new AccountData();
                accountData.Server_type = sMeta.Stype;
                accountData.ServerId = serverId;
                accountData.UserName = username;
                accountData.Password = password;
                accountData.RoleName = rolename;
                accountData.CustomLoginUrl = loginurl;
                accountData.CustomGameUrl = gameurl;
                accountData.IsLastLogin = true;
                GlobalConfig.getInstance().addAccount(accountData);
            }
            else
            {
                accountData.Server_type = sMeta.Stype;
                accountData.ServerId = serverId;
                accountData.UserName = username;
                accountData.Password = password;
                accountData.RoleName = rolename;
                accountData.CustomLoginUrl = loginurl;
                accountData.CustomGameUrl = gameurl;
                accountData.IsLastLogin = true;
                GlobalConfig.getInstance().setLastLoginAccount(accountData);
            }
            return accountData;
        }

        public NewLoginForm(NewMainForm form)
        {
            InitializeComponent();
            _loginMgr = new LoginMgr();
            _form = form;
            _users = new Dictionary<ServerType, List<AccountData>>();
            _serverMetas = new List<SMeta>();
            foreach (ServerType tp in Enum.GetValues(typeof(ServerType)))
            {
                _serverMetas.Add(new SMeta(tp, EnumString.getString(tp)));
            }
            comboServerList.DataSource = _serverMetas;
            comboServerList.DisplayMember = "Desc";
            loadUsers();
        }

        private void btn_delAccount_Click(object sender, EventArgs e)
        {
            if (comboServerList.SelectedIndex < 0)
            {
                return;
            }
            AccountData accountData = new AccountData();
            SMeta sMeta = comboServerList.SelectedItem as SMeta;
            accountData.Server_type = sMeta.Stype;
            accountData.ServerId = (int)txt_serverId.Value;
            accountData.UserName = combo_username.Text;
            accountData.Password = txt_password.Text;
            accountData.RoleName = txt_roleName.Text;
            if (accountData.UserName == "" || accountData.ServerId == 0)
            {
                return;
            }
            GlobalConfig.getInstance().delAccount(accountData);
            this.loadUsers();
        }

        private void btn_refresh_verify_code_Click(object sender, EventArgs e)
        {
            if (_login_result == null)
            {
                UiUtils.getInstance().error("程序出错了, 请关闭重新打开");
                return;
            }
            List<Cookie> webCookies = _login_result.WebCookies;
            Image captchaImage = _loginMgr.getCaptchaImage(_login_result.CaptchaUrl, ref webCookies);
            if (captchaImage == null)
            {
                UiUtils.getInstance().error("获取验证图片失败, 请重试");
                return;
            }
            img_verify_code.Image = captchaImage;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string host = txt_proxy_host.Text;
            int port = (int)num_proxy_port.Value;
            if (host != "" && port > 0)
            {
                TransferMgr._proxy_host = host;
                TransferMgr._proxy_port = port;
            }
            else
            {
                TransferMgr._proxy_host = "";
                TransferMgr._proxy_port = 0;
            }
            _acc_to_login = collectUserInfo();
            if (_acc_to_login == null)
            {
                return;
            }
            if (_acc_to_login.Server_type == ServerType.Custom)
            {
                _form.initGame(_acc_to_login, null);
                Close();
                return;
            }
            login_worker.CancelAsync();
            btn_refresh_verify_code.Enabled = false;
            txt_verify_code.Enabled = false;
            btn_login.Enabled = false;
            login_worker.RunWorkerAsync();
        }

        private void login_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CommonReceiver loginClient = new CommonReceiver(_acc_to_login, login_worker);
            if (_login_result != null && _login_result.StatusCode == LoginStatusCode.NeedVerifyCode)
            {
                List<Cookie> webCookies = _login_result.WebCookies;
                _login_result = _loginMgr.doLogin(loginClient, ref webCookies, txt_verify_code.Text, _login_result.CaptchaExtra);
            }
            else
            {
                List<Cookie> list = new List<Cookie>();
                _login_result = _loginMgr.doLogin(loginClient, ref list, null, null);
            }
            e.Result = _login_result;
        }

        private void login_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string state = e.UserState as string;
            if (state == null)
            {
                return;
            }
            lbl_login_status.Text = state;
        }

        private void login_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_login.Enabled = true;
            LoginResult loginResult = e.Result as LoginResult;
            if (loginResult == null)
            {
                lbl_login_status.Text = "登录失败, 请重试下吧";
                return;
            }
            string statusDesc = loginResult.StatusDesc;
            if (loginResult.StatusCode == LoginStatusCode.NeedVerifyCode)
            {
                if (loginResult.CaptchaImage == null)
                {
                    lbl_login_status.Text = "获取验证码图片失败, 请重试";
                    return;
                }
                img_verify_code.Image = loginResult.CaptchaImage;
                btn_refresh_verify_code.Enabled = true;
                txt_verify_code.Enabled = true;
            }
            if (loginResult.StatusCode == LoginStatusCode.Success)
            {
                _form.initGame(this._acc_to_login, loginResult);
                Close();
            }
            else
            {
                lbl_login_status.Text = statusDesc;
            }
        }

        private void NewLoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (login_worker.CancellationPending)
            {
                return;
            }
            if (login_worker.IsBusy)
            {
                login_worker.CancelAsync();
            }
        }

        private void comboServerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            getServerUsers();
        }

        private void combo_username_SelectedIndexChanged(object sender, EventArgs e)
        {
            getUserInfo();
        }
    }
}
