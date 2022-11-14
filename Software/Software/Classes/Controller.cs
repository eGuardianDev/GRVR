using Avalonia.Controls;
using Avalonia.Threading;
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
            Station station = new Station();

            //   Dispatcher.UIThread.Post(() => LongRunningTask(), DispatcherPriority.Background);
        }

        //begining of the background worker
        public void Loop()
        {
            // -- Some setup --
            //Connecting to station
            station.Connect();

            // -- infinite loop --
            //main logic will be here
            while (true)
            {
               // Thread.Sleep(500);
               // vm.Doge = "asd";
               // Thread.Sleep(500);
               // Logger.Warn("Hello");
               // vm.Doge = "Worlds";

                
                //Read from station
                //start a thread ??
                station.Loop();
                
                //Do mathematics/algorithms 
            }
        }
        

        public int GetDataFromSensor(int id)
        {
            return 0;
        }

        //initialize communication with station.


        //Add sensor to the local veriables
        
    }
}
