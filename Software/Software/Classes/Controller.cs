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
    public class Controller
    {
        Skeleton skeleton;
        public Station station;
        MainWindowViewModel vm;
        Bone bone;
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
            while((station.Sensors.Count < 1))
            {
            }
                Logger.Log("creating skeleton");
                skeleton = new Skeleton(this); 

            // -- program loop --
            while (true)
            {
                //Calculate every bone position
                    skeleton.Calculate();
                Bone b = skeleton.bone(0);
                //Display first bone position
                Logger.Log($"{Math.Round(b.EndPos.X,2)} {Math.Round(b.EndPos.Y,2)} {Math.Round(b.EndPos.Z, 2)}  {Math.Round(b.Rot.X, 2)}".ToString());
                //Display second bone position
               // Logger.Log(Math.Round(b2.EndPos.X,2).ToString());
              

                Thread.Sleep(15);
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
