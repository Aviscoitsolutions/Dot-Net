using ExampleService.DataAccess;

namespace ExampleService
{
    /// <summary>
    /// Data packet interface
    /// </summary>
    internal interface IPacket
    {
        /// <summary>
        /// Save packet to database
        /// </summary>
        /// <param name="dataAccess">Database interface</param>
        void SaveToDB(ITelemetryAccess dataAccess);
    }
}