using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace com.lover.common
{
	public class SockStateObject
	{
		private int _bufferSize = 1024;

		private byte[] _buffer;

		private Socket _sock;

		private DateTime _lastRefreshTime;

		private StringBuilder _strBuilder;

		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
		}

		public byte[] Buffer
		{
			get
			{
				return this._buffer;
			}
		}

		public Socket Sock
		{
			get
			{
				return this._sock;
			}
			set
			{
				this._sock = value;
			}
		}

		public DateTime LastRefreshTime
		{
			get
			{
				return this._lastRefreshTime;
			}
			set
			{
				this._lastRefreshTime = value;
			}
		}

		public StringBuilder StrBuilder
		{
			get
			{
				return this._strBuilder;
			}
			set
			{
				this._strBuilder = value;
			}
		}

		public string IpAddress
		{
			get
			{
				if (this._sock == null) return "";

                IPEndPoint iPEndPoint = this._sock.RemoteEndPoint as IPEndPoint;
                if (iPEndPoint == null)
                {
                    return "";
                }
                else if (iPEndPoint.Address == null)
                {
                    return "";
                }
                else
                {
                    return iPEndPoint.Address.ToString();
                }
			}
		}

		public SockStateObject()
		{
			this._buffer = new byte[this._bufferSize];
			this._strBuilder = new StringBuilder();
		}

		public void sendResponse(string response)
		{
			try
			{
				if (this._sock != null && this._sock.Connected)
				{
					this._sock.Send(Encoding.UTF8.GetBytes(response + "\r\n"));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public void clearSocket()
		{
			try
			{
				if (this._sock != null)
				{
					this._sock.Disconnect(true);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
