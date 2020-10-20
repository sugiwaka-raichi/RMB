﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonobitEngine.MonoBehaviour
{
    public enum ATK_TYPE
    {
        AT_FIRE,            //火属性
        AT_WARTER,          //水属性
        AT_WOOD,            //木属性
        AT_NONE             //無属性
    }

    protected ATK_TYPE atkType;       //攻撃種別
    protected int playerID;           //発射したプレイヤーのID

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //=========================================
    //発射したプレイヤーを設定
    //=========================================
    public virtual void SetPlayerID(int _ID)
    {
        playerID = _ID;
    }

    //=========================================
    //発射したプレイヤーの情報を取得
    //=========================================
    public virtual int GetShotPlayer()
    {
        return playerID;
    }

    //==========================================
    //攻撃種別を取得
    //==========================================
    public virtual ATK_TYPE GetType()
    {
        return atkType;
    }
}
