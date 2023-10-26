using System;
using Digst.Nemlogin.LookupService.Shared;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Digst.Nemlogin.LookupService.Wsc.Soap
{
    /// <summary>
    /// Wsc soap extensions for overriding wcf config file
    ///
    /// It is a design decision that we do this bit of fluff to ensure we do all configuration for both sample clients in code
    /// When the .modified config is created you can use this as the config for your soap client, if you prefer web.config configuration. 
    /// </summary>
    public static class WscConfigExtensions
    {
        public static string StsCertificateThumbprintInConfig = "StsCertificateThumbprintInConfig";
        public static string WscCertificateThumbprintInConfig = "WscCertificateThumbprintInConfig";
        public static string WspCertificateThumbprintInConfig = "WspCertificateThumbprintInConfig";
        public static string WspCertificateName = "WspCertificateName";

        public static Digst.OioIdws.Wsc.OioWsTrust.Configuration CreateOioIdwsConfiguration(this WscConfig config, WscCertificates wscCertificates)
        {
            var originalConfigPath = Path.Combine(Assembly.GetExecutingAssembly().GetAssemblyDirectory(), $"{Assembly.GetExecutingAssembly().GetName().Name}.exe.config");
            var modifiedConfigPath = originalConfigPath + ".modified";
            var originalAppConfigContent = File.ReadAllText(originalConfigPath);
            var modifiedAppConfigContent = originalAppConfigContent.Replace("#{BaseUrl}", config.BaseUrl);
            // prod-inttest uses sts-test-nemlog-in.cer for sts signing
            modifiedAppConfigContent = modifiedAppConfigContent.Replace(WscCertificateThumbprintInConfig, wscCertificates.WscClientCertificate.Thumbprint);
            modifiedAppConfigContent = modifiedAppConfigContent.Replace(StsCertificateThumbprintInConfig, wscCertificates.StsCertificate.Thumbprint);
            modifiedAppConfigContent = modifiedAppConfigContent.Replace(WspCertificateThumbprintInConfig, wscCertificates.WspFrontendCertificate.Thumbprint);
            modifiedAppConfigContent = modifiedAppConfigContent.Replace(WspCertificateName, wscCertificates.WspFrontendCertificate.GetNameInfo(X509NameType.SimpleName, false));
            modifiedAppConfigContent = modifiedAppConfigContent.Replace("#{Domain}", config.Domain);
            File.WriteAllText(modifiedConfigPath, modifiedAppConfigContent);
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = modifiedConfigPath
            };
            return (Digst.OioIdws.Wsc.OioWsTrust.Configuration)ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None).GetSection("oioIdwsWcfConfiguration");
        }
    }
}
