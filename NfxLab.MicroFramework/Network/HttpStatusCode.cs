#if NETWORK
using System;
using Microsoft.SPOT;

namespace NfxLab.MicroFramework.Network
{
    /// <summary>
    /// HTTP response status codes
    /// </summary>
    public static class HttpStatusCode
    {
        public const int OK = 200;
        public const int Bad = 400;
    }
}
#endif