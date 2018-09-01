using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ExampleService.Properties;

namespace ExampleService
{
    /// <summary>
    /// UDP server for listening and sending packets
    /// </summary>
    internal class UDPServer
    {
        private static bool? isMono;
        private byte[] buffer = new byte[1024];
        private Socket UDPSocket;
        private OutputControlData OutputControlData;
        private bool _active;

        private Logger Logger = new Logger("UDP");

        /// <summary>
        /// Constructor for UDP server
        /// </summary>
        /// <param name="ocd">Output control data storage</param>
        public UDPServer(OutputControlData ocd)
        {
            this.OutputControlData = ocd;
        }

        public bool Active
        {
            get { return _active; }
        }

        /// <summary>
        /// Starts the UDP server.
        /// </summary>
        public void Start()
        {
            if (Settings.Default.UDPPort != 0)
            {
                this._active = true;
                // Setup the socket and buffer.
                UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                if (!IsMono())
                {
                    UDPSocket.ExclusiveAddressUse = false;
                    UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    const int SIO_UDP_CONNRESET = -1744830452;
                    byte[] inValue = { 0, 0, 0, 0 };     // == false
                    byte[] outValue = { 0, 0, 0, 0 };    // initialize to 0
                    UDPSocket.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
                }
                UDPSocket.Bind(new IPEndPoint(IPAddress.Any, Settings.Default.UDPPort));
                buffer = new byte[1024];

                // Start listening for a new packet.
                EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
                Logger.Log("Started listening at port {0}", Settings.Default.UDPPort);
                UDPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, UDPSocket);
            }
            else
                Logger.Log("UDP port can't be set to 0. Server start terminated.");
        }

        /// <summary>
        /// Send a packet to the given endpoint
        /// </summary>
        /// <param name="response">Packet buffer</param>
        /// <param name="clientEP">Client endpoint</param>
        public void Send(byte[] response, EndPoint clientEP)
        {
            UDPSocket.SendTo(response, clientEP);
            Logger.Log("Output sent to: {0}:{1}",
                ((IPEndPoint)clientEP).Address,
                ((IPEndPoint)clientEP).Port);
        }

        /// <summary>
        /// Stop the UDP server
        /// </summary>
        public void Stop()
        {
            Logger.Log("UDP server manually stopped.");
            this._active = false;
            try
            {
                this.UDPSocket.Shutdown(SocketShutdown.Both);
                this.UDPSocket.Close();
            }
            catch (Exception)
            {
            }
        }

        private static bool IsMono()
        {
            if (isMono == null)
            {
                Type t = Type.GetType("Mono.Runtime");
                isMono = (t != null);
            }
            return isMono.Value;
        }

        private void DoReceiveFrom(IAsyncResult iar)
        {
            try
            {
                if (!this._active) return;
                // Get the received packet.
                var recvSock = (Socket)iar.AsyncState;
                EndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
                int packetLength = recvSock.EndReceiveFrom(iar, ref clientEP);
                Logger.Log("Recieved packet from: {0}:{1}",
                ((IPEndPoint)clientEP).Address,
                ((IPEndPoint)clientEP).Port);

                // Copy revieved packet to local buffer
                byte[] localBuffer = new byte[packetLength];
                Array.Copy(buffer, localBuffer, packetLength);

                //Start listening for a new packet.
                EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
                UDPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, UDPSocket);

                Logger.Log("Parsing recieved data.");
                Logger.Log("---- Parser log start ----");

                if (localBuffer[10] == 0xC1)
                {
                    // Small delay in case of thread racing condition
                    Thread.Sleep(200);
                    this.OutputControlData.TryConfirmPacket(localBuffer);
                }
                else
                {
                    var parseResults = Helper.ParseData(localBuffer);
                    if (parseResults != null)
                    {
                        Logger.Log(parseResults);
                        this.OutputControlData.InsertConnection(parseResults.Imei, clientEP);

                        Logger.Log("---- Parser log end ----");
                        Logger.Log("---- Saving to database ----");

                        SaveToDatabase(parseResults);

                        Logger.Log("---- Successfully saved to database ----");
                        if (parseResults.Response != null)
                            UDPSocket.SendTo(parseResults.Response, clientEP);
                    }
                }
            }
            catch
            {
                Logger.Log("Unable to recieve UDP packet.");
            }
        }

        /// <summary>
        /// Save packet parse results to database
        /// </summary>
        /// <param name="results">Parse results</param>
        private void SaveToDatabase(ParseResults results)
        {
            try
            {
                // Getting dataAccess of the database provided in settings and saving
                using (var dataAccess = Helper.GetDatabase(results.Imei))
                {
                    dataAccess.Open();
                    dataAccess.BeginTransaction();
                    foreach (var packet in results.Packets)
                        packet.SaveToDB(dataAccess);

                    dataAccess.Commit();
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error saving to database.");
                Logger.Log(e);
            }
        }
    }
}