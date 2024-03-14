using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webkit.Extensions.DataConversion;

namespace Webkit.Extensions.Compression
{
    public static class BrotliExtension
    {
        /// <summary>
        /// Compresses a byte array with brotli
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] BrotliCompress(this byte[] byteArray, CompressionLevel level)
        {
            using(MemoryStream memoryStream = new MemoryStream())
            {
                using (BrotliStream brotliStream = new BrotliStream(memoryStream, level))
                {
                    brotliStream.Write(byteArray);
                }
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Decompresses a byte array compressed with brotli
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static byte[] BrotliDecompress(this byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                using(MemoryStream outStream = new MemoryStream())
                {
                    using (BrotliStream brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
                    {
                        brotliStream.CopyTo(outStream);
                    }
                    return outStream.ToArray();
                }
            }
        }
    }
}
