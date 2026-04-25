using System.Reflection.PortableExecutable;

namespace Semaphor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                new Reader(i + 1);
            }


            Console.ReadLine();

        }
    }
}
