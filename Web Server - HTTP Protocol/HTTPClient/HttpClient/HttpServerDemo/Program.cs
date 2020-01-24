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
            //byte[] imageAsBytes = File.ReadAllBytes("picture.jpg");


            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() =>  ProcessClientAsync(tcpClient));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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
                // await networkStream.WriteAsync(imageAsBytes, 0, imageAsBytes.Length);
                Console.WriteLine(request);
                Console.WriteLine(new string('=', 60));
            }
        }
    }
}
