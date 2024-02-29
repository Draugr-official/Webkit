using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Webkit.Extensions
{
    public static class ConsoleExtensions
    {
        /// <summary>
        /// Writes data to the console with the specified color and the device newline
        /// </summary>
        /// <param name="consoleColor"></param>
        /// <param name="value"></param>
        public static void WriteLine(this ConsoleColor consoleColor, object value)
        {
            Write(consoleColor, value.ToString() + Environment.NewLine);
        }

        /// <summary>
        /// Writes data to the console with the specified color
        /// </summary>
        /// <param name="consoleColor"></param>
        /// <param name="value"></param>
        public static void Write(this ConsoleColor consoleColor, object value)
        {
            ConsoleColor previousColor = Console.ForegroundColor;

            Console.ForegroundColor = consoleColor;
            Console.WriteLine(value);
            Console.ForegroundColor = previousColor;
        }
    }
}
