#if NETWORK
using Microsoft.SPOT;
using NfxLab.MicroFramework.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NfxLab.MicroFramework.Network
{
    /// <summary>
    /// A HTTP response
    /// </summary>
    public class HttpResponse
    {
        public int StatusCode { get; set; }
        public string Reason { get; set; }
        public string Content { get; set; }


        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static HttpResponse Read(Stream stream)
        {
            HttpResponse response = new HttpResponse();
            StreamReader reader = new StreamReader(stream);


            // Status line
            var statusLine = reader.ReadLine();
            var elements = statusLine.Split(' ');

            response.StatusCode = int.Parse(elements[1]);
            response.Reason = elements[2];

            // Header
            string line = null;
            while (line != string.Empty)
            {
                line = reader.ReadLine();
            }

            // Content
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                response.Content += line;
            }

            reader.Close();

            return response;
        }
    }
}
#endif