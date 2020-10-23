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

    static int default_db = 0;             //デフォルトの音量

    [SerializeField]
    Slider slider;              //紐づけるスライダー
    [SerializeField]
    VOL_TYPE type;              //設定するボリュームの種類

    static AudioMixer mixer;           //ミキサー

    static float[] vol = new float[(int)VOL_TYPE.VT_NUM];     //音量一覧

    // Start is called before the first frame update
    void Start()
    {
        slider.value = vol[(int)type];
    }

    // Update is called once per frame
    void Update()
    {

    }

    //==================================================
    //設定関数(スライダーで扱う)
    //==================================================
    public void SetVol()
    {
        Debug.Log("音量変更:");
        
        mixer.SetFloat(type.ToString(), slider.value);        //ミキサーに設定
    }

    //===================================================
    //デフォルト設定に戻す
    //===================================================
    public static void SetDefault()
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
    public static void SoundConfSave()
    {
        VOL_TYPE volType = 0;

        //各グループの値記録
        for (int i = 0; i < (int)VOL_TYPE.VT_NUM; i++)
        {
            mixer.GetFloat(volType.ToString(),out vol[i]);
            PlayerPrefs.SetFloat(volType.ToString(), vol[i]);       //記録
            volType++;
            Debug.Log(vol[i]);
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
            vol[i] = PlayerPrefs.GetFloat(volType.ToString());           //読み込み
            Debug.Log("Load" + vol[i]);
            mixer.SetFloat(volType.ToString(), vol[i]);                 //反映
            volType++;
        }
    }
    
    //=====================================================
    //ミキサーを設定
    //=====================================================
    public static void SetMixer(AudioMixer _mixer)
    {
        Debug.Log("Setmixer");
        mixer = _mixer;
    }

    //====================================================
    //デフォルト設定の音量を取得
    //====================================================
    public static int GetDefaultdb()
    {
        return default_db;
    }
}
