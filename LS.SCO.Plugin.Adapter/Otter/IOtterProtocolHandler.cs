using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LS.SCO.Plugin.Adapter.Otter.Models;
using LS.SCO.Plugin.Adapter.Otter.Models.FromPOS;

public interface IOtterProtocolHandler
{
    // Methods for connecting
    void ConnectToSCO(string address, int port);

    // Methods for initialization
    void StartInitialization(Init initializeMsg);
    void StopInitialization();

    // Message handling
    void SendMessage(Message message);
    byte[] ConvertMessageToData(string message);

    
}
