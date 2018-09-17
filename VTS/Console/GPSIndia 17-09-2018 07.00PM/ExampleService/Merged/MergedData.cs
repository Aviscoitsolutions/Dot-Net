using System;

namespace ExampleService.Merged
{
    internal partial class MergedData : IPacket
    {
        public ushort[] AnalogInputs { get; set; }

        public CanAxleWeightPart? Axle { get; set; }

        public ushort[] AxleGroup { get; set; }

        public CanGeneralPart? CanGeneral { get; set; }

        public ushort? CarInPhone { get; set; }

        public ushort[] Counters { get; set; }

        public DallasPart? Dallas { get; set; }

        public ushort? DigitalInputs { get; set; }

        public byte[] DrivingQuality { get; set; }

        public ushort[] Dtc { get; set; }

        public byte? EngineCoolantTemperature { get; set; }

        public float? fuel_con_ins { get; set; }

        public ushort[] FuelLevels { get; set; }

        public GpsPart? Gps { get; set; }

        public GsmPart? Gsm { get; set; }

        public long? IButton { get; set; }

        public J1708Part? J1708 { get; set; }

        public J1939Part? J1939 { get; set; }

        public J1939Tco1Part? J1939Tco1 { get; set; }

        public J1979Part? J1979 { get; set; }

        public ushort[] J1979Dtc { get; set; }

        public LlsPart? Lls { get; set; }

        public VaryingItem[] MiscItems { get; set; }

        public int[] S32Val { get; set; }

        public DateTime Time { get; set; }

        public uint? wiegand26_id { get; set; }

        public long? GetMiscItemValue(ushort id)
        {
            if (this.MiscItems == null)
                return null;
            int index = MergedDataHelper.BinarySearch(this.MiscItems, id);
            if (index < 0)
                return null;
            return this.MiscItems[index].Value;
        }

        public bool HasItem(ushort id)
        {
            if (this.MiscItems == null)
                return false;
            return MergedDataHelper.BinarySearch(this.MiscItems, id) >= 0;
        }

        public void SaveToDB(DataAccess.ITelemetryAccess dataAccess)
        {
            dataAccess.TelemetryInsert(this);
        }
    }
}