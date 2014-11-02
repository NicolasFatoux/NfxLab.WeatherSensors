#if NETWORK

using System;
using Microsoft.SPOT;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace NfxLab.MicroFramework.Network
{
    /// <summary>
    /// HTTP client
    /// </summary>
    /// 
    public class HttpClient
    {
        const int PollDuration = 1000000;

        const int SendTimeout = 1000;
        const int ReceiveTimeout = 1000;
        
        Socket socket;


        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public HttpResponse ExecuteRequest(HttpRequest request)
        {
            // Executing the request
            Connect(request.Host, request.Port);
            SendRequest(request);
            var response = ReadResponse();

            return response;
        }


        /// <summary>
        /// Connects to a host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <exception cref="System.ApplicationException">Host not found</exception>
        void Connect(string host, int port)
        {
            // DNS Resolution
            var hostEntry = Dns.GetHostEntry(host);
            IPAddress hostAddress = null;
            for (int i = 0; i < hostEntry.AddressList.Length; i++)
                if (hostEntry.AddressList[i] != null)
                {
                    hostAddress = hostEntry.AddressList[i];
                    break;
                }

            if (hostAddress == null)
                throw new ApplicationException("Host not found : " + host);

            // Socket initialization
            var remoteEndPoint = new IPEndPoint(hostAddress, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(remoteEndPoint);
            socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            socket.SendTimeout = SendTimeout;
            socket.ReceiveTimeout = ReceiveTimeout;
        }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="request">The request.</param>
        void SendRequest(HttpRequest request)
        {
            var stream = new NetworkStream(socket);
            request.Write(stream);
            stream.Close();
        }

        /// <summary>
        /// Reads the response.
        /// </summary>
        /// <returns></returns>
        HttpResponse ReadResponse()
        {
            while (!socket.Poll(PollDuration, SelectMode.SelectRead)) ;

            var stream = new NetworkStream(socket);
            HttpResponse response = null;
            try
            {
                response = HttpResponse.Read(stream);
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
            stream.Close();
            return response;
        }



        /// <summary>
        /// Encode a string into an URL.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns></returns>
        public static string UrlEncode(string s)
        {
            string encoded = string.Empty;

            foreach (char c in s)
                if (
                    (c >= 'a' && c <= 'z')
                    || (c >= 'A' && c <= 'Z')
                    || (c >= '0' && c <= '9')
                    )
                {
                    encoded += c;
                }
                else
                {

                    int h = c;
                    encoded += "%" + h.ToString("x1");
                };

            return encoded;
        }
    }
}
#endif