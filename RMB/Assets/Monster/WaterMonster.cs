using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class WaterMonster : MonsterBase
{
    // Start is called before the first frame update
    void Start()
    {
        type = (int)MONSTER_TYPE.MT_WARTER;       // 水属性のモンスターであることを示す
    }

    //===========================
    //攻撃関数
    //===========================
    public override void Attack()
    {
        Debug.Log("water:" + type);

        //攻撃オブジェクトの生成位置を決める
        Vector3 atkPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //ToDo:詳細な攻撃処理は以下に記述

        //攻撃オブジェクト生成
        GameObject gameObject = MonobitNetwork.Instantiate(attackObj.name, atkPos, transform.parent.transform.rotation, 0, null, false, false, true);

        AttackBase attack = gameObject.GetComponent<AttackBase>();      //攻撃オブジェクトから攻撃のコンポーネントを取得

        attack.SetPlayerID(playerID);       //プレイヤーIDを設定


        base.Attack();      //その他攻撃時共通の処理があれば
    }

    //===========================
    //防御関数
    //===========================
    public override void Deffence()
    {
        Debug.Log("waterDiffence");
        base.Deffence();

        //ToDo:詳細な防御処理は以下に記述
        GameObject gameObject = MonobitNetwork.Instantiate(diffenceObj.name, transform.parent.transform.position, Quaternion.identity, 0);

    }
}
