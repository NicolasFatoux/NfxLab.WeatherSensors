#if NETWORK
using System;
using Microsoft.SPOT;
using System.Threading;
using System.Collections;
using NfxLab.MicroFramework.External;
//using System.Net;

namespace NfxLab.MicroFramework.Network
{
    class SignalRReceiver
    {
        SignalRClient client;

        HttpClient httpClient;
        Thread receptionThread;
        Hashtable callbacks;


        public SignalRReceiver(SignalRClient client)
        {
            this.client = client;

            httpClient = new HttpClient();
            callbacks = new Hashtable();
        }

        public void Start()
        {
            receptionThread = new Thread(HandleConnection);
            receptionThread.Priority = ThreadPriority.Lowest;
            receptionThread.Start();
        }

        public void Stop()
        {
            receptionThread.Abort();
            receptionThread = null;
        }

        public void Register(string method, SignalRCallback callback)
        {
            callbacks.Add(method, callback);
        }

        private void HandleConnection()
        {
            while (true)
            {
                var message = (Hashtable)CallConnect();
                if (message == null)
                    continue;

                var methodsParameters = (ArrayList)message["M"];
                foreach (Hashtable methodParameters in methodsParameters)
                {

                    var method = (string)methodParameters["M"];
                    if (!callbacks.Contains(method))
                        return;

                    var arguments = (ArrayList)methodParameters["A"];

                    var callback = callbacks[method] as SignalRCallback;
                    callback(arguments);
                }
            }
        }



        object CallConnect()
        {
            var json = JSON.JsonEncode(
                new ArrayList()
                    {
                        new Hashtable { { "name", client.Hub } }
                    }
            );
            var encoded = HttpClient.UrlEncode(json);

            var request = new HttpRequest
            {
                Url = client.Url
                        + "connect?transport=longPolling&connectionToken=" + client.ConnectionToken
                        + "&connectionData=" + encoded,
                Accept = "application/json",
            };
            var response = httpClient.ExecuteRequest(request);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            Debug.Print("!! " + response.Content);
            var data = JSON.JsonDecode(response.Content) as Hashtable;
            return data;
        }
    }
}
#endif