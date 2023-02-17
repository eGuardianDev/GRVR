using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NetCoreServer;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace Software.Classes.Controllers
{

    class MulticastServer : UdpServer
    {
        public MulticastServer(IPAddress address, int port) : base(address, port) { }

        protected override void OnError(SocketError error)
        {
            Logger.Error($"Multicast UDP server caught an error with code {error}");
        }
    }

    //TODO:
    // FIX IT
    public class API
    {
        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        private bool isOpen;

        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }


        public API(int port)
        {
            Port = port;
        }
        MulticastServer server;
        public void StopServer()
        {
            Logger.Log("Stopping api server");
            IsOpen = false;
            server.Stop();
        }
        public void StartServer()
        {
            if (IsOpen) return;
            string myIP = Dns.GetHostByName(Dns.GetHostName()).AddressList[1].ToString();
            //UDPServer

            Logger.Log($"UDP Server address: {myIP}:{Port}");
            server = new MulticastServer(IPAddress.Any, 0);

            Logger.Log("Server API...");
            server.Start(myIP, Port);

            IsOpen = true;

        }
        public void SendMessage(string data)
        {
            if (IsOpen)
            {
                server.Multicast(data);
            }

        }
    }
}
