using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTest
{
    /// <summary>
    /// 
    /// </summary>
    public class InfrastructureTest
    {
        /// <summary>
        /// 调用join后调用线程被阻塞,调用线程就是开启子线程的那个线程
        /// </summary>
        internal void JoinTest()
        {
            Console.WriteLine("我是爷爷辈线程,子线程马上要来工作了我得准备下让个位给他。");
            Thread t1 = new Thread(
                new ThreadStart
                    (
                     () =>
                     {
                         var t2 = new Thread(() =>
                         {
                             for (int i = 0; i < 10; i++)
                             {
                                 if (i == 0)
                                     Console.WriteLine("我是孙子线层{0}, 完成计数任务后我会把工作权交换给父亲线程", Thread.CurrentThread.Name);
                                 else
                                 {
                                     Console.WriteLine("我是孙子线层{0}, 计数值:{1}", Thread.CurrentThread.Name, i);
                                 }
                                 Thread.Sleep(500);
                             }
                         });
                         t2.Name = "线程2";
                         t2.Start();
                         t2.Join();
                         Console.WriteLine("我是父亲线层{0}, 完成计数任务后我会把工作权交换给主线程", Thread.CurrentThread.Name);
                     }
                    )
                );
            t1.Name = "线程1";
            t1.Start();

            //调用join后调用线程被阻塞,调用线程就是开启子线程的那个线程
            t1.Join();
            Console.WriteLine("终于轮到爷爷辈主线程干活了");
            Console.ReadLine();
        }

        /// <summary>
        /// 销毁线程
        /// </summary>
        public void TestAbort()
        {
            try
            {
                Thread.Sleep(10000);
            }
            catch
            {
                Console.WriteLine("线程{0}接受到被释放销毁的信号", Thread.CurrentThread.Name);
                Console.WriteLine("捕获到异常时线程{0}主线程的状态:{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ThreadState);
            }
            finally
            {
                Console.WriteLine("进入finally语句块后线程{0}主线程的状态:{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ThreadState);
            }
        }

        /// <summary>
        /// 将当前的调用该方法的线程处于挂起状态，同样在调用此方法的线程上引发一个异常：ThreadInterruptedException，和Abort方法不同的是，被挂起的线程可以唤醒
        /// </summary>
        public void TestInterrupt()
        {
            try
            {
                Thread.Sleep(3000);
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine("线程{0}接受到被Interrupt的信号", Thread.CurrentThread.Name);
                Console.WriteLine("捕获到Interrupt异常时线程{0}的状态:{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ThreadState);
            }
            finally
            {
                Console.WriteLine("进入finally语句块后线程{0}的状态:{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ThreadState);
            }
        }
        
    }
}
