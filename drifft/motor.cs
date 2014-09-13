/*
Copyright 2010 GHI Electronics LLC
Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License. 
*/

using System;
//using System.IO.Ports;
//using System.Runtime.CompilerServices;
using System.Threading;
//using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.Hardware;

namespace GHIElectronics.NETMF.FEZ
{
    public static partial class FEZ_Shields
    {
        static public class DCMotorDriver
        {
            static PWM _pwm1;
            static OutputPort _dir1;

            static PWM _pwm2;
            static OutputPort _dir2;

            static sbyte _last_speed1, _last_speed2;
            static public void Initialize()
            {
                _pwm1 = new PWM((PWM.Pin)FEZ_Pin.PWM.Di5);
                _dir1 = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di4, false);

                _pwm2 = new PWM((PWM.Pin)FEZ_Pin.PWM.Di6);
                _dir2 = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di7, false);
            }



            static public void Moves(sbyte speed1, sbyte speed2)
            {
                if (speed1 > 100 || speed1 < -100 || speed2 > 100 || speed2 < -100)
                    new ArgumentException();

                _last_speed1 = speed1;
                _last_speed2 = speed2;

                if (speed1 < 0)
                {
                    _dir1.Write(true);
                    _pwm1.Set(1000, (byte)(Math.Abs(speed1)));
                }
                else
                {
                    _dir1.Write(false);
                    _pwm1.Set(1000, (byte)(speed1));
                }



                ////////////////////////////

                if (speed2 < 0)
                {
                    _dir2.Write(true);
                    _pwm2.Set(1000, (byte)(Math.Abs(speed2)));
                }
                else
                {
                    _dir2.Write(false);
                    _pwm2.Set(1000, (byte)(speed2));
                }


            }
            static public void Move(sbyte speed1, sbyte speed2)
            {
                if (speed1 > 100 || speed1 < -100 || speed2 > 100 || speed2 < -100)
                    new ArgumentException();

                _last_speed1 = speed1;
                _last_speed2 = speed2;

                if (speed1 < 0)
                {
                    _dir1.Write(true);
                    _pwm1.Set(1000, (byte)(100 - Math.Abs(speed1)));
                }
                else
                {
                    _dir1.Write(false);
                    _pwm1.Set(1000, (byte)(speed1));
                }



                ////////////////////////////

                if (speed2 < 0)
                {
                    _dir2.Write(true);
                    _pwm2.Set(1000, (byte)(100 - Math.Abs(speed2)));
                }
                else
                {
                    _dir2.Write(false);
                    _pwm2.Set(1000, (byte)(speed2));
                }


            }

            static public void MoveRamp(sbyte speed1, sbyte speed2, byte ramping_delay)
            {
                sbyte temp_speed1 = _last_speed1;
                sbyte temp_speed2 = _last_speed2;

                while ((speed1 != temp_speed1) || (speed2 != temp_speed2))
                {
                    if (temp_speed1 > speed1)
                        temp_speed1--;
                    if (temp_speed1 < speed1)
                        temp_speed1++;

                    if (temp_speed2 > speed2)
                        temp_speed2--;
                    if (temp_speed2 < speed2)
                        temp_speed2++;

                    Move(temp_speed1, temp_speed2);
                    Thread.Sleep(ramping_delay);
                }
            }
        }
    }
}