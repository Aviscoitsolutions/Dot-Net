using System.Data.SqlClient;
using ExampleService.Merged;
using ExampleService.Properties;
using System;
using System.Collections.Generic;

namespace ExampleService.DataAccess
{
    internal class MSSqlDataAccess : ITelemetryAccess
    {
        private SqlConnection connection;
        private SqlTransaction transaction;
        private long Imei;

        public MSSqlDataAccess(long? imei)
        {
            if (imei != null)
                this.Imei = (long)imei;
            this.connection = new SqlConnection(Settings.Default.DatabaseConnectionString);
        }

        public void Open()
        {
            if (this.connection.State != System.Data.ConnectionState.Open)
                this.connection.Open();
        }

        public void BeginTransaction()
        {
            if (this.connection.State != System.Data.ConnectionState.Open)
                this.connection.Open();
            this.transaction = this.connection.BeginTransaction();
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        public void TelemetryInsert(DeviceError item)
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO device_errors (imei, time, server_time, \"function\", function_warning) VALUES (@imei, @time, @server_time, @function, @function_warning)", connection, this.transaction))
            {
                command.Parameters.AddWithValue("@imei", this.Imei);
                command.Parameters.AddWithValue("@time",item.Time.AddHours(5.5));
                command.Parameters.AddWithValue("@server_time", item.ServerTime);
                command.Parameters.AddWithValue("@function", item.Function);
                command.Parameters.AddWithValue("@function_warning", item.FunctionWarning);
                command.ExecuteNonQuery();
            }
        }

        public void TelemetryInsert(MergedData item)
        {
            var queryGenerator = new QueryGenerator("telemetry", this.connection);
            queryGenerator.AddParam("imei", this.Imei);
             queryGenerator.AddParam("time", item.Time.AddHours(5.5));
            if (item.CanGeneral != null)
            {
                queryGenerator.AddParam("acceleration_pedal_position", item.CanGeneral.Value.AccelarationPedalPosition);
                queryGenerator.AddParam("fuel_level", item.CanGeneral.Value.FuelLevel);
                queryGenerator.AddParam("wheel_speed", item.CanGeneral.Value.WheelSpeed);
                queryGenerator.AddParam("engine_speed", item.CanGeneral.Value.EngineSpeed);
                queryGenerator.AddParam("total_fuel_used", item.CanGeneral.Value.TotalFuelUsed);
                queryGenerator.AddParam("total_engine_hours", item.CanGeneral.Value.TotalEngineHours);
                queryGenerator.AddParam("total_vehicle_distance", item.CanGeneral.Value.TotalVehicleDistance);
            }

            if (item.AnalogInputs != null)
            {
                queryGenerator.AddParam("analog_inputs0", item.AnalogInputs[0]);
                queryGenerator.AddParam("analog_inputs1", item.AnalogInputs[1]);
                queryGenerator.AddParam("analog_inputs2", item.AnalogInputs[2]);
                queryGenerator.AddParam("analog_inputs3", item.AnalogInputs[3]);
                queryGenerator.AddParam("analog_inputs4", item.AnalogInputs[4]);
                queryGenerator.AddParam("analog_inputs5", item.AnalogInputs[5]);
                queryGenerator.AddParam("analog_inputs6", item.AnalogInputs[6]);
                queryGenerator.AddParam("analog_inputs7", item.AnalogInputs[7]);
            }

            if (item.Counters != null)
            {
                queryGenerator.AddParam("counters0", item.Counters[0]);
                queryGenerator.AddParam("counters1", item.Counters[1]);
                queryGenerator.AddParam("counters2", item.Counters[2]);
                queryGenerator.AddParam("counters3", item.Counters[3]);
            }

            queryGenerator.AddParam("digital_inputs", item.DigitalInputs);

            if (item.FuelLevels != null)
            {
                queryGenerator.AddParam("fuel_levels0", item.FuelLevels[0]);
                queryGenerator.AddParam("fuel_levels1", item.FuelLevels[1]);
            }

            if (item.Gps != null)
            {
                queryGenerator.AddParam("longitude", item.Gps.Value.Longitude);
                queryGenerator.AddParam("latitude", item.Gps.Value.Latitude);
                queryGenerator.AddParam("speed", item.Gps.Value.Speed);
                queryGenerator.AddParam("satellites", item.Gps.Value.Satellites);
                queryGenerator.AddParam("hdop", item.Gps.Value.Hdop);
                queryGenerator.AddParam("direction", item.Gps.Value.Direction);
                queryGenerator.AddParam("altitude", item.Gps.Value.Altitude);
                queryGenerator.AddParam("odometer", item.Gps.Value.Odometer);
            }

            if (item.Gsm != null)
            {
                queryGenerator.AddParam("mobile_country_code", item.Gsm.Value.MobileCountryCode);
                queryGenerator.AddParam("mobile_network_code", item.Gsm.Value.MobileNetworkCode);
                queryGenerator.AddParam("local_area_code", item.Gsm.Value.LocalAreaCode);
                queryGenerator.AddParam("cell_identifier", item.Gsm.Value.CellIdentifier);
                queryGenerator.AddParam("timing_advance", item.Gsm.Value.TimingAdvance);
                queryGenerator.AddParam("recieved_signal_strength", item.Gsm.Value.ReceivedSignalStrength);
                queryGenerator.AddParam("engine_coolant_temperature", item.EngineCoolantTemperature);
            }

            if (item.J1939 != null)
            {
                queryGenerator.AddParam("j1939_air_temperature", item.J1939.Value.air_temp);
                queryGenerator.AddParam("j1939_driver_id", item.J1939.Value.driver_id);
                queryGenerator.AddParam("j1939_fuel_rate", item.J1939.Value.fuel_rate);
                queryGenerator.AddParam("j1939_fuel_fms_economy", item.J1939.Value.fuel_fms_economy);
                queryGenerator.AddParam("j1939_fuel_consumption", item.J1939.Value.fuel_consumption);
                queryGenerator.AddParam("j1939_axle_number", item.J1939.Value.AxleNumber);
                queryGenerator.AddParam("j1939_tire_number", item.J1939.Value.TireNumber);
                queryGenerator.AddParam("j1939_axle_weight", item.J1939.Value.AxleWeight);
                queryGenerator.AddParam("j1939_mil_status", item.J1939.Value.mil_status);
            }

            if (item.J1939Tco1 != null)
            {
                queryGenerator.AddParam("j1939_driver1_work", item.J1939Tco1.Value.driver1_work);
                queryGenerator.AddParam("j1939_driver2_work", item.J1939Tco1.Value.driver2_work);
                queryGenerator.AddParam("j1939_motion", item.J1939Tco1.Value.motion);
                queryGenerator.AddParam("j1939_driver1_time", item.J1939Tco1.Value.driver1_time);
                queryGenerator.AddParam("j1939_driver1_crd", item.J1939Tco1.Value.driver1_crd);
                queryGenerator.AddParam("j1939_overspeed", item.J1939Tco1.Value.overspeed);
                queryGenerator.AddParam("j1939_driver2_time", item.J1939Tco1.Value.driver2_time);
                queryGenerator.AddParam("j1939_driver2_crd", item.J1939Tco1.Value.driver2_crd);
                queryGenerator.AddParam("j1939_tcoevent", item.J1939Tco1.Value.tcoevent);
                queryGenerator.AddParam("j1939_handl", item.J1939Tco1.Value.handl);
                queryGenerator.AddParam("j1939_perf", item.J1939Tco1.Value.perf);
                queryGenerator.AddParam("j1939_dir", item.J1939Tco1.Value.dir);
                queryGenerator.AddParam("j1939_speed", item.J1939Tco1.Value.speed);
            }

            if (item.Dtc != null)
            {
                queryGenerator.AddParam("dtc0", item.Dtc[0]);
                queryGenerator.AddParam("dtc1", item.Dtc[1]);
                queryGenerator.AddParam("dtc2", item.Dtc[2]);
                queryGenerator.AddParam("dtc3", item.Dtc[3]);
                queryGenerator.AddParam("dtc4", item.Dtc[4]);
                queryGenerator.AddParam("dtc5", item.Dtc[5]);
                queryGenerator.AddParam("dtc6", item.Dtc[6]);
                queryGenerator.AddParam("dtc7", item.Dtc[7]);
                queryGenerator.AddParam("dtc8", item.Dtc[8]);
                queryGenerator.AddParam("dtc9", item.Dtc[9]);
            }

            queryGenerator.AddParam("car_in_phone", item.CarInPhone);

            if (item.Dallas != null)
            {
                queryGenerator.AddParam("dallas_id", item.Dallas.Value.id);
                queryGenerator.AddParam("dallas_temperature", item.Dallas.Value.temp);
                queryGenerator.AddParam("dallas_humidity", item.Dallas.Value.humidity);
            }

            if (item.Lls != null)
            {
                queryGenerator.AddParam("lls_lvl_add0", item.Lls.Value.lvl_add[0]);
                queryGenerator.AddParam("lls_lvl_add1", item.Lls.Value.lvl_add[1]);
                queryGenerator.AddParam("lls_lvl_add2", item.Lls.Value.lvl_add[2]);
                queryGenerator.AddParam("lls_lvl_add3", item.Lls.Value.lvl_add[3]);
                queryGenerator.AddParam("lls_temp_add0", (short)item.Lls.Value.temp_add[0]);
                queryGenerator.AddParam("lls_temp_add1", (short)item.Lls.Value.temp_add[1]);
                queryGenerator.AddParam("lls_temp_add2", (short)item.Lls.Value.temp_add[2]);
                queryGenerator.AddParam("lls_temp_add3", (short)item.Lls.Value.temp_add[3]);
            }

            if (item.J1979 != null)
            {
                queryGenerator.AddParam("j1979_mil_status", item.J1979.Value.mil_status);
                queryGenerator.AddParam("j1979_engine_load", item.J1979.Value.engine_load);
                queryGenerator.AddParam("j1979_engine_temperature", item.J1979.Value.engine_temp);
                queryGenerator.AddParam("j1979_short_fuel_trim1", item.J1979.Value.short_fuel_trim1);
                queryGenerator.AddParam("j1979_long_fuel_trim1", item.J1979.Value.long_fuel_trim1);
                queryGenerator.AddParam("j1979_short_fuel_trim2", item.J1979.Value.short_fuel_trim2);
                queryGenerator.AddParam("j1979_long_fuel_trim2", item.J1979.Value.long_fuel_trim2);
                queryGenerator.AddParam("j1979_fuel_pressure", item.J1979.Value.fuel_pressure);
                queryGenerator.AddParam("j1979_map", item.J1979.Value.map);
                queryGenerator.AddParam("j1979_rpm", item.J1979.Value.rpm);
                queryGenerator.AddParam("j1979_speed", item.J1979.Value.speed);
                queryGenerator.AddParam("j1979_air_temp", item.J1979.Value.air_temp);
                queryGenerator.AddParam("j1979_maf", item.J1979.Value.maf);
                queryGenerator.AddParam("j1979_tps", item.J1979.Value.tps);
                queryGenerator.AddParam("j1979_fuel_level", item.J1979.Value.fuel_level);
                queryGenerator.AddParam("j1979_rail_pressure", item.J1979.Value.rail_pressure);
                queryGenerator.AddParam("j1979_hybrid_battery_life", item.J1979.Value.hybrid_battery_life);
                queryGenerator.AddParam("j1979_fuel_rate", item.J1979.Value.fuel_rate);
            }

            if (item.J1979Dtc != null)
            {
                queryGenerator.AddParam("j1979_dtc0", item.J1979Dtc[0]);
                queryGenerator.AddParam("j1979_dtc1", item.J1979Dtc[1]);
                queryGenerator.AddParam("j1979_dtc2", item.J1979Dtc[2]);
                queryGenerator.AddParam("j1979_dtc3", item.J1979Dtc[3]);
                queryGenerator.AddParam("j1979_dtc4", item.J1979Dtc[4]);
                queryGenerator.AddParam("j1979_dtc5", item.J1979Dtc[5]);
                queryGenerator.AddParam("j1979_dtc6", item.J1979Dtc[6]);
                queryGenerator.AddParam("j1979_dtc7", item.J1979Dtc[7]);
                queryGenerator.AddParam("j1979_dtc8", item.J1979Dtc[8]);
                queryGenerator.AddParam("j1979_dtc9", item.J1979Dtc[9]);
            }

            if (item.J1708 != null)
            {
                queryGenerator.AddParam("j1708_engine_hours", item.J1708.Value.engine_hrs);
                queryGenerator.AddParam("j1708_fuel_used", item.J1708.Value.fuel_used);
                queryGenerator.AddParam("j1708_fuel_level", item.J1708.Value.fuel_level);
            }

            if (item.DrivingQuality != null)
            {
                queryGenerator.AddParam("driving_quality0", item.DrivingQuality[0]);
                queryGenerator.AddParam("driving_quality1", item.DrivingQuality[1]);
                queryGenerator.AddParam("driving_quality2", item.DrivingQuality[2]);
                queryGenerator.AddParam("driving_quality3", item.DrivingQuality[3]);
                queryGenerator.AddParam("driving_quality4", item.DrivingQuality[4]);
                queryGenerator.AddParam("driving_quality5", item.DrivingQuality[5]);
                queryGenerator.AddParam("driving_quality6", item.DrivingQuality[6]);
                queryGenerator.AddParam("driving_quality7", item.DrivingQuality[7]);
                queryGenerator.AddParam("driving_quality8", item.DrivingQuality[8]);
                queryGenerator.AddParam("driving_quality9", item.DrivingQuality[9]);
                queryGenerator.AddParam("driving_quality10", item.DrivingQuality[10]);
                queryGenerator.AddParam("driving_quality11", item.DrivingQuality[11]);
                queryGenerator.AddParam("driving_quality12", item.DrivingQuality[12]);
                queryGenerator.AddParam("driving_quality13", item.DrivingQuality[13]);
                queryGenerator.AddParam("driving_quality14", item.DrivingQuality[14]);
                queryGenerator.AddParam("driving_quality15", item.DrivingQuality[15]);
                queryGenerator.AddParam("driving_quality16", item.DrivingQuality[16]);
                queryGenerator.AddParam("driving_quality17", item.DrivingQuality[17]);
                queryGenerator.AddParam("driving_quality18", item.DrivingQuality[18]);
                queryGenerator.AddParam("driving_quality19", item.DrivingQuality[19]);
                queryGenerator.AddParam("driving_quality20", item.DrivingQuality[20]);
            }

            queryGenerator.AddParam("wiegand26_id", item.wiegand26_id);
            queryGenerator.AddParam("fuel_con_ins", item.fuel_con_ins);

            if (item.AxleGroup != null)
            {
                queryGenerator.AddParam("axle_group0", item.AxleGroup[0]);
                queryGenerator.AddParam("axle_group1", item.AxleGroup[1]);
                queryGenerator.AddParam("axle_group2", item.AxleGroup[2]);
                queryGenerator.AddParam("axle_group3", item.AxleGroup[3]);
                queryGenerator.AddParam("axle_group4", item.AxleGroup[4]);
            }

            if (item.MiscItems != null)
            {
                foreach (var varyingItem in item.MiscItems)
                {
                    switch (varyingItem.Id)
                    {
                        case Merged.ValueIds.j1939_adblue:
                            queryGenerator.AddParam("j1939_adblue", varyingItem.Value);
                            break;

                        case Merged.ValueIds.prev_digital_inputs:
                            queryGenerator.AddParam("prev_digital_inputs", varyingItem.Value);
                            break;

                        case Merged.ValueIds.wln_accel_max:
                            queryGenerator.AddParam("wln_accel_max", varyingItem.Value);
                            break;

                        case Merged.ValueIds.wln_brk_max:
                            queryGenerator.AddParam("wln_brk_max", varyingItem.Value);
                            break;

                        case Merged.ValueIds.wln_crn_max:
                            queryGenerator.AddParam("wln_crn_max", varyingItem.Value);
                            break;

                        case Merged.ValueIds.CAN_Message_0700:
                            queryGenerator.AddParam("can_message_0700", varyingItem.Value);
                            break;

                        case Merged.ValueIds.CAN_Message_0760:
                            queryGenerator.AddParam("can_message_0760", varyingItem.Value);
                            break;

                        case Merged.ValueIds.OneWireTemp1:
                            queryGenerator.AddParam("one_wire_temp1", varyingItem.Value);
                            break;

                        case Merged.ValueIds.OneWireTemp1ID:
                            queryGenerator.AddParam("one_wire_temp1_id", varyingItem.Value);
                            break;

                        case Merged.ValueIds.OneWireTemp2:
                            queryGenerator.AddParam("one_wire_temp2", varyingItem.Value);
                            break;

                        case Merged.ValueIds.OneWireTemp2ID:
                            queryGenerator.AddParam("one_wire_temp2_id", varyingItem.Value);
                            break;

                        case Merged.ValueIds.OneWireTemp3:
                            queryGenerator.AddParam("one_wire_temp3", varyingItem.Value);
                            break;

                        case Merged.ValueIds.OneWireTemp3ID:
                            queryGenerator.AddParam("one_wire_temp3_id", varyingItem.Value);
                            break;

                        case Merged.ValueIds.OneWireTemp4:
                            queryGenerator.AddParam("one_wire_temp4", varyingItem.Value);
                            break;

                        case Merged.ValueIds.OneWireTemp4ID:
                            queryGenerator.AddParam("one_wire_temp4_id", varyingItem.Value);
                            break;
                    }
                }
            }
            queryGenerator.ExecuteSql(this.transaction);
        }

        public void TestInsert()
        {
            var item = new MergedData();
            item.Time = DateTime.Now;
            var CanGeneral = new Merged.MergedData.CanGeneralPart();
            var Gps = new Merged.MergedData.GpsPart();
            var Gsm = new Merged.MergedData.GsmPart();
            var J1939 = new Merged.MergedData.J1939Part();
            var J1939Tco1 = new Merged.MergedData.J1939Tco1Part();
            var Dallas = new Merged.MergedData.DallasPart();
            var Lls = new Merged.MergedData.LlsPart();
            var J1979 = new Merged.MergedData.J1979Part();
            var J1708 = new Merged.MergedData.J1708Part();
            item.AnalogInputs = new ushort[8];
            item.Counters = new ushort[4];
            item.FuelLevels = new ushort[2];
            item.Dtc = new ushort[10];
            item.J1979Dtc = new ushort[10];
            item.DrivingQuality = new byte[21];
            item.AxleGroup = new ushort[5];
            Lls.lvl_add = new ushort[4];
            Lls.temp_add = new sbyte[4];

            CanGeneral.AccelarationPedalPosition = byte.MinValue;
            CanGeneral.FuelLevel = byte.MinValue;
            CanGeneral.WheelSpeed = byte.MinValue;
            CanGeneral.EngineSpeed = byte.MinValue;
            CanGeneral.TotalEngineHours = int.MaxValue;
            CanGeneral.TotalVehicleDistance = int.MaxValue;
            CanGeneral.TotalFuelUsed = int.MaxValue;
            item.AnalogInputs[0] = byte.MinValue;
            item.AnalogInputs[1] = byte.MinValue;
            item.AnalogInputs[2] = byte.MinValue;
            item.AnalogInputs[3] = byte.MinValue;
            item.AnalogInputs[4] = byte.MinValue;
            item.AnalogInputs[5] = byte.MinValue;
            item.AnalogInputs[6] = byte.MinValue;
            item.AnalogInputs[7] = byte.MinValue;
            item.Counters[0] = byte.MinValue;
            item.Counters[1] = byte.MinValue;
            item.Counters[2] = byte.MinValue;
            item.Counters[3] = byte.MinValue;
            item.DigitalInputs = byte.MinValue;
            item.FuelLevels[0] = byte.MinValue;
            item.FuelLevels[1] = byte.MinValue;
            Gps.Longitude = float.MinValue;
            Gps.Latitude = float.MinValue;
            Gps.Speed = byte.MinValue;
            Gps.Satellites = byte.MinValue;
            Gps.Hdop = byte.MinValue;
            Gps.Direction = byte.MinValue;
            Gps.Altitude = byte.MinValue;
            Gps.Odometer = float.MinValue;
            Gsm.MobileCountryCode = byte.MinValue;
            Gsm.MobileNetworkCode = byte.MinValue;
            Gsm.LocalAreaCode = byte.MinValue;
            Gsm.CellIdentifier = byte.MinValue;
            Gsm.TimingAdvance = byte.MinValue;
            Gsm.ReceivedSignalStrength = byte.MinValue;
            item.EngineCoolantTemperature = byte.MinValue;
            J1939.air_temp = byte.MinValue;
            J1939.driver_id = byte.MinValue;
            J1939.fuel_rate = byte.MinValue;
            J1939.fuel_fms_economy = byte.MinValue;
            J1939.fuel_consumption = int.MaxValue;
            J1939.AxleNumber = byte.MinValue;
            J1939.TireNumber = byte.MinValue;
            J1939.AxleWeight = byte.MinValue;
            J1939.mil_status = byte.MinValue;
            J1939Tco1.driver1_work = byte.MinValue;
            J1939Tco1.driver2_work = byte.MinValue;
            J1939Tco1.motion = byte.MinValue;
            J1939Tco1.driver1_time = byte.MinValue;
            J1939Tco1.driver1_crd = byte.MinValue;
            J1939Tco1.overspeed = byte.MinValue;
            J1939Tco1.driver2_time = byte.MinValue;
            J1939Tco1.driver2_crd = byte.MinValue;
            J1939Tco1.tcoevent = byte.MinValue;
            J1939Tco1.handl = byte.MinValue;
            J1939Tco1.perf = byte.MinValue;
            J1939Tco1.dir = byte.MinValue;
            J1939Tco1.speed = byte.MinValue;
            item.Dtc[0] = byte.MinValue;
            item.Dtc[1] = byte.MinValue;
            item.Dtc[2] = byte.MinValue;
            item.Dtc[3] = byte.MinValue;
            item.Dtc[4] = byte.MinValue;
            item.Dtc[5] = byte.MinValue;
            item.Dtc[6] = byte.MinValue;
            item.Dtc[7] = byte.MinValue;
            item.Dtc[8] = byte.MinValue;
            item.Dtc[9] = byte.MinValue;
            item.CarInPhone = byte.MinValue;
            Dallas.id = int.MaxValue;
            Dallas.temp = byte.MinValue;
            Dallas.humidity = byte.MinValue;
            Lls.lvl_add[0] = byte.MinValue;
            Lls.lvl_add[1] = byte.MinValue;
            Lls.lvl_add[2] = byte.MinValue;
            Lls.lvl_add[3] = byte.MinValue;
            Lls.temp_add[0] = 5;
            Lls.temp_add[1] = 5;
            Lls.temp_add[2] = 5;
            Lls.temp_add[3] = 5;
            J1979.mil_status = byte.MinValue;
            J1979.engine_load = byte.MinValue;
            J1979.engine_temp = byte.MinValue;
            J1979.short_fuel_trim1 = byte.MinValue;
            J1979.long_fuel_trim1 = byte.MinValue;
            J1979.short_fuel_trim2 = byte.MinValue;
            J1979.long_fuel_trim2 = byte.MinValue;
            J1979.fuel_pressure = byte.MinValue;
            J1979.map = byte.MinValue;
            J1979.rpm = byte.MinValue;
            J1979.speed = byte.MinValue;
            J1979.air_temp = byte.MinValue;
            J1979.maf = byte.MinValue;
            J1979.tps = byte.MinValue;
            J1979.fuel_level = byte.MinValue;
            J1979.rail_pressure = byte.MinValue;
            J1979.hybrid_battery_life = byte.MinValue;
            J1979.fuel_rate = byte.MinValue;
            item.J1979Dtc[0] = byte.MinValue;
            item.J1979Dtc[1] = byte.MinValue;
            item.J1979Dtc[2] = byte.MinValue;
            item.J1979Dtc[3] = byte.MinValue;
            item.J1979Dtc[4] = byte.MinValue;
            item.J1979Dtc[5] = byte.MinValue;
            item.J1979Dtc[6] = byte.MinValue;
            item.J1979Dtc[7] = byte.MinValue;
            item.J1979Dtc[8] = byte.MinValue;
            item.J1979Dtc[9] = byte.MinValue;
            J1708.engine_hrs = int.MaxValue;
            J1708.fuel_used = int.MaxValue;
            J1708.fuel_level = byte.MinValue;
            item.DrivingQuality[0] = byte.MinValue;
            item.DrivingQuality[1] = byte.MinValue;
            item.DrivingQuality[2] = byte.MinValue;
            item.DrivingQuality[3] = byte.MinValue;
            item.DrivingQuality[4] = byte.MinValue;
            item.DrivingQuality[5] = byte.MinValue;
            item.DrivingQuality[6] = byte.MinValue;
            item.DrivingQuality[7] = byte.MinValue;
            item.DrivingQuality[8] = byte.MinValue;
            item.DrivingQuality[9] = byte.MinValue;
            item.DrivingQuality[10] = byte.MinValue;
            item.DrivingQuality[11] = byte.MinValue;
            item.DrivingQuality[12] = byte.MinValue;
            item.DrivingQuality[13] = byte.MinValue;
            item.DrivingQuality[14] = byte.MinValue;
            item.DrivingQuality[15] = byte.MinValue;
            item.DrivingQuality[16] = byte.MinValue;
            item.DrivingQuality[17] = byte.MinValue;
            item.DrivingQuality[18] = byte.MinValue;
            item.DrivingQuality[19] = byte.MinValue;
            item.DrivingQuality[20] = byte.MinValue;
            item.wiegand26_id = int.MaxValue;
            item.fuel_con_ins = float.MinValue;
            item.AxleGroup[0] = byte.MinValue;
            item.AxleGroup[1] = byte.MinValue;
            item.AxleGroup[2] = byte.MinValue;
            item.AxleGroup[3] = byte.MinValue;
            item.AxleGroup[4] = byte.MinValue;
            item.CanGeneral = CanGeneral;
            item.Gps = Gps;
            item.Gsm = Gsm;
            item.J1939 = J1939;
            item.J1939Tco1 = J1939Tco1;
            item.Dallas = Dallas;
            item.Lls = Lls;
            item.J1979 = J1979;
            item.J1708 = J1708;

            var ids = new ushort[] { 0x0000, 0x0001, 0x0002, 0x0003, 0x0004, 0x0005, 0x0006, 0x0007, 0x0008, 0x0009, 0x000A, 0x000B, 0x000C, 0x000D, 0x000E };
            item.MiscItems = new Merged.MergedData.VaryingItem[15];
            for (var i = 0; i < 15; i++)
                item.MiscItems[i] = new Merged.MergedData.VaryingItem { Id = ids[i], Value = int.MaxValue };

            TelemetryInsert(item);
        }

        public void Dispose()
        {
            this.connection.Dispose();
        }

        public int OutputInsert(OutputControl item)
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO outputs (output_id, output_data, status, expiration_date) VALUES (@output_id, @output_data, @status, @expiration_date); SELECT @@IDENTITY;", connection))
            {
                command.Parameters.AddWithValue("@output_id", item.OutputId);
                command.Parameters.AddWithValue("@output_data", item.OutputData);
                command.Parameters.AddWithValue("@status", 0);
                command.Parameters.AddWithValue("@expiration_date", item.ExpirationDate);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void OutputUpdate(int id, byte status)
        {
            using (SqlCommand command = new SqlCommand("UPDATE outputs SET status = @status WHERE id = @id;", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@status", status);
                command.ExecuteNonQuery();
            }
        }

        public List<int> CheckConfirmationExpirations()
        {
            var expired = new List<int>();
            using (SqlCommand command = new SqlCommand("SELECT * FROM outputs WHERE status = 0 OR status = 3 OR status = 6;", connection, transaction))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var date = (DateTime?)reader["expiration_date"];
                        if (date != null && date < DateTime.Now)
                            expired.Add((int)reader["id"]);
                    }
                }
            }

            foreach (var id in expired)
                OutputUpdate(id, 5);

            return expired;
        }
    }
}