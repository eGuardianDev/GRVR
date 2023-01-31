using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Classes.DataStructure
{
    public class Sensor
    {
        private int id;

        private double x;
        private double y;
        private double z;

        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public double Z { get { return z; } set { z = value; } }
        public int ID { get { return id; } set { id = value; } }

        public Sensor(int id = 0)
        {
            this.id = id;
        }


    }
}
