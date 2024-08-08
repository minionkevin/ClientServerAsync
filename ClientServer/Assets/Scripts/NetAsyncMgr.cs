using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetAsyncMgr : MonoBehaviour
{
    private static NetAsyncMgr instance;
    public static NetAsyncMgr Instance => instance;
    
    // socket with server
    private Socket socket;
    private byte[] cacheBytes = new byte[1024*1024];
    private int cacheNum = 0;
    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Connect(string ip, int port)
    {
        if (socket != null && socket.Connected) return;
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.RemoteEndPoint = ipPoint;
        args.Completed += (socket, args) => {
            if (args.SocketError == SocketError.Success)
            {
                print("CONNECT SUCCESS");
                SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
                receiveArgs.SetBuffer(cacheBytes,0,cacheBytes.Length);
                receiveArgs.Completed += ReceiveCallBack;
                this.socket.ReceiveAsync(receiveArgs);
            }
            else print("CONNECT FAILED " + args.SocketError);
        };
        socket.ConnectAsync(args);
    }

    private void ReceiveCallBack(object obj, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            string output = Encoding.UTF8.GetString(args.Buffer, 0, args.BytesTransferred);
            print(output);
            args.SetBuffer(0,args.Buffer.Length);
            if (!this.socket.Connected || this.socket == null) return;
            socket.ReceiveAsync(args);
        }
        else
        {
            print("RECEIVE FAILED " + args.SocketError);
            Close();
        }
    }

    public void Send(string str)
    {
        if (!this.socket.Connected || this.socket == null) return;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.SetBuffer(bytes, 0 , bytes.Length);
        args.Completed += (socket, args) => {
            if (args.SocketError != SocketError.Success)
            {
                print("SEND FAILED " + args.SocketError);
                Close();
            }
        };
        this.socket.SendAsync(args);
    }

    private void Close()
    {
        if (socket != null)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            socket = null;
        }
    }
    
    
    
    
}
