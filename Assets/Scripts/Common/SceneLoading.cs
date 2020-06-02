using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    public string level;
    public void loadLevel()
    {
        SceneManager.LoadScene(level);
    }
}
