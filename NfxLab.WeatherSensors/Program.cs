using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using NfxLab.MicroFramework.Logging;
using NfxLab.MicroFramework.Drivers.Grove;
using System.Collections;
using NfxLab.MicroFramework.External;
using System.Text;

namespace NfxLab.WeatherSensors
{
    public class Program
    {
        static Log Log;

        static TemperatureHumiditySensorPro Sensor;
        static RFTransmitter RFTransmitter;
        static Timer Timer;
        public static void Main()
        {
            // Log initialization
            Log = new Log(
                    new DebugAppender()
                    );

            try
            {
                Log.Info("WeatherSensors");
                Initialize();
                Start();
            }
            catch (Exception e)
            {
                Log.Info("Unhandled exception", e);
            }
        }

        static void Initialize()
        {
            Log.Info("Initialization");

            Log.Info("- Temperature & humidity sensor");
            Sensor = new TemperatureHumiditySensorPro(Configuration.TemperatureHumiditySensorPort);

            Log.Info("- RF transmitter");
            RFTransmitter = new RFTransmitter(Configuration.RFTransmitterPort);
        }

        static void Start()
        {
            Log.Info("Starting the timer");
            Timer = new Timer(UpdateData, null, new TimeSpan(0, 0, 1), Configuration.UpdateInterval);
        }
        static void UpdateData(object obj)
        {
            try
            {
                Log.Info("Reading sensor");
                Sensor.Read();

                var data = new Hashtable{
                { "temperature", Sensor.Temperature.ToString("f2") },
                {"humdity",Sensor.Humidity.ToString("f2")},
            };
                Log.Info("Temperature:", data["temperature"], "Humidity:", data["humidity"]);

                Log.Info("Sending data to MicroHub");
                var json = JSON.JsonEncode(data);
                var message = Encoding.UTF8.GetBytes(json);

                RFTransmitter.Send(message);
            }
            catch (Exception e)
            {
                Log.Warning("Exception", e);
            }
        }
    }
}
