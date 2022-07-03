using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SimAlpha
{
    internal class AuthUser
    {
        static HttpClient client;
        private static bool AUTENTICATION;
        public static string TOKEN;

        internal static async Task AuthToken()
        {
            while (AUTENTICATION == false)
            {
                Console.Write("Username -> ");
                string username = Console.ReadLine();
                Console.Write("Password -> ");
                string password = Console.ReadLine();
                Console.WriteLine("-----");
                object data = new { Email = username, Password = password };
                client = new() { BaseAddress = new Uri("https://2hfbqexqd4r3vruuahvdgcaqhu0vvhmf.lambda-url.eu-west-1.on.aws") };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/users/login", data);
                client.Dispose();
                if (response.StatusCode == HttpStatusCode.OK)
                { 
                    AUTENTICATION = true;
                    string rsp = await response.Content.ReadAsStringAsync();
                    string[] rspsp = rsp.Split('\"');
                    TOKEN = rspsp[3];
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized) { Console.WriteLine("Credenziali Errate"); }
                else { Console.WriteLine("Errore Di Connessione"); }
            }
        }
    }
}