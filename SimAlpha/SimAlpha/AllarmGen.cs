﻿using System;

namespace SimAlpha
{
    internal class AllarmGen
    {
        internal static void SendAllarm(AllarmType type)
        {
            switch (type)
            {
                case AllarmType.HEARTBEAT:
                    Console.WriteLine("ALLARM HEARTBEAT");
                    break;
                case AllarmType.FALL:
                    Console.WriteLine("ALLARM NFALL");
                    break;
                case AllarmType.LOW_BATTERY:
                    Console.WriteLine("ALLARM BATTERY");
                    break;
            }
        }
    }
}