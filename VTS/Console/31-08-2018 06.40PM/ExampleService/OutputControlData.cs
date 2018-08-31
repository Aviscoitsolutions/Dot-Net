using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ExampleService
{
    /// <summary>
    /// Output control data storage
    /// </summary>
    internal class OutputControlData
    {
        private List<OutputControl> stack;
        private Dictionary<long, DeviceConnection> connections;
        private Dictionary<long, List<PacketConfirmation>> outputConfirmations;
        private Logger Logger = new Logger("Output Control storage");

        /// <summary>
        /// Constructor
        /// </summary>
        public OutputControlData()
        {
            this.stack = new List<OutputControl>();
            this.connections = new Dictionary<long, DeviceConnection>();
            this.outputConfirmations = new Dictionary<long, List<PacketConfirmation>>();
        }

        /// <summary>
        /// Gets the number of pending confirmations
        /// </summary>
        /// <returns>Number of pending confirmations</returns>
        public int ConfirmationsCount
        {
            get { return this.outputConfirmations.Count; }
        }

        /// <summary>
        /// Gets the number of pending outputs
        /// </summary>
        /// <returns>Number of pending outputs</returns>
        public int StackCount
        {
            get { return this.stack.Count; }
        }

        /// <summary>
        /// Inserts a new packet to pending confirmations
        /// </summary>
        public void InsertConfirmation(OutputControl packet)
        {
            Logger.UpdateOutputStatus(packet.Id, 3);

            List<PacketConfirmation> list;
            lock (this.outputConfirmations)
            {
                if (!this.outputConfirmations.TryGetValue(packet.Imei, out list))
                {
                    list = new List<PacketConfirmation>();
                    this.outputConfirmations.Add(packet.Imei, list);
                }
            }
            lock (list)
            {
                list.Add(new PacketConfirmation
                {
                    PacketId = packet.Id,
                    OutputId = packet.OutputId
                });
            }
            lock (this.stack)
            {
                this.stack.Remove(packet);
            }
        }

        /// <summary>
        /// Inserts a device UDP endpoint to connections
        /// </summary>
        public void InsertConnection(long imei, EndPoint clientEP)
        {
            lock (this.connections)
            {
                DeviceConnection connection;
                if (this.connections.TryGetValue(imei, out connection))
                {
                    connection.Set(clientEP);
                }
                else
                {
                    this.connections.Add(imei, new DeviceConnection(clientEP));
                }
            }
        }

        /// <summary>
        /// Inserts a device TCP socket to connections
        /// </summary>
        public void InsertConnection(long imei, Socket socket)
        {
            lock (this.connections)
            {
                DeviceConnection connection;
                if (this.connections.TryGetValue(imei, out connection))
                {
                    connection.Set(socket);
                }
                else
                {
                    this.connections.Add(imei, new DeviceConnection(socket));
                }
            }
        }

        /// <summary>
        /// Removes TCP socket for specified imei from connections
        /// </summary>
        /// <param name="imei">Device IMEI</param>
        public void RemoveSocket(long imei)
        {
            lock (this.connections)
            {
                DeviceConnection connection;
                if (this.connections.TryGetValue(imei, out connection))
                {
                    connection.Set((Socket)null);
                }
            }
        }

        /// <summary>
        /// Tries to confirm the recieved response
        /// </summary>
        /// <param name="buffer">Packet buffer</param>
        public void TryConfirmPacket(byte[] buffer)
        {
            Logger.Log("Recieved output confirmation packet, parsing and confirming.");
            var imei = BitConverter.ToInt64(Helper.ReadBlock(buffer, 0, 8), 0);
            var outputId = buffer[11];

            lock (this.outputConfirmations)
            {
                List<PacketConfirmation> list;
                if (this.outputConfirmations.TryGetValue(imei, out list))
                {
                    lock (list)
                    {
                        for (var i = 0; i < list.Count; i++)
                        {
                            var packet = list[i];
                            if (packet.OutputId == outputId)
                            {
                                Logger.Log("Packet #" + packet.PacketId + " confirmed");
                                Logger.UpdateOutputStatus(packet.PacketId, 1);
                                if (list.Count == 1)
                                {
                                    list.Clear();
                                }
                                else
                                    list.RemoveAt(i);
                                break;
                            }
                        }
                        if (list.Count == 0)
                            this.outputConfirmations.Remove(imei);
                    }
                }
            }
        }

        /// <summary>
        /// Gets socket or endpoint of the provided device IMEI
        /// </summary>
        /// <param name="imei">Device IMEI</param>
        /// <returns>Device connection socket or endpoint as object</returns>
        public object GetConnection(long imei)
        {
            lock (this.connections)
            {
                DeviceConnection connection;
                if (this.connections.TryGetValue(imei, out connection))
                {
                    if (connection.TCPSocket != null)
                        return connection.TCPSocket;
                    else
                        return connection.UDPEndPoint;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets pending outputs from stack
        /// </summary>
        /// <returns>List of pending output packets</returns>
        public List<OutputControl> GetPacketsFromStack()
        {
            var result = new List<OutputControl>();
            if (this.stack.Count > 0)
            {
                lock (this.stack)
                {
                    for (var i = 0; i < this.stack.Count; i++)
                    {
                        var packet = this.stack[i];
                        if (packet.ExpirationDate > DateTime.Now)
                        {
                            lock (this.connections)
                            {
                                if (this.connections.ContainsKey(packet.Imei))
                                    result.Add(packet);
                            }
                        } 
                        else
                        {
                            Logger.Log("Packet expired for imei: " + packet.Imei + " with ID:" + packet.Id);
                            Logger.UpdateOutputStatus(packet.Id, 2);
                            this.stack.RemoveAt(i);
                        }
                    }
                }
            }
            return result;
        }

        public void RemovePacketConfirmation(List<int> packetIds)
        {
            var listToRemove = new List<ExpiredConfirmation>();
            foreach (var packets in this.outputConfirmations)
            {
                for (var i = 0; i < packets.Value.Count; i++)
                {
                    var packet = packets.Value[i];
                    if (packetIds.Contains(packet.PacketId))
                        listToRemove.Add(new ExpiredConfirmation { Imei = packets.Key, Index = i, PacketId = packet.PacketId });
                }
            }
            foreach (var expiredPacket in listToRemove)
            {
                Logger.Log("Packet waiting for confirmation expired for imei: {0} with ID: {1}", expiredPacket.Imei, expiredPacket.PacketId);
                this.outputConfirmations[expiredPacket.Imei].RemoveAt(expiredPacket.Index);
                if (this.outputConfirmations[expiredPacket.Imei].Count == 0)
                    this.outputConfirmations.Remove(expiredPacket.Imei);
            }    
        }

        /// <summary>
        /// Closes TCP sockets in device connections
        /// </summary>
        public void Stop()
        {
            lock (this.connections)
            {
                foreach (var conn in this.connections.Values)
                {
                    var socket = conn.TCPSocket;
                    if (socket != null)
                    {
                        try
                        {
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds output packet to pending stack
        /// </summary>
        /// <param name="output">Output packet</param>
        public void AddToStack(OutputControl output)
        {
            lock (this.stack)
                this.stack.Add(output);
        }

        private struct ExpiredConfirmation
        {
            public long Imei;
            public int Index;
            public int PacketId;
        }
    }
}