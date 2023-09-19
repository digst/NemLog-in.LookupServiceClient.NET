using System.Net.Http;
using System.Threading.Tasks;
using Digst.OioIdws.Rest.Client;

namespace Digst.Nemlogin.LookupService.Wsc.Rest
{
    public static class OioIdwsClientExtensions
    {
        public static async Task<string> Lookup(this OioIdwsClient idwsClient, string requestEndpoint)
        {
            // using the message handler from idws nuget that abstracts away the sts and access token flows
            var httpClient = new HttpClient(idwsClient.CreateMessageHandler());
            var response = await httpClient.PostAsync(requestEndpoint, new StringContent(""));
            return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        }
    }
}