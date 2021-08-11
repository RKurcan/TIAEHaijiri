using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static RTech.Demo.Utilities.WDMS;

namespace RTech.Demo.Utilities
{
    public class WebrequestService
    {
        WebClient wc = null;

        public string BaseUrl { get; private set; }
        public bool NoRequest { get; private set; }

        public WebrequestService(string baseUrl)
        {
            wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
           // wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.Headers.Add("Accept", "application/json");
            this.BaseUrl = baseUrl;
            
            this.NoRequest = RiddhaSession.EnableADMS==false;
            Log.SytemLog(RiddhaSession.EnableADMS.ToString());
        }
        public WebrequestService(string baseUrl,string contentType)
        {
            wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            //wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.Headers.Add("Accept", contentType);
            this.BaseUrl = baseUrl;

        }
        public async Task<T> Get<T>(string url)
        {
            //var factory =new Microsoft.Extensions.Logging.LoggerFactory();
            //var logger = new Microsoft.Extensions.Logging.Logger<Rest.RestClient>(factory);

            //Rest.RestClient rc = new Rest.RestClient(logger);
            //rc.GetAsync(url,"",);
            try
            {
                if(NoRequest)
                {
                    return DeserializeObject<T>("");
                }

                var handler = new HttpClientHandler()
                {
                    AllowAutoRedirect = true,
                    MaxAutomaticRedirections = 100,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                var client = new HttpClient(handler);

                client.MaxResponseContentBufferSize = 256000;

                client.BaseAddress = new Uri(BaseUrl);
                
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));
                string response = client.GetStringAsync(url).Result;
                T data = DeserializeObject<T>(response);
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }

            // var json = wc.DownloadString(url);


        }

        public async Task<T> Post<T>(string url, string jsonSerializeObject)
        {

            try
            {
                if (NoRequest)
                {
                    return DeserializeObject<T>("");
                }
                var handler = new HttpClientHandler()
                {
                    AllowAutoRedirect = true,
                    MaxAutomaticRedirections = 100,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                var client = new HttpClient(handler);

                client.MaxResponseContentBufferSize = 256000;

                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));
                var content = new StringContent(jsonSerializeObject.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response =  client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync();

                    T data = DeserializeObject<T>(str);
                    return data;
                }
                else
                {
                    return DeserializeObject<T>("");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string SerializeObject<T>(T obj)
        {
            var content = JsonConvert.SerializeObject(obj);
            return content;
        }

        public T DeserializeObject<T>(string obj)
        {
            var items = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(obj);
            return items;

        }
    }
}
