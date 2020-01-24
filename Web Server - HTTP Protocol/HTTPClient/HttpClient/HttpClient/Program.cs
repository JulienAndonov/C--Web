using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace HttpClientDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage httpResponse = await httpclient.GetAsync("http://localhost:1234");
            var receivedResponse = await httpResponse.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync("receivedPicture.jpg",receivedResponse);
        }
    }
}
