using System;
using System.Text;

namespace Inputs.Misc
{
    internal class XoR
    {
        private byte[] Keys { get; set; }

        public XoR(string password = null)
        {
            Keys = Encoding.ASCII.GetBytes(password ?? Guid.NewGuid().ToString());
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] result = new byte[data.Length];

            Array.Copy(data, result, data.Length);

            for (int i = 0; i < result.Length; i++)
                result[i] = (byte)(result[i] ^ Keys[i % Keys.Length]);

            return result;
        }

        public byte[] Decrypt(byte[] data)
        {
            byte[] result = new byte[data.Length];

            Array.Copy(data, result, data.Length);

            for (int i = 0; i < result.Length; i++)
                result[i] = (byte)(Keys[i % Keys.Length] ^ result[i]);

            return result;
        }
    }
}
