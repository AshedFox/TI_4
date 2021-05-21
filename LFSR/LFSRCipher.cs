using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSR
{
    class LFSRCipher
    {
        const int size = 26;
        long baseValue;
        public byte[] Key { get; private set; }

        public LFSRCipher(long baseValue)
        {
            this.baseValue = baseValue;
        }

        public void CountKey(long fileSize)
        {
            Key = new byte[fileSize];
            string buff;
            for (long i = 0; i < fileSize; i++)
            {
                buff = string.Empty;
                for (int j = 0; j < 8; j++)
                {
                    baseValue = ((((baseValue >> 8) ^ (baseValue >> 7) ^ (baseValue >> 1) ^ (baseValue))
                        & 1) << size) | (baseValue >> 1);
                    buff += (baseValue & 1);
                }
                Key[i] = Convert.ToByte(buff, 2);
            }
        }
    }
}
