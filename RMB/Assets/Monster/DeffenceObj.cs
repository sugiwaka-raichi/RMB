using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeffenceObj : MonoBehaviour
{
    [SerializeField]
    int timer;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timer);            //防御オブジェクトの削除
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
