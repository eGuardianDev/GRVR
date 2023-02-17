using ReactiveUI;
using Software.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Software.Classes
{
    public class Sensor : ViewModelBase
    {
        private int id;

        private double x;
        private double y;
        private double z;

        private string name = "";
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public double X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
        public double Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }
        public double Z { get => z; set => this.RaiseAndSetIfChanged(ref z, value); }


        private double offsetx;
        private double offsety;
        private double offsetz;
        public double OffsetX { get => offsetx; set => this.RaiseAndSetIfChanged(ref offsetx, value); }
        public double OffsetY { get => offsety; set => this.RaiseAndSetIfChanged(ref offsety, value); }
        public double OffsetZ { get => offsetz; set => this.RaiseAndSetIfChanged(ref offsetz, value); }

        private double finalX;
        private double finalY;
        private double finalZ;
        public double FinalX { get => finalX; set => this.RaiseAndSetIfChanged(ref finalX, value); }
        public double FinalY { get => finalY; set => this.RaiseAndSetIfChanged(ref finalY, value); }
        public double FinalZ { get => finalZ; set => this.RaiseAndSetIfChanged(ref finalZ, value); }


        public int ID { get { return id; }  set { this.id = value; }}

        public Sensor(int id = 0)
        {
            this.ID = id;
            this.Name = $"Sensor - {ID}";
        }
        public int CalibrateSensor()
        {
            this.OffsetX = X;
            this.OffsetY = Y;
            this.OffsetZ = Z;
            return 0;
        }

        public int Runtime()
        {
            FinalY = Math.Round(Y - OffsetY,2);
            FinalZ = Math.Round(Z - OffsetZ,2);
            FinalX = Math.Round(X - OffsetX,2);
                return 0;
        }
    }
}
