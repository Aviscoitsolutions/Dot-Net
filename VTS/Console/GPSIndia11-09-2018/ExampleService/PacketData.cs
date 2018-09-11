namespace ExampleService
{
    internal class PacketData
    {
        public int Length { get; set; }
        public byte Service { get; set; }
        public byte KeyOrPad { get; set; }
        public byte[] Data { get; set; }
    }
}
