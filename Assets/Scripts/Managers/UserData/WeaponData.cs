using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UserData
{
    [Serializable]
    public class WeaponData : CommonData
    {
        public int rare;
        public int damage;
        public Projectiles.ProjectileData.PROJECTILEELEMENT projectileElement;
        public WeaponData() : base()
        {

        }
        public WeaponData(byte[] data) : base()
        {
            LoadSerialize(data);
        }

        public WeaponData(int exp, byte lvl, ushort id) : base(exp, lvl, id)
        {

        }


        public override byte[] Serialize()
        {
            byte[] bytes = CommonData.addByteToArray(null, base.Serialize());
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(rare));
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(damage));
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes((int)projectileElement));
            return bytes;
        }

        public override CommonData LoadSerialize(byte[] data)
        {
            int j = 0;
            base.LoadSerialize(data);
            j += base.byteCount();
            rare = BitConverter.ToInt32(data, j);
            j += sizeof(int);
            damage = BitConverter.ToInt32(data, j);
            j += sizeof(int);
            projectileElement = (Projectiles.ProjectileData.PROJECTILEELEMENT)BitConverter.ToInt32(data, j);

            return this;
        }

        public override int byteCount()
        {
            return base.byteCount() + sizeof(int) + sizeof(int) + sizeof(int);
        }
    }

    [Serializable]
    public class EvoWeaponData : WeaponData
    {
        public NEAT.Person evoData;

        public EvoWeaponData() : base()
        {

        }
        public EvoWeaponData(byte[] data) : base()
        {
            LoadSerialize(data);
        }

        public EvoWeaponData(int exp, byte lvl, ushort id) : base(exp, lvl, id)
        {

        }

        public override byte[] Serialize()
        {
            byte[] bytes = CommonData.addByteToArray(null, base.Serialize());
            int count = System.Text.Encoding.Default.GetBytes(evoData.ToJson()).Length;
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(count));
            bytes = CommonData.addByteToArray(bytes, System.Text.Encoding.Default.GetBytes(evoData.ToJson()));
            return bytes;
        }

        public override CommonData LoadSerialize(byte[] data)
        {
            base.LoadSerialize(data);
            int size = BitConverter.ToInt32(data, base.byteCount());
            byte[] dataSub = new byte[size];
            Array.Copy(data, base.byteCount() + sizeof(int), dataSub, 0, size);
            string evo = System.Text.Encoding.Default.GetString(dataSub);
            evoData = ScriptableObject.CreateInstance<NEAT.Person>();
            evoData.fromJson(evo);
            return this;
        }

        public override int byteCount()
        {
            return base.byteCount() + sizeof(int) + System.Text.Encoding.Default.GetBytes(evoData.ToJson()).Length;
        }
    }
}