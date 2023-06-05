using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


namespace CasualKit.Quick.Connection.TCP
{

    public class TCPv2Connection : IConnection
    {
        public bool IsConnected => TCP != null ? TCP.DualMode : false;

        public Uri Address { get; set; }

        public Queue<string> MessageQ { get; set; }

        Socket TCP = null;

        public event Action OnConnecting;
        public event Action OnConnected;
        //public event Action<string> OnReceieved;
        public event Action<string> OnError;
        public event Action<CloseStatus> OnClosing;
        public event Action<CloseStatus> OnClosed;


        public class StateObject
        {
            // Client socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 256;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }


        // The port number for the remote device.  
        private const int port = 11000;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        // The response from the remote device.  
        private static String response = String.Empty;


        public TCPv2Connection(string url)
        {
            Address = new Uri(url);
            TCP = new Socket(SocketType.Stream, ProtocolType.Tcp);
            MessageQ = new Queue<string>();
            //TCP.ReceiveTimeout = 1;
        }


        private void StartClient()
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the
                // remote device is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                TCP = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                TCP.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), TCP);
                connectDone.WaitOne();

                // Send test data to the remote device.  
                //string data = "<root><text_req1 /></root>";

                //Send(client, data);
                //sendDone.WaitOne();

                //// Receive the response from the remote device.  
                //Receive(client);
                //receiveDone.WaitOne();

                // Write the response to the console.  
                //Console.WriteLine("Response received : {0}", response);

                // Release the socket.  

                //client.Shutdown(SocketShutdown.Both);
                //client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }












        public async void Connect()
        {
            OnConnecting?.Invoke();
            try
            {
                await TCP.ConnectAsync(Address.Host, Address.Port);
                OnConnected?.Invoke();
                StartReceiveLoop();
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        public async void StartReceiveLoop()
        {
            byte[] buffer = new byte[CKSettings.Quick.RecieveBufferLength];
            try
            {
                while (true)
                {
                    int bytesReceived;
                    bytesReceived = await TCP.ReceiveAsync(buffer, SocketFlags.None);
                    Debug.Log(bytesReceived);
                    if (bytesReceived > 0)
                        MessageQ.Enqueue(Encoding.ASCII.GetString(buffer, 0, bytesReceived));
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        public async void Send(string data)
        {
            if (TCP != null && TCP.Connected)
            {
                byte[] requestBytes = Encoding.ASCII.GetBytes(data);
                int bytesSent = 0;
                while (bytesSent < requestBytes.Length)
                    bytesSent += await TCP.SendAsync(requestBytes.AsMemory(bytesSent), SocketFlags.None);
            }
        }

        public void Disconnect(CloseStatus status = CloseStatus.NormalClosure, Action onDone = null)
        {
            if (TCP != null && TCP.Connected)
            {
                try
                {
                    OnClosing?.Invoke(status);
                    TCP.Shutdown(SocketShutdown.Both);
                }
                finally
                {
                    TCP.Close();
                    onDone?.Invoke();
                }
            }
            OnClosed?.Invoke(status);
            Dispose();
        }

        public void Dispose()
        {
            if (TCP != null)
            {
                TCP.Dispose();
                TCP = null;
            }
        }

    }

}