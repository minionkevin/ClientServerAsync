// See https://aka.ms/new-console-template for more information
using ServerAsync;

Console.WriteLine("Start server");
ServerSocket serverSocket = new ServerSocket();
serverSocket.Start("127.0.0.1", 8080, 1024);


while(true)
{
    string input = Console.ReadLine();
    if(input.Substring(0,2) == "B:")
    {
        serverSocket.BroadCast(input.Substring(2));
        Console.WriteLine("BOARDCAST TO CLIENT");
    }
}
