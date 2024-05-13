using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Digst.OioIdws.Rest.Client;

namespace Digst.Nemlogin.LookupService.Wsc.Rest
{
    public static class OioIdwsClientExtensions
    {
        public static async Task<string> Lookup(this OioIdwsClient client, Request request)
        {
            // using the message handler from IDWS library that abstracts away the STS and access token flows
            var httpClient = new HttpClient(client.CreateMessageHandler());
            var content = new FormUrlEncodedContent(request.Parameters);
            var response = await httpClient.PostAsync(request.Url, content);
            return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        }
    }
}  