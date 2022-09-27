namespace KnausTabbert.Display.GTX
{
    public class GtxFunction
    {
        private EGtxFunctionType _type = EGtxFunctionType.Appear;

        public EGtxFunctionType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public GtxFunction()
        { }

        public GtxFunction(EGtxFunctionType type = EGtxFunctionType.Appear)
        {
            _type = type;
        }

        public byte[] ToArray()
        {
            return new[]
            {
                // bit 6 and 7 = true because the protocol demands it
                ((byte)_type).SetBit(6, true).SetBit(7, true)
            };
        }

        public override string ToString()
        {
            return string.Format("Type: {0}", Type);
        }
    }
}
