using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorRaycast : MonoBehaviour
{
    public string levelName = "";
    public int clusterID = 0;
    public int planetID = 0;
    public int id = 0;

    public Text stars;
    public Text Lvl;
    // Start is called before the first frame update
    void OnEnable()
    {
        Lvl.text = "Lvl " + id;
        updateInfo(clusterID, planetID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateInfo(int clusterID, int planetID)
    {
        UserData.LevelData level = UserData.UserDataManager.Instance.userData.clusters[clusterID].planets[planetID].levels[id];
        stars.text = "";
        if(level != null)
        {
            for (int i = 0; i < level.difficultyLevel; i++)
            {
                stars.text += "";
            }
            if (level.unlocked)
            {
                GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0.3f;
            }
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 0.3f;
        }
    }

    public void clicked()
    {
        Debug.Log(levelName);
        if (GetComponent<CanvasGroup>().alpha > 0.5f)
        {
            if (Application.CanStreamedLevelBeLoaded(levelName))
                SceneManager.LoadScene(levelName);
            else
                Debug.LogError("No scene with name : " + levelName);
        }
    }
}
