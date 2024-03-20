using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Webkit.Extensions.Logging;

namespace Webkit
{
    /// <summary>
    /// A class for interacting with the OS command line
    /// <para>Supports windows and unix systems</para>
    /// </summary>
    public class CommandLine
    {
        StringBuilder OutputBuilder = new StringBuilder();
        string CliPath = "cmd";

        /// <summary>
        /// <inheritdoc cref="CommandLine"/>
        /// </summary>
        /// <param name="cliPath"></param>
        public CommandLine(string cliPath)
        {
            CliPath = cliPath;
        }

        /// <summary>
        /// Excutes a command to the specified cli path
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public string Execute(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(CliPath, command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                WorkingDirectory = Environment.CurrentDirectory
            };

            using (Process process = new Process())
            {
                process.StartInfo = processInfo;

                process.Start();
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (!process.HasExited)
                {
                    process.Kill();
                }

                process.Close();
            }

            string data = OutputBuilder.ToString().Trim();
            OutputBuilder.Clear();

            return data;
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputBuilder.AppendLine(e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputBuilder.AppendLine(e.Data);
        }
    }
}
