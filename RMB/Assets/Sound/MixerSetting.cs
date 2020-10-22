using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerSetting : MonoBehaviour
{
    //ミキサーをインスペクターからうけとる
    [SerializeField] AudioMixer mixer;
    //グループの情報をインスペクターからうけとる
    [SerializeField] AudioMixerGroup groupMusic;
    [SerializeField] AudioMixerGroup groupSE;

    // Start is called before the first frame update
    void Start()
    {
        //サウンドマネージャーへグループの設定処理を行う
        SoundManager.SetGroup(groupMusic, groupSE);
        SoundConfig.SetMixer(mixer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
