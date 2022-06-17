using System;

namespace SimAlpha
{
    internal class DataGen
    {
        public static readonly string UUID = "b7b55ed1-bb77-4057-918f-2386dd87161f";
        public static System.Timers.Timer TMR;
        public static DateTimeOffset TIME;
        public static DateTimeOffset GTIME;
        public static TimeSpan TSPAN;
        public static bool TOKEN = false;
        public static int COUNTER = 0;
        public static int SERENDIPITY;
        public static int STEPS;
        public static int HEARTBEAT;
        public static int NFALL;
        public static int BATTERY;
        public static int STANDING;
        public static string STATE;
        public static int RTIME;

        public static void Data()
        {
            TMR = new System.Timers.Timer(10000); // 60000
            TMR.Elapsed += TSData.SendData;
            TMR.AutoReset = true;
            TMR.Enabled = true;
            Random RVHeartBeat = new();
            HEARTBEAT = RVHeartBeat.Next(60, 101);
            Random RBattery = new();
            BATTERY = RBattery.Next(50, 101);
            Random RRTime = new();
            RTIME = RRTime.Next(0, 57600);
            while (true)
            {
                Random RState = new();
                int IRState = RState.Next(0, 4);
                switch (IRState)
                {
                    case 0: States.CState0.State0(); break;
                    case 1: States.CState1.State1(); break;
                    case 2: States.CState2.State2(); break;
                    case 3: States.CState3.State3(); break;
                    default: States.CState0.State0(); break;
                }
            }
        }
    }
}
