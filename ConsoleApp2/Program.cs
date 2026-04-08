using System.Diagnostics;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Process p = Process.Start("calc.exe");

            Console.WriteLine(p.ProcessName);
            Console.WriteLine(p.MachineName);
            Console.WriteLine(p.MainModule);
            Console.WriteLine(p.Handle);
            Console.WriteLine(p.Id);

            //Thread.Sleep(5000);
            p.WaitForExit();
            Console.WriteLine(p.ExitCode);

            //Process[] processes = Process.GetProcesses();
            //Array.Sort(processes, (x, y) => string.Compare(x.ProcessName, y.ProcessName, StringComparison.OrdinalIgnoreCase));
            //foreach (Process process in processes)
            //{
            //    Console.WriteLine($"{process.ProcessName} {process.Id}");
            //}



            //var processes = Process.GetProcessesByName(p.ProcessName);
            //foreach (var process in processes)
            //{
            //    //Console.WriteLine($"{process.ProcessName} {process.Id}");
            //    process.Kill();
            //}
        }
    }
}
