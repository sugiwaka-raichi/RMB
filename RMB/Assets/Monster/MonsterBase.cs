using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    protected GameObject attackObj;     //攻撃時に使うオブジェクトを格納
    [SerializeField]
    protected GameObject diffenceObj;     //防御時に使うオブジェクトを格納
    [SerializeField]
    protected SoundNetwork soundNet;        //ネットワーク上でサウンドを扱うためのもの

    //===============================
    //モンスターの属性値
    //===============================
    public enum MONSTER_TYPE
    {
        MT_WARTER = 0,      //水属性のモンスター
        MT_FIRE,            //火属性のモンスター
        MT_PLANT,           //木属性のモンスター
        MT_NONE             //無属性のモンスター
    }

    protected int type;                     //属性
    protected bool catchFlg = false;        //プレイヤーに持たれているかどうか
    protected float delTimer = 10.0f;      //消去されるまでの時間
    protected int playerID = -1;            //保持してるプレイヤーのID(未保持:-1)

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        //持たれていなければ
        if (!catchFlg || transform.parent == null)
        {
            SurvivalTimer();        //生存時間
        }
    }

    //================================
    //攻撃の仮想関数
    //================================
    virtual public void Attack()
    {
        Debug.Log("attack");
        delTimer = 2;           //削除までの時間
        //DelReport();            //削除報告
        soundNet.SendPlaySE(SoundData.SE_LIST.Shot.ToString());      //攻撃時音を鳴らす
        transform.parent = null;        //親子関係解除
        SendParentElimination();        //親子解消を全体に送る
    }

    //================================
    //防御の仮想関数
    //================================
    virtual public void Deffence()
    {
        Debug.Log("Diffence");
        MonobitEngine.MonobitNetwork.Destroy(this.gameObject);       //防御したら消える
    }

    //================================
    //モンスターの属性値を取得する
    //================================
    public int GetMonsterType()
    {
        return type;
    }

    //======================================
    //一定時間捕まえられなかったら消える
    //======================================
    protected void SurvivalTimer()
    {
        //Debug.Log("timer" + delTimer);
        delTimer -= Time.deltaTime;     //カウントを減らしていく

        //delTimerが0になったら削除開始
        if (delTimer <= 0)
        {
            DestroyMonster();       //削除
        }
    }

    //======================================
    //削除命令関数
    //======================================
    protected void DestroyMonster()
    {
        Debug.Log("destroy");
        DelReport();
        Destroy(this.gameObject);     //削除
    }

    //======================================
    //消えたときGameManagerに報告
    //======================================
    protected void DelReport()
    {
        if (MonobitEngine.MonobitNetwork.isHost)
        {
            GManager.CountdownMonster();
        }
    }

    //======================================
    //持たれているかどうかのフラグセッター
    //プレイヤーIDの設定状態で取得できる
    //======================================
    public void SetCatchFlg(bool _flg)
    {
        catchFlg = _flg;
        SendCatchFlg();
    }

    //=======================================
    //所有しているプレイヤーのIDを設定
    //=======================================
    public void SetPlayerID(int _ID)
    {
        playerID = _ID;
    }

    //========================================
    //キャッチフラグを渡す関数
    //========================================
    public bool GetCatchFlg()
    {
        return catchFlg;
    }

    //==============================================
    //キャッチフラグの変更があったとき受信関数
    //==============================================
    [MunRPC]
    public void RPCSetCatchFlg(bool _catchFlg)
    {
        Debug.Log("キャッチフラグ");
        catchFlg = _catchFlg;
    }

    //==============================================
    //キャッチフラグの変更があったとき送信関数
    //==============================================
    private void SendCatchFlg()
    {
        monobitView.RPC("RPCSetCatchFlg", MonobitEngine.MonobitTargets.All, catchFlg);
    }

    //==============================================
    //親子関係をネットワーク越しに解消受信
    //==============================================
    [MunRPC]
    public void RPCParentElimination()
    {

        Debug.Log("RPC parent null");
        this.transform.parent = null;
    }

    //==============================================
    //親子関係をネットワーク越しに解消送信
    //==============================================
    private void SendParentElimination()
    {
        Debug.Log("send parent null");
        monobitView.RPC("RPCParentElimination", MonobitEngine.MonobitTargets.All);
    }
}
