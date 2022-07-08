using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SimAlpha
{
    internal class APIData
    {
        static HttpClient client;
        public static async Task SendAPIAsync(string itemcache)
        {
            client = new() { BaseAddress = new Uri("https://0v0ark84yj.execute-api.eu-west-1.amazonaws.com") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthUser.TOKEN);
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Data")
            { Content = new StringContent(itemcache, Encoding.UTF8, "application/json") };
            HttpResponseMessage response = await client.SendAsync(request);
            Console.WriteLine(response.StatusCode.ToString());
            if (response.StatusCode == HttpStatusCode.Unauthorized) { AuthUser.AUTENTICATION = false; await AuthUser.AuthToken(); }
            client.Dispose();
        }
    }
}
