using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobile
{
    [SerializeField]
    [CreateAssetMenu(fileName = "UserMaterials", menuName = "Mobile/Data", order = 0)]
    public class MaterialsObject : ScriptableObject
    {
        int energy = 100;
        int coins = 0;
    }
}
