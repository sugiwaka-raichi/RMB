using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InitConfig : MonoBehaviour
{
    //ミキサーをインスペクターからうけとる
    [SerializeField] AudioMixer mixer;
    //グループの情報をインスペクターからうけとる
    [SerializeField] AudioMixerGroup groupMusic;
    [SerializeField] AudioMixerGroup groupSE;
    // Start is called before the first frame update
    void Start()
    {

        InitKey();
        //サウンドマネージャーへグループの設定処理を行う
        SoundManager.SetGroup(groupMusic, groupSE);
        SoundConfig.SetMixer(mixer);
        InitSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //===================================
    //キーの初期設定
    //===================================
    private void InitKey()
    {
        //-------------------------------
        //使用するキーを設定
        //-------------------------------
        KeyManager.AddKey("Shot", KeyCode.Mouse0);
        KeyManager.AddKey("Up", KeyCode.W);
        KeyManager.AddKey("Down", KeyCode.S);
        KeyManager.AddKey("Left", KeyCode.A);
        KeyManager.AddKey("Right", KeyCode.D);

        KeyManager.LoadKey();       //キー情報ロード
    }

    //====================================
    //音量設定
    //====================================
    private void InitSound()
    {
        SoundConfig.SoundConfLoad();        //サウンドの読み込み
    }


}
