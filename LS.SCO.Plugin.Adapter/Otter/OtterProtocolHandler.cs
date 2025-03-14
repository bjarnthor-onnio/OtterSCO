using LS.SCO.Interfaces.Log;
using LS.SCO.Plugin.Adapter.Otter.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace LS.SCO.Plugin.Adapter.Otter
{
    public class OtterProtocolHandler:IOtterProtocolHandler
    {
        protected OtterMessageDeserializer Deserializer = new OtterMessageDeserializer();
        protected readonly ILogManager _logService;
        private readonly ILogManager _logManager;
        protected IOtterTcpSocketClient TcpClient { get; set; }

        private bool _stopInit = false;

        private string _ScoServerAddress;
        private int _ScoServerPort;

        protected CancellationTokenSource InitializationCancellationTokenSource { get; set; }
        public Action<Message> OnMessageReceived { get; set; }
        public Action Disconnected { get; set; }

        public bool isConnected = false;

        const string MessagePattern = @"([\s\S]*?){([\s\S]*?)jsonrpc([\s\S]*?)method(([\s\S]*?)(params|result)([\s\S]*?){([\s\S]*?)}([\s\S]*?)?)([\s\S]*?)}";

        public OtterProtocolHandler(ILogManager logService, ILogManager logManager)
        {
            this._logService = logService;
            //this._configuration = configuration;
            this._logManager = logManager;

            //TcpClient = services.GetService<IOtterTcpSocketClient>();
            TcpClient = new OtterTcpSocketClient(_logService, _logManager); 
            _ScoServerAddress = "127.0.0.1";
            _ScoServerPort = 9000;
        }

        public void ConnectToSCO(string address, int port)
        {
            //TcpClient.Start(new CancellationToken(), _ScoServerAddress, _ScoServerPort);
            TcpClient.Start(new CancellationToken(), address, port);
            TcpClient.Connected += Connected;

            TcpClient.Disconnected += SocketDisconnected;
            TcpClient.DataReceived += DataReceived;
            

        }



        #region TCP CLIENT EVENTS
        private void Connected()
        {
            isConnected = true;

            StartInitialization(OtterMessages.InitMessage());

        }
        private void SocketDisconnected()
        {
            _logService.LogInfo($"ICS Protocol Handler - socket disconnected");
            isConnected = false;
            
            Disconnected?.Invoke();
        }
        private void DataReceived(string obj)
        {
            var message = UnpackMessage(Encoding.UTF8.GetBytes(obj));

            if (IsValid(message))
            {
                _logService.LogInfo($"ICS Protocol Handler - Valid msg \n{message}");

                var messageObject = Deserializer.DeserializeMessage(message);

                if (messageObject == null)
                {
                    _logService.LogInfo($"ICS Protocol Handler - Failed to DeserializeMessage\n{message}");
                    return;
                }

                OnMessageReceived?.Invoke(Deserializer.DeserializeMessage(message));
            }
            else
            {
                _logService.LogInfo($"ICS Protocol Handler - Ignored msg\n {message}");
            }
        }


        #region Events Functions
        private string UnpackMessage(byte[] message)
        {
            return Encoding.UTF8.GetString(message);
        }

        private bool IsValid(string message)
        {
            var match = Regex.Match(message, MessagePattern);
            return match.Success;
        }
        #endregion


        #endregion


        public void StartInitialization(Models.FromPOS.Init initializeMsg)
        {
            _logService.LogInfo("StartInitialization");
            _stopInit = false;

            Task.Run(async () =>
            {
                while (!_stopInit)
                {
                    SendMessage(initializeMsg);
                    await Task.Delay(5000);
                }
            });
        }
        public void StopInitialization()
        {
            _logService.LogInfo("StopInitialization");
            _stopInit = true;
        }


        public void SendMessage(Message message)
        {
            _logService.LogInfo(message.ToJson());
            var aaa = ConvertMessageToData(message.ToJson());
            TcpClient.Send(aaa);
        }

        public byte[] ConvertMessageToData(string message)
        {
            message = message.Length.ToString() + "#" + message;
            var messageByteArray = Encoding.UTF8.GetBytes(message);
            return messageByteArray;
        }

    }
}
