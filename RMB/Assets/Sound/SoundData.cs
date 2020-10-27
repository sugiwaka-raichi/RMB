using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundData : MonoBehaviour
{
    //================================
    //MUSICのLIST
    //================================
    public enum MUSIC_LIST
    {
        Title,      //タイトルシーン
        Lobby,      //ロビーシーン
        Stage       //ステージシーン
    }

    //================================
    //SEのLIST
    //================================
    public enum SE_LIST
    {
        Down,           //ダウン時
        Equipped,       //装備時
        FireMonster,    //火属性のモンスター
        FootSteps,      //足音
        GameStart,      //ゲームスタート
        Hit,            //ヒット
        KeyConfig,      //キーコンフィグ
        Shot,           //攻撃
        SystemCancel,   //システムキャンセル
        SystemDecision, //システム決定
        SystemSelect,   //システム選択
        WanwanBark,     //ワンワン吠え
        WarterMonster,  //水属性モンスター
        Winner,         //勝利時
        WoodMonster     //木属性モンスター
    }
}
