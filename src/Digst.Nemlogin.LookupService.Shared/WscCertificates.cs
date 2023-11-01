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
        /// Root CA certificate
        /// </summary>
        public X509Certificate2 CA { get; set; }

        /// <summary>
        /// Intermediate CA certificate
        /// </summary>
        public X509Certificate2 CAIntermediate { get; set; }

        /// <summary>
        /// NemLog-in STS certificate.
        /// 
        /// Public key is used for for validating signatures on SAML Tokens received from the STS.
        /// </summary>
        public X509Certificate2 StsCertificate { get; set; }
        
        /// <summary>
        /// Public key for validating WS-Security signatures for Lookup Service webservice.
        ///
        /// Only required for SOAP.
        /// </summary>
        public X509Certificate2 WspFrontendCertificate { get; set; }
        
        /// <summary>
        /// Certificate and private key for WSC ("systembrugers certifikat")
        ///
        /// NOTE: do NOT reuse this certificate for your own WSC entity metadata, this will make the WSC sample client fail.
        /// Create your own client certificate for your own client.
        /// </summary>
        public X509Certificate2 WscClientCertificate { get; set; }

        public WscCertificates()
        {
            CA = LoadCertificateFrom(@"Certificates\OCES3 ROOT CA - CTI.cer");
            CAIntermediate = LoadCertificateFrom(@"Certificates\OCES3 Intermediate CA - CTI.cer");
            StsCertificate = LoadCertificateFrom(@"Certificates\NemLog-in IdP - Test.cer");
            //The WspFrontendCertificate can be set to null if only REST is used.
            WspFrontendCertificate = LoadCertificateFrom(@"Certificates\NemLog-in LookupService - Test.cer"); 
            WscClientCertificate = LoadCertificateFrom(@"Certificates\NemLog-in LookupServices.TestWSC - Test.pfx");
            ValidateOrThrow();
        }

        public void ValidateOrThrow()
        {
            if (CA == null || CAIntermediate == null)
                throw new Exception(
                    $"CA:{CA?.Subject ?? "null"} CA Intermediate:{CAIntermediate?.Subject ?? "null"}");

            ValidateInstallation(StoreName.Root, CA, "Trusted Root Certification Authorities");
            ValidateInstallation(StoreName.CertificateAuthority, CAIntermediate, "Intermediate Certification Authorities");

            if (StsCertificate == null || WscClientCertificate == null)
                throw new Exception(
                    $"sts certificate:{StsCertificate?.Subject ?? "null"} wsc certificate:{WscClientCertificate?.Subject ?? "null"} eia frontend certificate:{WspFrontendCertificate?.Subject ?? "null"} ");
        }

        private static void ValidateInstallation(StoreName storeName, X509Certificate2 certificate, string whereToInstall)
        {
            using var store = new X509Store(storeName);
            store.Open(OpenFlags.ReadWrite);
            var collection = store.Certificates.Find(X509FindType.FindByThumbprint,certificate.Thumbprint,false);
            store.Close();
            if (collection.Count == 0)
                throw new Exception(
                    $"certificate with thumbprint {certificate.Thumbprint} should be installed in {whereToInstall}");

        }

        public void Install()
        {
            ValidateOrThrow();
            var certificateList = new List<X509Certificate2> { WscClientCertificate, WspFrontendCertificate, StsCertificate };
            certificateList.ForEach(c =>
            {
                RemoveExistingCertificateFromStore(c.Thumbprint, StoreLocation.CurrentUser);
                AddCertificateToStore(c, StoreLocation.CurrentUser);
            });
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

        private static List<X509Certificate2> LoadCertificateChainFrom(string filePath, string password)
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

        private static void RemoveExistingCertificateFromStore(string thumbprint, StoreLocation location)
        {
            using var store = new X509Store(StoreName.My, location);
            store.Open(OpenFlags.ReadWrite | OpenFlags.IncludeArchived);
            var col = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            foreach (var cert in col)
            {
                Console.Out.WriteLine($"Removing certificate: {cert.Subject}");
                store.Remove(cert);
            }
            store.Close();
        }

        private static void AddCertificateToStore(X509Certificate2 certificate, StoreLocation location)
        {
            using (var store = new X509Store(StoreName.My, location))
            {
                store.Open(OpenFlags.ReadWrite);
                Console.Out.WriteLine($"Adding certificate: {certificate.Subject}");
                store.Add(certificate);
                store.Close();
            }
        }
    }
}