using System;
using System.Threading;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;
using BreakContinue.Sensors;

namespace drifft
{
    public class Program
    {  //змінні

        public static int drive = 1;
        public static int steer;
        public static int danger = 50;
        public static int objectinfont = 80;
        public static int freespace = 140;

        public static void Main()
        {            // порти
            FEZ_Shields.DCMotorDriver.Initialize();
            ParallaxPing ping = new ParallaxPing((Cpu.Pin)FEZ_Pin.Digital.Di0);
      
            while (true)
            {
            Main:
                int distance = ping.GetDistance(ParallaxPing.DistanceUnits.Centimeters);
            Thread.Sleep(300);
               // Debug.Print("Distance in centimeters: " + distance.ToString());
           
                if (distance > danger)
                {
                                 
                    speedhead();
                
                    if (distance > freespace)
                    {
                       // turnleft();
                        turnright();
                        goto Main;
                    }
                    else if (distance < objectinfont)
                    {
                       // turnzero();
                        turnleft();
                       goto Main;
                    }
                }
                else if (distance > objectinfont && distance < freespace)
                {
                    turnzero();
                    goto Main;
                }


                else
                {
                    speedreverse();
                    goto Main;
                }
                goto Main;
            }

        }
            public static void turnright()
            {
             //   Debug.Print("left");
                FEZ_Shields.DCMotorDriver.Moves(100, -100);
                
            }

            public static void turnleft()
            {
               // Debug.Print("right");
               FEZ_Shields.DCMotorDriver.Move(100, 100);
               
            }

            public static void turnzero()
            {
               // Debug.Print("zero");
                FEZ_Shields.DCMotorDriver.Move(100, 0);
            }

            public static void speedhead()
            {
               // Debug.Print("speed");
                FEZ_Shields.DCMotorDriver.Move(70, 0);
            }

            public static void speedreverse()
            {
              //  Debug.Print("reverse");
                FEZ_Shields.DCMotorDriver.Moves(-100,0);
            }

            public static void stop()
            {
              //  Debug.Print("stop");
                FEZ_Shields.DCMotorDriver.Move(0, 0);
            }

    }
        }
        
