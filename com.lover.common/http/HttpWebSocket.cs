using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace com.lover.common.http
{
	public class HttpWebSocket
	{
        /// <summary>
        /// ��ȡ�������������Ӧ�ĳ�ʱʱ��,Ĭ��3��.
        /// </summary>
		public int TimeOut
		{
			get;
			set;
		}

        /// <summary>
        /// ��ȡ����������cookie
        /// </summary>
		public List<string> Cookies
		{
			get;
			set;
		}

        /// <summary>
        /// ��ȡ���󷵻ص� HTTP ͷ������
        /// </summary>
		public HttpHeader HttpHeaders
		{
			get;
			internal set;
		}

        /// <summary>
        /// ��ȡ�����ô�����Ϣ�ָ���
        /// </summary>
        private string ErrorMessageSeparate;

		public HttpWebSocket()
		{
			TimeOut = 3;
			Cookies = new List<string>();
			ErrorMessageSeparate = ";;";
			HttpHeaders = new HttpHeader();
		}

        /// <summary>
        /// get��post��ʽ����һ�� http �� https ��ַ.ʹ�� Socket ��ʽ
        /// </summary>
        /// <param name="url">������Ե�ַ</param>
        /// <param name="referer">������Դ��ַ,��Ϊ��</param>
        /// <param name="postData">post�������. ���ÿ�ֵΪget��ʽ����</param>
        /// <returns>����ͼ��</returns>
        public Image GetImageUseSocket(string url, string referer, string postData = null)
        {
            Image result = null;
            MemoryStream ms = GetSocketResult(url, referer, postData);
            try
            {
                if (ms != null)
                {
                    result = Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// get��post��ʽ����һ�� http �� https ��ַ.ʹ�� Socket ��ʽ
        /// </summary>
        /// <param name="url">������Ե�ַ</param>
        /// <param name="postData">post�������. ���ÿ�ֵΪget��ʽ����</param>
        /// <returns>���� html ����,��������쳣�������ϴ�http״̬�뼰�쳣��Ϣ</returns>
		public string GetHtmlUseSocket(string url, string postData = null)
		{
			return GetHtmlUseSocket(url, null, postData);
		}

        /// <summary>
        /// get��post��ʽ����һ�� http �� https ��ַ.ʹ�� Socket ��ʽ
        /// </summary>
        /// <param name="url">������Ե�ַ</param>
        /// <param name="referer">������Դ��ַ,��Ϊ��</param>
        /// <param name="postData">post�������. ���ÿ�ֵΪget��ʽ����</param>
        /// <returns>���� html ����,��������쳣�������ϴ�http״̬�뼰�쳣��Ϣ</returns>
        public string GetHtmlUseSocket(string url, string referer, string postData = null)
        {
            string result = string.Empty;
            try
            {
                MemoryStream ms = GetSocketResult(url, referer, postData);
                if (ms != null)
                {
                    result = Encoding.GetEncoding(IsNullOrWhiteSpace(HttpHeaders.Charset) ? "UTF-8" : HttpHeaders.Charset).GetString(ms.ToArray());
                }
            }
            catch (SocketException ex)
            {
                result = HttpHeaders.ResponseStatusCode + ErrorMessageSeparate + ex.ErrorCode.ToString() + ErrorMessageSeparate + ex.SocketErrorCode.ToString("G") + ErrorMessageSeparate + ex.Message;
            }
            catch (Exception ex)
            {
                result = HttpHeaders.ResponseStatusCode + ErrorMessageSeparate + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// get��post��ʽ����һ�� http �� https ��ַ.
        /// </summary>
        /// <param name="url">������Ե�ַ</param>
        /// <param name="referer">������Դ��ַ,��Ϊ��</param>
        /// <param name="postData">post�������. ���ÿ�ֵΪget��ʽ����</param>
        /// <returns>���ص��ѽ�ѹ����������</returns>
        private MemoryStream GetSocketResult(string url, string referer, string postData)
        {
            if (IsNullOrWhiteSpace(url))
            {
                throw new UriFormatException("'Url' cannot be empty.");
            }
            MemoryStream result = null;
            Uri uri = new Uri(url);
            if (uri.Scheme == "http")
            {
                result = GetHttpResult(uri, referer, postData);
            }
            else if (uri.Scheme == "https")
            {
                result = GetSslResult(uri, referer, postData);
            }
            else
            {
                throw new ArgumentException("url must start with HTTP or HTTPS.", "url");
            }
            if (!IsNullOrWhiteSpace(HttpHeaders.Location))
            {
                result = GetSocketResult(HttpHeaders.Location, uri.AbsoluteUri, null);
            }
            else
            {
                result = unGzip(result);
            }
            return result;
        }

        /// <summary>
        /// get��post��ʽ����һ�� http ��ַ.
        /// </summary>
        /// <param name="uri">������Ե�ַ</param>
        /// <param name="referer">������Դ��ַ,��Ϊ��</param>
        /// <param name="postData">post�������. ���ÿ�ֵΪget��ʽ����</param>
        /// <returns>����δ��ѹ��������</returns>
        private MemoryStream GetHttpResult(Uri uri, string referer, string postData)
        {
            MemoryStream result = new MemoryStream(10240);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = TimeOut * 1000;
            socket.ReceiveTimeout = TimeOut * 1000;
            try
            {
                byte[] send = GetSendHeaders(uri, referer, postData);
                socket.Connect(uri.Host, uri.Port);
                if (socket.Connected)
                {
                    socket.Send(send, SocketFlags.None);
                    ProcessData<Socket>(socket, ref result);
                }
                result.Flush();
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        /// <summary>
        /// ��֤�ص�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        /// <summary>
        /// get��post��ʽ����һ�� https ��ַ.
        /// </summary>
        /// <param name="uri">������Ե�ַ</param>
        /// <param name="referer">������Դ��ַ,��Ϊ��</param>
        /// <param name="postData">post�������. ���ÿ�ֵΪget��ʽ����</param>
        /// <returns>����δ��ѹ��������</returns>
        private MemoryStream GetSslResult(Uri uri, string referer, string postData)
        {
            MemoryStream result = new MemoryStream(10240);
            byte[] send = this.GetSendHeaders(uri, referer, postData);
            TcpClient client = new TcpClient(uri.Host, uri.Port);
            try
            {
                SslStream sslStream = new SslStream(client.GetStream(), true, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                sslStream.ReadTimeout = TimeOut * 1000;
                sslStream.WriteTimeout = TimeOut * 1000;
                X509Store store = new X509Store(StoreName.My);
                sslStream.AuthenticateAsClient(uri.Host, store.Certificates, SslProtocols.Default, false);
                if (sslStream.IsAuthenticated)
                {
                    sslStream.Write(send, 0, send.Length);
                    sslStream.Flush();
                    ProcessData<SslStream>(sslStream, ref result);
                }
                result.Flush();
            }
            finally
            {
                client.Close();
            }
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        /// <summary>
        /// ���������ͷ������ Cache-Control: no-cache Cache-Control: max-age=2592000
        /// </summary>
        /// <param name="uri">������Ե�ַ</param>
        /// <param name="referer">������Դ��ַ,��Ϊ��</param>
        /// <param name="postData">post�������. ���ÿ�ֵΪget��ʽ����</param>
        /// <returns>����ͷ������</returns>
        private byte[] GetSendHeaders(Uri uri, string referer, string postData)
        {
            string sendString = @"{0} {1} HTTP/1.1
Accept: text/html, application/xhtml+xml, */*
Referer: {2}
Accept-Language: zh-CN
User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)
Accept-Encoding: gzip, deflate
Host: {3}
Connection: Keep-Alive
Cache-Control: no-cache
";
            sendString = string.Format(sendString, IsNullOrWhiteSpace(postData) ? "GET" : "POST", uri.PathAndQuery, IsNullOrWhiteSpace(referer) ? uri.AbsoluteUri : referer, uri.Host);
            if (Cookies != null && Cookies.Count > 0)
            {
                sendString += string.Format("Cookie: {0}\r\n", string.Join("; ", this.Cookies.ToArray()));
            }
            if (IsNullOrWhiteSpace(postData))
            {
                sendString += "\r\n";
            }
            else
            {
                sendString += string.Format(@"Content-Type: application/x-www-form-urlencoded
Content-Length: {0}

{1}
", postData.Length, postData);
            }
            return Encoding.UTF8.GetBytes(sendString);
        }

		private bool IsNullOrWhiteSpace(string tester)
		{
			return tester == null || tester.Trim().Length == 0;
		}

        /// <summary>
        /// ���ô�����ֶ�
        /// </summary>
        /// <param name="headText">ͷ���ı�</param>
        private void SetThisHeaders(string headText)
        {
            if (IsNullOrWhiteSpace(headText))
            {
                throw new ArgumentNullException("'WithHeadersText' cannot be empty.");
            }
            string[] headers = headText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (headers == null || headers.Length == 0)
            {
                throw new ArgumentException("'WithHeadersText' param format error.");
            }
            HttpHeaders = new HttpHeader();
            foreach (string head in headers)
            {
                if (head.StartsWith("HTTP", StringComparison.OrdinalIgnoreCase))
                {
                    string[] ts = head.Split(' ');
                    if (ts.Length > 1)
                    {
                        HttpHeaders.ResponseStatusCode = ts[1];
                    }
                }
                else if (head.StartsWith("Set-Cookie:", StringComparison.OrdinalIgnoreCase))
                {
                    Cookies = Cookies ?? new List<string>();
                    string tCookie = head.Substring(11, head.IndexOf(";") < 0 ? head.Length - 11 : head.IndexOf(";") - 10).Trim();
                    if (!Cookies.Exists(f => f.Split('=')[0] == tCookie.Split('=')[0]))
                    {
                        Cookies.Add(tCookie);
                    }
                }
                else if (head.StartsWith("Location:", StringComparison.OrdinalIgnoreCase))
                {
                    HttpHeaders.Location = head.Substring(9).Trim();
                }
                else if (head.StartsWith("Content-Encoding:", StringComparison.OrdinalIgnoreCase))
                {
                    if (head.IndexOf("gzip", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        HttpHeaders.IsGzip = true;
                    }
                }
                else if (head.StartsWith("Content-Type:", StringComparison.OrdinalIgnoreCase))
                {
                    string[] types = head.Substring(13).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string t in types)
                    {
                        if (t.IndexOf("charset=", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            HttpHeaders.Charset = t.Trim().Substring(8);
                        }
                        else if (t.IndexOf('/') >= 0)
                        {
                            HttpHeaders.ContentType = t.Trim();
                        }
                    }
                }
                else if (head.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase))
                {
                    HttpHeaders.ContentLength = long.Parse(head.Substring(15).Trim());
                }
                else if (head.StartsWith("Transfer-Encoding:", StringComparison.OrdinalIgnoreCase) && head.EndsWith("chunked", StringComparison.OrdinalIgnoreCase))
                {
                    HttpHeaders.IsChunk = true;
                }
            }
        }

        /// <summary>
        /// ��ѹ������
        /// </summary>
        /// <param name="data">������, ѹ����δѹ����.</param>
        /// <returns>���ؽ�ѹ����������</returns>
        private MemoryStream unGzip(MemoryStream data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data cannot be null.", "data");
            }
            data.Seek(0, SeekOrigin.Begin);
            MemoryStream result = data;
            if (HttpHeaders.IsGzip)
            {
                GZipStream gs = new GZipStream(data, CompressionMode.Decompress);
                result = new MemoryStream(1024);
                try
                {
                    byte[] buffer = new byte[1024];
                    int length = -1;
                    do
                    {
                        length = gs.Read(buffer, 0, buffer.Length);
                        result.Write(buffer, 0, length);
                    }
                    while (length != 0);
                    gs.Flush();
                    result.Flush();
                }
                finally
                {
                    gs.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// �������󷵻ص�����.
        /// </summary>
        /// <typeparam name="T">����Դ����</typeparam>
        /// <param name="reader">����Դʵ��</param>
        /// <param name="body">�������ݵ���</param>
        private void ProcessData<T>(T reader, ref MemoryStream body)
        {
            byte[] data = new byte[10240];
            int bodyStart = -1;//���ݲ�����ʼλ��
            int readLength = 0;
            bodyStart = GetHeaders<T>(reader, ref data, ref readLength);
            if (bodyStart >= 0)
            {
                if (HttpHeaders.IsChunk)
                {
                    GetChunkData<T>(reader, ref data, ref bodyStart, ref readLength, ref body);
                }
                else
                {
                    GetBodyData<T>(reader, ref data, bodyStart, readLength, ref body);
                }
            }
        }

        /// <summary>
        /// ȡ�÷��ص�httpͷ������,�������������.
        /// </summary>
        /// <typeparam name="T">����Դ����</typeparam>
        /// <param name="reader">����Դʵ��</param>
        /// <param name="data">�����������</param>
        /// <param name="readLength">��ȡ�ĳ���</param>
        /// <returns>�������ݵ���ʼλ��,����-1��ʾδ����ͷ������</returns>
        private int GetHeaders<T>(T reader, ref byte[] data, ref int readLength)
        {
            int result = -1;
            StringBuilder sb = new StringBuilder(1024);
            do
            {
                readLength = ReadData<T>(reader, ref data);
                if (result < 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        char c = (char)data[i];
                        sb.Append(c);
                        if (c == '\n' && string.Concat(sb[sb.Length - 4], sb[sb.Length - 3], sb[sb.Length - 2], sb[sb.Length - 1]).Contains("\r\n\r\n"))
                        {
                            result = i + 1;
                            SetThisHeaders(sb.ToString());
                            break;
                        }
                    }
                }
                if (result >= 0)
                {
                    break;
                }
            }
            while (readLength > 0);
            return result;
        }

        /// <summary>
        /// ȡ��δ�ֿ����ݵ�����
        /// </summary>
        /// <typeparam name="T">����Դ����</typeparam>
        /// <param name="reader">����Դʵ��</param>
        /// <param name="data">�Ѷ�ȡδ������ֽ�����</param>
        /// <param name="startIndex">��ʼλ��</param>
        /// <param name="readLength">��ȡ�ĳ���</param>
        /// <param name="body">��������ݵ���</param>
        private void GetBodyData<T>(T reader, ref byte[] data, int startIndex, int readLength, ref MemoryStream body)
        {
            int contentTotal = 0;
            if (startIndex < data.Length)
            {
                int count = readLength - startIndex;
                body.Write(data, startIndex, count);
                contentTotal += count;
            }
            int tlength = 0;
            do
            {
                tlength = ReadData<T>(reader, ref data);
                contentTotal += tlength;
                body.Write(data, 0, tlength);
                if (HttpHeaders.ContentLength > 0 && contentTotal >= HttpHeaders.ContentLength)
                {
                    break;
                }
            }
            while (tlength > 0);
        }

        /// <summary>
        /// ȡ�÷ֿ�����
        /// </summary>
        /// <typeparam name="T">����Դ����</typeparam>
        /// <param name="reader">����Դʵ��</param>
        /// <param name="data">�Ѷ�ȡδ������ֽ�����</param>
        /// <param name="startIndex">��ʼλ��</param>
        /// <param name="readLength">��ȡ�ĳ���</param>
        /// <param name="body">��������ݵ���</param>
		private void GetChunkData<T>(T reader, ref byte[] data, ref int startIndex, ref int readLength, ref MemoryStream body)
		{
            int chunkSize = -1;//ÿ�����ݿ�ĳ���,���ڷֿ�����.������Ϊ0ʱ,˵����������ĩβ.
            do
            {
                chunkSize = GetChunkHead<T>(reader, ref data, ref startIndex, ref readLength);
                GetChunkBody<T>(reader, ref data, ref startIndex, ref readLength, ref body, chunkSize);
            }
            while (chunkSize > 0);
		}

        /// <summary>
        /// ȡ�÷ֿ����ݵ����ݳ���
        /// </summary>
        /// <typeparam name="T">����Դ����</typeparam>
        /// <param name="reader">����Դʵ��</param>
        /// <param name="data">�Ѷ�ȡδ������ֽ�����</param>
        /// <param name="startIndex">��ʼλ��</param>
        /// <param name="readLength">��ȡ�ĳ���</param>
        /// <returns>�鳤��,����0��ʾ�ѵ�ĩβ.</returns>
		private int GetChunkHead<T>(T reader, ref byte[] data, ref int startIndex, ref int readLength)
		{
            int chunkSize = -1;
            List<char> tChars = new List<char>();//������ʱ�洢�鳤���ַ�
            if (startIndex >= data.Length || startIndex >= readLength)
			{
                readLength = ReadData<T>(reader, ref data);
				startIndex = 0;
			}
            do
            {
                for (int i = startIndex; i < readLength; i++)
                {
                    char c = (char)data[i];
                    if (c == '\n')
                    {
                        try
                        {
                            chunkSize = Convert.ToInt32(new string(tChars.ToArray()).TrimEnd('\r'), 16);
                            startIndex = i + 1;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Maybe exists 'chunk-ext' field.", ex);
                        }
                        break;
                    }
                    tChars.Add(c);
                }
                if (chunkSize >= 0)
                {
                    break;
                }
                startIndex = 0;
                readLength = ReadData<T>(reader, ref data);
            }
            while (readLength > 0);
            return chunkSize;
		}

        /// <summary>
        /// ȡ�÷ֿ鴫�ص���������
        /// </summary>
        /// <typeparam name="T">����Դ����</typeparam>
        /// <param name="reader">����Դʵ��</param>
        /// <param name="data">�Ѷ�ȡδ������ֽ�����</param>
        /// <param name="startIndex">��ʼλ��</param>
        /// <param name="readLength">��ȡ�ĳ���</param>
        /// <param name="body">��������ݵ���</param>
        /// <param name="chunkSize">�鳤��</param>
        private void GetChunkBody<T>(T reader, ref byte[] data, ref int startIndex, ref int readLength, ref MemoryStream body, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                return;
            }
            int chunkReadLength = 0;//ÿ�����ݿ��Ѷ�ȡ����
            if (startIndex >= data.Length || startIndex >= readLength)
            {
                readLength = ReadData<T>(reader, ref data);
                startIndex = 0;
            }
            do
            {
                int owing = chunkSize - chunkReadLength;
                int count = Math.Min(readLength - startIndex, owing);
                body.Write(data, startIndex, count);
                chunkReadLength += count;
                if (owing <= count)
                {
                    startIndex += count + 2;
                    break;
                }
                startIndex = 0;
                readLength = ReadData<T>(reader, ref data);
            }
            while (readLength > 0);
        }

        /// <summary>
        /// ������Դ��ȡ����
        /// </summary>
        /// <typeparam name="T">����Դ����</typeparam>
        /// <param name="reader">����Դʵ��</param>
        /// <param name="data">���ڴ洢��ȡ������</param>
        /// <returns>��ȡ�����ݳ���,������Ϊ-1</returns>
		private int ReadData<T>(T reader, ref byte[] data)
		{
			int result = -1;
			if (reader is Socket)
			{
				result = (reader as Socket).Receive(data, SocketFlags.None);
			}
			else if (reader is SslStream)
			{
                result = (reader as SslStream).Read(data, 0, data.Length);
			}
			return result;
		}
	}
}
