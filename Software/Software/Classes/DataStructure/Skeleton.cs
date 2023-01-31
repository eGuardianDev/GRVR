using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Software.Classes.Controllers;
using Software.Classes.DataLoggin;

namespace Software.Classes.DataStructure
{
    public class Skeleton
    {
        List<Bone> bones;

        Bone ArmUp;
        Bone ArmDown;


        public Skeleton(Controller c)
        {
            bones = new List<Bone>();
            ArmUp = new Bone();
            ArmDown = new Bone(ArmUp);
            bones.Add(ArmUp);
            bones.Add(ArmDown);
        }
        public Bone GetBone(int boneIndex = 0)
        {
            if (boneIndex > bones.Count - 1)
            {
                Logger.Error("Out of bound bone selection"); return null;
            }

            return bones[boneIndex];
        }
        public int setupBone(int boneIndex, Sensor s, Bone b = null)
        {
            if (boneIndex > bones.Count - 1)
            {
                Logger.Error("Out of bound bone selection"); return 1;
            }

            bones[boneIndex].ConnctedSensor = s;
            bones[boneIndex].parentBone = b;
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
