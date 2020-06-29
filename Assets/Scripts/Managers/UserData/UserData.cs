using System.Collections;
using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    public class Craft : SerializableData<Craft>
    {
        public int amount;
        public int unlockAmount = 1;
        public Craft()
        {
            amount = 0;
            unlockAmount = 1;
        }
        public Craft(byte[] data)
        {
            LoadSerialize(data);
        }

        public override int byteCount()
        {
            return sizeof(int)*2;
        }

        public override Craft LoadSerialize(byte[] data)
        {
            amount = BitConverter.ToInt32(data, 0);
            unlockAmount = BitConverter.ToInt32(data, sizeof(int));
            return this;
        }

        public override byte[] Serialize()
        {
            byte[] bytes = CommonData.addByteToArray(null, BitConverter.GetBytes(amount));
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(unlockAmount));
            return bytes;
        }
    }

    public class Material : SerializableData<Material>
    {
        int energy;
        int energyMax;
        int repairKit;
        int coin;
        public Material()
        {
            energy = 100;
            energyMax = 100;
            repairKit = 3;
            coin = 0;
        }
        public Material(byte[] data)
        {
            LoadSerialize(data);
        }
        public override int byteCount()
        {
            return sizeof(int) * 4;
        }

        public override Material LoadSerialize(byte[] data)
        {
            int j = 0;
            energy = BitConverter.ToInt32(data, j);
            j += sizeof(int);
            energyMax = BitConverter.ToInt32(data, j);
            j += sizeof(int);
            repairKit = BitConverter.ToInt32(data, j);
            j += sizeof(int);
            coin = BitConverter.ToInt32(data, j);
            j += sizeof(int);
            return this;
        }

        public override byte[] Serialize()
        {
            byte[] bytes = CommonData.addByteToArray(null, BitConverter.GetBytes(energy));
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(energyMax));
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(repairKit));
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(coin));
            return bytes;
        }
    }

    [Serializable]
    public class UserData : SerializableData<UserData>
    {
        public enum USERTYPE : int { STANDARD = 0, FACEBOOK = 1, APPLE = 2, GOOGLE = 3, OFFLINE = 4}

        public List<ClusterData> clusters = new List<ClusterData>();
        public List<ShipData> ships = new List<ShipData>();
        public List<WeaponData> weapons = new List<WeaponData>();
        public List<EvoWeaponData> evoweapons = new List<EvoWeaponData>();

        public Material material = new Material();

        public int shipEquiped = 0;

        public bool createdInitial = false;

        public USERTYPE userType = USERTYPE.STANDARD;

        public UserData()
        {

        }
        public UserData(string filename)
        {
            LoadSerialize(filename);
        }

        public void Reset()
        {
            clusters = new List<ClusterData>();
            ships = new List<ShipData>();
            weapons = new List<WeaponData>();
            evoweapons = new List<EvoWeaponData>();
            material = new Material();
            shipEquiped = 0;
            createdInitial = false;
            userType = USERTYPE.STANDARD;
        }

        public void CreateInitial(USERTYPE type = USERTYPE.STANDARD)
        {
            Reset();
            ClusterData cluster = new ClusterData();
            PlanetData planetData = new PlanetData(true);
            for (int i = 0; i < 3; i++)
                planetData.levels.Add(new LevelData((ushort)i, false, 0));
            planetData.levels[0].unlocked = true;
            cluster.planets.Add(planetData);

            PlanetData planetData2 = new PlanetData(false);
            for (int i = 0; i < 30; i++)
                planetData2.levels.Add(new LevelData((ushort)(i + 3), false, 0));
            cluster.planets.Add(planetData2);

            PlanetData planetData3 = new PlanetData(false);
            for (int i = 0; i < 30; i++)
                planetData3.levels.Add(new LevelData((ushort)(i + 60), false, 0));
            cluster.planets.Add(planetData3);

            clusters.Add(cluster);

            ShipData ship = new ShipData(0, 0, (ushort)0);
            ship.unlocked = true;
            ships.Add(ship);

            material = new Material();

            weapons = new List<WeaponData>();
            WeaponData weapon = new WeaponData();
            weapons.Add(weapon);

            evoweapons = new List<EvoWeaponData>();
            EvoWeaponData evoweapon = new EvoWeaponData();
            evoweapons.Add(evoweapon);
            Addressables.LoadAssetAsync<NEAT.Person>("Assets/ScriptableObjects/NEAT/Sinus.asset").Completed += OnLoadDoneEvoWeapon;
            //createdInitial = true;

            userType = type;
        }

        private void OnLoadDoneEvoWeapon(AsyncOperationHandle<NEAT.Person> obj)
        {
            evoweapons[0].evoData = obj.Result;
            createdInitial = true;
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

        public void LoadSerialize(string filename)
        {
            LoadSerialize(EncryptDecrypt.LoadDecryptFile(filename));


            Debug.Log("load : " + EncryptDecrypt.LoadDecryptFile(filename).Length + "bytes");
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
            evoweapons = new List<EvoWeaponData>(size);
            j += sizeof(int);
            for (int i = 0; i < size; i++)
            {
                EvoWeaponData evoweapon = new EvoWeaponData(data.Skip(j).ToArray());
                evoweapons.Add(evoweapon);
                j += evoweapon.byteCount();
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
            j += sizeof(int);
            userType = (USERTYPE)BitConverter.ToInt32(data, j);
            j += sizeof(int);
            material = new Material(data.Skip(j).ToArray());
            return this;
        }

        public IEnumerator SaveSerialize(string filename)
        {
            byte[] byteArray = Serialize();
            yield return EncryptDecrypt.StoreEncryptFile(filename,byteArray);
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
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes(evoweapons.Count));
            for (int i = 0; i < evoweapons.Count; ++i)
            {
                byteArray = CommonData.addByteToArray(byteArray, evoweapons[i].Serialize());
            }
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes(ships.Count));
            for (int i = 0; i < ships.Count; ++i)
            {
                byteArray = CommonData.addByteToArray(byteArray, ships[i].Serialize());
            }
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes(shipEquiped));
            byteArray = CommonData.addByteToArray(byteArray, BitConverter.GetBytes((int)userType));
            byteArray = CommonData.addByteToArray(byteArray, material.Serialize());
            return byteArray;
        }

        public override int byteCount()
        {
            int b = 0;
            for (int i = 0; i < clusters.Count; i++)
                b += clusters[i].byteCount();
            for (int i = 0; i < weapons.Count; i++)
                b += weapons[i].byteCount();
            for (int i = 0; i < evoweapons.Count; i++)
                b += evoweapons[i].byteCount();
            for (int i = 0; i < ships.Count; i++)
                b += ships[i].byteCount();
            b += sizeof(int);
            b += sizeof(int);
            b += material.byteCount();
            return b;
        }

        public string SerializedString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
