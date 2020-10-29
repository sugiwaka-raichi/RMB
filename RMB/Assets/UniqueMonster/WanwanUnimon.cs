using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class WanwanUnimon : AttackBase
{
    [SerializeField]
    GameObject originObj;     // 原点オブジェクトを格納

    private Vector3 wanwanPos;      // 現在位置を保持
    [SerializeField]
    private Vector3 tmpPos;            // 初期座標格納用

    // 追尾用
    private GameObject targetObj;        // 追尾する対象
    [SerializeField]
    private float speedParameter = 10;          // 追尾速度

    private bool reTrackingFlag = false;            // 再追跡フラグ
    [SerializeField]
    private float reTrackingTimer = 0.0f;           // 再追跡タイマカウント
    [SerializeField]
    private float reTrackingOnTimer;           // 再追跡オンタイマ
    [SerializeField]
    private float reTrackingOffTimer;           // 再追跡オフタイマ

    private bool seFlg = true;                 // se再生フラグ

    [Header("Can Move Wanwan Set XZ Position Min and Max")]
    [SerializeField]
    private float xMinPos = -5.0f;      // 移動可能範囲指定Minｘ
    [SerializeField]
    private float xMaxPos = 5.0f;       // 移動可能範囲指定Maxｘ
    [SerializeField]
    private float zMinPos = -5.0f;      // 移動可能範囲指定Minｚ
    [SerializeField]
    private float zMaxPos = 5.0f;       // 移動可能範囲指定Maxｚ

    /*============================= Start =============================*/
    void Start()
    {
        // 追尾対象を検索
        targetObj = GameObject.FindGameObjectWithTag("Player");

        // 攻撃属性を指定
        atkType = ATK_TYPE.AT_NONE;

        // 初期位置を格納
        tmpPos = this.gameObject.transform.position;

        //原点オブジェクト生成
        GameObject gameObject = MonobitNetwork.Instantiate(originObj.name, tmpPos, Quaternion.identity, 0, null, false, false, true);
    }

    /*============================= Update =============================*/
    private void Update()
    {
        // ホストじゃなければ返す
        if (!MonobitNetwork.isHost)
        {
            return;
        }

        // 範囲内が有効であれば常にその位置が入る
        this.gameObject.transform.position = new Vector3(Mathf.Clamp(wanwanPos.x, tmpPos.x + xMinPos, tmpPos.x + xMaxPos),
                                                         tmpPos.y,
                                                         Mathf.Clamp(wanwanPos.z, tmpPos.z + zMinPos, tmpPos.z + zMaxPos));
    }

    /*============================= OnCollisionStay =============================*/
    private void OnTriggerStay(Collider other)
    {
        if (reTrackingFlag == true)
        {
            float speed = speedParameter * Time.deltaTime;
            // タグがPlayerであるか
            if (other.gameObject.tag == "Player")
            {
                if (seFlg == true)
                {
                    SoundManager.PlaySE("WanwanBark");
                    seFlg = false;
                }
                // ここで計算結果を反映(実質移動)
                wanwanPos = Vector3.MoveTowards(this.gameObject.transform.position, other.transform.position, speed);
            }
        }
        // 原点に戻る
        else if(reTrackingFlag == false)
        {
            float speed = speedParameter * Time.deltaTime;
            wanwanPos = Vector3.MoveTowards(this.gameObject.transform.position, tmpPos, speed);
        }

        if (other.gameObject.tag == "Player")
        {
            // 再追跡オン
            if (reTrackingTimer > reTrackingOnTimer && reTrackingFlag == false)
            {
                reTrackingFlag = true;
                reTrackingTimer = 0.0f;
                seFlg = true;
            }

            // 再追跡オフ
            if (reTrackingTimer > reTrackingOffTimer && reTrackingFlag == true)
            {
                reTrackingFlag = false;
                reTrackingTimer = 0.0f;
                seFlg = false;
            }
            reTrackingTimer += Time.deltaTime;
        }
    }

    /*============================= OnTriggerExit =============================*/
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            reTrackingTimer = 0.0f;
            reTrackingFlag = false;
            seFlg = false;
        }
    }

    /*============================= OnCollisionEnter =============================*/
    // 指定したオブジェクトとぶつかった時の処理
    private void OncollisionEnter(Collision _collision)
    {
        //ターゲットにしたオブジェクトにタグをつけとく
        if (_collision.gameObject.tag == "Player")
        {
            Debug.Log("重なった");
        }
    }
}
