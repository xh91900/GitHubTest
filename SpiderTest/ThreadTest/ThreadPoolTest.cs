using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTest
{
    public class ThreadPoolTest
    {
        readonly int count = 5;
        public void Test()
        {
            // 第一个参数是通过将一些回调函数放入线程池中让其形成队列，第二个参数是执行带有参数的回调函数时，该参数会将引用传入，回调方法中，供其使用
            ThreadPool.QueueUserWorkItem((p) => { Console.Write(p); }, "shit");

            ManualResetEvent[] events = new ManualResetEvent[count];
            Console.WriteLine("当前主线程id:{0}", Thread.CurrentThread.ManagedThreadId);
            //循环每个任务
            for (int i = 0; i < count; i++)
            {
                //实例化同步工具
                events[i] = new ManualResetEvent(false);
                //Test在这里就是任务类，将同步工具的引用传入能保证共享区内每次只有一个线程进入
                Test tst = new Test(events[i]);
                Thread.Sleep(1000);
                //将任务放入线程池中，让线程池中的线程执行该任务
                ThreadPool.QueueUserWorkItem(tst.DisplayNumber, new { num1 = 2 });
            }
            //注意这里，设定WaitAll是为了阻塞调用线程（主线程），让其余线程先执行完毕，
            //其中每个任务完成后调用其set()方法(收到信号),当所有
            //的任务都收到信号后，执行完毕，将控制权再次交回调用线程（这里的主线程）
            ManualResetEvent.WaitAll(events);
            Console.WriteLine("当前主线程id:{0}-{1}", Thread.CurrentThread.ManagedThreadId,"我调用线程又回来了");
            Console.ReadKey();

        }
    }

    public class Test
    {
        ManualResetEvent manualEvent;
        public Test(ManualResetEvent manualEvent)
        {
            this.manualEvent = manualEvent;
        }
        public void DisplayNumber(object a)
        {
            Console.WriteLine("当前运算结果:{0}", ((dynamic)a).num1);
            Console.WriteLine("当前子线程id:{0} 的状态:{1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.ThreadState);
            //这里是方法执行时间的模拟，如果注释该行代码，就能看出线程池的功能了,根据函数执行花费的时间创建相应数量的线程
            //Thread.Sleep(30000);
            //这里是释放共享锁，让其他线程进入
            manualEvent.Set();
        }
    }
}
