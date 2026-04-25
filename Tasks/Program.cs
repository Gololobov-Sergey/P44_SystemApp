using System.Numerics;

namespace Tasks
{
    internal class Program
    {

        static int Mult(int a, int b) => a * b;

        async static ValueTask<int> MultAsync(int a, int b)
        {
            await Task.Delay(3000);
            return a * b;
        }

        static void Print()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Hello Task");
        }

        static async Task PrintString(string message)
        {
            await Task.Delay(3000);
            Console.WriteLine(message);
        }


        async static Task PrintAsync()
        {
            await Task.Delay(3000);
            Console.WriteLine("Start PrintAsync");
            await Task.Run(Print);
            Console.WriteLine("End PrintAsync");
        }



        static async Task Main(string[] args)
        {
            //Task t1 = Task.Run(() => {
            //    Console.WriteLine("Hello Task");
            //});

            //t1.Wait();


            //Task t2 = new Task(() => {
            //    Console.WriteLine("Hello Task 2");
            //});
            //t2.Start();


            //Task t3 = Task.Factory.StartNew(() => {
            //    Console.WriteLine("Hello Task 3");
            //});

            ////
            ////

            //t3.Wait();


            //int a = 4, b = 5;

            //Task<int> t4 = Task.Run(() => Mult(a, b));
            ////
            ////
            //Console.WriteLine(t4.Result);



            //await PrintAsync();

            //var t1 = PrintString("Hello Task 1");
            //var t2 = PrintString("Hello Task 2");
            //var t3 = PrintString("Hello Task 3");

            //Func<String, Task> printStringFunc = async (message) =>
            //{
            //    await Task.Delay(3000);
            //    Console.WriteLine(message);
            //};

            //var t4 = printStringFunc("Hello Task 4");

            //await t1;
            //await t2;
            //await t3;
            //await t4;


            Console.WriteLine("Start main");

            var m1 = MultAsync(4, 5);
            var m2 = MultAsync(6, 7);
            //
            //
            int n1 = await m1;
            int n2 = await m2;

            Console.WriteLine(n1);
            Console.WriteLine(n2);


            BigInteger bigInteger = BigInteger.Pow(2, 1000);



            Console.WriteLine("End main");

        }
    }
}
