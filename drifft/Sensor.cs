using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace BreakContinue.Sensors
{
    /// <summary>
    /// Helper class to work with Parallax Ping))) sensor
    /// </summary>
    public class ParallaxPing
    {
        /// <summary>
        /// The PING))) returns a pulse width of 73.746 uS per inch.
        /// </summary>
        const int TO_INCH = 73746;

        /// <summary>
        /// The PING))) returns a pulse width of 29.033 uS per centimeter.
        /// </summary>
        const int TO_CM = 29034;

        /// <summary>
        /// Ticks per microsecond
        /// </summary>
        const int TicksPerMicrosecond = (int)TimeSpan.TicksPerMillisecond / 1000;


        /// <summary>
        /// Enumeration to indicate in what units we want the distance back
        /// </summary>
        public enum DistanceUnits
        {
            Inches,
            Centimeters,
            Millimeters
        }

        /// <summary>
        /// Read/Write port attached to the connected pin
        /// </summary>
        private TristatePort _pin;

        //Pin on the microcontroller that is used to communicate with Ping))) Sensor
        public ParallaxPing(Cpu.Pin pin)
        {
            _pin = new TristatePort(pin, false, false, Port.ResistorMode.Disabled);
        }

        /// <summary>
        /// Returns distance in requested units
        /// </summary>
        /// <param name="units">distance units to use</param>
        /// <returns>distance</returns>
        public int GetDistance(DistanceUnits units)
        {
            //Set Write mode 'ON'
            _pin.Active = true;
            _pin.Write(true);
            _pin.Write(false);

            //Set Read mode 'ON'
            _pin.Active = false;
            bool high = false;

            //Wait for start of the echo pulse.
            do
            {
                high = _pin.Read();
            }
            while (!high);

            //Measure current time
            long startTime = System.DateTime.Now.Ticks;

            // Wait for end of echo pulse.
            while (high) 
            {
                high = _pin.Read(); 
            }    

            //Get total ticks accounting for roundtrip
            int ticks = (int)(System.DateTime.Now.Ticks - startTime)>>1;

            int microSeconds = ticks / TicksPerMicrosecond;


            if( units == DistanceUnits.Inches )
                return microSeconds * 1000 / TO_INCH;
            else if( units == DistanceUnits.Centimeters)
                return microSeconds * 1000 / TO_CM;
            else
                return microSeconds * 10000 / TO_CM;
        }
    }
}
