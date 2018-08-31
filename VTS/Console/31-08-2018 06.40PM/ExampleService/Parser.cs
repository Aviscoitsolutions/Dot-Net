using System;
using System.Collections.Generic;
using ExampleService.Merged;

namespace ExampleService
{
    /// <summary>
    /// Packet parser
    /// </summary>
    internal class Parser
    {
        /// <summary>
        /// Minimal packet sizes
        /// </summary>
        private static int[] minimumPacketSizes = { 14, 6, 20, 12, 8, 8, 6, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private List<IPacket> parsedPackets;

        private Logger Logger = new Logger("Packet parser");

        /// <summary>
        /// Constructor for parser
        /// </summary>
        public Parser()
        {
            this.parsedPackets = new List<IPacket>();
        }

        public List<IPacket> GetPackets()
        {
            return this.parsedPackets;
        }

        /// <summary>
        /// Analyses packets
        /// </summary>
        /// <param name="buffer">Packet buffer</param>
        public void ParsePacket(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            int offset = 0;
            parsedPackets.Clear();

            while (offset < buffer.Length)
            {
                byte packetSize = buffer[offset];
                offset++;
                byte packetType = (byte)(buffer[offset] & 15);
                if (buffer.Length >= offset + packetSize && minimumPacketSizes[packetType] <= packetSize)
                {
                    switch (packetType)
                    {
                        case 6:
                            this.ParseType6(buffer, offset);
                            DisplayDetailedData(buffer, offset, packetSize);
                            break;

                        case 7:
                            if (!this.ParseType7(buffer, offset))
                                offset = buffer.Length;
                            DisplayDetailedData(buffer, offset, packetSize);
                            break;

                        default:
                            Logger.Log(string.Format(Resources.PacketType, offset, "Unknown: " + packetType));
                            DisplayDetailedData(buffer, offset - 1, packetSize + 1);
                            break;
                    }
                    offset += packetSize;
                }
                else
                {
                    Logger.Log(string.Format(Resources.PacketType, offset, packetType) + Resources.TooShortPacket);
                    return;
                }
            }
        }

        /// <summary>
        /// Checks is the time valid
        /// </summary>
        /// <param name="time">time</param>
        /// <returns>Is time valid</returns>
        private static bool IsValid(DateTime time)
        {
            DateTime now = DateTime.Now;
            if (now > time)
                return true;
            else
                return time.Subtract(now).TotalDays < 1;
        }

        /// <summary>
        /// Parses time
        /// </summary>
        /// <param name="buffer">Data buffer</param>
        /// <param name="offset">offset</param>
        /// <returns>time</returns>
        private static DateTime ParseTime(byte[] buffer, int offset)
        {
            uint dif = BitConverter.ToUInt32(buffer, offset) >> 4;
            return new DateTime(2008, 1, 1).AddSeconds(dif * 2);
        }

        /// <summary>
        /// Displays detailed data to log
        /// </summary>
        /// <param name="buffer">Packet buffer</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="length">Length</param>
        private void DisplayDetailedData(byte[] buffer, int startIndex, int length)
        {
            if (buffer.Length <= startIndex)
                startIndex = buffer.Length - 1;
            if (startIndex + length > buffer.Length)
                length = buffer.Length - startIndex;
            Logger.Log(HexConverter.ByteArrayToHex(buffer, startIndex, length));
        }

        /// <summary>
        /// Parses type 6 packet
        /// </summary>
        /// <param name="buffer">Data buffer</param>
        /// <param name="offset">Offset</param>
        private void ParseType6(byte[] buffer, int offset)
        {
            DeviceError data = new DeviceError();
            data.Time = ParseTime(buffer, offset);
            data.ServerTime = DateTime.Now;
            data.Function = buffer[offset + 4];
            data.FunctionWarning = buffer[offset + 5];
            Logger.Log(string.Format(Resources.PacketType, offset, 6) + " " + data.Time + " " + data.Function + "-" + data.FunctionWarning);
            parsedPackets.Add(data);
        }

        /// <summary>
        /// Parses type 7 packet
        /// </summary>
        /// <param name="buffer">Data buffer</param>
        /// <param name="offset">Offset</param>
        private bool ParseType7(byte[] buffer, int offset)
        {
            MergedData packet = new MergedData();
            packet.Time = ParseTime(buffer, offset);
            Logger.Log(string.Format(Resources.PacketType, offset, 7) + " " + packet.Time);
            if (IsValid(packet.Time))
            {
                ushort[] masks = new ushort[4];
                int k = 0;
                offset += 4;
                while (true)
                {
                    if (offset + 2 > buffer.Length)
                    {
                        Logger.Log("Too short packet. Masks parsing.");
                        return false;
                    }
                    ushort mask = BitConverter.ToUInt16(buffer, offset);
                    offset += 2;
                    if (k < masks.Length)
                        masks[k] = mask;
                    k++;
                    if ((mask & 0x8000) != 0x8000)
                    {
                        break;
                    }
                }
                ushort mask1 = masks[0];
                ushort mask2 = masks[1];
                ushort mask3 = masks[2];
                ushort mask4 = masks[3];
                Logger.Log(string.Format("Mask1={0}, Mask2={1}, Mask3={2}, Mask4={3}", mask1.ToString("X4"), mask2.ToString("X4"), mask3.ToString("X4"), mask4.ToString("X4")));
                int reqSize = CalculateRequiredSize(mask1, mask2, mask3, mask4);
                if (offset + reqSize > buffer.Length)
                {
                    Logger.Log("Too short packet. Data part. Expected: " + (offset + reqSize) + ", was: " + buffer.Length);
                    return false;
                }
                if ((mask1 & 0x0001) != 0)
                {
                    packet.Gps = ParseGpsFrom7(buffer, offset);
                    offset += 17;
                }

                if ((mask1 & 0x0002) != 0)
                {
                    packet.DigitalInputs = BitConverter.ToUInt16(buffer, offset);
                    offset += 2;
                }

                if ((mask1 & 0x06FC) != 0)
                {
                    packet.AnalogInputs = new ushort[8];
                    if ((mask1 & 0x0004) != 0)
                    {
                        packet.AnalogInputs[0] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask1 & 0x0008) != 0)
                    {
                        packet.AnalogInputs[1] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask1 & 0x0010) != 0)
                    {
                        packet.AnalogInputs[2] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask1 & 0x0020) != 0)
                    {
                        packet.AnalogInputs[3] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask1 & 0x0040) != 0)
                    {
                        packet.AnalogInputs[4] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask1 & 0x0080) != 0)
                    {
                        packet.AnalogInputs[5] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask1 & 0x0100) != 0)
                    {
                        packet.AnalogInputs[6] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask1 & 0x0200) != 0)
                    {
                        packet.AnalogInputs[7] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                }
                if ((mask1 & 0x0C00) != 0)
                {
                    packet.Counters = new ushort[4];
                    if ((mask1 & 0x0400) != 0)
                    {
                        packet.Counters[0] = BitConverter.ToUInt16(buffer, offset);
                        packet.Counters[1] = BitConverter.ToUInt16(buffer, offset + 2);
                        offset += 4;
                    }
                    if ((mask1 & 0x0800) != 0)
                    {
                        packet.Counters[2] = BitConverter.ToUInt16(buffer, offset);
                        packet.Counters[3] = BitConverter.ToUInt16(buffer, offset + 2);
                        offset += 4;
                    }
                }
                if ((mask1 & 0x3000) != 0)
                {
                    packet.FuelLevels = new ushort[2];
                    if ((mask1 & 0x1000) != 0)
                    {
                        packet.FuelLevels[0] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask1 & 0x2000) != 0)
                    {
                        packet.FuelLevels[1] = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                }
                if ((mask1 & 0x4000) != 0)
                {
                    packet.Gsm = ParseGsmFrom7(buffer, offset);
                    offset += 9;
                }

                if ((mask2 & 0x007F) != 0)
                {
                    MergedData.CanGeneralPart can = new MergedData.CanGeneralPart();
                    if ((mask2 & 0x0001) != 0)
                    {
                        can.WheelSpeed = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask2 & 0x0002) != 0)
                    {
                        can.AccelarationPedalPosition = buffer[offset];
                        offset += 1;
                    }
                    if ((mask2 & 0x0004) != 0)
                    {
                        can.TotalFuelUsed = BitConverter.ToUInt32(buffer, offset);
                        offset += 4;
                    }
                    if ((mask2 & 0x0008) != 0)
                    {
                        can.FuelLevel = buffer[offset];
                        offset += 1;
                    }
                    if ((mask2 & 0x0010) != 0)
                    {
                        can.EngineSpeed = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    if ((mask2 & 0x0020) != 0)
                    {
                        can.TotalEngineHours = BitConverter.ToUInt32(buffer, offset);
                        offset += 4;
                    }
                    if ((mask2 & 0x0040) != 0)
                    {
                        can.TotalVehicleDistance = BitConverter.ToUInt32(buffer, offset);
                        offset += 4;
                    }
                    packet.CanGeneral = can;
                }
                if ((mask2 & 0x0080) != 0)
                {
                    packet.EngineCoolantTemperature = buffer[offset];
                    offset += 1;
                }

                if ((mask2 & 0x0700) != 0)
                {
                    MergedData.J1939Part j1939 = packet.J1939 ?? new MergedData.J1939Part();

                    if ((mask2 & 0x0100) != 0)
                    {
                        j1939.fuel_level2 = buffer[offset];
                        offset += 1;
                    }

                    if ((mask2 & 0x0200) != 0)
                    {
                        j1939.engine_load = buffer[offset];
                        offset += 1;
                    }

                    if ((mask2 & 0x0400) != 0)
                    {
                        j1939.service_distance = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    packet.J1939 = j1939;
                }

                if ((mask2 & 0x0800) != 0)
                {
                    packet.J1939Tco1 = ParseTCO1From7(buffer, offset);
                    offset += 8;
                }
                if ((mask2 & 0x7000) != 0 || (mask3 & 0x000F) != 0)
                {
                    MergedData.J1939Part j1939;
                    if (packet.J1939 != null)
                    {
                        j1939 = packet.J1939.Value;
                    }
                    else
                        j1939 = new MergedData.J1939Part();

                    if ((mask2 & 0x1000) != 0)
                    {
                        j1939.air_temp = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }

                    if ((mask2 & 0x2000) != 0)
                    {
                        j1939.driver_id = BitConverter.ToUInt64(buffer, offset);
                        offset += 8;
                    }

                    if ((mask2 & 0x4000) != 0)
                    {
                        j1939.fuel_rate = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }

                    if ((mask3 & 0x0001) != 0)
                    {
                        j1939.fuel_fms_economy = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }

                    if ((mask3 & 0x0002) != 0)
                    {
                        j1939.fuel_consumption = BitConverter.ToUInt32(buffer, offset);
                        offset += 4;
                    }

                    if ((mask3 & 0x0004) != 0)
                    {
                        j1939.AxleNumber = (byte)(buffer[offset] >> 4);
                        j1939.TireNumber = (byte)(buffer[offset] & 0x0F);
                        j1939.AxleWeight = BitConverter.ToUInt16(buffer, offset + 1);
                        offset += 3;
                    }

                    if ((mask3 & 0x0008) != 0)
                    {
                        j1939.mil_status = buffer[offset];
                        offset += 1;
                    }

                    packet.J1939 = j1939;
                }
                if ((mask3 & 0x0010) != 0)
                {
                    packet.Dtc = new ushort[10];
                    for (int i = 0; i < 10; i++)
                        packet.Dtc[i] = BitConverter.ToUInt16(buffer, offset + i * 2);
                    offset += 20;
                }

                if ((mask3 & 0x0020) != 0)
                {
                    packet.CarInPhone = BitConverter.ToUInt16(buffer, offset);
                    offset += 2;
                }
                if ((mask3 & 0x01C0) != 0)
                {
                    MergedData.DallasPart dallas = new MergedData.DallasPart();
                    if ((mask3 & 0x0040) != 0)
                    {
                        dallas.id = BitConverter.ToUInt64(buffer, offset);
                        Logger.Log(string.Format("DallasId={0}", dallas.id));
                        offset += 8;
                    }

                    if ((mask3 & 0x0080) != 0)
                    {
                        dallas.temp = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }

                    if ((mask3 & 0x0100) != 0)
                    {
                        dallas.humidity = BitConverter.ToUInt16(buffer, offset);
                        offset += 2;
                    }
                    packet.Dallas = dallas;
                }
                if ((mask3 & 0x0600) != 0)
                {
                    MergedData.LlsPart lls = new MergedData.LlsPart();
                    lls.lvl_add = new ushort[4];
                    lls.temp_add = new sbyte[4];
                    if ((mask3 & 0x0200) != 0)
                    {
                        lls.lvl_add[0] = BitConverter.ToUInt16(buffer, offset);
                        lls.temp_add[0] = unchecked((sbyte)buffer[offset + 2]);
                        lls.lvl_add[1] = BitConverter.ToUInt16(buffer, offset + 3);
                        lls.temp_add[1] = unchecked((sbyte)buffer[offset + 5]);
                        offset += 6;
                    }
                    if ((mask3 & 0x0400) != 0)
                    {
                        lls.lvl_add[2] = BitConverter.ToUInt16(buffer, offset);
                        lls.temp_add[2] = unchecked((sbyte)buffer[offset + 2]);
                        lls.lvl_add[3] = BitConverter.ToUInt16(buffer, offset + 3);
                        lls.temp_add[3] = unchecked((sbyte)buffer[offset + 5]);
                        offset += 6;
                    }
                    packet.Lls = lls;
                }
                if ((mask3 & 0x0800) != 0)
                {
                    packet.J1979 = ParseJ1979Group1From7(buffer, offset);
                    offset += 21;
                }
                if ((mask3 & 0x1000) != 0)
                {
                    packet.J1979Dtc = new ushort[10];
                    for (int i = 0; i < 10; i++)
                        packet.J1979Dtc[i] = BitConverter.ToUInt16(buffer, offset + i * 2);
                    offset += 20;
                }

                if ((mask3 & 0x2000) != 0)
                {
                    packet.J1708 = ParseJ1708Group1From7(buffer, offset);
                    offset += 9;
                }

                if ((mask3 & 0x4000) != 0)
                {
                    packet.DrivingQuality = new byte[21];
                    Array.Copy(buffer, offset, packet.DrivingQuality, 0, 21);
                    offset += 21;
                }

                if ((mask4 & 0x0001) != 0)
                {
                    packet.wiegand26_id = BitConverter.ToUInt32(buffer, offset);
                    offset += 4;
                }

                if ((mask4 & 0x0004) != 0)
                {
                    packet.fuel_con_ins = BitConverter.ToSingle(buffer, offset);
                    offset += 4;
                }
                if ((mask4 & 0x0008) != 0)
                {
                    packet.AxleGroup = new ushort[5];
                    for (int i = 0; i < 5; i++)
                        packet.AxleGroup[i] = BitConverter.ToUInt16(buffer, offset + i * 2);
                    offset += 10;
                }
                if ((mask4 & 0x01F0) != 0)
                {
                    VaryingItemStorage storage = new VaryingItemStorage();
                    if ((mask4 & 0x0010) != 0)
                    {
                        storage.Add(ValueIds.j1939_adblue, buffer[offset]); // j1939_adblue
                        offset++;
                    }
                    if ((mask4 & 0x0020) != 0)
                    {
                        storage.Add(ValueIds.prev_digital_inputs, BitConverter.ToUInt16(buffer, offset)); // prev_digital_inputs
                        offset += 2;
                    }
                    if ((mask4 & 0x0040) != 0)
                    {
                        storage.Add(ValueIds.wln_accel_max, buffer[offset]); // wln_accel_max
                        storage.Add(ValueIds.wln_brk_max, buffer[offset + 1]); // wln_brk_max
                        storage.Add(ValueIds.wln_crn_max, buffer[offset + 2]); // wln_crn_max
                        offset += 3;
                    }
                    if ((mask4 & 0x0080) != 0)
                    {
                        storage.Add(ValueIds.CAN_Message_0700, BitConverter.ToInt64(buffer, offset)); // CAN_Message_0700
                        storage.Add(ValueIds.CAN_Message_0760, BitConverter.ToInt64(buffer, offset + 8)); // CAN_Message_0760
                        offset += 16;
                    }
                    if ((mask4 & 0x0100) != 0)
                    {
                        storage.Add(ValueIds.OneWireTemp1, BitConverter.ToUInt16(buffer, offset)); // OneWireTemp1
                        storage.Add(ValueIds.OneWireTemp1ID, BitConverter.ToInt64(buffer, offset + 2)); // OneWireTemp1ID
                        storage.Add(ValueIds.OneWireTemp2, BitConverter.ToUInt16(buffer, offset + 10)); // OneWireTemp2
                        storage.Add(ValueIds.OneWireTemp2ID, BitConverter.ToInt64(buffer, offset + 12)); // OneWireTemp2ID
                        storage.Add(ValueIds.OneWireTemp3, BitConverter.ToUInt16(buffer, offset + 20)); // OneWireTemp3
                        storage.Add(ValueIds.OneWireTemp3ID, BitConverter.ToInt64(buffer, offset + 22)); // OneWireTemp3ID
                        storage.Add(ValueIds.OneWireTemp4, BitConverter.ToUInt16(buffer, offset + 30)); // OneWireTemp4
                        storage.Add(ValueIds.OneWireTemp4ID, BitConverter.ToInt64(buffer, offset + 32)); // OneWireTemp4ID
                        offset += 40;
                    }
                    packet.MiscItems = storage.GetCalculated();
                }
                parsedPackets.Add(packet);
            }
            else
                Logger.Log(Resources.BadTime);
            return true;
        }

        private static MergedData.J1939Tco1Part ParseTCO1From7(byte[] buffer, int offset)
        {
            MergedData.J1939Tco1Part tco = new MergedData.J1939Tco1Part();
            tco.driver1_work = (byte)(buffer[offset] & 0x07);
            tco.driver2_work = (byte)((buffer[offset] >> 3) & 0x07);
            tco.motion = (byte)((buffer[offset] >> 6) & 0x03);
            tco.driver1_time = (byte)(buffer[offset + 1] & 0x07);
            tco.driver1_crd = (byte)((buffer[offset + 1] >> 3) & 0x03);
            tco.overspeed = (byte)((buffer[offset + 1] >> 5) & 0x03);

            tco.driver2_time = (byte)(buffer[offset + 2] & 0x07);
            tco.driver2_crd = (byte)((buffer[offset + 2] >> 3) & 0x03);

            tco.tcoevent = (byte)(buffer[offset + 3] & 0x03);
            tco.handl = (byte)((buffer[offset + 3] >> 2) & 0x03);
            tco.perf = (byte)((buffer[offset + 3] >> 4) & 0x03);
            tco.dir = (byte)((buffer[offset + 3] >> 6) & 0x03);

            tco.speed = BitConverter.ToUInt16(buffer, offset + 6);
            return tco;
        }

        private static MergedData.J1979Part ParseJ1979Group1From7(byte[] buffer, int offset)
        {
            MergedData.J1979Part j1979 = new MergedData.J1979Part();
            j1979.mil_status = buffer[offset];
            j1979.engine_load = buffer[offset + 1];
            j1979.engine_temp = buffer[offset + 2];
            j1979.short_fuel_trim1 = buffer[offset + 3];
            j1979.long_fuel_trim1 = buffer[offset + 4];
            j1979.short_fuel_trim2 = buffer[offset + 5];
            j1979.long_fuel_trim2 = buffer[offset + 6];
            j1979.fuel_pressure = buffer[offset + 7];
            j1979.map = buffer[offset + 8];
            j1979.rpm = BitConverter.ToUInt16(buffer, offset + 9);
            j1979.speed = buffer[offset + 11];
            j1979.air_temp = buffer[offset + 12];
            j1979.maf = BitConverter.ToUInt16(buffer, offset + 13);
            j1979.tps = buffer[offset + 15];
            j1979.fuel_level = buffer[offset + 16];
            j1979.rail_pressure = buffer[offset + 17];
            j1979.hybrid_battery_life = buffer[offset + 18];
            j1979.fuel_rate = BitConverter.ToUInt16(buffer, offset + 19);
            return j1979;
        }

        private static MergedData.J1708Part ParseJ1708Group1From7(byte[] buffer, int offset)
        {
            MergedData.J1708Part j1708 = new MergedData.J1708Part();
            j1708.engine_hrs = BitConverter.ToUInt32(buffer, offset + 0);
            j1708.fuel_used = BitConverter.ToUInt32(buffer, offset + 4);
            j1708.fuel_level = buffer[offset + 8];
            return j1708;
        }

        private static int CalculateRequiredSize(ushort mask1, ushort mask2, ushort mask3, ushort mask4)
        {
            int size = 0;
            if ((mask1 & 0x0001) != 0)
                size += 17;
            if ((mask1 & 0x0002) != 0)
                size += 2;
            if ((mask1 & 0x0004) != 0)
                size += 2;
            if ((mask1 & 0x0008) != 0)
                size += 2;
            if ((mask1 & 0x0010) != 0)
                size += 2;
            if ((mask1 & 0x0020) != 0)
                size += 2;
            if ((mask1 & 0x0040) != 0)
                size += 2;
            if ((mask1 & 0x0080) != 0)
                size += 2;
            if ((mask1 & 0x0100) != 0)
                size += 2;
            if ((mask1 & 0x0200) != 0)
                size += 2;
            if ((mask1 & 0x0400) != 0)
                size += 4;
            if ((mask1 & 0x0800) != 0)
                size += 4;
            if ((mask1 & 0x1000) != 0)
                size += 2;
            if ((mask1 & 0x2000) != 0)
                size += 2;
            if ((mask1 & 0x4000) != 0)
                size += 9;
            if ((mask2 & 0x0001) != 0)
                size += 2;
            if ((mask2 & 0x0002) != 0)
                size += 1;
            if ((mask2 & 0x0004) != 0)
                size += 4;
            if ((mask2 & 0x0008) != 0)
                size += 1;
            if ((mask2 & 0x0010) != 0)
                size += 2;
            if ((mask2 & 0x0020) != 0)
                size += 4;
            if ((mask2 & 0x0040) != 0)
                size += 4;
            if ((mask2 & 0x0080) != 0)
                size += 1;
            if ((mask2 & 0x0100) != 0)
                size += 1;
            if ((mask2 & 0x0200) != 0)
                size += 1;
            if ((mask2 & 0x0400) != 0)
                size += 2;
            if ((mask2 & 0x0800) != 0)
                size += 8;
            if ((mask2 & 0x1000) != 0)
                size += 2;
            if ((mask2 & 0x2000) != 0)
                size += 8;
            if ((mask2 & 0x4000) != 0)
                size += 2;
            if ((mask3 & 0x0001) != 0)
                size += 2;
            if ((mask3 & 0x0002) != 0)
                size += 4;
            if ((mask3 & 0x0004) != 0)
                size += 3;
            if ((mask3 & 0x0008) != 0)
                size += 1;
            if ((mask3 & 0x0010) != 0)
                size += 20;
            if ((mask3 & 0x0020) != 0)
                size += 2;
            if ((mask3 & 0x0040) != 0)
                size += 8;
            if ((mask3 & 0x0080) != 0)
                size += 2;
            if ((mask3 & 0x0100) != 0)
                size += 2;
            if ((mask3 & 0x0200) != 0)
                size += 6;
            if ((mask3 & 0x0400) != 0)
                size += 6;
            if ((mask3 & 0x0800) != 0)
                size += 21;
            if ((mask3 & 0x1000) != 0)
                size += 20;
            if ((mask3 & 0x2000) != 0)
                size += 9;
            if ((mask3 & 0x4000) != 0)
                size += 21;

            if ((mask4 & 0x0001) != 0)
                size += 4;
            if ((mask4 & 0x0004) != 0)
                size += 4;
            if ((mask4 & 0x0008) != 0)
                size += 10;

            return size;
        }

        /// <summary>
        /// Parses GPS packet from type 7 packet
        /// </summary>
        /// <param name="buffer">Data buffer</param>
        /// <param name="offset">Offset</param>
        /// <returns>GPS data</returns>
        private MergedData.GpsPart? ParseGpsFrom7(byte[] buffer, int offset)
        {
            MergedData.GpsPart gps = new MergedData.GpsPart();
            gps.Longitude = BitConverter.ToSingle(buffer, offset + 0);
            gps.Latitude = BitConverter.ToSingle(buffer, offset + 4);
            if (gps.Latitude >= -90 && gps.Latitude <= 90 && gps.Longitude >= -180 && gps.Longitude <= 180)
            {
                gps.Speed = buffer[offset + 8];
                gps.Satellites = (byte)(buffer[offset + 9] & 0x0F);
                gps.Hdop = (byte)((buffer[offset + 9] & 0xF0) >> 4);
                gps.Direction = (short)(buffer[offset + 10] * 2);
                gps.Altitude = BitConverter.ToInt16(buffer, offset + 11);
                gps.Odometer = BitConverter.ToSingle(buffer, offset + 13);
                Logger.Log(string.Format("7DT0: Spd={0},Sat={1},HDOP={2},Course={3},Altitude = {4},Odo={5}", gps.Speed, gps.Satellites, gps.Hdop, gps.Direction, gps.Altitude, gps.Odometer));
                return gps;
            }
            else
            {
                Logger.Log(Resources.BadCoordinates + "{X:" + gps.Longitude + " Y:" + gps.Latitude + "}");
                return null;
            }
        }

        /// <summary>
        /// Parses type 8 packet from type 7 packet
        /// </summary>
        /// <param name="buffer">Data buffer</param>
        /// <param name="offset">Offset</param>
        /// <returns>GSM cell data</returns>
        private static MergedData.GsmPart? ParseGsmFrom7(byte[] buffer, int offset)
        {
            MergedData.GsmPart gsm = new MergedData.GsmPart();
            gsm.MobileCountryCode = BitConverter.ToUInt16(buffer, offset);
            gsm.MobileNetworkCode = buffer[offset + 2];
            gsm.LocalAreaCode = BitConverter.ToUInt16(buffer, offset + 3);
            gsm.CellIdentifier = BitConverter.ToUInt16(buffer, offset + 5);
            gsm.TimingAdvance = buffer[offset + 7];
            gsm.ReceivedSignalStrength = buffer[offset + 8];
            return gsm;
        }
    }
}