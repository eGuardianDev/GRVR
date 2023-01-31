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
        List<Bone> bones;

        public bool IsReady { get { if (bones.Count == 0) { return false; } else return true; } }

        public Skeleton(Controller c)
        {
            Logger.Log("Initiliez Skeleton structure");
            bones = new List<Bone>();
        }
        public int LoadStructure()
        {
            //TODO:
            // 1. load file
            // 2. Save strucutre on bones
            
            return 0;
        }

        public Bone GetBone(int boneIndex = 0)
        {
            if (!IsReady)
            {
                Logger.Error("Bone structure isn't loaded"); return null;
            }
            if (boneIndex > bones.Count - 1)
            {
                Logger.Error("Out of bound bone selection"); return null;
            }
            if (boneIndex < 0)
            {
                Logger.Error("Cannot get bone with negative id"); return null;
            }
            return bones[boneIndex];
        }
        public int Reset()
        {
            Logger.Log("Reset skeleton!");
            bones.Clear();
            return 0;
        }
        public int setupBone(int boneIndex, Sensor sensor, Bone parentBone)
        {
            if (boneIndex > bones.Count - 1)
            {
                Logger.Error("Out of bound bone selection"); return 1;
            }

            bones[boneIndex].ConnctedSensor = sensor;
            bones[boneIndex].parentBone = parentBone;

            return 0;
        }
        public void Calculate()
        {
            foreach (var bone in bones)
            {
                // Logger.Log($"{bone.Rot.X}");
                bone.Calculate();
            }
        }
    }
}
