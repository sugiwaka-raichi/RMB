using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConfigMenu : MonoBehaviour
{
    [SerializeField]
    Slider[] volumes;           //音量設定のスライダーを設定

    //======================================
    // メニューを閉じる
    //======================================
    public void CloseMenu()
    {
        KeyManager.SaveKey();       //キーを保存する
        SoundConfig.SoundConfSave();        //音量設定を記録
        SoundManager.PlaySE(SoundData.SE_LIST.SystemDecision.ToString());       //決定音声を流す
        this.gameObject.SetActive(false);       //最後に呼ぶ
    }

    //=====================================
    //デフォルト設定にする
    //=====================================
    public void DefaultSetting()
    {
        SoundConfig.SetDefault();           //音量をデフォルト設定に
        int db = SoundConfig.GetDefaultdb();        //デフォルトの音量を取得
        for(int i = 0;i < volumes.Length; i++)
        {
            volumes[i].value = db;      //スライダーの値を変える
        }
    }

}
