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
        int rare;
        int damage;
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

            return this;
        }

        public override int byteCount()
        {
            return base.byteCount() + sizeof(int) + sizeof(int);
        }
    }

    [Serializable]
    public class EvoWeaponData : WeaponData
    {
        string evoData;
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
            bytes = CommonData.addByteToArray(bytes, System.Text.Encoding.Default.GetBytes(evoData));
            return bytes;
        }

        public override CommonData LoadSerialize(byte[] data)
        {
            base.LoadSerialize(data);
            evoData = System.Text.Encoding.Default.GetString(data.Skip(base.byteCount()).ToArray());
            return this;
        }

        public override int byteCount()
        {
            return base.byteCount() + evoData.Length;
        }
    }
}