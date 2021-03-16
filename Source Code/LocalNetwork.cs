using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Devcoons.Common.Networking
{
    public static class LocalNetwork
    {
        public static IEnumerable<string> Hosts(IPAddressExt address)
        {
            for (var host = address.NetworkAddress + 1; host < address.BroadcastAddress; host++)
            {
                yield return host.ToIpString();
            }
        }

        public async static Task<IEnumerable<string>> FindAliveHosts(IPAddressExt address)
        {
            return await Task.Run(() =>
            {
                List<string> activeHosts = new List<string>();
                List<string> hosts = LocalNetwork.Hosts(address).ToList();

                Parallel.ForEach(hosts, host =>
                {
                    Ping pingSender = new Ping();

                    PingReply reply = pingSender.Send(host, 1500);
                    if (reply.Status == IPStatus.Success)                
                        activeHosts.Add(reply.Address.ToString().GetHostName() + ":" + reply.Address.ToString()); 
                });

                return activeHosts;
            });
        }
    }
}
