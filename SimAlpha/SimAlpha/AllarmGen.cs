using System;

namespace SimAlpha
{
    internal class AllarmGen
    {
        internal static void SendAllarm(int AType)
        {
            switch (AType)
            {
                case 0:
                    Console.WriteLine("ALLARM HEARTBEAT");
                    break;
                case 1:
                    Console.WriteLine("ALLARM NFALL");
                    break;
                case 2:
                    Console.WriteLine("ALLARM BATTERY");
                    break;
            }
        }
    }
}