using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform startSpawn;
    public List<GameObject> objectToActivate;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreatePlayer());
    }

    private IEnumerator CreatePlayer()
    {
        AssetDataManager assetData = AssetDataManager.Instance;
        UserData.UserDataManager userData = UserData.UserDataManager.Instance;
        
        while (!assetData.loadedAssets)
        {
            yield return null;
        }
        GameObject player = GameObject.Instantiate(assetData.spaceshipObject.Find(obj => obj.GetComponent<PlayerSpaceship>().spaceshipData.id == userData.userData.shipEquiped));
        player.transform.position = startSpawn.position;
        player.transform.rotation = startSpawn.rotation;
        for(int i = 0; i < objectToActivate.Count; ++i)
            objectToActivate[i].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
