using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeffenceObj : MonoBehaviour
{
    //==========================
    //削除処理
    //==========================
    private void Start()
    {
        Destroy(this.gameObject, 2);
    }
}
