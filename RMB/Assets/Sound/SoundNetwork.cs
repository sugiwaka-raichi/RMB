using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

//*********************************************************
//接続しているクライアント共通で音を鳴らすためのクラス
//*********************************************************
public class SoundNetwork : MonobitEngine.MonoBehaviour
{
    //#########################################################
    //受信関数群
    //#########################################################

    //=========================================================
    //ネットワーク越しにMUSIC再生処理受信
    //=========================================================
    [MunRPC]
    public void RPCPlayMusic(string _musicName)
    {
        Debug.Log("受信処理:music");
        SoundManager.PlayMusic(_musicName);
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

    //=========================================================
    //音楽停止
    //=========================================================
    [MunRPC]
    public void RPCStopMusic(string _musicName)
    {
        Debug.Log("受信処理:music");
        SoundManager.StopMusic(_musicName);
    }

    //=========================================================
    //効果音停止
    //=========================================================
    [MunRPC]
    public void RPCStopSE(string _seName)
    {
        Debug.Log("受信処理:se");
        SoundManager.StopSE(_seName);
    }

    //=========================================================
    //音楽一時停止
    //=========================================================
    [MunRPC]
    public void RPCPauseMusic(string _musicName)
    {
        Debug.Log("受信処理:music");
        SoundManager.PauseMusic(_musicName);
    }

    //=========================================================
    //効果音一時停止
    //=========================================================
    //[MunRPC]
    //public void RPCPauseSE(string _seName)
    //{
    //    Debug.Log("受信処理:se");
    //    SoundManager.PauseSE(_seName);
    //}

    //##########################################################
    //送信関数群
    //##########################################################

    //==========================================================
    //MUSIC再生命令を出す
    //==========================================================
    public void SendPlayMusic(string _musicName)
    {
        Debug.Log("送信処理:music");

        monobitView.RPC("RPCPlayMusic", MonobitTargets.All, _musicName);
    }

    //==========================================================
    //SE再生命令を出す
    //==========================================================
    public void SendPlaySE(string _seName)
    {
        Debug.Log("送信処理:se");

        monobitView.RPC("RPCPlaySE", MonobitTargets.All, _seName);
    }
    //==========================================================
    //MUSIC再生命令を出す
    //==========================================================
    public void SendStopMusic(string _musicName)
    {
        Debug.Log("送信処理:music");

        monobitView.RPC("RPCStopMusic", MonobitTargets.All, _musicName);
    }

    //==========================================================
    //SE再生命令を出す
    //==========================================================
    public void SendStopSE(string _seName)
    {
        Debug.Log("送信処理:se");

        monobitView.RPC("RPCStopSE", MonobitTargets.All, _seName);
    }
    //==========================================================
    //MUSIC再生命令を出す
    //==========================================================
    public void SendPauseMusic(string _musicName)
    {
        Debug.Log("送信処理:music");

        monobitView.RPC("RPCPauseMusic", MonobitTargets.All, _musicName);
    }

    //==========================================================
    //SE再生命令を出す
    //==========================================================
    public void SendPauseSE(string _seName)
    {
        Debug.Log("送信処理:se");

        monobitView.RPC("RPCPauseSE", MonobitTargets.All, _seName);
    }
}
