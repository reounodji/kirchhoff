using System.Collections.Generic;

namespace KnausTabbert.Display.GTX
{
    public class GtxProtocol
    {
        private GtxHeader header;
        private GtxSerialStatus serialStatus;
        private GtxPageNumber pageNumber;
        private byte tempoOfPage;
        private GtxFunction function;
        private byte siteState;
        private GtxTextMessage textMessage;
        // end of textmessage
        private byte eot = 0x04;

        private byte checksum;

        public GtxProtocol(List<string> messageList, byte numberOfLines = 5, byte signAddress = 1, bool interrupModus = false, bool lastPage = true, bool confirmation = true, bool listMode = false)
        {
            header = new GtxHeader(numberOfLines, signAddress);
            serialStatus = new GtxSerialStatus(interrupModus, lastPage, confirmation, listMode);
            // pagenumber aktuell auf 001
            pageNumber = new GtxPageNumber(0x30, 0x30, 0x31);
            // == E4 hex --> display always on
            tempoOfPage = 0xE4;
            function = new GtxFunction();
            siteState = 0xA0; //0xA0 
            textMessage = new GtxTextMessage(messageList);

        }

        public byte[] ToArray()
        {
            List<byte> list = new List<byte>();

            foreach (var b in header.ToArray())
                list.Add(b);

            foreach (var b in serialStatus.ToArray())
                list.Add(b);

            foreach (var b in pageNumber.ToArray())
                list.Add(b);

            list.Add(tempoOfPage);

            foreach (var b in function.ToArray())
                list.Add(b);

            list.Add(siteState);

            foreach (var b in textMessage.ToArray())
                list.Add(b);

            list.Add(eot);

            checksum = list[0];
            for (int i = 1; i < list.Count; i++)
                checksum ^= list[i];

            list.Add(checksum);

            return list.ToArray();
        }
    }
}
