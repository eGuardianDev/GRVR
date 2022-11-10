using ReactiveUI;
using Software.Classes;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Windows.Input;

namespace Software.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string[] MyItems = new string[2];
        public string Doge => "doge!";

        public MainWindowViewModel(){
            ShowLogs = ReactiveCommand.Create(LogsSpawn);

            Controller c = new Controller(this);
            MyItems.Append("Hello");
            MyItems.Append("World");
            Thread t = new Thread(c.Loop);
            t.IsBackground = true;
           // t.Start();
        }
       

            public ICommand ShowLogs{ get; }


        public string Greeting => "Welcome to Avalonia!";
        public void LogsSpawn()
        {
            Logger.ShowHide();
        }
    }
}
