using Avalonia.Collections;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using SkiaSharp;
using Software.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Input;

namespace Software.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        Controller backend;

        private bool serverPort = false;
        public bool ServerPort
        {
            get => serverPort;
            set => this.RaiseAndSetIfChanged(ref serverPort, value);


        }
        private bool[] activeDisplayRow0 = { false, false, false, false, false, false, false };
        private bool[] activeDisplayRow1 = { false, false, false, false, false, false, false };
        private bool[] activeDisplayRow2 = { false, false, false, false, false, false, false };
        private bool[] activeDisplayRow3 = { false, false, false, false, false, false, false };
        private bool[] activeDisplayRow4 = { false, false, false, false, false, false, false };
        public bool[] ActiveDisplayRow0
        {
            get => activeDisplayRow0;
            set => this.RaiseAndSetIfChanged(ref activeDisplayRow0, value);

        }
        public bool[] ActiveDisplayRow1
        {
            get => activeDisplayRow1;
            set => this.RaiseAndSetIfChanged(ref activeDisplayRow1, value);


        }
        public bool[] ActiveDisplayRow2
        {
            get => activeDisplayRow2;
            set => this.RaiseAndSetIfChanged(ref activeDisplayRow2, value);


        }
        public bool[] ActiveDisplayRow3
        {
            get => activeDisplayRow3;
            set => this.RaiseAndSetIfChanged(ref activeDisplayRow3, value);


        }
        public bool[] ActiveDisplayRow4
        {
            get => activeDisplayRow4;
            set => this.RaiseAndSetIfChanged(ref activeDisplayRow4, value);


        }

        public ICommand OpenServerPort { get; }
        public void openServerPort()
        {
            Logger.Log($"Server status: {serverPort.ToString()}");
            backend.api.Port = APIPort;
            backend.StartAPIServer();
        }

        private List<string> ports;
        public List<string> Ports
        {
            get => ports;
            set => this.RaiseAndSetIfChanged(ref ports, value);


        }

        private string selectedPort;
        public string SelectedPort
        {
            get => selectedPort;
            set => this.RaiseAndSetIfChanged(ref selectedPort, value);


        }

        private string buttonForConnectingText = "Connect";
        public string ButtonForConnectingText
        {
            get => buttonForConnectingText;
            set => this.RaiseAndSetIfChanged(ref buttonForConnectingText, value);


        }

        private string stationStatus = "Offline";
        public string StationStatus
        {
            get => stationStatus;
            set => this.RaiseAndSetIfChanged(ref stationStatus, value);
        }
        private string statusColor = "Red";
        public ICommand ConnectToStation { get; }
        public void connectToStation()
        {
            if (selectedPort == null) return;
            if (backend.station.IsStationOnline)
            {
                ButtonForConnectingText = "Connect";
                backend.station.Disconnect();
            }
            else
            {
                backend.station.CommunicationPort = selectedPort;
                Logger.Log($"Connecting to station with port: {selectedPort.ToString()}");
                ButtonForConnectingText = "Disonnect";
                backend.station.Connect();
            }
        }


        private ObservableCollection<Sensor> sensors = new ObservableCollection<Sensor>();
        public ObservableCollection<Sensor> sens
        {
            get => sensors;
            set => this.RaiseAndSetIfChanged(ref sensors, value);
        }

        private Sensor device = new Sensor();
        public Sensor SelectedDevice
        {
            get => device;
            set => this.RaiseAndSetIfChanged(ref device, value);
        }

        private bool areSensorsLoaded= false;
        public bool AreSensorsLoaded
        {
            get => areSensorsLoaded;
            set => this.RaiseAndSetIfChanged(ref areSensorsLoaded, value);
        }



        private string newSensorName = "Untitled";
        public string NewSensorName
        {
            get => newSensorName;
            set => this.RaiseAndSetIfChanged(ref newSensorName, value);
        }
        private int _APIPort = 8585;
        public int APIPort
        {
            get => _APIPort;
            set => this.RaiseAndSetIfChanged(ref _APIPort, value);
        }
        public string StatusColor
        {
            get => statusColor;
            set => this.RaiseAndSetIfChanged(ref statusColor, value);
        }
        public ICommand SaveSensorName { get; }
        public void saveSensorName()
        {
            if (SelectedDevice.Name == "Unnamed 0")
            {
                Logger.Log("No sensor selected");
                return;
            }
            SelectedDevice.Name = NewSensorName;
        }



        public ICommand CalibrateSensors { get; }
        public void calibrateSensors()
        {
            if (SelectedDevice.Name == "Unnamed 0")
            {
                Logger.Log("No sensor selected");
                return;
            }
            SelectedDevice.OffsetX = SelectedDevice.X;
            SelectedDevice.OffsetY = SelectedDevice.Y;
            SelectedDevice.OffsetZ = SelectedDevice.Z;
        }
          public ICommand StartStopAPIServer { get; }
        public void startStopAPIServer()
        {
            if (backend.api.IsOpen == true)
            {
                backend.api.StopServer();
            }
            else
            {
                backend.api.StartServer();
            }
        }

        private string apiServerStatus = "Untitled";
        public string ApiServerStatus
        {
            get => apiServerStatus;
            set => this.RaiseAndSetIfChanged(ref apiServerStatus, value);
        }
        public MainWindowViewModel()
        {

            //start main procces controlls
            StartBackgroundProccess();

            //Setup button commands
            ShowLogs = ReactiveCommand.Create(LogsSpawn);
            ChangeDesign = ReactiveCommand.Create(() => { ActiveDisplayRow0[0] = true; });
            ConnectToStation = ReactiveCommand.Create(connectToStation);
            SaveSensorName = ReactiveCommand.Create(saveSensorName);
            CalibrateSensors = ReactiveCommand.Create(calibrateSensors);
            OpenServerPort = ReactiveCommand.Create(openServerPort);
            CurrentSensorSelected = ReactiveCommand.Create<string>(currentSensorSelected);
            StartStopAPIServer = ReactiveCommand.Create(startStopAPIServer);
            
        }

        public ReactiveCommand<string, Unit> CurrentSensorSelected { get; }
        public void currentSensorSelected(string args)
        {
            var data = args.Split().ToArray();
            string x = data[0];
            string y = data[1];


            Bone bone = backend.bsl.Bones.Where(b => b.displayPosX == x && b.displayPosY == y).First();
            SelectedBoneToModify = bone;

            Logger.Log(SelectedBoneToModify.name);
            BoneName = SelectedBoneToModify.name + " ";

            if (SelectedBoneToModify.parentBone != null)
            {
                BoneParent = SelectedBoneToModify.parentBone.name;
                Logger.Log(SelectedBoneToModify.parentBone.name);
            }
            else
            {
                BoneParent = "None";
            }

            if (SelectedBoneToModify.ConnctedSensor == null)
            {
                ConectedSensorName = "None";

            }
            else
            {

                string nameOfConnectedSensor = SelectedBoneToModify.ConnctedSensor.Name;
                Logger.Log(nameOfConnectedSensor);
                if (nameOfConnectedSensor == "" || nameOfConnectedSensor == null)
                {
                    ConectedSensorName = "None";

                }
                else
                {
                    ConectedSensorName = nameOfConnectedSensor;
                }
            }

        }
        public void ApplySensor()
        {
            System.Console.WriteLine(SelectedSensorToApply.Name);
            SelectedBoneToModify.ConnctedSensor = SelectedSensorToApply;
            ConectedSensorName = SelectedSensorToApply.Name;
            System.Console.WriteLine(SelectedBoneToModify.ConnctedSensor.Name);
        }

        private Sensor selectedSensorToApply = new Sensor();
        public Sensor SelectedSensorToApply
        {
            get => selectedSensorToApply;
            set => this.RaiseAndSetIfChanged(ref selectedSensorToApply, value);
        }
        private string conectedSensorName;
        public string ConectedSensorName
        {
            get => conectedSensorName;
            set => this.RaiseAndSetIfChanged(ref conectedSensorName, value);
        }
        private Bone selectedBoneToModify = new Bone();
        public Bone SelectedBoneToModify
        {
            get => selectedBoneToModify;
            set => this.RaiseAndSetIfChanged(ref selectedBoneToModify, value);
        }
        private string boneName;
        public string BoneName
        {
            get => boneName;
            set => this.RaiseAndSetIfChanged(ref boneName, value);
        }
        private string boneParent;
        public string BoneParent
        {
            get => boneParent;
            set => this.RaiseAndSetIfChanged(ref boneParent, value);
        }


        public ICommand ShowLogs { get; }
        public ICommand ChangeDesign { get; }
        public void LogsSpawn()
        {

            Logger.ShowHide();
        }

        //Show console
        //Starting background proccesses that will be used for connecting to the station
        public void StartBackgroundProccess()
        {
            this.backend = new Controller(this);
            Thread t = new Thread(backend.Setup);
            t.IsBackground = true;
            t.Start();
        }

    }
}
