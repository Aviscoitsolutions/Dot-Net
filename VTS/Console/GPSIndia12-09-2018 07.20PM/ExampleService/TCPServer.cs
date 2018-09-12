using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ExampleService.Properties;


namespace ExampleService
{
    
    /// <summary>
    /// TCP server for listening and sending packets
    /// </summary>
    internal class TCPServer
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        private Socket tcpListener;
        private OutputControlData OutputControlData;
        private bool _active;
     
        private Logger Logger = new Logger("TCP");
        public static string nowignation;






        /// <summary>
        /// Constructor for TCP server
        /// </summary>
        /// <param name="ocd">Output control data storage</param>
        public TCPServer(OutputControlData ocd)
        {
           
            this.OutputControlData = ocd;
        }

        public bool Active
        {
            get { return _active; }
        }

        /// <summary>
        /// Starts the TCP server.
        /// </summary>
        public void Start()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls;
            if (Settings.Default.TCPPort != 0)
            {
                this._active = true;
                // Establish the local endpoint for the socket.
             
                var localTCPEndPoint = new IPEndPoint(IPAddress.Any, Settings.Default.TCPPort);

                // Create a TCP/IP socket.
                tcpListener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.
                try
                {
                    tcpListener.Bind(localTCPEndPoint);
                    tcpListener.Listen(100);

                    // Start an asynchronous socket to listen for connections.
                    Logger.Log("Started listening at port {0}", Settings.Default.TCPPort);
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
                Logger.Log("TCP port can't be set to 0. Server start terminated.");
        }

        /// <summary>
        /// Stop the TCP server
        /// </summary>
        public void Stop()
        {
            Logger.Log("TCP server manually stopped.");
            this._active = false;
            this.OutputControlData.Stop();
            try
            {
                this.tcpListener.Close();
            }
            catch (Exception)
            {
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
            catch
            {
                Logger.Log("Error in tcpListener.BeginAccept()");
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
            byte[] localBuffer = null;

            // Try reading data from the client socket.
            try
            {
                state.BytesRecieved = handler.EndReceive(ar);
                // Copy retieved packet buffer to a local buffer for handling
                localBuffer = new byte[state.BytesRecieved];
                Array.Copy(state.buffer, localBuffer, state.BytesRecieved);
            }
            catch
            {
                Logger.Log("Connection broke");
            }

            if (state.BytesRecieved > 0 && localBuffer != null)
            {
               
                Logger.Log("Recieved {0} bytes from socket.", state.BytesRecieved);
                Logger.Log("Parsing Data....");

                if (state.Accepted == false)
                {
                   // Logger.Log("Checking for keyword validity.");
                    if (Helper.CheckForKeyword(localBuffer))
                    {
                        Logger.Log("Parsing Data....");
                        state.Accepted = true;
                    }
                    else
                    {
                        Logger.Log("Keyword invalid, closing connection.");
                        state.workSocket.Close();
                    }
                }
                else if (state.BytesRecieved >= 15)
                {
                    byte[] myArray = new byte[1000];
                    myArray = new byte[1000];
                    // int s = 36;
                    //var s= Array.IndexOf(localBuffer, 120,);
                    // for (int i=0;i<localBuffer.Length-1;i++)
                    // {

                    //     myArray[i] =localBuffer[i];
                    //    if(i==s)
                    //     {
                    //         Console.WriteLine(myArray);
                    //         s = s + s;
                    //         myArray = new byte[36];
                    //     }
                    // }


                    int position = 0;

                    int lastposition = localBuffer.Length - 1;
                    int c = 0;
                    for (int i = position; i < lastposition; i++)
                    {
                        
                       
                        if(localBuffer[i] == 13 && localBuffer[i+1] == 10)
                        {
                            int diff = i+2 - position;
                            myArray = new byte[diff];
                           
                            for (int j = position; j <= i + 1; j++)
                            {
                              //state.  Accepted = false;
                                myArray[c] = localBuffer[j];
                                c = c + 1;


                            }
                            position =i + 1;
                            c = 0;
                            if (myArray.Length == 15)
                            {

                                var ignation = myArray.Skip(4).Take(1).ToArray();
                                string ignationstring = BitConverter.ToString(ignation).Replace("-", "");
                                string binaryval;


                                binaryval = Convert.ToString(Convert.ToInt32(ignationstring, 16), 2);

                                char s = binaryval[binaryval.Length - 2];
                                if (s == '1')
                                {
                                    nowignation = "41209";
                                }
                                else
                                {
                                    nowignation = "32936";
                                }



                            }
                            else  if (myArray.Length ==37 )
                            {


                                Console.Write("Actual Data:" + BitConverter.ToString(myArray));

                                string imei = Helper.imei;
                                Console.WriteLine("IMEI:" + imei.ToString());

                                //Console.WriteLine("IMEI: " + BitConverter.ToString(selected));
                                var datetake = myArray.Skip(5).Take(6).ToArray();
                                string date1 = myArray[7].ToString() + "-" + myArray[6].ToString() + "-" + myArray[5].ToString() + " " + myArray[8].ToString() + ":" + myArray[9].ToString() + ":" + myArray[10].ToString();

                                var converteddate = Convert.ToDateTime(date1.ToString());
                                Console.WriteLine("Date:" + Convert.ToDateTime(date1.ToString()));
                                //Sattlite
                                var sattake = myArray.Skip(11).Take(1).ToArray();
                                string satstring = BitConverter.ToString(sattake).Replace("-", "");
                                
                                string sat = satstring[satstring.Length - 1].ToString();
                                decimal satd = (decimal)Int64.Parse(sat, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Sattalite Count:" + satd.ToString());
                                //Latitude
                                var lattake = myArray.Skip(12).Take(4).ToArray();
                                string latstring = BitConverter.ToString(lattake).Replace("-", "");
                                decimal latd = (decimal)Int64.Parse(latstring, System.Globalization.NumberStyles.HexNumber);
                                decimal latt = latd / 30000;
                                decimal finallat = latt / 60;
                                Console.WriteLine("Lattitude:" + finallat);
                            
                                //Longitude
                                var longtake = myArray.Skip(16).Take(4).ToArray();
                                string longstring = BitConverter.ToString(longtake).Replace("-", "");
                                decimal longd = (decimal)Int64.Parse(longstring, System.Globalization.NumberStyles.HexNumber);
                                decimal longt = longd / 30000;
                                decimal finallong = longt / 60;
                                 Console.WriteLine("Longitude:" + finallong);
                                //speed

                                var speedtake = myArray.Skip(20).Take(1).ToArray();
                                string speedstring = BitConverter.ToString(speedtake).Replace("-", "");                                decimal speedd = (decimal)Int64.Parse(speedstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Speed:" + speedd);

                                var courcetake = myArray.Skip(21).Take(2).ToArray();
                                string courcestring = BitConverter.ToString(courcetake).Replace("-", "");
                                decimal course = (decimal)Int64.Parse(courcestring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("CourseStetus:" + course);

                                //MCC
                                var mcctake = myArray.Skip(23).Take(2).ToArray();
                                string mccstring = BitConverter.ToString(mcctake).Replace("-", "");
                                decimal mcc = (decimal)Int64.Parse(mccstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Moblie Country Code:" + mcc);

                                //MNC
                                var mnctake = myArray.Skip(25).Take(1).ToArray();
                                string mncstring = BitConverter.ToString(mnctake).Replace("-", "");
                                decimal mnc = (decimal)Int64.Parse(mncstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Mobile Network Code:" + mnc);

                                //LAC
                                var lactake = myArray.Skip(26).Take(2).ToArray();
                                string lacstring = BitConverter.ToString(lactake).Replace("-", "");
                                decimal lac = (decimal)Int64.Parse(lacstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Local Area Code:" + lac);

                                //CellerID
                                var celleridtake = myArray.Skip(28).Take(3).ToArray();
                                string clleridstring = BitConverter.ToString(celleridtake).Replace("-", "");
                                decimal cellerid = (decimal)Int64.Parse(clleridstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Celler ID:" + cellerid);
                               Logger.Log("Saving Data Into Database......");
                                try
                                {
                                    telemetry t = new telemetry();
                                    t.id = maxid();
                                    t.imei = Convert.ToInt64(imei);
                                    t.speed = (short)speedd;
                                    t.satellites = (short)satd;
                                    t.latitude = (double)finallat;
                                    t.longitude = (double)finallong;
                                    t.mobile_country_code = (int)mcc;
                                    t.mobile_network_code = (short)mnc;
                                    t.local_area_code = (int)lac;
                                    t.cell_identifier = (int)cellerid;
                                    // t.time = converteddate;
                                    t.time = DateTime.Now;
                                    t.digital_inputs = Convert.ToInt32( nowignation);
                                    t.hdop = 1;
                                    db.telemetries.InsertOnSubmit(t);
                                    db.SubmitChanges();
                                   Logger.Log("Successfully Saved To Database......");
                                }
                                catch (Exception)
                                {

                                  
                                }


                            }
                            else if (myArray.Length == 36)
                            {Console.Write("Actual Data:" + BitConverter.ToString(myArray));
                                var imei = Helper.imei;
                                Console.WriteLine("IMEI:" + imei);
                                //Console.WriteLine("IMEI: " + BitConverter.ToString(selected));
                                var datetake = myArray.Skip(4).Take(6).ToArray();
                                //05 - 07 - 2018 2:24:24 AM
                                string date1 = myArray[6].ToString() + "-" + myArray[5].ToString() + "-" + myArray[4].ToString() + " " + myArray[7].ToString() + ":" + myArray[8].ToString() + ":" + myArray[9].ToString();

                                var converteddate = Convert.ToDateTime(date1.ToString());
                                Console.WriteLine("Date:" + Convert.ToDateTime(date1.ToString()));

                               


                                var sattake = myArray.Skip(10).Take(1).ToArray();
                                string satstring = BitConverter.ToString(sattake).Replace("-", "");
                              
                                string sat = satstring[satstring.Length - 1].ToString();
                                decimal satd = (decimal)Int64.Parse(sat, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Sattalite Count:" + satd.ToString());

                                var lattake = myArray.Skip(11).Take(4).ToArray();
                                string latstring = BitConverter.ToString(lattake).Replace("-", "");
                                decimal latd = (decimal)Int64.Parse(latstring, System.Globalization.NumberStyles.HexNumber);
                                decimal latt = latd / 30000;
                                decimal finallat = latt / 60;
                                Console.WriteLine("Lattitude:" + finallat);

                                var longtake = myArray.Skip(15).Take(4).ToArray();
                                string longstring = BitConverter.ToString(longtake).Replace("-", "");
                                decimal longd = (decimal)Int64.Parse(longstring, System.Globalization.NumberStyles.HexNumber);
                                  decimal longt = longd / 30000;
                                decimal finallong = longt / 60;
                                Console.WriteLine("Longitude:" + finallong);

                                var speedtake = myArray.Skip(19).Take(1).ToArray();
                                string speedstring = BitConverter.ToString(speedtake).Replace("-", "");
                                 decimal speedd = (decimal)Int64.Parse(speedstring, System.Globalization.NumberStyles.HexNumber);
                              Console.WriteLine("Speed:" +speedd);
                              //  var ll =latLng(bytes.slice(0, 8));


                                var courcetake = myArray.Skip(20).Take(2).ToArray();
                                string courcestring = BitConverter.ToString(courcetake).Replace("-", "");
                                decimal course = (decimal)Int64.Parse(courcestring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("CourseStetus:" + course);

                                var mcctake = myArray.Skip(22).Take(2).ToArray();
                                string mccstring = BitConverter.ToString(mcctake).Replace("-", "");
                                decimal mcc = (decimal)Int64.Parse(mccstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Moblie Country Code:" + mcc);


                                var mnctake = myArray.Skip(24).Take(1).ToArray();
                                string mncstring = BitConverter.ToString(mnctake).Replace("-", "");
                                decimal mnc = (decimal)Int64.Parse(mncstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Mobile Network Code:" + mnc);

                                var lactake = myArray.Skip(25).Take(2).ToArray();
                                string lacstring = BitConverter.ToString(lactake).Replace("-", "");
                                decimal lac = (decimal)Int64.Parse(lacstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Local Area Code:" + lac);


                                var celleridtake = myArray.Skip(27).Take(3).ToArray();
                                string clleridstring = BitConverter.ToString(celleridtake).Replace("-", "");
                                decimal cellerid = (decimal)Int64.Parse(clleridstring, System.Globalization.NumberStyles.HexNumber);
                                Console.WriteLine("Celler ID:" + cellerid);

                                Logger.Log("Saving Data Into Database......");
                                try
                                {
                                    telemetry t = new telemetry();
                                    t.id = maxid();
                                    t.imei = Convert.ToInt64(imei);
                                    t.speed = (short)speedd;
                                    t.satellites = (short)satd;
                                    t.latitude = (double)finallat;
                                    t.longitude = (double)finallong;
                                    t.mobile_country_code = (int)mcc;
                                    t.mobile_network_code = (short)mnc;
                                    t.local_area_code = (int)lac;
                                    t.cell_identifier = (int)cellerid;
                                    // t.time = converteddate;
                                    t.time = DateTime.Now;
                                    t.digital_inputs = Convert.ToInt32(nowignation);
                                    t.hdop = 1;
                                    db.telemetries.InsertOnSubmit(t);
                                    db.SubmitChanges();
                                    Logger.Log("Successfully Saved To Database......");
                                }
                                catch (Exception)
                                {

                                  
                                }

                            }
                            else if(myArray.Length==18)
                            {
                                Console.WriteLine("IMEI: " + BitConverter.ToString(myArray));
                                Helper.imei = BitConverter.ToString(myArray).Replace("-", "");
                              
                            }
                           
                            myArray = null;
                        }
                      
                    }


                }
                else
                    Logger.Log("Packet too short.");

                try
                {
                    // Listening for more packets
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
                catch
                {
                    Logger.Log("Connection closed.");
                }
            }
        }
        
        public int maxid()
        {
            try
            {
                int id = (from a in db.telemetries select a.id).Max();
                return id+1;
            }
            catch (Exception)
            {

                return 1;
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

        /// <summary>
        /// Complete response sending to the socket
        /// </summary>
        /// <param name="ar">Asynchronous operation state</param>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                handler.EndSend(ar);
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
            }
        }
    }
}