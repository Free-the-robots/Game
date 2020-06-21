using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform startSpawn;
    public LerpAlphaWhenEnabled fader;
    public List<GameObject> objectToActivate;

    [Header("Player Event")]
    public GameEventInt lifeEvent;
    public GameEventInt armorEvent;

    [Header("Ennemy Event")]
    public GameEvent enemyDeath;
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
        player.GetComponent<PlayerSpaceship>().lifeEvent = lifeEvent;
        player.GetComponent<PlayerSpaceship>().armorEvent = armorEvent;


        for (int i = 0; i < objectToActivate.Count; ++i)
        {
            objectToActivate[i].SetActive(true);
            if (objectToActivate[i].GetComponentsInChildren<EnnemyAISpaceship>().Length > 0)
            {
                foreach (EnnemyAISpaceship aiSpaceship in objectToActivate[i].GetComponentsInChildren<EnnemyAISpaceship>())
                {
                    aiSpaceship.enemyDead = enemyDeath;
                }
            }
        }

        fader.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
