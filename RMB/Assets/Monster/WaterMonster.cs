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

        //位置設定
        Vector3 pos = transform.parent.transform.position;
        pos.y += 4.5f;          //生成位置微調整
        GameObject gameObject = MonobitNetwork.Instantiate(diffenceObj.name, pos, Quaternion.identity, 0, null, false, false, true);
        Destroy(gameObject, 2);

        base.Deffence();

    }
}
