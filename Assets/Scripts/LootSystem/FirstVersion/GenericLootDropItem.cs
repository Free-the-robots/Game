using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericLootDropItem<T>
{
    public T Item;
    public float DropPercentage;
    public float WeightDrop;
    public bool AlwaysDrop;
    //Quand le check alwaysDrop est true afficher un nouveau field avec l'amount de l'item à drop
    [HideInInspector]
    public float ProbabilityRangeStart;
    [HideInInspector]
    public float ProbabilityRangeEnd;

}
