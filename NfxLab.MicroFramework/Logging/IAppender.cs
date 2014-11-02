using System;
using Microsoft.SPOT;

namespace NfxLab.MicroFramework.Logging
{
    public interface IAppender
    {
        void Write(string message);
    }
}
