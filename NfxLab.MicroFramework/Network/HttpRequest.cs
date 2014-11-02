#if NETWORK
using System;
using Microsoft.SPOT;
using System.IO;
using System.Collections;
using NfxLab.MicroFramework.Logging;
using System.Diagnostics;

namespace NfxLab.MicroFramework.Network
{
    /// <summary>
    /// A HTTP request
    /// </summary>
    public class HttpRequest
    {
        Hashtable customHeaders;

        string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                ParseUrl();
            }
        }

        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Query { get; private set; }

        public string Method { get; set; }
        public string Accept { get; set; }
        public string ContentType { get; set; }
        public string UserAgent { get; set; }

        public string Content { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        public HttpRequest()
        {
            Method = "GET";
        }

        /// <summary>
        /// Adds an HTTP header.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddHeader(string name, string value)
        {
            if (customHeaders == null)
                customHeaders = new Hashtable();
            customHeaders.Add(name, value);
        }

        /// <summary>
        /// Writes the request into a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Write(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);

            // HTTP header
            writer.WriteLine(Method + " " + Query + " HTTP/1.1");


            // Request headers

            // Host
            writer.WriteLine("Host: " + Host);

            // Accept
            if (Accept != null)
                writer.WriteLine("Accept: " + Accept);

            // Content-Type
            if (ContentType != null)
                writer.WriteLine("Content-Type: " + ContentType);

            // Connection
            writer.WriteLine("Connection: Close");

            // User-Agent
            if (UserAgent != null)
                writer.WriteLine("User-Agent: " + UserAgent);

            // Custom headers
            if (customHeaders != null)
                foreach (DictionaryEntry entry in customHeaders)
                    writer.WriteLine((string)entry.Key + ": " + entry.Value);

            // Request content
            if (Content != null)
            {
                writer.WriteLine("Content-Length: " + Content.Length);
                writer.WriteLine(string.Empty);
                writer.WriteLine(Content);
            }
            else
            {
                //writer.WriteLine("Content-Length: 0");
                writer.WriteLine(string.Empty);
            }

            writer.Close();
        }


        /// <summary>
        /// Parses the URL.
        /// </summary>
        void ParseUrl()
        {
            // Extracting host and port from the url
            string host;
            string query;
            int port;

            // Looking for the end of the host fragment
            var endIndex = Url.IndexOf('/', 7);
            if (endIndex > 0)
                host = Url.Substring(7, endIndex - 7);
            else host = Url.Substring(7);

            // Extracting the query
            if (endIndex > 0)
                query = Url.Substring(endIndex);
            else
                query = "/";

            // Parsing the port if present
            endIndex = host.IndexOf(':');
            if (endIndex > 0)
            {
                var sPort = host.Substring(endIndex + 1);
                port = int.Parse(sPort);
                host = host.Substring(0, endIndex);
            }
            else
            {
                port = 80;
            }

            // Setting properties
            Host = host;
            Port = port;
            Query = query;
        }
    }
}
#endif