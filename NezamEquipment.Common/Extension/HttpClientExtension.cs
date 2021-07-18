using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NezamEquipment.Common.Extension
{
    public static class HttpClientExtension
    {
        public static async Task<HttpClientResult> SendAndReadAsync(HttpMethod httpMethod, string url, IDictionary<string, string> headers = null,
            string username = null, string password = null)
        {
            var result = new HttpClientResult();

            var client = new HttpClient() { MaxResponseContentBufferSize = 256000 };

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
            }


            try
            {
                var uri = new Uri(url);

                var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);

                if (headers == null || !headers.Any())
                {
                    headers = new Dictionary<string, string>();
                }

                foreach (var header in headers)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }

                var response = await client.SendAsync(httpRequestMessage);
                //if (response.IsSuccessStatusCode)
                //{
                //}

                result.StatusCode = response.StatusCode;
                result.ResponseMessage = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            result.StatusCode = HttpStatusCode.ExpectationFailed;
            return result;
        }

        public static HttpClientResult SendAndRead(HttpMethod httpMethod, string url, IDictionary<string, string> headers = null,
            string username = null, string password = null)
        {
            var client = new HttpClient() { MaxResponseContentBufferSize = 256000 };

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
            }

            var result = new HttpClientResult();

            try
            {
                var uri = new Uri(url);

                var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);

                if (headers == null || !headers.Any())
                {
                    headers = new Dictionary<string, string>();
                }

                foreach (var header in headers)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }

                var response = client.SendAsync(httpRequestMessage).Result;
                //if (response.IsSuccessStatusCode)
                //{
                //}

                result.StatusCode = response.StatusCode;
                result.ResponseMessage = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            result.StatusCode = HttpStatusCode.ExpectationFailed;
            return result;
        }
    }

    public class HttpClientResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ResponseMessage { get; set; }
    }
}
