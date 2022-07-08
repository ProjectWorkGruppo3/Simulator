using System;
using System.Threading;

namespace SimAlpha.States
{
    internal class CState2
    {
        public static void State2()
        {
            Random random = new();
            int IRTime = random.Next(1, 3601);
            if (DataGen.STANDING == 0) { DataGen.GTIME = DateTimeOffset.UtcNow; }
            for (int i = 1; i < IRTime; i++)
            {
                DataGen.TIME = DateTimeOffset.UtcNow;

                DataGen.STATE = "Running";

                int IRSteps = random.Next(0, 10);
                DataGen.STEPS += IRSteps switch
                { 0 => 5, 1 => 4, 2 => 3, 3 => 1, _ => 2, };

                int IRHeartBeat = random.Next(0, 10);
                switch (IRHeartBeat)
                {
                    case 0: if (DataGen.HEARTBEAT > 102) { DataGen.HEARTBEAT -= 2; } else { DataGen.HEARTBEAT += 2; }; break;
                    case 1: if (DataGen.HEARTBEAT > 101) { DataGen.HEARTBEAT -= 1; } else { DataGen.HEARTBEAT += 1; }; break;
                    case 8: if (DataGen.HEARTBEAT < 219) { DataGen.HEARTBEAT += 1; } else { DataGen.HEARTBEAT -= 1; }; break;
                    case 9: if (DataGen.HEARTBEAT < 218) { DataGen.HEARTBEAT += 2; } else { DataGen.HEARTBEAT -= 2; }; break;
                    default: break;
                }
                if (DataGen.HEARTBEAT > 200) { AllarmGen.SendAllarm(0); };

                int IRNFall = random.Next(0, 5000);
                if (IRNFall == 5) { DataGen.NFALL++; AllarmGen.SendAllarm(AllarmType.FALL); };

                if (DataGen.COUNTER == 900) { DataGen.BATTERY--; DataGen.COUNTER = 0; };
                if (DataGen.BATTERY == 10) { AllarmGen.SendAllarm(AllarmType.LOW_BATTERY); };
                if (DataGen.BATTERY == 0) { DataGen.BATTERY = 100; };

                DataGen.TSPAN = DataGen.TIME - DataGen.GTIME;
                DataGen.STANDING = Convert.ToInt32(DataGen.TSPAN.TotalSeconds);
                DataGen.STANDING += DataGen.RTIME;

                DataGen.SERENDIPITY = random.Next(1, 21);
                if (DataGen.NFALL >= 1) { DataGen.SERENDIPITY += random.Next(1, 21); }
                if (DataGen.HEARTBEAT >= 200) { DataGen.SERENDIPITY += random.Next(1, 21); }

                int IRGPS = random.Next(0, 4);
                switch (IRGPS)
                {
                    case 0: DataGen.GPS[0] += random.Next(1, 10001) / 1000000.0; DataGen.GPS[1] += random.Next(1, 10001) / 1000000.0; break;
                    case 1: DataGen.GPS[0] += random.Next(1, 10001) / 1000000.0; DataGen.GPS[1] -= random.Next(1, 10001) / 1000000.0; break;
                    case 2: DataGen.GPS[0] -= random.Next(1, 10001) / 1000000.0; DataGen.GPS[1] += random.Next(1, 10001) / 1000000.0; break;
                    case 3: DataGen.GPS[0] -= random.Next(1, 10001) / 1000000.0; DataGen.GPS[1] -= random.Next(1, 10001) / 1000000.0; break;
                    default: break;
                }

                DataGen.GPS[0] = Math.Truncate(DataGen.GPS[0] * 1000000) / 1000000;
                DataGen.GPS[1] = Math.Truncate(DataGen.GPS[1] * 1000000) / 1000000;

                Thread.Sleep(1000);
                DataGen.COUNTER++;
                if (DateTimeOffset.UtcNow.Day != DataGen.TIME.Day) { DataGen.STEPS = 0; };
            }
        }
    }
}
