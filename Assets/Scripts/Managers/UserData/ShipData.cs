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
        public List<int> turrets = new List<int>();
        public Craft craft = new Craft();

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
            bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(turrets.Count));
            for(int i = 0; i < turrets.Count; ++i)
                bytes = CommonData.addByteToArray(bytes, BitConverter.GetBytes(turrets[i]));
            bytes = CommonData.addByteToArray(bytes, craft.Serialize());
            return bytes;
        }

        public override CommonData LoadSerialize(byte[] data)
        {
            int j = 0;
            base.LoadSerialize(data);
            int size = BitConverter.ToInt32(data,base.byteCount());
            j += base.byteCount();

            turrets = new List<int>(size);
            for (int i = 0; i < size; ++i)
            {
                turrets.Add(BitConverter.ToInt32(data, j));
                j += sizeof(int);
            }
            craft = new Craft(data.Skip(j).ToArray());
            return this;
        }

        public override int byteCount()
        {
            int turretByte = 0;
            for (int i = 0; i < turrets.Count; ++i)
                turretByte += sizeof(int);
            return base.byteCount() + sizeof(int) + turretByte + craft.byteCount();
        }
    }
}
