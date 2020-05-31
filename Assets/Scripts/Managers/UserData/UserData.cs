using System.Collections;
using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using UnityEngine;

namespace UserData
{
    [Serializable]
    public abstract class SerializableData<T>
    {

        public abstract byte[] Serialize();

        public abstract T LoadSerialize(byte[] data);

        public abstract int byteCount();
    }

    [Serializable]
    public class CommonData : SerializableData<CommonData>
    {
        public int exp;
        public byte level;
        public ushort id;
        public bool unlocked;

        public CommonData()
        {
            exp = 0;
            level = 0;
            id = 0;
            unlocked = false;
        }
        public CommonData(byte[] data)
        {
            LoadSerialize(data);
        }
        public CommonData(int exp, byte lvl, ushort id)
        {
            this.exp = exp;
            this.level = lvl;
            this.id = id;
        }

        public override byte[] Serialize()
        {
            byte[] bytes = CommonData.addByteToArray(null, BitConverter.GetBytes(exp));
            bytes = CommonData.addByteToArray(bytes, level);
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(id));
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(unlocked));
            return bytes;
        }

        public override CommonData LoadSerialize(byte[] data)
        {
            exp = BitConverter.ToInt32(data, 0);
            level = data[sizeof(int)];
            id = BitConverter.ToUInt16(data, sizeof(int) + 1);
            unlocked = BitConverter.ToBoolean(data, sizeof(int) + sizeof(byte) + sizeof(ushort));
            return this;
        }

        public override int byteCount()
        {
            return sizeof(int) + sizeof(byte) + sizeof(ushort) + sizeof(bool);
        }

        public static byte[] addByteToArray(byte[] bArray, byte[] newByte)
        {
            byte[] newArray;
            int bArrayL = 0;
            if (bArray != null)
            {
                newArray = new byte[bArray.Length + newByte.Length];
                bArrayL = bArray.Length;
                System.Array.Copy(bArray, 0, newArray, 0, bArrayL);
            }
            else
                newArray = new byte[newByte.Length];

            System.Array.Copy(newByte, 0, newArray, bArrayL, newByte.Length);

            return newArray;
        }

        public static byte[] addByteToArray(byte[] bArray, byte newByte)
        {
            byte[] newArray;
            int bArrayL = 0;
            if (bArray != null)
            {
                newArray = new byte[bArray.Length + 1];
                bArrayL = bArray.Length;
                System.Array.Copy(bArray, 0, newArray, 0, bArrayL);
            }
            else
                newArray = new byte[1];

            newArray[bArrayL] = newByte;

            return newArray;
        }
    }

    [Serializable]
    public class UserData : SerializableData<UserData>
    {
        public List<ClusterData> clusters = new List<ClusterData>();
        public List<ShipData> ships = new List<ShipData>();
        public List<WeaponData> weapons = new List<WeaponData>();

        public int shipEquiped = 0;

        public UserData()
        {

        }
        public UserData(string filename)
        {
            LoadSerialize(filename);
        }

        public void CreateInitial()
        {
            ClusterData cluster = new ClusterData();
            PlanetData planetData = new PlanetData(true);
            for (int i = 0; i < 3; i++)
                planetData.levels.Add(new LevelData((ushort)i, false, 0));
            cluster.planets.Add(planetData);

            PlanetData planetData2 = new PlanetData(false);
            for (int i = 0; i < 30; i++)
                planetData2.levels.Add(new LevelData((ushort)(i + 30), false, 0));
            cluster.planets.Add(planetData2);

            PlanetData planetData3 = new PlanetData(false);
            for (int i = 0; i < 30; i++)
                planetData3.levels.Add(new LevelData((ushort)(i + 60), false, 0));
            cluster.planets.Add(planetData3);

            clusters.Add(cluster);

            ShipData ship = new ShipData(0, 0, (ushort)0);
            ship.unlocked = true;
            ships.Add(ship);
        }

        public void CreateInitialWithEverything()
        {
            ClusterData cluster = new ClusterData();
            PlanetData planetData = new PlanetData(true);
            for (int i = 0; i < 3; i++)
                planetData.levels.Add(new LevelData((ushort)i, false, 0));
            cluster.planets.Add(planetData);

            PlanetData planetData2 = new PlanetData(false);
            for (int i = 0; i < 30; i++)
                planetData2.levels.Add(new LevelData((ushort)(i + 30), false, 0));
            cluster.planets.Add(planetData2);

            PlanetData planetData3 = new PlanetData(false);
            for (int i = 0; i < 30; i++)
                planetData3.levels.Add(new LevelData((ushort)(i + 60), false, 0));
            cluster.planets.Add(planetData3);

            clusters.Add(cluster);

            for(int i = 0; i < 3; i++)
            {
                ShipData ship = new ShipData(0, 0, (ushort)i);
                ship.unlocked = true;
                ships.Add(ship);
            }
        }

        public void test()
        {
            for (int i = 0; i < 4; ++i)
            {
                ClusterData cluster = new ClusterData();
                cluster.planets = new List<PlanetData>();
                int rand = (int)(UnityEngine.Random.value * 3+2);
                for (int j = 0; j < rand; ++j)
                {
                    PlanetData planet = new PlanetData();
                    planet.levels = new List<LevelData>();
                    int randLevel = (int)(UnityEngine.Random.value * 100 + 2);
                    for (int k = 0; k < randLevel; k++)
                        planet.levels.Add(new LevelData((ushort)k, false, 0));
                    cluster.planets.Add(planet);
                }
                clusters.Add(cluster);
            }
            for (int i = 0; i < 20; ++i)
            {
                weapons.Add(new WeaponData(0, (byte)i, (ushort)i));
                ships.Add(new ShipData(0, (byte)i, (ushort)i));
            }

            string test = "test : ";
            test += clusters.Count + " ";
            for (int i = 0; i < clusters.Count; i++)
            {
                test += clusters[i].planets.Count + " ";
            }
            test += clusters + " ";
            test += weapons.Count + " ";
            test += ships.Count + " ";
            Debug.Log(test);
        }

        public override int byteCount()
        {
            int b = 0;
            for (int i = 0; i < clusters.Count; i++)
                b += clusters[i].byteCount();
            for (int i = 0; i < ships.Count; i++)
                b += ships[i].byteCount();
            for (int i = 0; i < weapons.Count; i++)
                b += weapons[i].byteCount();
            return b;
        }

        public void LoadSerialize(string filename)
        {
            LoadSerialize(EncryptDecrypt.LoadDecryptFile(filename));
        }

        public override UserData LoadSerialize(byte[] data)
        {
            if (data == null)
                return null;

            int size = BitConverter.ToInt32(data, 0);
            clusters = new List<ClusterData>(size);
            int j = sizeof(int);
            for (int i = 0; i < size; i++)
            {
                ClusterData cluster = new ClusterData(data.Skip(j).ToArray());
                clusters.Add(cluster);
                j += cluster.byteCount();
                if (j > data.Length)
                    throw new ArgumentOutOfRangeException("j", "Data not serialized properly");
            }

            size = BitConverter.ToInt32(data, j);
            weapons = new List<WeaponData>(size);
            j += sizeof(int);
            for (int i = 0; i < size; i++)
            {
                WeaponData weapon = new WeaponData(data.Skip(j).ToArray());
                weapons.Add(weapon);
                j += weapon.byteCount();
                if (j > data.Length)
                    throw new ArgumentOutOfRangeException("j", "Data not serialized properly");
            }

            size = BitConverter.ToInt32(data, j);
            ships = new List<ShipData>(size);
            j += sizeof(int);
            for (int i = 0; i < size; i++)
            {
                ShipData ship = new ShipData(data.Skip(j).ToArray());
                ships.Add(ship);
                j += ship.byteCount();
                if (j > data.Length)
                    throw new ArgumentOutOfRangeException("j", "Data not serialized properly");
            }
            shipEquiped = BitConverter.ToInt32(data, j);

            return this;
        }

        public void SaveSerialize(string filename)
        {
            byte[] byteArray = Serialize();

            EncryptDecrypt.StoreEncryptFile(filename,byteArray);
            Debug.Log("writing to : " + filename);
        }

        public override byte[] Serialize()
        {
            byte[] byteArray = CommonData.addByteToArray(null, BitConverter.GetBytes(clusters.Count));
            for (int i = 0; i < clusters.Count; ++i)
            {
                byteArray = CommonData.addByteToArray(byteArray, clusters[i].Serialize());
            }
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes(weapons.Count));
            for (int i = 0; i < weapons.Count; ++i)
            {
                byteArray = CommonData.addByteToArray(byteArray, weapons[i].Serialize());
            }
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes(ships.Count));
            for (int i = 0; i < ships.Count; ++i)
            {
                byteArray = CommonData.addByteToArray(byteArray, ships[i].Serialize());
            }
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes(shipEquiped));
            return byteArray;
        }

        public string SerializedString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
