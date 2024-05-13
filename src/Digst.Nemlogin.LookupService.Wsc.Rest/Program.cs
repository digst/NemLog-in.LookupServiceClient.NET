using System;
using Digst.Nemlogin.LookupService.Shared;

namespace Digst.Nemlogin.LookupService.Wsc.Rest
{
    public static class Program
    {
        private static WscCertificates _wscCertificates;
        private static WscConfig _wscConfig;

        public static void Main(string[] args)
        {
            // Config implicitly loads certificate
            _wscCertificates = new WscCertificates();
            _wscConfig = new WscConfig();

            // Create rest client from config
            // OioIdwsClient rest client from nuget abstract away the call to sts,
            // exchanging assertion with access token (AS Service) and then calling the WSP with the access token.
            var client = _wscConfig.CreateOioIdwsClient(_wscCertificates);
            
            var pid = client.Lookup(Request.CprPid(_wscConfig, "0101790067")).GetAwaiter().GetResult();
            Console.Out.WriteLine("pid from cpr:" + pid);
            
            var cpr = client.Lookup(Request.PidCpr(_wscConfig, "9208-2002-2-011208511892")).GetAwaiter().GetResult();
            Console.Out.WriteLine("cpr from pid:" + cpr);
            
            cpr = client.Lookup(Request.RidCpr(_wscConfig, "44160315","7291848958")).GetAwaiter().GetResult();
            Console.Out.WriteLine("cpr from cvr,rid:" + cpr);
            
            var rid = client.Lookup(Request.SubjectSerialNumberRid(_wscConfig, "UI:DK-E:G:4bb8cb25-22b1-4760-b7ce-e5c83142a98e")).GetAwaiter().GetResult();
            Console.Out.WriteLine("rid from ssn:" + rid);
            
            var cprUuid = client.Lookup(Request.SubjectSerialNumberCprUuid(_wscConfig, "UI:DK-E:G:4bb8cb25-22b1-4760-b7ce-e5c83142a98e")).GetAwaiter().GetResult();
            Console.Out.WriteLine("cpruuid from ssn:" + cprUuid);
            
            cpr = client.Lookup(Request.SubjectSerialNumberCpr(_wscConfig, "UI:DK-E:G:4bb8cb25-22b1-4760-b7ce-e5c83142a98e")).GetAwaiter().GetResult();
            Console.Out.WriteLine("cpr from ssn:" + cpr);
            
            var match = client.Lookup(Request.PidMatchesCpr(_wscConfig, "9208-2002-2-011208511892", "0101790063")).GetAwaiter().GetResult();
            Console.Out.WriteLine("pidmatchescpr:" + match);

        }
    }
}
