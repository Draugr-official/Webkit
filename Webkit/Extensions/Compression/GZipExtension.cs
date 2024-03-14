using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webkit.Extensions.Compression
{
    public static class GZipExtension
    {
        /// <summary>
        /// Compresses a byte array with gzip
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] GZipCompress(this byte[] byteArray, CompressionLevel level)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(memoryStream, level))
                {
                    gZipStream.Write(byteArray);
                }
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Decompresses a byte array compressed with gzip
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static byte[] GZipDecompress(this byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        gZipStream.CopyTo(outStream);
                    }
                    return outStream.ToArray();
                }
            }
        }
    }
}
