using LS.SCO.Interfaces.Log;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;


namespace LS.SCO.Plugin.Adapter.Otter
{

    public class OtterTcpSocketClient : IOtterTcpSocketClient
	{
        protected readonly ILogManager _logService;
        private readonly ILogManager _logManager;
		private TcpClient _client;
		public Action<string> DataReceived { get; set; }
		public Action Connected { get; set; }
		public Action Disconnected { get; set; }
		protected string ServerAddress { get; set; }
		protected int ServerPort { get; set; }

		private readonly List<byte> _buffer;
		private bool _connectionStatus = false;

		public StringBuilder sb = new StringBuilder();
		private static String response = String.Empty;

		int intMessageLength = 0;

		//const string msgPattern = @"([\d]*?)#{([\s\S]*?)jsonrpc([\s\S]*?)method(([\s\S]*?)(params|result)([\s\S]*?){([\s\S]*?)}([\s\S]*?)?)([\s\S]*?)}";
		const string msgPattern = @"[0-9]+\#({[\s\S]*?})(?=[0-9]+\#|$)";

        public OtterTcpSocketClient(ILogManager logService, ILogManager logManager)
		{
            this._logService = logService;
            this._logManager = logManager;

            _buffer = new List<byte>();
		}

		private bool EnsureConnection(string server, int port)
		{
			if (_client == null)
			{
				_client = new TcpClient();
			}

			if (!_client.Connected)
			{
				try
				{
					_logService.LogInfo($"Otter Socket Client - Tcp socket Connecting {server}:{port}");
					_client.Client.Connect(server, port);

					_logService.LogInfo($"Otter Socket Client - Tcp socket is {(_client.Connected? "connected" : "not connected" )} "+
						$"to {server}:{port}");

					if (_client.Connected)
					{
						_connectionStatus = true;

                        Connected();
						Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
						StateObject stateObject = new StateObject();
						stateObject.workSocket = _client.Client;
						_client.Client.BeginReceive(stateObject.buffer, 0, StateObject.BufferSize, SocketFlags.None, ReceiveCallback, stateObject);
                    }

                    return _client.Connected;


                }
				catch (SocketException se)
				{
                    _logService.LogError($"Otter Socket Client - Tcp socket exception {se.ErrorCode} - {se.Message}");
					if(_connectionStatus)
					{
						_connectionStatus = false;
                    }
					if(_client.Client.IsBound)
					{
						_client.Client.Shutdown(SocketShutdown.Both);
						_client.Client.Disconnect(false);
						_client.Dispose();
					}
                    _client = null;
                    return false;
                }
				catch (Exception e)
				{
					_logService.LogError($"Otter Socket Client - Exception {e.Message}");
                    if (_client.Client.IsBound)
                    {
                        _client.Client.Shutdown(SocketShutdown.Both);
                        _client.Client.Disconnect(false);
                        _client.Dispose();
                    }
                    _client = null;
                    return false;
				}
			}

			return true;
		}



		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{

				StateObject stateObject = (StateObject)ar.AsyncState;
				Socket client = stateObject.workSocket;
				String strContent = string.Empty;

                if(!client.Connected)
				{
                    return;
				}

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);
				

				if (bytesRead > 0)
				{

					if (bytesRead > 4) //If bytesRead is bigger than 4, message is coming from SCOT with XML message and length
					{
						strContent = Encoding.GetEncoding("iso-8859-2").GetString(stateObject.buffer, 0, bytesRead);
                        _logService.LogInfo($"Otter Socket Client - Buffer data received \n{strContent}");

						var match = Regex.Match(strContent, msgPattern);
						if (match.Success)
						{
							while (match.Success)
							{
								int strLength = 0;
								Int32.TryParse(match.Value.Split('#', 2)[0], out strLength);

                                string strContentToSend = match.Value.Split('#', 2)[1];
                                
								if(strLength != strContentToSend.Length)
								{
									continue;
								}

                                if (strContentToSend.Length > 0)
								{
									_logService.LogInfo($"Otter Socket Client - Message received (length {strLength}) \n{strContentToSend}");
									DataReceived?.Invoke(strContentToSend);
								}

								match = match.NextMatch();
							}
						}
						
					}

					client.BeginReceive(stateObject.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, stateObject);
				}
				else if (bytesRead == 0)
				{
					try
					{
						client.Disconnect(false);
						Disconnected();
						
						//client.BeginReceive(stateObject.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, stateObject);
					}
					catch(Exception e)
					{
						_logService.LogInfo("Connection lost?");
						throw;
					}
					
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				//throw;
			}
		}

		public void Start(CancellationToken stoppingToken, string address, int port) => RunTcpClient(stoppingToken,address, port);

		private void RunTcpClient(CancellationToken stoppingToken, string address, int port)
		{
			ServerAddress = address;
			ServerPort = port;
			Task.Run(async () =>
			{
				while (!stoppingToken.IsCancellationRequested)
				{
					if (!EnsureConnection(ServerAddress, port))
					{
						await Task.Delay(10000);
						continue;
					}

					await Task.Delay(1000);
				}
			});

		}



		public void Send(byte[] message)
		{
			if (_client != null && _client.Connected)
			{
				_client.Client.SendAsync(message, SocketFlags.None);

				_logService.LogInfo($"Otter Socket Client - Message sent \n{Encoding.ASCII.GetString(message, 0, message.Length)}");
			}
		}
		public void Disconnect()
        {
            if (_client != null && _client.Connected)
            {
                _client.Client.Disconnect(false);
                Disconnected();
            }
        }

        private byte[] ProcessData(byte[] data)
		{

			var buffer = _buffer;
			byte[] response = null;

			foreach (var @byte in data)
			{
				if (IsTransportFrame(@byte, buffer))
				{
					continue;
				}

				buffer.Add(@byte);

				if (IsStartFrame(@byte, buffer) || !IsEndFrame(@byte, buffer))
				{
					continue;
				}

				var frame = buffer.ToArray();

				if (IsValidFrame(frame))
				{
					buffer.Clear();
					return frame.Skip(3).TakeWhile(x => x != ProtocolCharacter.ETX).ToArray(); ;
				}
				else
				{
					throw new Exception($"Otter Socket Client - Invalid message. Buffer { BitConverter.ToString(buffer.ToArray())}. Data: { BitConverter.ToString(data)} ");
				}
			}

			return null;
		}

		private static bool IsTransportFrame(byte @byte, List<byte> buffer)
		{
			return (@byte == ProtocolCharacter.ACK || @byte == ProtocolCharacter.NAK) && buffer.Count == 0;
		}

		private static bool IsStartFrame(byte @byte, List<byte> buffer)
		{
			return @byte == ProtocolCharacter.STX && buffer.Count == 0;
		}

		private static bool IsEndFrame(byte @byte, List<byte> buffer)
		{
			//return @byte == ProtocolCharacter.ETX && buffer.Count > 3;
			return @byte == ProtocolCharacter.ETX && buffer.Count > 3;
		}

		private static bool IsValidFrame(byte[] frame)
		{
			if (frame[0] != ProtocolCharacter.STX)
			{
				return false;
			}

			if (frame[^1] != ProtocolCharacter.ETX)
			{
				return false;
			}

			var actualLength = frame.Length - 4;
			var requiredLength = 256 * frame[1] + frame[2];

			return actualLength == requiredLength;
		}
	}
}
