using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine($"Name :: {nic.Name}");
                Console.WriteLine($"    ID :: {nic.Id}");
                Console.WriteLine($"    Status :: {nic.OperationalStatus}");
                Console.WriteLine($"    Speed :: {nic.Speed}");
            }
        }
    }
}
