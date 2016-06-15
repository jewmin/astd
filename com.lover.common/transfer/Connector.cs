using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;

namespace com.lover.common.transfer
{
	public class Connector
	{
		public delegate void ConnectedHandler(object sender, EventArgs evt);

		public delegate void ConnEvtHandler(object sender, ConnectorEvent evt);

		private string _ip;

		private int _port;

		private Socket _sock;

		private Timer _pingTimer;

		public event Connector.ConnectedHandler ConnectedEvt;

		public event Connector.ConnEvtHandler CommandEvent;

		public Connector(string ipaddress, int port)
		{
			this._ip = ipaddress;
			this._port = port;
			this._sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this._pingTimer = new Timer();
			this._pingTimer.Interval = 3000.0;
			this._pingTimer.Elapsed += new ElapsedEventHandler(this._pingTimer_Elapsed);
		}

		private void _pingTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			HeartBeatCommand command = new HeartBeatCommand();
			this.sendCommand(command);
		}

		public void init()
		{
			this._sock.BeginConnect(this._ip, this._port, new AsyncCallback(this.connected), 0);
		}

		private void connected(IAsyncResult result)
		{
			this._sock.EndConnect(result);
			if (this.ConnectedEvt != null)
			{
				this.ConnectedEvt(this, new EventArgs());
			}
			SockStateObject sockStateObject = new SockStateObject();
			sockStateObject.Sock = this._sock;
			try
			{
				this._sock.BeginReceive(sockStateObject.Buffer, 0, sockStateObject.BufferSize, SocketFlags.None, new AsyncCallback(this.recvCallback), sockStateObject);
			}
			catch (SocketException ex)
			{
				Console.WriteLine(ex.ToString() + ex.StackTrace);
			}
		}

		private void recvCallback(IAsyncResult ar)
		{
			try
			{
				string rawResponse = "";
				SockStateObject sockStateObject = (SockStateObject)ar.AsyncState;
				Socket sock = sockStateObject.Sock;
				int recv_length = sock.EndReceive(ar);
				if (recv_length > 0)
				{
					sockStateObject.StrBuilder.Append(Encoding.UTF8.GetString(sockStateObject.Buffer, 0, recv_length));
					int length = sockStateObject.StrBuilder.Length;
					if (length >= 2 && sockStateObject.StrBuilder[length - 1] == '\n' && sockStateObject.StrBuilder[length - 2] == '\r')
					{
						if (sockStateObject.StrBuilder.Length > 1)
						{
							rawResponse = sockStateObject.StrBuilder.ToString();
						}
						this.handleCmdEvent(rawResponse);
					}
					sock.BeginReceive(sockStateObject.Buffer, 0, sockStateObject.BufferSize, SocketFlags.None, new AsyncCallback(this.recvCallback), sockStateObject);
				}
				else
				{
					if (sockStateObject.StrBuilder.Length > 1)
					{
						rawResponse = sockStateObject.StrBuilder.ToString();
					}
					this.handleCmdEvent(rawResponse);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString() + ex.StackTrace);
			}
		}

		private void handleCmdEvent(string rawResponse)
		{
			ConnectorEvent evt = new ConnectorEvent(rawResponse);
			if (this.CommandEvent != null)
			{
				this.CommandEvent(this, evt);
			}
		}

		public void sendCommand(BaseCommand command)
		{
			string str = command.toString();
			byte[] bytes = Encoding.UTF8.GetBytes(str + "\r\n");
			this._sock.Send(bytes);
		}
	}
}
