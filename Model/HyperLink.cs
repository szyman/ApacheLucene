using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace EDwI_lab1.Model
{
    class HyperLink
    {
        public HyperLink(Uri url, IPAddress ipBaseServer)
        {
            Url = url;
            IpAddressBaseServer = ipBaseServer;
        }

        private Uri url;
        public Uri Url 
        {
            get
            {
                return url;
            }
            set
            {
                ipAddress = null;
                url = value;
            }
        }

        public IPAddress IpAddressBaseServer { get; set; }

        private IPAddress ipAddress;
        public IPAddress IpAddress
        {
            get
            {
                if (ipAddress == null && Url != null && !string.IsNullOrEmpty(Url.Host))
                {
                    ipAddress = Dns.GetHostAddresses(Url.Host).FirstOrDefault();
                }
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }

        public bool IsAbsoluteUrl
        {
            get
            {
                return Url.IsAbsoluteUri;  
            }
        }

        public bool IsBaseServer
        {
            get
            {
                bool result = true;

                if (IsAbsoluteUrl && IpAddress != null)
                {
                    if (IpAddress.Address != IpAddressBaseServer.Address)
                    {
                        result = false;
                    }
                }

                return result;
            }
        }
    }
}
