using Avalonia.Collections;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using Software.Classes;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Windows.Input;

namespace Software.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string doge = "";
        public string Doge { get => doge;
            set => this.RaiseAndSetIfChanged(ref doge, value);
        }
        private AvaloniaList<Sensor> sensors = new AvaloniaList<Sensor>();
        public AvaloniaList<Sensor> sens
        {
            get => sensors;
            set
            {
                sensors = value;
                this.RaiseAndSetIfChanged(ref sensors, value);
               
            }
        }

        //One of the first thing that starts when the program is ran
        public MainWindowViewModel(){

            //start main procces controlls
            StartBackgroundProccess();
            
            //Setup button commands
            ShowLogs = ReactiveCommand.Create(LogsSpawn);
        }


        public ICommand ShowLogs{ get; }

        //Show console
        public void LogsSpawn()
        {
            Logger.ShowHide();
        }
        //Starting background proccesses that will be used for connecting to the station
        public void StartBackgroundProccess()
        {
            Controller c = new Controller(this);
            Thread t = new Thread(c.Loop);
            t.IsBackground = true;
            t.Start();
        }

    }
}
