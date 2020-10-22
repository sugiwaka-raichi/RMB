using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent monsterAI;
    private MonsterBase monster;

    private bool aiflg = true;     //AIを動かすかどうか

    // Start is called before the first frame update
    void Start()
    {
        monsterAI = GetComponent<NavMeshAgent>();     //AIコンポーネント取得
        monster = GetComponent<MonsterBase>();        //モンスターの情報
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーに持たれた時
        if (monster.GetCatchFlg())
        {
            aiflg = false;      //AI無効
            Destroy(monsterAI);     //モンスターAIを削除
        }

    }

    //=====================================================
    //プレイヤーが入っている間処理をし続ける
    //=====================================================
    private void OnTriggerStay(Collider other)
    {
        //プレイヤーであれば
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Monsterflg:" + monster.GetCatchFlg());

            if (aiflg)
            {
                Vector3 destination = new Vector3();        //移動先ベクトルの設定

                //ベクトルを求める
                destination.x = (this.gameObject.transform.position.x - other.transform.position.x);
                destination.z = (this.gameObject.transform.position.z - other.transform.position.z);

                //求めたベクトルを自身の位置から加算して移動先を設定する
                monsterAI.destination = transform.position + destination;
            }
        }
    }
}
