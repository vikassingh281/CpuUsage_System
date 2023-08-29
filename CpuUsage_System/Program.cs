using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpuUsage_System
{
    class Program
    {
        private static bool _displayHeader = true;
        private static PerformanceCounter _pCounter = null;
        private static readonly object _opsMutex = new object();
        private static readonly ConcurrentBag<double> _cpuValBag = new ConcurrentBag<double>();

        static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to start");
            Console.WriteLine(string.Format("{0} PerformanceCounter instance creation starts....!!", DateTime.Now));
            _pCounter = new PerformanceCounter("Processor Information", "% Processor Utility", "_Total", true);
            Console.WriteLine(string.Format("{0} PerformanceCounter instance creation end....!!", DateTime.Now));
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            _pCounter.BeginInit();
            Console.WriteLine(string.Format("{0} PerformanceCounter.BeginInit end....!!", DateTime.Now));
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            TimeSpan timeSpan = TimeSpan.FromSeconds(5);
            Console.WriteLine(string.Format(" Cpu sample(s) will be collected at every {0}", timeSpan));
            Timer timer = new Timer();
            timer.Tick += new EventHandler(CpuUsageStartTimer);
            timer.Interval = Convert.ToInt32(timeSpan.TotalMilliseconds);
            timer.Enabled = true;
            Application.Run();
        }

        private static void CpuUsageStartTimer(object sender, EventArgs e)
        {
            lock (_opsMutex)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (_displayHeader)
                    {
                        Console.WriteLine("DateTime             | CpuValue");
                        _displayHeader = false;
                    }
                    double num = Math.Round(_pCounter.NextValue(), 0);
                    if (num >= 100)
                    {
                        num = 100;
                        Console.ForegroundColor = ConsoleColor.Red;
                        _cpuValBag.Add(num);
                    }
                    else if (num < 0)
                    {
                        num = 0;
                    }
                    Console.WriteLine(string.Format("{0} | {1}", DateTime.Now, num));
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("{0}", ex));
                }
                finally
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
