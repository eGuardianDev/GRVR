using Software.Classes;
using System.Linq;
using System.Threading;

namespace Software.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string[] MyItems = new string[2];
        public string Greeting = "Welcome to Avalonia!";

        public MainWindowViewModel(){
            Controller c = new Controller();
            MyItems.Append("Hello");
            MyItems.Append("World");
            Thread t = new Thread(c.Loop);
            t.IsBackground = true;
            t.Start();
            Greeting = "Deez Nuts";
        }
    }
}
