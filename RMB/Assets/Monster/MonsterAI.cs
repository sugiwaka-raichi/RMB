using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonobitEngine.MonoBehaviour
{
    public enum ANIM_TYPE
    {
        isWalk,            //歩きアニメーション
        isStop,            //停止アニメーション
    }

    //-------------------------
    //アニメーションで使う変数
    //-------------------------
    [SerializeField]
    private Animator animator;              //アニメーター
    [SerializeField]
    private ANIM_TYPE nowAnim;              //現在のアニメーション
    private bool playerOn = false;          //プレイヤーが範囲にいればtrue

    //-------------------------------
    //AIで使う変数
    //-------------------------------
    [SerializeField]
    private NavMeshAgent monsterAI;
    [SerializeField]
    private MonsterBase monster;
    private bool aiflg = true;     //AIを動かすかどうか

    // Start is called before the first frame update
    void Start()
    {
        nowAnim = ANIM_TYPE.isStop;         //停止状態から開始
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが範囲にいれば歩きモーションに変更
        if (playerOn)
        {
            animator.SetBool(ANIM_TYPE.isStop.ToString(), false);

            animator.SetBool(ANIM_TYPE.isWalk.ToString(), true);
        }
        else
        {
            animator.SetBool(ANIM_TYPE.isStop.ToString(), true);

            animator.SetBool(ANIM_TYPE.isWalk.ToString(), false);
        }

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
            //Debug.Log("Monsterflg:" + monster.GetCatchFlg());

            if (aiflg)
            {
                Vector3 destination = new Vector3();        //移動先ベクトルの設定

                //ベクトルを求める
                destination.x = (this.gameObject.transform.position.x - other.transform.position.x);
                destination.z = (this.gameObject.transform.position.z - other.transform.position.z);

                //求めたベクトルを自身の位置から加算して移動先を設定する
                monsterAI.destination = transform.position + destination;
                playerOn = true;
            }
        }
    }

    //=================================================
    //プレイヤーが範囲から消えればフラグを折る
    //=================================================
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerOn = false;

        }
    }
}
