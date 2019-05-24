using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXHSerialPort
{
    class Timeout
    {
        // 设定超时间隔为1000ms
        private readonly int TimeoutInterval = 2;
        // lastTicks 用于存储新建操作开始时的时间
        public long lastTicks;
        // 用于存储操作消耗的时间
        public long elapsedTicks;
        public Timeout()
        {
            lastTicks = DateTime.Now.Ticks;
        }
        public bool IsTimeout()
        {
            elapsedTicks = DateTime.Now.Ticks - lastTicks;
            TimeSpan span = new TimeSpan(elapsedTicks);
            double diff = span.TotalSeconds;
            if (diff > TimeoutInterval)
                return true;
            else
                return false;
        }
    }
}
