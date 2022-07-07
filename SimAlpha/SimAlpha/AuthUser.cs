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
        public static bool AUTENTICATION;
        public static string TOKEN;
        private static int COUNTER = 0;
        private static object CREDENTIALS;

        internal static async Task AuthToken()
        {
            while (AUTENTICATION == false)
            {
                if (COUNTER == 0)
                {
                    Console.Write("Username -> ");
                    string username = Console.ReadLine();
                    Console.Write("Password -> ");
                    string password = Console.ReadLine();
                    Console.WriteLine("-----");
                    CREDENTIALS = new { Email = username, Password = password };
                }
                client = new() { BaseAddress = new Uri("https://localhost:7013") };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/users/login", CREDENTIALS);
                client.Dispose();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    AUTENTICATION = true;
                    string rsp = await response.Content.ReadAsStringAsync();
                    string[] rspsp = rsp.Split('\"');
                    TOKEN = rspsp[3];
                    COUNTER++;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized) { Console.WriteLine("Credenziali Errate"); COUNTER = 0; }
                else { Console.WriteLine("Errore Di Connessione"); COUNTER = 0; }
            }
        }
    }
}