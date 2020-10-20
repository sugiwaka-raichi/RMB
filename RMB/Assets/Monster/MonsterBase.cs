using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonobitEngine.MonoBehaviour
{
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
    protected float delTimer = 100.0f;      //消去されるまでの時間
    protected int playerID = -1;            //保持してるプレイヤーのID(未保持:-1)

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        //持たれていなければ
        //if (!catchFlg)
        //{
        //    SurvivalTimer();        //生存時間
        //}

        //プレイヤーのIDが設定されていなければ
        if(playerID == -1)
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
        delTimer = 3;           //三秒後に削除
        catchFlg = false;       //もたれてるフラグをへし折る
    }

    //================================
    //防御の仮想関数
    //================================
    virtual public void Deffence()
    {
        Debug.Log("Diffence");
        Destroy(this.gameObject);       //防御したら消える
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
        Destroy(this.gameObject);
    }

    //======================================
    //消えたときGameManagerに報告
    //======================================
    protected void DelReport()
    {
        //ToDo:GameManagerに報告するための処理を書く
    }

    //======================================
    //持たれているかどうかのフラグセッター
    //プレイヤーIDの設定状態で取得できる
    //======================================
    public void SetCatchFlg(bool _flg)
    {
        catchFlg = _flg;        
    }

    //=======================================
    //所有しているプレイヤーのIDを設定
    //=======================================
    public void SetPlayerID(int _ID)
    {
        playerID = _ID;
    }


}
