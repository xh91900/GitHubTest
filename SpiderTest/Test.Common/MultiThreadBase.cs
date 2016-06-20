using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Common
{
    /// <summary>
    /// MultiThreadBase
    /// </summary>
    public abstract class MultiThreadBase<T> where T : class
    {
        // 锁对象只能是引用类型 因为只有引用类型才有那个什么我忘了
        private object locker = new object();

        // 线程数
        private int threadCount = 0;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="threadCount">线程数</param>
        public MultiThreadBase(int threadCount)
        {
            this.threadCount = threadCount;
        }

        // 待处理的数据集合
        private List<T> waitPrcessData { get; set; }

        // 已经处理完的数量
        private int completeCount = 0;

        // 待处理的数量
        private int waitProcessCount = 0;

        // 已完成委托
        public delegate void CompleteHandler();

        // 已完成事件
        public event CompleteHandler CompleteEvent;

        // 消息委托
        public delegate void MessageHandler(string message);

        // 消息事件
        public event MessageHandler MessageEvent;

        /// <summary>
        /// 开启多线程
        /// </summary>
        /// <param name="targetList">待处理的数据</param>
        public void Start(List<T> targetList)
        {
            this.waitPrcessData = targetList;
            this.waitProcessCount = targetList.Count;

            for(int i=0;i<threadCount;i++)
            {
                Thread thread = new Thread(SingleProcess);
                thread.IsBackground = true;
                thread.Name = (i + 1).ToString().PadLeft(this.threadCount.ToString().Length, '0');
                thread.Start();
            }
        }

        /// <summary>
        /// 处理单个线程
        /// </summary>
        public void SingleProcess()
        {
            while(true)
            {
                T t = default(T);

                try
                {
                    lock (locker)
                    {
                        if (waitPrcessData != null && waitPrcessData.Any())
                        {
                            t = waitPrcessData[0];
                            waitPrcessData.RemoveAt(0);
                            waitProcessCount--;
                        }
                        else
                        {
                            break;
                        }
                    }

                    this.ThreadBusiness(t);
                }
                catch (Exception ex)
                {
                    this.SendMessage(ex.ToString());
                }
            }
            lock (locker)
            {
                this.completeCount++;
                if(completeCount==this.threadCount)
                {
                    this.CompleteEvent();
                }
            }
        }

        /// <summary>
        /// 打印消息
        /// </summary>
        /// <param name="message">消息</param>
        private void SendMessage(string message)
        {
            if(this.MessageEvent!=null)
            {
                this.MessageEvent("线程" + Thread.CurrentThread.Name + "_" + message);
            }
        }

        /// <summary>
        /// 处理逻辑
        /// </summary>
        /// <param name="target">待处理对象</param>
        protected abstract void ThreadBusiness(T target);
    }
}
