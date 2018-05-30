using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Extensions
{
    public static class ByteExtensions
    {
        public static byte[] XOR(this byte[] buffer1, byte[] buffer2)
        {
            for (int i = 0; i < buffer1.Length; i++)
                buffer1[i] ^= buffer2[i];
            return buffer1;
        }
    }
}
