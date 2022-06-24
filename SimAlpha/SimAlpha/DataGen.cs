using System;

namespace SimAlpha
{
    internal class DataGen
    {
        public static string UUID;
        public static double[][] LATLON = new double[2][];
        public static double[] GPS = new double[2];
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
            Random random = new();
            TMR = new System.Timers.Timer(5000); // 60000
            TMR.Elapsed += TSData.SendData;
            TMR.AutoReset = true;
            TMR.Enabled = true;
            UUID = Guid.NewGuid().ToString();
            HEARTBEAT = random.Next(60, 101);
            BATTERY = random.Next(50, 101);
            RTIME = random.Next(0, 57600);
            LATLON[0] = new double[5] { 46.056041, 45.462712, 41.909986, 44.499096, 43.942856 };
            LATLON[1] = new double[5] { 13.174122, 9.107692, 12.395913, 11.261645, 12.390053 };
            int IRGPSL = random.Next(0, 5);
            GPS[0] = LATLON[0][IRGPSL];
            GPS[1] = LATLON[1][IRGPSL];
            while (true)
            {
                int IRState = random.Next(0, 4);
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
