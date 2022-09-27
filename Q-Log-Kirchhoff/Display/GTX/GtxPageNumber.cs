namespace KnausTabbert.Display.GTX
{
    public class GtxPageNumber
    {

        public byte X_100 { get; set; }
        public byte X_10 { get; set; }
        public byte X_1 { get; set; }

        public GtxPageNumber(byte x_100, byte x_10, byte x_1)
        {
            X_100 = x_100;
            X_10 = x_10;
            X_1 = x_1;
        }

        public byte[] ToArray()
        {
            byte[] arr = { X_100, X_10, X_1 };
            return arr;
        }

    }
}
