using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyManager : MonoBehaviour
{
    public enum KEY_MANAGER_MES
    {
        KMM_ADD = 0,
        KMM_SET,
        KMM_DELETE,
        KMM_ERROR
    }

    private static Dictionary<string, KeyCode> keyConfig = new Dictionary<string, KeyCode>();        //key一覧
    private readonly string configFilePath;                     //コンフィグファイルのファイルパス
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //========================================
    //入力状態を取得する関数群
    //========================================
    public static bool GetKey(string _key)
    {
        if (CheckKeyType(_key))
        {
            return Input.GetKey((keyConfig[_key]));
        }
        return false;
    }
    public static bool GetKeyDown(string _key)
    {
        if (CheckKeyType(_key))
        {
            return Input.GetKeyDown((keyConfig[_key]));
        }
        return false;
    }
    public static bool GetKeyUp(string _key)
    {
        if (CheckKeyType(_key))
        {
            return Input.GetKeyUp(keyConfig[_key]);
        }
        return false;
    }

    //=========================================
    //使用するkeyを追加で記録する関数
    //ArgumentExceptionが発生した場合
    //すでにkeyが登録されている
    //=========================================
    public static void AddKey(string _keyName)
    {
        keyConfig.Add(_keyName, 0);     //keyだけを追加
    }

    public static void AddKey(string _keyName,KeyCode _key)
    {
        keyConfig.Add(_keyName, _key);      //keyとvalue両方設定
    }

    //=======================================================
    //使用するkeyが登録されていなければ追加
    //されていれば設定する関数
    //Keyが重複してしまうので非推奨
    //=======================================================
    public static int AddSet(string _keyName,KeyCode _key)
    {
        //使用するキーが登録されているか調べる
        if (CheckKeyType(_keyName))
        {
            //登録されていれば
            SetKey(_keyName, _key);
            return (int)KEY_MANAGER_MES.KMM_SET;
        }
        else
        {
            //登録されていなければ
            AddKey(_keyName, _key);
            return (int)KEY_MANAGER_MES.KMM_ADD;
        }
    }

    //=======================================================
    //使用するkeyを決められたkeyに設定する関数
    //KeyNotFoundExceptionがでたらkeyが生成されていない
    //=======================================================
    public static void SetKey(string _keyName,KeyCode _key)
    {
        keyConfig[_keyName] = _key;
    }

    //========================================================
    //入力されたkeyを取得する関数
    //========================================================
    public static KeyCode KeyInput()
    {
        //そもそもkeyが押されているか確認
        if (Input.anyKeyDown)
        {
            //押されていればkeyの状態配列から一つずつcodeに取り出す
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                //押されたkeyと一致すれば押されたkey
                if (Input.GetKeyDown(code))
                {
                    return code;        //入力されたkeyを返す
                }
            }
        }
        return KeyCode.None;            //見つからなかった
    }

    //====================================================
    //keyが登録されているかどうかを調べる
    //====================================================
    public static bool CheckKeyType(string _keyName)
    {
        if (keyConfig.ContainsKey(_keyName))
        {
            //使われていればtrue
            return true;
        }
        else
        {
            //使われていなければfalse
            return false;
        }
    }

    //=====================================================
    //設定したkeyを保存する処理
    //=====================================================
    private void SaveKey()
    {
        
    }

    //=====================================================
    //設定したkeyを読込む処理
    //=====================================================
    private void LoadKey()
    {

    }

}
