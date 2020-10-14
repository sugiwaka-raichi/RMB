using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static Dictionary<string,AudioSource> audioDic = new Dictionary<string,AudioSource>();      //再生中のサウンドのリスト
    static GameObject soundObject;                                                              //どこにオーディオソースをつけるかを保持する

    //==================================================
    // 音楽再生
    //==================================================
    public static bool PlayeMusic(string _musicName)
    {
        CheckGameObject();

        if (!audioDic.ContainsKey(_musicName))
        {

            //オーディオソース作成
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = LoadMusic(_musicName);       //音楽の読み込み
            if (audioSource.clip == null)
            {
                //再生失敗
                return false;
            }
            audioDic.Add("_musicName", audioSource);        //オーディオソースを記録
        }
        audioDic[_musicName].Play();                             //音楽再生

        return true;
    }

    //==================================================
    // 音楽停止
    //==================================================
    public static void StopMusic(string _musicName)
    {
        //再生中の停止
        audioDic[_musicName].Stop();     //停止
        audioDic.Remove(_musicName);     //再生中のオーディオソースをなくす
    }

    //==================================================
    //効果音再生
    //==================================================
    public static bool PlaySE(string _seName)
    {
        CheckGameObject();

        //再生する音がロードされているかどうか
        if (!audioDic.ContainsKey(_seName))
        {
            //されていなければオーディオソース作成
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = LoadSE(_seName);       //音楽の読み込み
            if (audioSource.clip == null)
            {
                //再生失敗
                return false;
            }
            audioDic.Add(_seName, audioSource);        //オーディオソースを記録
        }
        audioDic[_seName].Play();                             //音楽再生

        return true;
    }

    //==================================================
    //効果音停止
    //==================================================
    public static void StopSE(string _seName)
    {
        
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
    //オーディオソースをアタッチするオブジェクトが登録されたか
    //=========================================================
    private static void CheckGameObject()
    {
        if (soundObject == null)
        {
            //登録されていない
            soundObject = new GameObject();     //空のオブジェクトを作る
            soundObject.name = "new Audio Source";      //新しいオーディオソース
        }
        else
        {
            //あるのでどうでもいい
        }
    }

    //=========================================================
    //
    //=========================================================

    //=========================================================
    //オーディオソースを適応させるオブジェクトの設定
    //=========================================================
    public static void SetGameObj(GameObject gameObject)
    {
        soundObject = gameObject;
    }
}
