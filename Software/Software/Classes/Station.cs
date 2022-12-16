using Avalonia.Collections;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Software.Classes
{
    public class Station
    {
        SerialPort Comms;
        Controller control;
        public List<Sensor> Sensors;


        private string communicationPort; //usb port
        public string CommunicationPort
        {
            get { return this.communicationPort; }
            set { this.communicationPort = value; }

        }
        
        private int speed;
        
        public bool AreSensorsReady
        {
            get
            {
                if (Sensors.Count < 1)
                {
                    return false;
                }
                else { return true; }
            }
        }
        public bool IsStationOnline { get { return this.Comms.IsOpen; } }
      
        public Station(Controller l, string comPort, int comSpeed)
        {
            Logger.Log("New station class was created.");

            this.control = l;

            this.Sensors = new List<Sensor>();
            this.CommunicationPort = comPort;
            this.speed = comSpeed;
            Comms = new SerialPort(CommunicationPort, speed);
        }
        
        public int Connect()
        {
            Logger.Info("Attempting to connect to stations.");
            Thread t = new Thread(new ThreadStart(connectingThread));
            t.Start();
            return 0;
        }
        public int Disconnect()
        {
            Logger.Info("Disconnecting from stations.");
            Comms.Close();
            Clean();
            return 0;
        }
        public int Clean()
        {
            Comms.Close();
            return 0;
        }
        public void connectingThread()
        {
            if (IsStationOnline) { Logger.Warn("The station is already connected. Abound command"); };
            Comms = new SerialPort(communicationPort, speed);

            try
            {
                if (!IsStationOnline)
                {
                    Logger.Log("Connecting to selected station");
                    Comms.Open();
                    Thread.Sleep(100);
                }

            }
            catch (Exception)
            {

                Logger.Error("Station cannot connected on current port. Check port, cable or other program using the same port");
                Disconnect();
                return;
            }

            if (IsStationOnline)
            {
                Logger.Info("Connected to station!");
                Comms.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            }
        }
      
        //Called every time new information is received
        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(15);
            try
            {
                string data = Comms.ReadLine();
                //   Logger.Log(data);
                var splitedData = data.Split().ToArray();
                int id = 0;
                try
                {
                    id = int.Parse(splitedData[0]);

                    if (Sensors.Any(x => x.ID == id))
                    {
                        Sensor sensor = Sensors.Where(x => x.ID == id).First();
                        sensor.X = double.Parse(splitedData[1]);
                        sensor.Y = double.Parse(splitedData[2]);
                        sensor.Z = double.Parse(splitedData[3]);

                        sensor = Sensors.Where(x => x.ID == id + 1).First();
                        sensor.X = double.Parse(splitedData[4]);
                        sensor.Y = double.Parse(splitedData[5]);
                        sensor.Z = double.Parse(splitedData[6]);

                    }
                    else
                    {
                        AddSensor(id);
                        AddSensor(id + 1);
                    }
                }
                catch (Exception) { }
            }

            catch (Exception)
            {
                Logger.Warn("Reading data denied! Check if connections isn't disturbed");
                this.Clean();
            }
            // ask for sensors
            // initilize
        }
        public void AddSensor(int id)
        {
            //Error handle
            if (id < 0)
            {
                Logger.Error("Cannot initilize sensor with negative id. || Check for sensor firmware corruption or communcation problems.");
                return;
            }
            Logger.Info($"New sensor id : {id}");
            var sen = new Sensor(id);
            Sensors.Add(sen);
            control.addToUI(sen);
            return;
        }
        public Sensor GetSensor(int number)
        {

            return Sensors[number];
        }
    }
}
