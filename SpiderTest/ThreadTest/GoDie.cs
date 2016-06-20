using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTest
{
    class GoDie
    {
        static void Main()
        {
            //Thread thread1 = new Thread(new InfrastructureTest().TestInterrupt);
            //thread1.Name = "Thread1";
            //thread1.Start();
            //Thread.Sleep(1000);
            //thread1.Interrupt();
            //thread1.Join();
            //Console.WriteLine("finally语句块后，线程{0}主线程的状态:{1}", thread1.Name, thread1.ThreadState);
            //Console.ReadKey();

            var test = new ThreadPoolTest();
            test.Test();
        }
    }
}
