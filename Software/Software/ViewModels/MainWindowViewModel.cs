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
using System.Threading;
using System.Windows.Input;

namespace Software.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        Controller backend;

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
        
        private string newSensorName = "Untitled";
        public string NewSensorName
        {
            get => newSensorName;
            set => this.RaiseAndSetIfChanged(ref newSensorName, value);
        } 
        public string StatusColor { get => statusColor;
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

        public MainWindowViewModel(){

            //start main procces controlls
            StartBackgroundProccess();
            
            //Setup button commands
            ShowLogs = ReactiveCommand.Create(LogsSpawn);
            ConnectToStation = ReactiveCommand.Create(connectToStation);
            SaveSensorName = ReactiveCommand.Create(saveSensorName);
            CalibrateSensors = ReactiveCommand.Create(calibrateSensors);
        }


        public ICommand ShowLogs{ get; }
        public void LogsSpawn()
        {
            
            Logger.ShowHide();
        }
        //Show console
        //Starting background proccesses that will be used for connecting to the station
        public void StartBackgroundProccess()
        {
            backend = new Controller(this);
            Thread t = new Thread(backend.Setup);
            t.IsBackground = true;
            t.Start();
        }

    }
}
