using PartnerTech.Otter.Client.Models.BaseModels;
using PartnerTech.Otter.Client.Models.ResponseModels;

public interface IOtterService
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
