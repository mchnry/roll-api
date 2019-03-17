﻿using Mchnry.Core.Encryption;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace roll_api.Infrastructure.Security
{
    public class MachineStoreKeyProvider : IRSAKeyProvider
    {
        private X509FindType findBy;
        private string certTP;

        public MachineStoreKeyProvider(X509FindType findBy, string certTP)
        {
            this.findBy = findBy;
            this.certTP = certTP;
        }

        public RSA GetKey()
        {

            X509Certificate2 cert = null;


            if (cert == null)
            {
                //Azure puts cert in my, currentuser
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certs = store.Certificates.Find(findBy
                    , certTP, false);


                if (certs.Count > 0)
                {
                    cert = certs[0];
                }
                store.Close();
            }

            return (RSA)cert.PrivateKey;

        }
    }
}
