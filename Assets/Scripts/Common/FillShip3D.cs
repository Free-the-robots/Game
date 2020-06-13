using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillShip3D : MonoBehaviour
{
    public GameObject lockedShip;
    // Start is called before the first frame update
    void OnEnable()
    {
        AssetDataManager assetManager = AssetDataManager.Instance;
        UserData.UserDataManager userData = UserData.UserDataManager.Instance;
        int count = assetManager.spaceshipObject.Count;
        for (int i = 0; i < count; ++i)
        {
            if (userData.userData.ships.Count > i)
            {
                if (userData.userData.ships[i].unlocked)
                {
                    InstantiateUnlocked(i);
                }
                else
                    InstantiateLocked(i);
            }
            else
                InstantiateLocked(i);
        }
        foreach(GameObject ship in assetManager.spaceshipObject)
        {
        }
    }

    private void OnDisable()
    {
        foreach(Transform trans in transform)
        {
            GameObject.Destroy(trans.gameObject);
        }
    }

    private void InstantiateUnlocked(int index)
    {
        AssetDataManager assetManager = AssetDataManager.Instance;
        UserData.UserDataManager userData = UserData.UserDataManager.Instance;
        GameObject gb = GameObject.Instantiate(assetManager.spaceshipObject[index]);
        gb.GetComponent<PlayerSpaceship>().enabled = false;
        foreach (Collider coll in gb.GetComponentsInChildren<Collider>())
            coll.enabled = false;
        gb.transform.SetParent(transform, false);
        gb.SetActive(false);
    }

    private void InstantiateLocked(int index)
    {
        AssetDataManager assetManager = AssetDataManager.Instance;
        UserData.UserData userData = UserData.UserDataManager.Instance.userData;
        SpaceshipData shipData = null;
        if (userData.ships.Count > index)
            shipData = assetManager.spaceshipScriptableData[index];

        GameObject gb = GameObject.Instantiate(lockedShip);

        //Update mesh
        int lastChild = assetManager.spaceshipObject[index].transform.childCount - 1;
        Mesh mesh = assetManager.spaceshipObject[index].transform.GetChild(lastChild).GetComponent<MeshFilter>().sharedMesh;
        for (int i = 0; i < 2; i++)
            gb.transform.GetChild(i).GetComponent<MeshFilter>().mesh = mesh;

        //Update plane
        float perc = 0f;
        float shipSize = 4f;
        float shipCenter = 2f;
        if (userData.ships.Count > index)
        {
            perc = userData.ships[index].craft.amount / (float)userData.ships[index].craft.unlockAmount;
            shipSize = shipData.maxLength - shipData.minLength;
            shipCenter = (shipData.maxLength + shipData.minLength) / 2f;
        }
        Vector3 lP = gb.transform.GetChild(gb.transform.childCount - 1).localPosition;
        lP.z = (1f - perc) * shipSize - shipCenter;
        gb.transform.GetChild(gb.transform.childCount -1).localPosition = lP;


        gb.transform.SetParent(transform, false);
        gb.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
