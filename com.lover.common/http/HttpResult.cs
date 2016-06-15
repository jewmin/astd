using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace com.lover.common.http
{
	public class HttpResult
	{
		private Uri _currentUri;

		private int _statusCode;

		private string _contentType = "";

		private int _contentLength = -1;

		private bool _gzip;

		private bool _chunked;

		private Encoding _encoding = Encoding.UTF8;

		private MemoryStream _contentStream;

		private List<Cookie> _cookies = new List<Cookie>();

		private Dictionary<string, string> _extraHeaders = new Dictionary<string, string>();

		public Uri CurrentUri
		{
			get
			{
				return this._currentUri;
			}
			set
			{
				this._currentUri = value;
			}
		}

		public int StatusCode
		{
			get
			{
				return this._statusCode;
			}
		}

		public string ContentType
		{
			get
			{
				return this._contentType;
			}
		}

		public int ContentLength
		{
			get
			{
				return this._contentLength;
			}
		}

		public bool Gzip
		{
			get
			{
				return this._gzip;
			}
			set
			{
				this._gzip = value;
			}
		}

		public bool Chunked
		{
			get
			{
				return this._chunked;
			}
			set
			{
				this._chunked = value;
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

		public List<Cookie> getCookies()
		{
			return this._cookies;
		}

		public string getExtraHeader(string key)
		{
			string result;
			if (key == null || key.Length == 0)
			{
				result = null;
			}
			else
			{
				if (this._extraHeaders.ContainsKey(key.ToLower()))
				{
                    result = this._extraHeaders[key.ToLower()];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		public string getContent(Encoding encoding)
		{
			this._encoding = encoding;
			return this.getContent();
		}

		public void setContentStream(MemoryStream stream)
		{
			this._contentStream = stream;
		}

		public MemoryStream getDecodedStream()
		{
			MemoryStream result;
			if (this._gzip)
			{
				GZipStream gs = new GZipStream(this._contentStream, CompressionMode.Decompress);
				gs.Flush();
				MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[10240];
				while (true)
				{
                    int length = gs.Read(buffer, 0, buffer.Length);
					if (length <= 0)
					{
						break;
					}
                    ms.Write(buffer, 0, length);
				}
				ms.Seek(0, SeekOrigin.Begin);
				result = ms;
			}
			else
			{
				result = this._contentStream;
			}
			return result;
		}

		public string getContent()
		{
			string result;
			if (this._contentStream == null)
			{
				result = "";
			}
			else
			{
				if (this._gzip)
				{
					new MemoryStream();
					GZipStream gs = new GZipStream(this._contentStream, CompressionMode.Decompress);
					gs.Flush();
					MemoryStream ms = new MemoryStream();
					byte[] buffer = new byte[10240];
					while (true)
					{
						int length = gs.Read(buffer, 0, buffer.Length);
						if (length <= 0)
						{
							break;
						}
						ms.Write(buffer, 0, length);
					}
					ms.Seek(0, SeekOrigin.Begin);
					StreamReader reader = new StreamReader(ms, this._encoding);
					string text = reader.ReadToEnd();
					reader.Close();
					result = text;
				}
				else
				{
					StreamReader reader = new StreamReader(this._contentStream, this._encoding);
					string text = reader.ReadToEnd();
					reader.Close();
					result = text;
				}
			}
			return result;
		}

        public void parseHeader(string header_string)
        {
            if (header_string == null || header_string.Length == 0)
            {
                return;
            }
            string[] headers = header_string.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string head in headers)
            {
                if (head != null && head.Length != 0)
                {
                    if (head.ToLower().StartsWith("http/"))
                    {
                        this.parseStatus(head);
                    }
                    else if (head.ToLower().StartsWith("content-type"))
                    {
                        this._contentType = this.removePrefix(head);
                    }
                    else if (head.ToLower().StartsWith("content-encoding"))
                    {
                        this._gzip = this.removePrefix(head).Contains("gzip");
                    }
                    else if (head.ToLower().StartsWith("transfer-encoding"))
                    {
                        this._chunked = this.removePrefix(head).Contains("chunked");
                    }
                    else if (head.ToLower().StartsWith("content-length"))
                    {
                        int.TryParse(this.removePrefix(head), out this._contentLength);
                    }
                    else if (head.ToLower().StartsWith("set-cookie"))
                    {
                        Cookie cookie = CommonUtils.parseCookie(this.removePrefix(head));
                        if (cookie != null)
                        {
                            this._cookies.Add(cookie);
                        }
                    }
                    else
                    {
                        int index = head.IndexOf(':');
                        if (index >= 0)
                        {
                            string key = head.Substring(0, index);
                            string value = head.Substring(index + 1).Trim();
                            this.addExtraHeader(key.ToLower(), value);
                        }
                    }
                }
            }
        }

		private void parseStatus(string status)
		{
            if (status == null || status.Length == 0)
            {
                return;
            }
            string[] ts = status.Split(' ');
            if (ts.Length >= 3)
            {
                int.TryParse(ts[1], out this._statusCode);
            }
		}

		private void parseCookie(string cookie_string)
		{
            if (cookie_string == null || cookie_string.Length == 0)
            {
                return;
            }
            string[] ts = cookie_string.Split(';');
            Cookie cookie = new Cookie();
            int first = 0;
            foreach (string text in ts)
            {
                if (text != null && text.Length != 0)
                {
                    int index = text.IndexOf('=');
                    if (index >= 0)
                    {
                        string key = text.Substring(0, index).Trim();
                        string value = text.Substring(index + 1).Trim();
                        if (first == 0)
                        {
                            cookie.Name = key;
                            cookie.Value = value;
                            first++;
                        }
                        else if (key.ToLower().Equals("path"))
                        {
                            cookie.Path = value;
                        }
                        else if (key.ToLower().Equals("domain"))
                        {
                            cookie.Domain = value;
                        }
                        else if (key.ToLower().Equals("expires"))
                        {
                            DateTime expires;
                            if (DateTime.TryParse(value, out expires))
                            {
                                cookie.Expires = expires;
                            }
                        }
                    }
                }
            }
            if (cookie.Path == null || cookie.Path.Length == 0)
            {
                cookie.Path = this._currentUri.AbsolutePath;
            }
            if (cookie.Domain == null || cookie.Domain.Length == 0)
            {
                cookie.Domain = this._currentUri.Host;
            }
            this._cookies.Add(cookie);
		}

		private void addExtraHeader(string key, string value)
		{
			if (this._extraHeaders.ContainsKey(key))
			{
				this._extraHeaders[key] = this._extraHeaders[key] + ", " + value;
			}
			else
			{
				this._extraHeaders.Add(key, value);
			}
		}

		private string removePrefix(string input_str)
		{
			string result;
			if (input_str == null || input_str.Length == 0)
			{
				result = "";
			}
			else
			{
				result = input_str.Substring(input_str.IndexOf(':') + 1).Trim();
			}
			return result;
		}

		public static void copyStream(Stream input, Stream output)
		{
			byte[] buffer = new byte[2000];
			int count;
			while ((count = input.Read(buffer, 0, 2000)) > 0)
			{
				output.Write(buffer, 0, count);
			}
			output.Flush();
		}

		public static int readAllBytesFromStream(Stream stream, byte[] buffer)
		{
			int offset = 0;
			while (true)
			{
				int length = stream.Read(buffer, offset, 100);
				if (length == 0)
				{
					break;
				}
				offset += length;
			}
            return offset;
		}
	}
}
