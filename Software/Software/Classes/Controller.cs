using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData.Binding;
using ReactiveUI;
using Software.ViewModels;
using Software.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Software.Classes
{
    public class Controller
    {
        Skeleton skeleton;      // structure of body 
        public Station station; // communication with hardware 
        MainWindowViewModel vm; // design  <- bad way of doing, but i'm too lazy to research
        API api;                // communication with other programs;
        public Controller(MainWindowViewModel vm)
        {
            this.vm = vm;
            
           
        }

        public int CreateAPI()
        {
            api = new API(8585);
            return 0;
        }
        public int CreateStation()
        {
            this.station = new Station(this, "COM6", 115200);
          
          //  while (station.Sensors.Count < 1) continue;
            
            return 0;
        }
        public int CreateSkeleton()
        {
            skeleton = new Skeleton(this);

            return 0;
        }
        public int CreateSensors()
        {
            skeleton = new Skeleton(this);

            return 0;
        }

        public void Setup()
        {
            // -- Some setup needed --
            //CreateAPI();

            CreateStation();

          //  CreateSensors();

            CreateSkeleton();

            station.Connect();
            Thread.Sleep(1000);
           skeleton.setupBone(0, station.GetSensor(0));
            skeleton.setupBone(1, station.GetSensor(1),skeleton.GetBone(0));
            // -- Loop --
            Loop();
        }
       // public void StartBackgroundProccess() {
        //Bone b = skeleton.GetBone(0);
             //       api.Message = ($"{Math.Round(b.EndPos.X, 2)} {Math.Round(b.EndPos.Y, 2)} {Math.Round(b.EndPos.Z, 2)} ");
        //     Thread t = new Thread(api.StartClient);
        //    t.IsBackground = true;
      //        t.Start();
      //  }
        public void Loop()
        {
            while (true)
            {
               if (station.AreSensorsReady)
                {
                    skeleton.Calculate();
                }

                // Logger.Log($"{station.GetSensor(0).X}");

                Bone b = skeleton.GetBone(1);
                Logger.Log($"{Math.Round(b.EndPos.X, 2)} {Math.Round(b.EndPos.Y, 2)} {Math.Round(b.EndPos.Z, 2)}  {Math.Round(b.Rot.X, 2)} {Math.Round(b.Rot.Y, 2)} {Math.Round(b.Rot.Z, 2)}".ToString());

                //   api.Message = ($"{Math.Round(b.EndPos.X, 2)} {Math.Round(b.EndPos.Y, 2)} {Math.Round(b.EndPos.Z, 2)} ");

                Thread.Sleep(15);
            }
        }


        // -- some front end communications 
        public void addToUI(Sensor s )
        {
            vm.sens.Add(s);
        }



        
    }
}
