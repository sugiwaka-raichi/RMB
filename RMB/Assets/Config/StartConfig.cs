using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConfig : MonoBehaviour
{
    [SerializeField]
    GameObject configMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //======================================
        // コンフィグを開く
        //======================================
        if (Input.GetKey(KeyCode.Escape))
        {
            configMenu.SetActive(true);
        }
    }
}
