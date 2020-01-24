using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace HttpServerDemo
{
    class Program
    {
        public async static Task Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 1234);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                await ProcessClientAsync(tcpClient);

            }
        }

        public static async Task ProcessClientAsync(TcpClient tcpClient)
        {
            const string NewLine = "\r\n";
            using (var networkStream = tcpClient.GetStream())
            {
                byte[] requestBytes = new byte[1000000];   //TODO USE Buffer
                int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                string request = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
                string responseText = @"<h1>Working...</h1>" +
                                          $"<form> <h1> Time is: {System.DateTime.Now} </h1> </form>";
                string response = "HTTP/1.1 200 OK" + NewLine
                                 + "Server: SoftuniServer/1.0 " + NewLine
                                 + "Content-Type: text/html" + NewLine
                                 + "Content-Length: " + responseText.Length + NewLine + NewLine
                                 + responseText;

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                Console.WriteLine(request);
                Console.WriteLine(new string('=', 60));
            }
        }
    }
}
