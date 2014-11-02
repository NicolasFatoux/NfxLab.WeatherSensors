using System;
using Microsoft.SPOT;
using System.IO.Ports;
using System.Text;
using Microsoft.SPOT.Hardware;
using System.Threading;
using System.IO;

namespace NfxLab.MicroFramework.Drivers.Grove
{
    public class RFReceiver
    {
        const int MaxBufferSize = 200;
        SerialPort port;


        public RFReceiver(string portName)
        {
            port = new SerialPort(portName, RFTransmitter.BaudRate, Parity.None, 8, StopBits.One);
            port.Open();
        }

        public Stream ReceiverStream
        {
            get
            {
                return port;
            }
        }


        public bool Enabled
        {
            get
            {
                return port.IsOpen;
            }
            set
            {
                if (value)
                    port.Open();
                else
                    port.Close();
            }
        }

        public void Unplug()
        {
            port.Close();
            port.Dispose();
            port = null;
        }
    }
}
