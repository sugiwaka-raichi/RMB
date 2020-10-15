using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

//*********************************************************
//接続しているクライアント共通で音を鳴らすためのクラス
//*********************************************************
public class SoundNetwork : MonobitEngine.MonoBehaviour
{
    //=========================================================
    //ネットワーク越しにMUSIC再生処理受信
    //=========================================================
    [MunRPC]
    public static void RPCPlayMusic(string _musicName)
    {
        Debug.Log("受信処理:music");
        SoundManager.PlayMusic(_musicName);
    }
    
    //==========================================================
    //MUSIC再生命令を出す
    //==========================================================
    public void SendPlayMusic(string _musicName)
    {
        Debug.Log("送信処理:music");

        monobitView.RPC("RPCPlayMusic", MonobitTargets.All, _musicName);
    }

    //=========================================================
    //ネットワーク越しにSE再生処理受信
    //=========================================================
    [MunRPC]
    public void RPCPlaySE(string _seName)
    {
        Debug.Log("受信処理:se");
        SoundManager.PlaySE(_seName);
    }

    //==========================================================
    //SE再生命令を出す
    //==========================================================
    public void SendPlaySE(string _seName)
    {
        Debug.Log("送信処理:se");

        monobitView.RPC("RPCPlaySE", MonobitTargets.All, _seName);
    }
}
