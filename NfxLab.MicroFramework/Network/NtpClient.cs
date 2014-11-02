#if NETWORK

using System;
using Microsoft.SPOT;
using System.Net;
using System.Net.Sockets;
using Microsoft.SPOT.Hardware;

namespace NfxLab.MicroFramework.Network
{
    /// <summary>
    /// NTP Client to synchronize the system clock
    /// Based on : http://weblogs.asp.net/mschwarz/wrong-datetime-on-net-micro-framework-devices
    /// </summary>
    public class NtpClient
    {
        /// <summary>
        /// Time server name
        /// </summary>
        public string TimeServer { get; set; }

        /// <summary>
        /// Time zone
        /// </summary>
        public int TimeZone { get; set; }

        /// <summary>
        /// Synchronize the system time
        /// </summary>
        public void SyncTime()
        {
            DateTime time = GetNetworkTime();

            DateTime localTime = time + new TimeSpan(TimeZone, 0 ,0);
            Utility.SetLocalTime(localTime);
        }
        
        /// <summary>
        /// Get the current UTC time from the time server
        /// </summary>
        /// <returns></returns>
        public DateTime GetNetworkTime()
        {
            IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(TimeServer).AddressList[0], 123);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.Connect(ep);

            byte[] ntpData = new byte[48]; // RFC 2030
            ntpData[0] = 0x1B;
            for (int i = 1; i < 48; i++)
                ntpData[i] = 0;

            s.Send(ntpData);
            s.Receive(ntpData);

            byte offsetTransmitTime = 40;
            ulong intpart = 0;
            ulong fractpart = 0;
            for (int i = 0; i <= 3; i++)
                intpart = 256 * intpart + ntpData[offsetTransmitTime + i];

            for (int i = 4; i <= 7; i++)
                fractpart = 256 * fractpart + ntpData[offsetTransmitTime + i];

            ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);

            s.Close();

            TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);
            DateTime dateTime = new DateTime(1900, 1, 1);
            dateTime += timeSpan;

            return dateTime;
        }
    }
}

#endif