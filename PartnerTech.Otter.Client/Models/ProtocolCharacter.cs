namespace PartnerTech.Otter.Client.Models
{
    public class ProtocolCharacter
    {
        public static byte STX { get { return 0x02; } }
        public static byte ETX { get { return 0x03; } }
        public static byte ACK { get { return 0x06; } }
        public static byte NAK { get { return 0x15; } }
    }
    
}
