using System.Collections.Generic;
using Digst.Nemlogin.LookupService.Shared;

namespace Digst.Nemlogin.LookupService.Wsc.Rest
{
    public class Request
    {
        private readonly IDictionary<string, string> _parameters = new Dictionary<string, string>();
        public string Url { get; private set; }

        public IEnumerable<KeyValuePair<string, string>> Parameters => _parameters;

        private Request()
        {
        }
        
        public static Request PidCpr(WscConfig config, string pid)
        {
            return RequestFor(config, nameof(PidCpr)).WithPid(pid);
        }

        public static Request CprPid(WscConfig config, string cpr)
        {
            return RequestFor(config, nameof(CprPid)).WithCpr(cpr);
        }

        public static Request RidCpr(WscConfig config, string cvr, string rid)
        {
            return RequestFor(config, nameof(RidCpr)).WithCvr(cvr).WithRid(rid);
        }

        public static Request SubjectSerialNumberRid(WscConfig config, string ssn)
        {
            return RequestFor(config, nameof(SubjectSerialNumberRid)).WithSubjectSerialNumber(ssn);
        }

        public static Request SubjectSerialNumberCprUuid(WscConfig config, string ssn)
        {
            return RequestFor(config, nameof(SubjectSerialNumberCprUuid)).WithSubjectSerialNumber(ssn);
        }

        public static Request SubjectSerialNumberCpr(WscConfig config, string ssn)
        {
            return RequestFor(config, nameof(SubjectSerialNumberCpr)).WithSubjectSerialNumber(ssn);
        }

        public static Request PidMatchesCpr(WscConfig config, string pid, string cpr)
        {
            return RequestFor(config, nameof(PidMatchesCpr)).WithCpr(cpr).WithPid(pid);
        }
        
        private Request WithPid(string pid)
        {
            _parameters.Add("pid", pid);
            return this;
        }

        private Request WithCpr(string cpr)
        {
            _parameters.Add("cpr", cpr);
            return this;
        }

        private Request WithRid(string rid)
        {
            _parameters.Add("rid", rid);
            return this;
        }

        private Request WithCvr(string cvr)
        {
            _parameters.Add("cvr", cvr);
            return this;
        }

        private Request WithSubjectSerialNumber(string ssn)
        {
            _parameters.Add("subjectserialnumber", ssn);
            return this;
        }

        private static Request RequestFor(WscConfig wscConfig, string methodName)
        {
            return new Request { Url = wscConfig.BaseUrl + PathFor(methodName) };
        }
        
        private static string PathFor(string methodName)
        {
            return  $"/api/lookup/{methodName}".ToLower();
        }
    }
}