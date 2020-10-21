using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class GunUnimon : MonsterBase
{
    [SerializeField]
    private GameObject transformObj;        // モンスター化した後のオブジェクトを格納

    [SerializeField]
    private GameObject attackObj;       // 攻撃時に使うオブジェクトを格納

    // モンスターの技のタグ
    [SerializeField] private string powertag = "";

    // 体力関連
    [SerializeField]
    private int HP;                     // 体力

    private bool changeSts = false;     // ユニモン状態(false)とモンスター状態(true)変数

    // Start is called before the first frame update
    void Start()
    {
        type = (int)MONSTER_TYPE.MT_NONE;       //無属性のモンスターであることを示す
    }

    private void Update()
    {
        // 攻撃を受けると体力が減り、無くなるとモンスター化する
        if(changeSts == false && this.gameObject.tag == "UniqueMonster")
        {
            if (HP < 0)
            {
                changeSts = true;
                this.gameObject.tag = "Monster";
            }
        }
        else if(changeSts == true)
        {
            
        }
    }

    //===========================
    //攻撃関数
    //===========================
    public override void Attack()
    {
        Debug.Log("fire:Gun");

        // 攻撃発生位置決め
        Vector3 vec3 = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z + 1.0f);

        // 攻撃オブジェクト生成
        GameObject gameObject = MonobitNetwork.Instantiate(attackObj.name, vec3, Quaternion.identity, 0, null, false, false, true);

        AttackBase attack = gameObject.GetComponent<AttackBase>();      //攻撃オブジェクトから攻撃のコンポーネントを取得

        attack.SetPlayerID(playerID);       //プレイヤーIDを設定

        base.Attack();      //攻撃時共通の処理があれば
    }

    // 被ダメージ関数
    private void Damage(int _damagevalue)
    {
        HP -= _damagevalue;
    }

    // 当たった時の処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == powertag)
        {
            Damage(20);
        }
    }
}
