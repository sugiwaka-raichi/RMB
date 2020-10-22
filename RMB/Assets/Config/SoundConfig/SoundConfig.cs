using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundConfig : MonoBehaviour
{
    //設定できる音量の種別
    public enum VOL_TYPE
    {
        Master,          //ミキサーに登録されるすべての音
        Music,           //音楽
        SE,              //SE
        VT_NUM              //種類の数
    }

    [SerializeField]
    int default_db;             //デフォルトの音量

    static AudioMixer mixer;           //ミキサー

    static float[] vol = new float[(int)VOL_TYPE.VT_NUM];     //音量一覧

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //==================================================
    //設定関数(スライダーで扱う)
    //==================================================
    public void SetVol(Slider _slider)
    {
        Debug.Log("音量変更:" + _slider.name);
        string _volType = _slider.name;
        
        mixer.SetFloat(_volType.ToString(), _slider.value);        //ミキサーに設定
    }

    //===================================================
    //デフォルト設定に戻す(ボタンで扱う)
    //===================================================
    public void SetDefault()
    {
        VOL_TYPE volType = 0;

        //各グループの音量を設定
        for (int i = 0; i < (int)VOL_TYPE.VT_NUM; i++)
        {
            vol[i] = default_db;         //dbを設定
            mixer.SetFloat(volType.ToString(), default_db);     //ミキサーに設定

            volType++;      //設定か所を進める
        }

    }

    //===================================================
    //記録処理(コンフィグ画面を閉じる際に呼び出す)
    //===================================================
    public void SoundConfSave()
    {
        VOL_TYPE volType = 0;

        //各グループの値記録
        for (int i = 0; i < (int)VOL_TYPE.VT_NUM; i++)
        {
            float tmp = 0;
            mixer.GetFloat(volType.ToString(),out tmp);
            PlayerPrefs.SetFloat(volType.ToString(), tmp);       //記録
        }
    }

    //====================================================
    //値読込
    //====================================================
    public static void SoundConfLoad()
    {
        VOL_TYPE volType = 0;

        //各グループの値読み込み
        for (int i = 0; i < (int)VOL_TYPE.VT_NUM; i++)
        {
            PlayerPrefs.GetFloat(volType.ToString(), vol[i]);           //読み込み
            mixer.SetFloat(volType.ToString(), vol[i]);                 //反映
        }
    }
    
    //=====================================================
    //ミキサーを設定
    //=====================================================
    public static void SetMixer(AudioMixer _mixer)
    {
        mixer = _mixer;
    }
}
