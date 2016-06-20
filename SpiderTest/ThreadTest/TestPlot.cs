using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    public class TestPlot
    {
        public void ThreadTest()
        {
            // 计时器
            System.Threading.Timer clock = new System.Threading.Timer((p => { Console.WriteLine(""); }), "", 10, 10);

            // 可传参
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart((p) => { Console.WriteLine(""); }));
            t.Start(null);

            System.Threading.Thread.Sleep(1000);// 在未来多少毫秒内此线程不参与cpu竞争。



            //尽量少的创建线程并且能将线程反复利用(线程池初始化时没有线程，有程序请求线程则创建线程)；
            //最好不要销毁而是挂起线程达到避免性能损失(线程池创建的线程完成任务后以挂起状态回到线程池中，等待下次请求)；
            //通过一个技术达到让应用程序一个个执行工作，类似于一个队列(多个应用程序请求线程池，线程池会将各个应用程序排队处理)；
            //如果某一线程长时间挂起而不工作的话，需要彻底销毁并且释放资源(线程池自动监控长时间不工作的线程，自动销毁)；
            //如果线程不够用的话能够创建线程，并且用户可以自己定制最大线程创建的数量(当队列过长，线程池里的线程不够用时，线程池不会坐视不理)；

            // 一个工作者线程，一个io线程
            System.Threading.ThreadPool.SetMaxThreads(1, 1);

            //System.Threading.WaitHandle[] waitHandleList = new System.Threading.WaitHandle[] { System.Threading.ThreadPool.QueueUserWorkItem };
            //System.Threading.WaitHandle.WaitAll();

            System.Threading.ThreadPool.QueueUserWorkItem((p) => { Console.WriteLine(""); }, null);// 启动线程池里得一个线程(队列的方式，如线程池暂时没空闲线程，则进入队列排队)

            // 通过委托开启线程
            DeleteTest TestHandeler = new DeleteTest((p) => { return ""; });
            IAsyncResult result = TestHandeler.BeginInvoke("(～￣▽￣～)", (p) => { Console.WriteLine(""); }, null);// 当线程完成后随即调用第二个参数回掉函数

            //比上个例子，只是利用多了一个IsCompleted属性，来判断异步线程是否完成
            while (!result.IsCompleted)
            {
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("异步线程还没完成，主线程干其他事!");
            }

            // 获得结果
            string data = TestHandeler.EndInvoke(result);
        }

        // 等线程池里的线程执行完了再执行主线程的方法
        public void testThreads()
        {
            System.Threading.ManualResetEvent[] _ManualEvents = new System.Threading.ManualResetEvent[10];
            for (int i = 0; i < 10; i++)
            {
                _ManualEvents[i] = new System.Threading.ManualResetEvent(false);
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(testMethod), _ManualEvents[i]);
            }
            System.Threading.WaitHandle.WaitAll(_ManualEvents);
            // 线程结束后执行后面的主线程代码 
            Console.WriteLine("结束了");
            Console.ReadLine();
        }
        public void testMethod(object objEvent)
        {
            //TODO: Add your code here
            System.Threading.ManualResetEvent e = (System.Threading.ManualResetEvent)objEvent;
            e.Set();

            Queue<int> q = new Queue<int>();// 队列 先进先出
            q.Enqueue(1);// 进队
            q.Dequeue();// 出队并移除
            q.Peek();// 出队不移除
            q.TrimExcess();// 设置队列大小为实际大小
        }

        delegate string DeleteTest(string param);
    }
}
