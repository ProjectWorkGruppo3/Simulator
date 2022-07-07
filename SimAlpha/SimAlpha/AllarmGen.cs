using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SimAlpha
{
    public enum AllarmType { HEARTBEAT, LOW_BATTERY, FALL }

    internal class AllarmGen
    {
        private static HttpRequestMessage REQUEST;
        static HttpClient client;
        private static string AJSON;

        internal static async void SendAllarm(AllarmType type)
        {
            client = new() { BaseAddress = new Uri("https://localhost:7013") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthUser.TOKEN);
            switch (type)
            {
                case AllarmType.HEARTBEAT:
                    AJSON = JsonConvert.SerializeObject(new { DeviceId = DataGen.UUID, Timestamp = DataGen.TIME, Type = "HEARTBEAT", HeartBeat = DataGen.HEARTBEAT });
                    Console.WriteLine("ALLARM HEARTBEAT");
                    break;
                case AllarmType.FALL:
                    AJSON = JsonConvert.SerializeObject(new { DeviceId = DataGen.UUID, Timestamp = DataGen.TIME, Type = "FALL" });
                    Console.WriteLine("ALLARM NFALL");
                    break;
                case AllarmType.LOW_BATTERY:
                    AJSON = JsonConvert.SerializeObject(new { DeviceId = DataGen.UUID, Timestamp = DataGen.TIME, Type = "LOW_BATTERY", BatteryCharge = DataGen.BATTERY });
                    Console.WriteLine("ALLARM BATTERY");
                    break;
            }
            REQUEST = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Alarms")
            { Content = new StringContent(AJSON, Encoding.UTF8, "application/json") };
            HttpResponseMessage response = await client.SendAsync(REQUEST);
            Console.WriteLine(response.StatusCode.ToString());
            if (response.StatusCode == HttpStatusCode.Unauthorized) { AuthUser.AUTENTICATION = false; await AuthUser.AuthToken(); }
            client.Dispose();
            //FIXME = CACHE ALLARMS
        }
    }
}