# GitHubTest
GitHub测试1
这里面主要放资料名字或者地址，温故而知新。

多线程学习
博客园地址：http://www.cnblogs.com/JimmyZheng/archive/2012/07/07/2580253.html
         :http://www.cnblogs.com/doforfuture/p/6293926.html

C#组件系列——又一款Excel处理神器Spire.XLS，你值得拥有
http://www.cnblogs.com/landeanfen/p/5888973.html

c#操作数据库和批量插入数据性能比较
http://blog.csdn.net/amandag/article/details/6393697


#region 按钮点击多次事件转换为1次
        public void MoreClickConvertOneClick(MoreClickEvent moreClick)
        {
            if (moreClick == null)
                return;

            var nowTime = DateTime.Now;
            var span = nowTime - lastClickTime;
            lastClickTime = nowTime;
            if (span.TotalMilliseconds > 300)
            {
                if (Timer != null)
                {
                    Timer.Stop();
                }
                Timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 300) };
                Timer.Tick += (s, e1) =>
                {
                    Timer.Stop();
                    moreClick();
                };
                Timer.Start();
            }
            else
            {
                Timer.Stop();
                moreClick();
            }
        }
        #endregion
