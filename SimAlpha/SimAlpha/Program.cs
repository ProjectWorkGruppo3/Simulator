using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace SimAlpha
{
    internal class Program
    {
        public static string AWSID;
        public static string AWSKEY;

        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var config = builder.Build();
            AWSID = config["Secrets:AWSCREDENTIALSID"];
            AWSKEY = config["Secrets:AWSCREDENTIALSKEY"];
            if (AuthUser.AuthToken() == true) { DataGen.Data(); }
            else { Console.WriteLine("Utente non trovato"); Main(); }
        }
    }
}
