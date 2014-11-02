using System;
using Microsoft.SPOT;
using System.IO.Ports;
using System.Text;

namespace NfxLab.MicroFramework.Drivers.Grove
{
    public class RFTransmitter
    {
        internal const int BaudRate = 4800;

        SerialPort port;

        public void Plug(string portName)
        {
            port = new SerialPort(portName, BaudRate, Parity.None, 8, StopBits.One);
            port.Open();
        }

        public void Unplug()
        {
            port.Close();
            port.Dispose();
            port = null;
        }

        public void Send(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);

             port.Write(bytes, 0, bytes.Length);
        }

        public void Send(byte[] data)
        {
            port.Write(data, 0, data.Length);
        }


    }
}
