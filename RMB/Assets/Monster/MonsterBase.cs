using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    //===============================
    //モンスターの属性値
    //===============================
    public enum MONSTER_TYPE
    {
        MT_WARTER = 0,
        MT_FIRE,
        MT_PLANT
    }

    protected int type;                     //属性
    protected bool catchFlg = false;        //プレイヤーに持たれているかどうか
    protected float delTimer = 60.0f; //消去されるまでの時間

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //================================
    //攻撃の仮想関数
    //================================
    virtual public void Attack()
    {
        Debug.Log("attack");
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
        DelMonster();
        Destroy(this.gameObject);
    }

    //======================================
    //消えたときGameManagerに報告
    //======================================
    protected void DelMonster()
    {
        //ToDo:GameManagerに報告するための処理を書く
    }
}
