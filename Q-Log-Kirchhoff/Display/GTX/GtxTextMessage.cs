using System.Collections.Generic;

namespace KnausTabbert.Display.GTX
{
    public class GtxTextMessage
    {
        private List<string> MessageList = new List<string>();

        public GtxTextMessage(List<string> messageList)
        {
            MessageList = messageList;
        }

        public void Clear()
        {
            MessageList.Clear();
        }

        public void AddLine(string text)
        {
            MessageList.Add(text);
        }

        public byte[] ToArray()
        {
            List<byte> list = new List<byte>();
            foreach (var msg in MessageList)
            {
                foreach (byte b in StringToByteArr(msg))
                {
                    list.Add(b);
                }
            }
            return list.ToArray();
        }

        private byte[] StringToByteArr(string msg)
        {
            List<byte> bytes = new List<byte>();
            // Steuercode damit erscheinungsform der zeichen geändert werden kann (zum Beispiel durch 'D')
            byte preFix = 0x01C;
            bytes.Add(preFix);
            // D == default settings. chars will be displayed normal (not blinking / different color/ ..)
            bytes.Add((byte)('D'));
            foreach (char c in msg)
            {
                byte val = (byte)c;
                switch (c)
                {
                    case 'Ü':
                        val = 0x9A;
                        break;
                    case 'Ö':
                        val = 0x99;
                        break;
                    case 'Ä':
                        val = 0x8E;
                        break;
                        //case 'ß':
                        //    val = 0xE1;
                        //    break;
                }
                bytes.Add(val);
            }
            return bytes.ToArray();
        }

    }
}
