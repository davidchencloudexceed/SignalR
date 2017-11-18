using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    class Program
    {
        static async Task AwaitAsync(int n)
        {
            await Task.Delay(n);
            Console.WriteLine("start await async");

        }
        static Task<int> NonAwaitAsync(int n)
        {
            Task.Delay(n).ContinueWith(t => Console.WriteLine("non await finished"));
            Console.WriteLine("start non await async");
            return Task.FromResult(0);

        }
        static void FuncTest(Func<int, Task> myFunc, int n)
        {
            myFunc(n);
        }
        private static int i = 0;
        static void Main(string[] args)
        {
            var t = new Thread(DoWork);
            //t.IsBackground = false;
            t.Start();
            
            
            Console.WriteLine($"main thread {i++}");

            Console.WriteLine($"main thread after join {i++}");
            //AwaitAsync(10000);
            //FuncTest(NonAwaitAsync, 10000);
            //Console.WriteLine("start main");
            Console.ReadKey();
        }

        static void DoWork()
        {
            Console.WriteLine($"Other thread before sleep {i++}");
            Thread.Sleep(5000);
            Console.WriteLine($"Other thread after sleep {i++}");
        }
    }
}
