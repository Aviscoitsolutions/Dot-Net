namespace ExampleService.Merged
{
    internal partial class MergedData
    {
        /// <remarks>size: 9 bytes</remarks>
        public struct GsmPart
        {
            public ushort MobileCountryCode;

            public byte MobileNetworkCode;

            public ushort LocalAreaCode;

            public ushort CellIdentifier;

            public byte TimingAdvance;

            public byte ReceivedSignalStrength;
        }
    }
}