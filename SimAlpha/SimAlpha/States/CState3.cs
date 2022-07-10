using System;
using System.Threading;

namespace SimAlpha.States
{
    internal class CState3
    {
        public static void State3()
        {
            DataGen.STANDING = 0;
            DataGen.RTIME = 0;
            Random random = new();
            int IRTime = random.Next(21600, 36001);
            if (DataGen.STANDING == 0) { DataGen.GTIME = DateTimeOffset.UtcNow; }
            for (int i = 1; i < IRTime; i++)
            {
                DataGen.TIME = DateTimeOffset.UtcNow;

                DataGen.STATE = "Sleeping";

                int IRHeartBeat = random.Next(0, 10);
                switch (IRHeartBeat)
                {
                    case 0: if (DataGen.HEARTBEAT > 42) { DataGen.HEARTBEAT -= 2; } else { DataGen.HEARTBEAT += 2; }; break;
                    case 1: if (DataGen.HEARTBEAT > 41) { DataGen.HEARTBEAT -= 1; } else { DataGen.HEARTBEAT += 1; }; break;
                    case 8: if (DataGen.HEARTBEAT < 49) { DataGen.HEARTBEAT += 1; } else { DataGen.HEARTBEAT -= 1; }; break;
                    case 9: if (DataGen.HEARTBEAT < 48) { DataGen.HEARTBEAT += 2; } else { DataGen.HEARTBEAT -= 2; }; break;
                    default: break;
                }

                int IRNFall = random.Next(0, 10000);
                if (IRNFall == 50) { DataGen.NFALL++; AllarmGen.SendAllarm(AllarmType.FALL); };

                if (DataGen.COUNTER == 900) { DataGen.BATTERY--; DataGen.COUNTER = 0; };
                if (DataGen.BATTERY == 10 && DataGen.BSEND == false) { AllarmGen.SendAllarm(AllarmType.LOW_BATTERY); DataGen.BSEND = true; };
                if (DataGen.BATTERY == 0) { DataGen.BATTERY = 100; DataGen.BSEND = false; };

                DataGen.SERENDIPITY = random.Next(1, 21);

                Thread.Sleep(1000);
                DataGen.COUNTER++;
                if (DateTimeOffset.UtcNow.Day != DataGen.TIME.Day) { DataGen.STEPS = 0; };
            }
        }
    }
}
