using Webkit.Extensions.Compression;
using Webkit.Extensions.DataConversion;
using Webkit.Extensions.Logging;

namespace Webkit.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string someData = "Wahhh";
            byte[] compressedData = someData.AsByteArray().BrotliCompress(System.IO.Compression.CompressionLevel.Optimal);
            compressedData.LogAsJson("Compressed: ");

            compressedData.BrotliDecompress().LogAsJson("Decompressed: ");

            System.Console.ReadLine();
        }
    }
}
