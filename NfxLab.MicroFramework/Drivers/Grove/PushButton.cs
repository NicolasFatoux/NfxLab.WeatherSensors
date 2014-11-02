using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace NfxLab.MicroFramework.Drivers.Grove
{
    public class PushButton : DigitalElement
    {
        InterruptPort interruptPort;


        public PushButton(BaseShield.DigitalPorts port)
            : base(port)
        {
            interruptPort = new InterruptPort(this.Pin1, true, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptNone);
            interruptPort.OnInterrupt += port_OnInterrupt;
        }

        void port_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (Push != null)
                Push();
        }

        public delegate void PushEventHandler();

        public event PushEventHandler Push;
    }
}
