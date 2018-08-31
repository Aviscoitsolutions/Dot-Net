namespace ExampleService.Merged
{
    internal partial class MergedData
    {
        public struct VaryingItem
        {
            public const ushort Type0 = 0x0000;
            public const ushort TypeUInt8 = 0x1000;
            public const ushort TypeUInt16 = 0x2000;
            public const ushort TypeUInt32 = 0x3000;
            public const ushort TypeInt8 = 0x4000;
            public const ushort TypeInt16 = 0x5000;
            public const ushort TypeInt32 = 0x6000;
            public const ushort TypeInt64 = 0x7000;

            public const ushort IdPartMask = 0x0FFF;
            public const ushort TypePartMask = 0xF000;

            public long Value;
            public ushort Id;
        }
    }
}