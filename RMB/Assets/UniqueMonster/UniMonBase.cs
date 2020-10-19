using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniMonBase : MonoBehaviour
{
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
        DestroyUnimon();
    }

    //================================
    //防御の仮想関数
    //================================
    virtual public void Deffence()
    {
        Debug.Log("Diffence");
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
            DestroyUnimon();
        }
    }

    //======================================
    //削除命令関数
    //======================================
    protected void DestroyUnimon()
    {
        Debug.Log("destroy：Unimon");
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
