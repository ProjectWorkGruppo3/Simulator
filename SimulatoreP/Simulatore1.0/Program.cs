using Amazon;
using Amazon.TimestreamWrite;
using Amazon.TimestreamWrite.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Simulatore1._0
{
    internal class Program
    {
        private static readonly string UUID = "949fad93-3541-4b57-bf20-cefa2a782ef5";
        private static AmazonTimestreamWriteClient writeClient1;
        private static DateTimeOffset TIME;
        private static int HBS;
        private static int BATTERY;
        private static int STEPS;
        private static int NFALL;
        private static int STANDING;
        private static int SERENDIPITY;

        static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var config = builder.Build();
            var NPGSQL = config["Secrets:NPGSQL"];
            var AWSCREDENTIALSID = config["Secrets:AWSCREDENTIALSID"];
            var AWSCREDENTIALSKEY = config["Secrets:AWSCREDENTIALSKEY"];
            await AWSConfiguration(AWSCREDENTIALSID, AWSCREDENTIALSKEY);
            AssetCheck(NPGSQL);
            await AssetDataAsync();
        }

        private static void AssetCheck(string NPGSQL)
        {
            try
            {
                using var conn = new NpgsqlConnection(NPGSQL);
                conn.Open();
                using var command = new NpgsqlCommand("SELECT * FROM asset", conn);
                var reader = command.ExecuteReader();
                bool check = false;
                while (reader.Read())
                {
                    string uuids = string.Format(reader.GetString(0));
                    if (uuids == UUID) { check = true; return; }
                }
                reader.Close();
                conn.Close();
                if (check == false)
                {
                    Console.WriteLine($"UUID NON TROVATO -> {UUID}");
                    Thread.Sleep(5000);
                    AssetCheck(NPGSQL);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private static async Task AssetDataAsync()
        {
            Random rnd1 = new();
            HBS = rnd1.Next(60, 101);
            Random rnd2 = new();
            BATTERY = rnd2.Next(1, 101);
            Random rnd3 = new();
            STEPS = rnd3.Next(0, 10000);
            Random rnd4 = new();
            NFALL = rnd4.Next(0, 5);
            Random rnd5 = new();
            STANDING = rnd5.Next(0, 25);
            Random rnd6 = new();
            SERENDIPITY = rnd6.Next(0, 101);
            while (true)
            {
                TIME = DateTimeOffset.UtcNow;
                Random rnd7 = new();
                int stateHBS = rnd7.Next(0, 10);
                if (stateHBS == 0) { if (HBS > 50) { --HBS; } else { ++HBS; } }
                if (stateHBS == 1) { if (HBS < 220) { ++HBS; } else { --HBS; } }
                Random rnd8 = new();
                int stateBATTERY = rnd8.Next(0, 10);
                if (stateBATTERY == 0) { --BATTERY; }
                if (BATTERY == 0) { Console.WriteLine("La Batteria è scarica!!!"); Environment.Exit(0); }
                Random rnd9 = new();
                int stateSTEPS = rnd9.Next(0, 3);
                if (stateSTEPS != 0) { ++STEPS; }
                Random rnd10 = new();
                int stateNFALL = rnd10.Next(0, 50);
                if (stateNFALL == 0) { ++NFALL; }
                Random rnd11 = new();
                int stateSTANDING = rnd11.Next(0, 10);
                if (stateSTANDING == 0) { ++STANDING; }
                Random rnd12 = new();
                int stateSERENDIPITY = rnd12.Next(0, 10);
                if (stateSERENDIPITY == 0) { if (SERENDIPITY > 0) { --SERENDIPITY; } else { ++SERENDIPITY; } }
                if (stateSERENDIPITY == 9) { if (SERENDIPITY < 100) { ++SERENDIPITY; } else { --SERENDIPITY; } }
                Console.WriteLine($"ID = {UUID} | ORA = {TIME} | SERENDIPITY = {SERENDIPITY} | PASSI = {STEPS} | BATTITO = {HBS} | CADUTE = {NFALL} | BATTERIA = {BATTERY} | TEMPO = {STANDING}");
                await WriteRecords();
                Thread.Sleep(1000);
            }
        }

        public static Task AWSConfiguration(string AWSCREDENTIALSID, string AWSCREDENTIALSKEY)
        {
            var CREDENTIALS = new Amazon.Runtime.BasicAWSCredentials(AWSCREDENTIALSID, AWSCREDENTIALSKEY);
            var writeClientConfig = new AmazonTimestreamWriteConfig
            {
                RegionEndpoint = RegionEndpoint.EUWest1,
                Timeout = TimeSpan.FromSeconds(20),
                MaxErrorRetry = 10,
            };
            writeClient1 = new AmazonTimestreamWriteClient(CREDENTIALS, writeClientConfig);
            return Task.CompletedTask;
        }

        private static async Task WriteRecords()
        {
            string currentTimeString = (TIME.ToUnixTimeMilliseconds()).ToString();
            List<Dimension> dimensions = new()
            { new Dimension { Name = "data", Value = "bracelet" }, };
            var json = JsonConvert.SerializeObject(new { uid = UUID, data = new { serendipity = SERENDIPITY, steps = STEPS, heartbeat = HBS, nFall = NFALL, battery = BATTERY, standing = STANDING } });
            var info = new Record
            {
                Dimensions = dimensions,
                MeasureName = "info",
                MeasureValue = json,
                MeasureValueType = MeasureValueType.VARCHAR,
                Time = currentTimeString
            };
            List<Record> records = new() { info };
            try
            {
                var writeRecordsRequest = new WriteRecordsRequest
                {
                    DatabaseName = "clod2021_ProjectWork_G3",
                    TableName = "SIMTest",
                    Records = records
                };
                WriteRecordsResponse response = await writeClient1.WriteRecordsAsync(writeRecordsRequest);
                Console.WriteLine($"Write records status code: {response.HttpStatusCode}");
            }
            catch (Exception e) { Console.WriteLine("Write records failure:" + e.ToString()); }
        }
    }
}