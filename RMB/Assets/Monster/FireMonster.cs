using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : MonsterBase
{
    //===========================
    //攻撃関数
    //===========================
    public override void Attack()
    {
        Debug.Log("fire");
        base.Attack();      //アタックの最低限の処理

        //***********************************************************
        //エフェクトマネージャにエフェクト生成処理を求めるとか
        //当たり判定の生成とか
        //***********************************************************

    }
}
