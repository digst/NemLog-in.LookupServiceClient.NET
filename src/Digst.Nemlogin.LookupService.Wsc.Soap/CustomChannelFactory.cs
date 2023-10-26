using System;
using System.Configuration;
using System.Reflection;
using System.ServiceModel;

namespace Digst.Nemlogin.LookupService.Wsc.Soap
{
    public class CustomChannelFactory : ChannelFactory<Lookup.LookupService>
    {
        private readonly Configuration _configuration;

        public CustomChannelFactory(Configuration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize()
        {
            if (_configuration == null) return;
            var method = GetType().BaseType.BaseType.GetMethod("ApplyConfiguration",
                BindingFlags.NonPublic | BindingFlags.Instance,
                Type.DefaultBinder, new[] { typeof(string), _configuration.GetType() }, null);
            method.Invoke(this, new object[] { "config_ILookupService", _configuration });
        }
    }
}