using System;

namespace SodukoSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Going to worker");

            Worker w = new Worker();
            w.Start();

            Console.WriteLine("program is done");
            Console.ReadKey();
        }
    }
}
