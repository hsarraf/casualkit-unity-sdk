using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace CasualKit.Quick.Connection.WS
{

    public class WSConnection : IConnection
    {
        public event Action OnConnecting;
        public event Action OnConnected;
        //public event Action<string> OnReceieved;
        public event Action<string> OnError;
        public event Action<CloseStatus> OnClosing;
        public event Action<CloseStatus> OnClosed;

        public Uri Address { get; set; }
        ClientWebSocket WS = null;

        public Queue<string> MessageQ { get; set; }

        public bool IsConnected => WS != null ? WS.State == WebSocketState.Open : false;

        public WSConnection(string url)
        {
            WS = new ClientWebSocket();
            Address = new Uri(url);
            MessageQ = new Queue<string>();
        }

        public async void Connect()
        {
            OnConnecting?.Invoke();
            try
            {
                await WS.ConnectAsync(Address, CancellationToken.None);
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
            byte[] buffer = new byte[bufferLen];
            WebSocketReceiveResult recieved = null;
            try
            {
                while (WS.State == WebSocketState.Open)
                {
                    recieved = await WS.ReceiveAsync(buffer, CancellationToken.None);
                    if (recieved.Count > 0)
                    {
                        Debug.Log("recv: " + Encoding.ASCII.GetString(buffer, 0, recieved.Count));
                        if (recieved.Count < bufferLen)
                            MessageQ.Enqueue(Encoding.ASCII.GetString(buffer, 0, recieved.Count));
                        else
                            throw new Exception("BUFFER FULL!!");
                    }
                    else
                        break;
                }
            }
            catch(Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        public void Send(string data)
        {
            if (WS != null)
            {
                Debug.Log("send: " + data);
                WS.SendAsync(new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(data)),
                    WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async void Disconnect(CloseStatus status = CloseStatus.NormalClosure, Action onDone = null)
        {
            if (WS != null && WS.State == WebSocketState.Open)
            {
                OnClosing?.Invoke(status);
                await WS.CloseAsync((WebSocketCloseStatus)status, null, CancellationToken.None);
                onDone?.Invoke();
                OnClosed?.Invoke(status);
            }
            Dispose();
        }

        public void Dispose()
        {
            if (WS != null)
            {
                WS.Dispose();
                WS = null;
            }
        }

    }

}