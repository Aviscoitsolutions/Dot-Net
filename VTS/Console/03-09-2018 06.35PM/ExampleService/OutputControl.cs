using System;

namespace ExampleService
{
    internal class OutputControl
    {
        public byte[] ImeiBuffer { get; set; }
        public DateTime ExpirationDate { get; set; }
        public byte OutputId { get; set; }
        public byte[] OutputData { get; set; }
        public int Id { get; set; }

        public long Imei
        {
            get
            {
                return BitConverter.ToInt64(this.ImeiBuffer, 0);
            }
        }
    }
}