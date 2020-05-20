using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UserData
{
    [Serializable]
    public class ShipData : CommonData
    {
        public ShipData() : base()
        {

        }

        public ShipData(byte[] data) : base()
        {
            LoadSerialize(data);
        }

        public ShipData(int exp, byte lvl) : base(exp, lvl)
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
