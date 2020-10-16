using System.Collections;
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
        base.Attack();      //攻撃時共通の処理があれば

        //ToDo:詳細な攻撃処理は以下に記述
        GameObject gameObject = MonobitNetwork.Instantiate(attackObj.name, transform.position, Quaternion.identity, 0, null, false, false, true);
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("StageScene");
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, scene);
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
