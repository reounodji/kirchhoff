using System;

namespace KnausTabbert.Display
{
    public static class ByteExtensionMethods
    {
        public static byte SetBit(this byte byteValue, byte position, bool state)
        {
            if (state)
                return (byte)(byteValue | (1 << position));

            return (byte)(byteValue & ~(1 << position));
        }



        public static byte SetBits(this byte byteValue, int bit7, int bit6, int bit5, int bit4, int bit3, int bit2, int bit1, int bit0)
        {
            if (bit0 < 0 || bit0 > 1) throw new ArgumentOutOfRangeException("bit0", "Only 0 and 1 are accepted.");
            if (bit1 < 0 || bit1 > 1) throw new ArgumentOutOfRangeException("bit1", "Only 0 and 1 are accepted.");
            if (bit2 < 0 || bit2 > 1) throw new ArgumentOutOfRangeException("bit2", "Only 0 and 1 are accepted.");
            if (bit3 < 0 || bit3 > 1) throw new ArgumentOutOfRangeException("bit3", "Only 0 and 1 are accepted.");
            if (bit4 < 0 || bit4 > 1) throw new ArgumentOutOfRangeException("bit4", "Only 0 and 1 are accepted.");
            if (bit5 < 0 || bit5 > 1) throw new ArgumentOutOfRangeException("bit5", "Only 0 and 1 are accepted.");
            if (bit6 < 0 || bit6 > 1) throw new ArgumentOutOfRangeException("bit6", "Only 0 and 1 are accepted.");
            if (bit7 < 0 || bit7 > 1) throw new ArgumentOutOfRangeException("bit7", "Only 0 and 1 are accepted.");

            return SetBits(
                byteValue,
                bit7 == 1,
                bit6 == 1,
                bit5 == 1,
                bit4 == 1,
                bit3 == 1,
                bit2 == 1,
                bit1 == 1,
                bit0 == 1);
        }

        public static byte SetBits(this byte byteValue, bool bit7, bool bit6, bool bit5, bool bit4, bool bit3, bool bit2, bool bit1, bool bit0)
        {
            byte result = byteValue
                .SetBit(0, bit0)
                .SetBit(1, bit1)
                .SetBit(2, bit2)
                .SetBit(3, bit3)
                .SetBit(4, bit4)
                .SetBit(5, bit5)
                .SetBit(6, bit6)
                .SetBit(7, bit7);

            return result;
        }

        public static bool GetBit(this byte byteValue, byte position)
        {
            return (byteValue & (1 << position)) != 0;
        }

        public static byte ReverseBitOrder(this byte byteValue)
        {
            byte reversed = 0x00;
            for (byte i = 0; i < 8; i++)
                reversed = SetBit(reversed, i, GetBit(byteValue, (byte)(7 - i)));

            return reversed;
        }

        public static string DisplayBits(this byte byteValue)
        {
            return (byteValue.GetBit(7) ? "1" : "0")
                + (byteValue.GetBit(6) ? "1" : "0")
                + (byteValue.GetBit(5) ? "1" : "0")
                + (byteValue.GetBit(4) ? "1" : "0")
                + (byteValue.GetBit(3) ? "1" : "0")
                + (byteValue.GetBit(2) ? "1" : "0")
                + (byteValue.GetBit(1) ? "1" : "0")
                + (byteValue.GetBit(0) ? "1" : "0");
        }
    }
}
