using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Software.Classes
{
    internal class Station
    {
         SerialPort Comms;

        private string communicationPort; //usb port
        public string CommunicationPort
        {
            get { return this.communicationPort; }
            set { this.communicationPort = value; }

        }

        private int speed;

        public bool IsStationOnline { get { return this.Comms.IsOpen; }  }

        public List<Sensor> Sensors;
        public Station(string comPort, int comSpeed)
        {
            Logger.Info("New station was created.");
            this.Sensors = new List<Sensor>();
            this.CommunicationPort = comPort;
            this.speed = comSpeed;
                Comms = new SerialPort(communicationPort, speed);
        }
        public int Connect()
        {
            Logger.Info("Connecting to station.");
            Thread t = new Thread(new ThreadStart(connectingThread));
            t.Start();
            return 0;
        }
        public int Clean()
        {
                Comms.Close();
            return 0;
        }
        public void connectingThread()
        {
            while(true)
            {
                if(IsStationOnline) continue;
                Comms = new SerialPort(communicationPort, speed);

                try
                {
                    Comms.Open();

                }
                catch (Exception )
                {
                    Logger.Error("Station isn't connected on current port. Check port or connection");
                }

                if (IsStationOnline)
                {
                    Logger.Info("Connected to station!");
                    Comms.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                }
            }

        }
        //Called every time new information is received
        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(15);
            try
            {
                string data = Comms.ReadLine();
                Logger.Info(data);

            }
            catch (UnauthorizedAccessException)
            {
                Logger.Warn("Reading data denied! Check if connections isn't disturbed");
                this.Clean();
            }
            // ask for sensors
            // initilize
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
            if (Sensors.Any(s => s.ID == id))
            {
                Logger.Warn($"Sensor with id {id} already registered in the system.");
                return 2;
            }

            Sensors.Add(new Sensor(id));

            return 0;
        }

    }
}
