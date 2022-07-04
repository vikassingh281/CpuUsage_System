using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CpuUsage_System
{
    class Program
    {
        private static PerformanceCounter _perfCounter = null;
        private static Timer _timer = null;

        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{DateTime.Now} :: Start calculating CPU Usage...");
                _perfCounter = new PerformanceCounter("Processor Information", "% Processor Utility", "_Total", true);
                _timer = new Timer();
                _timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
                _timer.Elapsed += CpuNextValue_Timer;
                _timer.Enabled = true;
                System.Windows.Forms.Application.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error :: {ex}");
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void CpuNextValue_Timer(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;
                double cpuValue = Math.Round(_perfCounter.NextValue(), 0);
                if (cpuValue <= 0)
                {
                    cpuValue = 0;
                }
                else if (cpuValue > 100)
                {
                    cpuValue = 100;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{DateTime.Now} :: CPU Value :: {cpuValue}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error :: {ex}");
            }
            finally
            {
                _timer.Enabled = true;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
