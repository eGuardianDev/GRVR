using Newtonsoft.Json;
using Software.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Software.Classes.DataLoggin
{
    public class BoneStructureLoader
    {
		Skeleton skeleton;
        MainWindowViewModel MainViewSettings;

		private string PathOfJsonConfig = @"D:\Projects\GRVR\Software\Software\output.json"; 

		public class BoneStructure
		{
			[JsonProperty("name")]
            public string name;
            [JsonProperty("idOfSensor")]
            public int idOfSensor;
            [JsonProperty("parentBone")]
            public string parentBone;
            [JsonProperty("PosX")]
            public string PosX;
            [JsonProperty("PosY")]
            public string PosY;
            [JsonProperty("Active")]
            public string Active;
        }
		public class SkeletonStructure
        {
            [JsonProperty("UpperBody")]
            public BoneStructure[] UpperBody;
            [JsonProperty("Legs")]
            public BoneStructure[] Legs;
		}

		private List<Bone> bones;

		public List<Bone> Bones
		{
			get { return bones; }
			set { bones = value; }
		}
		public BoneStructureLoader(Skeleton skeleton, MainWindowViewModel mv)
		{
			Bones = new List<Bone>();
			this.skeleton = skeleton;
			LoadJsonData(PathOfJsonConfig);
            this.MainViewSettings = mv;
            LoadStrucutreOnWindow();
		}

        public void LoadStrucutreOnWindow()
        {
            foreach (var item in Bones)
            {
                switch (int.Parse(item.displayPosY))
                {
                    case 0:

                        MainViewSettings.ActiveDisplayRow0[int.Parse(item.displayPosX)] = true;
                        break;
                    case 1:
                        MainViewSettings.ActiveDisplayRow1[int.Parse(item.displayPosX)] = true;
                        break;
                    case 2:
                        MainViewSettings.ActiveDisplayRow2[int.Parse(item.displayPosX)] = true;
                        break;
                    case 3:
                        MainViewSettings.ActiveDisplayRow3[int.Parse(item.displayPosX)] = true;
                        break;
                    case 4:
                        MainViewSettings.ActiveDisplayRow4[int.Parse(item.displayPosX)] = true;
                        break;

                }
            }
            MainViewSettings.ActiveDisplayRow0 = MainViewSettings.ActiveDisplayRow0.ToArray();
            MainViewSettings.ActiveDisplayRow1 = MainViewSettings.ActiveDisplayRow1.ToArray();
            MainViewSettings.ActiveDisplayRow2 = MainViewSettings.ActiveDisplayRow2.ToArray();
            MainViewSettings.ActiveDisplayRow3 = MainViewSettings.ActiveDisplayRow3.ToArray();
            MainViewSettings.ActiveDisplayRow4 = MainViewSettings.ActiveDisplayRow4.ToArray();
        }
		private void LoadJsonData(string path)
		{
			string loadedDataFromFile = File.ReadAllText(path);
            SkeletonStructure loadedDataJson = JsonConvert.DeserializeObject<SkeletonStructure>(loadedDataFromFile);

            foreach (var item in loadedDataJson.UpperBody)
            {
                string boneName = item.name;
                int idSensor = item.idOfSensor;
                string parentBoneName = item.parentBone;

                if (parentBoneName != "")
                {
                    Bone bone = new Bone(Bones.Where(x => x.name == parentBoneName).FirstOrDefault());
                    bone.name = boneName;
                    bone.displayPosX = item.PosX;
                    Logger.Log(bone.displayPosX);
                    bone.displayPosY = item.PosY;
                    if(item.Active== "true")
                    {
                        Bones.Add(bone);

                    }
                }
                else
                {
                    Bone bone = new Bone(null);
                    bone.name = boneName;
                    bone.displayPosX = item.PosX;
                    bone.displayPosY = item.PosY;
                    if (item.Active == "true")
                    {
                        Bones.Add(bone);

                    }
                }
            }
            foreach (var item in loadedDataJson.Legs)
            {
                string boneName = item.name;
                int idSensor = item.idOfSensor;
                string parentBoneName = item.parentBone;

                if (parentBoneName != "")
                {
                    Bone bone = new Bone(Bones.Where(x => x.name == parentBoneName).FirstOrDefault());
                    bone.name = boneName;
                    bone.displayPosX = item.PosX;
                    bone.displayPosY = item.PosY;
                    if (item.Active == "true")
                    {
                        Bones.Add(bone);

                    }

                }
                else
                {
                    Bone bone = new Bone(null);
                    bone.name = boneName;
                    bone.displayPosX = item.PosX;
                    bone.displayPosY = item.PosY;
                    if (item.Active == "true")
                    {
                        Bones.Add(bone);

                    }
                }
            }
        }
	}
}
