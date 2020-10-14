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
        MT_PLANT            //木属性のモンスター
    }

    protected int type;                     //属性
    protected bool catchFlg = false;        //プレイヤーに持たれているかどうか
    protected float delTimer = 100.0f;      //消去されるまでの時間

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        //持たれていなければ
        if (!catchFlg)
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
    }

    //================================
    //防御の仮想関数
    //================================
    virtual public void Deffence()
    {
        Debug.Log("Diffence");
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
            DestroyMonster();
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
    //======================================
    public void SetCatchFlg(bool _flg)
    {
        catchFlg = _flg;
    }
}
