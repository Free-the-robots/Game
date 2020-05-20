using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UserData
{

    [Serializable]
    public class LevelData : SerializableData<LevelData>
    {
        public ushort lvl;
        public bool unlocked;
        public byte difficultyLevel;

        public LevelData()
        {

        }
        public LevelData(byte[] data)
        {
            LoadSerialize(data);
        }

        public LevelData(ushort lvl, bool unlocked, byte difficultyLevel)
        {
            this.lvl = lvl;
            this.unlocked = unlocked;
            this.difficultyLevel = difficultyLevel;
        }

        public override byte[] Serialize()
        {
            byte[] array = new byte[4];
            System.Array.Copy(BitConverter.GetBytes(lvl), 0, array, 0, sizeof(ushort));
            array[2] = (byte)Convert.ToByte(unlocked);
            array[3] = (byte)difficultyLevel;
            return array;
        }

        public override LevelData LoadSerialize(byte[] data)
        {
            lvl = BitConverter.ToUInt16(data, 0);
            unlocked = BitConverter.ToBoolean(data, sizeof(ushort));
            difficultyLevel = data[sizeof(ushort) + sizeof(bool)];
            return this;
        }

        public override int byteCount()
        {
            return 4;
        }
    }

    [Serializable]
    public class PlanetData : SerializableData<PlanetData>
    {
        bool unlocked;
        public List<LevelData> levels = new List<LevelData>();


        public PlanetData()
        {

        }
        public PlanetData(bool unlock)
        {
            unlocked = unlock;
        }
        public PlanetData(byte[] data)
        {
            LoadSerialize(data);
        }

        public override byte[] Serialize()
        {
            byte[] byteArray = CommonData.addByteToArray(null, BitConverter.GetBytes(unlocked));
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes(levels.Count));
            for (int i = 0; i < levels.Count; ++i)
            {
                byteArray = CommonData.addByteToArray(byteArray, levels[i].Serialize());
            }
            return byteArray;
        }

        public override PlanetData LoadSerialize(byte[] data)
        {
            unlocked = BitConverter.ToBoolean(data, 0);
            int size = BitConverter.ToInt32(data, 1);
            levels = new List<LevelData>(size);
            int j = 0;
            for(int i = 0; i < size; ++i)
            {
                LevelData level = new LevelData(data.Skip(sizeof(int)+sizeof(bool) + j).ToArray());
                levels.Add(level);
                j += level.byteCount();
            }
            return this;
        }

        public override int byteCount()
        {
            int bytes = sizeof(int) + sizeof(bool);
            foreach (LevelData level in levels)
            {
                bytes += level.byteCount();
            }
            return bytes;
        }

        public float percentage()
        {
            return (float)(levels.Where(level => level.unlocked == true).Count()) / ((float)levels.Count);
        }
    }

    [Serializable]
    public class ClusterData : SerializableData<ClusterData>
    {
        bool unlocked;
        public List<PlanetData> planets = new List<PlanetData>();

        public ClusterData()
        {
        }

        public ClusterData(byte[] data)
        {
            LoadSerialize(data);
        }

        public override byte[] Serialize()
        {
            byte[] byteArray = CommonData.addByteToArray(null, BitConverter.GetBytes(unlocked));
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes(planets.Count));
            for (int i = 0; i < planets.Count; ++i)
            {
                byteArray = CommonData.addByteToArray(byteArray, planets[i].Serialize());
            }
            return byteArray;
        }

        public override ClusterData LoadSerialize(byte[] data)
        {
            unlocked = BitConverter.ToBoolean(data, 0);
            int size = BitConverter.ToInt32(data, sizeof(bool));
            planets = new List<PlanetData>(size);
            int j = 0;
            for (int i = 0; i < size; ++i)
            {
                PlanetData planet = new PlanetData(data.Skip(sizeof(int) + sizeof(bool) + j).ToArray());
                planets.Add(planet);
                j += planet.byteCount();
            }

            return this;
        }

        public override int byteCount()
        {
            int bytes = sizeof(int) + sizeof(bool);
            foreach(PlanetData planet in planets)
            {
                bytes += planet.byteCount();
            }
            return bytes;
        }

        public float percentage()
        {
            float res = 0f;
            int levelPerc = 0;
            foreach(PlanetData planet in planets)
            {
                res += planet.levels.Where(level => level.unlocked == true).Count();
                levelPerc += planet.levels.Count;
            }
            return res/((float)levelPerc);
        }
    }
}
