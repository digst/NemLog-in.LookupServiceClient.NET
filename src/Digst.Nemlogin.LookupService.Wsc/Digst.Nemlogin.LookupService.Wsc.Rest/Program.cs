using System;
using Digst.Nemlogin.LookupService.Shared;

namespace Digst.Nemlogin.LookupService.Wsc.Rest
{
    internal class Program
    {
        private static WscCertificates _wscCertificates;
        private static WscConfig _wscConfig;

        static void Main(string[] args)
        {
            // Config implicitly loads certificate
            _wscCertificates = new WscCertificates();
            _wscConfig = new WscConfig();

            // Create rest client from config
            // OioIdwsClient rest client from nuget abstract away the call to sts,
            // exchanging assertion with access token (AS Service) and then calling the WSP with the access token.
            var client = _wscConfig.CreateOioIdwsClient(_wscCertificates);
            var pid = client.Lookup(CprPid("0101790067")).GetAwaiter().GetResult();
            Console.Out.WriteLine("pid:" + pid);
        }

        private static string CprPid(string cpr)
        {
            return _wscConfig.BaseUrl + $"/api/lookup/cprpid?cpr={cpr}";
        }
    }
}
