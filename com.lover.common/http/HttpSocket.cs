using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace com.lover.common.http
{
	public class HttpSocket
	{
		public string _proxy_host = "";

		public int _proxy_port;

		public int _proxy_type;

		private int _read_block_size = 10240;

		private string _url;

		private Uri _uri;

		private string _method = "GET";

		private string _data;

		private string _userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:26.0) Gecko/20100101 Firefox/26.0";

		private string _referer;

		private Encoding _encoding = Encoding.UTF8;

		private int _sendTimeout = 30;

		private int _recvTimeout = 30;

		private Dictionary<string, string> _extraHeaders = new Dictionary<string, string>();

		private List<Cookie> _cookies;

		private HttpResult _result = new HttpResult();

		public string Url
		{
			get
			{
				return this._url;
			}
			set
			{
				this._url = value;
                if (this._url == null || this._url.Length == 0)
                {
                    return;
                }
                this._uri = new Uri(this._url);
			}
		}

		public string Method
		{
			get
			{
				return this._method;
			}
			set
			{
				if (value.Equals("GET") || value.Equals("POST"))
				{
					this._method = value;
				}
			}
		}

		public string Data
		{
			get
			{
				return this._data;
			}
			set
			{
				this._data = value;
			}
		}

		public string UserAgent
		{
			get
			{
				return this._userAgent;
			}
			set
			{
				this._userAgent = value;
			}
		}

		public string Referer
		{
			get
			{
				return this._referer;
			}
			set
			{
				this._referer = value;
			}
		}

		public Encoding Encoding
		{
			get
			{
				return this._encoding;
			}
			set
			{
				this._encoding = value;
			}
		}

		public int SendTimeout
		{
			get
			{
				return this._sendTimeout;
			}
			set
			{
				this._sendTimeout = value;
			}
		}

		public int RecvTimeout
		{
			get
			{
				return this._recvTimeout;
			}
			set
			{
				this._recvTimeout = value;
			}
		}

		public Dictionary<string, string> getExtraHeaders()
		{
			return this._extraHeaders;
		}

		public void addExtraHeader(string key, string value)
		{
			if (this._extraHeaders.ContainsKey(key))
			{
				this._extraHeaders[key] = value;
			}
			else
			{
				this._extraHeaders.Add(key, value);
			}
		}

		public void setCookies(List<Cookie> cookies)
		{
			this._cookies = cookies;
		}

		public HttpResult execute()
		{
			HttpResult result;
			if (this._uri == null)
			{
				result = null;
			}
			else
			{
				this._result.CurrentUri = this._uri;
				if (this._uri.Scheme.Equals("http"))
				{
					this.executeHttp();
				}
                else if (this._uri.Scheme.Equals("https"))
                {
                    this.executeHttps();
                }
				result = this._result;
			}
			return result;
		}

        private void executeHttp()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = this._recvTimeout * 1000;
            socket.SendTimeout = this._sendTimeout * 1000;
            try
            {
                if (this._proxy_host != "" && this._proxy_port != 0)
                {
                    try
                    {
                        socket.Connect(this._proxy_host, this._proxy_port);
                    }
                    catch
                    {
                    }
                    if (!socket.Connected)
                    {
                        return;
                    }
                    string headerString = this.getHeaderString(true);
                    byte[] header = this._encoding.GetBytes(headerString);
                    socket.Send(header, SocketFlags.None);
                    if (this._method.Equals("POST"))
                    {
                        byte[] data = this._encoding.GetBytes(this._data);
                        socket.Send(data, SocketFlags.None);
                    }
                }
                else
                {
                    try
                    {
                        socket.Connect(this._uri.Host, this._uri.Port);
                    }
                    catch
                    {
                    }
                    if (!socket.Connected)
                    {
                        return;
                    }
                    string headerString = this.getHeaderString(false);
                    byte[] header = this._encoding.GetBytes(headerString);
                    socket.Send(header, SocketFlags.None);
                    if (this._method.Equals("POST"))
                    {
                        byte[] data = this._encoding.GetBytes(this._data);
                        socket.Send(data, SocketFlags.None);
                    }
                }
                MemoryStream data_stream = new MemoryStream();
                int length = 0;
                int body_start = 0;
                byte[] buffer = new byte[this._read_block_size];
                int rec_length = socket.Receive(buffer, 0, this._read_block_size, SocketFlags.None);
                data_stream.Write(buffer, 0, rec_length);
                length += rec_length;
                data_stream.Seek(0, SeekOrigin.Begin);
                this.processHeader(data_stream, length, ref body_start);
                MemoryStream body_stream = new MemoryStream();
                body_stream.Seek(0, SeekOrigin.Begin);
                int body_length = length - body_start;
                if (this._result.Chunked)
                {
                    this.getChunkedData<Socket>(ref socket, ref data_stream, ref body_stream, ref body_start, length);
                }
                else
                {
                    if (length - body_start > 0)
                    {
                        data_stream.Seek(body_start, SeekOrigin.Begin);
                        byte[] body_buffer = new byte[length - body_start];
                        data_stream.Read(body_buffer, 0, body_buffer.Length);
                        body_stream.Write(body_buffer, 0, body_buffer.Length);
                    }
                    if (this._result.ContentLength >= 0)
                    {
                        if (this._result.ContentLength > body_length)
                        {
                            body_stream.Seek(0, SeekOrigin.End);
                            this.readSocketData<Socket>(socket, ref body_stream, ref body_length, this._result.ContentLength);
                        }
                    }
                    else
                    {
                        if (rec_length >= this._read_block_size || (this._result.StatusCode == (int)HttpStatusCode.OK && body_start == length))
                        {
                            body_stream.Seek(0, SeekOrigin.End);
                            this.readSocketData<Socket>(socket, ref body_stream, ref body_length, 0);
                        }
                    }
                }
                socket.Close();
                socket = null;
                this.processBody(body_stream, body_length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error::" + ex.StackTrace);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                    socket = null;
                }
            }
        }

		private int getChunkedHead(ref byte[] tmpbuffer, ref int start_index)
		{
            int chunkSize = -1;
            List<char> tChars = new List<char>();
            for (int i = 0; i < tmpbuffer.Length; i++)
            {
                char c = (char)tmpbuffer[i];
                if (c == '\n')
                {
                    try
                    {
                        try
                        {
                            string text = new string(tChars.ToArray()).TrimEnd('\r');
                            if (text != null && text.Length > 0)
                            {
                                chunkSize = Convert.ToInt32(text, 16);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Maybe exists 'chunk-ext' field." + ex.StackTrace);
                        }
                        break;
                    }
                    finally
                    {
                        start_index += i + 1;
                    }
                }
                tChars.Add(c);
            }
            return chunkSize;
		}

		private void getChunkedBody<T>(ref T reader, ref MemoryStream data_stream, ref MemoryStream body_stream, ref int chunk_body_start, ref int chunk_size, ref int data_stream_length)
		{
			if (data_stream_length < chunk_body_start + chunk_size)
			{
				data_stream.Seek(0, SeekOrigin.End);
				while (data_stream_length < chunk_body_start + chunk_size)
				{
					byte[] buffer = new byte[1024];
					int length = this.readData<T>(ref reader, ref buffer);
					if (length == 0)
					{
						break;
					}
					data_stream.Write(buffer, 0, length);
					data_stream_length += length;
				}
			}
			if (data_stream_length >= chunk_body_start + chunk_size)
			{
				data_stream.Seek(chunk_body_start, SeekOrigin.Begin);
                byte[] buffer = new byte[chunk_size];
                data_stream.Read(buffer, 0, chunk_size);
                body_stream.Write(buffer, 0, chunk_size);
				chunk_body_start += chunk_size;
			}
		}

		private void getChunkedData<T>(ref T reader, ref MemoryStream data_stream, ref MemoryStream body_stream, ref int body_start, int stream_length)
		{
			while (true)
			{
				data_stream.Seek(body_start, SeekOrigin.Begin);
				byte[] buffer = new byte[1024];
				if (data_stream.Read(buffer, 0, buffer.Length) == 0)
				{
					int length = this.readData<T>(ref reader, ref buffer);
					if (length == 0)
					{
						break;
					}
					data_stream.Seek(0, SeekOrigin.End);
					data_stream.Write(buffer, 0, length);
					stream_length += length;
				}
				else
				{
					int chunkedHead = this.getChunkedHead(ref buffer, ref body_start);
					if (chunkedHead >= 0)
					{
						if (chunkedHead == 0)
						{
							break;
						}
						this.getChunkedBody<T>(ref reader, ref data_stream, ref body_stream, ref body_start, ref chunkedHead, ref stream_length);
					}
				}
			}
		}

        private int readData<T>(ref T reader, ref byte[] buffer)
		{
			int result = 0;
            if (reader is Socket)
			{
                result = (reader as Socket).Receive(buffer, 0, buffer.Length, SocketFlags.None);
			}
            else if (reader is SslStream)
            {
                result = (reader as SslStream).Read(buffer, 0, buffer.Length);
            }
			return result;
		}

		private void readSocketData<T>(T reader, ref MemoryStream recv_stream, ref int stream_length, int target_stream_length = 0)
		{
			int length = 0;
			do
			{
				byte[] buffer = new byte[this._read_block_size];
				try
				{
					if (reader is Socket)
					{
						length = (reader as Socket).Receive(buffer, 0, this._read_block_size, SocketFlags.None);
					}
                    else if (reader is SslStream)
                    {
                        length = (reader as SslStream).Read(buffer, 0, this._read_block_size);
                    }
				}
				catch (Exception ex)
				{
					length = 0;
					Console.WriteLine(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
				}
				recv_stream.Write(buffer, 0, length);
				stream_length += length;
				if (target_stream_length > 0 && target_stream_length <= stream_length)
				{
					break;
				}
			}
			while (length > 0);
		}

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

		private void executeHttps()
		{
			string headerString = this.getHeaderString(false);
			byte[] header = this._encoding.GetBytes(headerString);
			TcpClient client = null;
			try
			{
				client = new TcpClient(this._uri.Host, this._uri.Port);
                SslStream sslStream = new SslStream(client.GetStream(), true, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
				sslStream.ReadTimeout = this._recvTimeout * 1000;
				sslStream.WriteTimeout = this._sendTimeout * 1000;
				X509Store store = new X509Store(StoreName.My);
				sslStream.AuthenticateAsClient(this._uri.Host, store.Certificates, SslProtocols.Default, false);
				if (sslStream.IsAuthenticated)
				{
					sslStream.Write(header, 0, header.Length);
					sslStream.Flush();
					if (this._method.Equals("POST"))
					{
						byte[] data = this._encoding.GetBytes(this._data);
						sslStream.Write(data, 0, data.Length);
					}
					MemoryStream data_stream = new MemoryStream(10240);
					int stream_length = 0;
					int body_start = 0;
					byte[] buffer = new byte[this._read_block_size];
					int rec_length = sslStream.Read(buffer, 0, buffer.Length);
					data_stream.Write(buffer, 0, rec_length);
					stream_length += rec_length;
					data_stream.Seek(0, SeekOrigin.Begin);
					this.processHeader(data_stream, stream_length, ref body_start);
					MemoryStream body_stream = new MemoryStream();
					body_stream.Seek(0, SeekOrigin.Begin);
					int body_length = stream_length - body_start;
					if (this._result.Chunked)
					{
						this.getChunkedData<SslStream>(ref sslStream, ref data_stream, ref body_stream, ref body_start, stream_length);
					}
					else
					{
						if (stream_length - body_start > 0)
						{
							byte[] body_buffer = new byte[stream_length - body_start];
							data_stream.Read(body_buffer, 0, body_buffer.Length);
							body_stream.Write(body_buffer, 0, body_buffer.Length);
						}
						if (this._result.ContentLength >= 0)
						{
							if (this._result.ContentLength > body_length)
							{
								body_stream.Seek(0, SeekOrigin.End);
								this.readSocketData<SslStream>(sslStream, ref body_stream, ref body_length, this._result.ContentLength);
							}
						}
						else
						{
							if (rec_length >= this._read_block_size || (this._result.StatusCode == (int)HttpStatusCode.OK && body_start == stream_length))
							{
								body_stream.Seek(0, SeekOrigin.End);
								this.readSocketData<SslStream>(sslStream, ref body_stream, ref body_length, 0);
							}
						}
					}
					client.Close();
					client = null;
					this.processBody(body_stream, body_length);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("ERROR::{0}::{1}", ex.Message, ex.StackTrace));
			}
			finally
			{
				if (client != null)
				{
					client.Close();
					client = null;
				}
			}
		}

        private void processHeader(MemoryStream data_stream, int now_data_length, ref int body_start)
        {
            byte[] data = new byte[now_data_length];
            data_stream.Read(data, 0, now_data_length);
            StringBuilder sb = new StringBuilder(1024);
            for (int i = 0; i < now_data_length; i++)
            {
                char c = (char)data[i];
                sb.Append(c);
                if (c == '\n' && string.Concat(sb[sb.Length - 4], sb[sb.Length - 3], sb[sb.Length - 2], sb[sb.Length - 1]).Contains("\r\n\r\n"))
                {
                    body_start = i + 1;
                    break;
                }
            }
            this._result.Encoding = this._encoding;
            this._result.parseHeader(sb.ToString());
        }

		private void processBody(MemoryStream body_stream, int stream_length)
		{
			body_stream.Seek(0, SeekOrigin.Begin);
			this._result.setContentStream(body_stream);
		}

		private string getHeaderString(bool is_in_proxy)
		{
			StringBuilder sb = new StringBuilder();
			if (is_in_proxy)
			{
				sb.Append(string.Format("{0} {1} HTTP/1.1\r\n", this._method, this._url));
			}
			else
			{
				sb.Append(string.Format("{0} {1} HTTP/1.1\r\n", this._method, this._uri.PathAndQuery));
			}
			sb.Append(string.Format("Host: {0}\r\n", this._uri.Host));
			sb.Append(string.Format("User-Agent: {0}\r\n", this._userAgent));
			sb.Append("Accept: */*\r\n");
			sb.Append("Accept-Encoding: gzip, deflate\r\n");
			sb.Append("Connection: keep-alive\r\n");
			if (this._referer != null && this._referer.Length > 0)
			{
				sb.Append(string.Format("Referer: {0}\r\n", this._referer));
			}
			if (this._cookies != null && this._cookies.Count > 0)
			{
				string text = "";
				List<Cookie> cookieByUrl = CommonUtils.getCookieByUrl(this._cookies, this._url);
				foreach (Cookie current in cookieByUrl)
				{
					text += string.Format("{0}={1};", current.Name, current.Value);
				}
				sb.Append(string.Format("Cookie: {0}\r\n", text));
			}
			if (this._extraHeaders != null && this._extraHeaders.Count > 0)
			{
				IEnumerator<string> enumerator = this._extraHeaders.Keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					sb.Append(string.Format("{0}: {1}\r\n", enumerator.Current, this._extraHeaders[enumerator.Current]));
				}
			}
			if (this._method.Equals("POST"))
			{
				sb.Append("Content-Type: application/x-www-form-urlencoded\r\n");
				sb.Append(string.Format("Content-Length: {0}\r\n", this._data.Length));
			}
			sb.Append("\r\n");
			return sb.ToString();
		}
	}
}
