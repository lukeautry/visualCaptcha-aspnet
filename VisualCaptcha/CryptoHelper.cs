using System;
using System.Security.Cryptography;
using System.Text;

namespace VisualCaptcha
{
    public sealed class CryptoHelper
    {
        private static readonly char[] Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private readonly RNGCryptoServiceProvider _crypto = new RNGCryptoServiceProvider();
        private readonly byte[] _buffer = new byte[4];

        public string GetRandomString(int length)
        {
            var bytes = new byte[length];
            _crypto.GetNonZeroBytes(bytes);

            var sb = new StringBuilder();
            foreach(var b in bytes)
            {
                sb.Append(Characters[b % (Characters.Length)]);
            }

            return sb.ToString();
        }

        public int GetRandomIndex(int finishIndex)
        {
            if (finishIndex <= 0) { return 0; }

            var diff = finishIndex;

            while (true)
            {
                _crypto.GetBytes(_buffer);
                var rng = BitConverter.ToUInt32(_buffer, 0);

                const long max = (1 + (Int64) UInt32.MaxValue);
                var remainder = max % diff;
                if (rng < max - remainder)
                {
                    return (Int32)(0 + (rng % diff));
                }
            }
        }
    }
}
