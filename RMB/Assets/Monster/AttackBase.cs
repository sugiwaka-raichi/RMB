using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class AttackBase : MonobitEngine.MonoBehaviour
{
    public enum ATK_TYPE
    {
        AT_FIRE,            //火属性
        AT_WARTER,          //水属性
        AT_WOOD,            //木属性
        AT_NONE             //無属性
    }

    [SerializeField]
    protected float timer;

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
    public void SetPlayerID(int _ID)
    {
        playerID = _ID;
        SendPlayerID();     //設定したら送信する
    }

    //=========================================
    //設定されたIDを受信する関数
    //=========================================
    [MunRPC]
    private void RPCSetPlayerID(int _ID)
    {
        playerID = _ID;
    }

    //=========================================
    //発射したプレイヤーの情報を取得
    //=========================================
    public int GetShotPlayer()
    {
        return playerID;
    }

    //==========================================
    //攻撃種別を取得
    //==========================================
    public ATK_TYPE GetATKType()
    {
        return atkType;
    }

    //================================================
    //設定されたIDを送信
    //================================================
    private void SendPlayerID()
    {
        monobitView.RPC("RPCSetPlayerID", MonobitTargets.All, playerID);
    }
}
