using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Digst.Nemlogin.LookupService.Shared
{
    /// <summary>
    /// Certificate configuration helper loading the three certificates
    /// </summary>
    public class WscCertificates
    {
        /// <summary>
        /// public key for validating signing from sts
        /// </summary>
        public X509Certificate2 StsCertificate { get; set; }
        /// <summary>
        /// public key for validating signing for lookup service wsp
        /// </summary>
        public X509Certificate2 WspFrontendCertificate { get; set; }
        /// <summary>
        /// private key for sts login and retrieving assertion
        ///
        /// NOTE: do NOT reuse this certificate for your own wsc entity metadata, this will make the wsc sample client fail.
        /// Create your own client certificate for your own client
        /// </summary>
        public X509Certificate2 WscClientCertificate { get; set; }

        public WscCertificates()
        {
            StsCertificate= LoadCertificateFrom(@"Certificates\NemLog-in IdP - Test.cer");
            WspFrontendCertificate = LoadCertificateFrom(@"Certificates\NemLog-in LookupService - Test.cer");
            WscClientCertificate = LoadCertificateFrom(@"Certificates\NemLog-in LookupServices.TestWSC - Test.pfx");
            ValidateOrThrow();
        }

        public void ValidateOrThrow()
        {
            if (StsCertificate == null || WspFrontendCertificate == null || WscClientCertificate == null)
                throw new Exception(
                    $"sts certificate:{StsCertificate?.Subject ?? "null"} wsc certificate:{WscClientCertificate?.Subject ?? "null"} eia frontend certificate:{WspFrontendCertificate?.Subject ?? "null"} ");
        }

        private X509Certificate2 LoadCertificateFrom(string certificatePath)
        {
            certificatePath = Path.Combine(typeof(WscCertificates).Assembly.GetAssemblyDirectory(), certificatePath);
            if (!File.Exists(certificatePath)) throw new Exception(certificatePath + " file not found");
            return LoadCertificateFrom(certificatePath, "Test1234");
        }

        private X509Certificate2 LoadCertificateFrom(string filePath, string password)
        {
            try
            {
                // Assumes the last one in the chain, is the one we want.
                return LoadCertificateChainFrom(filePath, password).LastOrDefault();
            }
            catch (global::System.Exception)
            {
                return null;
            }
        }

        private List<X509Certificate2> LoadCertificateChainFrom(string filePath, string password)
        {
            var collection = new X509Certificate2Collection();
            if (filePath.EndsWith(".cer"))
                collection.Import(filePath);
            else
                collection.Import(filePath, password, X509KeyStorageFlags.PersistKeySet);

            var certificates = new List<X509Certificate2>();
            foreach (var cert in collection) certificates.Add(cert);

            return certificates;
        }

    }
}