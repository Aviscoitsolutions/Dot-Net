using System;
using ExampleService.DataAccess;

namespace ExampleService
{
    internal struct DeviceError : IPacket
    {
        /// <summary>
        /// Time, UTC
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// Server time, UTC
        /// </summary>
        public DateTime ServerTime;

        /// <summary>
        /// Function
        /// </summary>
        public byte Function;

        /// <summary>
        /// Index of the function warning
        /// </summary>
        public byte FunctionWarning;

        /// <summary>
        /// Saves the packet to database
        /// </summary>
        /// <param name="dataAccess">database interface</param>
        public void SaveToDB(ITelemetryAccess dataAccess)
        {
            dataAccess.TelemetryInsert(this);
        }

        /// <summary>
        /// Prints the object as string
        /// </summary>
        /// <returns>String of the object</returns>
        public override string ToString()
        {
            return string.Format("DeviceError{{Time={0},ServerTime={1},Function={2},FunctionWarning={3}}}", this.Time, this.ServerTime, this.Function, this.FunctionWarning);
        }
    }
}