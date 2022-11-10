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
    internal class Controller
    {

        private bool isStationOnline;

        public List<Sensor> Sensors;
        
        public bool IsStationOnline { get { return this.isStationOnline; } private set { isStationOnline = value; } }

        public Controller()
        {
            this.Sensors = new List<Sensor>();
         //   Dispatcher.UIThread.Post(() => LongRunningTask(), DispatcherPriority.Background);
        }
 
        //infinite loop; main logic will be here
        public void Loop()
        {
            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("data");
            }
        }

        //initialize communication with station.
        public int InitializeStation()
        {
            //communicate serial

            
            return 0;
        }

        public int AddSensor(int id)
        {
            if(id < 0)
            {
                throw new ArgumentException("Sensor id cannot be negative");
            }

            Sensors.Add(new Sensor(id));

            return 0;
        }

    }
}
