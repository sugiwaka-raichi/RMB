using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using MonobitEngine;

public class SoundManager : MonobitEngine.MonoBehaviour
{
    static Dictionary<string,AudioSource> musicDic = new Dictionary<string,AudioSource>();      //再生中のサウンドのリスト
    static Dictionary<string,AudioSource> seDic = new Dictionary<string,AudioSource>();      //再生中のサウンドのリスト
    static GameObject soundObject;                  //どこにオーディオソースをつけるかを保持する
    
    //グループ
    static AudioMixerGroup groupMusic;
    static AudioMixerGroup groupSE;

    //=============================================
    //外部クラスから設定してあげる
    //=============================================
    public static void SetGroup(AudioMixerGroup _groupMusic, AudioMixerGroup _groupSE)
    {
        groupMusic = _groupMusic;
        groupSE = _groupSE;
    }

    //==================================================
    // 音楽再生
    //==================================================
    public static bool PlayMusic(string _musicName)
    {
        //ゲームオブジェクトが指定されているかどうか
        CheckGameObject();

        //キー値が存在するかどうか
        if (musicDic.ContainsKey(_musicName))
        {
            //存在すれば中身があるか確認
            if (ValueNullCheck(musicDic[_musicName]))
            {
                musicDic[_musicName].Play();                             //音楽再生

                return true;        //再生して終了

            }
        }
        else
        {
            //存在しないので
            musicDic.Add(_musicName, null);     //空を作る
        }

        //オーディオソース作成
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = groupMusic;     //グループを設定してあげる
        audioSource.clip = LoadMusic(_musicName);       //音楽の読み込み
        if (audioSource.clip == null)
        {
            //再生失敗
            return false;
        }

        musicDic[_musicName] = audioSource;        //オーディオソースを記録
        musicDic[_musicName].Play();                             //音楽再生
        return true;        //再生して終了

    }

    //==================================================
    // 音楽停止
    //==================================================
    public static void StopMusic(string _musicName)
    {
        //再生中の停止
        if (musicDic.ContainsKey(_musicName))       //キー値が存在するかどうか
        {
            musicDic[_musicName].Stop();     //停止
        }
    }

    //==================================================
    //音楽の一時停止
    //==================================================
    public static void PauseMusic(string _musicName)
    {
        //一時停止
        if (musicDic.ContainsKey(_musicName))       //キー値が存在するかどうか
        {
            musicDic[_musicName].Pause();     //停止
        }
    }

    //==================================================
    //効果音再生
    //==================================================
    public static bool PlaySE(string _seName)
    {
        CheckGameObject();

        //キー値が存在するかどうか
        if (seDic.ContainsKey(_seName))
        {
            //存在すれば中身があるか確認
            if (ValueNullCheck(seDic[_seName]))
            {
                seDic[_seName].Play();   //音楽再生

                return true;        //再生して終了

            }
        }
        else
        {
            //存在しないので
            seDic.Add(_seName, null);     //空を作る
        }

        //オーディオソース作成
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = groupSE;     //グループを設定してあげる
        audioSource.clip = LoadSE(_seName);       //音楽の読み込み
        if (audioSource.clip == null)
        {
            //再生失敗
            return false;
        }

        seDic[_seName] = audioSource;        //オーディオソースを記録
        seDic[_seName].Play();                             //音楽再生
        return true;        //再生して終了
    }

    //==================================================
    //効果音停止
    //==================================================
    public static void StopSE(string _seName)
    {
        //再生中の停止
        if (seDic.ContainsKey(_seName))       //キー値が存在するかどうか
        {
            seDic[_seName].Stop();     //停止
        }
    }

    //==================================================
    //効果音一時停止
    //==================================================
    public static void PauseSE(string _seName)
    {
        //再生中の停止
        if (seDic.ContainsKey(_seName))       //キー値が存在するかどうか
        {
            seDic[_seName].Pause();
        }
    }

    //===================================================
    //音楽の読み込み処理
    //===================================================
    private static AudioClip LoadMusic(string _musicName)
    {
        //オーディオの読み込み
        AudioClip audioClip = Resources.Load<AudioClip>("Sound/Music/" + _musicName);
        Debug.Log("Sound/Music/" + _musicName + ".ogg:" + audioClip);

        return audioClip;
    }

    //====================================================
    //効果音の読み込み処理
    //====================================================
    private static AudioClip LoadSE(string _seName)
    {
        //オーディオの読み込み
        AudioClip audioClip = Resources.Load<AudioClip>("Sound/SE/" + _seName);
        Debug.Log("Sound/SE/" + _seName + ".ogg:" + audioClip);

        return audioClip;
    }

    //=========================================================
    //ゲームオブジェクト登録有無
    //=========================================================
    private static void CheckGameObject()
    {
        //オーディオソースをアタッチするオブジェクトが登録されたか
        if (soundObject == null)
        {
            //登録されていない
            soundObject = new GameObject();     //空のオブジェクトを作る
            soundObject.name = "new Audio Source";      //新しいオーディオソース
        }
    }

    //=========================================================
    //オーディオソースを適応させるオブジェクトの設定
    //=========================================================
    public static void SetGameObj(GameObject _gameObject)
    {
        soundObject = _gameObject;
    }

    //=========================================================
    //valueの中身がnullでないか確認する
    //=========================================================
    private static bool ValueNullCheck(AudioSource _audioSource)
    {
        if (_audioSource == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //==========================================================
    //getオーディオソース
    //==========================================================
    public static AudioSource GetSESource(string _se)
    {
        if (seDic.ContainsKey(_se))
        {
            if(seDic[_se] == null)
            {
                CheckGameObject();
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();      //新規でオーディオソースを作成
                audioSource.outputAudioMixerGroup = groupSE;      //グループ設定
                audioSource.clip = LoadSE(_se);                   //SE読み込み
                seDic[_se] = audioSource;
            }
            return seDic[_se];        //一致するソースを渡す
        }
        else
        {
            CheckGameObject();
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();      //新規でオーディオソースを作成
            audioSource.outputAudioMixerGroup = groupSE;      //グループ設定
            audioSource.clip = LoadSE(_se);                   //SE読み込み

            seDic.Add(_se, audioSource);                      //新規登録

            return audioSource;        //新規作成したものを渡す
        }
    }

}
