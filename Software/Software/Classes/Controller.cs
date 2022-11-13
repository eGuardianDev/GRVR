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
using System.IO.Ports;

namespace Software.Classes
{
    internal class Controller
    {
        SerialPort sp;
        private bool isStationOnline;

        public List<Sensor> Sensors;
        
        public bool IsStationOnline { get { return this.isStationOnline; } private set { isStationOnline = value; } }
        MainWindowViewModel vm;
        public Controller(MainWindowViewModel vm)
        {
            this.vm = vm;
            this.Sensors = new List<Sensor>();
         //   Dispatcher.UIThread.Post(() => LongRunningTask(), DispatcherPriority.Background);
        }
 
        //infinite loop; main logic will be here
        public void Loop()
        {
            while (true)
            {
                Thread.Sleep(500);
                vm.Doge = "asd";
                Thread.Sleep(500);
                Logger.Warn("Hello");
               vm.Doge = "Worlds";
            }
        }

        //initialize communication with station.
        public int InitializeStation()
        {
            // Todo:
            // connect
            // ask for sensors
            // initilize
            // ask to send data
            // start the sending loop

            return 0;
        }

        //Add sensor to the local veriables
        public int AddSensor(int id)
        {
            //Error handle
            if(id < 0)
            {
                throw new ArgumentException("Sensor id cannot be negative");
            }

            Sensors.Add(new Sensor(id));

            return 0;
        }

    }
}
