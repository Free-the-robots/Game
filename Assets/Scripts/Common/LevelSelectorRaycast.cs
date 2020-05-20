using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorRaycast : MonoBehaviour
{
    public string levelName = "";
    public int id = 0;

    public Text stars;
    public Text Lvl;
    // Start is called before the first frame update
    void OnEnable()
    {
        Lvl.text = "Lvl " + id;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateInfo(int clusterID, int planetID)
    {
        UserData.LevelData level = UserData.UserDataManager.Instance.userData.clusters[clusterID].planets[planetID].levels[id];
        stars.text = "";
        for(int i = 0; i < level.difficultyLevel; i++)
        {
            stars.text += "";
        }
    }
}
