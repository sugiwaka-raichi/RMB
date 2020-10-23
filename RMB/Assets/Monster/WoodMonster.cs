using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class WoodMonster : MonsterBase
{
    // Start is called before the first frame update
    void Start()
    {
        type = (int)MONSTER_TYPE.MT_PLANT;       //木属性のモンスター
    }

    //=============================================
    //攻撃関数
    //=============================================
    public override void Attack()
    {
        //攻撃オブジェクトの生成位置を決める
        Vector3 atkPos = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z + 5);

        //攻撃オブジェクト生成
        GameObject gameObject = MonobitNetwork.Instantiate(attackObj.name, atkPos, Quaternion.identity, 0, null, false, false, true);

        AttackBase attack = gameObject.GetComponent<AttackBase>();      //攻撃オブジェクトから攻撃のコンポーネントを取得

        attack.SetPlayerID(playerID);       //プレイヤーIDを設定

        base.Attack();
    }

    //==============================================
    //防御関数
    //==============================================
    public override void Deffence()
    {
        //防御時生成
        GameObject gameObject = MonobitNetwork.Instantiate(diffenceObj.name, transform.parent.transform.position, Quaternion.identity, 0);

        base.Deffence();
    }

}
