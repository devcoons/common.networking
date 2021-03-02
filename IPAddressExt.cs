using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Devcoons.Common.Networking
{
    public class IPAddressExt
    {
        private UInt32 ip;
        private UInt32 mask;

        public IPAddressExt()
        {
        }

        public void Set(string IP)
        {
            ip = IP.ParseIpToUInt32();
            mask = IP.ParseSingleIPv4Address().GetSubnetMask().ToString().ParseIpToUInt32();
        }
        public UInt32 NumberOfHosts
        {
            get { return ~mask + 1; }
        }

        public UInt32 NetworkAddress
        {
            get { return ip & mask; }
        }

        public UInt32 BroadcastAddress
        {
            get { return NetworkAddress + ~mask; }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }
    }
}
