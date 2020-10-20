﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class FireMonster : MonsterBase
{
    [SerializeField]
    GameObject attackObj;     //攻撃時に使うオブジェクトを格納
    [SerializeField]
    GameObject diffenceObj;     //防御時に使うオブジェクトを格納

    void Start()
    {
        type = (int)MONSTER_TYPE.MT_FIRE;       //火属性のモンスターであることを示す
    }

    //===========================
    //攻撃関数
    //===========================
    public override void Attack()
    {
        Debug.Log("fire:" + type); 

        Vector3 vec3 = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z + 1.0f);

        //ToDo:詳細な攻撃処理は以下に記述

        //攻撃オブジェクト生成
        GameObject gameObject = MonobitNetwork.Instantiate(attackObj.name, vec3, Quaternion.identity, 0, null, false, false, true);

        AttackBase attack = gameObject.GetComponent<AttackBase>();      //攻撃オブジェクトから攻撃のコンポーネントを取得

        attack.SetPlayerID(playerID);       //プレイヤーIDを設定


        base.Attack();      //その他攻撃時共通の処理があれば
    }

    //===========================
    //防御関数
    //===========================
    public override void Deffence()
    {
        Debug.Log("fireDiffence");
        base.Deffence();

        //ToDo:詳細な防御処理は以下に記述
        GameObject gameObject = MonobitNetwork.Instantiate(diffenceObj.name, transform.parent.transform.position, Quaternion.identity, 0);

    }


}
