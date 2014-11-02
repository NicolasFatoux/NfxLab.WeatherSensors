#if NETWORK
using NfxLab.MicroFramework.External;
using NfxLab.MicroFramework.Logging;
using System;
using System.Collections;

namespace NfxLab.MicroFramework.Network
{
    /// <summary>
    /// xively.com feed client
    /// </summary>
    public class XivelyClient
    {
        string apiKey;
        string url;
        HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="XivelyClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="feedId">The feed id.</param>
        public XivelyClient(string apiKey, string feedId)
        {
            this.apiKey = apiKey;

            this.url = "http://api.xively.com/v2/feeds/" + feedId;

            this.httpClient = new HttpClient();
        }

        /// <summary>
        /// Puts new values into xively feed.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="values">The values.</param>
        /// <exception cref="System.ApplicationException">The xively server returned invalid status code.</exception>
        public void Put(string[] ids, object[] values)
        {
            var datastreams = new Hashtable[ids.Length];
            for (int i = 0; i < ids.Length; i++)
                datastreams[i] = new Hashtable
                                    {
                                        {"id",ids[i]},
                                        {"current_value",values[i]},
                                    };

            var json = new Hashtable
            {
                {"version","1.0.0"},
                {"datastreams",datastreams},
            };


            var request = new HttpRequest
            {
                Method = "PUT",
                Url = url,
                Content = JSON.JsonEncode(json),
            };

            request.AddHeader("X-ApiKey", apiKey);

            var response = httpClient.ExecuteRequest(request);

            if (response.StatusCode != 200)
                throw new ApplicationException("Xively server returned invalid status code : " + response.StatusCode);
        }
    }
}
#endif