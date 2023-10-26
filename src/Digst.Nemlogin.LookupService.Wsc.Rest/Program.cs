using System;
using Digst.Nemlogin.LookupService.Shared;

namespace Digst.Nemlogin.LookupService.Wsc.Rest
{
    public class Program
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
            
            var pid = client.Lookup(CprPid("0101790067")).GetAwaiter().GetResult();
            Console.Out.WriteLine("pid:" + pid);
            
            var cpr = client.Lookup(PidCpr("9208-2002-2-011208511892")).GetAwaiter().GetResult();
            Console.Out.WriteLine("cpr:" + cpr);

            cpr = client.Lookup(RidCpr("80123565","3A90552700604948885401F938664CAC")).GetAwaiter().GetResult();
            Console.Out.WriteLine("cpr:" + cpr);

            var rid = client.Lookup(SubjectSerialNumberRid("UI:DK-E:G:3A905527-0060-4948-8854-01F938664CAC")).GetAwaiter().GetResult();
            Console.Out.WriteLine("rid:" + rid);
            
            var cprUuid = client.Lookup(SubjectSerialNumberCprUuid("UI:DK-E:G:3A905527-0060-4948-8854-01F938664CAC")).GetAwaiter().GetResult();
            Console.Out.WriteLine("cpr:" + cprUuid);
            
            cpr = client.Lookup(SubjectSerialNumberCpr("UI:DK-E:G:9fc35481-9aec-48d0-9af5-8c9d98ae325d")).GetAwaiter().GetResult();
            Console.Out.WriteLine("cpr:" + cpr);

            var match = client.Lookup(PidMatchesCpr("9208-2002-2-011208511892", "1904481382")).GetAwaiter().GetResult();
            Console.Out.WriteLine("pidmatchescpr:" + match);
        }

        private static string CprPid(string cpr)
        {
            return _wscConfig.BaseUrl + $"/api/lookup/cprpid?cpr={cpr}";
        }

        private static string PidCpr(string pid)
        {
            return _wscConfig.BaseUrl + $"/api/lookup/pidcpr?pid={pid}";
        }

        private static string RidCpr(string cvr, string rid)
        {
            return _wscConfig.BaseUrl + $"/api/lookup/ridcpr?cvr={cvr}&rid={rid}";
        }

        private static string SubjectSerialNumberRid(string ssn)
        {
            return _wscConfig.BaseUrl + $"/api/lookup/subjectserialnumberrid?subjectserialnumber={ssn}";
        }

        private static string SubjectSerialNumberCprUuid(string ssn)
        {
            return _wscConfig.BaseUrl + $"/api/lookup/subjectserialnumbercpruuid?subjectserialnumber={ssn}";
        }
        
        private static string SubjectSerialNumberCpr(string ssn)
        {
            return _wscConfig.BaseUrl + $"/api/lookup/subjectserialnumbercpr?subjectserialnumber={ssn}";
        }

        private static string PidMatchesCpr(string pid, string cpr)
        {
            return _wscConfig.BaseUrl + $"/api/lookup/pidmatchescpr?pid={pid}&cpr={cpr}";
        }
    }
}
