namespace ThreadApp1
{
    internal class Program
    {

        static void Print()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"2 Thread {Thread.CurrentThread.ManagedThreadId} is running");
                //Thread.Sleep(400);
            }
        }


        static void Print2(object message)
        {
            if(message is Person p)
            {
                Console.WriteLine($"Name: {p.Name}, Age: {p.Age}");
            }
            else
            {
                Console.WriteLine("No Person");
            }
        }


        static void PrintNumber(int start, int end)
        {
            Console.WriteLine($"Start {Thread.CurrentThread.Name}");
            for (int i = start; i < end; i++)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
            Console.WriteLine($"End {Thread.CurrentThread.Name}");
        }

        static void Avg(List<int> list)
        {
            Console.WriteLine($"Avg: {list.Average()}");
        }

        static void Min(List<int> list)
        {
            Console.WriteLine($"Min: {list.Min()}");
        }

        static void Max(List<int> list)
        {
            Console.WriteLine($"Max: {list.Max()}");
        }

        public record Person(string Name, int Age);

        public record Rec(List<int> param1, double param2);

        static void Main(string[] args)
        {
            Console.WriteLine("Start Main Thread");
            //Thread t = Thread.CurrentThread;
            //t.Priority = ThreadPriority.Highest;
            //Console.WriteLine(t.Name);
            //t.Name = "Main Thread";
            //Console.WriteLine(t.Name);
            //Console.WriteLine(t.IsAlive);
            //Console.WriteLine(t.ManagedThreadId);
            //Console.WriteLine(t.Priority);
            //Console.WriteLine(t.ThreadState);


            //Thread t1 = new Thread(new ThreadStart(Print));
            ////t1.Priority = ThreadPriority.Highest;
            //t1.Start();


            //Person p = new Person("John", 30);


            //Thread t1 = new Thread(new ParameterizedThreadStart(Print2));
            //Thread t2 = new Thread(Print2);
            //Thread t3 = new Thread(message => Console.WriteLine(message));


            //t1.Start(p);
            //Thread.Sleep(100);
            //t2.Start("Hello from thread 2");
            //Thread.Sleep(100);
            //t3.Start("Hello from thread 3");


            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine($"1 Thread {Thread.CurrentThread.ManagedThreadId} is running");
            //    //Thread.Sleep(400);
            //}

            //t1.Join();
            //Console.WriteLine("End Main");

            //int a = Convert.ToInt32(Console.ReadLine());
            //int b = Convert.ToInt32(Console.ReadLine());

            //int t = Convert.ToInt32(Console.ReadLine());

            //Thread t1 = new Thread(() => PrintNumber(a, b));
            ////Thread t1 = new Thread(() => {
            ////    Console.WriteLine(Thread.CurrentThread.Name);
            ////    for (int i = 0; i < 20; i++)
            ////    {
            ////        Console.Write($"{i} ");
            ////    }
            ////    Console.WriteLine();
            ////    Console.WriteLine($"End {Thread.CurrentThread.Name}");
            ////});
            //t1.Name = "Thread for Number";
            //t1.Start();

            List<Thread> threads = new List<Thread>();

            //for (int i = 0; i < t; i++)
            //{
            //    threads.Add(new Thread(() => PrintNumber(a, b)));
            //    threads[i].Name = $"Thread {i} for Number";
            //    threads[i].Start();
            //}

            List<int> numbers = [];
            Random random = new Random();
            for (int i = 0; i < 10000; i++)
            {
                numbers.Add(random.Next(1, 10000));
            }   

            threads.Add(new Thread(() => Avg(numbers)));
            threads.Add(new Thread(() => Min(numbers)));
            threads.Add(new Thread(() => Max(numbers)));
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Start();
            }


            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }
            Console.WriteLine("End Main");

        }
    }
}
