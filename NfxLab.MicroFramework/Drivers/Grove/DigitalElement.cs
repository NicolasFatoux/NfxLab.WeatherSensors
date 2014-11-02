using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace NfxLab.MicroFramework.Drivers.Grove
{
    public abstract class DigitalElement : Element
    {
        protected Cpu.Pin Pin1 { get; private set; }
        protected Cpu.Pin Pin2 { get; private set; }

        public DigitalElement(BaseShield.DigitalPorts port)
        {
            var pins = BaseShield.GetDigitalPins(port);
            Pin1 = pins[0];
            Pin2 = pins[1];
        }
    }
}
