using System;
using System.Net.Sockets;
using System.Text;

namespace ServerAsync
{
    internal class ClientSocket
    {
        public Socket socket;
        public int clientID;
        private static int CLIENT_BEGIN_ID = 1;

        private byte[] cacheBytes = new byte[1024];
        private int cacheNum = 0;

        public ClientSocket(Socket socket)
        {
            this.clientID = CLIENT_BEGIN_ID++;
            this.socket = socket;

            // Receive message
            this.socket.BeginReceive(cacheBytes, cacheNum, cacheBytes.Length, SocketFlags.None, ReceiveCalllBack, this.socket);
        }

        private void ReceiveCalllBack(IAsyncResult result)
        {
            try 
            {
                cacheNum = this.socket.EndReceive(result);
                Console.WriteLine(Encoding.UTF8.GetString(cacheBytes, 0, cacheNum));
                cacheNum = 0;
                if(this.socket.Connected) this.socket.BeginReceive(cacheBytes, cacheNum, cacheBytes.Length, SocketFlags.None, ReceiveCalllBack, this.socket);

            }
            catch (SocketException e) 
            {
                Console.WriteLine("RECEIVE FAIL: " + e.SocketErrorCode + " " + e.Message);
            }
        }

        public void Send(string str)
        {
            if(this.socket.Connected)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                this.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCalllBack, this.socket);
            }
            else
            {

            }
        }

        private void SendCalllBack(IAsyncResult result)
        {
            try 
            {
                this.socket.EndSend(result);    
            }
            catch(SocketException e) 
            {
                Console.WriteLine("SEND FAIL");
            }

        }
    }
}
