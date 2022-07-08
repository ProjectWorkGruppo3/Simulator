using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SimAlpha
{
    internal class Program
    {
        public static string AWSID;
        public static string AWSKEY;
        public static string ARGS;

        static async Task Main(string[] args)
        {
            ARGS = args[0].Trim();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var config = builder.Build();
            AWSID = config["Secrets:AWSCREDENTIALSID"];
            AWSKEY = config["Secrets:AWSCREDENTIALSKEY"];
            await AuthUser.AuthToken();
            DataGen.Data();
        }
    }
}
