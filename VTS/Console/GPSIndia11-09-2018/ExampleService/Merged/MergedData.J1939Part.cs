namespace ExampleService.Merged
{
    internal partial class MergedData
    {
        /// <remarks>size: 27 bytes</remarks>
        public struct J1939Part
        {
            public byte fuel_level2;

            public byte engine_load;

            public ushort service_distance;

            public ushort air_temp;

            public ulong driver_id;

            public ushort fuel_rate;

            public ushort fuel_fms_economy;

            public uint fuel_consumption;

            public byte AxleNumber;
            public byte TireNumber;
            public ushort AxleWeight;
            public byte mil_status;
        }
    }
}