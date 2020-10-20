﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class GunUnimon : UniMonBase
{
    [SerializeField]
    GameObject attackObj;     //攻撃時に使うオブジェクトを格納

    // Start is called before the first frame update
    void Start()
    {

    }

    //===========================
    //攻撃関数
    //===========================
    public override void Attack()
    {
        Debug.Log("fire:wanwan");

        // 攻撃発生位置決め
        Vector3 vec3 = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z + 1.0f);

        // 攻撃オブジェクト生成
        GameObject gameObject = MonobitNetwork.Instantiate(attackObj.name, vec3, Quaternion.identity, 0, null, false, false, true);

        base.Attack();      //攻撃時共通の処理があれば
    }
}
