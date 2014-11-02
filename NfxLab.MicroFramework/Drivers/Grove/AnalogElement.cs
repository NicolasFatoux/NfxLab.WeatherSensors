using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace NfxLab.MicroFramework.Drivers.Grove
{
    public abstract class AnalogElement : Element
    {
        protected Cpu.AnalogChannel Channel1 {get;private set;}
        protected Cpu.AnalogChannel Channel2 { get; private set; }

        public AnalogElement(BaseShield.AnalogPorts port)
        {
            var channels = BaseShield.GetAnalogChannels(port);
            Channel1 = channels[0];
            Channel2 = channels[1];
        }

    }
}
