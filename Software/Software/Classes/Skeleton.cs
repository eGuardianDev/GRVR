using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Software.Classes
{
    public class Skeleton
    {
        List<Bone> bones = new List<Bone>();    

        public Skeleton(Controller c)
        {
            bones.Add(new Bone());
            bones.Add(new Bone(bones.Where(x => x.StartPos.X == 0).First()));
            bones[0].ConnctedSensor = c.station.Sensors[0];
            bones[1].ConnctedSensor = c.station.Sensors[1];
        }
        public Bone bone(int boneIndex = 0)
        {
            if (boneIndex > bones.Count - 1) { 
                Logger.Error("Out of bound bone selection"); return null;
            }

            return bones[boneIndex];
        }
        public void Calculate()
        {
            foreach(var bone in bones)
            {
                bone.Calculate();
            }
        }
    }
}
