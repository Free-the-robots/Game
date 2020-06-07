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
            return bytes;
        }

        public override CommonData LoadSerialize(byte[] data)
        {
            base.LoadSerialize(data);
            return this;
        }

        public override int byteCount()
        {
            return base.byteCount();
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