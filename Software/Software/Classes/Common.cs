using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Software.Classes
{
    public static class Common
    {
        public static void Delay(int miliseconds)
        {
            Thread.Sleep(miliseconds);
        }
    }
}
