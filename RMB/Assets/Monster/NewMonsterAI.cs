using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonsterAI : MonobitEngine.MonoBehaviour
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
    private MonsterBase monster;
    private bool aiflg = true;     //AIを動かすかどうか

    private Vector3 tmpPos;            // 初期座標格納用
    private bool thinkFlg;
    [SerializeField]
    private float thinkTimer;           // 方向決定タイマ
    [SerializeField]
    private float thinkOnTimer;         // 方向決定フラグオンタイマ
    [SerializeField]
    private float thinkOffTimer;        // 方向決定フラグオフタイマ
    private int direction;
    private bool directionFlg = false;
    [SerializeField]
    private float speedParameter = 3;          // 移動速度

    [Header("Can Move Wanwan Set XZ Position Min and Max")]
    [SerializeField]
    private float xMinPos = -5.0f;      // 移動可能範囲指定Minｘ
    [SerializeField]
    private float xMaxPos = 5.0f;       // 移動可能範囲指定Maxｘ
    [SerializeField]
    private float zMinPos = -5.0f;      // 移動可能範囲指定Minｚ
    [SerializeField]
    private float zMaxPos = 5.0f;       // 移動可能範囲指定Maxｚ

    // Start is called before the first frame update
    void Start()
    {
        nowAnim = ANIM_TYPE.isStop;         //停止状態から開始

        // 初期位置を格納
        tmpPos = this.gameObject.transform.position;
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
        }

        MonsterMove();

        // 範囲内が有効であれば常にその位置が入る
        this.gameObject.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, tmpPos.x + xMinPos, tmpPos.x + xMaxPos),
                                                         tmpPos.y,
                                                         Mathf.Clamp(this.transform.position.z, tmpPos.z + zMinPos, tmpPos.z + zMaxPos));
        thinkTimer += Time.deltaTime;
    }

    //=====================================================
    //移動
    //=====================================================
    private void MonsterMove()
    {
        if (aiflg)
        {
            Vector3 destination = new Vector3();        //移動先ベクトルの設定
            float speed = speedParameter * Time.deltaTime;
            if(directionFlg == true)
            {
                direction = Random.Range(0, 5);
                directionFlg = false;
            }

            if (thinkFlg == true)
            {
                switch (direction)
                {
                    case 0:
                        destination.x += speed;
                        destination.z -= speed;
                        break;

                    case 1:
                        destination.x -= speed;
                        destination.z += speed;
                        break;

                    case 2:
                        destination.x += speed;
                        destination.z += speed;
                        break;

                    case 3:
                        destination.x -= speed;
                        destination.z -= speed;
                        break;

                    case 4:
                        destination.x += speed;
                        break;

                    case 5:
                        destination.x -= speed;
                        break;
                }
            }

            if (thinkTimer > thinkOnTimer && thinkFlg == false)
            {
                thinkFlg = true;
                thinkTimer = 0.0f;
            }

            if (thinkTimer > thinkOffTimer && thinkFlg == true)
            {
                thinkFlg = false;
                thinkTimer = 0.0f;
                directionFlg = true;
            }
            this.transform.position += destination;
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
