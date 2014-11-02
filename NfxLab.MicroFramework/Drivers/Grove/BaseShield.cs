using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace NfxLab.MicroFramework.Drivers.Grove
{
    public class BaseShield
    {
        public enum DigitalPorts
        {
            Digital1, Digital2, Digital3, Digital4, Digital5,
            Digital6, Digital7, Digital8, Digital9, Digital10,
            Digital11, Digital12, Digital13
        };

        public enum AnalogPorts
        {
            Analog0, Analog1, Analog2, Analog3, Analog4
        };

        public enum I2CPorts
        {
            I2C1, I2C2
        }


        public static Cpu.AnalogChannel[] GetAnalogChannels(AnalogPorts port)
        {
            switch (port)
            {
                case AnalogPorts.Analog0:
                    return new[] { Cpu.AnalogChannel.ANALOG_0, Cpu.AnalogChannel.ANALOG_1 };
                case AnalogPorts.Analog1:
                    return new[] { Cpu.AnalogChannel.ANALOG_1, Cpu.AnalogChannel.ANALOG_2 };
                case AnalogPorts.Analog2:
                    return new[] { Cpu.AnalogChannel.ANALOG_2, Cpu.AnalogChannel.ANALOG_3 };
                case AnalogPorts.Analog3:
                    return new[] { Cpu.AnalogChannel.ANALOG_3, Cpu.AnalogChannel.ANALOG_4 };
                case AnalogPorts.Analog4:
                    return new[] { Cpu.AnalogChannel.ANALOG_4, Cpu.AnalogChannel.ANALOG_5 };
            }

            throw new ArgumentException("Invalid Analog port", "port");
        }


        public static Cpu.Pin[] GetDigitalPins(DigitalPorts port)
        {
            switch (port)
            {
                // Mapping tested with Netduino Plus

                case DigitalPorts.Digital1:
                    return new[] { (Cpu.Pin)28, (Cpu.Pin)0 };
                case DigitalPorts.Digital2:
                    return new[] { (Cpu.Pin)0, (Cpu.Pin)1 };
                case DigitalPorts.Digital3:
                    return new[] { (Cpu.Pin)1, (Cpu.Pin)12 };
                case DigitalPorts.Digital4:
                    return new[] { (Cpu.Pin)12, (Cpu.Pin)51 };
                case DigitalPorts.Digital5:
                    return new[] { (Cpu.Pin)51, (Cpu.Pin)52 };
                case DigitalPorts.Digital6:
                    return new[] { (Cpu.Pin)52, (Cpu.Pin)3 };
                case DigitalPorts.Digital7:
                    return new[] { (Cpu.Pin)3, (Cpu.Pin)4 };
                case DigitalPorts.Digital8:
                    return new[] { (Cpu.Pin)4, (Cpu.Pin)53 };
                case DigitalPorts.Digital9:
                    return new[] { (Cpu.Pin)53, (Cpu.Pin)54 };
                case DigitalPorts.Digital10:
                    return new[] { (Cpu.Pin)54, (Cpu.Pin)17 };
                case DigitalPorts.Digital11:
                    return new[] { (Cpu.Pin)17, (Cpu.Pin)16 };
                case DigitalPorts.Digital12:
                    return new[] { (Cpu.Pin)16, (Cpu.Pin)18 };
                case DigitalPorts.Digital13:
                    return new[] { (Cpu.Pin)18, (Cpu.Pin)284 };
            }

            throw new ArgumentException("Invalid Analog port", "port");
        }
    }
}
