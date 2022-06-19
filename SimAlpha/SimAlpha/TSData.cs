using System;
using System.Timers;

namespace SimAlpha
{
    internal class TSData
    {
        internal static void SendData(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"UUID = {DataGen.UUID} | TIME = {DataGen.TIME} | SERENDIPITY = {DataGen.SERENDIPITY} | STATE = {DataGen.STATE} | STEPS = {DataGen.STEPS} | HEARTBEAT {DataGen.HEARTBEAT} | NFALL {DataGen.NFALL} | BATTERY {DataGen.BATTERY} | STANDING {DataGen.STANDING}");
            bool connection = false;
            if (connection == false) { CacheData.SaveData(); }
        }
    }
}