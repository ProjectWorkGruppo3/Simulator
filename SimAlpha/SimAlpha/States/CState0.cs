using System;
using System.Threading;

namespace SimAlpha.States
{
    internal class CState0
    {
        public static void State0()
        {
            Random random = new();
            int IRTime = random.Next(1, 25); //14401
            if (DataGen.STANDING == 0)
            { DataGen.GTIME = DateTimeOffset.UtcNow; }
            for (int i = 1; i < IRTime; i++)
            {
                DataGen.TIME = DateTimeOffset.UtcNow;

                DataGen.STATE = "SIT";

                int IRSteps = random.Next(0, 100);
                if (IRSteps == 1) { DataGen.STEPS++; };

                int IRHeartBeat = random.Next(0, 10);
                switch (IRHeartBeat)
                {
                    case 0: if (DataGen.HEARTBEAT > 62) { DataGen.HEARTBEAT -= 2; } else { DataGen.HEARTBEAT += 2; }; break;
                    case 1: if (DataGen.HEARTBEAT > 61) { DataGen.HEARTBEAT -= 1; } else { DataGen.HEARTBEAT += 1; }; break;
                    case 8: if (DataGen.HEARTBEAT < 99) { DataGen.HEARTBEAT += 1; } else { DataGen.HEARTBEAT -= 1; }; break;
                    case 9: if (DataGen.HEARTBEAT < 98) { DataGen.HEARTBEAT += 2; } else { DataGen.HEARTBEAT -= 2; }; break;
                    default: break;
                }

                int IRNFall = random.Next(0, 1000);
                if (IRNFall == 50) { DataGen.NFALL++; AllarmGen.SendAllarm(AllarmType.FALL); };

                if (DataGen.COUNTER == 900) { DataGen.BATTERY--; DataGen.COUNTER = 0; };
                if (DataGen.BATTERY == 10) { AllarmGen.SendAllarm(AllarmType.LOW_BATTERY); };
                if (DataGen.BATTERY == 0) { DataGen.BATTERY = 100; };

                DataGen.TSPAN = DataGen.TIME - DataGen.GTIME;
                DataGen.STANDING = Convert.ToInt32(DataGen.TSPAN.TotalSeconds);
                DataGen.STANDING += DataGen.RTIME;

                DataGen.SERENDIPITY = random.Next(1, 21);
                if (DataGen.NFALL >= 1) { DataGen.SERENDIPITY += random.Next(1, 21); }

                Thread.Sleep(1000);
                DataGen.COUNTER++;
                if (DateTimeOffset.UtcNow.Day != DataGen.TIME.Day) { DataGen.STEPS = 0; };
            }
        }
    }
}
