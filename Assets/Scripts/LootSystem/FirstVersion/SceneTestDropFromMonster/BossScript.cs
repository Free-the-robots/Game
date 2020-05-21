using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossScript : MonoBehaviour
{
    [SerializeField]
    private GenericLootDropTableGameobject LootDropTable;

    private void OnValidate()
    {
        LootDropTable.ValidateTable();
    }

    private void OnDestroy()
    {
        SpawnLoot(); 
    }

    private void SpawnLoot()
    {
        List<GenericDropLootItemGameobject> mList = LootDropTable.ListDropItem(); 
        for(int i = 0; i < mList.Count; ++i)
        {
            GameObject SelectedDropItem = Instantiate(mList[i].Item);
            SelectedDropItem.transform.position = new Vector2(i / 2F, 2F);
        }
    }


}
