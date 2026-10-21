using System;
using System.Threading;


namespace DaysOn.Facade.Tests.Acceptance.Helper
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
