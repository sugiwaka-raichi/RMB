using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class wanwanUnimon : MonsterBase
{
    [SerializeField]
    GameObject originObj;     // 原点オブジェクトを格納

    private Vector3 wanwanPos;      // 現在位置を保持
    [SerializeField]
    private Vector3 tmp;            // 初期座標格納用

    // 二点間の距離を求める用
    private float x_Abs;
    private float y_Abs;
    private float z_Abs;
    // 二点間の距離を計算後格納する用
    private Vector3[] absArray = null;

    // 追尾用
    private GameObject targetObj;        // 追尾する対象
    Coroutine coroutine;                 // コルーチン
    [SerializeField]
    private float speedParameter = 10;          // 追尾速度

    [SerializeField]
    private float xMinPos = -5.0f, xMaxPos = 5.0f;       // 移動可能範囲指定ｘ
    [SerializeField]
    private float zMinPos = -5.0f, zMaxPos = 5.0f;       // 移動可能範囲指定ｚ

    /*============================= Start =============================*/
    void Start()
    {
        // 追尾対象を検索
        targetObj = GameObject.Find("Player(Clone)");

        // 初期位置を格納
        tmp = this.gameObject.transform.position;

        //原点オブジェクト生成
        GameObject gameObject = MonobitNetwork.Instantiate(originObj.name, tmp, Quaternion.identity, 0, null, false, false, true);
    }

    /*============================= Update =============================*/
    private void Update()
    {
        // 追尾処理
        Tracking();

        // 範囲内が有効であれば常にその位置が入る
        this.gameObject.transform.position = new Vector3(Mathf.Clamp(wanwanPos.x,tmp.x + xMinPos, tmp.x + xMaxPos), 
                                                         tmp.y, 
                                                         Mathf.Clamp(wanwanPos.z, tmp.z + zMinPos, tmp.z + zMaxPos));
    }

    /*============================= CalculateDistance =============================*/
    // 二点間の距離を求める処理
    private void CalculateDistance()
    {
        GameObject[] playerobjects = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < MonobitNetwork.player.ID; i++)
        {
            // 二点間の距離を求める
            x_Abs = Mathf.Abs(this.gameObject.transform.position.x - playerobjects[i].transform.position.x);
            y_Abs = Mathf.Abs(this.gameObject.transform.position.y - playerobjects[i].transform.position.y);
            z_Abs = Mathf.Abs(this.gameObject.transform.position.z - playerobjects[i].transform.position.z);

            absArray[i] = new Vector3(x_Abs, y_Abs, z_Abs);
        }

        // ここの条件分岐に関しては相談してから考える
        for(int i = 0; i < MonobitNetwork.player.ID; i++)
        {
//            if(absArray[i])
        }
    }

    /*============================= Tracking =============================*/
    void Tracking()
    {
        // コルーチンが有効な間は追尾し続ける
        if (coroutine == null)
        {
            coroutine = StartCoroutine(MoveCoroutine());
        }
    }

    /*============================= MoveCoroutine =============================*/
    // 追尾のコルーチン
    IEnumerator MoveCoroutine()
    {
        float speed = speedParameter * Time.deltaTime;

        // x,y,zのどれか一つでも0以上
        while (true /*this.gameObject.transform.position != targetObj.transform.position*/)
        {
            yield return new WaitForEndOfFrame();

            // ここで計算結果を反映(実質移動)
            wanwanPos = Vector3.MoveTowards(this.gameObject.transform.position, targetObj.transform.position, speed);
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
