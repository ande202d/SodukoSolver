using System;

namespace SodukoSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Going to worker");

            //Worker w = new Worker();
            //w.Start();

            WorkerV2 w2 = new WorkerV2(WorkerV2.Difficulty.Hard);
            w2.Start();

            Console.WriteLine("program is done");
            Console.ReadKey();
        }
    }
}
