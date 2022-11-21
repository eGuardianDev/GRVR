using System;

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
        private Bone parentBone;

        //This variables stores the sensor used for calculating rotiaton
        private Sensor connectedSensor;
        public Sensor ConnctedSensor {
            get { return this.connectedSensor; }
            set { this.connectedSensor = value; } }

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
            Logger.Info("New bone was created.");
            this.parentBone = parent;
            this.lenght = lenght;

            if(parentBone != null)
            {
                StartPos.X = parentBone.StartPos.X;
                StartPos.Y = parentBone.StartPos.Y;
                StartPos.Z = parentBone.StartPos.Z;
            }
        }
        //recalculate the rotaiton of sensor
        public int Calculate()
        {
            //2d caculations
            double sin = Math.Sin((Rot.Y * Math.PI) / 180);
            double cos = Math.Cos((Rot.Y* Math.PI) / 180);



            double x = Math.Sin((Rot.X * Math.PI) / 180);
            double y = Math.Cos((Rot.X * Math.PI) / 180);
            double z = 0;

            //adding 3th demention
            double oldX = x;
            double oldz = z;
            x = oldX * cos - oldz * sin;
            z = oldz * cos + oldX * sin;

            //end savings
            EndPos.X = StartPos.X + x * Lenght;
            EndPos.Y = StartPos.Y + y * Lenght;
            EndPos.Z = StartPos.Z + z * Lenght;
            return 0;
        }
    }
}