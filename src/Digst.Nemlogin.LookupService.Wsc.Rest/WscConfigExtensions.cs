using System;
using Digst.Nemlogin.LookupService.Shared;
using Digst.OioIdws.Rest.Client;

namespace Digst.Nemlogin.LookupService.Wsc.Rest
{
    public static class WscConfigExtensions
    {
        public static OioIdwsClient CreateOioIdwsClient(this WscConfig wscConfig, WscCertificates wscCertificates)
        {
            var settings = new OioIdwsClientSettings
            {
                ClientCertificate = wscCertificates.WscClientCertificate,
                AudienceUri = new Uri(wscConfig.AudienceUri),
                AccessTokenIssuerEndpoint = new Uri(wscConfig.AsEndpoint),
                SecurityTokenService = new OioIdwsStsSettings
                {
                    Certificate = wscCertificates.StsCertificate,
                    EndpointAddress = new Uri(wscConfig.StsEndpointAddress),
                    TokenLifeTime = TimeSpan.FromSeconds(wscConfig.TokenLifetimeInSeconds)
                }
            };
            return new OioIdwsClient(settings);
        }
    }
}