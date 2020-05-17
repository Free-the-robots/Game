using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericLootDropTable<T, U> where T : GenericLootDropItem<U>
{
    [SerializeField]
    private List<T> LootDropitems;
    [SerializeField]
    private int NumberDropItem;

    private List<T> LootAlwaysDrop;
    private List<T> ItemDropList;

    private float SumAllWeightDropItems;

    private float CurrentWeightMaximum;

    private float RandomFromLootTable;

    public void ValidateTable()
    {
        LootAlwaysDrop = new List<T>();
        ItemDropList = new List<T>();

        CurrentWeightMaximum = 0F;

        if(LootDropitems != null
            && LootDropitems.Count > 0)
        {
            foreach(T item in LootDropitems)
            {
                if (item.AlwaysDrop)
                {
                    item.WeightDrop = 0;
                    LootAlwaysDrop.Add(item);
                }
                    
                if(item.WeightDrop < 0F)
                {
                    Debug.LogError("Can't have weight < 0 - so put the weight to 0");
                    item.WeightDrop = 0;
                }
                else
                {
                    item.ProbabilityRangeStart = CurrentWeightMaximum;
                    CurrentWeightMaximum += item.WeightDrop;
                    item.ProbabilityRangeEnd = CurrentWeightMaximum;
                }
            }

            SumAllWeightDropItems = CurrentWeightMaximum;
            foreach (T item in LootDropitems)
            {
                item.DropPercentage = (item.WeightDrop / SumAllWeightDropItems) * 100F;
            }

        }
        for(int i = 0; i < NumberDropItem; ++i)
            PickRandomLootItem();
    }

    private void PickRandomLootItem()
    {
        RandomFromLootTable = Random.Range(0, SumAllWeightDropItems);
        foreach(T item in LootDropitems)
        {
            if (RandomFromLootTable > item.ProbabilityRangeStart && RandomFromLootTable < item.ProbabilityRangeEnd)
                ItemDropList.Add(item);
        }
    }

    public List<T> ListDropItem()
    {
        if (LootAlwaysDrop != null && LootAlwaysDrop.Count > 0)
        {
            foreach (T item in LootAlwaysDrop)
            {
                ItemDropList.Add(item);
            }
        }
           
        return ItemDropList;
    }
}
