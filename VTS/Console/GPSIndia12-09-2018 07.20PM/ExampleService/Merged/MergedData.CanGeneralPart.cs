namespace ExampleService.Merged
{
    internal partial class MergedData
    {
        /// <remarks>size: 18 bytes</remarks>
        public struct CanGeneralPart
        {
            public byte AccelarationPedalPosition;

            public byte FuelLevel;

            public ushort WheelSpeed;

            public ushort EngineSpeed;

            public uint TotalFuelUsed;

            public uint TotalEngineHours;

            public uint TotalVehicleDistance;
        }
    }
}