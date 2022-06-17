using System;
using System.Threading;

namespace SimAlpha.States
{
    internal class CState1
    {
        public static void State1()
        {
            Random RTime = new();
            int IRTime = RTime.Next(1, 25);   //7201
            if (DataGen.STANDING == 0) { DataGen.GTIME = DateTimeOffset.UtcNow; }
            for (int i = 1; i < IRTime; i++)
            {
                DataGen.TIME = DateTimeOffset.UtcNow;

                DataGen.STATE = "WALK";

                Random RSteps = new();
                int IRSteps = RSteps.Next(0, 10);
                DataGen.STEPS += IRSteps switch
                { 0 => 3, 1 => 2, _ => 1, };

                Random RHeartBeat = new();
                int IRHeartBeat = RHeartBeat.Next(0, 10);
                switch (IRHeartBeat)
                {
                    case 0:
                        if (DataGen.HEARTBEAT > 62) { DataGen.HEARTBEAT -= 2; } else { DataGen.HEARTBEAT += 2; };
                        break;
                    case 1:
                        if (DataGen.HEARTBEAT > 61) { DataGen.HEARTBEAT -= 1; } else { DataGen.HEARTBEAT += 1; };
                        break;
                    case 8:
                        if (DataGen.HEARTBEAT < 99) { DataGen.HEARTBEAT += 1; } else { DataGen.HEARTBEAT -= 1; };
                        break;
                    case 9:
                        if (DataGen.HEARTBEAT < 98) { DataGen.HEARTBEAT += 2; } else { DataGen.HEARTBEAT -= 2; };
                        break;
                    default:
                        break;
                }

                Random RNFall = new();
                int IRNFall = RNFall.Next(0, 3000);
                if (IRNFall == 50) { DataGen.NFALL++; AllarmGen.SendAllarm(1); };

                if (DataGen.COUNTER == 900) { DataGen.BATTERY--; DataGen.COUNTER = 0; };
                if (DataGen.BATTERY == 10) { AllarmGen.SendAllarm(2); };
                if (DataGen.BATTERY == 0) { DataGen.BATTERY = 100; };

                DataGen.TSPAN = DataGen.TIME - DataGen.GTIME;
                DataGen.STANDING = Convert.ToInt32(DataGen.TSPAN.TotalSeconds);
                DataGen.STANDING += DataGen.RTIME;

                Random RSerendipity = new();
                DataGen.SERENDIPITY = RSerendipity.Next(1, 21);
                if (DataGen.NFALL >= 1) { DataGen.SERENDIPITY += RSerendipity.Next(1, 21); }

                Thread.Sleep(1000);
                DataGen.COUNTER++;
                if (DateTimeOffset.UtcNow.Day != DataGen.TIME.Day) { DataGen.STEPS = 0; };
            }
        }
    }
}
