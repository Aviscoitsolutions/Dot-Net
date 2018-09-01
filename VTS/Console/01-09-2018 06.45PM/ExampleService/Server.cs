using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ExampleService.Properties;

namespace ExampleService
{
    /// <summary>
    /// Base server to manipulate TCP, UDP and TCP output control servers
    /// </summary>
    public class Server
    {
        private UDPServer UDP;
        private TCPServer TCP;
        private TCPOutputControl TCPOutput;

        /// <summary>
        /// Constructor for server
        /// </summary>
        public Server()
        {
            if (Settings.Default.LoggingLevel.Contains("File"))
            {
                string folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
               
                folder = Path.Combine(folder,"logs");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(folder, "ExampleServiceLog_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log")));
            }
            if (Settings.Default.LoggingLevel.Contains("Console"))
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
            }
            Trace.AutoFlush = true;
         
            var OutputControlData = new OutputControlData();
            UDP = new UDPServer(OutputControlData);
            TCP = new TCPServer(OutputControlData);
            TCPOutput = new TCPOutputControl(OutputControlData, UDP);
            //Logger.Log("New server created");
        }

        /// <summary>
        /// Starts all servers
        /// </summary>
        public void Start()
        {
            UDP.Start();
            TCP.Start();
            TCPOutput.Start();
        }

        /// <summary>
        /// Checks if any of the servers is running
        /// </summary>
        /// <returns>Any server is running</returns>
        public bool IsActive()
        {
            return UDP.Active || TCP.Active || TCPOutput.Active;
        }

        /// <summary>
        /// Stops all servers
        /// </summary>
        public void Stop()
        {
            UDP.Stop();
            TCP.Stop();
            TCPOutput.Stop();
        }
    }
}