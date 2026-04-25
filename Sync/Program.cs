namespace Sync
{
    internal class Program
    {
        static Mutex mutex = new Mutex();

        static void Main(string[] args)
        {
            Console.WriteLine($"Start Main Thread");

            Thread[] thread = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                thread[i] = new Thread(Calculate);
                thread[i].Name = $"Thread #{i + 1}";
                thread[i].Start();
            }

            for (int i = 0; i < 5; i++)
            {
                thread[i].Join();
            }

            Console.WriteLine($"X = {SharedResource.X}");
            Console.WriteLine($"Y = {SharedResource.Y}");


            Console.ReadLine();
        }

        static void Calculate()
        {
            mutex.WaitOne();

            Console.WriteLine($"Start Thread - {Thread.CurrentThread.Name}");
            for (int i = 0; i < 1000000; i++)
            {
                // Interlocked

                //Interlocked.Increment(ref SharedResource.X);

                //if(SharedResource.X % 4 == 0)
                //{
                //    Interlocked.Increment(ref SharedResource.Y);
                //}


                //lock

                //lock(objLocker)
                //{
                //    SharedResource.X++;
                //    if(SharedResource.X % 4 == 0)
                //    {
                //        SharedResource.Y++;
                //    }
                //}

                // Monitor
                //Monitor.Enter(objLocker);

                //SharedResource.X++;
                //if (SharedResource.X % 4 == 0)
                //{
                //    SharedResource.Y++;
                //}

                //Monitor.Exit(objLocker);


                // Mutex

                SharedResource.X++;
                if (SharedResource.X % 4 == 0)
                {
                    SharedResource.Y++;
                }



            }
            Console.WriteLine($"Stop Thread - {Thread.CurrentThread.Name}");
            mutex.ReleaseMutex();
        }


        static object objLocker = new object();

        static public class SharedResource
        {
            public static int X = 0;
            public static int Y = 0;
        }
    }

}
}
