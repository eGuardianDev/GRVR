using Avalonia.Controls.Platform;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Software.Classes
{
    public class Bone
    {
        /*

               z=0 
                |\  
    cosz0 =   a | \ c
    z0/len      |  \
     a   c      |___\
               x  b  -z,+x
         sinz0 = b/c  
         b = sinz0 *c


        
        spinning or something
            oldX = x
            x =  oldX *cos - y *sin
            y = y* cos + oldX *sin


        */


        //This is the bone that is connected before it
        //It's used for repositioning
        public Bone parentBone;

        //This variables stores the sensor used for calculating rotiaton
        private Sensor connectedSensor;
        public Sensor ConnctedSensor
        {
            get { return this.connectedSensor; }
            set { this.connectedSensor = value; }
        }

        //lenght of the bone. Used to caculate end of the bone
        private float lenght;
        public float Lenght
        {
            set { this.lenght = value; }
            get { return this.lenght; }
        }

        public struct Position
        {
            public double X;
            public double Y;
            public double Z;
        }
        public struct Rotation
        {
            public double X; // Axis connected to Y
            public double Y; // Pith coonected to sin cos deg
            public double Z; // Row  connected to next Bone
            // i Thing z can be ignored because everything will be calculated later on
            // but this may cost problems when using the accelerometer and there it will be used for corrections
        }

        //
        public Position StartPos;
        public Position EndPos;
        public Rotation Rot;

        public Bone(Bone parent = null, float lenght = 2)
        {
            Logger.Info("Bone created!");
            this.parentBone = parent;
            this.Lenght = lenght;


        }
        //recalculate the rotaiton of sensor
        public int Calculate()
        {
            /* if(parentBone != null)
             {
                 StartPos.X = parentBone.EndPos.X;
                 StartPos.Y = parentBone.EndPos.Y;
                 StartPos.Z = parentBone.EndPos.Z;
             }*/
            Rot.X = ConnctedSensor.X - connectedSensor.OffsetY;
            Rot.Y = ConnctedSensor.Y - connectedSensor.OffsetY;
            Rot.Z = ConnctedSensor.Z - connectedSensor.OffsetZ;
            int size = 2;


            // -- calculate 2D --
            double deg = Rot.Y;
            
            double sin = Math.Sin(deg * Math.PI / 180);
            double cos = Math.Cos(deg * Math.PI / 180);
            sin = Math.Round(sin, 2);
            cos = Math.Round(cos, 2);

            double x = cos;
            double y = sin;

            bool flag = false;
            //Logger.Log($"x: {x} y: {y}");
            if ((x < 0 &&(Rot.X < 90 && Rot.X > -90) )|| (x>0 && Rot.X <-90))
            {
                x = -x;
                flag = true;
            }

            // -- add 3th demension -- 
            double z = 0;
           double alpha = Rot.X;

            sin = Math.Sin(alpha * Math.PI / 180);
            cos = Math.Cos(alpha * Math.PI / 180);
            sin = Math.Round(sin, 2);
            cos = Math.Round(cos, 2);
            z = sin * x;
            x = cos * x;


            if (flag)
            {
                z = -z;
            }

            /*   if (z < 0)
               {
                   x += z;

               }
               else x -= z;

            */

            //adding 3th demention
            //   double oldX = x;
            //   double oldz = z;
            //  x = oldX * cos - oldz * sin;
            //     z = oldz * cos + oldX * sin;
            if(parentBone != null) StartPos = parentBone.EndPos;

            //end savings
            EndPos.X = StartPos.X + (x * Lenght);
            EndPos.Y = StartPos.Y + (y * Lenght);
            EndPos.Z = StartPos.Z + (z * Lenght);
            return 0;
        }
    }
}