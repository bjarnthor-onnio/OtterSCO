using System.Net.Sockets;

namespace LS.SCO.Plugin.Adapter.Otter
{
    public class StateObject
    {
		// Client  socket.
		public Socket workSocket = null;
		// Size of receive buffer.
		public const int BufferSize = 2048;
		// Receive buffer.
		public byte[] buffer = new byte[BufferSize];
	}
}
