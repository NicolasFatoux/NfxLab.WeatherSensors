using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;
using NfxLab.MicroFramework.Logging;
using System.Diagnostics;

namespace NfxLab.MicroFramework.Drivers.Grove
{
    public class TemperatureHumiditySensorPro : DigitalElement
    {
        const long Threshold = 1050;
        const int ReadTimeout = 50;
        const int ReadInterval = 500;
        const int ReadRetry = 50;

        #region Members

        // ports
        TristatePort outputPort;
        InterruptPort inputPort;

        // synchronization
        AutoResetEvent dataReceivedEvent;

        // read buffer 
        long buffer;

        // reading 
        long previousTicks;
        int bitsCount;

        #endregion

        #region Properties

        public double Humidity { get; private set; }

        public double Temperature { get; private set; }

        #endregion

        #region Public methods

        public TemperatureHumiditySensorPro(BaseShield.DigitalPorts port)
            : base(port)
        {
            outputPort = new TristatePort(Pin1, true, false, Port.ResistorMode.PullUp);

            inputPort = new InterruptPort(Pin2, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);
            inputPort.DisableInterrupt();
            inputPort.OnInterrupt += new NativeEventHandler(inputPort_OnInterrupt);

            dataReceivedEvent = new AutoResetEvent(false);
        }

        public void Read()
        {
            for (int i = 0; i < ReadRetry; i++)
            {
                bool succeded = TryRead();
                if (succeded)
                    return;

                Thread.Sleep(ReadInterval);
            }

            throw new Exception("Sensor data couldn't be read.");
        }

        public bool TryRead()
        {
            // Initialization
            bitsCount = 0;
            buffer = 0;

            // Start Sequence
            outputPort.Active = true;
            outputPort.Write(true);
            Thread.Sleep(2);

            // Receiving data
            outputPort.Write(false);
            inputPort.EnableInterrupt();
            outputPort.Active = false;

            // Waiting data
            if (!dataReceivedEvent.WaitOne(ReadTimeout, false))
                return false;
            
            // Reading data
            Humidity = ((buffer >> 24) & 0xFFFF) * 0.1F;
            
            Temperature = ((buffer >> 8) & 0xFFFF) * 0.1F;
            
            byte checksum = (byte)(buffer & 0xFF);
            
            // Checksum verification
            byte myChecksum = (byte)(buffer >> 8);
            myChecksum += (byte)(buffer >> 16);
            myChecksum += (byte)(buffer >> 24);
            myChecksum += (byte)(buffer >> 32);

            if (myChecksum != checksum)
                return false;
            
            // Values verification
            if (Temperature < -40 || Temperature > 40
                || Humidity < 5 || Humidity > 99)
                return false;
            
            return true;
        }

        #endregion

        #region Private methods
        void inputPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            var ticks = time.Ticks;
            var delta = ticks - previousTicks;

            buffer <<= 1;

            if (delta > Threshold)
                buffer |= 1;

            bitsCount++;
            previousTicks = ticks;

            if (bitsCount == 43)
            {
                inputPort.DisableInterrupt();
                dataReceivedEvent.Set();
            }
        }
        #endregion
    }
}
