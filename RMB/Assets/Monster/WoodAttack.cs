using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodAttack : AttackBase
{
    private void Awake()
    {
        //攻撃属性
        atkType = ATK_TYPE.AT_WOOD;
    }

    // Start is called before the first frame update
    void Start()
    {
        //有効時間
        Destroy(this.gameObject, timer);
        //木が生えるコルーチン呼出
        StartCoroutine(Grow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //========================================
    //木が生えてくる処理
    //========================================
    IEnumerator Grow()
    {  

        //yが0未満
        while(transform.position.y < 0)
        {
            //座標を上に挙げる
            transform.position += new Vector3(0, 0.1f, 0);
            yield return null;
        }

    }
}
