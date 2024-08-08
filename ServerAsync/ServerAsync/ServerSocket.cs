using System;
using System.Net;
using System.Net.Sockets;

namespace ServerAsync
{
    internal class ServerSocket
    {
        private Socket socket;
        private Dictionary<int, ClientSocket> clientDic = new Dictionary<int, ClientSocket>();

        public void Start(string ip, int port, int num)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip),port);
            try
            {
                socket.Bind(ipPoint);
                socket.Listen(num);

                socket.BeginAccept(AcceptCallBack, null);
            }
            catch(Exception e)
            {
                throw;
            }
        }

        private void AcceptCallBack(IAsyncResult result)
        {
            try 
            {
                Socket clientSocket = socket.EndAccept(result);
                ClientSocket client = new ClientSocket(clientSocket);
                clientDic.Add(client.clientID, client);

                socket.BeginAccept(AcceptCallBack, null);

            }
            catch(Exception e) 
            {
                Console.WriteLine("CONNECT FAIL");
            }
        }

        public void BroadCast(string str)
        {
            foreach(var clientSocket in clientDic.Values)
            {
                clientSocket.Send(str);
            }
        }
    }
}
