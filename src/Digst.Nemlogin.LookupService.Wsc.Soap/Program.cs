using System;
using Digst.Nemlogin.LookupService.Shared;
using Digst.OioIdws.Wsc.OioWsTrust;
using Lookup;

namespace Digst.Nemlogin.LookupService.Wsc.Soap
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

            var cprResponse = channelWithIssuedToken.PidCpr(new PidCprRequest("9208-2002-2-011208511892"));
            Console.Out.WriteLine("cpr:" + cprResponse?.PidCprResult?.Cpr);
            
            var ridResponse = channelWithIssuedToken.RidCpr(new RidCprRequest("80123565", "3A90552700604948885401F938664CAC"));
            Console.Out.WriteLine("cpr:" + ridResponse?.RidCprResult?.Cpr);

            var ssnridResponse = channelWithIssuedToken.SubjectSerialNumberRid(new SubjectSerialNumberRidRequest("UI:DK-E:G:3A905527-0060-4948-8854-01F938664CAC"));
            Console.Out.WriteLine("rid:" + ssnridResponse?.SubjectSerialNumberRidResult?.Rid);

            var ssncpruuidResponse = channelWithIssuedToken.SubjectSerialNumberCprUuid(new SubjectSerialNumberCprUuidRequest("UI:DK-E:G:3A905527-0060-4948-8854-01F938664CAC"));
            Console.Out.WriteLine("cpruuid:" + ssncpruuidResponse?.SubjectSerialNumberCprUuidResult?.CprUuid);

            var ssncprResponse = channelWithIssuedToken.SubjectSerialNumberCpr(new SubjectSerialNumberCprRequest("UI:DK-E:G:3A905527-0060-4948-8854-01F938664CAC"));
            Console.Out.WriteLine("cpr:" + ssncprResponse?.SubjectSerialNumberCprResult?.Cpr);

            var pidmatchescprResponse = channelWithIssuedToken.PidMatchesCpr(new PidMatchesCprRequest("9208-2002-2-011208511892", "1904481382"));
            Console.Out.WriteLine("match:" + pidmatchescprResponse?.PidMatchesCprResult?.Result);
        }
    }
}
