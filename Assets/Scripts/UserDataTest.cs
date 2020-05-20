using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UserData.UserData userData = new UserData.UserData();
        userData.test();
        userData.SaveSerialize(Application.persistentDataPath + "DataFile.dat");

        UserData.UserData userData2 = new UserData.UserData(Application.persistentDataPath + "DataFile.dat");
        string test = "read : ";
        test += userData2.clusters.Count + " ";
        for (int i = 0; i < userData2.clusters.Count; i++)
        {
            test += userData2.clusters[i].planets.Count + " ";
        }
        test += userData2.clusters + " ";
        test += userData2.weapons.Count + " ";
        test += userData2.ships.Count + " ";
        Debug.Log(test);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
