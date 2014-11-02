using System;
using Microsoft.SPOT;
using System.Diagnostics;

namespace NfxLab.MicroFramework.Logging
{
    public class DebugAppender : IAppender
    {
        public void Write(string message)
        {
            Debug.Print(message);
        }

    }
}
