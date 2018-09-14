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
           // byte[] s = HexConverter.HexToByteArray(buffer);
            string hext =HexConverter.ByteArrayToHexWithSpaces(buffer);
        
            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.

            //Console.WriteLine("byte array: " + BitConverter.ToString(buffer));
            string local = BitConverter.ToString(buffer);
            if (buffer.Length == 15)
            {

                var ignation = buffer.Skip(4).Take(1).ToArray();
                string ignationstring = BitConverter.ToString(ignation).Replace("-", "");
                string binaryval;


                binaryval = Convert.ToString(Convert.ToInt32(ignationstring, 16), 2);

                char s = binaryval[binaryval.Length - 2];
                if (s == '1')
                {
                    TCPServer.nowignation= "41209";
                }
                else
                {
                    TCPServer.nowignation = "32936";
                }



            }
           else if (buffer.Length == 18)
            {

                var selected = buffer.Skip(4).Take(8).ToArray();
                string imei1 = BitConverter.ToString(selected).Replace("-", "");
                Console.WriteLine("IMEI: " + BitConverter.ToString(selected).Replace("-",""));
                imei = imei1.ToString();
            }
            else if (buffer.Length == 36)
            {
                var imei = Helper.imei;
                Console.WriteLine("IMEI:" + imei);
                //Console.WriteLine("IMEI: " + BitConverter.ToString(selected));
                var datetake = buffer.Skip(4).Take(6).ToArray();
                //05 - 07 - 2018 2:24:24 AM
                string date1 = buffer[6].ToString() + "-" + buffer[5].ToString() + "-" + buffer[4].ToString() + " " + buffer[7].ToString() + ":" + buffer[8].ToString() + ":" + buffer[9].ToString();
                var converteddate = Convert.ToDateTime(date1.ToString());
                Console.WriteLine("Date:" +Convert.ToDateTime( date1.ToString()));




                var sattake = buffer.Skip(10).Take(1).ToArray();
                string satstring = BitConverter.ToString(sattake).Replace("-", "");
                string sat = satstring[satstring.Length - 1].ToString();
                decimal satd = (decimal)Int64.Parse(sat, System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine("Sattalite Count:" + satd.ToString());

                var lattake = buffer.Skip(11).Take(4).ToArray();
                string latstring = BitConverter.ToString(lattake).Replace("-", "");
                decimal latd = (decimal)Int64.Parse(latstring, System.Globalization.NumberStyles.HexNumber);
                decimal latt = latd / 30000;
                decimal finallat = latt / 60;
                Console.WriteLine("Lattitude:" + finallat);
              

                var longtake = buffer.Skip(15).Take(4).ToArray();
                string longstring = BitConverter.ToString(longtake).Replace("-", "");
                decimal longd = (decimal)Int64.Parse(longstring, System.Globalization.NumberStyles.HexNumber);
                decimal longt = longd / 30000;
                decimal finallong = longt / 60;
                
                
                Console.WriteLine("Longitude:" + finallong);


                var speedtake = buffer.Skip(19).Take(1).ToArray();
                string speedstring = BitConverter.ToString(speedtake).Replace("-", "");
                decimal speedd = (decimal)Int64.Parse(speedstring, System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine("Speed:" + speedd);


                var courcetake = buffer.Skip(20).Take(2).ToArray();
                string courcestring = BitConverter.ToString(courcetake).Replace("-", "");
                decimal course = (decimal)Int64.Parse(courcestring, System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine("CourseStetus:" + course);

                var mcctake = buffer.Skip(22).Take(2).ToArray();
                string mccstring = BitConverter.ToString(mcctake).Replace("-", "");
                decimal mcc = (decimal)Int64.Parse(mccstring, System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine("Moblie Country Code:" + mcc);


                var mnctake = buffer.Skip(24).Take(1).ToArray();
                string mncstring = BitConverter.ToString(mnctake).Replace("-", "");
                decimal mnc = (decimal)Int64.Parse(mncstring, System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine("Mobile Network Code:" + mnc);

                var lactake = buffer.Skip(25).Take(2).ToArray();
                string lacstring = BitConverter.ToString(lactake).Replace("-", "");
                decimal lac = (decimal)Int64.Parse(lacstring, System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine("Local Area Code:" + lac);


                var celleridtake = buffer.Skip(27).Take(3).ToArray();
                string clleridstring = BitConverter.ToString(celleridtake).Replace("-", "");
                decimal cellerid = (decimal)Int64.Parse(clleridstring, System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine("Celler ID:" + cellerid);
                Console.WriteLine("Ignetion:" + TCPServer.nowignation);
               
                Log("Saving Data Into Database......");
                try
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    int id =0;
                        try
                        {
                            id = (from a in db.telemetries select a.id).Max();
                            id= id + 1;
                        }
                        catch (Exception)
                        {

                           id =1;
                        }
                   
                    telemetry t = new telemetry();
                    t.id = id;
                    t.imei = Convert.ToInt64(imei);
                    t.speed = (short)speedd;
                    t.satellites = (short)satd;
                    t.latitude = (double)finallat;
                    t.longitude = (double)finallong;
                    t.mobile_country_code = (int)mcc;
                    t.mobile_network_code = (short)mnc;
                    t.local_area_code = (int)lac;
                    t.cell_identifier = (int)cellerid;
                    //  t.time = converteddate;
                    t.time = DateTime.Now;
                    t.digital_inputs =Convert.ToInt32( TCPServer.nowignation);
                    t.hdop = 1;
                    db.telemetries.InsertOnSubmit(t);
                    db.SubmitChanges();
                    Log("Successfully Saved To Database......");
                }
                                catch (Exception)
                {


                }
               
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