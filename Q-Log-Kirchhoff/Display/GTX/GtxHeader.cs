
using KnausTabbert.Display.Interfaces;
using System;

namespace KnausTabbert.Display.GTX
{
    public class GtxHeader : IGtxHeader
    {
        // Header start
        private const byte HeaderSync = 0x00;

        private byte _numberOfLines = 0x05;
        public byte NumberOfLines
        {
            get { return _numberOfLines; }
            set
            {
                if (value > 127)
                    throw new ArgumentOutOfRangeException("Number of Lines needs to be <= 127");

                _numberOfLines = value;
            }
        }

        private byte _signAddress = 0x01;
        public byte SignAddress
        {
            get { return _signAddress; }
            set
            {
                if (value > 127)
                    throw new ArgumentOutOfRangeException("Number of Lines needs to be <= 127");

                _signAddress = value;
            }
        }

        // Header ende
        private const byte Etx = 0x03;

        public GtxHeader(byte numberOfLines = 5, byte signAddress = 1)
        {
            NumberOfLines = numberOfLines;
            SignAddress = signAddress;
        }

        public byte[] ToArray()
        {
            return new[]
            {
                HeaderSync,
                _numberOfLines,
                _signAddress,
                Etx
            };
        }

        public override string ToString()
        {
            return string.Format("NumberOfLines: {0}, SignAddress: {1}", NumberOfLines, SignAddress);
        }

    }
}
