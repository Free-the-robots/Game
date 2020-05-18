using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateLimit : MonoBehaviour
{
    public int framerate = 45;
    void Awake()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = framerate;
#endif
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
