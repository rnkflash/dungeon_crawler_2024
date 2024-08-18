using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootsrtap : MonoBehaviour
{
    public bool dontControlFps = true;
    public int target = 60;
    public int vSyncCount = 0;
     
    void Awake()
    {
        if (!dontControlFps)
        {
            QualitySettings.vSyncCount = vSyncCount;
            Application.targetFrameRate = target;    
        }
    }
     
    void Update()
    {
        if (!dontControlFps)
        {
            if(Application.targetFrameRate != target)
                Application.targetFrameRate = target;    
        }
    }
}
