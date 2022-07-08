using Newtonsoft.Json;
using System;
using System.Threading;
using System.Timers;

namespace SimAlpha
{
    internal class TSData
    {
        public static string JSON;
        public static string ITEM = "";

        internal static async void SendData(Object source, ElapsedEventArgs e)
        {
            JSON = JsonConvert.SerializeObject(new
            {
                DeviceId = DataGen.UUID,
                Timestamp = DataGen.TIME,
                data = new
                {
                    serendipity = DataGen.SERENDIPITY,
                    state = DataGen.STATE,
                    stepsWalked = DataGen.STEPS,
                    heartbeat = DataGen.HEARTBEAT,
                    numberOfFalls = DataGen.NFALL,
                    battery = DataGen.BATTERY,
                    standings = DataGen.STANDING,
                    latitude = DataGen.GPS[0],
                    longitude = DataGen.GPS[1]
                }
            });
            Console.WriteLine(JSON);
            CacheData.SaveData();
            try
            {
                foreach (var item in CacheData.CACHE)
                {
                    await APIData.SendAPIAsync(item.Value.ToString());
                    CacheData.CACHE.Remove(item.Key);
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
    }
}