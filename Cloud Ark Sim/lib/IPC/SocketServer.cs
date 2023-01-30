using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Cloud_Ark_Sim.lib.IPC
{
    class SocketServer
    {
        public static WebSocketServer wssv;
        public static void Start()
        {
            wssv = new("ws://localhost:1234");

            wssv.AddWebSocketService<SendData>("/Data");
            wssv.Start();
        }
    }

    public class SendData : WebSocketBehavior
    {
        protected override void OnMessage (MessageEventArgs e)
        {
            
        }

        public void Send(String data)
        {
            Sessions.Broadcast(data);
        }
    }
}
