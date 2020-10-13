using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    //===============================
    //モンスターの属性
    //===============================
    public enum MONSTER_TYPE
    {
        MT_WARTER = 0,
        MT_FIRE,
        MT_PLANT
    }

    private int type;

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
    

}
