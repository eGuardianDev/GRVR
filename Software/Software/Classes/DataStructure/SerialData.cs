using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Classes.DataStructure
{
    public enum Type
    { 
        Sensor = 0,
        Controller = 1
    }

    public struct SerialData
    {
        public int id;
        public Type type;
        public double x1;
        public double y1;
        public double z1;
        public double x2;
        public double y2;
        public double z2;

        public double XAxis;
        public double YAxis;
        public int trigger;
        public int button1;
        public int button2;
    }
}
