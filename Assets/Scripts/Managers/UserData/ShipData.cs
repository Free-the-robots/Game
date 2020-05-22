using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UserData
{
    [Serializable]
    public class ShipData : CommonData
    {
        List<WeaponData> turrents = new List<WeaponData>();
        public ShipData() : base()
        {

        }

        public ShipData(byte[] data) : base()
        {
            LoadSerialize(data);
        }

        public ShipData(int exp, byte lvl, ushort id) : base(exp, lvl, id)
        {
        }

        public override byte[] Serialize()
        {
            byte[] bytes = CommonData.addByteToArray(null, base.Serialize());
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(turrents.Count));
            for(int i = 0; i < turrents.Count; ++i)
                bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(turrents[i].id));
            return bytes;
        }

        public override CommonData LoadSerialize(byte[] data)
        {
            int j = 0;
            base.LoadSerialize(data);
            int size = BitConverter.ToInt32(data,base.byteCount());
            j += base.byteCount();

            turrents = new List<WeaponData>(size);
            for (int i = 0; i < size; ++i)
            {
                WeaponData weaponEquiped = UserDataManager.Instance.userData.weapons.Find(obj => obj.id == BitConverter.ToUInt16(data, j));
                turrents.Add(weaponEquiped);
                j += sizeof(ushort);
            }
            return this;
        }

        public override int byteCount()
        {
            return base.byteCount() + sizeof(int) + turrents.Count*sizeof(ushort);
        }
    }
}
