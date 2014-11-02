using System;
using Microsoft.SPOT;
using NfxLab.MicroFramework.Drivers.Grove;

namespace NfxLab.WeatherSensors
{
    static class Configuration
    {
        public const string RFTransmitterPort = "COM1";

        public const BaseShield.DigitalPorts TemperatureHumiditySensorPort = BaseShield.DigitalPorts.Digital1;

        public static readonly TimeSpan UpdateInterval = new TimeSpan(0, 0, 5);

    }
}
