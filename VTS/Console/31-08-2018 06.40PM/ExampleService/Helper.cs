using System;
using System.Collections.Generic;
using System.Text;
using ExampleService.DataAccess;
using ExampleService.Properties;
using System.Diagnostics;
using System.Linq;

namespace ExampleService
{
   
    internal static class Helper
    {
        public static string imei;
        /// <summary>
        /// Creates a new database access from the database type provided in settings
        /// </summary>
        /// <param name="imei">Device IMEI</param>
        /// <returns>Database access</returns>
        public static ITelemetryAccess GetDatabase(long? imei)
        {
            if (Settings.Default.DatabaseUsed.ToLower() == "mysql")
                return new MySqlDataAccess(imei);
            else if (Settings.Default.DatabaseUsed.ToLower() == "mssql")
                return new MSSqlDataAccess(imei);
            else
                return new PostgreSqlDataAccess(imei);
        }

        /// <summary>
        /// Retrieves the data section of a packet
        /// </summary>
        /// <param name="buffer">Packet buffer</param>
        /// <param name="index">Start index</param>
        /// <returns>Object with data information of a package</returns>
        public static PacketData GetData(byte[] buffer, int index)
        {
            var packet = new PacketData();
            packet.Length = BitConverter.ToUInt16(buffer, index);
            packet.Service = buffer[index + 2];
            packet.KeyOrPad = buffer[index + 3];
            packet.Data = ReadBlock(buffer, index + 4, packet.Length - 2);
            return packet;
        }

        /// <summary>
        /// Retrieves a specific portion of the provided buffer
        /// </summary>
        /// <param name="buffer">Packet buffer</param>
        /// <param name="offset">Byte offset</param>
        /// <param name="length">Portion length</param>
        /// <returns>Buffer portion</returns>
        public static byte[] ReadBlock(byte[] buffer, int offset, int length)
        {
            var block = new byte[length];
            var index = 0;
            for (var i = offset; i < offset + length; i++)
                block[index++] = buffer[i];
            return block;
        }

        /// <summary>
        /// Counts the checksum of the provided buffer
        /// </summary>
        /// <param name="buffer">Packet buffer</param>
        /// <param name="length">Bytes to count from start</param>
        /// <returns>Checksum</returns>
        public static byte CountCheckSum(byte[] buffer, int length)
        {
            int sum = 0;
            for (var i = 0; i < length; i++)
                sum += buffer[i];
            return (byte)(sum & 10);
        }

        /// <summary>
        /// Validates the buffer
        /// </summary>
        /// <param name="buffer">Packet buffer</param>
        /// <returns>Does the keys match</returns>
        public static bool CheckForKeyword(byte[] buffer)
        {

            Console.Write("Actual Data:" + BitConverter.ToString( buffer));
        // If the system architecture is little-endian (that is, little end first),
        // reverse the byte array.

        //Console.WriteLine("byte array: " + BitConverter.ToString(buffer));
        string local = BitConverter.ToString(buffer);
            if (buffer.Length == 18)
            {

                var selected = buffer.Skip(4).Take(8).ToArray();
                string imei1 = BitConverter.ToString(selected).Replace("-", "");
                Console.WriteLine("IMEI: " + BitConverter.ToString(selected));
                imei = imei1.ToString();
            }

           

            //ParseResults parseResults = null;
            //parseResults = new ParseResults();
            //parseResults.ImeiBuffer = 
            //Log("IMEI: " + BitConverter.ToUInt64(parseResults.ImeiBuffer, 0));

            //var checkBuffer = Encoding.UTF8.GetBytes("#BCE#\r\n");
            //var checkBuffer = Encoding.UTF8.GetBytes("xxx");

            //if (checkBuffer.Length != buffer.Length)
            //    return false;

            //for (var i = 0; i < checkBuffer.Length; i++)
            //{
            //    if (checkBuffer[i] != buffer[i])
            //        return false;
            //}

            return true;
        }

        /// <summary>
        /// Parse the data from packet
        /// </summary>
        /// <param name="localBuffer">Packet buffer</param>
        /// <returns>Parse result object</returns>
        public static ParseResults ParseData(byte[] localBuffer)
        {
            ParseResults parseResults = null;
            try
            {
               
               
               
                parseResults = new ParseResults();
               var a= localBuffer.Skip(4).Take(8).ToArray();
                parseResults.ImeiBuffer = Helper.ReadBlock(localBuffer, 0, 8);
                //Log("IMEI: " + BitConverter.ToUInt64(parseResults.ImeiBuffer,0));

                var cs = localBuffer[localBuffer.Length-1];

                if (cs == Helper.CountCheckSum(localBuffer, localBuffer.Length - 1))
                {
                    var lengthIndex = 8;
                    var packets = new List<PacketData>();
                    byte? key = null;

                    // Iterating in case there are multiple data sections in one package
                    while (lengthIndex < localBuffer.Length - 1)
                    {
                        var packetData = Helper.GetData(localBuffer, lengthIndex);
                        packets.Add(packetData);
                        lengthIndex += packetData.Length + 2;
                        if (packetData.Service == 0xA5)
                            key = packetData.KeyOrPad;
                    }

                    var parser = new Parser();
                    foreach (var packet in packets)
                    {
                        parser.ParsePacket(packet.Data);
                        parseResults.Packets.AddRange(parser.GetPackets());
                    }

                    // If packet has key byte, form a response
                    if (key != null)
                    {
                        var responseBuffer = new byte[13];
                        for (var i = 0; i < 8; i++)
                            responseBuffer[i] = parseResults.ImeiBuffer[i];
                        responseBuffer[8] = 02;
                        responseBuffer[9] = 00;
                        responseBuffer[10] = 0x19;
                        responseBuffer[11] = (byte)(key & 0x7F);
                        responseBuffer[12] = Helper.CountCheckSum(responseBuffer, 12);
                        parseResults.Response = responseBuffer;
                    }
                }
                else
                    Log("Checksum does not match.");
            }
            catch
            {
                Log("Error parsing data.");
            }
            return parseResults;
        }

        private static void Log(string message)
        {
            Trace.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Helper: " + message);
        }
    }
}