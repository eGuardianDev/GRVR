using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData.Binding;
using ReactiveUI;
using Software.ViewModels;
using Software.Views;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
        public MainWindowViewModel vm; // design  <- bad way of doing, but i'm too lazy to research
        API api;                // communication with other programs;
        public Controller(MainWindowViewModel vm)
        {
            this.vm = vm;


        }

        public int CreateAPI()
        {
            api = new API(8585);
            return 0;
        } //generate API Coms class
        public int CreateStation()
        {
            this.station = new Station(this, "COM6", 115200);

            //  while (station.Sensors.Count < 1) continue;

            return 0;
        }//generate station com class
        public int CreateSkeleton()
        {
            skeleton = new Skeleton(this);

            return 0;
        } //generate skeleton class

        public void Setup()
        {
            // -- Some setup needed --
            //CreateAPI();

            CreateStation();


            CreateSkeleton();

            Thread.Sleep(1000);
            vm.Ports = SerialPort.GetPortNames().ToList();
            // -- Loop --
            Loop();
        }
      
        public void Loop()
        {
            while (true)
            {
                if (vm.Ports.Count != SerialPort.GetPortNames().ToList().Count)
                {
                    vm.Ports = SerialPort.GetPortNames().ToList();
                }
                if (vm.SelectedPort != null)
                {
                    station.CommunicationPort = vm.SelectedPort;
                }
                //Station is offline, but the skeleton was working
                // this means that there was a station and was setup,
                // but now it is offline and the system is reset
                if (!station.IsStationOnline && skeleton.isReady)
                {
                    Logger.Warn("Station is found offline");
                    Logger.Warn("Reseting the skeleton");
                    skeleton.Reset();
                    station.Reset();
                    vm.sens.Clear();
                }
                // If the station is connected, but the skeleton isn't ready
                // setup the skeelton

                // ! this is temporary, need to change the bones setup;
                if (!skeleton.isReady && station.AreSensorsReady)
                {
                    Logger.Log("Setting up the skeleton");
                    skeleton.setupBone(0, station.GetSensor(0), null);
                    skeleton.setupBone(1, station.GetSensor(1), skeleton.GetBone(0));
                }
                // if the skeleton is setup and there are sensors 
                if (skeleton.isReady && station.AreSensorsReady)
                {
                    //calculate
                    skeleton.Calculate();
                }

                //information display
                vm.StationStatus = $"{(station.IsStationOnline ? "Online" : "Offline")}";
                vm.StatusColor = $"{(station.IsStationOnline ? "Green" : "Red")}";





                Thread.Sleep(15);
            }
        }

        public void DisplayTestValues()
        {

            Bone b = skeleton.GetBone(0);
            Logger.Log($"{skeleton.GetBone(0).parentBone != null} {Math.Round(b.EndPos.X, 2)} {Math.Round(b.EndPos.Y, 2)} {Math.Round(b.EndPos.Z, 2)}  {Math.Round(b.Rot.X, 2)} {Math.Round(b.Rot.Y, 2)} {Math.Round(b.Rot.Z, 2)}".ToString());


            b = skeleton.GetBone(1);
            Logger.Log($"{skeleton.GetBone(1).parentBone != null} {Math.Round(b.EndPos.X, 2)} {Math.Round(b.EndPos.Y, 2)} {Math.Round(b.EndPos.Z, 2)}  {Math.Round(b.Rot.X, 2)} {Math.Round(b.Rot.Y, 2)} {Math.Round(b.Rot.Z, 2)}".ToString());

        }
        public void addToUI(Sensor s)
        {
            vm.sens.Add(s);
        }




    }
}
