using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spaceship", menuName = "Spaceship Data Test LootTable", order = 51)]
public class SpaceShipSO : ScriptableObject
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private int Stars;
    [SerializeField]
    private GameObject SpaceshipObject;

    public string SpaceshipName
    {
        get { return Name; }
    }

    public int SpaceshipStars
    {
        get { return Stars; }
    }

    public GameObject SpaceshipGO
    {
        get { return SpaceshipObject; }
    }
}
