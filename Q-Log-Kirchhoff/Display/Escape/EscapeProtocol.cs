using System.Collections.Generic;

namespace KnausTabbert.Display.Escape
{
    public class EscapeProtocol
    {
        private const byte _esc = 0x1B;
        private byte address = 0x21;   // 21h is std
        private byte offset = 0x20;
        private byte[] message = new byte[10];         // 30h == off 31h == on
        private const byte _carriageReturn = 0x0D;

        public EscapeProtocol(byte[] message, byte address = 0x21, byte offset = 0x20)
        {
            this.address = address;
            this.offset = offset;
            this.message = message;
        }

        public byte[] ToArray()
        {
            List<byte> list = new List<byte>();
            list.Add(_esc);
            list.Add(address);
            list.Add(offset);
            foreach (var b in message)
                list.Add(b);
            list.Add(_carriageReturn);
            return list.ToArray();
        }
    }
}
