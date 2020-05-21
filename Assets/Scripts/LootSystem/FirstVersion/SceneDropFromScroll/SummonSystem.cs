using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonSystem : MonoBehaviour
{
    [SerializeField]
    private GenericLootDropTableSO DropTableSO;
    [SerializeField]
    private Text NameSummonedItem;
    [SerializeField]
    private Text NumberStars;

    private List<GenericLootDropItemSO> DropItem;

    private SpaceShipSO SpaceshipScriptableObject;

    private GameObject SummonedSpaceship;

    private void OnValidate()
    {
        //DropTableSO.ValidateTable();
    }
    // Start is called before the first frame update
    void Start()
    {
        DropItem = new List<GenericLootDropItemSO>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Summon()
    {
        Debug.Log("Summon");
        SpawnSpaceship();
    }

    private void SpawnSpaceship()
    {
        ClearUI();
        DropTableSO.ValidateTable();
        DropItem = DropTableSO.ListDropItem();
        Debug.Log("DROP ITEM COUNT : " + DropItem.Count);
        SpaceshipScriptableObject = (SpaceShipSO)DropItem[0].Item;
        SummonedSpaceship = Instantiate(SpaceshipScriptableObject.SpaceshipGO);
        SummonedSpaceship.transform.position = new Vector3(400F, 240F, -54F);
        NameSummonedItem.text = SpaceshipScriptableObject.SpaceshipName;
        for (int i = 0; i < SpaceshipScriptableObject.SpaceshipStars; ++i)
            NumberStars.text += "*";
    }

    private void ClearUI()
    {
        DropItem.Clear();
        NameSummonedItem.text = "";
        NumberStars.text = "";
        if (SummonedSpaceship != null)
            Destroy(SummonedSpaceship);

    }
}
