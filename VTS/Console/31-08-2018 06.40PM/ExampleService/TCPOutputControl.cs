using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ExampleService.Properties;

namespace ExampleService
{
    /// <summary>
    /// TCP server for listening and sending outputs
    /// </summary>
    internal class TCPOutputControl
    {
        private Socket tcpListener;
        private OutputControlData OutputControlData;
        private bool _active;

        private UDPServer UDPServer;

        private int PackagesSent;

        private Logger Logger = new Logger("TCP Output Control");

        /// <summary>
        /// Constructor for TCP output control server
        /// </summary>
        /// <param name="ocd">Output control data storage</param>
        /// <param name="udp">UDP server to send outputs</param>
        public TCPOutputControl(OutputControlData ocd, UDPServer udp)
        {
            this.OutputControlData = ocd;
            this.UDPServer = udp;
            this.PackagesSent = 0;
        }

        public bool Active
        {
            get { return _active; }
        }

        /// <summary>
        /// Starts the TCP output control server.
        /// </summary>
        public void Start()
        {
            if (Settings.Default.OutputControlPort != 0)
            {
                this._active = true;

                // Starting a sender thread to check for pending outputs and send them
                var Sender = new Thread(SendOutputs);
                Sender.Start();

                // Starting a checker thread to check for expired confirmations and remove them
                var Checker = new Thread(CheckConfirmationExpiration);
                Checker.Start();

                // Establish the local endpoint for the socket.
                var localTCPEndPoint = new IPEndPoint(IPAddress.Any, Settings.Default.OutputControlPort);

                // Create a TCP/IP socket.
                this.tcpListener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.
                try
                {
                    tcpListener.Bind(localTCPEndPoint);
                    tcpListener.Listen(100);

                    // Start an asynchronous socket to listen for connections.
                    Logger.Log("Started listening at port {0}", Settings.Default.OutputControlPort);
                    tcpListener.BeginAccept(
                        new AsyncCallback(AcceptTCPCallback),
                        tcpListener);
                }
                catch (Exception e)
                {
                    Logger.Log(e.ToString());
                }
            }
            else
                Logger.Log("TCP output control port can't be set to 0. Server start terminated.");
        }

        /// <summary>
        /// Stop the TCP output control server
        /// </summary>
        public void Stop()
        {
            Logger.Log("TCP output server manually stopped.");
            this._active = false;
            try
            {
                this.tcpListener.Close();
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
        }

        private void AcceptTCPCallback(IAsyncResult ar)
        {
            if (!this._active) return;
            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            Logger.Log("Got new connection: " + handler.RemoteEndPoint);
            // Reinitiate socket listening
            try
            {
                tcpListener.BeginAccept(
                        new AsyncCallback(AcceptTCPCallback),
                        tcpListener);
            }
            catch(Exception e)
            {
                Logger.Log("Error in tcpListener.BeginAccept(): " + e.Message);
            }

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;

            // Begin recieving packet.
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Try reading data from the client socket.
            try
            {
                state.BytesRecieved = handler.EndReceive(ar);
            }
            catch(Exception e)
            {
                Logger.Log("Connection broke: " + e.Message);
            }

            if (state.BytesRecieved > 0)
            {
                Logger.Log("Recieved {0} bytes from socket.", state.BytesRecieved);

                if (state.BytesRecieved >= 11)
                {
                    ParseData(state);
                    try
                    {
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }
                    catch(Exception e)
                    {
                        Logger.Log("Connection closed: " + e.Message);
                    }
                }
                else
                    Logger.Log("Packet too short.");
            }
        }

        /// <summary>
        /// Parse and add output control packet to stack
        /// </summary>
        /// <param name="state">Socket state</param>
        private void ParseData(StateObject state)
        {
            var result = new OutputControl();
            try
            {
                result.ImeiBuffer = Helper.ReadBlock(state.buffer, 0, 8);
                var cs = state.buffer[state.BytesRecieved - 1];
                Logger.Log(BitConverter.ToString(state.buffer, 0, state.BytesRecieved));
                if (cs == Helper.CountCheckSum(state.buffer, state.BytesRecieved - 1))
                {
                    result.OutputId = state.buffer[8];
                    result.ExpirationDate = DateTime.Now.AddMilliseconds(BitConverter.ToUInt32(state.buffer, 9));
                    byte datasize = state.buffer[13];
                    result.OutputData = Helper.ReadBlock(state.buffer, 14, datasize);
                    var id = Logger.LogOutput(result);
                    if (id != null)
                    {
                        result.Id = (int)id;
                        Logger.Log(result);
                        this.OutputControlData.AddToStack(result);
                    }
                    else
                    {
                        Logger.Log("Failed to save output packet to database, addition to stack terminated.");
                    }
                }
                else
                    Logger.Log("Checksum does not match.");
            }
            catch(Exception e)
            {
                Logger.Log("Error parsing data: " + e.Message);
            }
            //0000000000000000 01 A0860100 05 0203040506 41
        }

        /// <summary>
        /// Output sender
        /// </summary>
        private void SendOutputs()
        {
            try
            {
                while (this._active)
                {
                    Console.WriteLine("Pending output stack: " + this.OutputControlData.StackCount + "; Pending confirmation for IMEI stack: " + this.OutputControlData.ConfirmationsCount);
                    foreach (var packet in this.OutputControlData.GetPacketsFromStack())
                        SendOutput(packet);

                    Thread.Sleep(1000);
                }
            }
            catch(Exception e)
            {
                Logger.Log("Error in output sender: " + e.Message);
            }
        }

        /// <summary>
        /// Send output through available connection
        /// </summary>
        /// <param name="packet">Output control packet object</param>
        private void SendOutput(OutputControl packet)
        {
            Logger.Log("Connection for imei {0} found, trying to send output signal.", packet.Imei);
            try
            {
                var response = new byte[14 + packet.OutputData.Length];
                Array.Copy(packet.ImeiBuffer, response, 8);
                var length = 3 + packet.OutputData.Length;
                response[8] = (byte)length;
                response[9] = (byte)(length >> 8);
                response[10] = 0x41;
                response[11] = packet.OutputId;
                response[12] = (byte)this.PackagesSent;
                var index = 0;

                for (var i = 13; index < packet.OutputData.Length; i++)
                    response[i] = packet.OutputData[index++];

                response[response.Length - 1] = Helper.CountCheckSum(response, response.Length - 1);
                this.PackagesSent++;

                // If the packet is sent successfully, insert packet to output control data to wait for confirmation
                if (SendOutput(packet.Id, packet.Imei, response, this.OutputControlData.GetConnection(packet.Imei)))
                    this.OutputControlData.InsertConfirmation(packet);
            }
            catch(Exception e)
            {
                Logger.Log("Failed to send output: " + e.Message);
            }
        }

        /// <summary>
        /// Send output to the specified connection
        /// </summary>
        /// <param name="id">Output control packet id</param>
        /// <param name="imei">Device IMEI</param>
        /// <param name="response">Packet buffer for sending</param>
        /// <param name="connection">Device connection object</param>
        private bool SendOutput(int id, long imei, byte[] response, object connection)
        {
            if (connection.GetType() == typeof(Socket))
            {
                try
                {
                    ((Socket)connection).Send(response, SocketFlags.None);
                }
                catch(Exception e)
                {
                    Logger.Log("Can't reach TCP socket to send packet #{0}", id);
                    Logger.Log("Error message: " + e.Message);
                    Logger.UpdateOutputStatus(id, 6);
                    this.OutputControlData.RemoveSocket(imei);
                    return false;
                }
                return true;
            }
            else if (connection.GetType() == typeof(IPEndPoint))
            {
                this.UDPServer.Send(response, ((EndPoint)connection));
                return true;
            }

            Logger.Log("Unable to get connection type for output sending.");
            return false;
        }

        private void CheckConfirmationExpiration()
        {
            try
            {
                while (this._active)
                {
                    using (var dataAccess = Helper.GetDatabase(null))
                    {
                        dataAccess.Open();
                        dataAccess.BeginTransaction();

                        var packetIds = dataAccess.CheckConfirmationExpirations();
                        if (packetIds.Count > 0)
                            this.OutputControlData.RemovePacketConfirmation(packetIds);

                        dataAccess.Commit();
                    }

                    Thread.Sleep(5000);
                }
            }
            catch(Exception e)
            {
                Logger.Log("Error in confirmation checker.");
                Logger.Log(e);
            }
        }
    }
}