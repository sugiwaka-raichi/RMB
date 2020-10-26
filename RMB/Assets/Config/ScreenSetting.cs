using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSetting : MonoBehaviour
{
    [SerializeField]
    int screenWidth = 1024;
    [SerializeField]
    int screenHeight = 768;
    [SerializeField]
    bool fullScreen = false;
    [SerializeField]
    int refreshRate = 60;


    // Start is called before the first frame update
    void Start()
    {
        //スクリーン情報を設定
        Screen.SetResolution(screenWidth, screenHeight, fullScreen, refreshRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
