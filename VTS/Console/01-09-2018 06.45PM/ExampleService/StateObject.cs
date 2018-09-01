using System.Net.Sockets;

namespace ExampleService
{
    /// <summary>
    /// Object for saving socket and additional information
    /// </summary>
    internal class StateObject
    {
        /// <summary>
        /// Client socket
        /// </summary>
        public Socket workSocket = null;

        /// <summary>
        /// Size of receive buffer
        /// </summary>
        public const int BufferSize = 1024;

        /// <summary>
        /// Connection validity
        /// </summary>
        public bool Accepted = false;

        /// <summary>
        /// Receive buffer
        /// </summary>
        public byte[] buffer = new byte[BufferSize];

        /// <summary>
        /// Bytes recieved
        /// </summary>
        public int BytesRecieved = 0;
    }
}