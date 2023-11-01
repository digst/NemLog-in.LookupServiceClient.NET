# NemLog-in.LookupServiceClient.NET
An OIO IDWS client sample for NemLog-in Lookup Services.

## Introduction

This code shows how to invoke NemLog-in Lookup Services using the OIO IDWS authorization model. 

The services are described in section 2 of [SS].
Please refer to [SS] for detailed documentation of available services. 

Note especially, that services described in section 3 of [SS] (UUID match services) 
use a different authentication and authorization model and that this sample is not relevant
if you need to use the UUID match services.

## Solution

The solution consists of four projects:

* ```Digst.Nemlogin.LookupService.Wsc.Rest``` - a command line REST client
* ```Digst.Nemlogin.LookupService.Wsc.Soap``` - a command line SOAP client
* ```Digst.Nemlogin.LookupService.Shared``` - a library shared by both clients
* ```Digst.Nemlogin.LookupService.Test``` - a unittest project that invokes both clients


## User guide - Windows

* Open solution in your preferred IDE.
* Build the solution
* Run the client of preference - we recommend using REST, see below.

The clients will:

* Install the required certificates (included in ```Shared/certificates``` folder) in CurrentUser certificate store.
* Request a SAML token at the NemLog-in STS
* Exchange the SAML token for a JWT token (only for REST)
* Invoke the full set of business methods in the NemLog-in pre-production environment (domain ```test-devtest4-nemlog-in.dk```)

See ```WscConfig``` and ```WscCertificates```.

## Certificates

A number of certificates are included in the ```Shared/certificates``` folder. These are

<dl>
<dt>OCES3 Root CA - CTI.cer</dt>
<dd>
The test OCES3 root CA certificate. Signer of intermediate CA certificates. 
Used for establishing trust to OCES3 test certificates.
</dd>

<dt>OCES3 Intermediate CA - CTI.cer</dt>
<dd>
The OCES3 issuing CA certificate. Signer of end user test certificates. 
Used for establishing trust to OCES3 test certificates.
</dd>

<dt>NemLog-in IdP - Test</dt>
<dd>
Signing certificate for the NemLog-in pre-production STS.
Used for verifying authenticity of the SAML tokens issued by the STS.
<br>
Note that SOAP and REST clients must update this certificate in their environment when it is updated in NemLog-in.
</dd>


<dt>NemLog-in LookupServices.TestWSC - Test.pfx</dt>
<dd>Certificate used by SOAP and REST clients (WSC's) to sign the STS request.</dd>

<dt>NemLog-in LookupService - Test.cer</dt>
<dd>WS-Security signing certificate used by the Lookup Service SOAP webservice.
Used for signing SOAP responses sent to the SOAP client.
Note that this certificate is only required by SOAP clients and that SOAP clients must update
this certificate in their environment when it is updated in NemLog-in.
</dd>
</dl>

The certificate installation is included in the code for convenience. When using the code in production you should install
certificates prior to executing the client, and in that case the WscCertificates constructor should merely
construct ```X509Certificate2``` instances by looking up the certificates in the certificate store.


## References

<dl>
<dt>[SS]</dt>
<dd>NemLog-in Supporting Services documentation, available at https://tu.nemlog-in.dk/oprettelse-og-administration-af-tjenester/log-in/dokumentation.og.guides/. 
</dd>
</dl>
