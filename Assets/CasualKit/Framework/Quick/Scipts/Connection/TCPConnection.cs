using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


namespace CasualKit.Quick.Connection.TCP
{

    public class TCPConnection : IConnection
    {
        public bool IsConnected => TCP != null ? TCP.Connected && TCP.DualMode : false;

        public Uri Address { get; set; }

        public Queue<string> MessageQ { get; set; }

        Socket TCP = null;

        public event Action OnConnecting;
        public event Action OnConnected;
        //public event Action<string> OnReceieved;
        public event Action<string> OnError;
        public event Action<CloseStatus> OnClosing;
        public event Action<CloseStatus> OnClosed;

        public TCPConnection(string url)
        {
            Address = new Uri(url);
            TCP = new Socket(SocketType.Stream, ProtocolType.Tcp);
            MessageQ = new Queue<string>();
            //TCP.ReceiveTimeout = 1;
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
            int bufferLen = CKSettings.Quick.RecieveBufferLength;
            int bytesReceived;
            byte[] buffer = new byte[bufferLen];
            try
            {
                while (true)
                {
                    bytesReceived = await TCP.ReceiveAsync(buffer, SocketFlags.None);
                    if (bytesReceived > 0)
                    {
                        Debug.Log("recv: " + Encoding.ASCII.GetString(buffer, 0, bytesReceived));
                        if (bytesReceived < bufferLen)
                            MessageQ.Enqueue(Encoding.ASCII.GetString(buffer, 0, bytesReceived));
                        else
                            throw new Exception("BUFFER FULL!!");
                    }
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
                Debug.Log("send: " + data);
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
                    OnClosed?.Invoke(status);
                }
            }
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