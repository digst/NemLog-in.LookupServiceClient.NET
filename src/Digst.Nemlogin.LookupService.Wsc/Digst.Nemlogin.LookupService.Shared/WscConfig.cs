﻿namespace Digst.Nemlogin.LookupService.Shared
{
    /// <summary>
    /// Config in code, used in both sample clients.
    /// </summary>
    public class WscConfig
    {
        public string BaseUrl { get; set; }

        public string Domain { get; }

        public string AudienceUri { get; set; }

        public string AsEndpoint { get; set; }

        public string StsEndpointAddress { get; set; }

        public int TokenLifetimeInSeconds { get; set; }

        public WscConfig()
        {
            Domain = "test-devtest4-nemlog-in.dk";
            AudienceUri = $"https://saml.wsp.lookupservice.{Domain}";
            BaseUrl = $"https://lookupservice.{Domain}";
            AsEndpoint = $"{BaseUrl}/api/accesstoken/issue";
            StsEndpointAddress = $"https://securetokenservice.{Domain}/SecurityTokenService.svc";
            TokenLifetimeInSeconds = 7200;
        }
    }
}