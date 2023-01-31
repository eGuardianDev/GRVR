using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData.Binding;
using ReactiveUI;
using Software.Classes.DataLoggin;
using Software.Classes.DataStructure;
using Software.ViewModels;
using Software.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Software.Classes.Controllers
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
        public int InitiliazeStation()
        {
            station = new Station(this, "COM1", 115200);

            return 0;
        }
        public int InitiliazeSkeleton()
        {
            skeleton = new Skeleton(this);
           
            return 0;
        }

        public void Setup()
        {
            // -- Some setup needed --

            InitiliazeStation();

            InitiliazeSkeleton();


            Common.Delay(100);


            StartLoop();
        }

     
        public void StartLoop()
        {
            while (true)
            {
                if (station.AreSensorsReady)
                {
                    skeleton.Calculate();
                }

                //localDebug();

                Thread.Sleep(15);
            }
        }
        public void localDebug()
        {
                // Logger.Log($"{station.GetSensor(0).X}");
                Bone b = skeleton.GetBone(1);


                Logger.Log($"{Math.Round(b.EndPos.X, 2)} {Math.Round(b.EndPos.Y, 2)} {Math.Round(b.EndPos.Z, 2)}  {Math.Round(b.Rot.X, 2)} {Math.Round(b.Rot.Y, 2)} {Math.Round(b.Rot.Z, 2)}".ToString());
                //   api.Message = ($"{Math.Round(b.EndPos.X, 2)} {Math.Round(b.EndPos.Y, 2)} {Math.Round(b.EndPos.Z, 2)} ");


        }

        // -- some front end communications 
        public void addToUI(Sensor s)
        {
            vm.sens.Add(s);
        }
    }
}
