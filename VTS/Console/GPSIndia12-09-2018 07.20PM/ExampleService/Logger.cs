using ExampleService.Merged;
using ExampleService.Properties;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace ExampleService
{
    /// <summary>
    /// Logger for server progress and error logging
    /// </summary>
    internal class Logger
    {
        private string Prefix;
        private JsonSerializerSettings JsonSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefix">Logger prefix</param>
        public Logger(string prefix)
        {
            this.Prefix = prefix;
            this.JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="ex">Exception</param>
        public void Log(Exception ex)
        {
            Log(ex.ToString());
        }

        /// <summary>
        /// Log string message with params
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="args">Additional arguments</param>
        public void Log(string message, params object[] args)
        {
            Log(string.Format(message, args));
        }

        /// <summary>
        /// Log string message
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            Trace.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " "+this.Prefix+": " + message);
        }

        public void Log(ParseResults result)
        {
            if (Settings.Default.ShowRecievedPacketDetails)
            {
                var message = string.Format("Parse results:\n" +
                "Parsed items from packet: {0}\n", result.Packets.Count);

                foreach (var item in result.Packets)
                {
                    var type = item.GetType();
                    if (type == typeof(MergedData))
                        message += "\nMerged data packet:\n";
                    if (type == typeof(DeviceError))
                        message += "\nDevice error packet:\n";
                    message += JsonConvert.SerializeObject(item, this.JsonSettings) + "\n";
                }

                Log(message);
            }
        }

        public void Log(OutputControl item)
        {
            if (Settings.Default.ShowRecievedPacketDetails)
            {
                var message = "\nRecieved output signal, parse results:\n";
                message += JsonConvert.SerializeObject(item, this.JsonSettings) + "\n";
                Log(message);
            }
        }

        /// <summary>
        /// Add output packet status (pending confirmation) to database log
        /// </summary>
        /// <param name="item">Output packet object</param>
        public int? LogOutput(OutputControl item)
        {
            try
            {
                using (var dataAccess = Helper.GetDatabase(null))
                {
                    dataAccess.Open();
                    return dataAccess.OutputInsert(item);
                }
            }
            catch (Exception e)
            {
                Log("Database error:");
                Log(e);
            }
            return null;
        }

        /// <summary>
        /// Update output packet status int database log.<para />
        /// 0 : pending,<para />
        /// 1 : complete,<para />
        /// 2 : expired,<para />
        /// 3 : output sent, waiting for confirmation,<para />
        /// 4 : confirmation recieved, bad checksum,<para />
        /// 5 : confirmation response timeout,<para />
        /// 6 : can't reach TCP socket, retrying until timeout
        /// </summary>
        /// <param name="id">Output packet id</param>
        /// <param name="status">Output packet status index</param>
        public void UpdateOutputStatus(int id, byte status)
        {
            try
            {
                using (var dataAccess = Helper.GetDatabase(null))
                {
                    dataAccess.Open();
                    dataAccess.OutputUpdate(id, status);
                }
            }
            catch(Exception e)
            {
                Log("Database error:");
                Log(e);
            }
        }
    }
}