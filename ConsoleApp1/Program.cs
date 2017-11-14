using System;
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
        static void Main(string[] args)
        {
            AwaitAsync(10000);
            //FuncTest(NonAwaitAsync, 10000);
            Console.WriteLine("start main");
            Console.ReadLine();
        }
    }
}
