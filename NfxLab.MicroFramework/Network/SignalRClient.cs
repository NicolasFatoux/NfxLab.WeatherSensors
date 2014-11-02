#if NETWORK
using System;
using Microsoft.SPOT;
//using System.Net;
using NfxLab.MicroFramework.External;
using System.IO;
using System.Collections;
using System.Threading;

namespace NfxLab.MicroFramework.Network
{
    public delegate object SignalRCallback(IList args);

    public class SignalRClient
    {
        private SignalRReceiver receiver;
        private HttpClient httpClient;
        private int counter;


        #region Public Properties
        public string Url { get; private set; }

        public string Hub { get; set; }

        public string ConnectionToken { get; private set; }

        public string ConnectionId { get; private set; }
        #endregion


        #region Public Methods
        public SignalRClient(string url, string hub)
        {
            Url = url;
            Hub = hub.ToLower() + "hub";

            receiver = new SignalRReceiver(this);
        }

        public void Connect()
        {
            httpClient = new HttpClient();

            Negociate();
            Receive();
        }

        public object Call(string method, params object[] arguments)
        {
            var message = new Hashtable
            {
                {"H",Hub},
                {"M",method},
                {"A",arguments},
                {"I",counter++},
            };
            var json = JSON.JsonEncode(message);

            var request = new HttpRequest
            {
                Url = Url + "send?transport=longPolling&connectionToken=" + ConnectionToken,              
                Method = "POST",
                Accept = "application/json",
                ContentType = "application/x-www-form-urlencoded; charset=UTF-8",
                Content = "data=" + HttpClient.UrlEncode(json),
            };
            var response = httpClient.ExecuteRequest(request);

            return JSON.JsonDecode(response.Content);
        }

        public void Register(string method, SignalRCallback callback)
        {
            receiver.Register(method, callback);
        }
        #endregion


        #region Private Methods
        private void Negociate()
        {
            var random = new Random();
            var id = random.NextDouble();

            var request = new HttpRequest
            {
                Url = Url + "negotiate?_=" + id,
                Accept = "application/json",
            };

            var response = httpClient.ExecuteRequest(request);
            if (response.StatusCode != HttpStatusCode.OK)
                return;


            int startIndex = response.Content.IndexOf('{');
            int endIndex = response.Content.LastIndexOf('}');
            var json = response.Content.Substring(startIndex, endIndex - startIndex+1);
            var parameters = (Hashtable)JSON.JsonDecode(json);
            ConnectionToken = parameters["ConnectionToken"] as string;
            ConnectionId = parameters["ConnectionId"] as string;
        }

        private void Receive()
        {
            receiver.Start();
        }
        #endregion
    }
}
#endif