using Amazon;
using Amazon.TimestreamWrite;
using Amazon.TimestreamWrite.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SimAlpha
{
    internal class TSData
    {
        private static AmazonTimestreamWriteClient WC;
        public static string JSON;
        public static string ITEM = "";

        internal static async void SendData(Object source, ElapsedEventArgs e)
        {
            JSON = JsonConvert.SerializeObject(new { uuid = DataGen.UUID, data = new { /*time = DataGen.TIME, */serendipity = DataGen.SERENDIPITY, state = DataGen.STATE, steps = DataGen.STEPS, heartbeat = DataGen.HEARTBEAT, nFall = DataGen.NFALL, battery = DataGen.BATTERY, standing = DataGen.STANDING, latitude = DataGen.GPS[0], longitude = DataGen.GPS[1] } });
            Console.WriteLine(JSON);
            CacheData.SaveData();
            AWSConfiguration();
            try
            {
                foreach (var item in CacheData.CACHE)
                {
                    
                    await AWSWrite(item.Value.ToString());
                    CacheData.CACHE.Remove(item.Key);
                    Thread.Sleep(1000);
                }
            }
            catch{ Console.WriteLine("Error"); }
        }

        private static void AWSConfiguration()
        {
            var CREDENTIALS = new Amazon.Runtime.BasicAWSCredentials(Program.AWSID, Program.AWSKEY);
            var writeClientConfig = new AmazonTimestreamWriteConfig
            {
                RegionEndpoint = RegionEndpoint.EUWest1,
                Timeout = TimeSpan.FromSeconds(20),
                MaxErrorRetry = 0,
            };
            WC = new AmazonTimestreamWriteClient(CREDENTIALS, writeClientConfig);
        }

        private static async Task AWSWrite(string data)
        {
            string currentTimeString = (DataGen.TIME.ToUnixTimeMilliseconds()).ToString();
            List<Dimension> dimensions = new()
                { new Dimension { Name = "data", Value = "bracelet" }, };
            var info = new Record
            {
                Dimensions = dimensions,
                MeasureName = "info",
                MeasureValue = data,
                MeasureValueType = MeasureValueType.VARCHAR,
                Time = currentTimeString
            };
            List<Record> records = new() { info };

            var writeRecordsRequest = new WriteRecordsRequest
            {
                DatabaseName = "clod2021_ProjectWork_G3",
                TableName = "SIMTest2",
                Records = records
            };
            WriteRecordsResponse response = await WC.WriteRecordsAsync(writeRecordsRequest);
            Console.WriteLine($"Write records status code: {response.HttpStatusCode}");
        }
    }
}