using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent monster;
    
    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<NavMeshAgent>();     //コンポーネント取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //=====================================================
    //プレイヤーが入っている間処理をし続ける
    //=====================================================
    private void OnTriggerStay(Collider other)
    {
        //プレイヤーであれば
        if (other.gameObject.tag == "Player")
        {
            Vector3 destination = new Vector3();        //移動先ベクトルの設定

            //ベクトルを求める
            destination.x = (this.gameObject.transform.position.x - other.transform.position.x);
            destination.z = (this.gameObject.transform.position.z - other.transform.position.z);
            
            //求めたベクトルを自身の位置から加算して移動先を設定する
            monster.destination = transform.position + destination;
        }
    }
}
