using System.Collections;
using System;
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

        public WeaponData(int exp, byte lvl) : base(exp, lvl)
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
        public EvoWeaponData(byte[] data) : base()
        {
            LoadSerialize(data);
        }

        public EvoWeaponData(int exp, byte lvl) : base(exp, lvl)
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
}