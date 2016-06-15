using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.lover.astd.common.manager;
using System.Net;
using System.IO;
using ComponentAce.Compression.Libs.zlib;
using com.lover.common;

namespace com.lover.astd.game.ui
{
    public partial class NewTranslator : Form
    {
        private TimeMgr _time_mgr;

        private List<Cookie> _cookies = new List<Cookie>();

        private string _gameUrl = "";

        private string _jsessionId = "";

        public NewTranslator(string gameUrl, string jsessionId)
        {
            InitializeComponent();
            _time_mgr = new TimeMgr();
            _gameUrl = gameUrl;
            Uri uri = new Uri(_gameUrl);
            if (uri.Port == 80 || uri.Port == 0)
            {
                _gameUrl = string.Format("{0}://{1}", uri.Scheme, uri.Host);
            }
            else
            {
                _gameUrl = string.Format("{0}://{1}:{2}", uri.Scheme, uri.Host, uri.Port);
            }
            _jsessionId = jsessionId;
            _cookies.Clear();
            Uri uri2 = new Uri(gameUrl);
            Cookie cookie = new Cookie();
            cookie.Path = "/root";
            cookie.Domain = uri2.Host;
            cookie.Name = "JSESSIONID";
            cookie.Value = jsessionId;
            _cookies.Add(cookie);
        }

        private void btn_openUrl_Click(object sender, EventArgs e)
        {
            if (txt_url.Text == "")
            {
                UiUtils.getInstance().info("还未输入网址");
                return;
            }
            CookieContainer cookieContainer = new CookieContainer(10000, 1000, 409600);
            if (web_view != null && web_view.Document != null)
            {
                string text = web_view.Document.Cookie;
                if (text != null)
                {
                    text = text.Replace(';', ',');
                    cookieContainer.SetCookies(new Uri(txt_url.Text), text);
                }
            }
            string url = string.Format("{0}{1}?{2}", _gameUrl, txt_url.Text, _time_mgr.TimeStamp);
            string result = "";
            if (txt_data.Text == "")
            {
                result = TransferMgr.doGet(url, ref _cookies);
            }
            else
            {
                result = TransferMgr.doPost(url, txt_data.Text, ref _cookies);
            }
            txt_result.Text = result;
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
            //MemoryStream memoryStream = new MemoryStream();
            //ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
            try
            {
                //TransferMgr.CopyStream(fileStream, zOutputStream);
                //memoryStream.Position = 0;
                //string text = new StreamReader(memoryStream).ReadToEnd();
                string text = new StreamReader(fileStream).ReadToEnd();
                txt_result.Text = text.Substring(text.IndexOf("<results>"));
            }
            catch (Exception ex)
            {
                UiUtils.getInstance().error("出错了, " + ex.Message);
            }
            finally
            {
                fileStream.Close();
                //memoryStream.Close();
                //zOutputStream.Close();
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_result == null || txt_result.Text == "")
            {
                UiUtils.getInstance().info("还没输入文件或网址呢");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CreatePrompt = true;
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            StreamWriter sw = new StreamWriter(saveFileDialog.OpenFile());
            sw.Write(txt_result.Text);
            sw.Flush();
            sw.Close();
        }
    }
}
