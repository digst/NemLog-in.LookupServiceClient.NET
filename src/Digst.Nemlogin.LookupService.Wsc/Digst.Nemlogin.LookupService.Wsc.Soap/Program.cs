using System;
using Digst.Nemlogin.LookupService.Shared;
using Digst.OioIdws.Wsc.OioWsTrust;
using Lookup;

namespace Digst.Nemlogin.LookupService.Wsc.Soap
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
            var configuration = _wscConfig.CreateOioIdwsConfiguration(_wscCertificates);

            // Retrieve token from sts
            var stsTokenServiceConfiguration = TokenServiceConfigurationFactory.CreateConfiguration(configuration);
            var stsTokenService = new OioIdws.OioWsTrust.StsTokenServiceCache(stsTokenServiceConfiguration);
            var securityToken = stsTokenService.GetToken();

            // Create client with sts token
            var channelFactory = new CustomChannelFactory(configuration.CurrentConfiguration);
            channelFactory.Initialize();
            var channelWithIssuedToken = channelFactory.CreateChannelWithIssuedToken(securityToken);
            
            // Perform lookup
            var pidResponse = channelWithIssuedToken.CprPid(new CprPidRequest("0101790067"));
            Console.Out.WriteLine("pid:"+pidResponse?.CprPidResult?.Pid);
        }
    }
}
