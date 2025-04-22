namespace PartnerTech.Otter.Client.Interface
{
    public interface IOtterTcpSocketClient
    {
        void Start(CancellationToken stoppingToken, string address, int port);
        void Send(byte[] message);
        Action<string> DataReceived { get; set; }
        Action Connected { get; set; }
        Action Disconnected { get; set; }
    }
}