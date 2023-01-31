using Avalonia.Collections;
using Software.Classes.DataLoggin;
using Software.Classes.DataStructure;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Software.Classes.Controllers
{
    public class Station
    {
        SerialPort Comms;
        Controller control;

        Thread communicationThread;

        private string communicationPort;
        public string CommunicationPort
        {
            get { return communicationPort; }
            set { communicationPort = value; }

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
        public bool IsStationOnline { get { return Comms.IsOpen; } }



        public List<Sensor> Sensors;
        public Station(Controller cont, string comPort, int comSpeed)
        {
            Logger.Log("Station was initialized was created.");

            control = cont;

            Sensors = new List<Sensor>();
            CommunicationPort = comPort;
            speed = comSpeed;
            Comms = new SerialPort(CommunicationPort, speed);
        }
        public int Connect()
        {
            Logger.Info("Connecting to station.");
            communicationThread = new Thread(new ThreadStart(ThreadLoop));
            communicationThread.Start();
            return 0;
        }
        public int Clean()
        {
            if (Comms.IsOpen) Comms.Close();
            if (communicationThread.IsAlive) communicationThread.Join();
            return 0;
        }
        public void ThreadLoop()
        {
            if (IsStationOnline)
            {
                this.Clean();
                return;
            }

                Comms = new SerialPort(communicationPort, speed);

            try
            {
                if (!IsStationOnline)
                {
                    Comms.Open();
                }

            }
            catch (Exception e)
            {

                Logger.Error($"Error trying to connect to station {e.Message}");

                
                this.Clean();
                return;
            }

            if (!Comms.IsOpen)
            {
                Logger.Error("Cannot get data. Port is closed.");
                return;
            }


            if (IsStationOnline)
            {
                Logger.Info("Connected to station!");
                Comms.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            }
            this.Clean();
            
        }
        //Called every time new information is received
        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(15);
            try
            {
                string data = Comms.ReadLine();
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
                catch (Exception error) {
                    Logger.Error($"Something unexpected {error.Message}");
                }
            }

            catch (UnauthorizedAccessException)
            {
                Logger.Warn("Reading data denied! Check if connections isn't disturbed");
                Clean();
            }
        }
        public int AddSensor(int id)
        {
            if (id < 0)
            {
                Logger.Error("Cannot add sensor with negative id.");
                return 1;
              
            }
         
            Logger.Info($"New sensor connected [ id: {id} ]");
            var sen = new Sensor(id);
            Sensors.Add(sen);
            control.addToUI(sen);


            return 0;
        }



        public Sensor GetSensor(int number)
        {
            if(number < 0)
            {
                Logger.Error("Cannot call sensor with negative id");
                return null;
            }
            return Sensors[number];
        }
    }
}
