using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Semaphor
{
    class Reader
    {
        static Semaphore semaphor = new Semaphore(3, 3);

        Thread thread;

        public Reader(int i)
        {
            thread = new Thread(Read);
            thread.IsBackground = true;
            thread.Name = $"Reader {i}";
            thread.Start();
        }

        public void Read()
        {
            semaphor.WaitOne();
            Console.WriteLine($"{thread.Name} enter library");
            Console.WriteLine($"{thread.Name} read book");
            Thread.Sleep(1000);
            Console.WriteLine($"{thread.Name} exit library");
            semaphor.Release();
        }
    }
}
