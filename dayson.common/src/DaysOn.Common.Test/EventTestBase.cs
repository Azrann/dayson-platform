using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DaysOn.Common.Test
{
    public class EventTestBase
    {
        public bool WaitFor(Func<bool> exitFunction, int millisecondsMaxTimeOut, out int timeWaited)
        {
            int i = 0;
            while (i * 50 < millisecondsMaxTimeOut && !exitFunction())
            {
                Thread.Sleep(50);
                i++;
            }
            timeWaited = i * 50;
            return exitFunction();
        }
    }
}
