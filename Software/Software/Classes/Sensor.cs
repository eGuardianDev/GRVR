using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Classes
{
    internal class Sensor
    {
        private int id;

        private int x;
        private int y;
        private int z;

        public int X { get { return x; } private set { x = value; } }
        public int Y { get { return Y; } private set { y = value; } }
        public int Z { get { return Z; } private set { z = value; } }

        public Sensor(int id = 0)
        {
            this.id = id;
        }


    }
}
