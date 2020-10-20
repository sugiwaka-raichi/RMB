using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class WanwanUnimon : UniMonBase
{
    [SerializeField]
    GameObject attackObj;     //攻撃時に使うオブジェクトを格納

    [SerializeField]
    private Vector3 wanwanPos;
    [SerializeField]
    private Vector3 tmp;            // 初期座標格納用

    // 追尾用
    private GameObject targetObj;        // 追尾する対象
    Coroutine coroutine;                // コルーチン
    // 二点間の距離を求める用
    private float x_Abs;
    private float y_Abs;
    private float z_Abs;
    [SerializeField]
    private float speedParameter = 10;          // 追尾速度

    [SerializeField]
    private float xMinPos = -5.0f, xMaxPos = 5.0f;       // 移動可能範囲指定ｘ
    [SerializeField]
    private float zMinPos = -5.0f, zMaxPos = 5.0f;       // 移動可能範囲指定ｚ

    // Start is called before the first frame update
    void Start()
    {
        // 追尾対象を検索
        targetObj = GameObject.Find("Player(Clone)");

        // 初期位置を格納
        tmp = this.gameObject.transform.position;
    }

    protected override void Update()
    {
        // 位置が常に範囲内か監視(対象物,Min,Max)
        //wanwanPos.x = Mathf.Clamp(wanwanPos.x, tmp.x -5.0f, tmp.x + 5.0f);
        //wanwanPos.z = Mathf.Clamp(wanwanPos.z, tmp.z -5.0f, tmp.z + 5.0f);

        Tracking();

        // 範囲内が有効であれば常にその位置が入る
        this.gameObject.transform.position = new Vector3(Mathf.Clamp(wanwanPos.x,tmp.x + xMinPos, tmp.x + xMaxPos), 
                                                         tmp.y, 
                                                         Mathf.Clamp(wanwanPos.z, tmp.x + zMinPos, tmp.x + zMaxPos));
    }

    void Tracking()
    {
        // 二点間の距離を求める
        x_Abs = Mathf.Abs(this.gameObject.transform.position.x - targetObj.transform.position.x);
        y_Abs = Mathf.Abs(this.gameObject.transform.position.y - targetObj.transform.position.y);
        z_Abs = Mathf.Abs(this.gameObject.transform.position.z - targetObj.transform.position.z);

        // コルーチンが有効な間は追尾し続ける
        if (coroutine == null)
        {
            coroutine = StartCoroutine(MoveCoroutine());
        }
    }

    // 追尾のコルーチン
    IEnumerator MoveCoroutine()
    {
        float speed = speedParameter * Time.deltaTime;

        // x,y,zのどれか一つでも0以上
        while (this.gameObject.transform.position != targetObj.transform.position)
        {
            yield return new WaitForEndOfFrame();

            // ここで計算結果を反映(実質移動)
            wanwanPos = Vector3.MoveTowards(this.gameObject.transform.position, targetObj.transform.position, speed);
        }

        Debug.Log("重なった");
    }

    // 指定したオブジェクトとぶつかった時の処理
    void OnTriggerEnter(Collider other)
    {
        //ターゲットにしたオブジェクトにタグをつけとく
        if (other.gameObject.tag == "Player")
        {
            
        }
    }
}
