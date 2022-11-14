using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Classes
{
    internal class Station
    {
        // SerialPort Comms;
        private bool isStationOnline;
        public bool IsStationOnline { get { return this.isStationOnline; } private set { isStationOnline = value; } }

        public List<Sensor> Sensors;
        public Station()
        {
            this.Sensors = new List<Sensor>();
        }
        public int Connect()
        {
            // Todo:
            // connect
            // ask for sensors
            // initilize
            // ask to send data
            // start the sending loop

            return 0;
        }
        public int Loop()
        {
            //read from station
            int id = 0;
            //save data to sensor
            if(!Sensors.Any(s => s.ID == id))
            {
                AddSensor(id);
            }

            return 0;
        }
        public int AddSensor(int id)
        {
            //Error handle
            if (id < 0)
            {
                Logger.Error("Cannot initilize sensor with negative id. || Check for sensor firmware corruption or communcation problems.");
                return 1;
                //throw new ArgumentException("Sensor id cannot be negative");
            }


            Sensors.Add(new Sensor(id));

            return 0;
        }

    }
}
