using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData.Binding;
using Software.ViewModels;
using Software.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Software.Classes
{
    public struct SensorData
    {
        public float x1;
        public float y1;
        public float z1;
        public float x2;
        public float y2;
        public float z2;
        public float id;
    }
    internal class Controller
    {
        Station station;
        MainWindowViewModel vm;
        public Controller(MainWindowViewModel vm)
        {
            this.vm = vm;
            this.station = new Station(this, "COM6", 115200);

        }

        //beginning of the background worker
        public void Loop()
        {
            // -- Some setup --
            //Connecting to station
            station.Connect();
           

            // -- program loop --
            while (true)
            {
                if (station.Sensors.Count > 0)
                {
                    Logger.Log(station.GetSensor(0).X.ToString());
                }
                Thread.Sleep(150);
            
             // vm.sens = station.Sensors;
                //Do mathematics/algorithms 
            }
        }

        public void addToUI(Sensor s )
        {
            vm.sens.Add(s);
        }

        //initialize communication with station.


        //Add sensor to the local veriables
        
    }
}
